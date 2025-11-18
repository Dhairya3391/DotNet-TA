# Database Connectivity and Implementation of Read Operation

## 1. Description
Database connectivity establishes a communication channel between your ASP.NET Core application and a database server. The Read operation retrieves and displays data from database tables, typically showing all records in a structured format like tables or lists. Entity Framework Core simplifies this process through object-relational mapping (ORM).

## 2. Why It Is Important
Reading data from databases is fundamental to most applications. Proper database connectivity ensures data integrity, security, and performance. Read operations are the most common database operation, forming the basis for displaying lists, reports, search results, and dashboard information to users.

## 3. Real-World Examples
- Student management system displaying all enrolled students
- E-commerce site showing product catalog
- Hospital application listing patient records
- Banking app displaying transaction history
- Inventory system showing all available products
- Blog platform displaying all published articles

## 4. Syntax & Explanation

### Database Connection Configuration

**appsettings.json**:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=your_server_name;Database=YourDatabaseName;Trusted_Connection=true;MultipleActiveResultSets=true;TrustServerCertificate=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

**Program.cs** (ASP.NET Core 6+):
```csharp
using Microsoft.EntityFrameworkCore;
using YourProjectName.Data;
using YourProjectName.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllersWithViews();

// Configure database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
```

### Entity Model

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    [Column(TypeName = "nvarchar(100)")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    [Column(TypeName = "nvarchar(255)")]
    public string Email { get; set; }

    [StringLength(20)]
    [Column(TypeName = "nvarchar(20)")]
    public string Phone { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [StringLength(50)]
    [Column(TypeName = "nvarchar(50)")]
    public string Course { get; set; }

    [StringLength(10)]
    [Column(TypeName = "nvarchar(10)")]
    public string EnrollmentNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    // Navigation property (if related to other entities)
    public virtual ICollection<Enrollment> Enrollments { get; set; }
}
```

### Database Context (DbContext)

```csharp
using Microsoft.EntityFrameworkCore;
using YourProjectName.Models;

namespace YourProjectName.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        // DbSet properties for each table
        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Enrollment> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure Student entity
            modelBuilder.Entity<Student>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.FirstName).IsRequired();
                entity.Property(e => e.LastName).IsRequired();
                entity.Property(e => e.Email).IsRequired();
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.EnrollmentNumber).IsRequired();
                entity.HasIndex(e => e.EnrollmentNumber).IsUnique();
            });

            // Seed initial data (optional)
            modelBuilder.Entity<Student>().HasData(
                new Student
                {
                    Id = 1,
                    FirstName = "John",
                    LastName = "Doe",
                    Email = "john.doe@example.com",
                    Phone = "1234567890",
                    DateOfBirth = new DateTime(2000, 5, 15),
                    Course = "Computer Science",
                    EnrollmentNumber = "CS2024001"
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Jane",
                    LastName = "Smith",
                    Email = "jane.smith@example.com",
                    Phone = "0987654321",
                    DateOfBirth = new DateTime(2001, 8, 22),
                    Course = "Information Technology",
                    EnrollmentNumber = "IT2024001"
                }
            );
        }
    }
}
```

### Controller with Read Operations

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourProjectName.Data;
using YourProjectName.Models;

public class StudentsController : Controller
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Students - Display all records
    public async Task<IActionResult> Index()
    {
        try
        {
            var students = await _context.Students
                .Where(s => s.IsActive) // Only active students
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();

            return View(students);
        }
        catch (Exception ex)
        {
            // Log error (implement logging)
            ModelState.AddModelError("", "Unable to load student data. Please try again later.");
            return View(new List<Student>());
        }
    }

    // GET: Students/Details/5 - Display single record details
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var student = await _context.Students
                .FirstOrDefaultAsync(m => m.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Unable to load student details. Please try again later.");
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Students/Search - Search students
    public async Task<IActionResult> Search(string searchTerm)
    {
        try
        {
            var students = await _context.Students
                .Where(s => s.IsActive &&
                           (string.IsNullOrEmpty(searchTerm) ||
                            s.FirstName.Contains(searchTerm) ||
                            s.LastName.Contains(searchTerm) ||
                            s.Email.Contains(searchTerm) ||
                            s.EnrollmentNumber.Contains(searchTerm)))
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();

            return View("Index", students);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Search failed. Please try again later.");
            return RedirectToAction(nameof(Index));
        }
    }

    // GET: Students/ByCourse - Filter by course
    public async Task<IActionResult> ByCourse(string course)
    {
        try
        {
            var students = await _context.Students
                .Where(s => s.IsActive && s.Course == course)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();

            ViewBag.SelectedCourse = course;
            return View("Index", students);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "Unable to load students by course. Please try again later.");
            return RedirectToAction(nameof(Index));
        }
    }
}
```

