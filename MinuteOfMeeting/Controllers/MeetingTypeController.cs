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
    public class MeetingTypeController : Controller
    {
        // GET: MeetingType
        [SessionAuthorize]
        public IActionResult Index()
        {
            try
            {
                DataTable dt = MeetingTypeDAL.SelectAll();
                return View(dt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading meeting types: " + ex.Message;
                return View(new DataTable());
            }
        }

        // GET: MeetingType/AddEdit
        [SessionAuthorize]
        public IActionResult AddEdit(int? id)
        {
            MeetingType model = new MeetingType();

            if (id.HasValue)
            {
                try
                {
                    DataTable dt = MeetingTypeDAL.SelectByPK(id.Value);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        model.MeetingTypeID = Convert.ToInt32(row["MeetingTypeID"]);
                        model.MeetingTypeName = row["MeetingTypeName"].ToString();
                        model.Remarks = row["Remarks"].ToString();
                        model.Created = Convert.ToDateTime(row["Created"]);
                        model.Modified = Convert.ToDateTime(row["Modified"]);
                    }
                    else
                    {
                        TempData["Error"] = "Meeting type not found.";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error loading meeting type: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        // POST: MeetingType/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public IActionResult Save(MeetingType model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddEdit", model);
            }

            try
            {
                if (model.MeetingTypeID == 0)
                {
                    // Insert new meeting type
                    int newId = MeetingTypeDAL.Insert(model);
                    if (newId > 0)
                    {
                        TempData["Success"] = "Meeting type added successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to add meeting type.";
                        return View("AddEdit", model);
                    }
                }
                else
                {
                    // Update existing meeting type
                    int rowsAffected = MeetingTypeDAL.Update(model);
                    if (rowsAffected > 0)
                    {
                        TempData["Success"] = "Meeting type updated successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to update meeting type.";
                        return View("AddEdit", model);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation
                {
                    ModelState.AddModelError("MeetingTypeName", "This meeting type already exists.");
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

        // POST: MeetingType/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public IActionResult Delete(int id)
        {
            try
            {
                int rowsAffected = MeetingTypeDAL.Delete(id);
                if (rowsAffected > 0)
                {
                    TempData["Success"] = "Meeting type deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete meeting type. It may be referenced by other records.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting meeting type: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: MeetingType/Details
        [SessionAuthorize]
        public IActionResult Details(int id)
        {
            try
            {
                DataTable dt = MeetingTypeDAL.SelectByPK(id);
                if (dt.Rows.Count > 0)
                {
                    return View(dt);
                }
                else
                {
                    TempData["Error"] = "Meeting type not found.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading meeting type details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: MeetingType/ExportToExcel
        [SessionAuthorize]
        public IActionResult ExportToExcel()
        {
            try
            {
                DataTable dt = MeetingTypeDAL.SelectAll();
                byte[] fileBytes = ExportHelper.ExportToExcel(dt, "MeetingTypes");

                return File(fileBytes,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            $"MeetingTypes_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error exporting data: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: MeetingType/BulkDelete
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
                    if (MeetingTypeDAL.Delete(id) > 0)
                    {
                        deletedCount++;
                    }
                }

                if (deletedCount > 0)
                {
                    TempData["Success"] = $"{deletedCount} meeting type(s) deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete selected meeting types. They may be referenced by other records.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error during bulk delete: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // AJAX: Check if meeting type name exists
        [HttpPost]
        [SessionAuthorize]
        public IActionResult CheckMeetingTypeName(string meetingTypeName, int? excludeId)
        {
            try
            {
                bool exists = MeetingTypeDAL.CheckMeetingTypeNameExists(meetingTypeName, excludeId);
                return Json(new { available = !exists });
            }
            catch
            {
                return Json(new { available = false, error = true });
            }
        }
    }
}