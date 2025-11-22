# File Upload in ASP.NET Core

## 1. Description
**File upload** is the process of allowing users to send files from their local machine to the server. ASP.NET Core provides built-in features to handle file uploads smoothly and securely. This is typically done using an HTML form with a specific encoding type and handling the uploaded file(s) in a controller action via the `IFormFile` interface.

## 2. Why It Is Important
File upload functionality is a common requirement for a vast number of web applications. It is crucial for:
- **User Content:** Allowing users to upload profile pictures, documents, photos, videos, etc.
- **Data Import:** Enabling administrators to import data in bulk from files like CSV or Excel.
- **Content Management:** Letting content creators upload images and other assets for a website.

Properly handling file uploads is critical for **security** and **performance**. You must validate file size, type, and content to prevent malicious uploads, and you should process files efficiently to avoid consuming excessive server resources.

## 3. Real-World Examples
- A social media site where users upload a profile picture.
- An online job portal where candidates upload their resumes as PDF or Word documents.
- A data analysis application where a user uploads a CSV file containing sales data to be processed and visualized.

## 4. Syntax & Explanation

Handling file uploads involves two main parts: the client-side view (HTML form) and the server-side controller action.

### 1. The View (The HTML Form)
To enable file uploads, your `<form>` element must have two specific attributes:
1.  `method="post"`: File uploads must be sent via an HTTP POST request.
2.  `enctype="multipart/form-data"`: This encoding type is required for forms that include file inputs (`<input type="file">`). It allows the file data to be sent to the server in binary format.

**`Views/Files/Upload.cshtml`**
```cshtml
@{
    ViewData["Title"] = "Upload a File";
}

<h1>File Upload</h1>

<form method="post" enctype="multipart/form-data" asp-controller="Files" asp-action="Upload">
    <div class="form-group">
        <label for="file">Choose a file to upload:</label>
        
        @* The 'name' attribute ("file" in this case) must match the parameter name in the controller action *@
        <input type="file" name="file" class="form-control" />
    </div>
    
    <button type="submit" class="btn btn-primary mt-3">Upload File</button>
</form>

```
You can also upload multiple files by adding the `multiple` attribute to the input: `<input type="file" name="files" multiple />`.

### 2. The Controller (Handling the Upload)
The controller action that handles the POST request will have a parameter of type `IFormFile` (for a single file) or `List<IFormFile>` (for multiple files). The parameter name must match the `name` attribute of the `<input>` tag in the form.

The `IFormFile` interface gives you access to the file's metadata (like its name and size) and a stream to read its content.

**`Controllers/FilesController.cs`**
```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting; // Required for IHostEnvironment

public class FilesController : Controller
{
    // Inject IHostEnvironment to get the application's root path (e.g., wwwroot)
    private readonly IHostEnvironment _hostingEnvironment;

    public FilesController(IHostEnvironment hostingEnvironment)
    {
        _hostingEnvironment = hostingEnvironment;
    }

    // Action to display the upload form
    public IActionResult Upload()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        // --- 1. Basic Validation ---
        if (file == null || file.Length == 0)
        {
            ModelState.AddModelError("file", "Please select a file to upload.");
            return View();
        }

        // --- 2. Security and Size Validation ---
        
        // Example: Limit file size to 5MB
        if (file.Length > 5 * 1024 * 1024) 
        {
            ModelState.AddModelError("file", "The file size cannot exceed 5MB.");
            return View();
        }

        // Example: Restrict to certain file types (check extension)
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
        if (string.IsNullOrEmpty(extension) || !allowedExtensions.Contains(extension))
        {
            ModelState.AddModelError("file", "Invalid file type. Only JPG, PNG, and GIF are allowed.");
            return View();
        }
        
        // --- 3. Save the File ---
        
        // It's a good practice to generate a unique filename to prevent overwrites and path traversal attacks.
        var uniqueFileName = Path.GetRandomFileName() + extension;
        
        // Get the path to save the file. The 'wwwroot/uploads' folder is a good place for public files.
        // Ensure this folder exists.
        var uploadsFolder = Path.Combine(_hostingEnvironment.ContentRootPath, "wwwroot", "uploads");
        Directory.CreateDirectory(uploadsFolder); // This will do nothing if the directory already exists.
        
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        // Use a 'using' statement to ensure the stream is properly disposed of.
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            // Copy the file content to the new file stream.
            await file.CopyToAsync(stream);
        }

        // --- 4. Provide Feedback to the User ---
        
        ViewData["SuccessMessage"] = $"File uploaded successfully! Saved as {uniqueFileName}.";
        ViewData["UploadedFilePath"] = $"/uploads/{uniqueFileName}"; // Path for displaying the image

        return View();
    }
}
```

## 5. Mini Practice Task
1. Create a new ASP.NET Core MVC project.
2. Create a form that allows a user to upload a single image file (`.png` or `.jpg`).
3. Create a controller action to handle the upload.
4. In the controller, add validation to ensure the file is not larger than 1MB.
5. Save the uploaded image to a folder named `wwwroot/images/profiles`.
6. After a successful upload, display the uploaded image on the same page along with a success message.
7. (Bonus) Modify the form and controller action to handle multiple file uploads at once.