### View for Displaying All Records (Index.cshtml)

```cshtml
@model IEnumerable<Student>
@{
    ViewData["Title"] = "Students";
}

<div class="container">
    <div class="row mb-4">
        <div class="col-md-6">
            <h1>Students</h1>
        </div>
        <div class="col-md-6 text-end">
            <a asp-action="Create" class="btn btn-primary">Add New Student</a>
        </div>
    </div>

    <!-- Search Form -->
    <div class="row mb-3">
        <div class="col-md-8">
            <form asp-action="Search" method="get" class="d-flex">
                <input type="text" name="searchTerm" class="form-control me-2"
                       placeholder="Search by name, email, or enrollment number..."
                       value="@ViewData["CurrentFilter"]" />
                <button type="submit" class="btn btn-outline-primary">Search</button>
                @if (!string.IsNullOrEmpty(ViewData["CurrentFilter"]?.ToString()))
                {
                    <a asp-action="Index" class="btn btn-outline-secondary ms-2">Clear</a>
                }
            </form>
        </div>
        <div class="col-md-4">
            <form asp-action="ByCourse" method="get" class="d-flex">
                <select name="course" class="form-select me-2" onchange="this.form.submit()">
                    <option value="">All Courses</option>
                    <option value="Computer Science" selected="@(ViewBag.SelectedCourse == "Computer Science")">Computer Science</option>
                    <option value="Information Technology" selected="@(ViewBag.SelectedCourse == "Information Technology")">Information Technology</option>
                    <option value="Electronics" selected="@(ViewBag.SelectedCourse == "Electronics")">Electronics</option>
                    <option value="Mechanical" selected="@(ViewBag.SelectedCourse == "Mechanical")">Mechanical</option>
                </select>
            </form>
        </div>
    </div>

    <!-- Results Summary -->
    @if (Model != null)
    {
        <div class="alert alert-info">
            <strong>Total Records:</strong> @Model.Count() students found
            @if (!string.IsNullOrEmpty(ViewBag.SelectedCourse))
            {
                <span>in <strong>@ViewBag.SelectedCourse</strong></span>
            }
        </div>
    }

    <!-- Data Table -->
    <div class="card">
        <div class="card-body">
            @if (Model != null && Model.Any())
            {
                <div class="table-responsive">
                    <table class="table table-striped table-hover">
                        <thead class="table-dark">
                            <tr>
                                <th>
                                    <a asp-action="Index" asp-route-sortOrder="@(ViewData["NameSortParm"])">
                                        @Html.DisplayNameFor(model => model.FirstName)
                                    </a>
                                </th>
                                <th>@Html.DisplayNameFor(model => model.LastName)</th>
                                <th>@Html.DisplayNameFor(model => model.Email)</th>
                                <th>@Html.DisplayNameFor(model => model.Phone)</th>
                                <th>@Html.DisplayNameFor(model => model.Course)</th>
                                <th>@Html.DisplayNameFor(model => model.EnrollmentNumber)</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var item in Model)
                            {
                                <tr>
                                    <td>@Html.DisplayFor(modelItem => item.FirstName)</td>
                                    <td>@Html.DisplayFor(modelItem => item.LastName)</td>
                                    <td>@Html.DisplayFor(modelItem => item.Email)</td>
                                    <td>@Html.DisplayFor(modelItem => item.Phone)</td>
                                    <td>@Html.DisplayFor(modelItem => item.Course)</td>
                                    <td>@Html.DisplayFor(modelItem => item.EnrollmentNumber)</td>
                                    <td>
                                        <div class="btn-group" role="group">
                                            <a asp-action="Details" asp-route-id="@item.Id"
                                               class="btn btn-sm btn-outline-primary" title="View Details">
                                                <i class="bi bi-eye"></i>
                                            </a>
                                            <a asp-action="Edit" asp-route-id="@item.Id"
                                               class="btn btn-sm btn-outline-warning" title="Edit">
                                                <i class="bi bi-pencil"></i>
                                            </a>
                                            <a asp-action="Delete" asp-route-id="@item.Id"
                                               class="btn btn-sm btn-outline-danger" title="Delete">
                                                <i class="bi bi-trash"></i>
                                            </a>
                                        </div>
                                    </td>
                                </tr>
                            }
                        </tbody>
                    </table>
                </div>
            }
            else
            {
                <div class="text-center py-4">
                    <i class="bi bi-people fs-1 text-muted"></i>
                    <h5 class="text-muted mt-3">No students found</h5>
                    <p class="text-muted">Get started by adding your first student.</p>
                    <a asp-action="Create" class="btn btn-primary">Add Student</a>
                </div>
            }
        </div>
    </div>
</div>

@section Scripts {
    <script>
        // Auto-submit course filter
        document.querySelector('select[name="course"]')?.addEventListener('change', function() {
            this.form.submit();
        });
    </script>
}
```

