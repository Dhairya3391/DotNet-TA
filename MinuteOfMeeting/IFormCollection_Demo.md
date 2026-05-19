# IFormCollection Demo - Simple CRUD Example

## What is IFormCollection?

`IFormCollection` is a way to receive form data in ASP.NET Core **without creating a Model class**. It's like a dictionary that holds all form values.

**Namespace:** `Microsoft.AspNetCore.Http`

---

## Why Use It?

✅ **Use IFormCollection When:**
- You have a simple form with just 2-3 fields
- You don't want to create a Model class
- Quick prototyping or testing

❌ **Don't Use IFormCollection When:**
- Complex forms with many fields → Use Model instead
- Need validation → Model with data annotations is better

---

## Complete Example: Department Add/Edit Using IFormCollection

### Step 1: Create Form View (AddEdit.cshtml)

```html
@{
    ViewData["Title"] = "Add/Edit Department";
    int deptId = ViewBag.DepartmentID ?? 0;
    string deptName = ViewBag.DepartmentName ?? "";
}

<div class="container mt-4">
    <div class="row justify-content-center">
        <div class="col-md-6">
            <div class="card shadow">
                <div class="card-header bg-primary text-white">
                    <h4 class="mb-0">@(deptId == 0 ? "Add" : "Edit") Department</h4>
                </div>
                <div class="card-body">
                    <form asp-action="AddEdit" method="post">
                        
                        <!-- Hidden field for ID (0 for new, number for edit) -->
                        <input type="hidden" name="DepartmentID" value="@deptId" />
                        
                        <!-- Department Name -->
                        <div class="form-group mb-3">
                            <label class="form-label">Department Name</label>
                            <input type="text" 
                                   name="DepartmentName" 
                                   value="@deptName" 
                                   class="form-control" 
                                   placeholder="Enter Department Name" 
                                   required />
                        </div>
                        
                        <!-- Buttons -->
                        <div class="d-flex justify-content-between">
                            <button type="submit" class="btn btn-success">
                                @(deptId == 0 ? "Save" : "Update")
                            </button>
                            <a asp-action="Index" class="btn btn-secondary">
                                Back to List
                            </a>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>
</div>
```

### Step 2: Create Index View (Index.cshtml)

```html
@model List<Department>

<div class="container mt-4">
    <h2 class="mb-4">Department List</h2>

    <div class="mb-3">
        <a asp-action="AddEdit" class="btn btn-primary">
            Add New Department
        </a>
    </div>

    @if (TempData["Success"] != null)
    {
        <div class="alert alert-success alert-dismissible fade show">
            @TempData["Success"]
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    }
    
    @if (TempData["Error"] != null)
    {
        <div class="alert alert-danger alert-dismissible fade show">
            @TempData["Error"]
            <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
        </div>
    }

    @if (Model != null && Model.Count > 0)
    {
        <table class="table table-bordered table-striped table-hover">
            <thead class="thead-dark">
                <tr>
                    <th>Department ID</th>
                    <th>Department Name</th>
                    <th style="width: 200px;">Actions</th>
                </tr>
            </thead>
            <tbody>
                @foreach (var department in Model)
                {
                    <tr>
                        <td>@department.DepartmentId</td>
                        <td>@department.DepartmentName</td>
                        <td>
                            <a asp-action="AddEdit" 
                               asp-route-id="@department.DepartmentId"
                               class="btn btn-sm btn-warning">
                                Edit
                            </a>
                            <a asp-action="Delete" 
                               asp-route-id="@department.DepartmentId" 
                               class="btn btn-sm btn-danger">
                                Delete
                            </a>
                        </td>
                    </tr>
                }
            </tbody>
        </table>
    }
    else
    {
        <div class="alert alert-info">
            No departments found. Click "Add New Department" to create one.
        </div>
    }
</div>
```

### Step 3: Controller with IFormCollection

