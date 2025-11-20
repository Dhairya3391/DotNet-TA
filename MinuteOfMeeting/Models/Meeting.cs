using System.ComponentModel.DataAnnotations;

namespace MinuteOfMeeting.Models
{
    /// <summary>
    /// Meeting Model
    /// Represents meeting records with scheduling and attendance information
    /// </summary>
    public class Meeting
    {
        public int MeetingID { get; set; }

        [Required(ErrorMessage = "Meeting date and time is required")]
        [Display(Name = "Meeting Date & Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime MeetingDate { get; set; }

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
        public string? MeetingDescription { get; set; }

        [StringLength(250, ErrorMessage = "Document path cannot exceed 250 characters")]
        [Display(Name = "Document Path")]
        public string? DocumentPath { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Display(Name = "Modified Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Modified { get; set; } = DateTime.Now;

        [Display(Name = "Is Cancelled")]
        public bool IsCancelled { get; set; }

        [Display(Name = "Cancellation Date & Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CancellationDateTime { get; set; }

        [StringLength(250, ErrorMessage = "Cancellation reason cannot exceed 250 characters")]
        [Display(Name = "Cancellation Reason")]
        public string? CancellationReason { get; set; }

        // Navigation properties for display (nullable since they're populated separately)
        public string? MeetingVenueName { get; set; }
        public string? MeetingTypeName { get; set; }
        public string? DepartmentName { get; set; }
        public string? MeetingDateTime { get; set; }
        public string? MeetingStatus { get; set; }
        public int AttendeeCount { get; set; }

        // File upload property (nullable since upload is optional)
        [Display(Name = "Meeting Document")]
        public IFormFile? DocumentFile { get; set; }
    }
}