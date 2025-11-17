# Form Collection & `Bind` Attribute

## 1. Description
Form collection refers to collecting form values posted to the server. Model binding maps form fields to action parameters or model properties. The `[Bind]` attribute restricts which properties are bound for security and clarity.

## 2. Why It Is Important
Understanding how binding works prevents over-posting attacks (malicious clients setting properties they should not) and gives control over which fields are updated during model updates.

## 3. Real-World Examples
- Receiving a `UserViewModel` from a registration form.
- Using `[Bind(Include = "Name,Email")]` to accept only specified fields (note: in Core use `Bind` constructor or view models instead).

## 4. Syntax & Explanation
Binding to a model directly:

```csharp
[HttpPost]
public IActionResult Edit(Product model)
{
    if (!ModelState.IsValid) return View(model);
    // Update saved entity using model
    return RedirectToAction("Index");
}
```

Using `IFormCollection` to read raw form data:

```csharp
using Microsoft.AspNetCore.Http;

[HttpPost]
public IActionResult Submit(IFormCollection form)
{
    var name = form["Name"]; // access by field name
    // process
    return Ok();
}
```

Using `[Bind]` to restrict bound properties (ASP.NET Core):

```csharp
[HttpPost]
public IActionResult Edit([Bind("Id,Name,Price")] Product product)
{
    // Only Id, Name, Price will be bound from the request
}
```

Best practice: prefer view models that contain only the properties the view should post, rather than relying heavily on `[Bind]`.

## 5. Use Cases
- Protecting against over-posting when updating entities.
- Reading non-model fields or dynamic fields using `IFormCollection`.
- Handling form fields that don't map directly to model properties.

## 6. Mini Practice Task
1. Write an edit form that updates only `Name` and `Price` of a `Product` while preventing changes to `IsAdminOnly` fields.
2. Read a CSV-like textarea value from `IFormCollection` and parse it into a `List<string>`.
