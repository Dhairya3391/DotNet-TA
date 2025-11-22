---
title: Lab 26 - Implementation of Update Functionality
sidebar_position: 26
---
# Implementation of Update Functionality

## 1. Description
Update functionality modifies existing records in the database through pre-populated forms that allow users to edit data. Unlike Insert (create) and Delete operations, Update requires loading existing data, validating changes, handling concurrency conflicts, and maintaining data integrity during the modification process.

## 2. Why It Is Important
Update operations are essential for maintaining accurate, current information. They allow users to correct errors, update status changes, modify contact information, and reflect real-world changes in the system. Proper update implementation prevents data corruption, handles concurrent access, and provides audit trails for tracking changes.

## 3. Real-World Examples
- Student management updating contact information, course changes, or status updates
- E-commerce platform modifying product prices, descriptions, or inventory levels
- HR systems updating employee details, salary changes, or department transfers
- Healthcare applications modifying patient records, treatment plans, or medication changes
- Inventory management updating stock levels, supplier information, or pricing
- Content management editing blog posts, product descriptions, or website content

## 4. Syntax & Explanation

### Enhanced Student Model for Updates

```csharp
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Student
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; }

    [StringLength(20)]
    [Phone]
    public string Phone { get; set; }

    [DataType(DataType.Date)]
    public DateTime DateOfBirth { get; set; }

    [StringLength(50)]
    public string Course { get; set; }

    [StringLength(10)]
    public string EnrollmentNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? UpdatedDate { get; set; }

    // Concurrency token for optimistic concurrency
    [Timestamp]
    public byte[] RowVersion { get; set; }

    // Audit fields
    public string? LastModifiedBy { get; set; }
    public string? ModificationReason { get; set; }

    // Navigation properties
    public virtual ICollection<Enrollment> Enrollments { get; set; }
}

// ViewModel for edit operations
public class StudentEditViewModel
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required]
    [EmailAddress]
    [StringLength(255)]
    [Display(Name = "Email Address")]
    public string Email { get; set; }

    [StringLength(20)]
    [Phone]
    [Display(Name = "Phone Number")]
    public string Phone { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime DateOfBirth { get; set; }

    [StringLength(50)]
    [Display(Name = "Course")]
    public string Course { get; set; }

    [StringLength(10)]
    [Display(Name = "Enrollment Number")]
    public string EnrollmentNumber { get; set; }

    [Display(Name = "Active Status")]
    public bool IsActive { get; set; }

    // Fields for audit trail
    [StringLength(500)]
    [Display(Name = "Reason for Change")]
    public string ModificationReason { get; set; }

    // For concurrency handling
    public byte[] RowVersion { get; set; }

    // Original values for change tracking
    public string OriginalEmail { get; set; }
    public string OriginalEnrollmentNumber { get; set; }
    public string OriginalCourse { get; set; }
}
```

### Controller with Update Operations

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourProjectName.Data;
using YourProjectName.Models;
using System.Security.Claims;

public class StudentsController : Controller
{
    private readonly AppDbContext _context;
    private readonly ILogger<StudentsController> _logger;

    public StudentsController(AppDbContext context, ILogger<StudentsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: Students/Edit/5 - Display edit form with existing data
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        try
        {
            var student = await _context.Students
                .AsNoTracking()
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }

            // Create view model with original values for change tracking
            var viewModel = new StudentEditViewModel
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                Email = student.Email,
                OriginalEmail = student.Email,
                Phone = student.Phone,
                DateOfBirth = student.DateOfBirth,
                Course = student.Course,
                OriginalCourse = student.Course,
                EnrollmentNumber = student.EnrollmentNumber,
                OriginalEnrollmentNumber = student.EnrollmentNumber,
                IsActive = student.IsActive,
                RowVersion = student.RowVersion
            };

