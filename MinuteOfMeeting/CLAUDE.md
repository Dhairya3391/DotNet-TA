# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with the **Minutes of Meeting (MOM) Management System** project.

## Project Overview

This is a complete, production-ready ASP.NET Core MVC application for managing organizational meetings, their schedules, participants, and documentation. Built as a Teaching Assistant (TA) reference implementation for course **2301CS412 - ASP.NET Core** at Darshan University.

**Purpose**: Demonstrate best practices for building a full-stack MVC application with authentication, CRUD operations, file uploads, dashboards, and reporting features using ADO.NET and stored procedures.

---

## Technology Stack

- **.NET Version**: .NET 8.0 (LTS)
- **Framework**: ASP.NET Core MVC
- **Database**: SQL Server (LocalDB or full SQL Server)
- **Data Access**: ADO.NET with Stored Procedures
- **Frontend**: Bootstrap 5.3, jQuery, Chart.js
- **Export Library**: EPPlus (Excel generation)
- **Authentication**: Custom session-based authentication

---

## Project Structure

```
MinuteOfMeeting/
├── Controllers/
│   ├── AccountController.cs           # Login, Logout, Register
│   ├── DashboardController.cs         # Main dashboard with statistics
│   ├── MeetingTypeController.cs       # Meeting type CRUD
│   ├── DepartmentController.cs        # Department CRUD
│   ├── MeetingVenueController.cs      # Venue CRUD
│   ├── StaffController.cs             # Staff/Member CRUD
│   ├── MeetingController.cs           # Meeting scheduling & management
│   └── MeetingMemberController.cs     # Attendance tracking
├── Models/
│   ├── MeetingType.cs                 # Meeting type entity with validations
│   ├── Department.cs                  # Department entity
│   ├── MeetingVenue.cs                # Venue entity
│   ├── Staff.cs                       # Staff/Member entity
│   ├── Meeting.cs                     # Meeting entity with relationships
│   ├── MeetingMember.cs               # Attendance/participation entity
│   ├── User.cs                        # User authentication entity
│   └── ViewModels/                    # Complex view models
│       ├── DashboardViewModel.cs
│       ├── MeetingFormViewModel.cs
│       └── AttendanceViewModel.cs
├── Views/
│   ├── Shared/
│   │   ├── _Layout.cshtml             # Master layout
│   │   ├── _Header.cshtml             # Header partial
│   │   ├── _Footer.cshtml             # Footer partial
│   │   └── _ValidationScriptsPartial.cshtml
│   ├── Account/
│   │   ├── Login.cshtml
│   │   └── Register.cshtml
│   ├── Dashboard/
│   │   └── Index.cshtml               # Main dashboard
│   ├── MeetingType/
│   │   ├── Index.cshtml               # List view
│   │   └── AddEdit.cshtml             # Add/Edit form
│   ├── Department/
│   ├── MeetingVenue/
│   ├── Staff/
│   ├── Meeting/
│   │   ├── Index.cshtml               # List with filters
│   │   ├── AddEdit.cshtml             # Scheduling form
│   │   ├── Details.cshtml             # Meeting details
│   │   └── Cancel.cshtml              # Cancellation form
│   └── MeetingMember/
│       ├── ManageAttendance.cshtml    # Attendance marking
│       └── AttendanceSummary.cshtml
├── wwwroot/
│   ├── css/
│   │   ├── site.css                   # Custom styles
│   │   └── dashboard.css
│   ├── js/
│   │   ├── site.js                    # Common JavaScript
│   │   └── dashboard.js               # Dashboard charts
│   ├── lib/                           # Bootstrap, jQuery, Chart.js
│   └── uploads/
│       └── meeting-docs/              # Uploaded meeting documents
├── DAL/                               # Data Access Layer
│   ├── DBHelper.cs                    # Database connection helper
│   ├── MeetingTypeDAL.cs              # Meeting type data operations
│   ├── DepartmentDAL.cs
│   ├── MeetingVenueDAL.cs
│   ├── StaffDAL.cs
│   ├── MeetingDAL.cs
│   ├── MeetingMemberDAL.cs
│   └── UserDAL.cs
├── Helpers/
│   ├── SessionHelper.cs               # Session management utilities
│   ├── FileUploadHelper.cs            # File upload utilities
│   └── ExportHelper.cs                # Excel export utilities
├── DatabaseScripts/
│   ├── 01_CreateDatabase.sql          # Database creation
│   ├── 02_CreateTables.sql            # All table schemas
│   ├── 03_SP_MeetingType.sql          # MeetingType stored procedures
│   ├── 04_SP_Department.sql
│   ├── 05_SP_MeetingVenue.sql
│   ├── 06_SP_Staff.sql
│   ├── 07_SP_Meeting.sql
│   ├── 08_SP_MeetingMember.sql
│   ├── 09_SP_User.sql
│   ├── 10_SP_Dashboard.sql            # Dashboard statistics queries
│   └── 99_SeedData.sql                # Sample data for testing
├── appsettings.json                   # Configuration & connection string
├── Program.cs                         # App configuration
└── CLAUDE.md                          # This file
```

