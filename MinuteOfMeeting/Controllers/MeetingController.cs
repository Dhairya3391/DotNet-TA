using Microsoft.AspNetCore.Mvc;
using MinuteOfMeeting.DAL;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MinuteOfMeeting.Helpers;
using MinuteOfMeeting.Models;
using MinuteOfMeeting.Models.ViewModels;
using System.Data;

namespace MinuteOfMeeting.Controllers
{
    [SessionAuthorize]
    public class MeetingController : Controller
    {
        // GET: Meeting
        public IActionResult Index(int? meetingTypeId, int? departmentId, int? venueId,
            DateTime? startDate, DateTime? endDate, string searchText)
        {
            try
            {
                DataTable dt = MeetingDAL.SelectWithFilters(
                    startDate: startDate,
                    endDate: endDate,
                    meetingTypeID: meetingTypeId,
                    meetingVenueID: venueId,
                    departmentID: departmentId,
                    searchKeyword: searchText);

                // Populate dropdowns
                ViewBag.MeetingTypes = GetMeetingTypesDropdown();
                ViewBag.Departments = GetDepartmentsDropdown();
                ViewBag.Venues = GetVenuesDropdown();

                // Store filter values for form
                ViewBag.MeetingTypeId = meetingTypeId;
                ViewBag.DepartmentId = departmentId;
                ViewBag.VenueId = venueId;
                ViewBag.StartDate = startDate?.ToString("yyyy-MM-dd");
                ViewBag.EndDate = endDate?.ToString("yyyy-MM-dd");
                ViewBag.SearchText = searchText;

                return View(dt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading meetings: " + ex.Message;
                return View(new DataTable());
            }
        }

        // GET: Meeting/Create
        public IActionResult Create(int? meetingTypeId, int? departmentId, int? venueId)
        {
            Meeting model = new Meeting
            {
                MeetingDate = DateTime.Now.AddDays(1), // Default to tomorrow
                IsCancelled = false
            };

            // Pre-fill values if provided
            if (meetingTypeId.HasValue)
                model.MeetingTypeID = meetingTypeId.Value;
            if (departmentId.HasValue)
                model.DepartmentID = departmentId.Value;
            if (venueId.HasValue)
                model.MeetingVenueID = venueId.Value;

            PopulateDropdowns();
            return View(model);
        }

        // POST: Meeting/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Meeting model, IFormFile documentFile)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(model);
            }

            try
            {
                // Check for venue conflicts
                var conflict = MeetingDAL.CheckConflict(model.MeetingVenueID, model.MeetingDate, null);
                if (conflict.HasConflict)
                {
                    string message = "This venue is already booked for the selected date and time.";
                    if (conflict.ConflictMeetingID > 0)
                    {
                        message += $" (Conflict with meeting #{conflict.ConflictMeetingID}: {conflict.ConflictDescription})";
                    }
                    ModelState.AddModelError(string.Empty, message);
                    PopulateDropdowns();
                    return View(model);
                }

                // Handle file upload
                if (documentFile != null)
                {
                    model.DocumentPath = await FileUploadHelper.UploadFile(documentFile, "meeting-docs");
                }

                int newId = MeetingDAL.Insert(model);
                if (newId > 0)
                {
                    TempData["Success"] = "Meeting scheduled successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Failed to schedule meeting.";
                    PopulateDropdowns();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error scheduling meeting: " + ex.Message);
                PopulateDropdowns();
                return View(model);
            }
        }

