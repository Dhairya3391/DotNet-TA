# Implementation of Excel Export Functionality

## 1. Description
Excel Export functionality allows users to download data from web applications into Microsoft Excel format (.xlsx). This feature enables data analysis, reporting, and offline access to application data. Export can include all records, filtered search results, or selected records with custom formatting, styling, and formulas.

## 2. Why It Is Important
Excel export is crucial for business applications as it allows users to perform advanced data analysis, create reports, share information with stakeholders, and maintain offline records. It bridges the gap between web applications and productivity tools, enhancing data usability and decision-making capabilities.

## 3. Real-World Examples
- Student management system exporting class lists, grades, and attendance records
- E-commerce platform exporting sales reports, inventory data, and customer information
- Healthcare application exporting patient lists, appointment schedules, and medical records
- Financial system exporting transaction reports, account statements, and audit logs
- HR management exporting employee directories, payroll data, and performance reports
- Inventory management exporting stock levels, supplier information, and sales data

## 4. Syntax & Explanation

### Excel Export Models and Configuration

```csharp
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Data;

// Excel export configuration model
public class ExcelExportConfiguration
{
    public string FileName { get; set; }
    public string WorksheetName { get; set; } = "Sheet1";
    public string Title { get; set; }
    public bool IncludeHeaders { get; set; } = true;
    public bool AutoFitColumns { get; set; } = true;
    public bool IncludeTotals { get; set; } = false;
    public string DateFormat { get; set; } = "dd-MM-yyyy";
    public string NumberFormat { get; set; } = "#,##0.00";
    public Dictionary<string, string> ColumnHeaders { get; set; } = new();
    public List<string> HiddenColumns { get; set; } = new();
    public List<string> DateColumns { get; set; } = new();
    public List<string> NumberColumns { get; set; } = new();
    public List<string> CurrencyColumns { get; set; } = new();
}

// Export result model
public class ExcelExportResult
{
    public byte[] FileBytes { get; set; }
    public string FileName { get; set; }
    public string ContentType { get; set; } = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    public int RecordCount { get; set; }
    public bool Success { get; set; }
    public string ErrorMessage { get; set; }
}

// Export request model
public class ExportRequest
{
    public string ExportType { get; set; } // "All", "Filtered", "Selected"
    public List<int> SelectedIds { get; set; } = new();
    public Dictionary<string, string> Filters { get; set; } = new();
    public string SortColumn { get; set; }
    public string SortDirection { get; set; }
    public string Format { get; set; } = "Excel"; // Excel, CSV, PDF
}
```

### Excel Export Service