---

## Database Architecture

### Database Name
**MOM_Database** (Minutes of Meeting Database)

### Tables Overview

#### 1. **MOM_MeetingType** (Master Table - Priority 3)
Stores different types of meetings (Review, Planning, Training, etc.)

| Column | Type | Constraints |
|--------|------|-------------|
| MeetingTypeID | INT | PK, Identity(1,1) |
| MeetingTypeName | NVARCHAR(100) | NOT NULL, Unique |
| Remarks | NVARCHAR(100) | NOT NULL |
| Created | DATETIME | NOT NULL, DEFAULT GETDATE() |
| Modified | DATETIME | NOT NULL |

**Stored Procedures:**
- `PR_MeetingType_SelectAll` - Get all meeting types
- `PR_MeetingType_SelectByPK` - Get by ID
- `PR_MeetingType_Insert` - Create new
- `PR_MeetingType_Update` - Update existing
- `PR_MeetingType_Delete` - Delete (check dependencies)
- `PR_MeetingType_SelectForDropdown` - Get ID/Name pairs

#### 2. **MOM_Department** (Master Table - Priority 4)
Stores organizational departments

| Column | Type | Constraints |
|--------|------|-------------|
| DepartmentID | INT | PK, Identity(1,1) |
| DepartmentName | NVARCHAR(100) | NOT NULL, Unique |
| Created | DATETIME | NOT NULL, DEFAULT GETDATE() |
| Modified | DATETIME | NOT NULL |

**Stored Procedures:**
- `PR_Department_SelectAll`
- `PR_Department_SelectByPK`
- `PR_Department_Insert`
- `PR_Department_Update`
- `PR_Department_Delete`
- `PR_Department_SelectForDropdown`

#### 3. **MOM_MeetingVenue** (Master Table - Priority 3)
Stores meeting venues (rooms, virtual links)

| Column | Type | Constraints |
|--------|------|-------------|
| MeetingVenueID | INT | PK, Identity(1,1) |
| MeetingVenueName | NVARCHAR(100) | NOT NULL, Unique |
| Created | DATETIME | NOT NULL, DEFAULT GETDATE() |
| Modified | DATETIME | NOT NULL |

**Stored Procedures:**
- `PR_MeetingVenue_SelectAll`
- `PR_MeetingVenue_SelectByPK`
- `PR_MeetingVenue_Insert`
- `PR_MeetingVenue_Update`
- `PR_MeetingVenue_Delete`
- `PR_MeetingVenue_SelectForDropdown`
- `PR_MeetingVenue_CheckAvailability` - Check venue conflicts

#### 4. **MOM_Staff** (Master Table - Priority 3)
Stores staff/member information

| Column | Type | Constraints |
|--------|------|-------------|
| StaffID | INT | PK, Identity(1,1) |
| DepartmentID | INT | FK → Department, NOT NULL |
| StaffName | NVARCHAR(50) | NOT NULL |
| MobileNo | NVARCHAR(20) | NOT NULL |
| EmailAddress | NVARCHAR(50) | NOT NULL, Unique |
| Remarks | NVARCHAR(250) | NULL |
| Created | DATETIME | NOT NULL, DEFAULT GETDATE() |
| Modified | DATETIME | NOT NULL |

**Stored Procedures:**
- `PR_Staff_SelectAll`
- `PR_Staff_SelectByPK`
- `PR_Staff_SelectByDepartment`
- `PR_Staff_Insert`
- `PR_Staff_Update`
- `PR_Staff_Delete`
- `PR_Staff_SelectForDropdown`
- `PR_Staff_CheckEmailExists`

