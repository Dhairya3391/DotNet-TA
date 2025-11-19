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
    public class MeetingVenueController : Controller
    {
        // GET: MeetingVenue
        [SessionAuthorize]
        public IActionResult Index()
        {
            try
            {
                DataTable dt = MeetingVenueDAL.SelectAll();
                return View(dt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading meeting venues: " + ex.Message;
                return View(new DataTable());
            }
        }

        // GET: MeetingVenue/AddEdit
        [SessionAuthorize]
        public IActionResult AddEdit(int? id)
        {
            MeetingVenue model = new MeetingVenue();

            if (id.HasValue)
            {
                try
                {
                    DataTable dt = MeetingVenueDAL.SelectByPK(id.Value);
                    if (dt.Rows.Count > 0)
                    {
                        DataRow row = dt.Rows[0];
                        model.MeetingVenueID = Convert.ToInt32(row["MeetingVenueID"]);
                        model.MeetingVenueName = row["MeetingVenueName"].ToString();
                        model.Created = Convert.ToDateTime(row["Created"]);
                        model.Modified = Convert.ToDateTime(row["Modified"]);
                    }
                    else
                    {
                        TempData["Error"] = "Meeting venue not found.";
                        return RedirectToAction("Index");
                    }
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "Error loading meeting venue: " + ex.Message;
                    return RedirectToAction("Index");
                }
            }

            return View(model);
        }

        // POST: MeetingVenue/Save
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public IActionResult Save(MeetingVenue model)
        {
            if (!ModelState.IsValid)
            {
                return View("AddEdit", model);
            }

            try
            {
                if (model.MeetingVenueID == 0)
                {
                    // Insert new meeting venue
                    int newId = MeetingVenueDAL.Insert(model);
                    if (newId > 0)
                    {
                        TempData["Success"] = "Meeting venue added successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to add meeting venue.";
                        return View("AddEdit", model);
                    }
                }
                else
                {
                    // Update existing meeting venue
                    int rowsAffected = MeetingVenueDAL.Update(model);
                    if (rowsAffected > 0)
                    {
                        TempData["Success"] = "Meeting venue updated successfully!";
                    }
                    else
                    {
                        TempData["Error"] = "Failed to update meeting venue.";
                        return View("AddEdit", model);
                    }
                }

                return RedirectToAction("Index");
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation
                {
                    ModelState.AddModelError("MeetingVenueName", "This meeting venue already exists.");
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

        // POST: MeetingVenue/Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public IActionResult Delete(int id)
        {
            try
            {
                int rowsAffected = MeetingVenueDAL.Delete(id);
                if (rowsAffected > 0)
                {
                    TempData["Success"] = "Meeting venue deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete meeting venue. It may be referenced by existing meetings.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error deleting meeting venue: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: MeetingVenue/Details
        [SessionAuthorize]
        public IActionResult Details(int id)
        {
            try
            {
                DataTable dt = MeetingVenueDAL.SelectByPK(id);
                if (dt.Rows.Count > 0)
                {
                    return View(dt);
                }
                else
                {
                    TempData["Error"] = "Meeting venue not found.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading meeting venue details: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // GET: MeetingVenue/ExportToExcel
        [SessionAuthorize]
        public IActionResult ExportToExcel()
        {
            try
            {
                DataTable dt = MeetingVenueDAL.SelectAll();
                byte[] fileBytes = ExportHelper.ExportToExcel(dt, "MeetingVenues");

                return File(fileBytes,
                            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                            $"MeetingVenues_{DateTime.Now:yyyyMMdd}.xlsx");
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error exporting data: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // POST: MeetingVenue/BulkDelete
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
                    if (MeetingVenueDAL.Delete(id) > 0)
                    {
                        deletedCount++;
                    }
                }

                if (deletedCount > 0)
                {
                    TempData["Success"] = $"{deletedCount} meeting venue(s) deleted successfully!";
                }
                else
                {
                    TempData["Error"] = "Failed to delete selected meeting venues. They may be referenced by existing meetings.";
                }
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error during bulk delete: " + ex.Message;
            }

            return RedirectToAction("Index");
        }

        // GET: MeetingVenue/Schedule
        [SessionAuthorize]
        public IActionResult Schedule(int id)
        {
            try
            {
                DataTable venueDt = MeetingVenueDAL.SelectByPK(id);
                if (venueDt.Rows.Count == 0)
                {
                    TempData["Error"] = "Meeting venue not found.";
                    return RedirectToAction("Index");
                }

                ViewBag.Venue = venueDt.Rows[0];

                // Get today's schedule and next 7 days
                DataTable scheduleDt = MeetingVenueDAL.GetVenueSchedule(id, DateTime.Today, DateTime.Today.AddDays(7));
                return View(scheduleDt);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading venue schedule: " + ex.Message;
                return RedirectToAction("Index");
            }
        }

        // AJAX: Check if venue name exists
        [HttpPost]
        [SessionAuthorize]
        public IActionResult CheckVenueName(string venueName, int? excludeId)
        {
            try
            {
                bool exists = MeetingVenueDAL.CheckVenueNameExists(venueName, excludeId);
                return Json(new { available = !exists });
            }
            catch
            {
                return Json(new { available = false, error = true });
            }
        }

        // AJAX: Check venue availability
        [HttpPost]
        [SessionAuthorize]
        public IActionResult CheckAvailability(int venueId, DateTime meetingDate, int? excludeMeetingId)
        {
            try
            {
                bool isAvailable = MeetingVenueDAL.CheckAvailability(venueId, meetingDate, excludeMeetingId);
                return Json(new { available = isAvailable });
            }
            catch
            {
                return Json(new { available = false, error = true });
            }
        }

        // AJAX: Get venue statistics
        [HttpPost]
        [SessionAuthorize]
        public IActionResult GetVenueStatistics(int venueId)
        {
            try
            {
                var stats = new
                {
                    totalMeetings = MeetingVenueDAL.GetTotalMeetingCount(venueId),
                    upcomingMeetings = MeetingVenueDAL.GetUpcomingMeetingCount(venueId),
                    thisWeekMeetings = MeetingVenueDAL.GetThisWeekMeetingCount(venueId),
                    utilizationRate = MeetingVenueDAL.GetUtilizationRate(venueId)
                };

                return Json(stats);
            }
            catch
            {
                return Json(new { error = true });
            }
        }

        // AJAX: Get venue availability calendar
        [HttpPost]
        [SessionAuthorize]
        public IActionResult GetAvailabilityCalendar(int venueId, int year, int month)
        {
            try
            {
                DateTime startDate = new DateTime(year, month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1);

                var availability = MeetingVenueDAL.GetVenueAvailabilityCalendar(venueId, startDate, endDate);
                return Json(availability);
            }
            catch
            {
                return Json(new { error = true });
            }
        }
    }
}