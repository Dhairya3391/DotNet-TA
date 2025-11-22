# Layout Pages in ASP.NET Core MVC

## 1. Description
A **Layout Page** in ASP.NET Core MVC (commonly named `_Layout.cshtml`) is a special Razor view that defines a common HTML structure for multiple views in your application. It acts as a master template, typically including the `<html>`, `<head>`, and `<body>` tags, along with common UI elements like headers, navigation menus, and footers.

Individual views specify the content to be inserted into the layout, which is rendered at the location of the `@RenderBody()` method call within the layout page.

## 2. Why It Is Important
Layout pages are fundamental to building maintainable and consistent web applications. Their primary benefits are:
- **Consistency:** They ensure that all pages in your application share a consistent look and feel, providing a better user experience.
- **Code Reusability (DRY Principle):** They eliminate the need to duplicate common HTML markup (like headers, footers, and script/style references) in every single view.
- **Maintainability:** If you need to make a site-wide change, such as updating the navigation bar or adding a new CSS file, you only need to modify the layout page in one place.

## 3. Real-World Examples
- A corporate website where every page has the same company logo, navigation bar, and footer.
- An e-commerce application where the shopping cart summary is always visible in the header.
- An admin dashboard where all management pages share the same sidebar navigation and top bar.

## 4. Syntax & Explanation

### The Layout Page (`_Layout.cshtml`)
This file is usually located in the `Views/Shared` folder. A typical layout page looks like this:

```cshtml
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - MyApp</title>
    
    <!-- Common CSS files -->
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    <link rel="stylesheet" href="~/css/site.css" />
    
    <!-- A section for page-specific styles -->
    @await RenderSectionAsync("Styles", required: false)
</head>
<body>
    <header>
        <nav class="navbar navbar-expand-sm navbar-light bg-white border-bottom box-shadow mb-3">
            <div class="container">
                <a class="navbar-brand" asp-area="" asp-controller="Home" asp-action="Index">MyApp</a>
                <!-- Navigation links go here -->
            </div>
        </nav>
    </header>
    
    <div class="container">
        <main role="main" class="pb-3">
            @RenderBody() @* This is where the content of individual views will be rendered *@
        </main>
    </div>

    <footer class="border-top footer text-muted">
        <div class="container">
            &copy; @DateTime.Now.Year - MyApp - <a asp-area="" asp-controller="Home" asp-action="Privacy">Privacy</a>
        </div>
    </footer>

    <!-- Common JavaScript files -->
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/bootstrap/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/js/site.js" asp-append-version="true"></script>

    <!-- A section for page-specific scripts -->
    @await RenderSectionAsync("Scripts", required: false)
</body>
</html>
```
**Key elements:**
- `@RenderBody()`: This is the placeholder where the main content of a specific view (like `Index.cshtml` or `Details.cshtml`) is injected.
- `@ViewData["Title"]`: Allows individual views to set the page title.
- `@RenderSectionAsync("Scripts", required: false)`: Defines an optional section where views can add their own specific scripts or styles.

### Specifying a Layout
There are two primary ways to tell a view which layout to use.

**1. Using `_ViewStart.cshtml` (Recommended for global setting)**
The `_ViewStart.cshtml` file in the `Views` folder is a special file that runs before any view in that folder (and its subfolders). It's the perfect place to set the default layout for your entire application.

**`Views/_ViewStart.cshtml`**
```cshtml
@{
    Layout = "_Layout";
}
```
With this file in place, all views will automatically use `_Layout.cshtml` unless they explicitly override it.

**2. Specifying in the View (For overriding the default)**
A specific view can choose a different layout or no layout at all by setting the `Layout` property at the top of the file.

```cshtml
@{
    ViewData["Title"] = "A Special Page";
    Layout = "_SpecialLayout"; // Use a different layout for this page
}

<h1>This is a special page.</h1>
```
To have a view with no layout (e.g., for returning a raw HTML fragment for an AJAX call), you can set `Layout = null;`.

### Using Sections
Sections provide a way for content views to inject HTML into specific parts of the layout page.

**In the Layout Page (`_Layout.cshtml`):**
```html
...
@await RenderSectionAsync("Scripts", required: false)
...
```

**In a Content View (e.g., `Index.cshtml`):**
```cshtml
@{
    ViewData["Title"] = "Home Page";
}

<h1>Welcome!</h1>

@section Scripts {
    <script>
        // This script will be rendered in the "Scripts" section of the layout page.
        console.log("This is a script from the home page.");
    </script>
}
```

## 5. Mini Practice Task
1. Create a new ASP.NET Core MVC project. It will come with a default `_Layout.cshtml`.
2. Modify the footer in `_Layout.cshtml` to include your name.
3. Create a new view called `About.cshtml` in the `Views/Home` folder.
4. Add some simple HTML content to `About.cshtml` (e.g., an `<h1>` and a `<p>` tag). Do not specify a layout in this file.
5. Run the application and navigate to the "Home" and "About" pages. Verify that both pages share the same header and your modified footer.
6. Now, create a new section in your `_Layout.cshtml` just before the closing `</body>` tag called "FooterScripts".
7. In your `About.cshtml` view, add a `FooterScripts` section and include a simple JavaScript alert.
8. Run the application and check that the alert only appears when you navigate to the "About" page.