            return View(viewModel);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error loading student for editing");
            ModelState.AddModelError("", "Unable to load student data. Please try again.");
            return RedirectToAction(nameof(Index));
        }
    }

    // POST: Students/Edit/5 - Process the update
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, StudentEditViewModel viewModel)
    {
        if (id != viewModel.Id)
        {
            return NotFound();
        }

        try
        {
            // Load the original student with RowVersion for concurrency
            var studentToUpdate = await _context.Students
                .FirstOrDefaultAsync(s => s.Id == id);

            if (studentToUpdate == null)
            {
                return NotFound();
            }

            // Check for concurrency conflicts
            if (studentToUpdate.RowVersion == null || viewModel.RowVersion == null ||
                !studentToUpdate.RowVersion.SequenceEqual(viewModel.RowVersion))
            {
                ModelState.AddModelError(string.Empty, "The record has been modified by another user. Please reload and try again.");
                return View(viewModel);
            }

            // Custom validation
            await ValidateStudentUpdate(viewModel, studentToUpdate);

            if (ModelState.IsValid)
            {
                // Track changes for audit
                var changes = new List<string>();

                // Update fields and track changes
                if (studentToUpdate.FirstName != viewModel.FirstName)
                {
                    changes.Add($"FirstName: '{studentToUpdate.FirstName}' → '{viewModel.FirstName}'");
                    studentToUpdate.FirstName = viewModel.FirstName;
                }

                if (studentToUpdate.LastName != viewModel.LastName)
                {
                    changes.Add($"LastName: '{studentToUpdate.LastName}' → '{viewModel.LastName}'");
                    studentToUpdate.LastName = viewModel.LastName;
                }

                if (studentToUpdate.Email != viewModel.Email)
                {
                    changes.Add($"Email: '{studentToUpdate.Email}' → '{viewModel.Email}'");
                    studentToUpdate.Email = viewModel.Email;
                }

                if (studentToUpdate.Phone != viewModel.Phone)
                {
                    changes.Add($"Phone: '{studentToUpdate.Phone}' → '{viewModel.Phone}'");
                    studentToUpdate.Phone = viewModel.Phone;
                }

                if (studentToUpdate.DateOfBirth != viewModel.DateOfBirth)
                {
                    changes.Add($"DateOfBirth: '{studentToUpdate.DateOfBirth:yyyy-MM-dd}' → '{viewModel.DateOfBirth:yyyy-MM-dd}'");
                    studentToUpdate.DateOfBirth = viewModel.DateOfBirth;
                }

                if (studentToUpdate.Course != viewModel.Course)
                {
                    changes.Add($"Course: '{studentToUpdate.Course}' → '{viewModel.Course}'");
                    studentToUpdate.Course = viewModel.Course;
                }

                if (studentToUpdate.EnrollmentNumber != viewModel.EnrollmentNumber)
                {
                    changes.Add($"EnrollmentNumber: '{studentToUpdate.EnrollmentNumber}' → '{viewModel.EnrollmentNumber}'");
                    studentToUpdate.EnrollmentNumber = viewModel.EnrollmentNumber;
                }

                if (studentToUpdate.IsActive != viewModel.IsActive)
                {
                    changes.Add($"IsActive: '{studentToUpdate.IsActive}' → '{viewModel.IsActive}'");
                    studentToUpdate.IsActive = viewModel.IsActive;
                }

                // Update audit fields
                studentToUpdate.UpdatedDate = DateTime.Now;
                studentToUpdate.LastModifiedBy = User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";
                studentToUpdate.ModificationReason = viewModel.ModificationReason;

                // Update RowVersion for concurrency
                studentToUpdate.RowVersion = null; // Will be updated by database

                _context.Update(studentToUpdate);
                await _context.SaveChangesAsync();

                // Log the changes
                if (changes.Any())
                {
                    _logger.LogInformation("Student {StudentId} updated. Changes: {Changes}",
                        studentToUpdate.Id, string.Join("; ", changes));
                }

                TempData["Success"] = $"Student {viewModel.FirstName} {viewModel.LastName} has been successfully updated.";

                return RedirectToAction(nameof(Index));
            }
        }
        catch (DbUpdateConcurrencyException ex)
        {
            var entry = ex.Entries.Single();
            var clientValues = (Student)entry.Entity;
            var databaseEntry = entry.GetDatabaseValues();

            if (databaseEntry == null)
            {
                ModelState.AddModelError(string.Empty, "Unable to save changes. The student was deleted by another user.");
            }
            else
            {
                var databaseValues = (Student)databaseEntry.ToObject();
                ModelState.AddModelError(string.Empty, "The record has been modified by another user. Please reload and try again.");

                // Show the differences
                if (databaseValues.FirstName != clientValues.FirstName)
                    ModelState.AddModelError("FirstName", $"Current value: {databaseValues.FirstName}");
                if (databaseValues.LastName != clientValues.LastName)
                    ModelState.AddModelError("LastName", $"Current value: {databaseValues.LastName}");
                if (databaseValues.Email != clientValues.Email)
                    ModelState.AddModelError("Email", $"Current value: {databaseValues.Email}");
                // Add other fields as needed

                // Update RowVersion to prevent further conflicts
                viewModel.RowVersion = databaseValues.RowVersion;
            }
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Database update error for student {StudentId}", id);
            ModelState.AddModelError("", "Unable to save changes. Please try again.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unexpected error updating student {StudentId}", id);
            ModelState.AddModelError("", "An unexpected error occurred. Please try again.");
        }

        return View(viewModel);
    }

    // Custom validation for student updates
    private async Task ValidateStudentUpdate(StudentEditViewModel viewModel, Student existingStudent)
    {
        // Check for duplicate email (excluding current student)
        if (viewModel.Email != existingStudent.Email)
        {
            var emailExists = await _context.Students
                .AnyAsync(s => s.Email == viewModel.Email && s.Id != viewModel.Id);

            if (emailExists)
            {
                ModelState.AddModelError("Email", "This email is already registered to another student.");
            }
        }

        // Check for duplicate enrollment number (excluding current student)
        if (viewModel.EnrollmentNumber != existingStudent.EnrollmentNumber)
        {
            var enrollmentExists = await _context.Students
                .AnyAsync(s => s.EnrollmentNumber == viewModel.EnrollmentNumber && s.Id != viewModel.Id);

            if (enrollmentExists)
            {
                ModelState.AddModelError("EnrollmentNumber", "This enrollment number is already assigned to another student.");
            }
        }

        // Age validation
        if (viewModel.DateOfBirth >= DateTime.Today)
        {
            ModelState.AddModelError("DateOfBirth", "Date of birth must be in the past.");
        }
        else
        {
            var age = DateTime.Today.Year - viewModel.DateOfBirth.Year;
            if (viewModel.DateOfBirth > DateTime.Today.AddYears(-age)) age--;
            if (age < 15 || age > 100)
            {
                ModelState.AddModelError("DateOfBirth", "Student age must be between 15 and 100 years.");
            }
        }

        // Require modification reason for significant changes
        var significantChanges = new[]
        {
            viewModel.Email != existingStudent.Email,
            viewModel.EnrollmentNumber != existingStudent.EnrollmentNumber,
            viewModel.Course != existingStudent.Course,
            viewModel.IsActive != existingStudent.IsActive
        };

        if (significantChanges.Any(x => x) && string.IsNullOrWhiteSpace(viewModel.ModificationReason))
        {
            ModelState.AddModelError("ModificationReason", "Please provide a reason for these changes.");
        }
    }

    // GET: Students/QuickEdit/5 - Partial update for specific fields
    public async Task<IActionResult> QuickEdit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students
            .FindAsync(id);

        if (student == null)
        {
            return NotFound();
        }

        // Return partial view for AJAX editing
        return PartialView("_QuickEdit", student);
    }

    // POST: Students/QuickUpdate - AJAX partial update
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> QuickUpdate(int id, string phone, bool isActive)
    {
        try
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return Json(new { success = false, message = "Student not found." });
            }

            student.Phone = phone;
            student.IsActive = isActive;
            student.UpdatedDate = DateTime.Now;
            student.LastModifiedBy = User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";

            _context.Update(student);
            await _context.SaveChangesAsync();

            return Json(new {
                success = true,
                message = "Student updated successfully.",
                phone = phone,
                isActive = isActive
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in quick update for student {StudentId}", id);
            return Json(new { success = false, message = "Update failed. Please try again." });
        }
    }

    // POST: Students/BulkUpdate - Update multiple records
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BulkUpdate(int[] selectedIds, string newCourse, bool newStatus)
    {
        if (selectedIds == null || selectedIds.Length == 0)
        {
            TempData["Error"] = "No students selected for update.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var studentsToUpdate = await _context.Students
                .Where(s => selectedIds.Contains(s.Id))
                .ToListAsync();

            if (!studentsToUpdate.Any())
            {
                TempData["Error"] = "No valid students found for update.";
                return RedirectToAction(nameof(Index));
            }

            foreach (var student in studentsToUpdate)
            {
                student.Course = newCourse;
                student.IsActive = newStatus;
                student.UpdatedDate = DateTime.Now;
                student.LastModifiedBy = User.FindFirstValue(ClaimTypes.Name) ?? "Unknown";
                student.ModificationReason = "Bulk update operation";
            }

            _context.UpdateRange(studentsToUpdate);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{studentsToUpdate.Count} student(s) have been successfully updated.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error in bulk update operation");
            TempData["Error"] = "Bulk update failed. Please try again.";
            return RedirectToAction(nameof(Index));
        }
    }
}
```

### Edit View (Edit.cshtml)

```cshtml
@model StudentEditViewModel
@{
    ViewData["Title"] = "Edit Student";
}

