using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MinuteOfMeeting.Models.ViewModels
{
    /// <summary>
    /// Attendance Management View Model
    /// Used for managing meeting attendance
    /// </summary>
    public class AttendanceManagementViewModel
    {
        public int MeetingID { get; set; }
        public string MeetingDateTime { get; set; }
        public string MeetingDescription { get; set; }
        public string MeetingVenueName { get; set; }
        public string MeetingTypeName { get; set; }
        public string DepartmentName { get; set; }
        public string MeetingStatus { get; set; }

        // Staff selection
        [Display(Name = "Select Staff Members")]
        public List<int> SelectedStaffIDs { get; set; } = new List<int>();

        public List<SelectListItem> AvailableStaffList { get; set; } = new List<SelectListItem>();
        public List<MeetingMember> CurrentAttendees { get; set; } = new List<MeetingMember>();

        // Attendance summary
        public int TotalInvited { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public decimal AttendancePercentage { get; set; }

        // Success/Error messages
        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }

    /// <summary>
    /// Attendance Summary View Model
    /// Used for displaying attendance statistics
    /// </summary>
    public class AttendanceSummaryViewModel
    {
        public int MeetingID { get; set; }
        public string MeetingDateTime { get; set; }
        public string MeetingDescription { get; set; }
        public string MeetingVenueName { get; set; }
        public string MeetingTypeName { get; set; }
        public string DepartmentName { get; set; }

        public int TotalInvited { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public decimal AttendancePercentage { get; set; }

        public List<MeetingMember> Attendees { get; set; } = new List<MeetingMember>();
    }

    /// <summary>
    /// Staff Attendance History View Model
    /// Used for displaying individual staff member's meeting history
    /// </summary>
    public class StaffAttendanceHistoryViewModel
    {
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public string EmailAddress { get; set; }

        // Statistics
        public int TotalMeetings { get; set; }
        public int AttendedMeetings { get; set; }
        public int MissedMeetings { get; set; }
        public decimal AttendancePercentage { get; set; }

        // Meeting history
        public List<MeetingMember> MeetingHistory { get; set; } = new List<MeetingMember>();

        // Filter options
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }

    /// <summary>
    /// Bulk Attendance Update View Model
    /// Used for updating multiple attendance records at once
    /// </summary>
    public class BulkAttendanceUpdateViewModel
    {
        public int MeetingID { get; set; }
        public string MeetingDateTime { get; set; }
        public string MeetingDescription { get; set; }

        public List<AttendanceUpdateItem> Attendees { get; set; } = new List<AttendanceUpdateItem>();

        public string SuccessMessage { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class AttendanceUpdateItem
    {
        public int MeetingMemberID { get; set; }
        public int StaffID { get; set; }
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public bool IsPresent { get; set; }
        public string Remarks { get; set; }
    }

    /// <summary>
    /// Attendance Report View Model
    /// Used for generating attendance reports
    /// </summary>
    public class AttendanceReportViewModel
    {
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; }

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentID { get; set; }

        [Display(Name = "Meeting Type")]
        public int? MeetingTypeID { get; set; }

        [Display(Name = "Report Type")]
        public string ReportType { get; set; } // Summary, Detailed, Staff-wise

        // Dropdown lists
        public List<SelectListItem> DepartmentList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MeetingTypeList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> ReportTypeList { get; set; } = new List<SelectListItem>();

        // Report data
        public List<AttendanceReportItem> ReportData { get; set; } = new List<AttendanceReportItem>();
        public DateTime GeneratedOn { get; set; } = DateTime.Now;
    }

    public class AttendanceReportItem
    {
        public string MeetingDateTime { get; set; }
        public string MeetingDescription { get; set; }
        public string MeetingTypeName { get; set; }
        public string DepartmentName { get; set; }
        public string StaffName { get; set; }
        public string StaffDepartment { get; set; }
        public bool IsPresent { get; set; }
        public string Remarks { get; set; }
    }
}