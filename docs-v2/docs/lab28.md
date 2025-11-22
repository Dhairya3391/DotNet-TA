---
title: Lab 28 - Implementation of Search Functionality
sidebar_position: 28
---
# Implementation of Search Functionality

## 1. Description
Search functionality allows users to find specific records by filtering data based on various criteria. It can range from simple text-based searches to complex multi-field filtering with operators, ranges, and sorting. Search functionality enhances user experience by helping users quickly locate relevant information in large datasets.

## 2. Why It Is Important
Search is essential for usability in data-driven applications. Users need to efficiently find specific records without manually browsing through pages of data. Good search functionality improves productivity, reduces frustration, and makes applications more accessible and user-friendly, especially when dealing with large datasets.

## 3. Real-World Examples
- Student management system searching by name, ID, course, or enrollment date
- E-commerce platform searching products by name, category, price range, or brand
- Healthcare application searching patients by name, ID, medical record, or appointment date
- Employee directory searching by name, department, position, or location
- Inventory system searching products by SKU, name, category, or stock status
- Library catalog searching books by title, author, ISBN, or subject

## 4. Syntax & Explanation

### Search Models and ViewModels

```csharp
// Base search parameters model
public class SearchParameters
{
    public string SearchTerm { get; set; }
    public string SearchField { get; set; } // Which field to search in
    public string SortBy { get; set; } = "Name";
    public string SortDirection { get; set; } = "Asc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

// Advanced search with multiple filters
public class AdvancedSearchParameters : SearchParameters
{
    // Date range filters
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    // Numeric range filters
    public decimal? MinAmount { get; set; }
    public decimal? MaxAmount { get; set; }

    // Dropdown filters
    public string SelectedCourse { get; set; }
    public string SelectedDepartment { get; set; }
    public string SelectedStatus { get; set; }

    // Boolean filters
    public bool? IsActive { get; set; }
    public bool HasEnrollments { get; set; }

    // Multi-select filters
    public List<string> SelectedSkills { get; set; } = new();
}

// Paginated result model
public class PaginatedResult<T>
{
    public List<T> Data { get; set; } = new();
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalCount { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
    public bool HasPreviousPage => CurrentPage > 1;
    public bool HasNextPage => CurrentPage < TotalPages;
}

// Student search view model
public class StudentSearchViewModel : AdvancedSearchParameters
{
    // Search form data
    public List<SelectListItem> CourseOptions { get; set; } = new();
    public List<SelectListItem> DepartmentOptions { get; set; } = new();
    public List<SelectListItem> StatusOptions { get; set; } = new();
    public List<SelectListItem> SortOptions { get; set; } = new();

    // Results
    public PaginatedResult<Student> Results { get; set; } = new();

    // Search statistics
    public string SearchSummary { get; set; }
    public TimeSpan SearchTime { get; set; }
    public bool HasResults => Results.Data.Any();
}

// Search suggestion model
public class SearchSuggestion
{
    public string Text { get; set; }
    public string Type { get; set; } // "Name", "Email", "Course", etc.
    public int Count { get; set; }
    public string Highlight { get; set; }
}
```

### Enhanced Controller with Search Operations

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourProjectName.Data;
using YourProjectName.Models;
using System.Diagnostics;