```csharp
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Reflection;

public interface IExcelExportService
{
    Task<ExcelExportResult> ExportStudentsAsync(IEnumerable<Student> students, ExcelExportConfiguration config = null);
    Task<ExcelExportResult> ExportToExcelAsync<T>(IEnumerable<T> data, ExcelExportConfiguration config = null);
    Task<ExcelExportResult> ExportDataTableAsync(DataTable dataTable, ExcelExportConfiguration config = null);
}

public class ExcelExportService : IExcelExportService
{
    public ExcelExportService()
    {
        // Set EPPlus license context (required for EPPlus 5+)
        ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
    }

    public async Task<ExcelExportResult> ExportStudentsAsync(IEnumerable<Student> students, ExcelExportConfiguration config = null)
    {
        try
        {
            config ??= GetDefaultStudentExportConfig();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(config.WorksheetName);

                // Apply title if provided
                if (!string.IsNullOrEmpty(config.Title))
                {
                    ApplyWorksheetTitle(worksheet, config.Title);
                }

                // Get properties for column mapping
                var studentProperties = typeof(Student).GetProperties()
                    .Where(p => !config.HiddenColumns.Contains(p.Name))
                    .ToList();

                int startRow = config.ColumnHeaders.Any() ? 2 : 1;

                // Add headers
                if (config.IncludeHeaders)
                {
                    await AddHeadersAsync(worksheet, config, studentProperties, startRow);
                    startRow++;
                }

                // Add data rows
                int currentRow = startRow;
                foreach (var student in students)
                {
                    await AddStudentRowAsync(worksheet, config, student, studentProperties, currentRow);
                    currentRow++;
                }

                // Apply formatting
                ApplyColumnFormatting(worksheet, config, studentProperties, startRow, currentRow - 1);

                // Add totals if requested
                if (config.IncludeTotals && students.Any())
                {
                    await AddTotalsRowAsync(worksheet, config, studentProperties, currentRow);
                }

                // Auto-fit columns
                if (config.AutoFitColumns)
                {
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                // Apply styling
                ApplyWorksheetStyling(worksheet, config, startRow - 1, currentRow - 1);

                return new ExcelExportResult
                {
                    FileBytes = await package.GetAsByteArrayAsync(),
                    FileName = config.FileName,
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    RecordCount = students.Count(),
                    Success = true
                };
            }
        }
        catch (Exception ex)
        {
            return new ExcelExportResult
            {
                Success = false,
                ErrorMessage = $"Error exporting data: {ex.Message}"
            };
        }
    }

    public async Task<ExcelExportResult> ExportToExcelAsync<T>(IEnumerable<T> data, ExcelExportConfiguration config = null)
    {
        try
        {
            config ??= new ExcelExportConfiguration
            {
                FileName = $"Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                WorksheetName = typeof(T).Name
            };

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(config.WorksheetName);

                // Get properties
                var properties = typeof(T).GetProperties()
                    .Where(p => !config.HiddenColumns.Contains(p.Name))
                    .ToList();

                int startRow = 1;

                // Add headers
                if (config.IncludeHeaders)
                {
                    await AddGenericHeadersAsync(worksheet, config, properties, startRow);
                    startRow++;
                }

                // Add data
                int currentRow = startRow;
                foreach (var item in data)
                {
                    await AddGenericRowAsync(worksheet, config, item, properties, currentRow);
                    currentRow++;
                }

                // Apply formatting
                ApplyGenericFormatting(worksheet, config, properties, startRow, currentRow - 1);

                // Auto-fit columns
                if (config.AutoFitColumns)
                {
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                return new ExcelExportResult
                {
                    FileBytes = await package.GetAsByteArrayAsync(),
                    FileName = config.FileName,
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    RecordCount = data.Count(),
                    Success = true
                };
            }
        }
        catch (Exception ex)
        {
            return new ExcelExportResult
            {
                Success = false,
                ErrorMessage = $"Error exporting data: {ex.Message}"
            };
        }
    }

    public async Task<ExcelExportResult> ExportDataTableAsync(DataTable dataTable, ExcelExportConfiguration config = null)
    {
        try
        {
            config ??= new ExcelExportConfiguration
            {
                FileName = $"Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                WorksheetName = "Data"
            };

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add(config.WorksheetName);

                // Add headers
                if (config.IncludeHeaders)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        var columnName = config.ColumnHeaders.ContainsKey(dataTable.Columns[col].ColumnName)
                            ? config.ColumnHeaders[dataTable.Columns[col].ColumnName]
                            : dataTable.Columns[col].ColumnName;

                        worksheet.Cells[1, col + 1].Value = columnName;
                    }
                }

                // Add data
                for (int row = 0; row < dataTable.Rows.Count; row++)
                {
                    for (int col = 0; col < dataTable.Columns.Count; col++)
                    {
                        worksheet.Cells[row + (config.IncludeHeaders ? 2 : 1), col + 1].Value = dataTable.Rows[row][col];
                    }
                }

                // Auto-fit columns
                if (config.AutoFitColumns)
                {
                    worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();
                }

                return new ExcelExportResult
                {
                    FileBytes = await package.GetAsByteArrayAsync(),
                    FileName = config.FileName,
                    ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
                    RecordCount = dataTable.Rows.Count,
                    Success = true
                };
            }
        }
        catch (Exception ex)
        {
            return new ExcelExportResult
            {
                Success = false,
                ErrorMessage = $"Error exporting data: {ex.Message}"
            };
        }
    }

    private ExcelExportConfiguration GetDefaultStudentExportConfig()
    {
        return new ExcelExportConfiguration
        {
            FileName = $"Students_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
            WorksheetName = "Students",
            Title = "Student List Report",
            IncludeHeaders = true,
            AutoFitColumns = true,
            IncludeTotals = false,
            ColumnHeaders = new Dictionary<string, string>
            {
                { "Id", "Student ID" },
                { "FirstName", "First Name" },
                { "LastName", "Last Name" },
                { "Email", "Email Address" },
                { "Phone", "Phone Number" },
                { "DateOfBirth", "Date of Birth" },
                { "Course", "Course" },
                { "EnrollmentNumber", "Enrollment Number" },
                { "IsActive", "Status" },
                { "CreatedDate", "Created Date" }
            },
            HiddenColumns = new List<string> { "RowVersion", "LastModifiedBy", "ModificationReason" },
            DateColumns = new List<string> { "DateOfBirth", "CreatedDate", "UpdatedDate" },
            CurrencyColumns = new List<string>()
        };
    }

    private void ApplyWorksheetTitle(ExcelWorksheet worksheet, string title)
    {
        worksheet.Cells["A1"].Value = title;
        worksheet.Cells["A1"].Style.Font.Bold = true;
        worksheet.Cells["A1"].Style.Font.Size = 16;
        worksheet.Cells["A1"].Style.Fill.PatternType = ExcelFillStyle.Solid;
        worksheet.Cells["A1"].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(31, 78, 121));
        worksheet.Cells["A1"].Style.Font.Color.SetColor(Color.White);
        worksheet.Cells["A1:K1"].Merge = true;
        worksheet.Cells["A1:K1"].Style.HorizontalAlignment = ExcelHorizontalAlignment.Center;
    }

    private async Task AddHeadersAsync(ExcelWorksheet worksheet, ExcelExportConfiguration config, List<PropertyInfo> properties, int row)
    {
        int col = 1;
        foreach (var property in properties)
        {
            var headerName = config.ColumnHeaders.ContainsKey(property.Name)
                ? config.ColumnHeaders[property.Name]
                : property.Name;

            worksheet.Cells[row, col].Value = headerName;
            worksheet.Cells[row, col].Style.Font.Bold = true;
            worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            worksheet.Cells[row, col].Style.Font.Color.SetColor(Color.White);
            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);

            col++;
        }
    }

    private async Task AddStudentRowAsync(ExcelWorksheet worksheet, ExcelExportConfiguration config, Student student, List<PropertyInfo> properties, int row)
    {
        int col = 1;
        foreach (var property in properties)
        {
            var value = property.GetValue(student);

            // Handle special formatting
            if (property.Name == "IsActive" && value is bool isActive)
            {
                worksheet.Cells[row, col].Value = isActive ? "Active" : "Inactive";
                worksheet.Cells[row, col].Style.Font.Color.SetColor(isActive ? Color.Green : Color.Red);
            }
            else if (value != null)
            {
                worksheet.Cells[row, col].Value = value;
            }

            worksheet.Cells[row, col].Style.Border.BorderAround(ExcelBorderStyle.Thin);
            col++;
        }
    }

    private async Task AddHeadersAsync(ExcelWorksheet worksheet, ExcelExportConfiguration config, List<PropertyInfo> properties, int row)
    {
        int col = 1;
        foreach (var property in properties)
        {
            var headerName = config.ColumnHeaders.ContainsKey(property.Name)
                ? config.ColumnHeaders[property.Name]
                : property.Name;

            worksheet.Cells[row, col].Value = headerName;
            worksheet.Cells[row, col].Style.Font.Bold = true;
            worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            worksheet.Cells[row, col].Style.Font.Color.SetColor(Color.White);
            col++;
        }
    }

    private async Task AddGenericHeadersAsync(ExcelWorksheet worksheet, ExcelExportConfiguration config, List<PropertyInfo> properties, int row)
    {
        int col = 1;
        foreach (var property in properties)
        {
            var headerName = config.ColumnHeaders.ContainsKey(property.Name)
                ? config.ColumnHeaders[property.Name]
                : property.Name;

            worksheet.Cells[row, col].Value = headerName;
            worksheet.Cells[row, col].Style.Font.Bold = true;
            worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
            worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.FromArgb(79, 129, 189));
            worksheet.Cells[row, col].Style.Font.Color.SetColor(Color.White);
            col++;
        }
    }

    private async Task AddGenericRowAsync(ExcelWorksheet worksheet, ExcelExportConfiguration config, T item, List<PropertyInfo> properties, int row)
    {
        int col = 1;
        foreach (var property in properties)
        {
            var value = property.GetValue(item);
            if (value != null)
            {
                worksheet.Cells[row, col].Value = value;
            }
            col++;
        }
    }

    private void ApplyColumnFormatting(ExcelWorksheet worksheet, ExcelExportConfiguration config, List<PropertyInfo> properties, int startRow, int endRow)
    {
        int col = 1;
        foreach (var property in properties)
        {
            var range = worksheet.Cells[startRow, col, endRow, col];

            if (config.DateColumns.Contains(property.Name))
            {
                range.Style.Numberformat.Format = config.DateFormat;
            }
            else if (config.CurrencyColumns.Contains(property.Name))
            {
                range.Style.Numberformat.Format = config.NumberFormat;
            }
            else if (config.NumberColumns.Contains(property.Name))
            {
                range.Style.Numberformat.Format = "#,##0";
            }

            col++;
        }
    }

    private void ApplyGenericFormatting(ExcelWorksheet worksheet, ExcelExportConfiguration config, List<PropertyInfo> properties, int startRow, int endRow)
    {
        int col = 1;
        foreach (var property in properties)
        {
            var range = worksheet.Cells[startRow, col, endRow, col];

            if (config.DateColumns.Contains(property.Name))
            {
                range.Style.Numberformat.Format = config.DateFormat;
            }
            else if (config.CurrencyColumns.Contains(property.Name))
            {
                range.Style.Numberformat.Format = config.NumberFormat;
            }

            col++;
        }
    }

    private async Task AddTotalsRowAsync(ExcelWorksheet worksheet, ExcelExportConfiguration config, List<PropertyInfo> properties, int row)
    {
        int col = 1;
        foreach (var property in properties)
        {
            if (property.PropertyType == typeof(decimal) || property.PropertyType == typeof(int))
            {
                worksheet.Cells[row, col].Formula = $"SUM({worksheet.Cells[startRow, col].Address}:{worksheet.Cells[row - 1, col].Address})";
                worksheet.Cells[row, col].Style.Font.Bold = true;
                worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }
            else if (property.Name == "FirstName")
            {
                worksheet.Cells[row, col].Value = "TOTAL";
                worksheet.Cells[row, col].Style.Font.Bold = true;
                worksheet.Cells[row, col].Style.Fill.PatternType = ExcelFillStyle.Solid;
                worksheet.Cells[row, col].Style.Fill.BackgroundColor.SetColor(Color.LightGray);
            }
            col++;
        }
    }

    private void ApplyWorksheetStyling(ExcelWorksheet worksheet, ExcelExportConfiguration config, int headerRow, int lastDataRow)
    {
        // Apply alternating row colors
        for (int row = headerRow + 1; row <= lastDataRow; row++)
        {
            var range = worksheet.Cells[row, 1, row, worksheet.Dimension.End.Column];
            if (row % 2 == 0)
            {
                range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                range.Style.Fill.BackgroundColor.SetColor(Color.FromArgb(242, 242, 242));
            }
        }

        // Apply border to entire data range
        var dataRange = worksheet.Cells[headerRow, 1, lastDataRow, worksheet.Dimension.End.Column];
        dataRange.Style.Border.Top.Style = ExcelBorderStyle.Thin;
        dataRange.Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
        dataRange.Style.Border.Left.Style = ExcelBorderStyle.Thin;
        dataRange.Style.Border.Right.Style = ExcelBorderStyle.Thin;
    }
}
```

