---
title: Lab 16 - Custom Tag Helpers
sidebar_position: 16
---
# Custom Tag Helpers

## 1. Description
Custom Tag Helpers let you create reusable, HTML-like components that run server-side code to render HTML. They are classes that inherit from `TagHelper` and are used in Razor views via custom element/attribute names.

## 2. Why It Is Important
They encapsulate repeated rendering logic, improve view readability, and integrate with Tag Helper pipeline (attributes, context, child content).

## 3. Real-World Examples
- A `PagerTagHelper` to render pagination controls.
- A `BadgeTagHelper` to render styled status badges (e.g., `<span badge status="active">Active</span>`).

## 4. Syntax & Explanation
Example: a simple `BadgeTagHelper` that outputs a colored span.

`BadgeTagHelper.cs`:

```csharp
using Microsoft.AspNetCore.Razor.TagHelpers;

[HtmlTargetElement("badge")]
public class BadgeTagHelper : TagHelper
{
    public string Status { get; set; } // attribute: status

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        output.TagName = "span";
        var css = Status == "active" ? "badge badge-success" : "badge badge-secondary";
        output.Attributes.SetAttribute("class", css);
    }
}
```

Usage in `.cshtml`:

```cshtml
<badge status="active">Active</badge>
```

Registration: Tag Helpers are discovered automatically if the assembly is referenced; ensure `_ViewImports.cshtml` has `@addTagHelper *, YourAssemblyName` (usually available by default in ASP.NET Core projects).

## 5. Use Cases
- Encapsulate UI widgets (tables, pagers, badges).
- Keep view markup consistent and DRY.
- Provide attribute-driven components for designers and developers.

## 6. Mini Practice Task
1. Implement a `PagerTagHelper` that accepts `Page`, `TotalPages` attributes and renders previous/next links.
2. Create a `btn` tag helper that renders a bootstrap-styled button with `asp-action` support.
