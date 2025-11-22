---
title: Lab 17 - Partial Views
sidebar_position: 17
---
# Partial Views

## 1. Description
Partial views are reusable view fragments rendered inside other views. They are useful for components like headers, footers, lists, or small UI blocks that repeat across pages.

## 2. Why It Is Important
They promote DRY (Don't Repeat Yourself) and make views easier to maintain by extracting repeated markup into a single file.

## 3. Real-World Examples
- A `_LoginPartial.cshtml` showing current user status in the site header.
- A `_ProductCard.cshtml` partial to render product summary across multiple pages.

## 4. Syntax & Explanation
Create `_ProductCard.cshtml` (file under `Views/Shared`):

```cshtml
@model MyApp.Models.Product
<div class="product-card">
    <h3>@Model.Name</h3>
    <p>@Model.Price.ToString("C")</p>
    <a asp-controller="Products" asp-action="Details" asp-route-id="@Model.Id">Details</a>
</div>
```

Render partial in a view:

```cshtml
@model IEnumerable<MyApp.Models.Product>

@foreach(var p in Model)
{
    @await Html.PartialAsync("_ProductCard", p)
}
```

Or use `PartialTagHelper` (Razor `<partial />`):

```cshtml
<partial name="_ProductCard" model="productInstance" />
```

Notes:
- Use `Html.Partial`/`PartialAsync` or `<partial />` for asynchronous rendering.
- For larger reusable components, consider view components or tag helpers.

## 5. Use Cases
- Small, repeatable UI fragments (cards, lists, navs).
- Breaking complex pages into manageable pieces.
- Reusing the same markup across different views and layouts.

## 6. Mini Practice Task
1. Create a `_LoginPartial.cshtml` that shows the user's name if logged in and a `Login` link otherwise.
2. Replace repeated product list markup with `_ProductCard` partial and verify pages still render.
