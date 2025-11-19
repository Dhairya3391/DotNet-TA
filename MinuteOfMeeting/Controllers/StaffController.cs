using Microsoft.AspNetCore.Mvc;
using MinuteOfMeeting.Helpers;
using MinuteOfMeeting.Models;
using MinuteOfMeeting.DAL;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MinuteOfMeeting.Controllers
{
    public class StaffController : Controller
    {
        // GET: Staff
        [SessionAuthorize]
        public IActionResult Index(int? departmentId, string searchText)
        {
            try
            {
                DataTable dt;
                if (departmentId.HasValue)
                {
                    dt = StaffDAL.SelectByDepartment(departmentId.Value);
                    ViewBag.DepartmentId = departmentId.Value;
                }
                else
                {
                    dt = StaffDAL.SelectAll();
                }

                if (!string.IsNullOrEmpty(searchText))
                {
                    var filteredRows = dt.Select($"StaffName LIKE '%{searchText}%' OR EmailAddress LIKE '%{searchText}%' OR MobileNo LIKE '%{searchText}%'");
                    dt = filteredRows.Length > 0 ? filteredRows.CopyToDataTable() : dt.Clone();
                }

                // Get departments for dropdown
                DataTable departments = DepartmentDAL.SelectAll();
                ViewBag.Departments = departments;

                ViewBag.SearchText = searchText;
                return View(dt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading staff members: " + ex.Message;
                return View(new DataTable());
            }
        }

        // GET: Staff/AddEdit
        [SessionAuthorize]
        public IActionResult AddEdit(int? id, int? departmentId)
        {
            Staff model = new Staff();

            if (departmentId.HasValue)
            {
                model.DepartmentID = departmentId.Value;
            }

            if (id.HasValue)
            {
                try
                {
                    DataTable dt = StaffDAL.SelectByPK(id.Value);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        model.StaffID = Convert.ToInt32(row["StaffID"]);
                        model.DepartmentID = Convert.ToInt32(row["DepartmentID"]);
                        model.StaffName = row["StaffName"].ToString();
                        model.MobileNo = row["MobileNo"].ToString();
                        model.EmailAddress = row["EmailAddress"].ToString();
                        model.Remarks = row["Remarks"].ToString();
                        model.Created = Convert.ToDateTime(row["Created"]);
                        model.Modified = Convert.ToDateTime(row["Modified"]);
                    }
                    else
                    {
                        TempData["Error"] = "Staff member not found.";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error loading staff member: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }

            // Populate departments dropdown
            try
            {
                DataTable departments = DepartmentDAL.SelectAll();
                ViewBag.Departments = departments;
            }
            catch
            {
                ViewBag.Departments = new DataTable();
            }

            return View(model);
        }

        // POST: Staff/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public IActionResult Save(Staff model)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate departments dropdown
                DataTable departments = DepartmentDAL.SelectAll();
                ViewBag.Departments = departments;
                return View("AddEdit", model);
            }

            try
            {
                if (model.StaffID == 0)
                {
                    // Check if email already exists
                    if (StaffDAL.CheckEmailExists(model.EmailAddress, null))
                    {
                        ModelState.AddModelError("EmailAddress", "This email address is already registered.");
                        DataTable departments = DepartmentDAL.SelectAll();
                        ViewBag.Departments = departments;
                        return View("AddEdit", model);
                    }

                    // Insert new staff member
                    int newId = StaffDAL.Insert(model);
                    if (newId > 0)
                    {
                        TempData["Success"] = "Staff member added successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to add staff member.";
                        DataTable departments = DepartmentDAL.SelectAll();
                        ViewBag.Departments = departments;
                        return View("AddEdit", model);
                    }
                }
                else
                {
                    // Check if email already exists (excluding current staff)
                    if (StaffDAL.CheckEmailExists(model.EmailAddress, model.StaffID))
                    {
                        ModelState.AddModelError("EmailAddress", "This email address is already registered.");
                        DataTable departments = DepartmentDAL.SelectAll();
                        ViewBag.Departments = departments;
                        return View("AddEdit", model);
                    }

                    // Update existing staff member
                    int rowsAffected = StaffDAL.Update(model);
                    if (rowsAffected > 0)
                    {
                        TempData["Success"] = "Staff member updated successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to update staff member.";
                        DataTable departments = DepartmentDAL.SelectAll();
                        ViewBag.Departments = departments;
                        return View("AddEdit", model);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation
                {
                    ModelState.AddModelError("EmailAddress", "This email address is already registered.");
                }
                else
                {
                    ModelState.AddModelError("", "Database error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred: " + ex.Message);
            }

            // Repopulate departments dropdown on error
            DataTable depts = DepartmentDAL.SelectAll();
            ViewBag.Departments = depts;
            return View("AddEdit", model);
        }

        // POST: Staff/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public IActionResult Delete(int id)
        {
            try
            {
                int rowsAffected = StaffDAL.Delete(id);
                if (rowsAffected > 0)
                {
                    TempData["Success"] = "Staff member deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete staff member. They may be associated with meetings or have a user account.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting staff member: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: Staff/Details
        [SessionAuthorize]
        public IActionResult Details(int id)
        {
            try
            {
                DataTable dt = StaffDAL.SelectByPK(id);
                if (dt.Rows.Count > 0)
                {
                    return View(dt);
                }
                else
                {
                    TempData["Error"] = "Staff member not found.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading staff member details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Staff/ExportToExcel
        [SessionAuthorize]
        public IActionResult ExportToExcel(int? departmentId)
        {
            try
            {
                DataTable dt;
                if (departmentId.HasValue)
                {
                    dt = StaffDAL.SelectByDepartment(departmentId.Value);
                }
                else
                {
                    dt = StaffDAL.SelectAll();
                }

                byte[] fileBytes = ExportHelper.ExportToExcel(dt, "StaffMembers");

                return File(fileBytes,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            $"StaffMembers_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error exporting data: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Staff/BulkDelete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public IActionResult BulkDelete(List<int> selectedIds)
        {
            if (selectedIds == null || !selectedIds.Any())
            {
                TempData["Warning"] = "No items selected for deletion.";
                return RedirectToAction("Index");
            }

            try
            {
                int deletedCount = 0;
                foreach (int id in selectedIds)
                {
                    if (StaffDAL.Delete(id) > 0)
                    {
                        deletedCount++;
                    }
                }

                if (deletedCount > 0)
                {
                    TempData["Success"] = $"{deletedCount} staff member(s) deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete selected staff members. They may be associated with meetings or have user accounts.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error during bulk delete: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: Staff/Meetings
        [SessionAuthorize]
        public IActionResult Meetings(int id)
        {
            try
            {
                DataTable staffDt = StaffDAL.SelectByPK(id);
                if (staffDt.Rows.Count == 0)
                {
                    TempData["Error"] = "Staff member not found.";
                    return RedirectToAction("Index");
                }

                DataTable meetingsDt = StaffDAL.GetStaffMeetings(id);
                ViewBag.Staff = staffDt.Rows[0];
                return View(meetingsDt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading staff meetings: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // AJAX: Check if email exists
        [HttpPost]
        [SessionAuthorize]
        public IActionResult CheckEmail(string emailAddress, int? excludeId)
        {
            try
            {
                bool exists = StaffDAL.CheckEmailExists(emailAddress, excludeId);
                return Json(new { available = !exists });
            }
            catch
            {
                return Json(new { available = false, error = true });
            }
        }

        // AJAX: Get staff statistics
        [HttpPost]
        [SessionAuthorize]
        public IActionResult GetStaffStatistics(int staffId)
        {
            try
            {
                var stats = new
                {
                    totalMeetings = StaffDAL.GetStaffMeetingCount(staffId),
                    attendedMeetings = StaffDAL.GetStaffAttendedMeetingCount(staffId),
                    upcomingMeetings = StaffDAL.GetStaffUpcomingMeetingCount(staffId),
                    attendanceRate = StaffDAL.GetStaffAttendanceRate(staffId)
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