#### 5. **MOM_Meetings** (Transaction Table - Priority 1)
Main meeting records with scheduling information

| Column | Type | Constraints |
|--------|------|-------------|
| MeetingID | INT | PK, Identity(1,1) |
| MeetingDate | DATETIME | NOT NULL |
| MeetingVenueID | INT | FK → MeetingVenue, NOT NULL |
| MeetingTypeID | INT | FK → MeetingType, NOT NULL |
| DepartmentID | INT | FK → Department, NOT NULL |
| MeetingDescription | NVARCHAR(250) | NULL |
| DocumentPath | NVARCHAR(250) | NULL |
| Created | DATETIME | NOT NULL, DEFAULT GETDATE() |
| Modified | DATETIME | NOT NULL |
| IsCancelled | BIT | NULL, DEFAULT 0 |
| CancellationDateTime | DATETIME | NULL |
| CancellationReason | NVARCHAR(250) | NULL |

**Stored Procedures:**
- `PR_Meeting_SelectAll`
- `PR_Meeting_SelectByPK`
- `PR_Meeting_SelectWithFilters` - Filter by date, type, venue, department
- `PR_Meeting_SelectUpcoming` - Future meetings
- `PR_Meeting_SelectCompleted` - Past meetings
- `PR_Meeting_SelectCancelled` - Cancelled meetings
- `PR_Meeting_Insert`
- `PR_Meeting_Update`
- `PR_Meeting_Cancel` - Mark as cancelled with reason
- `PR_Meeting_Delete`
- `PR_Meeting_CheckConflict` - Check venue/time conflicts

#### 6. **MOM_MeetingMember** (Junction Table - Priority 2)
Many-to-many relationship between Meetings and Staff (attendance tracking)

| Column | Type | Constraints |
|--------|------|-------------|
| MeetingMemberID | INT | PK, Identity(1,1) |
| MeetingID | INT | FK → Meeting, NOT NULL |
| StaffID | INT | FK → Staff, NOT NULL |
| IsPresent | BIT | NOT NULL, DEFAULT 0 |
| Remarks | NVARCHAR(250) | NULL |
| Created | DATETIME | NOT NULL, DEFAULT GETDATE() |
| Modified | DATETIME | NOT NULL |

**Unique Constraint:** (MeetingID, StaffID) - Prevent duplicate attendance records

**Stored Procedures:**
- `PR_MeetingMember_SelectByMeeting` - Get all attendees for a meeting
- `PR_MeetingMember_SelectByStaff` - Get all meetings for a staff member
- `PR_MeetingMember_Insert` - Add attendee to meeting
- `PR_MeetingMember_UpdateAttendance` - Mark present/absent
- `PR_MeetingMember_Delete` - Remove attendee
- `PR_MeetingMember_BulkInsert` - Add multiple attendees (Table-Valued Parameter)

#### 7. **MOM_User** (Authentication Table - Custom)
User authentication and session management

| Column | Type | Constraints |
|--------|------|-------------|
| UserID | INT | PK, Identity(1,1) |
| StaffID | INT | FK → Staff, NULL (optional link) |
| Username | NVARCHAR(50) | NOT NULL, Unique |
| Password | NVARCHAR(255) | NOT NULL (hashed) |
| Role | NVARCHAR(20) | NOT NULL (Admin/Organizer/Staff) |
| IsActive | BIT | NOT NULL, DEFAULT 1 |
| LastLogin | DATETIME | NULL |
| Created | DATETIME | NOT NULL, DEFAULT GETDATE() |

**Stored Procedures:**
- `PR_User_SelectByUsername` - Authentication
- `PR_User_Insert` - Registration
- `PR_User_UpdateLastLogin` - Track login time
- `PR_User_UpdatePassword` - Change password

#### 8. **Dashboard Views/Procedures**
- `PR_Dashboard_GetStatistics` - Overall counts
- `PR_Dashboard_GetUpcomingMeetings` - Next 10 meetings
- `PR_Dashboard_GetRecentMeetings` - Last 10 meetings
- `PR_Dashboard_GetMeetingsByType` - Count by type (for chart)
- `PR_Dashboard_GetMeetingsByDepartment` - Count by department
- `PR_Dashboard_GetMostActiveDepartments` - Top 5 departments
- `PR_Dashboard_GetStaffParticipation` - Top participants

