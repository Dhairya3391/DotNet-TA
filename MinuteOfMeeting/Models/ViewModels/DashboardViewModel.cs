using System.ComponentModel.DataAnnotations;

namespace MinuteOfMeeting.Models.ViewModels
{
    /// <summary>
    /// Dashboard View Model
    /// Contains dashboard statistics and data for display
    /// </summary>
    public class DashboardViewModel
    {
        // Overall Statistics
        [Display(Name = "Total Meetings")]
        public int TotalMeetings { get; set; }

        [Display(Name = "Upcoming Meetings")]
        public int UpcomingMeetings { get; set; }

        [Display(Name = "Completed Meetings")]
        public int CompletedMeetings { get; set; }

        [Display(Name = "Cancelled Meetings")]
        public int CancelledMeetings { get; set; }

        [Display(Name = "Total Staff")]
        public int TotalStaff { get; set; }

        [Display(Name = "Total Departments")]
        public int TotalDepartments { get; set; }

        [Display(Name = "Total Venues")]
        public int TotalVenues { get; set; }

        [Display(Name = "Total Users")]
        public int TotalUsers { get; set; }

        [Display(Name = "Today's Meetings")]
        public int TodaysMeetings { get; set; }

        [Display(Name = "This Week's Meetings")]
        public int ThisWeekMeetings { get; set; }

        [Display(Name = "This Month's Meetings")]
        public int ThisMonthMeetings { get; set; }

        // List data for display
        public List<UpcomingMeetingViewModel> UpcomingMeetingsList { get; set; } = new List<UpcomingMeetingViewModel>();
        public List<RecentMeetingViewModel> RecentMeetingsList { get; set; } = new List<RecentMeetingViewModel>();
        public List<MeetingByTypeViewModel> MeetingsByType { get; set; } = new List<MeetingByTypeViewModel>();
        public List<MeetingByDepartmentViewModel> MeetingsByDepartment { get; set; } = new List<MeetingByDepartmentViewModel>();
        public List<MonthlyMeetingTrendViewModel> MonthlyTrends { get; set; } = new List<MonthlyMeetingTrendViewModel>();
        public List<ActiveDepartmentViewModel> ActiveDepartments { get; set; } = new List<ActiveDepartmentViewModel>();
        public List<StaffParticipationViewModel> TopParticipants { get; set; } = new List<StaffParticipationViewModel>();
        public List<VenueUtilizationViewModel> VenueUtilization { get; set; } = new List<VenueUtilizationViewModel>();
    }

    public class UpcomingMeetingViewModel
    {
        public int MeetingID { get; set; }
        public string MeetingDateTime { get; set; }
        public string MeetingDescription { get; set; }
        public string MeetingVenueName { get; set; }
        public string MeetingTypeName { get; set; }
        public string DepartmentName { get; set; }
        public int AttendeeCount { get; set; }
        public string UrgencyLevel { get; set; }
        public int HoursUntilMeeting { get; set; }
    }

    public class RecentMeetingViewModel
    {
        public int MeetingID { get; set; }
        public string MeetingDateTime { get; set; }
        public string MeetingDescription { get; set; }
        public string MeetingVenueName { get; set; }
        public string MeetingTypeName { get; set; }
        public string DepartmentName { get; set; }
        public string Status { get; set; }
        public int AttendeeCount { get; set; }
        public int PresentCount { get; set; }
        public int DaysAgo { get; set; }
        public string CancellationReason { get; set; }
    }

    public class MeetingByTypeViewModel
    {
        public int MeetingTypeID { get; set; }
        public string MeetingTypeName { get; set; }
        public int MeetingCount { get; set; }
        public string Percentage { get; set; }
    }

    public class MeetingByDepartmentViewModel
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int MeetingCount { get; set; }
        public string Percentage { get; set; }
    }

    public class MonthlyMeetingTrendViewModel
    {
        public string YearMonth { get; set; }
        public string MonthLabel { get; set; }
        public int MeetingCount { get; set; }
        public int CancelledCount { get; set; }
        public int CompletedCount { get; set; }
    }

    public class ActiveDepartmentViewModel
    {
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        public int MeetingCount { get; set; }
        public int UniqueParticipants { get; set; }
        public decimal AvgAttendancePercentage { get; set; }
        public DateTime? LastMeetingDate { get; set; }
    }

    public class StaffParticipationViewModel
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public int TotalInvitations { get; set; }
        public int Attended { get; set; }
        public int Missed { get; set; }
        public decimal AttendancePercentage { get; set; }
    }

    public class VenueUtilizationViewModel
    {
        public int MeetingVenueID { get; set; }
        public string MeetingVenueName { get; set; }
        public int TotalMeetings { get; set; }
        public int DaysUsed { get; set; }
        public int DepartmentsUsed { get; set; }
        public int MeetingTypesUsed { get; set; }
        public DateTime? LastMeetingDate { get; set; }
        public decimal MeetingsPerDay { get; set; }
    }
}