### Enhanced Controller with Export Functionality

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourProjectName.Data;
using YourProjectName.Models;
using YourProjectName.Services;

public class StudentsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IExcelExportService _excelExportService;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(
        AppDbContext context,
        IExcelExportService excelExportService,
        ILogger<StudentsController> logger)
    {
        _context = context;
        _excelExportService = excelExportService;
        _logger = logger;
    }

    // GET: Students
    public async Task<IActionResult> Index(StudentSearchViewModel searchModel)
    {
        // Existing search logic...
        var result = await SearchStudents(searchModel);
        searchModel.Results = result;
        await InitializeSearchOptions(searchModel);
        return View(searchModel);
    }

    // POST: Students/ExportAll
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExportAll()
    {
        try
        {
            var allStudents = await _context.Students
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();

            var config = new ExcelExportConfiguration
            {
                FileName = $"All_Students_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                WorksheetName = "All Students",
                Title = "Complete Student List",
                IncludeHeaders = true,
                AutoFitColumns = true,
                IncludeTotals = false
            };

            var exportResult = await _excelExportService.ExportStudentsAsync(allStudents, config);

            if (exportResult.Success)
            {
                return File(exportResult.FileBytes, exportResult.ContentType, exportResult.FileName);
            }

            TempData["Error"] = exportResult.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting all students");
            TempData["Error"] = "Export failed. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Students/ExportFiltered
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExportFiltered(StudentSearchViewModel searchModel)
    {
        try
        {
            var filteredStudents = await SearchStudents(searchModel, includeAll: true);

            if (!filteredStudents.Data.Any())
            {
                TempData["Warning"] = "No students found matching the current filters.";
                return RedirectToAction(nameof(Index));
            }

            var config = new ExcelExportConfiguration
            {
                FileName = $"Filtered_Students_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                WorksheetName = "Filtered Students",
                Title = $"Filtered Student List - {filteredStudents.TotalCount} records",
                IncludeHeaders = true,
                AutoFitColumns = true,
                IncludeTotals = false
            };

            var exportResult = await _excelExportService.ExportStudentsAsync(filteredStudents.Data, config);

            if (exportResult.Success)
            {
                TempData["Success"] = $"Successfully exported {exportResult.RecordCount} students.";
                return File(exportResult.FileBytes, exportResult.ContentType, exportResult.FileName);
            }

            TempData["Error"] = exportResult.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting filtered students");
            TempData["Error"] = "Export failed. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Students/ExportSelected
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExportSelected(int[] selectedIds)
    {
        try
        {
            if (selectedIds == null || selectedIds.Length == 0)
            {
                TempData["Warning"] = "No students selected for export.";
                return RedirectToAction(nameof(Index));
            }

            var selectedStudents = await _context.Students
                .Where(s => selectedIds.Contains(s.Id))
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .ToListAsync();

            var config = new ExcelExportConfiguration
            {
                FileName = $"Selected_Students_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                WorksheetName = "Selected Students",
                Title = $"Selected Students - {selectedStudents.Count} records",
                IncludeHeaders = true,
                AutoFitColumns = true,
                IncludeTotals = false
            };

            var exportResult = await _excelExportService.ExportStudentsAsync(selectedStudents, config);

            if (exportResult.Success)
            {
                TempData["Success"] = $"Successfully exported {exportResult.RecordCount} selected students.";
                return File(exportResult.FileBytes, exportResult.ContentType, exportResult.FileName);
            }

            TempData["Error"] = exportResult.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting selected students");
            TempData["Error"] = "Export failed. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Students/ExportCustom
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ExportCustom(ExportRequest exportRequest)
    {
        try
        {
            IEnumerable<Student> students;

            switch (exportRequest.ExportType)
            {
                case "All":
                    students = await _context.Students.ToListAsync();
                    break;
                case "Filtered":
                    students = await GetFilteredStudents(exportRequest.Filters);
                    break;
                case "Selected":
                    students = await _context.Students
                        .Where(s => exportRequest.SelectedIds.Contains(s.Id))
                        .ToListAsync();
                    break;
                default:
                    students = await _context.Students.ToListAsync();
                    break;
            }

            var config = new ExcelExportConfiguration
            {
                FileName = $"Custom_Export_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx",
                WorksheetName = "Student Export",
                Title = "Custom Student Export",
                IncludeHeaders = true,
                AutoFitColumns = true,
                IncludeTotals = exportRequest.Filters.ContainsKey("IncludeTotals") &&
                               bool.Parse(exportRequest.Filters["IncludeTotals"])
            };

            // Apply custom column headers if provided
            if (exportRequest.Filters.ContainsKey("CustomHeaders"))
            {
                var headers = exportRequest.Filters["CustomHeaders"].Split(',');
                foreach (var header in headers)
                {
                    var parts = header.Split(':');
                    if (parts.Length == 2)
                    {
                        config.ColumnHeaders[parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }

            var exportResult = await _excelExportService.ExportStudentsAsync(students, config);

            if (exportResult.Success)
            {
                return File(exportResult.FileBytes, exportResult.ContentType, exportResult.FileName);
            }

            return BadRequest(exportResult.ErrorMessage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in custom export");
            return BadRequest("Export failed. Please try again.");
        }
    }

    // Helper method to get filtered students
    private async Task<List<Student>> GetFilteredStudents(Dictionary<string, string> filters)
    {
        var query = _context.Students.AsQueryable();

        if (filters.ContainsKey("Course") && !string.IsNullOrEmpty(filters["Course"]))
        {
            query = query.Where(s => s.Course == filters["Course"]);
        }

        if (filters.ContainsKey("Status") && !string.IsNullOrEmpty(filters["Status"]))
        {
            var isActive = filters["Status"].ToLower() == "active";
            query = query.Where(s => s.IsActive == isActive);
        }

        if (filters.ContainsKey("StartDate") && DateTime.TryParse(filters["StartDate"], out var startDate))
        {
            query = query.Where(s => s.CreatedDate >= startDate);
        }

        if (filters.ContainsKey("EndDate") && DateTime.TryParse(filters["EndDate"], out var endDate))
        {
            var endDateTime = endDate.AddDays(1);
            query = query.Where(s => s.CreatedDate < endDateTime);
        }

        return await query.ToListAsync();
    }

    // Existing helper methods...
    private async Task<PaginatedResult<Student>> SearchStudents(StudentSearchViewModel searchModel, bool includeAll = false)
    {
        var query = _context.Students.AsQueryable();

        // Apply filters
        if (!string.IsNullOrWhiteSpace(searchModel.SearchTerm))
        {
            query = query.Where(s =>
                s.FirstName.Contains(searchModel.SearchTerm) ||
                s.LastName.Contains(searchModel.SearchTerm) ||
                s.Email.Contains(searchModel.SearchTerm) ||
                s.EnrollmentNumber.Contains(searchModel.SearchTerm));
        }

        if (searchModel.IsActive.HasValue)
        {
            query = query.Where(s => s.IsActive == searchModel.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchModel.SelectedCourse))
        {
            query = query.Where(s => s.Course == searchModel.SelectedCourse);
        }

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

    private async Task InitializeSearchOptions(StudentSearchViewModel searchModel)
    {
        // Initialize search options...
    }
}
```

### Enhanced View with Export Buttons

```cshtml
@model StudentSearchViewModel
@{
    ViewData["Title"] = "Students";
}

<div class="container-fluid">
    <!-- Header with Export Options -->
    <div class="row mb-4">
        <div class="col">
            <h1>Students</h1>
        </div>
        <div class="col-auto">
            <div class="btn-group">
                <div class="btn-group" role="group">
                    <button type="button" class="btn btn-success dropdown-toggle" data-bs-toggle="dropdown" aria-expanded="false">
                        <i class="bi bi-file-earmark-excel"></i> Export
                    </button>
                    <ul class="dropdown-menu">
                        <li>
                            <form asp-action="ExportAll" method="post" class="d-inline">
                                <button type="submit" class="dropdown-item">
                                    <i class="bi bi-download"></i> Export All Students
                                </button>
                            </form>
                        </li>
                        <li>
                            <form asp-action="ExportFiltered" method="post" class="d-inline">
                                @Html.HiddenFor(m => m.SearchTerm)
                                @Html.HiddenFor(m => m.SelectedCourse)
                                @Html.HiddenFor(m => m.IsActive)
                                <button type="submit" class="dropdown-item">
                                    <i class="bi bi-funnel"></i> Export Filtered Results
                                </button>
                            </form>
                        </li>
                        <li>
                            <button type="button" class="dropdown-item" onclick="exportSelected()">
                                <i class="bi bi-check2-square"></i> Export Selected
                            </button>
                        </li>
                        <li><hr class="dropdown-divider"></li>
                        <li>
                            <button type="button" class="dropdown-item" onclick="showCustomExportModal()">
                                <i class="bi bi-gear"></i> Custom Export
                            </button>
                        </li>
                    </ul>
                </div>
                <button class="btn btn-primary" onclick="toggleAdvancedSearch()">
                    <i class="bi bi-funnel"></i> Advanced Search
                </button>
                <a asp-action="Create" class="btn btn-primary">
                    <i class="bi bi-plus-circle"></i> Add Student
                </a>
            </div>
        </div>
    </div>

    <!-- Export Progress Indicator -->
    <div id="exportProgress" class="alert alert-info" style="display: none;">
        <div class="d-flex align-items-center">
            <div class="spinner-border spinner-border-sm me-2" role="status">
                <span class="visually-hidden">Loading...</span>
            </div>
            <span id="exportStatusText">Preparing export...</span>
        </div>
        <div class="progress mt-2">
            <div class="progress-bar" id="exportProgressBar" role="progressbar" style="width: 0%"></div>
        </div>
    </div>

    <!-- Existing search and results sections... -->
    <div class="card">
        <div class="card-header d-flex justify-content-between align-items-center">
            <h5 class="mb-0">
                Search Results
                @if (Model.Results.TotalCount > 0)
                {
                    <span class="badge bg-primary ms-2">@Model.Results.TotalCount</span>
                }
            </h5>

            <!-- Selection controls -->
            @if (Model.HasResults)
            {
                <div class="d-flex align-items-center">
                    <div class="form-check me-3">
                        <input type="checkbox" class="form-check-input" id="selectAllCheckbox" onchange="toggleAllSelections()">
                        <label class="form-check-label" for="selectAllCheckbox">
                            Select All
                        </label>
                    </div>
                    <span class="text-muted">
                        <span id="selectedCount">0</span> selected
                    </span>
                </div>
            }
        </div>

        <div class="card-body">
            @if (Model.HasResults)
            {
                <div class="table-responsive">
                    <table class="table table-hover" id="studentsTable">
                        <thead class="table-light">
                            <tr>
                                <th style="width: 50px;">
                                    <input type="checkbox" class="form-check-input" id="headerCheckbox" onchange="toggleAllSelections()">
                                </th>
                                <th>Student Name</th>
                                <th>Email</th>
                                <th>Phone</th>
                                <th>Course</th>
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
                                        <input type="checkbox" class="form-check-input row-checkbox" value="@student.Id" onchange="updateSelectionCount()">
                                    </td>
                                    <td>
                                        <strong>@student.FirstName @student.LastName</strong>
                                    </td>
                                    <td>@student.Email</td>
                                    <td>@student.Phone</td>
                                    <td>@student.Course</td>
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

                <!-- Pagination (existing code)... -->
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

<!-- Custom Export Modal -->
<div class="modal fade" id="customExportModal" tabindex="-1" aria-labelledby="customExportModalLabel" aria-hidden="true">
    <div class="modal-dialog modal-lg">
        <div class="modal-content">
            <div class="modal-header">
                <h5 class="modal-title" id="customExportModalLabel">Custom Export Configuration</h5>
                <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
            </div>
            <form id="customExportForm" method="post" asp-action="ExportCustom">
                <div class="modal-body">
                    <div class="row">
                        <div class="col-md-6">
                            <div class="mb-3">
                                <label class="form-label">Export Type</label>
                                <select name="ExportType" class="form-select">
                                    <option value="All">All Students</option>
                                    <option value="Filtered">Filtered Results</option>
                                    <option value="Selected">Selected Students</option>
                                </select>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <div class="mb-3">
                                <label class="form-label">File Format</label>
                                <select name="Format" class="form-select">
                                    <option value="Excel">Excel (.xlsx)</option>
                                    <option value="CSV">CSV (.csv)</option>
                                </select>
                            </div>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Include Options</label>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" name="IncludeHeaders" checked>
                            <label class="form-check-label">Include Headers</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" name="AutoFitColumns" checked>
                            <label class="form-check-label">Auto-fit Columns</label>
                        </div>
                        <div class="form-check">
                            <input class="form-check-input" type="checkbox" name="IncludeTotals">
                            <label class="form-check-label">Include Totals Row</label>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label class="form-label">Column Selection</label>
                        <div class="row">
                            <div class="col-md-6">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" name="Columns" value="FirstName" checked>
                                    <label class="form-check-label">First Name</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" name="Columns" value="LastName" checked>
                                    <label class="form-check-label">Last Name</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" name="Columns" value="Email" checked>
                                    <label class="form-check-label">Email</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" name="Columns" value="Phone" checked>
                                    <label class="form-check-label">Phone</label>
                                </div>
                            </div>
                            <div class="col-md-6">
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" name="Columns" value="Course" checked>
                                    <label class="form-check-label">Course</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" name="Columns" value="EnrollmentNumber" checked>
                                    <label class="form-check-label">Enrollment Number</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" name="Columns" value="DateOfBirth">
                                    <label class="form-check-label">Date of Birth</label>
                                </div>
                                <div class="form-check">
                                    <input class="form-check-input" type="checkbox" name="Columns" value="CreatedDate">
                                    <label class="form-check-label">Created Date</label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
                    <button type="submit" class="btn btn-success">
                        <i class="bi bi-download"></i> Export
                    </button>
                </div>
            </form>
        </div>
    </div>
</div>

@section Scripts {
    <script>
        // Selection management
        function toggleAllSelections() {
            const headerCheckbox = document.getElementById('headerCheckbox');
            const rowCheckboxes = document.querySelectorAll('.row-checkbox');

            rowCheckboxes.forEach(checkbox => {
                checkbox.checked = headerCheckbox.checked;
            });

            updateSelectionCount();
        }

        function updateSelectionCount() {
            const selectedCheckboxes = document.querySelectorAll('.row-checkbox:checked');
            const selectedCount = selectedCheckboxes.length;
            document.getElementById('selectedCount').textContent = selectedCount;

            // Update header checkbox state
            const headerCheckbox = document.getElementById('headerCheckbox');
            const totalCheckboxes = document.querySelectorAll('.row-checkbox');

            if (selectedCount === 0) {
                headerCheckbox.indeterminate = false;
                headerCheckbox.checked = false;
            } else if (selectedCount === totalCheckboxes.length) {
                headerCheckbox.indeterminate = false;
                headerCheckbox.checked = true;
            } else {
                headerCheckbox.indeterminate = true;
                headerCheckbox.checked = false;
            }
        }

        // Export selected students
        function exportSelected() {
            const selectedCheckboxes = document.querySelectorAll('.row-checkbox:checked');
            const selectedIds = Array.from(selectedCheckboxes).map(cb => cb.value);

            if (selectedIds.length === 0) {
                alert('Please select at least one student to export.');
                return;
            }

            showExportProgress();

            fetch('@Url.Action("ExportSelected", "Students")', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                    'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
                },
                body: JSON.stringify({ selectedIds: selectedIds })
            })
            .then(response => {
                if (response.ok) {
                    return response.blob();
                }
                throw new Error('Export failed');
            })
            .then(blob => {
                const url = window.URL.createObjectURL(blob);
                const a = document.createElement('a');
                a.href = url;
                a.download = `Selected_Students_${new Date().toISOString().slice(0,19).replace(/:/g, '-')}.xlsx`;
                document.body.appendChild(a);
                a.click();
                window.URL.revokeObjectURL(url);
                document.body.removeChild(a);
                hideExportProgress();
                showNotification('Export completed successfully!', 'success');
            })
            .catch(error => {
                console.error('Export error:', error);
                hideExportProgress();
                showNotification('Export failed. Please try again.', 'error');
            });
        }

        // Show custom export modal
        function showCustomExportModal() {
            const modal = new bootstrap.Modal(document.getElementById('customExportModal'));
            modal.show();
        }

        // Export progress management
        function showExportProgress() {
            document.getElementById('exportProgress').style.display = 'block';
            let progress = 0;
            const progressBar = document.getElementById('exportProgressBar');
            const statusText = document.getElementById('exportStatusText');

            const interval = setInterval(() => {
                progress += Math.random() * 30;
                if (progress > 90) progress = 90;

                progressBar.style.width = progress + '%';

                if (progress < 30) {
                    statusText.textContent = 'Preparing data...';
                } else if (progress < 60) {
                    statusText.textContent = 'Generating Excel file...';
                } else {
                    statusText.textContent = 'Finalizing export...';
                }
            }, 500);

            // Store interval ID to clear it later
            window.exportProgressInterval = interval;
        }

        function hideExportProgress() {
            if (window.exportProgressInterval) {
                clearInterval(window.exportProgressInterval);
            }
            document.getElementById('exportProgress').style.display = 'none';
        }

        // Notification system
        function showNotification(message, type) {
            const alertClass = type === 'success' ? 'alert-success' : 'alert-danger';
            const alertHtml = `
                <div class="alert ${alertClass} alert-dismissible fade show" role="alert">
                    ${message}
                    <button type="button" class="btn-close" data-bs-dismiss="alert"></button>
                </div>
            `;

            const container = document.querySelector('.container-fluid');
            container.insertAdjacentHTML('afterbegin', alertHtml);

            // Auto-dismiss after 5 seconds
            setTimeout(() => {
                const alert = container.querySelector('.alert');
                if (alert) {
                    const bsAlert = new bootstrap.Alert(alert);
                    bsAlert.close();
                }
            }, 5000);
        }

        // Initialize
        document.addEventListener('DOMContentLoaded', function() {
            updateSelectionCount();
        });
    </script>
}
```

### Program.cs Configuration for Excel Export

```csharp
using YourProjectName.Services;
using OfficeOpenXml;

var builder = WebApplication.CreateBuilder(args);

// Configure EPPlus license
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add services to the container
builder.Services.AddControllersWithViews();

// Register Excel Export Service
builder.Services.AddScoped<IExcelExportService, ExcelExportService>();

// Add DbContext
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

## 5. Use Cases

- **Educational Institutions**: Exporting class lists, grade reports, attendance records, and student directories
- **E-commerce**: Exporting sales reports, customer lists, inventory data, and order histories
- **Healthcare**: Exporting patient lists, appointment schedules, medical records, and billing data
- **Financial Services**: Exporting transaction reports, account statements, audit logs, and compliance data
- **HR Management**: Exporting employee directories, payroll reports, performance data, and attendance records
- **Inventory Management**: Exporting stock levels, supplier information, sales data, and purchase orders
- **Project Management**: Exporting task lists, resource allocations, timelines, and progress reports

## 6. Mini Practice Task

1. **Basic Excel Export**:
   - Install EPPlus NuGet package
   - Create simple export method for Student entity
   - Add export button that downloads all students as Excel file
   - Include basic formatting (headers, autofit columns)

2. **Enhanced Export Features**:
   - Implement filtered search results export
   - Add selected records export with checkboxes
   - Include professional styling and formatting
   - Add progress indicators for large exports

3. **Advanced Export Functionality**:
   - Create custom export modal with column selection
   - Implement multiple export formats (Excel, CSV, PDF)
   - Add export scheduling and background processing
   - Create export templates and saved configurations
   - Implement export history and download management
   - Add data validation and export preview functionality