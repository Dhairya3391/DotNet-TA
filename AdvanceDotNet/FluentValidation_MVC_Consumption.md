# Consuming FluentValidation API in MVC

## Overview

This guide explains how to consume an ASP.NET Core API that uses FluentValidation from an MVC client. Specifically, it covers handling custom API error responses (`success: false`) and displaying them in the View using `ModelState`.

---

## 1. Understand the API Response

The API uses a **custom wrapper** for responses. When a validation error occurs (HTTP 400), the response follows this structure:

**Example JSON Response (400 Bad Request):**
```json
{
  "success": false,
  "message": "Validation Error",
  "data": null,
  "errors": [
    "The DepartmentName field is required.",
    "Department name is required"
  ]
}
```

> ⚠️ **Note:** The `errors` array is a simple list of strings. It does not map errors to specific property names (like `DepartmentName`). Therefore, we will display these errors globally in the MVC View.

---

## 2. MVC Controller Logic

In your Controller, you must:
1.  **Call the API** using `HttpClient`.
2.  **Deserialize** the response into a helper class.
3.  **Check `Success`**.
4.  **Map Errors** to `ModelState` if the call failed.

### Response Wrapper Class

First, define a generic class to match the API structure:

```csharp
public class ApiResponse<T>
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public List<string> Errors { get; set; }
}
```

### Controller Action Example

```csharp
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text;

public class DepartmentController : Controller
{
    private readonly HttpClient _httpClient;

    // Constructor: Get HttpClient from factory
    public DepartmentController(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("MyApi");
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(DepartmentViewModel model)
    {
        // STEP 1: Convert your form data to JSON
        var json = JsonSerializer.Serialize(model);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        // STEP 2: Send data to the API
        var response = await _httpClient.PostAsync("api/Department/Create", content);

        // STEP 3: Read what the API sent back
        var responseBody = await response.Content.ReadAsStringAsync();

        // STEP 4: Convert JSON string to C# object
        var result = JsonSerializer.Deserialize<ApiResponse<object>>(responseBody, 
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        // STEP 5: Check if API said "success = true"
        if (result != null && result.Success)
        {
            return RedirectToAction("Index"); // All good! Go to list page.
        }

        // STEP 6: If failed, show errors in the View
        if (result?.Errors != null)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error); // "" means show in summary
            }
        }

        return View(model); // Stay on same page with errors
    }
}
```

> 💡 **Think of it like this:**
> 1. Pack your data into a box (JSON)
> 2. Send the box to the API
> 3. Open the box the API sends back
> 4. Check if the API said "success" or "error"
> 5. If error, show the messages on screen

---

## 3. Displaying Errors in MVC View

Since the API returns a flat list of errors (without field names), use the **Validation Summary** to display them at the top of the form.

### View Example (`Create.cshtml`)

```html
@model DepartmentViewModel

<h2>Create Department</h2>

<!-- CRITICAL: API errors will appear here -->
<div asp-validation-summary="All" class="alert alert-danger"></div>

<form asp-action="Create" method="post">
    
    <div class="form-group">
        <label asp-for="DepartmentName"></label>
        <input asp-for="DepartmentName" class="form-control" />
        
        <!-- This handles client-side jQuery validation -->
        <span asp-validation-for="DepartmentName" class="text-danger"></span>
    </div>

    <button type="submit" class="btn btn-primary">Submit</button>
</form>
```

---

## Summary

| Component | Responsibility |
| :--- | :--- |
| **API** | Returns `success: false` and `errors: ["Error 1", "Error 2"]`. |
| **Controller** | Deserializes JSON, loops through `errors`, calls `ModelState.AddModelError(string.Empty, error)`. |
| **View** | Uses `<div asp-validation-summary="All">` to show the list of errors. |