public class StudentsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(AppDbContext context, ILogger<StudentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Students - Main list with search functionality
    public async Task<IActionResult> Index(StudentSearchViewModel searchModel)
    {
        var stopwatch = Stopwatch.StartNew();

        try
        {
            // Initialize search options
            await InitializeSearchOptions(searchModel);

            // Perform search
            var result = await SearchStudents(searchModel);
            searchModel.Results = result;

            // Generate search summary
            searchModel.SearchSummary = GenerateSearchSummary(searchModel, result.TotalCount);
            searchModel.SearchTime = stopwatch.Elapsed;

            return View(searchModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during student search");
            ModelState.AddModelError("", "Search failed. Please try again.");
            return View(searchModel);
        }
    }

    // GET: Students/SearchSuggestions - AJAX search suggestions
    [HttpGet]
    public async Task<IActionResult> GetSearchSuggestions(string term)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(term) || term.Length < 2)
                return Json(new List<SearchSuggestion>());

            var suggestions = new List<SearchSuggestion>();

            // Name suggestions
            var nameSuggestions = await _context.Students
                .Where(s => s.IsActive && (s.FirstName.Contains(term) || s.LastName.Contains(term)))
                .GroupBy(s => s.FirstName + " " + s.LastName)
                .Select(g => new SearchSuggestion
                {
                    Text = g.Key,
                    Type = "Name",
                    Count = g.Count(),
                    Highlight = HighlightSearchTerm(g.Key, term)
                })
                .Take(5)
                .ToListAsync();

            suggestions.AddRange(nameSuggestions);

            // Email suggestions
            var emailSuggestions = await _context.Students
                .Where(s => s.IsActive && s.Email.Contains(term))
                .Select(s => new SearchSuggestion
                {
                    Text = s.Email,
                    Type = "Email",
                    Count = 1,
                    Highlight = HighlightSearchTerm(s.Email, term)
                })
                .Take(3)
                .ToListAsync();

            suggestions.AddRange(emailSuggestions);

            // Course suggestions
            var courseSuggestions = await _context.Students
                .Where(s => s.IsActive && s.Course.Contains(term))
                .GroupBy(s => s.Course)
                .Select(g => new SearchSuggestion
                {
                    Text = g.Key,
                    Type = "Course",
                    Count = g.Count(),
                    Highlight = HighlightSearchTerm(g.Key, term)
                })
                .Take(3)
                .ToListAsync();

            suggestions.AddRange(courseSuggestions);

            return Json(suggestions.Take(10));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting search suggestions for term: {Term}", term);
            return Json(new List<SearchSuggestion>());
        }
    }

    // GET: Students/QuickSearch - Quick search for header/navigation
    [HttpGet]
    public async Task<IActionResult> QuickSearch(string q)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
                return Json(new { success = false, message = "Empty search term" });

            var results = await _context.Students
                .Where(s => s.IsActive &&
                           (s.FirstName.Contains(q) ||
                            s.LastName.Contains(q) ||
                            s.Email.Contains(q) ||
                            s.EnrollmentNumber.Contains(q)))
                .Select(s => new
                {
                    s.Id,
                    Name = s.FirstName + " " + s.LastName,
                    s.Email,
                    s.Course,
                    s.EnrollmentNumber
                })
                .Take(10)
                .ToListAsync();

            return Json(new
            {
                success = true,
                results = results,
                total = results.Count
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in quick search for term: {Term}", q);
            return Json(new { success = false, message = "Search failed" });
        }
    }

    // POST: Students/ExportSearch - Export search results
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExportSearch(StudentSearchViewModel searchModel)
    {
        try
        {
            // Get all matching results (no pagination)
            var allResults = await SearchStudents(searchModel, includeAll: true);

            // Generate CSV
            var csv = GenerateCsvFromResults(allResults.Data);
            var fileName = $"Students_Search_{DateTime.Now:yyyyMMdd_HHmmss}.csv";

            return File(System.Text.Encoding.UTF8.GetBytes(csv), "text/csv", fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting search results");
            TempData["Error"] = "Export failed. Please try again.";
            return RedirectToAction(nameof(Index), searchModel);
        }
    }

    // Private helper methods
    private async Task<PaginatedResult<Student>> SearchStudents(StudentSearchViewModel searchModel, bool includeAll = false)
    {
        var query = _context.Students.AsQueryable();

        // Apply filters
        query = ApplyFilters(query, searchModel);

        // Apply search term
        if (!string.IsNullOrWhiteSpace(searchModel.SearchTerm))
        {
            query = query.Where(s =>
                s.FirstName.Contains(searchModel.SearchTerm) ||
                s.LastName.Contains(searchModel.SearchTerm) ||
                s.Email.Contains(searchModel.SearchTerm) ||
                s.EnrollmentNumber.Contains(searchModel.SearchTerm) ||
                s.Course.Contains(searchModel.SearchTerm));
        }

        // Apply sorting
        query = ApplySorting(query, searchModel.SortBy, searchModel.SortDirection);

        // Get total count
        var totalCount = await query.CountAsync();

        // Apply pagination (unless exporting all)
        if (!includeAll)
        {
            query = query.Skip((searchModel.Page - 1) * searchModel.PageSize)
                        .Take(searchModel.PageSize);
        }

        var data = await query.ToListAsync();

        return new PaginatedResult<Student>
        {
            Data = data,
            CurrentPage = searchModel.Page,
            PageSize = searchModel.PageSize,
            TotalCount = totalCount
        };
    }

    private IQueryable<Student> ApplyFilters(IQueryable<Student> query, StudentSearchViewModel searchModel)
    {
        // Status filter
        if (searchModel.IsActive.HasValue)
        {
            query = query.Where(s => s.IsActive == searchModel.IsActive.Value);
        }

        // Course filter
        if (!string.IsNullOrWhiteSpace(searchModel.SelectedCourse))
        {
            query = query.Where(s => s.Course == searchModel.SelectedCourse);
        }

        // Department filter (if you have department field)
        if (!string.IsNullOrWhiteSpace(searchModel.SelectedDepartment))
        {
            query = query.Where(s => s.Department == searchModel.SelectedDepartment);
        }

        // Date range filter
        if (searchModel.StartDate.HasValue)
        {
            query = query.Where(s => s.CreatedDate >= searchModel.StartDate.Value);
        }

        if (searchModel.EndDate.HasValue)
        {
            var endDate = searchModel.EndDate.Value.AddDays(1); // Include end date
            query = query.Where(s => s.CreatedDate < endDate);
        }

        // Has enrollments filter
        if (searchModel.HasEnrollments)
        {
            query = query.Where(s => _context.Enrollments.Any(e => e.StudentId == s.Id));
        }

        return query;
    }

    private IQueryable<Student> ApplySorting(IQueryable<Student> query, string sortBy, string sortDirection)
    {
        var isDescending = sortDirection.ToLower() == "desc";

        return sortBy.ToLower() switch
        {
            "name" => isDescending ? query.OrderByDescending(s => s.LastName).ThenByDescending(s => s.FirstName)
                                    : query.OrderBy(s => s.LastName).ThenBy(s => s.FirstName),
            "email" => isDescending ? query.OrderByDescending(s => s.Email)
                                    : query.OrderBy(s => s.Email),
            "enrollmentnumber" => isDescending ? query.OrderByDescending(s => s.EnrollmentNumber)
                                               : query.OrderBy(s => s.EnrollmentNumber),
            "course" => isDescending ? query.OrderByDescending(s => s.Course)
                                     : query.OrderBy(s => s.Course),
            "createddate" => isDescending ? query.OrderByDescending(s => s.CreatedDate)
                                        : query.OrderBy(s => s.CreatedDate),
            _ => isDescending ? query.OrderByDescending(s => s.LastName).ThenByDescending(s => s.FirstName)
                              : query.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
        };
    }

    private async Task InitializeSearchOptions(StudentSearchViewModel searchModel)
    {
        // Course options
        searchModel.CourseOptions = await _context.Students
            .Where(s => s.IsActive && !string.IsNullOrEmpty(s.Course))
            .GroupBy(s => s.Course)
            .Select(g => new SelectListItem
            {
                Value = g.Key,
                Text = $"{g.Key} ({g.Count()})",
                Selected = g.Key == searchModel.SelectedCourse
            })
            .OrderBy(o => o.Text)
            .ToListAsync();

        // Add "All Courses" option
        searchModel.CourseOptions.Insert(0, new SelectListItem { Value = "", Text = "All Courses" });

        // Department options (if applicable)
        searchModel.DepartmentOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "All Departments" },
            new SelectListItem { Value = "Computer Science", Text = "Computer Science" },
            new SelectListItem { Value = "Information Technology", Text = "Information Technology" },
            new SelectListItem { Value = "Electronics", Text = "Electronics" }
        };

        // Status options
        searchModel.StatusOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "", Text = "All Status" },
            new SelectListItem { Value = "true", Text = "Active Only" },
            new SelectListItem { Value = "false", Text = "Inactive Only" }
        };

        // Sort options
        searchModel.SortOptions = new List<SelectListItem>
        {
            new SelectListItem { Value = "name", Text = "Name" },
            new SelectListItem { Value = "email", Text = "Email" },
            new SelectListItem { Value = "enrollmentnumber", Text = "Enrollment Number" },
            new SelectListItem { Value = "course", Text = "Course" },
            new SelectListItem { Value = "createddate", Text = "Created Date" }
        };

        // Set default page if not provided
        if (searchModel.Page <= 0) searchModel.Page = 1;
        if (searchModel.PageSize <= 0) searchModel.PageSize = 10;
    }

    private string GenerateSearchSummary(StudentSearchViewModel searchModel, int totalCount)
    {
        var summaryParts = new List<string>();

        if (totalCount == 0)
            return "No students found matching your criteria.";

        summaryParts.Add($"{totalCount:N0} student{(totalCount != 1 ? "s" : "")} found");

        if (!string.IsNullOrWhiteSpace(searchModel.SearchTerm))
            summaryParts.Add($"for \"{searchModel.SearchTerm}\"");

        if (!string.IsNullOrWhiteSpace(searchModel.SelectedCourse))
            summaryParts.Add($"in {searchModel.SelectedCourse}");

        if (searchModel.IsActive.HasValue)
            summaryParts.Add(searchModel.IsActive.Value ? "active students" : "inactive students");

        if (searchModel.StartDate.HasValue || searchModel.EndDate.HasValue)
        {
            var dateRange = "";
            if (searchModel.StartDate.HasValue)
                dateRange += $"from {searchModel.StartDate.Value:MMM d, yyyy} ";
            if (searchModel.EndDate.HasValue)
                dateRange += $"to {searchModel.EndDate.Value:MMM d, yyyy}";
            summaryParts.Add(dateRange.Trim());
        }

        return string.Join(" ", summaryParts);
    }

    private string HighlightSearchTerm(string text, string term)
    {
        if (string.IsNullOrWhiteSpace(term) || string.IsNullOrWhiteSpace(text))
            return text;

        var index = text.IndexOf(term, StringComparison.OrdinalIgnoreCase);
        if (index < 0) return text;

        return text.Substring(0, index) +
               "<mark>" + text.Substring(index, term.Length) + "</mark>" +
               text.Substring(index + term.Length);
    }

    private string GenerateCsvFromResults(List<Student> students)
    {
        var csv = new System.Text.StringBuilder();

        // Header
        csv.AppendLine("First Name,Last Name,Email,Phone,Course,Enrollment Number,Status,Created Date");

        // Data rows
        foreach (var student in students)
        {
            csv.AppendLine($"\"{student.FirstName}\"," +
                          $"\"{student.LastName}\"," +
                          $"\"{student.Email}\"," +
                          $"\"{student.Phone}\"," +
                          $"\"{student.Course}\"," +
                          $"\"{student.EnrollmentNumber}\"," +
                          $"\"{(student.IsActive ? "Active" : "Inactive")}\"," +
                          $"\"{student.CreatedDate:yyyy-MM-dd HH:mm}\"");
        }

        return csv.ToString();
    }
}
```

### Enhanced Search View (Index.cshtml)

```cshtml
@model StudentSearchViewModel
@{
    ViewData["Title"] = "Students";
}

