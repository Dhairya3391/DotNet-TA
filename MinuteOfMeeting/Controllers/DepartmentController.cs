using Microsoft.AspNetCore.Mvc;
using MinuteOfMeeting.DAL;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using MinuteOfMeeting.Helpers;
using MinuteOfMeeting.Models;
using System.Data;

namespace MinuteOfMeeting.Controllers
{
    public class DepartmentController : Controller
    {
        // GET: Department
        [SessionAuthorize]
        public IActionResult Index()
        {
            try
            {
                DataTable dt = DepartmentDAL.SelectAll();
                return View(dt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading departments: " + ex.Message;
                return View(new DataTable());
            }
        }

        // GET: Department/AddEdit
        [SessionAuthorize]
        public IActionResult AddEdit(int? id)
        {
            Department model = new Department();

            if (id.HasValue)
            {
                try
                {
                    DataTable dt = DepartmentDAL.SelectByPK(id.Value);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        model.DepartmentID = Convert.ToInt32(row["DepartmentID"]);
                        model.DepartmentName = row["DepartmentName"].ToString();
                        model.Created = Convert.ToDateTime(row["Created"]);
                        model.Modified = Convert.ToDateTime(row["Modified"]);
                    }
                    else
                    {
                        TempData["Error"] = "Department not found.";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error loading department: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        // POST: Department/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public IActionResult Save(Department model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddEdit", model);
            }

            try
            {
                if (model.DepartmentID == 0)
                {
                    // Insert new department
                    int newId = DepartmentDAL.Insert(model);
                    if (newId > 0)
                    {
                        TempData["Success"] = "Department added successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to add department.";
                        return View("AddEdit", model);
                    }
                }
                else
                {
                    // Update existing department
                    int rowsAffected = DepartmentDAL.Update(model);
                    if (rowsAffected > 0)
                    {
                        TempData["Success"] = "Department updated successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to update department.";
                        return View("AddEdit", model);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation
                {
                    ModelState.AddModelError("DepartmentName", "This department already exists.");
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

            return View("AddEdit", model);
        }

        // POST: Department/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public IActionResult Delete(int id)
        {
            try
            {
                int rowsAffected = DepartmentDAL.Delete(id);
                if (rowsAffected > 0)
                {
                    TempData["Success"] = "Department deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete department. It may be referenced by staff members or meetings.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting department: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: Department/Details
        [SessionAuthorize]
        public IActionResult Details(int id)
        {
            try
            {
                DataTable dt = DepartmentDAL.SelectByPK(id);
                if (dt.Rows.Count > 0)
                {
                    return View(dt);
                }
                else
                {
                    TempData["Error"] = "Department not found.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading department details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: Department/ExportToExcel
        [SessionAuthorize]
        public IActionResult ExportToExcel()
        {
            try
            {
                DataTable dt = DepartmentDAL.SelectAll();
                byte[] fileBytes = ExportHelper.ExportToExcel(dt, "Departments");

                return File(fileBytes,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            $"Departments_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error exporting data: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: Department/BulkDelete
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
                    if (DepartmentDAL.Delete(id) > 0)
                    {
                        deletedCount++;
                    }
                }

                if (deletedCount > 0)
                {
                    TempData["Success"] = $"{deletedCount} department(s) deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete selected departments. They may be referenced by staff members or meetings.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error during bulk delete: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: Department/StaffMembers
        [SessionAuthorize]
        public IActionResult StaffMembers(int id)
        {
            try
            {
                DataTable departmentDt = DepartmentDAL.SelectByPK(id);
                if (departmentDt.Rows.Count == 0)
                {
                    TempData["Error"] = "Department not found.";
                    return RedirectToAction("Index");
                }

                DataTable staffDt = StaffDAL.SelectByDepartment(id);
                ViewBag.Department = departmentDt.Rows[0];
                return View(staffDt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading staff members: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // AJAX: Check if department name exists
        [HttpPost]
        [SessionAuthorize]
        public IActionResult CheckDepartmentName(string departmentName, int? excludeId)
        {
            try
            {
                bool exists = DepartmentDAL.CheckDepartmentNameExists(departmentName, excludeId);
                return Json(new { available = !exists });
            }
            catch
            {
                return Json(new { available = false, error = true });
            }
        }

        // AJAX: Get department statistics
        [HttpPost]
        [SessionAuthorize]
        public IActionResult GetDepartmentStatistics(int departmentId)
        {
            try
            {
                var stats = new
                {
                    totalStaff = StaffDAL.GetStaffCountByDepartment(departmentId),
                    totalMeetings = DepartmentDAL.GetMeetingCountByDepartment(departmentId),
                    upcomingMeetings = DepartmentDAL.GetUpcomingMeetingCountByDepartment(departmentId)
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