# Dynamic Dashboard Design - I

## 1. Description
A dynamic dashboard displays summarized data and statistics in a flexible, tabular format that can be updated based on user selections, time periods, or data filters. It aggregates data from multiple sources and presents it in an organized, visual layout for quick insights and decision-making.

## 2. Why It Is Important
Dashboards transform raw data into actionable insights. They help users monitor key metrics, identify trends, and make informed decisions quickly. Dynamic dashboards adapt to changing requirements and provide real-time visibility into business operations, making them essential for management, analytics, and reporting applications.

## 3. Real-World Examples
- Sales dashboard showing revenue by region, product performance, and monthly trends
- Student management system displaying enrollment statistics, grade distributions, and attendance summaries
- E-commerce admin panel showing orders, inventory levels, and customer metrics
- Healthcare dashboard with patient counts, appointment schedules, and treatment statistics

## 4. Syntax & Explanation

### Dashboard Models

```csharp
// Dashboard summary model for statistics
public class DashboardSummary
{
    public string Title { get; set; }
    public decimal Value { get; set; }
    public string Format { get; set; } = "N0"; // Number format
    public string Icon { get; set; }
    public string Color { get; set; } = "primary";
}

// Table data model for dynamic tabular display
public class DashboardTable
{
    public string Title { get; set; }
    public List<string> Headers { get; set; } = new();
    public List<List<string>> Rows { get; set; } = new();
    public string ActionUrl { get; set; }
}

// Chart data model
public class DashboardChart
{
    public string Title { get; set; }
    public List<string> Labels { get; set; } = new();
    public List<decimal> Values { get; set; } = new();
    public string ChartType { get; set; } = "bar";
}

// Main dashboard view model
public class DashboardViewModel
{
    public List<DashboardSummary> Summaries { get; set; } = new();
    public List<DashboardTable> Tables { get; set; } = new();
    public List<DashboardChart> Charts { get; set; } = new();
    public DateTime LastUpdated { get; set; }
    public string SelectedPeriod { get; set; } = "ThisMonth";
}
```

### Dashboard Controller

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data.SqlClient;
using System.Data;