<div class="container-fluid">
    <!-- Header -->
    <div class="row mb-4">
        <div class="col">
            <h1>Students</h1>
        </div>
        <div class="col-auto">
            <button class="btn btn-success" onclick="toggleAdvancedSearch()">
                <i class="bi bi-funnel"></i> Advanced Search
            </button>
            <a asp-action="Create" class="btn btn-primary">
                <i class="bi bi-plus-circle"></i> Add Student
            </a>
        </div>
    </div>

    <!-- Quick Search Bar -->
    <div class="row mb-4">
        <div class="col-12">
            <form asp-action="Index" method="get" id="quickSearchForm">
                <div class="input-group input-group-lg">
                    <input type="text" class="form-control" name="SearchTerm"
                           placeholder="Search by name, email, or enrollment number..."
                           value="@Model.SearchTerm" id="searchInput"
                           autocomplete="off" data-suggestions-url="@Url.Action("GetSearchSuggestions")">
                    <button class="btn btn-outline-primary" type="submit">
                        <i class="bi bi-search"></i> Search
                    </button>
                    @if (!string.IsNullOrEmpty(Model.SearchTerm))
                    {
                        <a asp-action="Index" class="btn btn-outline-secondary">
                            <i class="bi bi-x-circle"></i> Clear
                        </a>
                    }
                </div>
                <!-- Search suggestions dropdown -->
                <div id="searchSuggestions" class="position-absolute w-100 bg-white border border-top-0 shadow-sm" style="z-index: 1000; display: none;"></div>
            </form>
        </div>
    </div>

    <!-- Advanced Search Panel -->
    <div class="row mb-4" id="advancedSearchPanel" style="display: none;">
        <div class="col-12">
            <div class="card">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h5 class="mb-0">Advanced Search Filters</h5>
                    <button type="button" class="btn-close" onclick="toggleAdvancedSearch()"></button>
                </div>
                <div class="card-body">
                    <form asp-action="Index" method="get" id="advancedSearchForm">
                        <input type="hidden" name="SearchTerm" value="@Model.SearchTerm" />

                        <div class="row">
                            <!-- Course Filter -->
                            <div class="col-md-3 mb-3">
                                <label asp-for="SelectedCourse" class="form-label">Course</label>
                                <select asp-for="SelectedCourse" class="form-select">
                                    @foreach (var option in Model.CourseOptions)
                                    {
                                        <option value="@option.Value" selected="@option.Selected">@option.Text</option>
                                    }
                                </select>
                            </div>

                            <!-- Status Filter -->
                            <div class="col-md-3 mb-3">
                                <label asp-for="IsActive" class="form-label">Status</label>
                                <select name="IsActive" class="form-select">
                                    <option value="">All Status</option>
                                    <option value="true" selected="@(Model.IsActive == true)">Active</option>
                                    <option value="false" selected="@(Model.IsActive == false)">Inactive</option>
                                </select>
                            </div>

                            <!-- Date Range Filter -->
                            <div class="col-md-3 mb-3">
                                <label class="form-label">Start Date</label>
                                <input type="date" name="StartDate" class="form-control"
                                       value="@(Model.StartDate?.ToString("yyyy-MM-dd"))" />
                            </div>

                            <div class="col-md-3 mb-3">
                                <label class="form-label">End Date</label>
                                <input type="date" name="EndDate" class="form-control"
                                       value="@(Model.EndDate?.ToString("yyyy-MM-dd"))" />
                            </div>
                        </div>

                        <div class="row">
                            <!-- Sort Options -->
                            <div class="col-md-3 mb-3">
                                <label asp-for="SortBy" class="form-label">Sort By</label>
                                <select asp-for="SortBy" class="form-select">
                                    @foreach (var option in Model.SortOptions)
                                    {
                                        <option value="@option.Value" selected="@option.Value == Model.SortBy">@option.Text</option>
                                    }
                                </select>
                            </div>

                            <div class="col-md-3 mb-3">
                                <label asp-for="SortDirection" class="form-label">Sort Order</label>
                                <select asp-for="SortDirection" class="form-select">
                                    <option value="Asc" selected="@(Model.SortDirection == "Asc")">Ascending</option>
                                    <option value="Desc" selected="@(Model.SortDirection == "Desc")">Descending</option>
                                </select>
                            </div>

                            <!-- Page Size -->
                            <div class="col-md-3 mb-3">
                                <label class="form-label">Results per page</label>
                                <select name="PageSize" class="form-select">
                                    <option value="5" selected="@(Model.PageSize == 5)">5</option>
                                    <option value="10" selected="@(Model.PageSize == 10)">10</option>
                                    <option value="25" selected="@(Model.PageSize == 25)">25</option>
                                    <option value="50" selected="@(Model.PageSize == 50)">50</option>
                                </select>
                            </div>

                            <div class="col-md-3 mb-3 d-flex align-items-end">
                                <div class="btn-group w-100">
                                    <button type="submit" class="btn btn-primary">
                                        <i class="bi bi-search"></i> Apply Filters
                                    </button>
                                    <a asp-action="Index" class="btn btn-outline-secondary">
                                        <i class="bi bi-arrow-clockwise"></i> Reset
                                    </a>
                                </div>
                            </div>
                        </div>

                        <!-- Has Enrollments Filter -->
                        <div class="row">
                            <div class="col-12">
                                <div class="form-check">
                                    <input type="checkbox" name="HasEnrollments" class="form-check-input"
                                           @(Model.HasEnrollments ? "checked" : "")>
                                    <label class="form-check-label">Only students with enrollments</label>
                                </div>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>
    </div>

    <!-- Search Results Summary -->
    @if (!string.IsNullOrEmpty(Model.SearchSummary))
    {
        <div class="row mb-3">
            <div class="col-12">
                <div class="alert alert-info">
                    <div class="d-flex justify-content-between align-items-center">
                        <span>@Model.SearchSummary</span>
                        <small class="text-muted">
                            Search completed in @Model.SearchTime.TotalMilliseconds.ToString("F0")ms
                        </small>
                    </div>
                </div>
            </div>
        </div>
    }

    <!-- Results Table -->
    <div class="card">
        <div class="card-header d-flex justify-content-between align-items-center">
            <h5 class="mb-0">Search Results</h5>
            @if (Model.HasResults)
            {
                <div class="btn-group">
                    <button class="btn btn-outline-primary btn-sm" onclick="refreshSearch()">
                        <i class="bi bi-arrow-clockwise"></i> Refresh
                    </button>
                    <form asp-action="ExportSearch" method="post" class="d-inline">
                        @Html.HiddenFor(m => m.SearchTerm)
                        @Html.HiddenFor(m => m.SelectedCourse)
                        @Html.HiddenFor(m => m.IsActive)
                        @Html.HiddenFor(m => m.StartDate)
                        @Html.HiddenFor(m => m.EndDate)
                        <button type="submit" class="btn btn-outline-success btn-sm">
                            <i class="bi bi-download"></i> Export CSV
                        </button>
                    </form>
                </div>
            }
        </div>
        <div class="card-body">
            @if (Model.HasResults)
            {
                <div class="table-responsive">
                    <table class="table table-hover">
                        <thead class="table-light">
                            <tr>
                                <th>
                                    <a href="#" onclick="sortResults('name')">
                                        Name
                                        @if (Model.SortBy == "name")
                                        {
                                            <i class="bi bi-sort-@(Model.SortDirection == "Asc" ? "down" : "up")"></i>
                                        }
                                    </a>
                                </th>
                                <th>
                                    <a href="#" onclick="sortResults('email')">
                                        Email
                                        @if (Model.SortBy == "email")
                                        {
                                            <i class="bi bi-sort-@(Model.SortDirection == "Asc" ? "down" : "up")"></i>
                                        }
                                    </a>
                                </th>
                                <th>Phone</th>
                                <th>
                                    <a href="#" onclick="sortResults('course')">
                                        Course
                                        @if (Model.SortBy == "course")
                                        {
                                            <i class="bi bi-sort-@(Model.SortDirection == "Asc" ? "down" : "up")"></i>
                                        }
                                    </a>
                                </th>
                                <th>Enrollment Number</th>
                                <th>Status</th>
                                <th>Actions</th>
                            </tr>
                        </thead>
                        <tbody>
                            @foreach (var student in Model.Results.Data)
                            {
                                <tr>
                                    <td>
                                        <strong>@student.FirstName @student.LastName</strong>
                                        @if (!string.IsNullOrEmpty(Model.SearchTerm) &&
                                             (student.FirstName.Contains(Model.SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                              student.LastName.Contains(Model.SearchTerm, StringComparison.OrdinalIgnoreCase)))
                                        {
                                            <span class="badge bg-warning ms-1">Match</span>
                                        }
                                    </td>
                                    <td>
                                        @student.Email
                                        @if (!string.IsNullOrEmpty(Model.SearchTerm) &&
                                             student.Email.Contains(Model.SearchTerm, StringComparison.OrdinalIgnoreCase))
                                        {
                                            <span class="badge bg-warning ms-1">Match</span>
                                        }
                                    </td>
                                    <td>@student.Phone</td>
                                    <td>
                                        @student.Course
                                        @if (!string.IsNullOrEmpty(Model.SearchTerm) &&
                                             student.Course.Contains(Model.SearchTerm, StringComparison.OrdinalIgnoreCase))
                                        {
                                            <span class="badge bg-warning ms-1">Match</span>
                                        }
                                    </td>
                                    <td>@student.EnrollmentNumber</td>
                                    <td>
                                        @if (student.IsActive)
                                        {
                                            <span class="badge bg-success">Active</span>
                                        }
                                        else
                                        {
                                            <span class="badge bg-secondary">Inactive</span>
                                        }
                                    </td>
                                    <td>
                                        <div class="btn-group" role="group">
                                            <a asp-action="Details" asp-route-id="@student.Id"
                                               class="btn btn-sm btn-outline-primary" title="View">
                                                <i class="bi bi-eye"></i>
                                            </a>
                                            <a asp-action="Edit" asp-route-id="@student.Id"
                                               class="btn btn-sm btn-outline-warning" title="Edit">
                                                <i class="bi bi-pencil"></i>
                                            </a>
                                            <a asp-action="Delete" asp-route-id="@student.Id"
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

                <!-- Pagination -->
                @if (Model.Results.TotalPages > 1)
                {
                    <nav aria-label="Search results pagination">
                        <ul class="pagination justify-content-center">
                            @if (Model.Results.HasPreviousPage)
                            {
                                <li class="page-item">
                                    <a class="page-link" href="#" onclick="goToPage(@(Model.Results.CurrentPage - 1))">Previous</a>
                                </li>
                            }

                            @for (int i = 1; i <= Model.Results.TotalPages; i++)
                            {
                                <li class="page-item @(i == Model.Results.CurrentPage ? "active" : "")">
                                    <a class="page-link" href="#" onclick="goToPage(@i)">@i</a>
                                </li>
                            }

                            @if (Model.Results.HasNextPage)
                            {
                                <li class="page-item">
                                    <a class="page-link" href="#" onclick="goToPage(@(Model.Results.CurrentPage + 1))">Next</a>
                                </li>
                            }
                        </ul>
                    </nav>
                }
            }
            else
            {
                <div class="text-center py-5">
                    <i class="bi bi-search fs-1 text-muted"></i>
                    <h5 class="text-muted mt-3">No students found</h5>
                    <p class="text-muted">Try adjusting your search criteria or add a new student.</p>
                    <a asp-action="Create" class="btn btn-primary">Add New Student</a>
                </div>
            }
        </div>
    </div>
</div>

@section Scripts {
    <script>
        // Toggle advanced search panel
        function toggleAdvancedSearch() {
            const panel = document.getElementById('advancedSearchPanel');
            panel.style.display = panel.style.display === 'none' ? 'block' : 'none';
        }

        // Search suggestions
        const searchInput = document.getElementById('searchInput');
        const suggestionsDiv = document.getElementById('searchSuggestions');
        let suggestionTimeout;

        searchInput.addEventListener('input', function() {
            clearTimeout(suggestionTimeout);
            const term = this.value.trim();

            if (term.length < 2) {
                suggestionsDiv.style.display = 'none';
                return;
            }

            suggestionTimeout = setTimeout(() => {
                fetch(`${suggestionsDiv.dataset.suggestionsUrl}?term=${encodeURIComponent(term)}`)
                    .then(response => response.json())
                    .then(suggestions => {
                        displaySuggestions(suggestions);
                    })
                    .catch(error => {
                        console.error('Error getting suggestions:', error);
                        suggestionsDiv.style.display = 'none';
                    });
            }, 300);
        });

        function displaySuggestions(suggestions) {
            if (!suggestions || suggestions.length === 0) {
                suggestionsDiv.style.display = 'none';
                return;
            }

            let html = '';
            suggestions.forEach(suggestion => {
                html += `
                    <div class="suggestion-item p-2 border-bottom" style="cursor: pointer;"
                         onclick="selectSuggestion('${suggestion.text}')">
                        <div class="d-flex justify-content-between">
                            <span>${suggestion.highlight || suggestion.text}</span>
                            <small class="text-muted">${suggestion.type}</small>
                        </div>
                        ${suggestion.count > 1 ? `<small class="text-muted">${suggestion.count} results</small>` : ''}
                    </div>
                `;
            });

            suggestionsDiv.innerHTML = html;
            suggestionsDiv.style.display = 'block';
        }

        function selectSuggestion(text) {
            searchInput.value = text;
            suggestionsDiv.style.display = 'none';
            document.getElementById('quickSearchForm').submit();
        }

        // Hide suggestions when clicking outside
        document.addEventListener('click', function(e) {
            if (!searchInput.contains(e.target) && !suggestionsDiv.contains(e.target)) {
                suggestionsDiv.style.display = 'none';
            }
        });

        // Sorting
        function sortResults(sortBy) {
            const form = document.getElementById('advancedSearchForm');
            const sortByInput = form.querySelector('input[name="SortBy"]') ||
                               createHiddenInput('SortBy', sortBy, form);
            sortByInput.value = sortBy;

            const sortDirectionInput = form.querySelector('input[name="SortDirection"]') ||
                                      createHiddenInput('SortDirection', 'Asc', form);

            // Toggle sort direction if same column
            if (sortByInput.value === sortBy && sortDirectionInput.value === 'Asc') {
                sortDirectionInput.value = 'Desc';
            } else {
                sortDirectionInput.value = 'Asc';
            }

            form.submit();
        }

        // Pagination
        function goToPage(page) {
            const form = document.getElementById('advancedSearchForm');
            const pageInput = form.querySelector('input[name="Page"]') ||
                            createHiddenInput('Page', page, form);
            pageInput.value = page;
            form.submit();
        }

        // Helper function to create hidden inputs
        function createHiddenInput(name, value, form) {
            const input = document.createElement('input');
            input.type = 'hidden';
            input.name = name;
            input.value = value;
            form.appendChild(input);
            return input;
        }

        // Refresh search
        function refreshSearch() {
            document.getElementById('quickSearchForm').submit();
        }

        // Highlight search terms in results
        document.addEventListener('DOMContentLoaded', function() {
            const searchTerm = '@Model.SearchTerm';
            if (searchTerm) {
                // Add highlighting to matching text
                // This can be enhanced with more sophisticated highlighting logic
            }
        });
    </script>

    <style>
        .suggestion-item:hover {
            background-color: #f8f9fa;
        }

        mark {
            background-color: #fff3cd;
            padding: 0;
        }
    </style>
}
```

## 5. Use Cases

- **Student Management**: Search students by name, ID, course, enrollment date, or status
- **E-commerce**: Product search with filters for category, price range, brand, and ratings
- **Healthcare**: Patient search by name, ID, medical record, or appointment date
- **HR Systems**: Employee directory search with department, position, and skill filters
- **Inventory Management**: Product search with category, warehouse, and stock status filters
- **Library Systems**: Book search by title, author, ISBN, subject, or availability
- **Financial Applications**: Transaction search with date range, amount, and category filters

## 6. Mini Practice Task

1. **Basic Search Implementation**:
   - Create simple text search functionality for Product entity
   - Add search by name and description fields
   - Implement search results highlighting

2. **Advanced Search Features**:
   - Add multiple filter fields (category, price range, status)
   - Implement sorting options with clickable column headers
   - Add pagination to search results

3. **Enhanced Search Experience**:
   - Implement AJAX-powered search suggestions
   - Add real-time search as user types (with debouncing)
   - Create export functionality for search results
   - Implement saved search functionality
   - Add search history and recent searches