<div class="container">
    <div class="row mb-4">
        <div class="col">
            <h1>Edit Student</h1>
        </div>
        <div class="col-auto">
            <a asp-action="Index" class="btn btn-outline-secondary">Back to List</a>
        </div>
    </div>

    <div class="row">
        <div class="col-md-8">
            <div class="card">
                <div class="card-header d-flex justify-content-between align-items-center">
                    <h5 class="mb-0">Student Information</h5>
                    <div class="badge bg-info">ID: @Model.Id</div>
                </div>
                <div class="card-body">
                    <form asp-action="Edit" method="post">
                        <div asp-validation-summary="ModelOnly" class="alert alert-danger"></div>

                        <!-- Hidden fields for concurrency and tracking -->
                        <input type="hidden" asp-for="Id" />
                        <input type="hidden" asp-for="RowVersion" />
                        <input type="hidden" asp-for="OriginalEmail" />
                        <input type="hidden" asp-for="OriginalEnrollmentNumber" />
                        <input type="hidden" asp-for="OriginalCourse" />

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label asp-for="FirstName" class="form-label"></label>
                                <input asp-for="FirstName" class="form-control" />
                                <span asp-validation-for="FirstName" class="text-danger"></span>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label asp-for="LastName" class="form-label"></label>
                                <input asp-for="LastName" class="form-control" />
                                <span asp-validation-for="LastName" class="text-danger"></span>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label asp-for="Email" class="form-label"></label>
                                <input asp-for="Email" class="form-control" />
                                <span asp-validation-for="Email" class="text-danger"></span>
                                @if (Model.Email != Model.OriginalEmail)
                                {
                                    <small class="form-text text-warning">
                                        <i class="bi bi-exclamation-triangle"></i> Email will be changed from @Model.OriginalEmail
                                    </small>
                                }
                            </div>
                            <div class="col-md-6 mb-3">
                                <label asp-for="Phone" class="form-label"></label>
                                <input asp-for="Phone" class="form-control" />
                                <span asp-validation-for="Phone" class="text-danger"></span>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label asp-for="DateOfBirth" class="form-label"></label>
                                <input asp-for="DateOfBirth" class="form-control" type="date" />
                                <span asp-validation-for="DateOfBirth" class="text-danger"></span>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label asp-for="Course" class="form-label"></label>
                                <select asp-for="Course" class="form-select">
                                    <option value="">Select Course</option>
                                    <option value="Computer Science" selected="@(Model.Course == "Computer Science")">Computer Science</option>
                                    <option value="Information Technology" selected="@(Model.Course == "Information Technology")">Information Technology</option>
                                    <option value="Electronics" selected="@(Model.Course == "Electronics")">Electronics</option>
                                    <option value="Mechanical" selected="@(Model.Course == "Mechanical")">Mechanical</option>
                                    <option value="Civil" selected="@(Model.Course == "Civil")">Civil</option>
                                    <option value="Chemical" selected="@(Model.Course == "Chemical")">Chemical</option>
                                </select>
                                <span asp-validation-for="Course" class="text-danger"></span>
                                @if (Model.Course != Model.OriginalCourse)
                                {
                                    <small class="form-text text-warning">
                                        <i class="bi bi-exclamation-triangle"></i> Course will be changed from @Model.OriginalCourse
                                    </small>
                                }
                            </div>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label asp-for="EnrollmentNumber" class="form-label"></label>
                            <input asp-for="EnrollmentNumber" class="form-control" />
                            <span asp-validation-for="EnrollmentNumber" class="text-danger"></span>
                            @if (Model.EnrollmentNumber != Model.OriginalEnrollmentNumber)
                            {
                                <small class="form-text text-warning">
                                    <i class="bi bi-exclamation-triangle"></i> Enrollment number will be changed from @Model.OriginalEnrollmentNumber
                                </small>
                            }
                        </div>

                        <div class="form-check mb-3">
                            <input asp-for="IsActive" class="form-check-input" />
                            <label asp-for="IsActive" class="form-check-label">
                                Active Student
                            </label>
                        </div>

                        <div class="mb-3">
                            <label asp-for="ModificationReason" class="form-label"></label>
                            <textarea asp-for="ModificationReason" class="form-control" rows="3"
                                      placeholder="Please provide a reason for these changes..."></textarea>
                            <span asp-validation-for="ModificationReason" class="text-danger"></span>
                            <small class="form-text text-muted">Required for email, enrollment number, course, or status changes</small>
                        </div>

                        <div class="d-flex justify-content-between">
                            <div>
                                <a asp-action="Details" asp-route-id="@Model.Id" class="btn btn-outline-info">
                                    <i class="bi bi-eye"></i> View Details
                                </a>
                                <a asp-action="Delete" asp-route-id="@Model.Id" class="btn btn-outline-danger">
                                    <i class="bi bi-trash"></i> Delete
                                </a>
                            </div>
                            <div>
                                <a asp-action="Index" class="btn btn-outline-secondary">Cancel</a>
                                <button type="submit" class="btn btn-primary">
                                    <i class="bi bi-check-circle"></i> Save Changes
                                </button>
                            </div>
                        </div>
                    </form>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card">
                <div class="card-header">
                    <h6 class="mb-0">Edit Guidelines</h6>
                </div>
                <div class="card-body">
                    <h6>Important Notes:</h6>
                    <ul class="small">
                        <li>Email and Enrollment numbers must be unique</li>
                        <li>Course changes require justification</li>
                        <li>Age validation: 15-100 years</li>
                        <li>All changes are logged for audit purposes</li>
                    </ul>

                    <h6>Change Tracking:</h6>
                    <ul class="small">
                        <li>Modified fields are highlighted</li>
                        <li>Original values shown for reference</li>
                        <li>Modification reason is required for significant changes</li>
                    </ul>

                    @if (Model.UpdatedDate.HasValue)
                    {
                        <div class="alert alert-info small">
                            <strong>Last Updated:</strong><br>
                            @Model.UpdatedDate.Value.ToString("MMM dd, yyyy HH:mm")<br>
                            @if (!string.IsNullOrEmpty(Model.LastModifiedBy))
                            {
                                <span>By: @Model.LastModifiedBy</span>
                            }
                        </div>
                    }
                </div>
            </div>

            <div class="card mt-3">
                <div class="card-header">
                    <h6 class="mb-0">Quick Actions</h6>
                </div>
                <div class="card-body">
                    <div class="d-grid gap-2">
                        <button type="button" class="btn btn-outline-info btn-sm" onclick="showQuickEdit()">
                            <i class="bi bi-lightning"></i> Quick Edit
                        </button>
                        <button type="button" class="btn btn-outline-secondary btn-sm" onclick="resetForm()">
                            <i class="bi bi-arrow-counterclockwise"></i> Reset Form
                        </button>
                        <a asp-action="Details" asp-route-id="@Model.Id" class="btn btn-outline-primary btn-sm">
                            <i class="bi bi-info-circle"></i> Full Details
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>

