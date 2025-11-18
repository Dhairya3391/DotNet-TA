using Microsoft.AspNetCore.Mvc;

namespace Lab10_AreasIActionResult.Areas.Manager.Controllers
{
    [Area("Manager")]
    public class ManagerController : Controller
    {
        // View Result
        public IActionResult Index()
        {
            ViewBag.Area = "Manager";
            ViewBag.Message = "Welcome to Manager Area";
            ViewBag.TeamSize = 12;
            ViewBag.Department = "Operations";
            return View();
        }

        // Content Result with XML
        public IActionResult ContentDemo()
        {
            string xmlContent = @"<?xml version='1.0' encoding='UTF-8'?>
                <manager>
                    <area>Manager</area>
                    <department>Operations</description>
                    <team_size>12</team_size>
                    <generated>" + DateTime.Now.ToString() + @"</generated>
                    <responsibilities>
                        <task>Team Management</task>
                        <task>Project Oversight</task>
                        <task>Performance Reviews</task>
                    </responsibilities>
                </manager>";
            return Content(xmlContent, "application/xml");
        }

        // JSON Result with Manager Data
        public IActionResult JsonDemo()
        {
            var managerData = new
            {
                Area = "Manager",
                Department = "Operations",
                TeamMembers = new[]
                {
                    new { Id = 1, Name = "Employee 1", Position = "Developer", Status = "Active" },
                    new { Id = 2, Name = "Employee 2", Position = "Designer", Status = "Active" },
                    new { Id = 3, Name = "Employee 3", Position = "Tester", Status = "On Leave" }
                },
                Projects = new[]
                {
                    new { Name = "Project Alpha", Status = "In Progress", Completion = 75 },
                    new { Name = "Project Beta", Status = "Planning", Completion = 20 },
                    new { Name = "Project Gamma", Status = "Completed", Completion = 100 }
                },
                Metrics = new
                {
                    TeamProductivity = 87,
                    OnTimeDelivery = 92,
                    CustomerSatisfaction = 4.5
                }
            };
            return Json(managerData);
        }

        // File Result (CSV)
        public IActionResult FileDemo()
        {
            string csvContent = "Name,Position,Status,Performance\n" +
                               "Employee 1,Developer,Active,Excellent\n" +
                               "Employee 2,Designer,Active,Good\n" +
                               "Employee 3,Tester,On Leave,Good\n" +
                               "Employee 4,Developer,Active,Excellent\n" +
                               "Employee 5,Analyst,Active,Very Good";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(csvContent);
            return File(byteArray, "text/csv", "TeamReport.csv");
        }

        // Redirect to another area
        public IActionResult RedirectDemo()
        {
            TempData["Message"] = "Manager redirected to Employee area!";
            return RedirectToAction("Index", "Employee", new { area = "Employee" });
        }

        // Status Code Results
        public IActionResult StatusCodeDemo()
        {
            return StatusCode(202, "Manager request accepted for processing");
        }

        public IActionResult NotFoundDemo()
        {
            return NotFound("Manager resource not found");
        }

        public IActionResult ForbiddenDemo()
        {
            return StatusCode(403, "Manager access to this resource is forbidden");
        }
    }
}