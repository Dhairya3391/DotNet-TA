---
title: Lab 11A - Working with Form Collections
sidebar_position: 11.1
---

# Working with Form Collections

## 1. Description
When an HTML form is submitted to an ASP.NET Core controller, the data from the form's input fields is sent in the body of the HTTP request. **Form Collection** refers to the set of key-value pairs that represent this data.

ASP.NET Core's **Model Binding** system is the primary mechanism for processing this data. Model binding automatically maps the form data to the parameters of your controller action methods. While the most common approach is to bind to a strongly-typed model object (a view model), there are times when you might need to access the raw form data more directly.

## 2. Why It Is Important
Understanding how to access form data is fundamental to handling user input. While strongly-typed model binding should be your default approach, knowing how to access the raw form collection is useful for:
- **Handling Dynamic Forms:** When the form fields are not known at compile time (e.g., a dynamically generated survey).
- **Debugging:** Inspecting the raw `IFormCollection` can be a useful way to debug model binding issues.
- **Reading Non-Model Data:** Accessing a single value from the form without needing to create a full view model for it.

## 3. Real-World Examples
- A "form builder" application where users can create their own forms with custom fields. The server-side code wouldn't have a pre-defined model and would need to loop through the form collection to save the submitted data.
- Handling a form that includes a dynamic set of checkboxes, where you need to iterate through the keys to see which ones were checked.
- A quick prototype where you need to process a simple form post without the overhead of creating a dedicated view model class.

## 4. Syntax & Explanation

There are several ways to access form data in a controller action.

### 1. Binding to a ViewModel (Preferred Method)
This is the standard, strongly-typed, and most secure method. The model binder maps form fields to the properties of your view model class.

**ViewModel:**
```csharp
public class ProductViewModel
{
    public string Name { get; set; }
    public decimal Price { get; set; }
}
```

**Controller Action:**
```csharp
[HttpPost]
public IActionResult Create(ProductViewModel model)
{
    // The 'model' object is automatically populated with data from the form.
    // e.g., model.Name will have the value from the <input name="Name"> field.
    if (ModelState.IsValid)
    {
        // ... process data ...
    }
    return View(model);
}
```

### 2. Binding to Individual Parameters
For simple forms, you can bind directly to primitive type parameters. The parameter names must match the `name` attributes of the form's input fields.

**Controller Action:**
```csharp
[HttpPost]
public IActionResult Create(string name, decimal price)
{
    // The 'name' and 'price' variables are populated from the form.
    // ... process data ...
    return RedirectToAction("Index");
}
```

### 3. Using `IFormCollection`
To access the raw collection of form data, you can have the model binder provide an `IFormCollection` object. This gives you a dictionary-like object where the keys are the `name` attributes of the input fields.

**Controller Action:**
```csharp
using Microsoft.AspNetCore.Http;

[HttpPost]
public IActionResult Create(IFormCollection form)
{
    // Access values by their key (the 'name' attribute of the input)
    string name = form["Name"];
    string priceString = form["Price"];

    // Note: All values from IFormCollection are strings.
    // You are responsible for parsing and converting them to the correct types.
    if (decimal.TryParse(priceString, out decimal price))
    {
        // ... process data ...
    }

    return RedirectToAction("Index");
}
```
**Warning:** Using `IFormCollection` bypasses the strongly-typed validation and type conversion provided by the model binding system. You must handle type casting, validation, and potential errors manually, which can be more error-prone.

## 5. Mini Practice Task
1. Create a simple HTML form in a Razor view with two text inputs: one with `name="FirstName"` and one with `name="LastName"`.
2. Create a `[HttpPost]` controller action that accepts an `IFormCollection` as its parameter.
3. Inside the action, retrieve the values for "FirstName" and "LastName" from the `IFormCollection`.
4. Combine them into a full name and store it in `TempData`.
5. Redirect to another page that displays the full name from `TempData`.
6. Test your form by submitting some names.