@section Scripts {
    @{
        await Html.RenderPartialAsync("_ValidationScriptsPartial");
    }
    <script>
        function resetForm() {
            if (confirm('Reset all changes to original values?')) {
                location.reload();
            }
        }

        function showQuickEdit() {
            // Implementation for quick edit modal/popup
            alert('Quick edit functionality would open here');
        }

        // Auto-save draft functionality
        let autoSaveTimer;
        const form = document.querySelector('form');

        form.addEventListener('input', function() {
            clearTimeout(autoSaveTimer);
            autoSaveTimer = setTimeout(function() {
                // Save form data to localStorage
                const formData = new FormData(form);
                const data = {};
                formData.forEach((value, key) => {
                    data[key] = value;
                });
                localStorage.setItem('studentEditDraft', JSON.stringify(data));
                console.log('Draft saved');
            }, 5000); // Save after 5 seconds of inactivity
        });

        // Load draft on page load
        window.addEventListener('load', function() {
            const draft = localStorage.getItem('studentEditDraft');
            if (draft) {
                const data = JSON.parse(draft);
                // Optionally restore form data
                console.log('Draft loaded:', data);
            }
        });

        // Clear draft after successful submission
        @if (TempData["Success"] != null)
        {
            <text>
                localStorage.removeItem('studentEditDraft');
            </text>
        }
    </script>
}
```

### Quick Edit Partial View (_QuickEdit.cshtml)

```cshtml
@model Student
@{
    Layout = null;
}

