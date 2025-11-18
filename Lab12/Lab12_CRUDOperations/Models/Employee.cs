using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Lab12_CRUDOperations.Models
{
    public class Employee
    {
        public int EmployeeID { get; set; }

        [Required(ErrorMessage = "Employee Code is required")]
        [StringLength(20, ErrorMessage = "Employee Code cannot exceed 20 characters")]
        [Display(Name = "Employee Code")]
        public string EmployeeCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100, ErrorMessage = "Email cannot exceed 100 characters")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "Phone Number cannot exceed 20 characters")]
        [Phone(ErrorMessage = "Invalid phone number format")]
        [Display(Name = "Phone Number")]
        public string? PhoneNumber { get; set; }

        [Required(ErrorMessage = "Department is required")]
        [StringLength(50, ErrorMessage = "Department cannot exceed 50 characters")]
        [Display(Name = "Department")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "Position is required")]
        [StringLength(50, ErrorMessage = "Position cannot exceed 50 characters")]
        [Display(Name = "Position")]
        public string Position { get; set; } = string.Empty;

        [Required(ErrorMessage = "Salary is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Salary must be greater than 0")]
        [Column(TypeName = "decimal(18,2)")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        [Display(Name = "Salary")]
        public decimal Salary { get; set; }

        [Required(ErrorMessage = "Hire Date is required")]
        [DataType(DataType.Date)]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }

        public bool IsActive { get; set; } = true;

        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        // Computed properties
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        public string EmploymentStatus => IsActive ? "Active" : "Inactive";

        [NotMapped]
        public int YearsOfService
        {
            get
            {
                var today = DateTime.Today;
                var years = today.Year - HireDate.Year;
                if (HireDate.Date > today.AddYears(-years)) years--;
                return years;
            }
        }
    }
}