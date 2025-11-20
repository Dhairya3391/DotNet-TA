using Microsoft.AspNetCore.Mvc;
using MinuteOfMeeting.DAL;
using MinuteOfMeeting.Helpers;
using MinuteOfMeeting.Models;
using MinuteOfMeeting.Models.ViewModels;
using System.Data;

namespace MinuteOfMeeting.Controllers
{
    [SessionAuthorize]
    public class MeetingMemberController : Controller
    {
        // GET: MeetingMember
        public IActionResult Index()
        {
            try
            {
                // Get upcoming meetings for attendance management
                DataTable meetingsDt = MeetingDAL.SelectUpcoming(20);
                return View(meetingsDt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading meetings: " + ex.Message;
                return View(new DataTable());
            }
        }

        // GET: MeetingMember/ManageAttendance/5
        public IActionResult ManageAttendance(int id)
        {
            try
            {
                DataTable meetingDt = MeetingDAL.SelectByPK(id);
                if (meetingDt.Rows.Count == 0)
                {
                    TempData["Error"] = "Meeting not found.";
                    return RedirectToAction("Index");
                }

                ViewBag.Meeting = meetingDt.Rows[0];

                // Get all staff for department
                int departmentId = Convert.ToInt32(meetingDt.Rows[0]["DepartmentID"]);
                DataTable staffDt = StaffDAL.SelectByDepartment(departmentId);

                // Get current attendees
                DataTable attendeesDt = MeetingMemberDAL.SelectByMeeting(id);

                // Create view model
                var attendanceList = new List<MeetingAttendeeViewModel>();

                foreach (DataRow staffRow in staffDt.Rows)
                {
                    var attendance = new MeetingAttendeeViewModel
                    {
                        StaffID = Convert.ToInt32(staffRow["StaffID"]),
                        StaffName = staffRow["StaffName"].ToString(),
                        EmailAddress = staffRow["EmailAddress"].ToString(),
                        IsInvited = false,
                        IsPresent = false,
                        Remarks = string.Empty
                    };

                    // Check if staff is already an attendee
                    foreach (DataRow attendeeRow in attendeesDt.Rows)
                    {
                        if (Convert.ToInt32(attendeeRow["StaffID"]) == attendance.StaffID)
                        {
                            attendance.IsInvited = true;
                            attendance.IsPresent = Convert.ToBoolean(attendeeRow["IsPresent"]);
                            attendance.Remarks = attendeeRow["Remarks"].ToString();
                            break;
                        }
                    }

                    attendanceList.Add(attendance);
                }

                return View(attendanceList);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading attendance: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: MeetingMember/UpdateAttendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAttendance(int meetingId, List<int> selectedStaff, List<string> attendanceStatus, List<string> remarks)
        {
            try
            {
                // Validate meeting exists
                DataTable meetingDt = MeetingDAL.SelectByPK(meetingId);
                if (meetingDt.Rows.Count == 0)
                {
                    TempData["Error"] = "Meeting not found.";
                    return RedirectToAction("Index");
                }

                // Clear existing attendees
                MeetingMemberDAL.DeleteByMeeting(meetingId);

                // Add new attendees
                if (selectedStaff != null && selectedStaff.Count > 0)
                {
                    int successCount = 0;
                    for (int i = 0; i < selectedStaff.Count; i++)
                    {
                        try
                        {
                            int staffId = selectedStaff[i];
                            bool isPresent = attendanceStatus != null && i < attendanceStatus.Count &&
                                            attendanceStatus[i] == "present";
                            string remark = (remarks != null && i < remarks.Count) ? remarks[i] : "";

                            MeetingMember attendance = new MeetingMember
                            {
                                MeetingID = meetingId,
                                StaffID = staffId,
                                IsPresent = isPresent,
                                Remarks = remark
                            };

                            int result = MeetingMemberDAL.Insert(attendance);
                            if (result > 0)
                            {
                                successCount++;
                            }
                        }
                        catch (Exception innerEx)
                        {
                            // Log individual staff member errors but continue processing
                            System.Diagnostics.Debug.WriteLine($"Error adding staff {selectedStaff[i]}: {innerEx.Message}");
                        }
                    }

                    if (successCount > 0)
                    {
                        TempData["Success"] = $"Attendance updated successfully for {successCount} staff member(s)!";
                    }
                    else
                    {
                        TempData["Warning"] = "No staff members were added to the meeting.";
                    }
                }
                else
                {
                    TempData["Info"] = "No staff members were selected for this meeting.";
                }

                return RedirectToAction("AttendanceDetails", new { id = meetingId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating attendance: " + ex.Message;
                return RedirectToAction("ManageAttendance", new { id = meetingId });
            }
        }

        // GET: MeetingMember/AttendanceDetails/5
        public IActionResult AttendanceDetails(int id)
        {
            try
            {
                DataTable meetingDt = MeetingDAL.SelectByPK(id);
                if (meetingDt.Rows.Count == 0)
                {
                    TempData["Error"] = "Meeting not found.";
                    return RedirectToAction("Index");
                }

                ViewBag.Meeting = meetingDt.Rows[0];

                // Get attendance summary
                DataTable attendanceDt = MeetingMemberDAL.SelectByMeeting(id);
                return View(attendanceDt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading attendance details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: MeetingMember/StaffAttendance/5
        public IActionResult StaffAttendance(int staffId)
        {
            try
            {
                DataTable staffDt = StaffDAL.SelectByPK(staffId);
                if (staffDt.Rows.Count == 0)
                {
                    TempData["Error"] = "Staff member not found.";
                    return RedirectToAction("Index");
                }

                ViewBag.Staff = staffDt.Rows[0];

                // Get all meetings for this staff member
                DataTable meetingsDt = MeetingMemberDAL.SelectByStaff(staffId);
                return View(meetingsDt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading staff attendance: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: MeetingMember/AttendanceReport
        public IActionResult AttendanceReport(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if (!startDate.HasValue)
                    startDate = DateTime.Now.AddMonths(-1); // Default to last month

                if (!endDate.HasValue)
                    endDate = DateTime.Now;

                // Get attendance summary for the date range
                DataTable reportDt = MeetingMemberDAL.GetAttendanceSummary(startDate.Value, endDate.Value);

                ViewBag.StartDate = startDate.Value.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate.Value.ToString("yyyy-MM-dd");

                return View(reportDt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error generating attendance report: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: MeetingMember/ExportAttendance
        public IActionResult ExportAttendance(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                if (!startDate.HasValue)
                    startDate = DateTime.Now.AddMonths(-1);

                if (!endDate.HasValue)
                    endDate = DateTime.Now;

                DataTable reportDt = MeetingMemberDAL.GetAttendanceSummary(startDate.Value, endDate.Value);

                byte[] fileBytes = ExportHelper.ExportAttendanceToExcel(reportDt);

                return File(fileBytes,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            $"Attendance_Report_{startDate.Value:yyyyMMdd}_to_{endDate.Value:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error exporting attendance report: " + ex.Message;
                return RedirectToAction("AttendanceReport");
            }
        }

        // AJAX: Get meeting statistics
        [HttpPost]
        public IActionResult GetMeetingStatistics()
        {
            try
            {
                var stats = new
                {
                    totalMeetings = MeetingDAL.GetCount(),
                    upcomingMeetings = MeetingDAL.GetCountByStatus("upcoming"),
                    completedMeetings = MeetingDAL.GetCountByStatus("completed"),
                    cancelledMeetings = MeetingDAL.GetCountByStatus("cancelled")
                };

                return Json(stats);
            }
            catch
            {
                return Json(new { error = true });
            }
        }
    }
}