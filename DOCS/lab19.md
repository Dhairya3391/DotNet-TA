# Server-Side Validation in ASP.NET Core

## 1. Description
**Server-side validation** is the process of verifying user input on the server after it has been submitted. It acts as the authoritative check to ensure that the data conforms to the application's rules and constraints before it is processed or stored in a database.

In ASP.NET Core, this is typically done within a controller action by checking the `ModelState.IsValid` property. The `ModelState` object automatically gets populated with validation errors based on the **Data Annotations** applied to your model.

## 2. Why It Is Important
Server-side validation is **absolutely essential** for the security and integrity of your application. While client-side validation (using JavaScript) provides a good user experience by giving immediate feedback, it can be easily bypassed by a malicious user or simply fail if JavaScript is disabled.

Server-side validation is your application's last line of defense. It ensures that:
- **Data Integrity is Maintained:** No invalid or corrupt data is saved to your database.
- **Your Application is Secure:** It protects against various forms of attacks, such as mass assignment or over-posting, where a user might try to submit data for fields they are not supposed to modify.
- **Business Rules are Enforced:** It guarantees that all data adheres to your business logic, regardless of what happens on the client side.

**Rule of thumb: Always perform server-side validation, even if you also have client-side validation.**

## 3. Real-World Examples
- A user registration form: The server checks if the submitted email address is already in use, a check that can only be done on the server.
- An e-commerce checkout: The server validates that the price of the items in the cart hasn't been tampered with on the client side and that the product is still in stock.
- Creating a blog post: The server ensures that the `PostId` is not being set by the user, protecting against an over-posting attack.

## 4. Syntax & Explanation

Server-side validation is a seamless process in ASP.NET Core MVC that involves the model, the view, and the controller.

### 1. The Model (with Data Annotations)
First, you define your validation rules on your model using data annotations.

```csharp
public class ProductViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Product name is required.")]
    [StringLength(50)]
    public string Name { get; set; }

    [Range(0.01, 10000.00, ErrorMessage = "Price must be between 0.01 and 10,000.")]
    public decimal Price { get; set; }
}
```

### 2. The View
The view uses Tag Helpers to display the form and the validation messages.

```cshtml
@model ProductViewModel

<form asp-action="Create" method="post">
    @* This displays a summary of all validation errors *@
    <div asp-validation-summary="ModelOnly" class="text-danger"></div>

    <div class="form-group">
        <label asp-for="Name"></label>
        <input asp-for="Name" class="form-control" />
        @* This displays the error for the 'Name' property specifically *@
        <span asp-validation-for="Name" class="text-danger"></span>
    </div>

    <div class="form-group">
        <label asp-for="Price"></label>
        <input asp-for="Price" class="form-control" />
        <span asp-validation-for="Price" class="text-danger"></span>
    </div>

    <button type="submit" class="btn btn-primary">Save</button>
</form>
```

### 3. The Controller (The Core of Server-Side Validation)
When the form is submitted, the controller action checks `ModelState.IsValid`.

```csharp
public class ProductsController : Controller
{
    // GET action to display the form
    public IActionResult Create()
    {
        return View();
    }

    // POST action to handle the form submission
    [HttpPost]
    [ValidateAntiForgeryToken] // Important for security
    public IActionResult Create(ProductViewModel model)
    {
        // The framework automatically validates the incoming model
        // and populates ModelState with any errors.
        
        if (ModelState.IsValid)
        {
            // If the data is valid, proceed with the business logic.
            // e.g., save the product to the database.
            
            // ... save logic ...
            
            TempData["SuccessMessage"] = "Product created successfully!";
            return RedirectToAction("Index");
        }
        else
        {
            // If the data is NOT valid, return the view.
            // The model is passed back to the view so that the user's input
            // is not lost, and the validation messages (from ModelState)
            // will be displayed by the Tag Helpers.
            return View(model);
        }
    }
}
```

### Custom Validation
You can also add custom validation logic directly to your controller action if a rule is too complex for a data annotation.

```csharp
[HttpPost]
public IActionResult Create(ProductViewModel model)
{
    // Example: A custom rule that cannot be a simple annotation
    if (model.Name.Contains("test"))
    {
        ModelState.AddModelError("Name", "The word 'test' is not allowed in a product name.");
    }

    if (ModelState.IsValid)
    {
        // ... proceed ...
    }

    return View(model);
}
```

## 5. Mini Practice Task
1.  Create a simple `EventViewModel` with properties for `EventName` (string), `Date` (DateTime), and `Capacity` (int).
2.  Add data annotations to enforce the following rules:
    -   `EventName` is required.
    -   `Capacity` must be between 10 and 500.
3.  Create a `Create` view with a form for this model.
4.  In your controller's `[HttpPost] Create` action, add a server-side check to ensure the `Date` of the event is in the future. If it's not, use `ModelState.AddModelError()` to add a custom error.
5.  If `ModelState.IsValid` is `true` after your custom check, redirect to a success page. Otherwise, return the view with the model to display the errors.
6.  Test your form by submitting both valid and invalid data (e.g., an empty name, a capacity of 5, a date in the past).