        // GET: Meeting/Edit
        public IActionResult Edit(int id)
        {
            try
            {
                DataTable dt = MeetingDAL.SelectByPK(id);
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    Meeting model = new Meeting
                    {
                        MeetingID = Convert.ToInt32(row["MeetingID"]),
                        MeetingDate = Convert.ToDateTime(row["MeetingDate"]),
                        MeetingVenueID = Convert.ToInt32(row["MeetingVenueID"]),
                        MeetingTypeID = Convert.ToInt32(row["MeetingTypeID"]),
                        DepartmentID = Convert.ToInt32(row["DepartmentID"]),
                        MeetingDescription = row["MeetingDescription"].ToString(),
                        DocumentPath = row["DocumentPath"].ToString(),
                        IsCancelled = Convert.ToBoolean(row["IsCancelled"]),
                        CancellationDateTime = row["CancellationDateTime"] as DateTime?,
                        CancellationReason = row["CancellationReason"].ToString()
                    };

                    PopulateDropdowns();
                    return View(model);
                }
                else
                {
                    TempData["Error"] = "Meeting not found.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading meeting: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Meeting/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Meeting model, IFormFile documentFile, bool removeDocument)
        {
            if (!ModelState.IsValid)
            {
                PopulateDropdowns();
                return View(model);
            }

            try
            {
                // Check for venue conflicts (excluding current meeting)
                var conflict = MeetingDAL.CheckConflict(model.MeetingVenueID, model.MeetingDate, model.MeetingID);
                if (conflict.HasConflict)
                {
                    string message = "This venue is already booked for the selected date and time.";
                    if (conflict.ConflictMeetingID > 0)
                    {
                        message += $" (Conflict with meeting #{conflict.ConflictMeetingID}: {conflict.ConflictDescription})";
                    }
                    ModelState.AddModelError(string.Empty, message);
                    PopulateDropdowns();
                    return View(model);
                }

                // Handle document removal
                if (removeDocument && !string.IsNullOrEmpty(model.DocumentPath))
                {
                    FileUploadHelper.DeleteFile(model.DocumentPath);
                    model.DocumentPath = null;
                }

                // Handle new file upload
                if (documentFile != null)
                {
                    // Delete old document if exists
                    if (!string.IsNullOrEmpty(model.DocumentPath))
                    {
                        FileUploadHelper.DeleteFile(model.DocumentPath);
                    }

                    model.DocumentPath = await FileUploadHelper.UploadFile(documentFile, "meeting-docs");
                }

                bool success = MeetingDAL.Update(model) > 0;
                if (success)
                {
                    TempData["Success"] = "Meeting updated successfully!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Error"] = "Failed to update meeting.";
                    PopulateDropdowns();
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Error updating meeting: " + ex.Message);
                PopulateDropdowns();
                return View(model);
            }
        }

        // GET: Meeting/Details
        public IActionResult Details(int id)
        {
            try
            {
                DataTable dt = MeetingDAL.SelectByPK(id);
                if (dt.Rows.Count > 0)
                {
                    // Get meeting members
                    DataTable membersDt = MeetingMemberDAL.SelectByMeeting(id);
                    ViewBag.Members = membersDt;
                    return View(dt);
                }
                else
                {
                    TempData["Error"] = "Meeting not found.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading meeting details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Meeting/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            try
            {
                // Get meeting details to delete associated documents
                DataTable meetingDt = MeetingDAL.SelectByPK(id);
                if (meetingDt.Rows.Count > 0)
                {
                    string documentPath = meetingDt.Rows[0]["DocumentPath"].ToString();

                    // Delete document file if exists
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        FileUploadHelper.DeleteFile(documentPath);
                    }
                }

                int rowsAffected = MeetingDAL.Delete(id);
                bool success = rowsAffected > 0;
                if (success)
                {
                    TempData["Success"] = "Meeting deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete meeting.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting meeting: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: Meeting/Cancel
        public IActionResult Cancel(int id)
        {
            try
            {
                DataTable dt = MeetingDAL.SelectByPK(id);
                if (dt.Rows.Count > 0)
                {
                    return View(dt);
                }
                else
                {
                    TempData["Error"] = "Meeting not found.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading meeting: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Meeting/Cancel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Cancel(int id, string cancellationReason)
        {
            try
            {
                bool success = MeetingDAL.Cancel(id, cancellationReason) > 0;
                if (success)
                {
                    TempData["Success"] = "Meeting cancelled successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to cancel meeting.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error cancelling meeting: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: Meeting/ManageAttendance
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

        // POST: Meeting/UpdateAttendance
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult UpdateAttendance(int meetingId, List<int> selectedStaff, List<string> attendanceStatus, List<string> remarks)
        {
            try
            {
                // Clear existing attendees
                MeetingMemberDAL.DeleteByMeeting(meetingId);

                // Add new attendees
                if (selectedStaff != null && selectedStaff.Count > 0)
                {
                    for (int i = 0; i < selectedStaff.Count; i++)
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

                        MeetingMemberDAL.Insert(attendance);
                    }
                }

                TempData["Success"] = "Attendance updated successfully!";
                return RedirectToAction("Details", new { id = meetingId });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error updating attendance: " + ex.Message;
                return RedirectToAction("ManageAttendance", new { id = meetingId });
            }
        }

        // GET: Meeting/Calendar
        public IActionResult Calendar(int year, int month)
        {
            try
            {
                if (year == 0) year = DateTime.Now.Year;
                if (month == 0) month = DateTime.Now.Month;

                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                DataTable meetingsDt = MeetingDAL.GetByDateRange(startDate, endDate);

                ViewBag.Year = year;
                ViewBag.Month = month;
                ViewBag.MonthName = startDate.ToString("MMMM yyyy");

                return View(meetingsDt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading calendar: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // AJAX: Check venue availability
        [HttpPost]
        public IActionResult CheckVenueAvailability(int venueId, DateTime meetingDate, int? excludeMeetingId)
        {
            try
            {
                var conflict = MeetingDAL.CheckConflict(venueId, meetingDate, excludeMeetingId);
                bool isAvailable = !conflict.HasConflict;
                return Json(new { available = isAvailable });
            }
            catch
            {
                return Json(new { available = false, error = true });
            }
        }

        // GET: Meeting/DownloadDocument
        public IActionResult DownloadDocument(int id)
        {
            try
            {
                DataTable dt = MeetingDAL.SelectByPK(id);
                if (dt.Rows.Count > 0)
                {
                    string documentPath = dt.Rows[0]["DocumentPath"].ToString();
                    if (!string.IsNullOrEmpty(documentPath))
                    {
                        string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", documentPath.TrimStart('/'));
                        if (System.IO.File.Exists(fullPath))
                        {
                            byte[] fileBytes = System.IO.File.ReadAllBytes(fullPath);
                            string fileName = Path.GetFileName(fullPath);
                            return File(fileBytes, "application/octet-stream", fileName);
                        }
                    }
                }

                TempData["Error"] = "Document not found.";
                return RedirectToAction("Details", new { id });
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error downloading document: " + ex.Message;
                return RedirectToAction("Details", new { id });
            }
        }

        private void PopulateDropdowns()
        {
            ViewBag.MeetingTypes = GetMeetingTypesDropdown();
            ViewBag.Departments = GetDepartmentsDropdown();
            ViewBag.Venues = GetVenuesDropdown();
        }

        private DataTable GetMeetingTypesDropdown()
        {
            try
            {
                return MeetingTypeDAL.SelectForDropdown();
            }
            catch
            {
                return new DataTable();
            }
        }

        private DataTable GetDepartmentsDropdown()
        {
            try
            {
                return DepartmentDAL.SelectForDropdown();
            }
            catch
            {
                return new DataTable();
            }
        }

        private DataTable GetVenuesDropdown()
        {
            try
            {
                return MeetingVenueDAL.SelectForDropdown();
            }
            catch
            {
                return new DataTable();
            }
        }
    }
}