---

## Key Implementation Patterns

### 1. Data Access Layer (DAL)

**Pattern:** Repository-like pattern with ADO.NET

```csharp
// Example: MeetingTypeDAL.cs
public class MeetingTypeDAL
{
    // Get all meeting types
    public static DataTable SelectAll()
    {
        using (SqlConnection conn = DBHelper.GetConnection())
        {
            SqlCommand cmd = new SqlCommand("PR_MeetingType_SelectAll", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            SqlDataAdapter adapter = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            adapter.Fill(dt);
            return dt;
        }
    }

    // Insert with output parameter
    public static int Insert(MeetingType model)
    {
        using (SqlConnection conn = DBHelper.GetConnection())
        {
            SqlCommand cmd = new SqlCommand("PR_MeetingType_Insert", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@MeetingTypeName", model.MeetingTypeName);
            cmd.Parameters.AddWithValue("@Remarks", model.Remarks);
            cmd.Parameters.AddWithValue("@Created", DateTime.Now);
            cmd.Parameters.AddWithValue("@Modified", DateTime.Now);

            SqlParameter outputParam = new SqlParameter("@MeetingTypeID", SqlDbType.Int);
            outputParam.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(outputParam);

            conn.Open();
            cmd.ExecuteNonQuery();
            return Convert.ToInt32(outputParam.Value);
        }
    }
}
```

**DBHelper Pattern:**
```csharp
public static class DBHelper
{
    private static string ConnectionString
    {
        get
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build()
                .GetConnectionString("DefaultConnection");
        }
    }

    public static SqlConnection GetConnection()
    {
        return new SqlConnection(ConnectionString);
    }
}
```

### 2. Model Validation with Data Annotations

```csharp
public class Staff
{
    public int StaffID { get; set; }

    [Required(ErrorMessage = "Department is required")]
    [Display(Name = "Department")]
    public int DepartmentID { get; set; }

    [Required(ErrorMessage = "Staff name is required")]
    [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    [Display(Name = "Staff Name")]
    public string StaffName { get; set; }

    [Required(ErrorMessage = "Mobile number is required")]
    [RegularExpression(@"^\d{10}$", ErrorMessage = "Enter valid 10-digit mobile number")]
    [Display(Name = "Mobile Number")]
    public string MobileNo { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Enter valid email address")]
    [Display(Name = "Email Address")]
    public string EmailAddress { get; set; }

    [StringLength(250)]
    public string Remarks { get; set; }

    public DateTime Created { get; set; }
    public DateTime Modified { get; set; }

    // Navigation property (not stored in DB)
    public string DepartmentName { get; set; }
}
```

### 3. Session-Based Authentication

**SessionHelper.cs:**
```csharp
public static class SessionHelper
{
    public static void SetUserSession(HttpContext context, User user)
    {
        context.Session.SetInt32("UserID", user.UserID);
        context.Session.SetString("Username", user.Username);
        context.Session.SetString("Role", user.Role);
    }

    public static bool IsUserLoggedIn(HttpContext context)
    {
        return context.Session.GetInt32("UserID") != null;
    }

    public static int GetUserID(HttpContext context)
    {
        return context.Session.GetInt32("UserID") ?? 0;
    }

    public static void ClearSession(HttpContext context)
    {
        context.Session.Clear();
    }
}
```

**Custom Authorization Attribute:**
```csharp
public class AuthorizeSession : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        if (!SessionHelper.IsUserLoggedIn(context.HttpContext))
        {
            context.Result = new RedirectToActionResult("Login", "Account", null);
        }
    }
}

// Usage in controllers
[AuthorizeSession]
public class DashboardController : Controller
{
    // All actions require login
}
```

### 4. Controller Pattern with Error Handling

