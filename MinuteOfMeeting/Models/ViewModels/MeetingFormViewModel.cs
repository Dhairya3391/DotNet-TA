using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MinuteOfMeeting.Models.ViewModels
{
    /// <summary>
    /// Meeting Form View Model
    /// Used for meeting creation/editing forms with dropdown data
    /// </summary>
    public class MeetingFormViewModel
    {
        public int MeetingID { get; set; }

        [Required(ErrorMessage = "Meeting date and time is required")]
        [Display(Name = "Meeting Date & Time")]
        [DataType(DataType.DateTime)]
        public DateTime MeetingDate { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "Venue is required")]
        [Display(Name = "Venue")]
        public int MeetingVenueID { get; set; }

        [Required(ErrorMessage = "Meeting type is required")]
        [Display(Name = "Meeting Type")]
        public int MeetingTypeID { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        [StringLength(250, ErrorMessage = "Description cannot exceed 250 characters")]
        [Display(Name = "Meeting Description")]
        public string MeetingDescription { get; set; }

        [Display(Name = "Meeting Document")]
        public IFormFile DocumentFile { get; set; }

        public string ExistingDocumentPath { get; set; }

        // Dropdown lists
        public List<SelectListItem> VenueList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MeetingTypeList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DepartmentList { get; set; } = new List<SelectListItem>();

        // Conflict detection
        public bool HasConflict { get; set; }
        public string ConflictMessage { get; set; }
    }

    /// <summary>
    /// Meeting Filter View Model
    /// Used for filtering meetings on list page
    /// </summary>
    public class MeetingFilterViewModel
    {
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime? StartDate { get; set; } = DateTime.Now.AddMonths(-1);

        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime? EndDate { get; set; } = DateTime.Now.AddMonths(1);

        [Display(Name = "Meeting Type")]
        public int? MeetingTypeID { get; set; }

        [Display(Name = "Venue")]
        public int? MeetingVenueID { get; set; }

        [Display(Name = "Department")]
        public int? DepartmentID { get; set; }

        [Display(Name = "Search")]
        public string SearchKeyword { get; set; }

        [Display(Name = "Status")]
        public string Status { get; set; } // Upcoming, Completed, Cancelled

        // Dropdown lists
        public List<SelectListItem> VenueList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> MeetingTypeList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> DepartmentList { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> StatusList { get; set; } = new List<SelectListItem>();
    }

    /// <summary>
    /// Meeting Cancellation View Model
    /// Used for meeting cancellation form
    /// </summary>
    public class MeetingCancellationViewModel
    {
        public int MeetingID { get; set; }
        public string MeetingDescription { get; set; }
        public string MeetingDateTime { get; set; }
        public string MeetingVenueName { get; set; }
        public string MeetingTypeName { get; set; }
        public string DepartmentName { get; set; }

        [Required(ErrorMessage = "Cancellation reason is required")]
        [StringLength(250, ErrorMessage = "Cancellation reason cannot exceed 250 characters")]
        [Display(Name = "Cancellation Reason")]
        public string CancellationReason { get; set; }

        public DateTime CancellationDateTime { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// Meeting Details View Model
    /// Used for displaying detailed meeting information
    /// </summary>
    public class MeetingDetailsViewModel
    {
        public int MeetingID { get; set; }
        public string MeetingDateTime { get; set; }
        public string MeetingDescription { get; set; }
        public string DocumentPath { get; set; }
        public string MeetingVenueName { get; set; }
        public string MeetingTypeName { get; set; }
        public string DepartmentName { get; set; }
        public string MeetingStatus { get; set; }
        public int AttendeeCount { get; set; }
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }
        public bool IsCancelled { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? CancellationDateTime { get; set; }

        // Attendance summary
        public List<MeetingMember> Attendees { get; set; } = new List<MeetingMember>();
        public int TotalInvited { get; set; }
        public int PresentCount { get; set; }
        public int AbsentCount { get; set; }
        public decimal AttendancePercentage { get; set; }
    }
}