# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

This repository contains lab assignments for course **2301CS412 - ASP.NET Core** (Semester IV, 2025-26) at Darshan University's Computer Science and Engineering Department. The repository serves as Teaching Assistant (TA) work, containing solutions and reference implementations for 30 lab assignments.

## Course Structure

The course is divided into distinct phases:

1. **Labs 1-6**: C# Fundamentals (Console Applications)
   - Variables, Data Types, Operators
   - Conditions & Looping
   - Classes, Objects, Constructors & Exception Handling
   - Method Overloading, Overriding & Access Modifiers
   - Inheritance, Interface & Abstraction
   - Collection Classes & Strings

2. **Labs 7-8**: Frontend Basics
   - Bootstrap static web design
   - Admin theme conversion

3. **Labs 9-30**: ASP.NET Core MVC Development
   - Razor syntax, ViewBag/ViewData/TempData
   - Areas & IActionResult
   - Database operations & stored procedures
   - Layouts, routing, and HTML helpers
   - Tag helpers, partial views, model binding
   - File uploads, dashboards, CRUD operations
   - Authentication and Excel export

## Technology Stack

- **.NET Version**: .NET 8 (Latest LTS)
- **Primary Frameworks**: Console Apps (Labs 1-6), ASP.NET Core MVC (Labs 9-30)
- **Database**: SQL Server (with ADO.NET and stored procedures)
- **Frontend**: Bootstrap, Razor views, HTML helpers

## Repository Organization

Programs are organized by lab number in folders named `Lab01/`, `Lab02/`, etc. Each lab folder contains:
- Individual console applications (Labs 1-6)
- MVC projects or components (Labs 9-30)
- A README or comments explaining which problem each file solves

### Naming Convention
- Console apps: `Lab{XX}_Problem{YY}_{Description}/` (e.g., `Lab01_Problem01_PrintDetails/`)
- Problem difficulty indicated by letters (A = Easy, B = Medium, C = Complex)

## Common Commands

### Creating New Projects

**Console Application** (Labs 1-6):
```bash
dotnet new console -n Lab{XX}_Problem{YY}_{Description}
cd Lab{XX}_Problem{YY}_{Description}
dotnet run
```

**ASP.NET Core MVC Application** (Labs 9-30):
```bash
dotnet new mvc -n Lab{XX}_{Description}
cd Lab{XX}_{Description}
dotnet run
```

### Running Projects

**Single Console App**:
```bash
cd Lab01/Lab01_Problem01_PrintDetails
dotnet run
```

**MVC Application**:
```bash
cd Lab13/Lab13_LayoutPage
dotnet run
# Application typically runs on https://localhost:5001
```

### Building and Testing

```bash
# Build specific project
dotnet build Lab01/Lab01_Problem01_PrintDetails

# Build all projects (from root)
find . -name "*.csproj" -exec dotnet build {} \;

# Clean build artifacts
dotnet clean
```

### Database Operations

**Connection String Location**: `appsettings.json` in MVC projects

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=AddressBook;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

**Running SQL Scripts**:
- Stored procedures are typically in `DatabaseScripts/` folder within relevant lab folders
- Use SQL Server Management Studio (SSMS) or Azure Data Studio to execute

## Key Implementation Patterns

### Database Access (Labs 11-12, 24-30)
- Use **ADO.NET** with stored procedures (primary approach for this course)
- Connection management: Use `SqlConnection` with `using` statements
- Command execution: Use `SqlCommand` with parameters to prevent SQL injection
- Data retrieval: Use `SqlDataReader` or `DataTable` depending on the requirement

### Model-View-Controller Pattern (Labs 9-30)
- **Models**: Data classes with data annotations for validation (Lab 18)
- **Views**: Razor views (.cshtml) with HTML helpers or tag helpers
- **Controllers**: Handle HTTP requests, interact with database, return IActionResult

### Data Passing Techniques (Lab 9)
- **ViewBag**: Dynamic property bag for simple data
- **ViewData**: Dictionary-based, requires casting
- **TempData**: Persists data across redirects (single request)
- **Strongly-typed models**: Preferred approach for complex data

### Form Handling (Labs 15, 20)
- Use HTML Helpers (`Html.BeginForm`, `Html.TextBoxFor`) or Tag Helpers (`<input asp-for="">`)
- Server-side validation with data annotations (Lab 19)
- FormCollection for loosely-typed form data
- Bind attribute for selective model binding

## Database Schema Reference

**AddressBook Database** (Labs 11-12, 24-30):
```sql
-- Core Tables
Country (CountryID, CountryName, CountryCode)
State (StateID, StateName, StateCode, CountryID)
City (CityID, CityName, CityCode, StateID)

-- Required Stored Procedures
PR_Country_SelectAll, PR_Country_SelectByPK
PR_Country_Insert, PR_Country_Update, PR_Country_Delete
-- Similar procedures for State and City tables
```

## Important Considerations

### For Console Applications (Labs 1-6)
- Each problem should be a separate runnable console application
- Include clear console output with labels
- Handle invalid input gracefully with try-catch blocks
- Use meaningful variable names and add comments for complex logic

### For MVC Applications (Labs 9-30)
- Follow MVC folder structure: Models/, Views/, Controllers/
- Use Areas for modular organization (Lab 10: Admin, Manager, Employee)
- Layout pages (`_Layout.cshtml`) should be in Views/Shared/
- Partial views (`_PartialViewName.cshtml`) with underscore prefix
- Static files (CSS, JS, images) in wwwroot/

### Security Best Practices
- **Never hardcode credentials** in source code
- Use **parameterized queries** or stored procedures to prevent SQL injection
- Implement proper **input validation** (client-side and server-side)
- Use **data annotations** for model validation
- Store connection strings in `appsettings.json`, not in code