```csharp
public class MeetingTypeController : Controller
{
    [HttpGet]
    public IActionResult Index()
    {
        try
        {
            DataTable dt = MeetingTypeDAL.SelectAll();
            return View(dt);
        }
        catch (Exception ex)
        {
            TempData["Error"] = "Error loading meeting types: " + ex.Message;
            return View(new DataTable());
        }
    }

    [HttpGet]
    public IActionResult AddEdit(int? id)
    {
        MeetingType model = new MeetingType();

        if (id.HasValue)
        {
            DataTable dt = MeetingTypeDAL.SelectByPK(id.Value);
            if (dt.Rows.Count > 0)
            {
                DataRow row = dt.Rows[0];
                model.MeetingTypeID = Convert.ToInt32(row["MeetingTypeID"]);
                model.MeetingTypeName = row["MeetingTypeName"].ToString();
                model.Remarks = row["Remarks"].ToString();
            }
        }

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Save(MeetingType model)
    {
        if (!ModelState.IsValid)
        {
            return View("AddEdit", model);
        }

        try
        {
            if (model.MeetingTypeID == 0)
            {
                MeetingTypeDAL.Insert(model);
                TempData["Success"] = "Meeting type added successfully";
            }
            else
            {
                model.Modified = DateTime.Now;
                MeetingTypeDAL.Update(model);
                TempData["Success"] = "Meeting type updated successfully";
            }

            return RedirectToAction("Index");
        }
        catch (SqlException ex)
        {
            if (ex.Number == 2627) // Unique constraint violation
            {
                ModelState.AddModelError("MeetingTypeName", "This meeting type already exists");
            }
            else
            {
                ModelState.AddModelError("", "Database error: " + ex.Message);
            }

            return View("AddEdit", model);
        }
    }
}
```

### 5. File Upload Pattern

**FileUploadHelper.cs:**
```csharp
public static class FileUploadHelper
{
    public static async Task<string> UploadFile(IFormFile file, string folder)
    {
        if (file == null || file.Length == 0)
            return null;

        // Validate file type
        string[] allowedExtensions = { ".pdf", ".doc", ".docx", ".txt" };
        string extension = Path.GetExtension(file.FileName).ToLower();

        if (!allowedExtensions.Contains(extension))
            throw new Exception("Invalid file type. Only PDF, DOC, DOCX, TXT allowed.");

        // Validate file size (5MB max)
        if (file.Length > 5 * 1024 * 1024)
            throw new Exception("File size cannot exceed 5MB");

        // Generate unique filename
        string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
        string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", folder);

        // Ensure directory exists
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        string filePath = Path.Combine(uploadsFolder, fileName);

        // Save file
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        return $"/uploads/{folder}/{fileName}";
    }

    public static void DeleteFile(string filePath)
    {
        if (!string.IsNullOrEmpty(filePath))
        {
            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));
            if (File.Exists(fullPath))
                File.Delete(fullPath);
        }
    }
}
```

### 6. Excel Export with EPPlus

**ExportHelper.cs:**
```csharp
using OfficeOpenXml;

public static class ExportHelper
{
    public static byte[] ExportToExcel(DataTable dt, string sheetName)
    {
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

        using (var package = new ExcelPackage())
        {
            var worksheet = package.Workbook.Worksheets.Add(sheetName);

            // Add headers
            for (int i = 0; i < dt.Columns.Count; i++)
            {
                worksheet.Cells[1, i + 1].Value = dt.Columns[i].ColumnName;
                worksheet.Cells[1, i + 1].Style.Font.Bold = true;
            }

            // Add data
            for (int row = 0; row < dt.Rows.Count; row++)
            {
                for (int col = 0; col < dt.Columns.Count; col++)
                {
                    worksheet.Cells[row + 2, col + 1].Value = dt.Rows[row][col];
                }
            }

            // Auto-fit columns
            worksheet.Cells.AutoFitColumns();

            return package.GetAsByteArray();
        }
    }
}

// Usage in controller
public IActionResult ExportToExcel()
{
    DataTable dt = MeetingTypeDAL.SelectAll();
    byte[] fileBytes = ExportHelper.ExportToExcel(dt, "MeetingTypes");

    return File(fileBytes,
                "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                $"MeetingTypes_{DateTime.Now:yyyyMMdd}.xlsx");
}
```

### 7. Dashboard with Charts

**Dashboard Controller:**
```csharp
public IActionResult Index()
{
    DashboardViewModel model = new DashboardViewModel
    {
        TotalMeetings = DashboardDAL.GetTotalMeetings(),
        UpcomingMeetings = DashboardDAL.GetUpcomingMeetingsCount(),
        CompletedMeetings = DashboardDAL.GetCompletedMeetingsCount(),
        CancelledMeetings = DashboardDAL.GetCancelledMeetingsCount(),
        RecentMeetings = DashboardDAL.GetRecentMeetings(10),
        MeetingsByType = DashboardDAL.GetMeetingsByType()
    };

    return View(model);
}

public IActionResult GetChartData()
{
    var data = DashboardDAL.GetMeetingsByType();
    return Json(data);
}
```

