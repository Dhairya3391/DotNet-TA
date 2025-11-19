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
    [SessionAuthorize]
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public IActionResult Index()
        {
            try
            {
                DashboardViewModel model = new DashboardViewModel();

                // Get basic statistics
                model.TotalMeetings = DashboardDAL.GetTotalMeetings();
                model.UpcomingMeetings = DashboardDAL.GetUpcomingMeetingsCount();
                model.CompletedMeetings = DashboardDAL.GetCompletedMeetingsCount();
                model.CancelledMeetings = DashboardDAL.GetCancelledMeetingsCount();

                // Get recent meetings
                model.RecentMeetings = DashboardDAL.GetRecentMeetings(10);

                // Get upcoming meetings
                model.UpcomingMeetingsList = DashboardDAL.GetUpcomingMeetings(10);

                // Get meetings by type for chart
                model.MeetingsByType = DashboardDAL.GetMeetingsByType();

                // Get meetings by department for chart
                model.MeetingsByDepartment = DashboardDAL.GetMeetingsByDepartment();

                // Get monthly meeting trend
                model.MonthlyMeetingTrend = DashboardDAL.GetMonthlyMeetingTrend();

                // Get most active departments
                model.MostActiveDepartments = DashboardDAL.GetMostActiveDepartments(5);

                // Get staff participation stats
                model.StaffParticipation = DashboardDAL.GetStaffParticipation(5);

                // Get today's meetings
                model.TodayMeetings = DashboardDAL.GetTodayMeetings();

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading dashboard: " + ex.Message;
                return View(new DashboardViewModel());
            }
        }

        // GET: Dashboard/GetChartData
        public IActionResult GetChartData(string chartType)
        {
            try
            {
                object data = null;

                switch (chartType.ToLower())
                {
                    case "meetingsbytype":
                        data = DashboardDAL.GetMeetingsByType();
                        break;
                    case "meetingsbydepartment":
                        data = DashboardDAL.GetMeetingsByDepartment();
                        break;
                    case "monthlytrend":
                        data = DashboardDAL.GetMonthlyMeetingTrend();
                        break;
                    case "departmentparticipation":
                        data = DashboardDAL.GetDepartmentParticipation();
                        break;
                    default:
                        return Json(new { error = "Invalid chart type" });
                }

                return Json(data);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // GET: Dashboard/QuickStats
        public IActionResult QuickStats()
        {
            try
            {
                var stats = new
                {
                    totalMeetings = DashboardDAL.GetTotalMeetings(),
                    upcomingMeetings = DashboardDAL.GetUpcomingMeetingsCount(),
                    todayMeetings = DashboardDAL.GetTodayMeetingsCount(),
                    thisWeekMeetings = DashboardDAL.GetThisWeekMeetingsCount(),
                    totalStaff = DashboardDAL.GetTotalStaffCount(),
                    totalDepartments = DashboardDAL.GetTotalDepartmentsCount(),
                    totalVenues = DashboardDAL.GetTotalVenuesCount(),
                    totalTypes = DashboardDAL.GetTotalTypesCount()
                };

                return Json(stats);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // GET: Dashboard/Calendar
        public IActionResult Calendar(int year, int month)
        {
            try
            {
                if (year == 0) year = DateTime.Now.Year;
                if (month == 0) month = DateTime.Now.Month;

                var calendarData = DashboardDAL.GetCalendarData(year, month);
                ViewBag.Year = year;
                ViewBag.Month = month;
                ViewBag.MonthName = new DateTime(year, month, 1).ToString("MMMM yyyy");

                return View(calendarData);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading calendar: " + ex.Message;
                return View(new DataTable());
            }
        }

        // GET: Dashboard/RecentActivity
        public IActionResult RecentActivity()
        {
            try
            {
                DataTable activities = DashboardDAL.GetRecentActivities(20);
                return PartialView("_RecentActivityPartial", activities);
            }
            catch (Exception ex)
            {
                return PartialView("_RecentActivityPartial", new DataTable());
            }
        }

        // GET: Dashboard/Notifications
        public IActionResult Notifications()
        {
            try
            {
                var notifications = DashboardDAL.GetNotifications(SessionHelper.GetUserID(HttpContext));
                return Json(notifications);
            }
            catch (Exception ex)
            {
                return Json(new { error = ex.Message });
            }
        }

        // POST: Dashboard/MarkNotificationRead
        [HttpPost]
        public IActionResult MarkNotificationRead(int notificationId)
        {
            try
            {
                bool success = DashboardDAL.MarkNotificationRead(notificationId, SessionHelper.GetUserID(HttpContext));
                return Json(new { success = success });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, error = ex.Message });
            }
        }

        // GET: Dashboard/StaffDashboard (for staff role users)
        public IActionResult StaffDashboard()
        {
            try
            {
                int staffId = SessionHelper.GetStaffID(HttpContext) ?? 0;
                if (staffId == 0)
                {
                    TempData["Error"] = "Staff profile not linked to your account.";
                    return RedirectToAction("Profile", "Account");
                }

                var model = new StaffDashboardViewModel
                {
                    StaffId = staffId,
                    TotalMeetings = DashboardDAL.GetStaffTotalMeetings(staffId),
                    UpcomingMeetings = DashboardDAL.GetStaffUpcomingMeetings(staffId),
                    AttendedMeetings = DashboardDAL.GetStaffAttendedMeetings(staffId),
                    AttendanceRate = DashboardDAL.GetStaffAttendanceRate(staffId),
                    RecentMeetings = DashboardDAL.GetStaffRecentMeetings(staffId, 5),
                    UpcomingMeetingsList = DashboardDAL.GetStaffUpcomingMeetingsList(staffId, 5)
                };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading staff dashboard: " + ex.Message;
                return View(new StaffDashboardViewModel());
            }
        }

        // GET: Dashboard/AdminDashboard
        [SessionAuthorize("Admin")]
        public IActionResult AdminDashboard()
        {
            try
            {
                var model = new AdminDashboardViewModel();

                // System statistics
                model.TotalUsers = DashboardDAL.GetTotalUsersCount();
                model.ActiveUsers = DashboardDAL.GetActiveUsersCount();
                model.TotalStaff = DashboardDAL.GetTotalStaffCount();
                model.TotalDepartments = DashboardDAL.GetTotalDepartmentsCount();
                model.TotalVenues = DashboardDAL.GetTotalVenuesCount();
                model.TotalMeetingTypes = DashboardDAL.GetTotalTypesCount();

                // Meeting statistics
                model.TotalMeetings = DashboardDAL.GetTotalMeetings();
                model.MeetingsThisMonth = DashboardDAL.GetThisMonthMeetingsCount();
                model.MeetingsThisWeek = DashboardDAL.GetThisWeekMeetingsCount();
                model.TodayMeetings = DashboardDAL.GetTodayMeetingsCount();

                // System health
                model.SystemUptime = DashboardDAL.GetSystemUptime();
                model.LastBackup = DashboardDAL.GetLastBackupTime();
                model.DatabaseSize = DashboardDAL.GetDatabaseSize();

                // Recent user activities
                model.RecentUserActivities = DashboardDAL.GetRecentUserActivities(10);

                // Storage usage
                model.StorageUsage = DashboardDAL.GetStorageUsage();

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading admin dashboard: " + ex.Message;
                return View(new AdminDashboardViewModel());
            }
        }
    }

    // Additional View Models for Dashboard
    public class DashboardViewModel
    {
        public int TotalMeetings { get; set; }
        public int UpcomingMeetings { get; set; }
        public int CompletedMeetings { get; set; }
        public int CancelledMeetings { get; set; }

        public DataTable RecentMeetings { get; set; }
        public DataTable UpcomingMeetingsList { get; set; }
        public DataTable TodayMeetings { get; set; }

        public DataTable MeetingsByType { get; set; }
        public DataTable MeetingsByDepartment { get; set; }
        public DataTable MonthlyMeetingTrend { get; set; }
        public DataTable MostActiveDepartments { get; set; }
        public DataTable StaffParticipation { get; set; }
    }

    public class StaffDashboardViewModel
    {
        public int StaffId { get; set; }
        public int TotalMeetings { get; set; }
        public int UpcomingMeetings { get; set; }
        public int AttendedMeetings { get; set; }
        public double AttendanceRate { get; set; }

        public DataTable RecentMeetings { get; set; }
        public DataTable UpcomingMeetingsList { get; set; }
    }

    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int ActiveUsers { get; set; }
        public int TotalStaff { get; set; }
        public int TotalDepartments { get; set; }
        public int TotalVenues { get; set; }
        public int TotalMeetingTypes { get; set; }

        public int TotalMeetings { get; set; }
        public int MeetingsThisMonth { get; set; }
        public int MeetingsThisWeek { get; set; }
        public int TodayMeetings { get; set; }

        public string SystemUptime { get; set; }
        public DateTime? LastBackup { get; set; }
        public string DatabaseSize { get; set; }

        public DataTable RecentUserActivities { get; set; }
        public DataTable StorageUsage { get; set; }
    }
}