### Common Pitfalls to Avoid
- Don't forget `[HttpPost]` attribute on controller actions that handle form submissions
- Always dispose database connections (use `using` statements)
- Validate model state with `ModelState.IsValid` before processing
- Include proper error handling in controllers (try-catch blocks)
- Test file upload functionality with appropriate file size limits

## Lab-Specific Notes

### Lab 1-6: C# Console Applications
- Each problem is a separate console application
- Lab 1: Variables, data types, operators (temperature conversion, salary calculation, shape area/perimeter, currency conversion, discount calculation, cab fare system)
- Lab 2: Conditions & loops (multiplication tables, character counting, grading, factorial, prime numbers, palindrome, Fibonacci)
- Lab 3: Classes, objects, constructors & exception handling (Student, Rectangle, BankAccount, ShoppingCart, CarRental, FlightTicket classes)
- Lab 4: Method overloading/overriding & access modifiers (Calculator, Employee, Animal/Dog/Cat hierarchy, Shape/Circle/Rectangle/Triangle)
- Lab 5: Inheritance, interfaces & abstraction (Vehicle hierarchy, Appliance abstraction, IPrintable/IMovable/ISound interfaces, Payment system, Employee/Manager/Developer, vehicle rental)
- Lab 6: Collections & strings (Stack for recent tasks, Queue for customer service, vowel/consonant counting, palindrome check, word frequency, shopping list, Dictionary word count, HashSet for emails, library borrowing with Dictionary<string, Queue<string>>, hospital queues)

### Lab 7-8: Bootstrap & Theme Conversion
- Lab 7: Design static web pages using Bootstrap
- Lab 8: Multiple page admin theme conversion with required pages
- Use Bootstrap 5.x for responsive design
- Ensure mobile responsiveness for all pages
- Admin themes typically from free template sources (AdminLTE, SB Admin, etc.)

### Lab 9-10: Razor Basics & Areas
- Lab 9: Razor syntax (table of 5, student SPI table), ViewBag/ViewData/TempData demonstrations
- Lab 10: Areas implementation (Admin, Manager, Employee), IActionResult types (View, Content, JSON, File, Redirect, Status Code)

### Lab 11-12: Stored Procedures
- Lab 11: Create AddressBook database, SelectAll and SelectByPK procedures for Country/State/City. Filter by city name, display cities by state, display states with city count by country
- Lab 12: Insert, Update, Delete procedures for Country/State/City and custom table (6-7 columns)
- All CRUD operations must use stored procedures
- Use `SqlParameter` to pass values safely
- Return appropriate status codes from procedures

### Lab 13-14: Layouts & Routing
- Lab 13: Design layout page with header and footer views
- Lab 14: Design list and add/edit pages with attribute routing
- Master layout with header/footer partials
- Use attribute routing: `[Route("controller/action")]`
- Organize routes logically for RESTful patterns

### Lab 15-17: HTML Helpers, Tag Helpers & Partial Views
- Lab 15: Student/Employee registration forms using standard and strongly typed HTML helpers, Job inquiry form
- Lab 16: Custom tag helpers for alerts (Success, Warning, Info) and email link generation
- Lab 17: Partial views for reusable sections (header, footer, navigation bar)

### Lab 18-19: Models & Validation
- Lab 18: Model classes with data annotations
- Lab 19: Server-side validation for all submit requests
- Use data annotations for validation rules

### Lab 20-21: Form Handling & File Upload
- Lab 20: Feedback form using FormCollection (Id, Name, Email, Subject, Observation), [Bind] attribute for Student model (Id, Name, Email, Password)
- Lab 21: File upload for resume display and profile picture upload
- Store uploaded files in `wwwroot/uploads/`
- Validate file type and size before saving
- Generate unique filenames to prevent overwrites

### Lab 22-23: Dashboards
- Lab 22: Dynamic dashboard with summaries and statistics in tabular format
- Lab 23: Dynamic dashboard with data visualization using charts
- Use Chart.js or similar library for data visualization
- Fetch data from database and format as JSON for charts
- Display statistics in card-based layouts

### Lab 24-28: Full CRUD Implementation
- Lab 24: Database connectivity and display all records
- Lab 25: Insert and delete functionality
- Lab 26: Update functionality
- Lab 27: Dropdown functionality
- Lab 28: Search functionality for all list pages

### Lab 29: Authentication
- Lab 29A: Login functionality
- Lab 29B: User registration functionality
- Use session-based authentication (simple approach)
- Store user session data securely
- Implement logout functionality to clear sessions

### Lab 30: Excel Export
- Lab 30A: Search functionality for all list pages
- Lab 30B: Export table data to Excel
- Use libraries like EPPlus or ClosedXML
- Export List/Grid data to Excel format (.xlsx)
- Include proper headers and formatting

## Grading Criteria (for student submissions)

Each lab is categorized by difficulty:
- **Type A**: Basic implementation, fundamental concepts
- **Type B**: Intermediate complexity, multiple features
- **Type C**: Advanced scenarios, integration of multiple concepts

Students should ensure:
1. Code compiles without errors
2. Functionality matches lab requirements exactly
3. Proper error handling is implemented
4. Code is well-commented and readable
5. Database scripts are included for DB-related labs

## Reference Documentation

- [ASP.NET Core Documentation](https://learn.microsoft.com/en-us/aspnet/core/)
- [C# Programming Guide](https://learn.microsoft.com/en-us/dotnet/csharp/)
- [Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/) - Alternative to ADO.NET
- [Bootstrap 5 Documentation](https://getbootstrap.com/docs/5.0/)
- [Razor Syntax Reference](https://learn.microsoft.com/en-us/aspnet/core/mvc/views/razor)
