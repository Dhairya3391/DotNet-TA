using Lab12_CRUDOperations.Data;
using Lab12_CRUDOperations.Models;
using Microsoft.AspNetCore.Mvc;

namespace Lab12_CRUDOperations.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly EmployeeRepository _employeeRepository;

        public EmployeeController(EmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        // GET: Employee
        public IActionResult Index()
        {
            try
            {
                List<Employee> employees = _employeeRepository.GetAllEmployees();
                return View(employees);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving employees: {ex.Message}";
                return View(new List<Employee>());
            }
        }

        // GET: Employee/Details/5
        public IActionResult Details(int id)
        {
            try
            {
                Employee? employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving employee details: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Employee employee)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    CRUDResult result = _employeeRepository.InsertEmployee(employee);
                    if (result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", result.Message);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error creating employee: {ex.Message}");
                }
            }
            return View(employee);
        }

        // GET: Employee/Edit/5
        public IActionResult Edit(int id)
        {
            try
            {
                Employee? employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving employee for editing: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Employee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Employee employee)
        {
            if (id != employee.EmployeeID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    CRUDResult result = _employeeRepository.UpdateEmployee(employee);
                    if (result.Success)
                    {
                        TempData["Success"] = result.Message;
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        ModelState.AddModelError("", result.Message);
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", $"Error updating employee: {ex.Message}");
                }
            }
            return View(employee);
        }

        // GET: Employee/Delete/5
        public IActionResult Delete(int id)
        {
            try
            {
                Employee? employee = _employeeRepository.GetEmployeeById(id);
                if (employee == null)
                {
                    return NotFound();
                }
                return View(employee);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error retrieving employee for deletion: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            try
            {
                CRUDResult result = _employeeRepository.DeleteEmployee(id);
                if (result.Success)
                {
                    TempData["Success"] = result.Message;
                }
                else
                {
                    TempData["Error"] = result.Message;
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error deleting employee: {ex.Message}";
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Employee/Search
        public IActionResult Search(string searchTerm)
        {
            try
            {
                List<Employee> employees;
                if (string.IsNullOrEmpty(searchTerm))
                {
                    employees = _employeeRepository.GetAllEmployees();
                    ViewBag.SearchTerm = "All Employees";
                }
                else
                {
                    employees = _employeeRepository.SearchEmployees(searchTerm);
                    ViewBag.SearchTerm = $"Employees containing '{searchTerm}'";
                }
                return View(employees);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Error searching employees: {ex.Message}";
                return View(new List<Employee>());
            }
        }

        // GET: Employee/TestCRUD
        public IActionResult TestCRUD()
        {
            ViewBag.TestResults = new List<string>();
            return View();
        }

        // POST: Employee/TestInsert
        [HttpPost]
        public IActionResult TestInsert()
        {
            try
            {
                var testEmployee = new Employee
                {
                    EmployeeCode = "TEST001",
                    FirstName = "Test",
                    LastName = "User",
                    Email = "test.user@example.com",
                    PhoneNumber = "1234567890",
                    Department = "IT",
                    Position = "Tester",
                    Salary = 50000.00m,
                    HireDate = DateTime.Today,
                    IsActive = true
                };

                CRUDResult result = _employeeRepository.InsertEmployee(testEmployee);
                ViewBag.TestResults = new List<string>
                {
                    $"Insert Test: {result.Message}",
                    $"Success: {result.Success}",
                    $"Generated ID: {result.GeneratedId}",
                    $"Rows Affected: {result.RowsAffected}"
                };
            }
            catch (Exception ex)
            {
                ViewBag.TestResults = new List<string> { $"Insert Test Failed: {ex.Message}" };
            }

            return View("TestCRUD");
        }

        // POST: Employee/TestUpdate
        [HttpPost]
        public IActionResult TestUpdate()
        {
            try
            {
                // Try to get first employee for testing
                var employees = _employeeRepository.GetAllEmployees();
                if (employees.Any())
                {
                    var employee = employees.First();
                    employee.LastName += " (Updated)";
                    employee.Salary += 1000;

                    CRUDResult result = _employeeRepository.UpdateEmployee(employee);
                    ViewBag.TestResults = new List<string>
                    {
                        $"Update Test: {result.Message}",
                        $"Success: {result.Success}",
                        $"Rows Affected: {result.RowsAffected}"
                    };
                }
                else
                {
                    ViewBag.TestResults = new List<string> { "No employees found for update test" };
                }
            }
            catch (Exception ex)
            {
                ViewBag.TestResults = new List<string> { $"Update Test Failed: {ex.Message}" };
            }

            return View("TestCRUD");
        }

        // POST: Employee/TestDelete
        [HttpPost]
        public IActionResult TestDelete()
        {
            try
            {
                // Try to get first employee for testing
                var employees = _employeeRepository.GetAllEmployees();
                if (employees.Any())
                {
                    var employee = employees.First();
                    CRUDResult result = _employeeRepository.DeleteEmployee(employee.EmployeeID);
                    ViewBag.TestResults = new List<string>
                    {
                        $"Delete Test: {result.Message}",
                        $"Success: {result.Success}",
                        $"Rows Affected: {result.RowsAffected}"
                    };
                }
                else
                {
                    ViewBag.TestResults = new List<string> { "No employees found for delete test" };
                }
            }
            catch (Exception ex)
            {
                ViewBag.TestResults = new List<string> { $"Delete Test Failed: {ex.Message}" };
            }

            return View("TestCRUD");
        }
    }
}