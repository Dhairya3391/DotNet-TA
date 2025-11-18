using Lab11_StoredProcedures.Data;
using Lab11_StoredProcedures.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab11_StoredProcedures.Controllers
{
    public class CityController : Controller
    {
        private readonly CityRepository _cityRepository;
        private readonly StateRepository _stateRepository;

        public CityController(CityRepository cityRepository, StateRepository stateRepository)
        {
            _cityRepository = cityRepository;
            _stateRepository = stateRepository;
        }

        // GET: City
        public IActionResult Index()
        {
            try
            {
                List<City> cities = _cityRepository.GetAllCities();
                return View(cities);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving cities: {ex.Message}";
                return View(new List<City>());
            }
        }

        // GET: City/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                City? city = _cityRepository.GetCityById(id);
                if (city == null)
                {
                    return NotFound();
                }
                return View(city);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving city details: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: City/Search
        // Lab 11 specific: Filter by city name
        public IActionResult Search(string cityName)
        {
            try
            {
                List<City> cities;
                if (string.IsNullOrEmpty(cityName))
                {
                    cities = _cityRepository.GetAllCities();
                    ViewBag.SearchTerm = "All Cities";
                }
                else
                {
                    cities = _cityRepository.GetCitiesByName(cityName);
                    ViewBag.SearchTerm = $"Cities containing '{cityName}'";
                }
                return View(cities);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error searching cities: {ex.Message}";
                return View(new List<City>());
            }
        }

        // GET: City/ByState/5
        // Lab 11 specific: Display cities by state
        public IActionResult ByState(int stateId)
        {
            try
            {
                List<City> cities = _cityRepository.GetCitiesByState(stateId);
                var state = _stateRepository.GetStateById(stateId);
                ViewBag.StateName = state?.StateName ?? "Unknown State";
                return View(cities);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving cities by state: {ex.Message}";
                return View(new List<City>());
            }
        }

        // GET: City/TestSelectAll
        public IActionResult TestSelectAll()
        {
            try
            {
                var cities = _cityRepository.GetAllCities();
                ViewBag.Message = $"Successfully retrieved {cities.Count} cities using PR_City_SelectAll";
                return View("TestResult", cities);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error testing PR_City_SelectAll: {ex.Message}";
                return View("TestResult", new List<City>());
            }
        }

        // GET: City/TestSelectByPK/5
        public IActionResult TestSelectByPK(int id)
        {
            try
            {
                var city = _cityRepository.GetCityById(id);
                if (city != null)
                {
                    ViewBag.Message = $"Successfully retrieved city using PR_City_SelectByPK with ID: {id}";
                    return View("TestResult", new List<City> { city });
                }
                else
                {
                    ViewBag.Message = $"No city found with ID: {id}";
                    return View("TestResult", new List<City>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error testing PR_City_SelectByPK: {ex.Message}";
                return View("TestResult", new List<City>());
            }
        }

        // GET: City/TestSearchByName
        public IActionResult TestSearchByName(string cityName = "Ahmedabad")
        {
            try
            {
                var cities = _cityRepository.GetCitiesByName(cityName);
                ViewBag.Message = $"Successfully retrieved {cities.Count} cities using PR_City_SelectByName with search term: '{cityName}'";
                return View("TestResult", cities);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error testing PR_City_SelectByName: {ex.Message}";
                return View("TestResult", new List<City>());
            }
        }

        // GET: City/TestCitiesByState/5
        public IActionResult TestCitiesByState(int stateId = 1)
        {
            try
            {
                var cities = _cityRepository.GetCitiesByState(stateId);
                var state = _stateRepository.GetStateById(stateId);
                ViewBag.Message = $"Successfully retrieved {cities.Count} cities using PR_City_SelectByState for state: '{state?.StateName}' (ID: {stateId})";
                return View("TestResult", cities);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error testing PR_City_SelectByState: {ex.Message}";
                return View("TestResult", new List<City>());
            }
        }
    }
}