using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Lab09_RazorSyntax.Models;

namespace Lab09_RazorSyntax.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    // Lab 9: Razor Syntax - Table of 5
    public IActionResult TableOfFive()
    {
        return View();
    }

    // Lab 9: Razor Syntax - Student SPI Table
    public IActionResult StudentSPI()
    {
        return View();
    }

    // Lab 9: ViewBag demonstration
    public IActionResult ViewBagDemo()
    {
        ViewBag.Message = "This is a ViewBag message";
        ViewBag.CurrentTime = DateTime.Now;
        ViewBag.Student = new { Name = "John Doe", Age = 21, Grade = "A" };
        return View();
    }

    // Lab 9: ViewData demonstration
    public IActionResult ViewDataDemo()
    {
        ViewData["Message"] = "This is a ViewData message";
        ViewData["CurrentTime"] = DateTime.Now;
        ViewData["Student"] = new { Name = "Jane Smith", Age = 22, Grade = "B+" };
        return View();
    }

    // Lab 9: TempData demonstration
    public IActionResult SetTempData()
    {
        TempData["Message"] = "This message persists across redirects";
        TempData["UserName"] = "Alice";
        return RedirectToAction("GetTempData");
    }

    public IActionResult GetTempData()
    {
        return View();
    }

    [HttpPost]
    public IActionResult RefreshPage()
    {
        return View("GetTempData");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
