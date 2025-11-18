# Working with Areas & `IActionResult`

## 1. Description
Areas are a way to partition an ASP.NET Core MVC app into logical sections (e.g., `Admin`, `Shop`) each with its own controllers, views, and routes. `IActionResult` is the common return type for controller actions; it represents various result types like `ViewResult`, `RedirectResult`, `JsonResult`, etc.

## 2. Why It Is Important
Areas help organize large apps and group related functionality. Returning `IActionResult` gives flexibility to return different result types from the same action (view, redirect, file, JSON), which is useful for API endpoints and UI controllers.

## 3. Real-World Examples
- An `Admin` area containing dashboard controllers and views segregated from public site code.
- Actions that sometimes return a view or sometimes return `NotFound()` for missing resources.

## 4. Syntax & Explanation
Area folder structure (convention):

- Areas/
  - Admin/
    - Controllers/
    - Views/
    - AdminAreaRegistration (not required in Core)

Controller in an area:

```csharp
using Microsoft.AspNetCore.Mvc;

[Area("Admin")]
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View(); // returns ViewResult
    }

    public IActionResult Details(int id)
    {
        var model = GetModel(id);
        if (model == null) return NotFound(); // returns NotFoundResult
        return View(model);
    }

    [HttpPost]
    public IActionResult Save(MyModel m)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);
        // save
        return RedirectToAction("Index"); // RedirectToActionResult
    }
}
```

Routing to an area (by default): `/Admin/Dashboard/Index`

Common `IActionResult` types:
- `View()` / `View(model)` → render a view.
- `RedirectToAction(...)` → redirect.
- `Json(object)` → JSON result.
- `File(...)` → serve a file.
- `NotFound()` / `BadRequest()` / `Ok()` → status results.

## 5. Use Cases
- Separate admin functionality from public website code.
- Return different HTTP responses based on validation or resource state.
- Serve files, JSON, or views from the same controller depending on context.

## 6. Mini Practice Task
1. Create an `Admin` area with a `UsersController` and an `Index` view that lists users.
2. Implement an action that returns `NotFound()` when an ID is missing and `View(model)` otherwise.
