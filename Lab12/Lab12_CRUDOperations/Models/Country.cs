using System.ComponentModel.DataAnnotations;

namespace Lab12_CRUDOperations.Models
{
    public class Country
    {
        public int CountryID { get; set; }

        [Required(ErrorMessage = "Country Name is required")]
        [StringLength(100, ErrorMessage = "Country Name cannot exceed 100 characters")]
        [Display(Name = "Country Name")]
        public string CountryName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country Code is required")]
        [StringLength(10, ErrorMessage = "Country Code cannot exceed 10 characters")]
        [Display(Name = "Country Code")]
        public string CountryCode { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        [DataType(DataType.DateTime)]
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        // Computed properties
        public string Status => IsActive ? "Active" : "Inactive";
    }
}