```csharp
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

public class DepartmentController : Controller
{
    // GET: Show blank form for Add OR filled form for Edit
    [HttpGet]
    public IActionResult AddEdit(int? id)
    {
        if (id > 0)
        {
            // Edit Mode - Load existing data
            Department department = GetDepartmentById(id.Value);
            ViewBag.DepartmentID = department.DepartmentId;
            ViewBag.DepartmentName = department.DepartmentName;
        }
        else
        {
            // Add Mode
            ViewBag.DepartmentID = 0;
        }
        
        return View();
    }
    
    // POST: Save form data (works for both Add and Edit)
    [HttpPost]
    public IActionResult AddEdit(IFormCollection formData)
    {
        try
        {
            // Get values from form using field names
            int deptId = Convert.ToInt32(formData["DepartmentID"]);
            string deptName = formData["DepartmentName"].ToString();
            
            // Simple validation
            if (string.IsNullOrEmpty(deptName))
            {
                TempData["Error"] = "Department name is required";
                return RedirectToAction("AddEdit", new { id = deptId > 0 ? deptId : 0 });
            }
            
            // Database connection
            SqlConnection con = new SqlConnection(
                "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;TrustServerCertificate=True;");
            
            // Decide: Insert or Update?
            if (deptId == 0)
            {
                // INSERT - New department
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_InsertDepartment";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentName", deptName);
                
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                
                TempData["Success"] = "Department added successfully";
            }
            else
            {
                // UPDATE - Existing department
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = con;
                cmd.CommandText = "sp_UpdateDepartment";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DepartmentId", deptId);
                cmd.Parameters.AddWithValue("@DepartmentName", deptName);
                
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                
                TempData["Success"] = "Department updated successfully";
            }
            
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error: " + ex.Message;
            return RedirectToAction("Index");
        }
    }
    
    // Helper method to load department for edit
    private Department GetDepartmentById(int id)
    {
        Department dept = new Department();
        
        SqlConnection con = new SqlConnection(
            "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;TrustServerCertificate=True;");
        
        SqlCommand cmd = new SqlCommand("sp_GetDepartmentById", con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@DepartmentId", id);
        
        con.Open();
        SqlDataReader reader = cmd.ExecuteReader();
        
        if (reader.Read())
        {
            dept.DepartmentId = Convert.ToInt32(reader["DepartmentId"]);
            dept.DepartmentName = reader["DepartmentName"].ToString();
        }
        
        con.Close();
        return dept;
    }
}
```

---

## How It Works: Step-by-Step

### 1. **User Clicks "Add New Department"**
   - URL: `/Department/AddEdit`
   - `id` is `null`
   - Controller sets `ViewBag.DepartmentID = 0`
   - Empty form is shown

### 2. **User Fills Form and Clicks Save**
   - Form submits with `DepartmentID = 0` and `DepartmentName = "IT"`
   - POST action receives `IFormCollection formData`
   - `deptId == 0` → INSERT query runs
   - Success message shown

### 3. **User Clicks "Edit" Button**
   - URL: `/Department/AddEdit?id=5`
   - `id` is `5`
   - Controller loads data from database
   - Form is pre-filled with existing data

### 4. **User Modifies and Clicks Update**
   - Form submits with `DepartmentID = 5` and new `DepartmentName`
   - POST action receives `IFormCollection formData`
   - `deptId == 5` → UPDATE query runs
   - Success message shown

---

## Key Points to Remember

### ✅ Important Concepts

1. **Hidden Field is Critical**
   ```html
   <input type="hidden" name="DepartmentID" value="@deptId" />
   ```
   - For Add: value is `0`
   - For Edit: value is actual ID (e.g., `5`)
   - This tells the POST action whether to INSERT or UPDATE

2. **Same Action for Both Add and Edit**
   ```csharp
   if (deptId == 0)
       // INSERT
   else
       // UPDATE
   ```

3. **Accessing Form Values**
   ```csharp
   int deptId = Convert.ToInt32(formData["DepartmentID"]);
   string deptName = formData["DepartmentName"].ToString();
   ```
   - Use field **name** from HTML (not ID)
   - Always convert to appropriate type

4. **ViewBag for Passing Data to View**
   ```csharp
   ViewBag.DepartmentID = 5;
   ViewBag.DepartmentName = "Computer Science";
   ```
   - Used in GET action to pre-fill form

---

## Comparison: IFormCollection vs Model

### Using IFormCollection (No Model Class Needed)

