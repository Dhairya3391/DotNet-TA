# Custom Tag Helpers in ASP.NET Core

## 1. Description
**Tag Helpers** are a server-side feature in ASP.NET Core that allows you to participate in the rendering of HTML elements in your Razor views. A **Custom Tag Helper** is one that you create yourself to encapsulate reusable or complex rendering logic.

You can create custom tag helpers that target existing HTML elements (like `<p>` or `<a>`) and modify them, or you can create entirely new tags (like `<email-link>` or `<pager>`). They are written in C# and provide a much cleaner and more HTML-friendly syntax than traditional HTML Helpers.

## 2. Why It Is Important
Custom Tag Helpers are a powerful tool for creating clean, readable, and maintainable Razor views. They allow you to:
- **Encapsulate Logic:** Move complex C# rendering logic out of your views and into a reusable C# class.
- **Improve Readability:** Replace complex C# code blocks in your views with simple, declarative HTML-like tags.
- **Promote Reusability (DRY):** Create reusable UI components (like status badges, pagers, modal dialogs) that can be used across your entire application.
- **Enable Better Collaboration:** Designers and front-end developers who may not be C# experts can easily work with and understand the markup in the views.

## 3. Real-World Examples
- A `<pager>` tag helper that generates the complete HTML for a pagination control based on the current page, total pages, and a URL template.
- An `<environment>` tag helper (which is built-in) that renders its content only when the application is running in a specific environment (e.g., Development, Staging, or Production).
- A `conditional-class` attribute tag helper that adds a CSS class to an element only if a certain C# condition is true.
- An `<email-link>` tag helper that creates a `mailto:` link and obfuscates the email address to protect it from spam bots.

## 4. Syntax & Explanation

Creating a custom tag helper involves three main steps:
1.  **Create the Tag Helper Class:** Create a C# class that inherits from `Microsoft.AspNetCore.Razor.TagHelpers.TagHelper`.
2.  **Define Target and Attributes:** Use attributes to specify which HTML tag your helper will target and what properties will be available as attributes on that tag.
3.  **Implement the Logic:** Override the `Process` or `ProcessAsync` method to implement the rendering logic.
4.  **Register the Tag Helper:** Make the tag helper available to your views by adding an `@addTagHelper` directive in `_ViewImports.cshtml`.

### Example: A Simple `BadgeTagHelper`
Let's create a `<badge>` tag helper that renders a Bootstrap-style badge.

**Step 1 & 2: Create the Class and Define Target/Attributes**
```csharp
using Microsoft.AspNetCore.Razor.TagHelpers;

// This attribute specifies that our tag helper will target an HTML element named "badge"
[HtmlTargetElement("badge")]
public class BadgeTagHelper : TagHelper
{
    // Public properties become attributes on the tag helper.
    // The name is converted from PascalCase to kebab-case (e.g., BadgeColor -> badge-color).
    
    // <badge badge-color="success">...</badge>
    public string BadgeColor { get; set; } = "secondary"; // Default to "secondary"

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        // Set the HTML element that will be rendered.
        output.TagName = "span";
        
        // Add the CSS classes for the Bootstrap badge.
        output.Attributes.SetAttribute("class", $"badge bg-{BadgeColor}");
    }
}
```

**Step 3: Register the Tag Helper**
To make this tag helper available in all your views, add the following line to `Views/_ViewImports.cshtml`.

**`Views/_ViewImports.cshtml`**
```cshtml
@* ... other using statements ... *@
@addTagHelper *, YourAssemblyName @* Replace "YourAssemblyName" with the actual name of your project's assembly *@
```

**Step 4: Use the Tag Helper in a View**
Now you can use your custom tag helper in any Razor view like a regular HTML element.

```cshtml
<h1>User Status</h1>
<p>
    Current Status: <badge badge-color="success">Active</badge>
</p>
<p>
    Previous Status: <badge badge-color="danger">Inactive</badge>
</p>
<p>
    Pending Status: <badge>Pending</badge> @* This will use the default color "secondary" *@
</p>
```

**Rendered HTML:**
```html
<h1>User Status</h1>
<p>
    Current Status: <span class="badge bg-success">Active</span>
</p>
<p>
    Previous Status: <span class="badge bg-danger">Inactive</span>
</p>
<p>
    Pending Status: <span class="badge bg-secondary">Pending</span>
</p>
```

## 5. Mini Practice Task
1. Create a new custom tag helper called `EmailLinkTagHelper`.
2. It should target a tag named `email-link`.
3. It should have a public property `string MailTo` which will be used as an attribute `mail-to` on the tag.
4. In the `Process` method, configure the `TagHelperOutput` to render an `<a>` tag.
5. The `href` attribute of the `<a>` tag should be set to `mailto:{MailTo}`.
6. The content of the `<a>` tag should be the value of the `MailTo` property.
7. Register and use your new tag helper in a view like this: `<email-link mail-to="test@example.com"></email-link>`.
8. Verify that it renders the correct HTML: `<a href="mailto:test@example.com">test@example.com</a>`.