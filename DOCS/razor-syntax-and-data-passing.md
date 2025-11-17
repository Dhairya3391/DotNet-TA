# Razor Syntax Overview & Data Passing Techniques

## 1. Description
Razor is the view engine used by ASP.NET Core MVC to combine C# code with HTML. It uses the `@` symbol to switch into C# and provides concise syntax for expressions, loops, conditionals, and layouts. Data passing techniques are ways to send data from controllers to views: strongly-typed models, `ViewData`, `ViewBag`, and `TempData`.

## 2. Why It Is Important
Razor keeps server-side logic readable and compact inside HTML views, enabling dynamic pages. Knowing data-passing techniques lets you choose the right scope and lifetime for values (request-only vs next-request, typed vs untyped).

## 3. Real-World Examples
- Render a product list by passing a `List<Product>` model to the view.
- Use `TempData` to show a success message after redirecting following a POST.
- Use `ViewData`/`ViewBag` for small UI flags (e.g., `IsAdmin`) that do not require a full model.

## 4. Syntax & Explanation
Razor basics (in a `.cshtml` view):

```cshtml
@model List<MyApp.Models.Product>
@{
    ViewData["Title"] = "Products";
}
<h1>@ViewData["Title"]</h1>
<ul>
@foreach (var p in Model)
{
    <li>@p.Name - @p.Price.ToString("C")</li>
}
</ul>
```

Controller passing a model and TempData:

```csharp
using Microsoft.AspNetCore.Mvc;
using MyApp.Models;
using System.Collections.Generic;

public class ProductsController : Controller
{
    public IActionResult Index()
    {
        var products = new List<Product> {
            new Product { Id = 1, Name = "Pen", Price = 1.20m }
        };
        return View(products); // strongly-typed model
    }

    [HttpPost]
    public IActionResult Create(Product p)
    {
        // save logic...
        TempData["Success"] = "Product created"; // survives redirect
        return RedirectToAction("Index");
    }
}
```

Accessing `TempData` in the redirected view:

```cshtml
@if (TempData["Success"] != null)
{
    <div class="alert alert-success">@TempData["Success"]</div>
}
```

Notes:
- `Model` is strongly typed when view declares `@model`.
- `ViewData` is a dictionary: `ViewData["key"]` (object typed).
- `ViewBag` is dynamic: `ViewBag.Message = "hi"`.
- `TempData` persists until it's read or the session ends (good for post-redirect-get messages).

## 5. Use Cases
- Sending page data (lists, single objects) via model binding.
- Showing one-time messages after redirects using `TempData`.
- Small UI flags or helper values via `ViewData`/`ViewBag`.

## 6. Mini Practice Task
1. Create an `Index` view that lists `Product` models and shows a `TempData` success message after a redirect.
2. Use `ViewBag` to pass a `bool IsAdmin` to the view and render admin-only markup conditionally.
