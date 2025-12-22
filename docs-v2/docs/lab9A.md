---
title: Lab 9A - Razor Syntax Overview
sidebar_position: 9.1
---

# Razor Syntax Overview

## 1. Description
**Razor** is a markup syntax for embedding server-side C# code into webpages. It is the default view engine for ASP.NET Core MVC and Razor Pages. Razor syntax is designed to be clean, concise, and easy to read, allowing for a fluid transition between HTML and C#.

The `@` symbol is the key character in Razor. It is used to transition from HTML to C#.

## 2. Why It Is Important
Razor is fundamental to building dynamic web pages in ASP.NET Core. It allows you to:
- **Render Dynamic Content:** Display data from your application (e.g., product names, user information) directly in your HTML.
- **Implement Logic in Views:** Use C# constructs like `if` statements, `for` loops, and `foreach` loops to control how your HTML is rendered.
- **Keep Views Clean:** The syntax is designed to be minimal, making your view files more readable and maintainable compared to older, more verbose view engines.

## 3. Real-World Examples
- Displaying a list of products from a database in an HTML table.
- Showing a "Welcome, [Username]!" message if the user is logged in, or a "Login" button if they are not.
- Creating reusable UI components (like headers, footers, and sidebars) using Razor layouts and partial views.

## 4. Syntax & Explanation
Here are some of the most common Razor syntax constructs you will see in a `.cshtml` file.

```cshtml
@* This is a Razor comment. It will not be sent to the browser. *@

<!-- This is a regular HTML comment. -->

@{
    // This is a C# code block.
    // You can write multiple lines of C# here.
    var name = "Alice";
    var a = 10;
    var b = 20;
    var sum = a + b;
}

<h1>Razor Syntax Examples</h1>

<h2>Expressions</h2>
<p>Hello, @name!</p> @* Inline expression to render a variable *@
<p>The sum of @a and @b is @sum.</p>
<p>You can also use expressions: @(a + b)</p> @* Complex expressions go in parentheses *@

<h2>Code Blocks</h2>
@{
    var products = new List<string> { "Laptop", "Mouse", "Keyboard" };
}
<h3>Product List</h3>
<ul>
    @foreach (var product in products)
    {
        <li>@product</li>
    }
</ul>

<h2>Conditional Statements</h2>
@if (name == "Alice")
{
    <p>Welcome, Alice!</p>
}
else
{
    <p>Welcome, guest!</p>
}

<h2>Mixing HTML and C#</h2>
@foreach (var p in products)
{
    // Razor is smart enough to switch back to HTML for tags
    <div>
        <strong>Product Name:</strong> @p
    </div>
}
```

## 5. Use Cases
- **Displaying Data:** The primary use is to render data from your model or other sources into your HTML.
- **Conditional Rendering:** Showing or hiding certain HTML elements based on a condition (e.g., user's role, a value being null).
- **Looping:** Iterating over a collection of items to generate repetitive HTML, such as rows in a table or items in a list.
- **Creating Reusable UI:** Building partial views and layouts to avoid duplicating HTML code.

## 6. Mini Practice Task
1. Create a new Razor view (`.cshtml` file).
2. In a C# code block at the top, create a `List<int>` with some numbers.
3. Use a Razor `foreach` loop to iterate through the list.
4. Inside the loop, use an `if` statement to check if each number is even or odd.
5. Render a paragraph `<p>` for each number, stating whether it is even or odd (e.g., "<p>5 is odd</p>", "<p>6 is even</p>").
6. Use CSS styling to make the text for even numbers green and the text for odd numbers red.
