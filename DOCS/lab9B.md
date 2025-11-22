# Data Passing Techniques in ASP.NET Core MVC

## 1. Description
In ASP.NET Core MVC, **data passing** refers to the techniques used to send data from a controller action method to its corresponding view. There are several ways to do this, each with its own use case.

The primary methods are:
- **Strongly-Typed Models:** Passing a specific model object to the view. This is the most common and recommended approach.
- **`ViewData`:** A dictionary-like object that lets you pass data using string keys.
- **`ViewBag`:** A dynamic object that is a wrapper around `ViewData`. It allows you to create dynamic properties.
- **`TempData`:** A dictionary-like object similar to `ViewData`, but it persists data for one additional request. It's mainly used for passing data between redirects.

## 2. Why It Is Important
Choosing the right data-passing technique is important for writing clean, maintainable, and type-safe code.
- **Strongly-typed models** are the best practice because they provide compile-time type checking and IntelliSense support in the view, which helps prevent runtime errors.
- `ViewData` and `ViewBag` are useful for passing small, secondary pieces of data that don't belong in the main model.
- `TempData` is essential for the common "Post-Redirect-Get" pattern, where you need to display a status message (e.g., "Your form was submitted successfully!") after a redirect.

## 3. Real-World Examples
- **Strongly-Typed Model:** A controller action for a product details page would pass a `Product` object to the view.
- **`ViewData`/`ViewBag`:** Passing a page title or a small piece of UI-specific data, like `ViewBag.PageTitle = "Product List";`.
- **`TempData`:** After a user successfully creates a new product and is redirected to the product list page, `TempData` can be used to show a one-time success message: `TempData["SuccessMessage"] = "Product created successfully!";`.

## 4. Syntax & Explanation

### Controller Action (`ProductsController.cs`)
```csharp
using Microsoft.AspNetCore.Mvc;
using YourApp.Models; // Assuming you have a Product model
using System.Collections.Generic;

public class ProductsController : Controller
{
    public IActionResult Index()
    {
        // 1. Strongly-Typed Model
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 1200.00m },
            new Product { Id = 2, Name = "Mouse", Price = 25.00m }
        };

        // 2. ViewData
        ViewData["PageTitle"] = "Our Products";

        // 3. ViewBag
        ViewBag.ItemsInStock = 150;

        // Passing the model to the view
        return View(products);
    }

    [HttpPost]
    public IActionResult Create(Product product)
    {
        // ... logic to save the product ...

        // 4. TempData - for use after a redirect
        TempData["SuccessMessage"] = $"Product '{product.Name}' was successfully created.";
        
        return RedirectToAction("Index");
    }
}
```

### View (`Index.cshtml`)
```cshtml
@* 1. Declare the strongly-typed model *@
@model List<YourApp.Models.Product>

@{
    // 2. Accessing ViewData
    ViewData["Title"] = ViewData["PageTitle"];
    
    // 3. Accessing ViewBag (no casting needed)
    var stockCount = ViewBag.ItemsInStock;
}

<h1>@ViewData["Title"]</h1>
<p>Total items in stock: @stockCount</p>

@* 4. Accessing TempData (check for null) *@
@if (TempData["SuccessMessage"] != null)
{
    <div class="alert alert-success">
        @TempData["SuccessMessage"]
    </div>
}

<h2>Product List</h2>
<table>
    <thead>
        <tr>
            <th>Name</th>
            <th>Price</th>
        </tr>
    </thead>
    <tbody>
        @* Accessing the strongly-typed Model *@
        @foreach (var product in Model)
        {
            <tr>
                <td>@product.Name</td>
                <td>@product.Price.ToString("C")</td>
            </tr>
        }
    </tbody>
</table>

```
### Summary of Techniques

| Technique             | Type                       | Lifetime                  | Casting in View?           | Use Case                                                    |
| --------------------- | -------------------------- | ------------------------- | -------------------------- | ----------------------------------------------------------- |
| **Strongly-Typed Model** | `Model` property (typed)   | Current request           | No                         | Primary data for the view (e.g., a list of products).       |
| **`ViewData`**        | `ViewDataDictionary`       | Current request           | Yes (e.g., `(string)ViewData["Title"]`) | Small amounts of secondary data.                            |
| **`ViewBag`**         | `dynamic`                  | Current request           | No (but no IntelliSense)   | Small amounts of secondary data, more concise syntax.     |
| **`TempData`**        | `ITempDataDictionary`      | Current and next request  | Yes (like `ViewData`)        | Passing data between redirects (e.g., status messages). |


## 5. Mini Practice Task
1. In a controller action, create a `List` of strings (e.g., a list of your favorite hobbies).
2. Pass this list to a view as a **strongly-typed model**.
3. In the same action, use **`ViewBag`** to pass your name to the view.
4. Use **`ViewData`** to pass the current date to the view.
5. In the view, display your name and the date from `ViewBag` and `ViewData`.
6. Use a `foreach` loop to render the list of hobbies from the `Model` in an unordered list (`<ul>`).