### Details View (Details.cshtml)

```cshtml
@model Student
@{
    ViewData["Title"] = "Student Details";
}

<div class="container">
    <div class="row mb-4">
        <div class="col">
            <h1>Student Details</h1>
        </div>
        <div class="col-auto">
            <a asp-action="Index" class="btn btn-outline-secondary">Back to List</a>
        </div>
    </div>

    <div class="row">
        <div class="col-md-8">
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0">Personal Information</h5>
                </div>
                <div class="card-body">
                    <dl class="row">
                        <dt class="col-sm-3">Full Name:</dt>
                        <dd class="col-sm-9">@Model.FirstName @Model.LastName</dd>

                        <dt class="col-sm-3">Email:</dt>
                        <dd class="col-sm-9">@Model.Email</dd>

                        <dt class="col-sm-3">Phone:</dt>
                        <dd class="col-sm-9">@Model.Phone</dd>

                        <dt class="col-sm-3">Date of Birth:</dt>
                        <dd class="col-sm-9">@Model.DateOfBirth.ToString("MMM dd, yyyy")</dd>

                        <dt class="col-sm-3">Course:</dt>
                        <dd class="col-sm-9">@Model.Course</dd>

                        <dt class="col-sm-3">Enrollment Number:</dt>
                        <dd class="col-sm-9">@Model.EnrollmentNumber</dd>

                        <dt class="col-sm-3">Status:</dt>
                        <dd class="col-sm-9">
                            @if (Model.IsActive)
                            {
                                <span class="badge bg-success">Active</span>
                            }
                            else
                            {
                                <span class="badge bg-secondary">Inactive</span>
                            }
                        </dd>

                        <dt class="col-sm-3">Created Date:</dt>
                        <dd class="col-sm-9">@Model.CreatedDate.ToString("MMM dd, yyyy HH:mm")</dd>

                        @if (Model.UpdatedDate.HasValue)
                        {
                            <dt class="col-sm-3">Last Updated:</dt>
                            <dd class="col-sm-9">@Model.UpdatedDate.Value.ToString("MMM dd, yyyy HH:mm")</dd>
                        }
                    </dl>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0">Actions</h5>
                </div>
                <div class="card-body">
                    <div class="d-grid gap-2">
                        <a asp-action="Edit" asp-route-id="@Model.Id" class="btn btn-warning">
                            <i class="bi bi-pencil"></i> Edit Student
                        </a>
                        <a asp-action="Delete" asp-route-id="@Model.Id" class="btn btn-danger">
                            <i class="bi bi-trash"></i> Delete Student
                        </a>
                        <a asp-action="Index" class="btn btn-outline-secondary">
                            <i class="bi bi-arrow-left"></i> Back to List
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
```

## 5. Use Cases

- **Student Information Systems**: Displaying complete student rosters with search and filtering
- **Employee Management**: Showing staff directories with department filtering
- **Product Catalogs**: E-commerce sites displaying all products with category filters
- **Inventory Systems**: Warehouse management showing all stock items
- **Patient Records**: Healthcare applications displaying patient lists
- **Financial Systems**: Banking apps showing transaction histories
- **Content Management**: Blog platforms displaying article lists

## 6. Mini Practice Task

1. **Basic Database Setup**:
   - Create a simple `Product` model with properties like Name, Price, Category, and Stock
   - Set up database connection and DbContext
   - Create a controller to display all products

2. **Enhanced Read Operations**:
   - Add search functionality to search products by name or category
   - Implement filtering by price range
   - Add sorting options (by name, price, stock)

3. **Advanced Features**:
   - Implement pagination for large datasets
   - Add export functionality to download data as CSV
   - Create detailed view for individual records
   - Add soft delete functionality (show only active records by default)