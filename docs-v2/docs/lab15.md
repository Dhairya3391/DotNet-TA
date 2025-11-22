---
title: Lab 15 - HTML Helpers
sidebar_position: 15
---
# HTML Helpers

## 1. Description
HTML Helpers are methods that render HTML elements from Razor views. ASP.NET Core provides tag helpers (preferred) and legacy HTML helper methods (`Html.TextBoxFor`, `Html.DropDownListFor`) for building form inputs and links.

## 2. Why It Is Important
Helpers keep views concise, reduce repetition, and integrate with model metadata (like validation attributes). They also produce correct `name` and `id` attributes needed for model binding.

## 3. Real-World Examples
- Render an input bound to a model property using `Html.TextBoxFor` or `asp-for` tag helper.
- Render validation messages tied to model state.

## 4. Syntax & Explanation
Legacy HTML helper example (`.cshtml`):

```cshtml
@model MyApp.Models.Product

@using (Html.BeginForm())
{
    @Html.LabelFor(m => m.Name)
    @Html.TextBoxFor(m => m.Name, new { @class = "form-control" })
    @Html.ValidationMessageFor(m => m.Name)

    <button type="submit">Save</button>
}
```

Tag helper equivalent (recommended):

```cshtml
@model MyApp.Models.Product

<form asp-action="Create" method="post">
    <label asp-for="Name"></label>
    <input asp-for="Name" class="form-control" />
    <span asp-validation-for="Name" class="text-danger"></span>
    <button type="submit">Save</button>
</form>
```

Notes:
- Tag helpers (`asp-for`, `asp-action`) are HTML-friendly and more readable.
- Helpers integrate with `DataAnnotations` to produce validation attributes.

## 5. Use Cases
- Building forms tied to view models.
- Generating links, images, and form elements with correct attributes.
- Working with validation and unobtrusive client-side behavior.

## 6. Mini Practice Task
1. Create a form for a `User` model using tag helpers and show validation errors.
2. Use `Html.ActionLink` to create a link to a `Details` action for a given id.
