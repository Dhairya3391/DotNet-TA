using System.ComponentModel.DataAnnotations;

namespace Lab11_StoredProcedures.Models
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

        // Navigation property for country name
        public string? CountryName { get; set; }

        // Additional property for Lab 11 requirements
        [Display(Name = "City Count")]
        public int CityCount { get; set; }
    }
}