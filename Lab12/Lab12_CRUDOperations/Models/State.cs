using System.ComponentModel.DataAnnotations;

namespace Lab12_CRUDOperations.Models
{
    public class State
    {
        public int StateID { get; set; }

        [Required(ErrorMessage = "State Name is required")]
        [StringLength(100, ErrorMessage = "State Name cannot exceed 100 characters")]
        [Display(Name = "State Name")]
        public string StateName { get; set; } = string.Empty;

        [Required(ErrorMessage = "State Code is required")]
        [StringLength(10, ErrorMessage = "State Code cannot exceed 10 characters")]
        [Display(Name = "State Code")]
        public string StateCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        [Display(Name = "Country")]
        public int CountryID { get; set; }

        public bool IsActive { get; set; } = true;

        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        // Navigation properties
        public string? CountryName { get; set; }

        // Computed properties
        public string Status => IsActive ? "Active" : "Inactive";
    }
}