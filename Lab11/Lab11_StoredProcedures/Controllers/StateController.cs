using Lab11_StoredProcedures.Data;
using Lab11_StoredProcedures.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab11_StoredProcedures.Controllers
{
    public class StateController : Controller
    {
        private readonly StateRepository _stateRepository;

        public StateController(StateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        // GET: State
        public IActionResult Index()
        {
            try
            {
                List<State> states = _stateRepository.GetAllStates();
                return View(states);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving states: {ex.Message}";
                return View(new List<State>());
            }
        }

        // GET: State/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                State? state = _stateRepository.GetStateById(id);
                if (state == null)
                {
                    return NotFound();
                }
                return View(state);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving state details: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: State/StatesWithCityCount
        // Lab 11 specific: Display states with city count by country
        public IActionResult StatesWithCityCount()
        {
            try
            {
                List<State> states = _stateRepository.GetStatesWithCityCount();
                return View(states);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving states with city count: {ex.Message}";
                return View(new List<State>());
            }
        }

        // GET: State/TestSelectAll
        public IActionResult TestSelectAll()
        {
            try
            {
                var states = _stateRepository.GetAllStates();
                ViewBag.Message = $"Successfully retrieved {states.Count} states using PR_State_SelectAll";
                return View("TestResult", states);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error testing PR_State_SelectAll: {ex.Message}";
                return View("TestResult", new List<State>());
            }
        }

        // GET: State/TestSelectByPK/5
        public IActionResult TestSelectByPK(int id)
        {
            try
            {
                var state = _stateRepository.GetStateById(id);
                if (state != null)
                {
                    ViewBag.Message = $"Successfully retrieved state using PR_State_SelectByPK with ID: {id}";
                    return View("TestResult", new List<State> { state });
                }
                else
                {
                    ViewBag.Message = $"No state found with ID: {id}";
                    return View("TestResult", new List<State>());
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error testing PR_State_SelectByPK: {ex.Message}";
                return View("TestResult", new List<State>());
            }
        }

        // GET: State/TestStateWithCityCount
        public IActionResult TestStateWithCityCount()
        {
            try
            {
                var states = _stateRepository.GetStatesWithCityCount();
                ViewBag.Message = $"Successfully retrieved {states.Count} states with city count using PR_State_SelectWithCityCount";
                return View("TestResult", states);
            }
            catch (Exception ex)
            {
                ViewBag.Message = $"Error testing PR_State_SelectWithCityCount: {ex.Message}";
                return View("TestResult", new List<State>());
            }
        }
    }
}