<div class="modal-header">
    <h5 class="modal-title">Quick Edit - @Model.FirstName @Model.LastName</h5>
    <button type="button" class="btn-close" data-bs-dismiss="modal" aria-label="Close"></button>
</div>
<div class="modal-body">
    <form id="quickEditForm">
        <input type="hidden" name="id" value="@Model.Id" />

        <div class="mb-3">
            <label for="phone" class="form-label">Phone Number</label>
            <input type="text" class="form-control" id="phone" name="phone" value="@Model.Phone" />
        </div>

        <div class="form-check mb-3">
            <input class="form-check-input" type="checkbox" id="isActive" name="isActive" @(Model.IsActive ? "checked" : "") />
            <label class="form-check-label" for="isActive">
                Active Student
            </label>
        </div>
    </form>
</div>
<div class="modal-footer">
    <button type="button" class="btn btn-secondary" data-bs-dismiss="modal">Cancel</button>
    <button type="button" class="btn btn-primary" onclick="saveQuickEdit()">Save Changes</button>
</div>

<script>
    function saveQuickEdit() {
        const form = document.getElementById('quickEditForm');
        const formData = new FormData(form);

        fetch('@Url.Action("QuickUpdate", "Students")', {
            method: 'POST',
            body: formData,
            headers: {
                'RequestVerificationToken': document.querySelector('input[name="__RequestVerificationToken"]').value
            }
        })
        .then(response => response.json())
        .then(data => {
            if (data.success) {
                // Update the table row
                const row = document.querySelector(`tr[data-student-id="${formData.get('id')}"]`);
                if (row) {
                    row.querySelector('.phone-cell').textContent = data.phone;
                    const statusBadge = row.querySelector('.status-badge');
                    if (statusBadge) {
                        statusBadge.className = data.isActive ? 'badge bg-success' : 'badge bg-secondary';
                        statusBadge.textContent = data.isActive ? 'Active' : 'Inactive';
                    }
                }

                // Close modal
                const modal = bootstrap.Modal.getInstance(document.querySelector('.modal'));
                modal.hide();

                // Show success message
                showNotification(data.message, 'success');
            } else {
                showNotification(data.message, 'error');
            }
        })
        .catch(error => {
            showNotification('An error occurred while saving.', 'error');
        });
    }
</script>
```

## 5. Use Cases

- **Student Management**: Updating contact information, changing courses, modifying enrollment status
- **E-commerce**: Updating product prices, descriptions, inventory levels, categories
- **HR Systems**: Modifying employee details, salary changes, department transfers, status updates
- **Healthcare**: Updating patient records, treatment plans, medication changes, appointment scheduling
- **Inventory Management**: Modifying stock levels, supplier information, pricing, product details
- **Content Management**: Editing articles, updating product descriptions, modifying website content
- **Financial Systems**: Updating account details, modifying transaction records, changing customer information

## 6. Mini Practice Task

1. **Basic Update Implementation**:
   - Create edit form for a Product entity with pre-populated data
   - Implement update validation with duplicate checking
   - Add success/error messaging and redirect to list

2. **Enhanced Update Features**:
   - Add concurrency handling with RowVersion
   - Implement change tracking and audit trail
   - Create modification reason requirement for significant changes

3. **Advanced Update Functionality**:
   - Implement quick edit with AJAX for common fields
   - Add bulk update for multiple records
   - Create auto-save draft functionality
   - Add field-level permissions (read-only fields for certain users)
   - Implement approval workflow for critical updates