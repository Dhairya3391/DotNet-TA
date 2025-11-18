using System.ComponentModel.DataAnnotations;

namespace Lab11_StoredProcedures.Models
{
    public class City
    {
        public int CityID { get; set; }

        [Required(ErrorMessage = "City Name is required")]
        [StringLength(100, ErrorMessage = "City Name cannot exceed 100 characters")]
        [Display(Name = "City Name")]
        public string CityName { get; set; } = string.Empty;

        [Required(ErrorMessage = "City Code is required")]
        [StringLength(10, ErrorMessage = "City Code cannot exceed 10 characters")]
        [Display(Name = "City Code")]
        public string CityCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        [Display(Name = "State")]
        public int StateID { get; set; }

        // Navigation properties
        public string? StateName { get; set; }
        public string? CountryName { get; set; }
        public int CountryID { get; set; }
    }
}