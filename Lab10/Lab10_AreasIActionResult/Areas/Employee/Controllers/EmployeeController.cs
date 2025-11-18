using Microsoft.AspNetCore.Mvc;

namespace Lab10_AreasIActionResult.Areas.Employee.Controllers
{
    [Area("Employee")]
    public class EmployeeController : Controller
    {
        // View Result
        public IActionResult Index()
        {
            ViewBag.Area = "Employee";
            ViewBag.Message = "Welcome to Employee Area";
            ViewBag.EmployeeName = "John Doe";
            ViewBag.EmployeeId = "EMP001";
            ViewBag.Department = "Development";
            return View();
        }

        // Content Result with Plain Text
        public IActionResult ContentDemo()
        {
            string textContent = $"EMPLOYEE DASHBOARD\n" +
                               $"==================\n" +
                               $"Name: John Doe\n" +
                               $"ID: EMP001\n" +
                               $"Department: Development\n" +
                               $"Generated: {DateTime.Now}\n" +
                               $"Status: Active\n" +
                               $"Today's Tasks: 5\n" +
                               $"Completed: 3\n" +
                               $"Pending: 2";
            return Content(textContent, "text/plain");
        }

        // JSON Result with Employee Data
        public IActionResult JsonDemo()
        {
            var employeeData = new
            {
                Area = "Employee",
                Employee = new
                {
                    Id = "EMP001",
                    Name = "John Doe",
                    Department = "Development",
                    Position = "Senior Developer",
                    Email = "john.doe@company.com",
                    JoinDate = new DateTime(2020, 05, 15)
                },
                Tasks = new[]
                {
                    new { Id = 1, Title = "Complete feature development", Status = "In Progress", Priority = "High" },
                    new { Id = 2, Title = "Code review", Status = "Completed", Priority = "Medium" },
                    new { Id = 3, Title = "Update documentation", Status = "Pending", Priority = "Low" }
                },
                Attendance = new
                {
                    DaysPresent = 22,
                    DaysAbsent = 2,
                    LateArrivals = 1,
                    OvertimeHours = 8.5
                },
                Performance = new
                {
                    Rating = 4.2,
                    GoalsAchieved = 85,
                    TeamCollaboration = 90,
                    InnovationScore = 78
                }
            };
            return Json(employeeData);
        }

        // File Result (PDF - simulated)
        public IActionResult FileDemo()
        {
            string pdfContent = "%PDF-1.4\n1 0 obj\n<<\n/Type /Catalog\n/Pages 2 0 R\n>>\nendobj\n" +
                               "2 0 obj\n<<\n/Type /Pages\n/Kids [3 0 R]\n/Count 1\n>>\nendobj\n" +
                               "3 0 obj\n<<\n/Type /Page\n/Parent 2 0 R\n/MediaBox [0 0 612 792]\n" +
                               "/Contents 4 0 R\n>>\nendobj\n" +
                               "4 0 obj\n<<\n/Length 44\n>>\nstream\n" +
                               "BT\n/F1 12 Tf\n100 700 Td\n(Employee Pay Slip) Tj\nET\n" +
                               "endstream\nendobj\n" +
                               "xref\n0 5\n0000000000 65535 f\n0000000009 00000 n\n" +
                               "0000000058 00000 n\n0000000115 00000 n\n0000000274 00000 n\n" +
                               "trailer\n<<\n/Size 5\n/Root 1 0 R\n>>\nstartxref\n374\n%%EOF";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(pdfContent);
            return File(byteArray, "application/pdf", "EmployeePaySlip.pdf");
        }

        // Redirect within same area
        public IActionResult RedirectDemo()
        {
            TempData["Message"] = "Redirected within Employee area!";
            return RedirectToAction("JsonDemo");
        }

        // Status Code Results
        public IActionResult StatusCodeDemo()
        {
            return StatusCode(200, "Employee operation successful");
        }

        public IActionResult NotFoundDemo()
        {
            return NotFound("Employee record not found");
        }

        public IActionResult PaymentRequiredDemo()
        {
            return StatusCode(402, "Payment required for premium employee features");
        }
    }
}