using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Lab10_AreasIActionResult.Models;

namespace Lab10_AreasIActionResult.Controllers;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        // Check for TempData messages from area redirects
        if (TempData["Message"] != null)
        {
            ViewBag.RedirectMessage = TempData["Message"];
        }
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