public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IActionResult> Index(string period = "ThisMonth")
    {
        var model = new DashboardViewModel
        {
            SelectedPeriod = period,
            LastUpdated = DateTime.Now
        };

        // Generate summary statistics
        model.Summaries = await GetDashboardSummaries(period);

        // Generate tabular data
        model.Tables = await GetDashboardTables(period);

        // Generate chart data
        model.Charts = await GetDashboardCharts(period);

        return View(model);
    }

    private async Task<List<DashboardSummary>> GetDashboardSummaries(string period)
    {
        var summaries = new List<DashboardSummary>();

        // Example: Total Students
        var totalStudents = await _context.Students.CountAsync();
        summaries.Add(new DashboardSummary
        {
            Title = "Total Students",
            Value = totalStudents,
            Icon = "bi-people-fill",
            Color = "primary"
        });

        // Example: Active Courses
        var activeCourses = await _context.Courses
            .Where(c => c.IsActive)
            .CountAsync();
        summaries.Add(new DashboardSummary
        {
            Title = "Active Courses",
            Value = activeCourses,
            Icon = "bi-book-fill",
            Color = "success"
        });

        // Example: Total Revenue (if applicable)
        var totalRevenue = await _context.Enrollments
            .Where(e => e.CreatedDate >= GetPeriodStartDate(period))
            .SumAsync(e => e.Fee);
        summaries.Add(new DashboardSummary
        {
            Title = "Period Revenue",
            Value = totalRevenue,
            Format = "C2",
            Icon = "bi-currency-dollar",
            Color = "info"
        });

        return summaries;
    }

    private async Task<List<DashboardTable>> GetDashboardTables(string period)
    {
        var tables = new List<DashboardTable>();

        // Top Courses by Enrollment
        var topCourses = await _context.Courses
            .Select(c => new
            {
                c.CourseName,
                EnrollmentCount = _context.Enrollments.Count(e => e.CourseId == c.Id),
                Instructor = c.Instructor.Name
            })
            .OrderByDescending(c => c.EnrollmentCount)
            .Take(5)
            .ToListAsync();

        tables.Add(new DashboardTable
        {
            Title = "Top Courses by Enrollment",
            Headers = new List<string> { "Course", "Students", "Instructor" },
            Rows = topCourses.Select(c => new List<string>
            {
                c.CourseName,
                c.EnrollmentCount.ToString(),
                c.Instructor
            }).ToList(),
            ActionUrl = "/Courses"
        });

        // Recent Enrollments
        var recentEnrollments = await _context.Enrollments
            .Include(e => e.Student)
            .Include(e => e.Course)
            .Where(e => e.CreatedDate >= GetPeriodStartDate(period))
            .OrderByDescending(e => e.CreatedDate)
            .Take(10)
            .ToListAsync();

        tables.Add(new DashboardTable
        {
            Title = "Recent Enrollments",
            Headers = new List<string> { "Student", "Course", "Date", "Status" },
            Rows = recentEnrollments.Select(e => new List<string>
            {
                e.Student.Name,
                e.Course.CourseName,
                e.CreatedDate.ToString("MMM dd, yyyy"),
                e.Status.ToString()
            }).ToList(),
            ActionUrl = "/Enrollments"
        });

        return tables;
    }

    private async Task<List<DashboardChart>> GetDashboardCharts(string period)
    {
        var charts = new List<DashboardChart>();

        // Enrollment by Month
        var monthlyEnrollments = await _context.Enrollments
            .Where(e => e.CreatedDate >= GetPeriodStartDate(period))
            .GroupBy(e => new { e.CreatedDate.Year, e.CreatedDate.Month })
            .Select(g => new
            {
                Month = $"{g.Key.Year}-{g.Key.Month:D2}",
                Count = g.Count()
            })
            .OrderBy(g => g.Month)
            .ToListAsync();

        charts.Add(new DashboardChart
        {
            Title = "Monthly Enrollments",
            Labels = monthlyEnrollments.Select(m => m.Month).ToList(),
            Values = monthlyEnrollments.Select(m => (decimal)m.Count).ToList(),
            ChartType = "line"
        });

        return charts;
    }

    private DateTime GetPeriodStartDate(string period)
    {
        return period switch
        {
            "Today" => DateTime.Today,
            "ThisWeek" => DateTime.Today.AddDays(-7),
            "ThisMonth" => new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1),
            "ThisYear" => new DateTime(DateTime.Today.Year, 1, 1),
            _ => DateTime.Today.AddDays(-30)
        };
    }

    [HttpPost]
    public async Task<IActionResult> RefreshData(string period)
    {
        var model = await GetDashboardData(period);
        return Json(model);
    }

    private async Task<DashboardViewModel> GetDashboardData(string period)
    {
        return new DashboardViewModel
        {
            SelectedPeriod = period,
            LastUpdated = DateTime.Now,
            Summaries = await GetDashboardSummaries(period),
            Tables = await GetDashboardTables(period),
            Charts = await GetDashboardCharts(period)
        };
    }
}
```

### Dashboard View (Index.cshtml)

```cshtml
@model DashboardViewModel
@{
    ViewData["Title"] = "Dashboard";
}