```csharp
// Controller
[HttpPost]
public IActionResult AddEdit(IFormCollection formData)
{
    // Manually get each field by name
    int deptId = Convert.ToInt32(formData["DepartmentID"]);
    string deptName = formData["DepartmentName"].ToString();
    
    // Manual validation
    if (string.IsNullOrEmpty(deptName))
    {
        TempData["Error"] = "Name required";
        return RedirectToAction("AddEdit");
    }
    
    // Save to database
    SqlCommand cmd = new SqlCommand("sp_InsertDepartment", con);
    cmd.Parameters.AddWithValue("@DepartmentName", deptName);
    
    return RedirectToAction("Index");
}
```

**Pros:**
- No Model class needed → Less files, quick setup
- Good for simple demos
- Direct access to form fields

**Cons:**
- No IntelliSense (no auto-complete)
- Manual validation needed
- Type conversion errors possible

---

### Using Model (Need Model Class)

```csharp
// Model class (Department.cs)
public class Department
{
    public int DepartmentId { get; set; }
    
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100)]
    public string DepartmentName { get; set; }
}

// View - using Model
@model Department

<form asp-action="AddEdit" method="post">
    <input asp-for="DepartmentId" type="hidden" />
    <input asp-for="DepartmentName" class="form-control" />
    <span asp-validation-for="DepartmentName"></span>
</form>

// Controller
[HttpPost]
public IActionResult AddEdit(Department model)
{
    // Automatic binding - no need to manually get values
    // Automatic validation
    if (!ModelState.IsValid)
    {
        return View(model);
    }
    
    // Save to database - properties auto-bound
    SqlCommand cmd = new SqlCommand("sp_InsertDepartment", con);
    cmd.Parameters.AddWithValue("@DepartmentName", model.DepartmentName);
    
    return RedirectToAction("Index");
}
```

**Pros:**
- Type-safe (compile-time checking)
- Automatic validation with data annotations
- IntelliSense support (auto-complete)
- Best for production code

**Cons:**
- Need Model class → Extra file to create
- More initial setup

---

## Quick Reference

### ⚠️ CRITICAL: Field Names Must Match!

```html
<!-- In HTML Form -->
<input type="text" name="DepartmentName" value="IT" />

<!-- In Controller -->
string deptName = formData["DepartmentName"];  // ✅ Same name - WORKS
string deptName = formData["DeptName"];        // ❌ Different name - NULL
```

**Rule:** The string inside `formData["..."]` **MUST EXACTLY MATCH** the `name` attribute in HTML.

---

## When Should You Use IFormCollection?

| Your Situation | Recommendation |
|----------------|----------------|
| Learning/Practice project | ✅ Use IFormCollection |
| Simple 2-3 field form | ✅ Use IFormCollection |
| Quick demo to students | ✅ Use IFormCollection |
| Complex form (10+ fields) | ❌ Use Model instead |
| Need strong validation | ❌ Use Model with Data Annotations |
| Production enterprise app | ❌ Use Model for maintainability |

**Bottom Line:** IFormCollection is great for learning and simple forms. For real projects with complex forms, use strongly-typed Models.

---

## Try It Yourself!

### Step-by-Step Exercise

1. **Create the Model** (Department.cs)
   ```csharp
   public class Department
   {
       public int DepartmentId { get; set; }
       public string DepartmentName { get; set; }
   }
   ```

2. **Create stored procedures** in SQL Server
   ```sql
   -- Insert
   CREATE PROCEDURE sp_InsertDepartment
       @DepartmentName NVARCHAR(100)
   AS
   BEGIN
       INSERT INTO Departments (DepartmentName) 
       VALUES (@DepartmentName)
   END
   
   -- Update
   CREATE PROCEDURE sp_UpdateDepartment
       @DepartmentId INT,
       @DepartmentName NVARCHAR(100)
   AS
   BEGIN
       UPDATE Departments 
       SET DepartmentName = @DepartmentName 
       WHERE DepartmentId = @DepartmentId
   END
   ```

3. **Copy the AddEdit.cshtml view** from Step 1 above

4. **Copy the controller code** from Step 3 above

5. **Update connection string** in controller
   ```csharp
   SqlConnection con = new SqlConnection(
       "Server=YOUR_SERVER;Database=YOUR_DATABASE;Trusted_Connection=True;");
   ```

6. **Run and test:**
   - Add a new department
   - Edit existing department
   - Check TempData messages work

**Done! ✅** You've created a working CRUD without using Model binding!

---
