---
title: Lab 18 - Model Creation & Data Annotations
sidebar_position: 18
---
# Model Creation & Data Annotations

## 1. Description
Models represent data used by the application (domain models, view models). Data annotations are attributes on model properties that provide metadata for validation, display names, formats, and binding behavior (e.g., `[Required]`, `[StringLength]`, `[Display(Name = "...")]`).

## 2. Why It Is Important
Data annotations enable server-side validation, client-side validation scaffolding, and consistent UI labels/formatting without writing repetitive validation code.

## 3. Real-World Examples
- A `RegisterViewModel` with `[Required]` for email and password.
- `[Display(Name = "Phone Number")]` to show a friendly label in UI.

## 4. Syntax & Explanation
Example model with annotations:

```csharp
using System.ComponentModel.DataAnnotations;

public class UserViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Display(Name = "Birth Date")]
    [DataType(DataType.Date)]
    public DateTime? BirthDate { get; set; }
}
```

Using the model in a Razor view will wire up validation messages automatically when client validation is enabled.

## 5. Use Cases
- View models for forms and APIs.
- Automatic validation in controllers via `ModelState`.
- Generating UI labels and input formats.

## 6. Mini Practice Task
1. Create a `ProductViewModel` with `[Required] Name`, `[Range(0.01, 10000)] Price`, and `[StringLength(500)] Description`.
2. Use the model in a create form and verify validation messages show for invalid inputs.
