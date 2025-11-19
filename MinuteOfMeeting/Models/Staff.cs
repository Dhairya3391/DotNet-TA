using System.ComponentModel.DataAnnotations;

namespace MinuteOfMeeting.Models
{
    /// <summary>
    /// Staff Model
    /// Represents staff/member information with department association
    /// </summary>
    public class Staff
    {
        public int StaffID { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [Display(Name = "Department")]
        public int DepartmentID { get; set; }

        [Required(ErrorMessage = "Staff name is required")]
        [StringLength(50, ErrorMessage = "Staff name cannot exceed 50 characters")]
        [Display(Name = "Staff Name")]
        public string StaffName { get; set; }

        [Required(ErrorMessage = "Mobile number is required")]
        [StringLength(20, ErrorMessage = "Mobile number cannot exceed 20 characters")]
        [RegularExpression(@"^[0-9+\-\s()]*$", ErrorMessage = "Enter a valid mobile number")]
        [Display(Name = "Mobile Number")]
        public string MobileNo { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [StringLength(50, ErrorMessage = "Email address cannot exceed 50 characters")]
        [EmailAddress(ErrorMessage = "Enter a valid email address")]
        [Display(Name = "Email Address")]
        public string EmailAddress { get; set; }

        [StringLength(250, ErrorMessage = "Remarks cannot exceed 250 characters")]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }

        [Display(Name = "Created Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Created { get; set; }

        [Display(Name = "Modified Date")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime Modified { get; set; }

        // Navigation properties (not stored in database, used for display)
        public string DepartmentName { get; set; }
    }
}