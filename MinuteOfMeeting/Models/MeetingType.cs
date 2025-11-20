using System.ComponentModel.DataAnnotations;

namespace MinuteOfMeeting.Models
{
    /// <summary>
    /// Meeting Type Model
    /// Represents different types of meetings (Review, Planning, Training, etc.)
    /// </summary>
    public class MeetingType
    {
        public int MeetingTypeID { get; set; }

        [Required(ErrorMessage = "Meeting type is required")]
        [StringLength(100, ErrorMessage = "Meeting type name cannot exceed 100 characters")]
        [Display(Name = "Meeting Type")]
        public string MeetingTypeName { get; set; }

        [Required(ErrorMessage = "Remarks are required")]
        [StringLength(100, ErrorMessage = "Remarks cannot exceed 100 characters")]
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
    }
}