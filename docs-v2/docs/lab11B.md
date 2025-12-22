---
title: Lab 11B - The [Bind] Attribute
sidebar_position: 11.2
---

# The `[Bind]` Attribute

## 1. Description
The `[Bind]` attribute is an attribute in ASP.NET Core MVC that gives you explicit control over the model binding process. It allows you to specify exactly which properties of a model should be included (whitelisting) or excluded (blacklisting) when the model binder is populating your model from the request data.

Its primary use is as a security measure to prevent **over-posting** attacks.

## 2. Why It Is Important
### Preventing Over-posting
An **over-posting** (or mass assignment) attack occurs when a user submits data for more properties than the form is intended to allow.

Consider a `User` model that has an `IsAdmin` property:
```csharp
public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public bool IsAdmin { get; set; } // Sensitive property
}
```
Imagine you have an "Edit Profile" page that only allows a user to change their `Username`. The form would only have an input for `Username`. However, a malicious user could use browser developer tools to manually add another form field to the request, like `<input type="hidden" name="IsAdmin" value="true">`.

If your controller action simply accepts the `User` model like this:
```csharp
[HttpPost]
public IActionResult Edit(User user)
{
    // ... update user in database ...
}
```
The model binder would see the `IsAdmin` value in the request and happily set `user.IsAdmin` to `true`. When you save this `user` object to the database, you have just unintentionally granted a user admin privileges.

The `[Bind]` attribute helps prevent this by letting you specify *only* the properties you expect to receive from the form.

## 3. Real-World Examples
- An "Edit Profile" page where a user can change their display name and email, but not their role or account creation date.
- An "Update Product" form where an employee can change the product's name and price, but not its internal `SKU` or `ProfitMargin`.

## 4. Syntax & Explanation

### Using `[Bind]` to Whitelist Properties
You can use the `[Bind]` attribute on your controller action's parameter to specify which properties are allowed to be bound. Any properties not in this list will be ignored, even if they are present in the request.

This is the most secure way to use `[Bind]`.

```csharp
[HttpPost]
public async Task<IActionResult> Edit(int id, [Bind("Id, Username")] User user)
{
    // Even if the user submits a value for 'IsAdmin', the model binder
    // will ignore it because it's not included in the [Bind] attribute.
    // user.IsAdmin will remain false (the default value).
    
    if (id != user.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        // ... safe to update the user in the database ...
        return RedirectToAction(nameof(Index));
    }
    return View(user);
}
```

### The Best Practice: Use View Models
While `[Bind]` is a useful tool, the **recommended best practice** in modern ASP.NET Core development is to use **View Models** instead.

A View Model is a class that is specifically designed for the needs of a single view. It contains only the properties that are displayed or edited in that view.

**ViewModel:**
```csharp
public class UserEditViewModel
{
    public int Id { get; set; }
    
    [Required]
    public string Username { get; set; }
}
```

**Controller Action:**
```csharp
[HttpPost]
public async Task<IActionResult> Edit(int id, UserEditViewModel viewModel)
{
    if (id != viewModel.Id)
    {
        return NotFound();
    }

    if (ModelState.IsValid)
    {
        // 1. Fetch the original User entity from the database.
        var userFromDb = await _context.Users.FindAsync(id);
        if (userFromDb == null)
        {
            return NotFound();
        }

        // 2. Manually map the properties from the view model to the database entity.
        // This ensures that you only update the properties you intend to.
        userFromDb.Username = viewModel.Username;
        
        // The 'IsAdmin' property is never touched because it doesn't exist on the view model.

        // 3. Save the updated entity.
        _context.Update(userFromDb);
        await _context.SaveChangesAsync();

        return RedirectToAction(nameof(Index));
    }
    return View(viewModel);
}
```
Using a view model is generally considered safer and more robust because it creates a clear separation between your domain model (what's in your database) and your view model (what's in your UI). It makes over-posting impossible for properties that are not on the view model.

## 5. Mini Practice Task
1. Create a simple `Product` model with `Id`, `Name`, `Price`, and `InternalNotes` (string) properties.
2. Create an "Edit" form for this model, but only include input fields for `Name` and `Price`.
3. Create a `[HttpPost]` "Edit" action that accepts the `Product` model as a parameter.
4. Use the `[Bind]` attribute on the parameter to ensure that only the `Id`, `Name`, and `Price` properties are bound from the request.
5. In the action, use a debugger or print the value of `product.InternalNotes` to the console to verify that it is `null`, even if you use browser tools to submit a value for it.