**Dashboard View with Chart.js:**
```html
<canvas id="meetingTypeChart"></canvas>

<script>
fetch('/Dashboard/GetChartData')
    .then(response => response.json())
    .then(data => {
        new Chart(document.getElementById('meetingTypeChart'), {
            type: 'bar',
            data: {
                labels: data.map(x => x.MeetingTypeName),
                datasets: [{
                    label: 'Number of Meetings',
                    data: data.map(x => x.Count),
                    backgroundColor: 'rgba(54, 162, 235, 0.2)',
                    borderColor: 'rgba(54, 162, 235, 1)',
                    borderWidth: 1
                }]
            }
        });
    });
</script>
```

### 8. Conflict Detection

```csharp
public static bool CheckVenueConflict(int venueId, DateTime meetingDate, int? excludeMeetingId = null)
{
    using (SqlConnection conn = DBHelper.GetConnection())
    {
        SqlCommand cmd = new SqlCommand("PR_Meeting_CheckConflict", conn);
        cmd.CommandType = CommandType.StoredProcedure;

        cmd.Parameters.AddWithValue("@MeetingVenueID", venueId);
        cmd.Parameters.AddWithValue("@MeetingDate", meetingDate);
        cmd.Parameters.AddWithValue("@ExcludeMeetingID", excludeMeetingId ?? (object)DBNull.Value);

        SqlParameter outputParam = new SqlParameter("@HasConflict", SqlDbType.Bit);
        outputParam.Direction = ParameterDirection.Output;
        cmd.Parameters.Add(outputParam);

        conn.Open();
        cmd.ExecuteNonQuery();

        return Convert.ToBoolean(outputParam.Value);
    }
}

// Stored procedure logic
/*
CREATE PROCEDURE PR_Meeting_CheckConflict
    @MeetingVenueID INT,
    @MeetingDate DATETIME,
    @ExcludeMeetingID INT = NULL,
    @HasConflict BIT OUTPUT
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM MOM_Meetings
        WHERE MeetingVenueID = @MeetingVenueID
        AND CAST(MeetingDate AS DATE) = CAST(@MeetingDate AS DATE)
        AND DATEPART(HOUR, MeetingDate) = DATEPART(HOUR, @MeetingDate)
        AND IsCancelled = 0
        AND (@ExcludeMeetingID IS NULL OR MeetingID != @ExcludeMeetingID)
    )
        SET @HasConflict = 1
    ELSE
        SET @HasConflict = 0
END
*/
```

---

## Configuration & Setup

### appsettings.json

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=.;Database=MOM_Database;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=true"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*",
  "FileUpload": {
    "MaxSizeInMB": 5,
    "AllowedExtensions": [".pdf", ".doc", ".docx", ".txt"]
  }
}
```

### Program.cs Configuration

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllersWithViews();

// Session configuration
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

var app = builder.Build();

// Configure HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession(); // Must be before UseAuthorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
```

---

## Database Setup Instructions

### Step 1: Create Database
```bash
# Run in SQL Server Management Studio or Azure Data Studio
# Execute: DatabaseScripts/01_CreateDatabase.sql
```

### Step 2: Create Tables
```bash
# Execute: DatabaseScripts/02_CreateTables.sql
```

### Step 3: Create Stored Procedures
```bash
# Execute in order:
# 03_SP_MeetingType.sql
# 04_SP_Department.sql
# 05_SP_MeetingVenue.sql
# 06_SP_Staff.sql
# 07_SP_Meeting.sql
# 08_SP_MeetingMember.sql
# 09_SP_User.sql
# 10_SP_Dashboard.sql
```

### Step 4: Load Sample Data (Optional)
```bash
# Execute: 99_SeedData.sql
```

---

## Running the Application

### Prerequisites
- .NET 8 SDK installed
- SQL Server or SQL Server Express LocalDB
- Visual Studio 2022 or VS Code

### Commands

```bash
# Restore packages
dotnet restore

# Build project
dotnet build

# Run application
dotnet run

# Run with watch (hot reload)
dotnet watch run

# Application will be available at:
# https://localhost:5001
# http://localhost:5000
```

