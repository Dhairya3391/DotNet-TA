---
title: Lab 10A - ASP.NET Core MVC Areas
sidebar_position: 10.1
---

# ASP.NET Core MVC Areas

## 1. Description
**Areas** are a feature in ASP.NET Core MVC used to partition a large web application into smaller, functional groupings. Each area has its own separate MVC structure, including its own `Controllers`, `Views`, and `Models` folders. This helps to organize different logical sections of your application.

For example, in a large e-commerce application, you might have separate areas for "Products", "Orders", and "Admin".

## 2. Why It Is Important
As an application grows in complexity, it can become difficult to manage all the controllers, views, and models in the default MVC folder structure. Areas provide a way to:
- **Organize Code:** Group related functionality together, making the project structure cleaner and more intuitive.
- **Reduce Complexity:** Break down a large application into smaller, more manageable pieces.
- **Improve Maintainability:** It's easier to find and work on the code for a specific feature when it's all located within its own area.
- **Enable Team Collaboration:** Different teams can work on different areas of the application with less risk of interfering with each other's work.

## 3. Real-World Examples
- An **Admin** area for site administrators to manage users, content, and settings, completely separate from the public-facing part of the site.
- A **Blog** area within a larger corporate website.
- A **Support** or **Helpdesk** section of an application with its own set of controllers and views.
- In a university portal, you might have separate areas for "Students", "Faculty", and "Admissions".

## 4. Syntax & Explanation

### Folder Structure
By convention, areas are created in a top-level `Areas` folder in your project.

```
/
|-- Areas/
|   |-- Admin/
|   |   |-- Controllers/
|   |   |   |-- DashboardController.cs
|   |   |-- Views/
|   |   |   |-- Dashboard/
|   |   |   |   |-- Index.cshtml
|   |   |   |-- _ViewStart.cshtml
|   |   |-- Models/ (optional)
|-- Controllers/
|-- Views/
|-- wwwroot/
```

### Area Controller
A controller within an area must be decorated with the `[Area]` attribute to associate it with the correct area.

**`Areas/Admin/Controllers/DashboardController.cs`**
```csharp
using Microsoft.AspNetCore.Mvc;

[Area("Admin")] // This attribute is crucial
public class DashboardController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}
```

### Routing
ASP.NET Core's default routing needs to be updated to include areas. This is typically done in `Program.cs`.

**`Program.cs`**
```csharp
// ... other services

app.MapControllerRoute(
    name: "MyArea",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

// ...
```
With this setup, a request to `/Admin/Dashboard/Index` would be routed to the `Index` action of the `DashboardController` in the `Admin` area.

### Generating Links
To generate a link to an action within an area, you must specify the `area` in your tag helpers or URL helpers.

**In a Razor View (`.cshtml`)**
```cshtml
@* Link to the Admin Dashboard's Index action *@
<a asp-area="Admin" asp-controller="Dashboard" asp-action="Index">Admin Dashboard</a>
```

## 5. Use Cases
- **Large-scale Applications:** Any application with multiple distinct functional areas that would benefit from being separated.
- **Multi-tenant Applications:** Where different tenants might have different UI or functionality.
- **Modular Applications:** Where you want to develop features as self-contained modules.

## 6. Mini Practice Task
1. Create a new ASP.NET Core MVC project.
2. Add a new area to the project called "Management".
3. Inside the "Management" area, create a `SettingsController` with an `Index` action.
4. Create a corresponding `Index.cshtml` view for the `SettingsController`.
5. Update your routing in `Program.cs` to support areas.
6. Add a link in your main `_Layout.cshtml` file that navigates to the `Index` action of the `SettingsController` in the "Management" area.
7. Run the application and test that you can navigate to your new area.
