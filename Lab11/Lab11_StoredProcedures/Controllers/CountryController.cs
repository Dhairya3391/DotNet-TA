using Lab11_StoredProcedures.Data;
using Lab11_StoredProcedures.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab11_StoredProcedures.Controllers
{
    public class CountryController : Controller
    {
        private readonly CountryRepository _countryRepository;

        public CountryController(CountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        // GET: Country
        public IActionResult Index()
        {
            try
            {
                List<Country> countries = _countryRepository.GetAllCountries();
                return View(countries);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving countries: {ex.Message}";
                return View(new List<Country>());
            }
        }

        // GET: Country/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                Country? country = _countryRepository.GetCountryById(id);
                if (country == null)
                {
                    return NotFound();
                }
                return View(country);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving country details: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Country/TestSelectAll
        public IActionResult TestSelectAll()
        {
            try
            {
                var countries = _countryRepository.GetAllCountries();
                ViewBag.Message = $"Successfully retrieved {countries.Count} countries using PR_Country_SelectAll";
                return View("TestResult", countries);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error testing PR_Country_SelectAll: {ex.Message}";
                return View("TestResult", new List<Country>());
            }
        }

        // GET: Country/TestSelectByPK/5
        public IActionResult TestSelectByPK(int id)
        {
            try
            {
                var country = _countryRepository.GetCountryById(id);
                if (country != null)
                {
                    ViewBag.Message = $"Successfully retrieved country using PR_Country_SelectByPK with ID: {id}";
                    return View("TestResult", new List<Country> { country });
                }
                else
                {
                    ViewBag.Message = $"No country found with ID: {id}";
                    return View("TestResult", new List<Country>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error testing PR_Country_SelectByPK: {ex.Message}";
                return View("TestResult", new List<Country>());
            }
        }
    }
}