### Default Login Credentials (from seed data)
- **Admin**: username: `admin`, password: `admin123`
- **Organizer**: username: `organizer`, password: `org123`
- **Staff**: username: `staff`, password: `staff123`

---

## Key Features Implementation

### 1. Master Data Management
- **MeetingType**: Basic CRUD with duplicate prevention
- **Department**: Basic CRUD with duplicate prevention
- **MeetingVenue**: CRUD + availability checking
- **Staff**: CRUD with email uniqueness validation

### 2. Meeting Management
- **Scheduling**: Date, time, venue, type, department selection
- **Conflict Detection**: Prevent double-booking venues
- **File Upload**: Attach meeting documents (PDF, DOC, DOCX)
- **Cancellation**: Track cancellation with reason and timestamp
- **Filtering**: Filter by date range, type, venue, department
- **Search**: Search by description or participant name

### 3. Attendance Tracking
- **Add Participants**: Select multiple staff members for a meeting
- **Mark Attendance**: Track who attended vs. invited
- **Attendance Summary**: View participation statistics per staff
- **Meeting History**: View all meetings a staff member attended

### 4. Dashboard & Analytics
- **Quick Stats**: Total, upcoming, completed, cancelled counts
- **Recent Meetings**: Last 10 meetings with details
- **Upcoming Meetings**: Next 10 scheduled meetings
- **Charts**:
  - Meetings by Type (Bar chart)
  - Meetings by Department (Pie chart)
  - Monthly Meeting Trend (Line chart)
- **Top Participants**: Most active staff members
- **Busiest Departments**: Departments with most meetings

### 5. Reporting & Export
- **Excel Export**: Export any list to Excel (.xlsx)
- **Filters**: Apply filters before exporting
- **Multiple Reports**:
  - All meetings report
  - Department-wise meeting report
  - Staff participation report
  - Meeting type summary

### 6. Authentication & Authorization
- **Registration**: New user signup with role selection
- **Login**: Session-based authentication
- **Logout**: Clear session and redirect
- **Session Timeout**: 30 minutes idle timeout
- **Role-based Access**: Admin sees all, Organizer can schedule, Staff can view

---

## Common Development Tasks

### Adding a New Entity

1. **Create Model** in `Models/`
2. **Create DAL class** in `DAL/`
3. **Create stored procedures** in `DatabaseScripts/`
4. **Create controller** in `Controllers/`
5. **Create views** in `Views/[EntityName]/`
6. **Add navigation link** in `_Layout.cshtml`

### Adding Validation

```csharp
// In Model
[Required(ErrorMessage = "Field is required")]
[StringLength(100, ErrorMessage = "Max 100 characters")]
public string PropertyName { get; set; }

// In Controller
if (!ModelState.IsValid)
{
    return View(model);
}
```

### Adding Dropdown in View

```html
<!-- In Controller -->
ViewBag.Departments = new SelectList(DepartmentDAL.SelectForDropdown(), "DepartmentID", "DepartmentName");

<!-- In View -->
@Html.DropDownListFor(m => m.DepartmentID,
    (SelectList)ViewBag.Departments,
    "-- Select Department --",
    new { @class = "form-control" })
```

### Adding Search Functionality

```csharp
// Controller
public IActionResult Index(string searchText)
{
    DataTable dt = EntityDAL.Search(searchText);
    ViewBag.SearchText = searchText;
    return View(dt);
}

// View
<form method="get">
    <input type="text" name="searchText" value="@ViewBag.SearchText" />
    <button type="submit">Search</button>
</form>
```

---

## Testing Checklist

### Master Tables
- [ ] Create new record
- [ ] Update existing record
- [ ] Delete record (check dependencies)
- [ ] Duplicate name validation
- [ ] Required field validation
- [ ] Export to Excel

### Meetings
- [ ] Schedule new meeting
- [ ] Update meeting details
- [ ] Cancel meeting with reason
- [ ] Venue conflict detection
- [ ] File upload (PDF, DOC)
- [ ] View meeting details
- [ ] Filter by date, type, venue, department
- [ ] Export meeting list

### Attendance
- [ ] Add multiple participants
- [ ] Mark attendance (present/absent)
- [ ] View attendance summary
- [ ] Prevent duplicate attendance records
- [ ] View staff meeting history

