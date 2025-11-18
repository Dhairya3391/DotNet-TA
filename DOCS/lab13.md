# Layout Page (Usage)

## 1. Description
A layout page (commonly `_Layout.cshtml`) defines the shared HTML structure for views (header, footer, scripts). Individual views are rendered inside the layout's `@RenderBody()` placeholder. Layouts help maintain consistent UI.

## 2. Why It Is Important
Layouts avoid duplication of common markup and make global changes (styles, nav) in one place. They are essential for consistent design and maintainability.

## 3. Real-World Examples
- A site-wide header/nav and footer defined in `_Layout.cshtml`.
- Per-area or per-section layout variants (e.g., `_AdminLayout.cshtml`).

## 4. Syntax & Explanation
Typical `_Layout.cshtml`:

```cshtml
<!DOCTYPE html>
<html>
<head>
    <meta charset="utf-8" />
    <title>@ViewData["Title"] - MyApp</title>
    <link rel="stylesheet" href="/css/site.css" />
    @RenderSection("Styles", required: false)
</head>
<body>
    <header>
        <nav><!-- nav links --></nav>
    </header>
    <main role="main" class="container">
        @RenderBody()
    </main>
    <footer>
        &copy; @DateTime.Now.Year - MyApp
    </footer>
    <script src="/js/site.js"></script>
    @RenderSection("Scripts", required: false)
</body>
</html>
```

Using layout in a view (views usually inherit layout by `_ViewStart.cshtml`):

`_ViewStart.cshtml`:

```cshtml
@{
    Layout = "_Layout"; // default layout for views
}
```

Override title and add sections inside a view:

```cshtml
@{
    ViewData["Title"] = "Home";
}
<h1>Welcome</h1>
@section Scripts {
    <script>console.log('page script')</script>
}
```

## 5. Use Cases
- Site-wide templates (header, footer, analytics scripts).
- Different layouts for public pages and admin areas.
- Injecting page-specific styles/scripts using `RenderSection`.

## 6. Mini Practice Task
1. Create `_Layout.cshtml` with a navigation bar and apply it globally via `_ViewStart.cshtml`.
2. Build a view that provides a `Scripts` section to add a page-specific JavaScript snippet.
