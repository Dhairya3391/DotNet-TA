using System.ComponentModel.DataAnnotations;

namespace Lab11_StoredProcedures.Models
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
    }
}