### Dashboard
- [ ] Statistics display correctly
- [ ] Charts render properly
- [ ] Recent/upcoming meetings list
- [ ] Data updates in real-time

### Authentication
- [ ] Login with valid credentials
- [ ] Login fails with invalid credentials
- [ ] Registration creates new user
- [ ] Session persists across pages
- [ ] Logout clears session
- [ ] Unauthorized access redirects to login

### Export
- [ ] Excel file downloads
- [ ] All columns included
- [ ] Data matches screen
- [ ] Filters applied to export

---

## Common Errors & Solutions

### Error: Connection String Not Found
**Solution:** Check `appsettings.json` for correct connection string format and database name.

### Error: Stored Procedure Not Found
**Solution:** Ensure all SQL scripts in `DatabaseScripts/` are executed in order.

### Error: Session Null Reference
**Solution:** Add `app.UseSession()` in `Program.cs` before `app.UseAuthorization()`.

### Error: File Upload Fails
**Solution:** Ensure `wwwroot/uploads/meeting-docs/` directory exists and has write permissions.

### Error: Chart Not Rendering
**Solution:** Verify Chart.js library is included in `_Layout.cshtml` and data format is correct.

### Error: Unique Constraint Violation
**Solution:** Catch `SqlException` with `Number == 2627` and show user-friendly message.

---

## Best Practices for Students

1. **Always use stored procedures** - Never write inline SQL queries
2. **Use `using` statements** for database connections - Ensures proper disposal
3. **Validate on both client and server** - Client-side for UX, server-side for security
4. **Handle exceptions gracefully** - Show user-friendly error messages
5. **Use parameterized queries** - Prevent SQL injection attacks
6. **Follow naming conventions** - PascalCase for classes/methods, camelCase for variables
7. **Comment complex logic** - Help others understand your code
8. **Test edge cases** - Empty data, duplicate entries, invalid inputs
9. **Keep controllers thin** - Business logic in DAL, not controllers
10. **Use TempData for messages** - Show success/error messages after redirects

---

## Project Evaluation Criteria

### Database Design (20%)
- Proper normalization (3NF)
- Correct primary/foreign key relationships
- Appropriate data types
- Unique constraints where needed
- Default values and audit fields

### Stored Procedures (20%)
- All CRUD operations via SPs
- Parameterized to prevent SQL injection
- Output parameters for Insert operations
- Error handling in SPs
- Efficient queries with proper indexing

### Application Logic (25%)
- Clean MVC architecture
- Proper separation of concerns (DAL, Controllers, Views)
- Error handling with try-catch
- Session management implemented
- File upload functionality working

### User Interface (15%)
- Responsive design with Bootstrap
- User-friendly forms with validation
- Clear navigation
- Consistent layout across pages
- Proper use of TempData for messages

### Advanced Features (20%)
- Dashboard with statistics and charts
- Excel export functionality
- Conflict detection for scheduling
- Attendance tracking working correctly
- Search and filter functionality

### Code Quality (10%)
- Well-commented code
- Consistent naming conventions
- No hardcoded values
- Proper use of ViewBag/TempData
- Follows ASP.NET Core best practices

---

## Resources & References

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [ADO.NET Tutorial](https://learn.microsoft.com/en-us/dotnet/framework/data/adonet/)
- [Bootstrap 5 Documentation](https://getbootstrap.com/docs/5.3/)
- [Chart.js Documentation](https://www.chartjs.org/docs/latest/)
- [EPPlus Documentation](https://github.com/EPPlusSoftware/EPPlus)
- [SQL Server Stored Procedures](https://learn.microsoft.com/en-us/sql/relational-databases/stored-procedures/stored-procedures-database-engine)

---

## Version History

- **v1.0** (2025-01-19): Initial complete implementation
  - All 6 tables with full CRUD
  - Authentication system
  - Dashboard with charts
  - Excel export
  - File upload
  - Conflict detection
  - Attendance tracking

---

## Support & Contact

For questions or issues with this project:
- Review this CLAUDE.md file thoroughly
- Check the code comments in relevant files
- Refer to the project documentation in `project.md`
- Contact TA or instructor during lab hours

---

*This project is designed to be a comprehensive learning resource for ASP.NET Core MVC development with ADO.NET and stored procedures. Every feature is implemented with best practices and extensive comments to aid student understanding.*
