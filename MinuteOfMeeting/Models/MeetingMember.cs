using System.ComponentModel.DataAnnotations;

namespace MinuteOfMeeting.Models
{
    /// <summary>
    /// Meeting Member Model
    /// Represents attendance tracking for meetings (Many-to-Many relationship)
    /// </summary>
    public class MeetingMember
    {
        public int MeetingMemberID { get; set; }

        [Required(ErrorMessage = "Meeting is required")]
        [Display(Name = "Meeting")]
        public int MeetingID { get; set; }

        [Required(ErrorMessage = "Staff member is required")]
        [Display(Name = "Staff Member")]
        public int StaffID { get; set; }

        [Required(ErrorMessage = "Attendance status is required")]
        [Display(Name = "Present")]
        public bool IsPresent { get; set; }

        [StringLength(250, ErrorMessage = "Remarks cannot exceed 250 characters")]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; } = DateTime.Now;

        [Display(Name = "Modified Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Modified { get; set; } = DateTime.Now;

        // Navigation properties for display
        public string StaffName { get; set; }
        public string MobileNo { get; set; }
        public string EmailAddress { get; set; }
        public string DepartmentName { get; set; }

        // Meeting information for staff view
        public string MeetingDateTime { get; set; }
        public string MeetingDescription { get; set; }
        public string MeetingVenueName { get; set; }
        public string MeetingTypeName { get; set; }
        public string MeetingStatus { get; set; }
    }
}