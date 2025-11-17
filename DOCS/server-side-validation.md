# Server-Side Validation

## 1. Description
Server-side validation checks that incoming data meets rules and constraints before processing. In ASP.NET Core, validation is often driven by `DataAnnotations` on models and verified in controllers using `ModelState.IsValid`.

## 2. Why It Is Important
Client-side validation can be bypassed. Server-side validation ensures data integrity, security, and correct business behavior regardless of client capabilities.

## 3. Real-World Examples
- Validate an order's total is non-negative.
- Ensure required registration fields are provided and email is valid.

## 4. Syntax & Explanation
Model with annotations (see `UserViewModel` in earlier file). Controller action performing validation:

```csharp
[HttpPost]
public IActionResult Register(UserViewModel model)
{
    if (!ModelState.IsValid)
    {
        // Return the view with validation errors displayed
        return View(model);
    }

    // Continue processing (save to DB, etc.)
    return RedirectToAction("Success");
}
```

If custom validation is needed, you can:
- Implement `IValidatableObject` on the model and its `Validate` method.
- Create custom `ValidationAttribute` classes.

Example custom validation attribute:

```csharp
public class NotZeroAttribute : ValidationAttribute
{
    public override bool IsValid(object value)
    {
        if (value is int i) return i != 0;
        return true;
    }
}
```

## 5. Use Cases
- Protecting endpoints from invalid or malicious input.
- Enforcing business rules before database writes.
- Returning detailed errors for API clients.

## 6. Mini Practice Task
1. Add server-side validation to a checkout form ensuring `Quantity >= 1` and `Price >= 0`.
2. Implement a custom attribute `[MaxWords(50)]` that limits a description to a number of words.
