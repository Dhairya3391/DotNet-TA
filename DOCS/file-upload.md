# File Upload

## 1. Description
File upload in ASP.NET Core is handled using the `IFormFile` interface. Files are posted from an HTML form (`enctype="multipart/form-data"`) and can be validated, streamed, and saved to disk, cloud storage, or processed in memory.

## 2. Why It Is Important
Uploading files is a common requirement (profile pictures, documents). Proper handling prevents security problems (unrestricted file writes), and provides a smooth user experience.

## 3. Real-World Examples
- User uploads profile photo which is resized and saved.
- Admin uploads a CSV file to import product data.

## 4. Syntax & Explanation
View (form):

```cshtml
<form asp-action="Upload" method="post" enctype="multipart/form-data">
    <input type="file" name="file" />
    <button type="submit">Upload</button>
</form>
```

Controller action:

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public class FilesController : Controller
{
    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file selected");

        // Basic validation: limit file size (example: 5MB)
        if (file.Length > 5 * 1024 * 1024) return BadRequest("File too large");

        // Save to wwwroot/uploads (ensure folder exists and is safe)
        var uploads = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(uploads);
        var filePath = Path.Combine(uploads, Path.GetRandomFileName() + Path.GetExtension(file.FileName));

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return Ok(new { path = "/uploads/" + Path.GetFileName(filePath) });
    }
}
```

Security tips:
- Validate file type (by extension and, ideally, by file signature).
- Store uploads outside of app root or use randomized filenames to avoid overwrite and path traversal.
- Limit size and scan files if necessary.

## 5. Use Cases
- Profile photo upload and processing.
- Importing CSV, Excel files for data import.
- Document management features.

## 6. Mini Practice Task
1. Build an upload form and save the file, then display an `<img>` tag for uploaded images.
2. Add server-side validation to accept only `.png` and `.jpg` files under 2 MB.
