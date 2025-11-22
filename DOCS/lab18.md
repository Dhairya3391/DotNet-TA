# Model Creation & Data Annotations

## 1. Description
In ASP.NET Core MVC, a **Model** is a C# class that represents the data of your application. Models are the part of the MVC pattern responsible for handling the application's data and business logic. They can represent domain objects (like a `Product` or `Customer`), or they can be tailored to the specific needs of a view, in which case they are called **View Models**.

**Data Annotations** are attributes that you can apply to the properties of your model classes. These attributes provide metadata about the data, which the framework can use for validation, display formatting, and database schema generation.

## 2. Why It Is Important
- **Structure and Organization:** Models provide a structured way to organize and manage your application's data.
- **Validation:** Data annotations are the cornerstone of validation in ASP.NET Core. They allow you to define your validation rules (e.g., a field is required, a string must have a certain length) in one place—the model itself. This single source of truth is then used for both server-side and client-side validation.
- **User Experience:** Data annotations help create a better user experience by providing clear, user-friendly labels for form fields and ensuring that data is formatted correctly.
- **Database Design:** When using an Object-Relational Mapper (ORM) like Entity Framework Core, data annotations can influence how the database schema is generated.

## 3. Real-World Examples
- A `RegisterViewModel` used for a user registration form, with data annotations like `[Required]`, `[EmailAddress]`, and `[StringLength]` to ensure the user provides valid input.
- A `Product` model with a `[Display(Name = "Product Code")]` annotation to show a more user-friendly label on the UI.
- A `BlogPost` model with a `[DataType(DataType.Date)]` annotation on a `PublishedDate` property to ensure it's displayed and edited as a date, not a date and time.

## 4. Syntax & Explanation

Here is an example of a `UserViewModel` that demonstrates several common data annotations. This class would typically be used as the model for a user creation or editing form.

**`ViewModels/UserViewModel.cs`**
```csharp
// You need to add this using statement for the data annotations
using System.ComponentModel.DataAnnotations;

public class UserViewModel
{
    // The user's ID, often hidden on a create form but used for edits.
    public int Id { get; set; }

    // --- Validation Annotations ---

    [Required(ErrorMessage = "Please enter the user's full name.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Full name must be between 2 and 100 characters.")]
    public string FullName { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
    public string Email { get; set; }
    
    [Required]
    [DataType(DataType.Password)]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "The password must be at least 8 characters long.")]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [Display(Name = "Confirm Password")]
    public string ConfirmPassword { get; set; }

    [Range(0, 150, ErrorMessage = "Age must be between 0 and 150.")]
    public int Age { get; set; }

    // --- Display and Formatting Annotations ---

    [Display(Name = "Birth Date")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
    public DateTime? BirthDate { get; set; }
}
```

### Common Data Annotations

| Category       | Annotation                | Description                                                                 |
| -------------- | ------------------------- | --------------------------------------------------------------------------- |
| **Validation** | `[Required]`              | Specifies that a property must have a value.                                |
|                | `[StringLength(max, Min=min)]` | Specifies the maximum and optionally minimum length for a string property.  |
|                | `[Range(min, max)]`         | Specifies the minimum and maximum value for a numeric property.               |
|                | `[RegularExpression(pattern)]` | Validates that the property value matches a specified regular expression. |
|                | `[EmailAddress]`          | Validates that the property has the format of an email address.             |
|                | `[Phone]`                 | Validates that the property has the format of a phone number.               |
|                | `[Compare(otherProperty)]`  | Compares two properties of a model (e.g., for password confirmation).       |
| **Display**    | `[Display(Name="...")]`     | Specifies the text to use for labels and headers in the UI.                 |
|                | `[DisplayFormat(...)]`      | Specifies how a property value should be formatted for display.             |
|                | `[DataType(type)]`          | Specifies the data type of the property (e.g., `Date`, `Password`, `EmailAddress`). This helps the view engine render the appropriate HTML5 input type. |
|                | `[ScaffoldColumn(false)]`   | Specifies that a property should be excluded from UI scaffolding.         |

## 5. Mini Practice Task
1.  Create a new C# class named `MovieViewModel`.
2.  Add the following properties to the class:
    -   `Title` (string)
    -   `ReleaseDate` (DateTime)
    -   `Genre` (string)
    -   `Price` (decimal)
    -   `Rating` (string, e.g., "G", "PG", "PG-13", "R")
3.  Apply appropriate data annotations to enforce the following rules:
    -   `Title` is required and must be between 3 and 60 characters long.
    -   `ReleaseDate` is required and should be displayed as a date only.
    -   `Genre` is required.
    -   `Price` is required and must be between 0.01 and 100.00. Use a display format to show it as currency.
    -   `Rating` is required.
4.  Create a simple Razor view that uses this model and a form to see how the data annotations affect the generated labels and input types.