<div class="container-fluid">
    <!-- Header with Period Selector -->
    <div class="d-flex justify-content-between align-items-center mb-4">
        <h1>Dashboard</h1>
        <div class="d-flex align-items-center">
            <select id="periodSelector" class="form-select me-2" style="width: auto;">
                <option value="Today" selected="@(Model.SelectedPeriod == "Today")">Today</option>
                <option value="ThisWeek" selected="@(Model.SelectedPeriod == "ThisWeek")">This Week</option>
                <option value="ThisMonth" selected="@(Model.SelectedPeriod == "ThisMonth")">This Month</option>
                <option value="ThisYear" selected="@(Model.SelectedPeriod == "ThisYear")">This Year</option>
            </select>
            <button id="refreshBtn" class="btn btn-outline-primary">
                <i class="bi bi-arrow-clockwise"></i> Refresh
            </button>
        </div>
    </div>

    <!-- Last Updated -->
    <div class="text-muted mb-3">
        Last updated: @Model.LastUpdated.ToString("MMM dd, yyyy HH:mm:ss")
    </div>

    <!-- Summary Cards -->
    <div class="row mb-4">
        @foreach (var summary in Model.Summaries)
        {
            <div class="col-md-3 mb-3">
                <div class="card border-0 shadow-sm">
                    <div class="card-body">
                        <div class="d-flex align-items-center">
                            <div class="flex-grow-1">
                                <h6 class="text-muted mb-2">@summary.Title</h6>
                                <h3 class="mb-0 text-@summary.Color">@summary.Value.ToString(summary.Format)</h3>
                            </div>
                            <div class="text-@summary.Color">
                                <i class="bi @summary.Icon fs-2"></i>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        }
    </div>

    <!-- Tables Section -->
    <div class="row">
        @foreach (var table in Model.Tables)
        {
            <div class="col-lg-6 mb-4">
                <div class="card">
                    <div class="card-header d-flex justify-content-between align-items-center">
                        <h5 class="mb-0">@table.Title</h5>
                        @if (!string.IsNullOrEmpty(table.ActionUrl))
                        {
                            <a href="@table.ActionUrl" class="btn btn-sm btn-outline-primary">View All</a>
                        }
                    </div>
                    <div class="card-body">
                        <div class="table-responsive">
                            <table class="table table-sm">
                                <thead>
                                    <tr>
                                        @foreach (var header in table.Headers)
                                        {
                                            <th>@header</th>
                                        }
                                    </tr>
                                </thead>
                                <tbody>
                                    @foreach (var row in table.Rows)
                                    {
                                        <tr>
                                            @foreach (var cell in row)
                                            {
                                                <td>@cell</td>
                                            }
                                        </tr>
                                    }
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </div>
        }
    </div>

    <!-- Charts Section (if using chart library) -->
    @if (Model.Charts.Any())
    {
        <div class="row">
            @foreach (var chart in Model.Charts)
            {
                <div class="col-lg-6 mb-4">
                    <div class="card">
                        <div class="card-header">
                            <h5 class="mb-0">@chart.Title</h5>
                        </div>
                        <div class="card-body">
                            <canvas id="@($"chart_{chart.Title.Replace(" ", "_")}")" height="100"></canvas>
                        </div>
                    </div>
                </div>
            }
        </div>
    }
</div>

@section Scripts {
    <script>
        $(document).ready(function() {
            // Period selector change
            $('#periodSelector').change(function() {
                refreshDashboard($(this).val());
            });

            // Refresh button click
            $('#refreshBtn').click(function() {
                var period = $('#periodSelector').val();
                refreshDashboard(period);
            });

            function refreshDashboard(period) {
                $.ajax({
                    url: '@Url.Action("RefreshData", "Dashboard")',
                    type: 'POST',
                    data: { period: period },
                    success: function(result) {
                        // Update the page with new data
                        location.reload(); // Simple refresh, or update specific elements
                    },
                    error: function() {
                        alert('Error refreshing dashboard');
                    }
                });
            }
        });
    </script>
}
```

## 5. Use Cases

- **Educational Institutions**: Student enrollment tracking, course performance metrics, faculty workload analysis
- **Business Analytics**: Sales performance, inventory management, customer behavior analysis
- **Healthcare**: Patient statistics, appointment scheduling, treatment outcomes
- **E-commerce**: Order processing, product performance, customer metrics
- **Project Management**: Task completion, resource allocation, timeline tracking
- **Financial Services**: Transaction monitoring, portfolio performance, risk assessment

## 6. Mini Practice Task

1. **Basic Dashboard**: Create a simple dashboard that displays:
   - Total number of students in the system
   - Number of active courses
   - Recent 5 enrollments in a table

2. **Enhanced Dashboard**: Extend the basic dashboard by adding:
   - Period selector (Today, This Week, This Month)
   - Summary cards with icons and colors
   - Top 3 courses by enrollment count
   - AJAX refresh functionality

3. **Advanced Features**: Add to your dashboard:
   - Chart showing enrollment trends over time
   - Department-wise student distribution
   - Instructor course load statistics
   - Export functionality for table data