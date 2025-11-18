using Microsoft.AspNetCore.Mvc;

namespace Lab10_AreasIActionResult.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AdminController : Controller
    {
        // IActionResult Types Demonstrations

        // View Result
        public IActionResult Index()
        {
            ViewBag.Area = "Admin";
            ViewBag.Message = "Welcome to Admin Area";
            return View();
        }

        // Content Result
        public IActionResult ContentDemo()
        {
            string htmlContent = @"
                <div style='padding: 20px; border: 2px solid #007bff; border-radius: 5px;'>
                    <h3 style='color: #007bff;'>Content Result Demo</h3>
                    <p>This is raw HTML content returned as ContentResult.</p>
                    <p><strong>Area:</strong> Admin</p>
                    <p><strong>Time:</strong> " + DateTime.Now.ToString() + @"</p>
                </div>";
            return Content(htmlContent, "text/html");
        }

        // JSON Result
        public IActionResult JsonDemo()
        {
            var adminData = new
            {
                Area = "Admin",
                Administrators = new[]
                {
                    new { Id = 1, Name = "Admin User 1", Role = "Super Admin", Email = "admin1@example.com" },
                    new { Id = 2, Name = "Admin User 2", Role = "System Admin", Email = "admin2@example.com" },
                    new { Id = 3, Name = "Admin User 3", Role = "Content Admin", Email = "admin3@example.com" }
                },
                Permissions = new[] { "Create", "Read", "Update", "Delete", "Manage Users" },
                SystemInfo = new
                {
                    Version = "1.0.0",
                    LastUpdated = DateTime.Today,
                    Status = "Active"
                }
            };
            return Json(adminData);
        }

        // File Result (Text File)
        public IActionResult FileDemo()
        {
            string content = $"Admin Area Report\nGenerated: {DateTime.Now}\n\nAdministrators:\n1. Admin User 1 - Super Admin\n2. Admin User 2 - System Admin\n3. Admin User 3 - Content Admin\n\nPermissions: Create, Read, Update, Delete, Manage Users";
            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(content);
            return File(byteArray, "text/plain", "AdminReport.txt");
        }

        // Redirect Result
        public IActionResult RedirectDemo()
        {
            TempData["Message"] = "Redirected from Admin area!";
            return RedirectToAction("Index", "Home", new { area = "" });
        }

        // Redirect to External URL
        public IActionResult ExternalRedirectDemo()
        {
            return Redirect("https://learn.microsoft.com/aspnet/core");
        }

        // Status Code Results
        public IActionResult StatusCodeDemo()
        {
            return StatusCode(200, "Admin operation completed successfully");
        }

        public IActionResult NotFoundDemo()
        {
            return NotFound("Admin resource not found");
        }

        public IActionResult UnauthorizedDemo()
        {
            return Unauthorized("Admin access required");
        }

        public IActionResult BadRequestDemo()
        {
            return BadRequest("Invalid admin request parameters");
        }
    }
}