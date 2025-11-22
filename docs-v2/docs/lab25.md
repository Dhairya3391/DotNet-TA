---
title: Lab 25 - Implementation of Insert and Delete Functionality
sidebar_position: 25
---

# Implementation of Insert and Delete Functionality

## 1. Description

Insert (Create) functionality adds new records to the database through forms with validation. Delete functionality removes records from the database, typically requiring user confirmation to prevent accidental data loss. Both operations are essential CRUD (Create, Read, Update, Delete) operations in data management applications.

## 2. Why It Is Important

Insert and Delete operations are fundamental to data management. Insert allows users to add new information (students, products, orders), while Delete enables data cleanup and removal of obsolete records. Proper implementation ensures data integrity, prevents duplicate entries, and provides safeguards against accidental deletions.

## 3. Real-World Examples

- Student enrollment system adding new students and removing graduates
- E-commerce platform adding new products and discontinuing old ones
- Hospital management admitting new patients and archiving discharged patients
- Employee management hiring new staff and processing resignations
- Inventory system adding new stock and removing expired items
- Blog platform creating new posts and removing inappropriate content

## 4. Syntax & Explanation

### Controller with Insert and Delete Operations

```csharp
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using YourProjectName.Data;
using YourProjectName.Models;
using System.ComponentModel.DataAnnotations;

public class StudentsController : Controller
{
    private readonly AppDbContext _context;

    public StudentsController(AppDbContext context)
    {
        _context = context;
    }

    // GET: Students/Create - Display form for new record
    public IActionResult Create()
    {
        // Initialize with default values if needed
        var model = new Student
        {
            IsActive = true,
            CreatedDate = DateTime.Now
        };
        return View(model);
    }

    // POST: Students/Create - Process form submission
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Phone,DateOfBirth,Course,EnrollmentNumber")] Student student)
    {
        try
        {
            // Check for duplicate enrollment number
            var existingEnrollment = await _context.Students
                .AnyAsync(s => s.EnrollmentNumber == student.EnrollmentNumber);

            if (existingEnrollment)
            {
                ModelState.AddModelError("EnrollmentNumber", "Enrollment number already exists.");
            }

            // Check for duplicate email
            var existingEmail = await _context.Students
                .AnyAsync(s => s.Email == student.Email);

            if (existingEmail)
            {
                ModelState.AddModelError("Email", "Email already registered.");
            }

            // Custom validation: Date of Birth must be in the past
            if (student.DateOfBirth >= DateTime.Today)
            {
                ModelState.AddModelError("DateOfBirth", "Date of birth must be in the past.");
            }

            // Custom validation: Age must be between 15 and 100
            var age = DateTime.Today.Year - student.DateOfBirth.Year;
            if (student.DateOfBirth > DateTime.Today.AddYears(-age)) age--;
            if (age < 15 || age > 100)
            {
                ModelState.AddModelError("DateOfBirth", "Student age must be between 15 and 100 years.");
            }

            if (ModelState.IsValid)
            {
                // Set additional fields
                student.IsActive = true;
                student.CreatedDate = DateTime.Now;

                _context.Add(student);
                await _context.SaveChangesAsync();

                // Add success message
                TempData["Success"] = $"Student {student.FirstName} {student.LastName} has been successfully added.";

                return RedirectToAction(nameof(Index));
            }
        }
        catch (DbUpdateException ex)
        {
            ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while saving the student record.");
        }

        return View(student);
    }

    // GET: Students/Delete/5 - Display confirmation page
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _context.Students
            .FirstOrDefaultAsync(m => m.Id == id);

        if (student == null)
        {
            return NotFound();
        }

        // Check if student has related records (enrollments, grades, etc.)
        var hasRelatedRecords = await _context.Enrollments
            .AnyAsync(e => e.StudentId == student.Id);

        if (hasRelatedRecords)
        {
            ViewBag.HasRelatedRecords = true;
            ViewBag.RelatedRecords = await _context.Enrollments
                .Where(e => e.StudentId == student.Id)
                .Include(e => e.Course)
                .ToListAsync();
        }

        return View(student);
    }

    // POST: Students/Delete/5 - Confirm and execute deletion
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Check for related records before deletion
            var hasRelatedRecords = await _context.Enrollments
                .AnyAsync(e => e.StudentId == student.Id);

            if (hasRelatedRecords)
            {
                ModelState.AddModelError("", "Cannot delete student with existing enrollments. Please remove enrollments first.");
                return View(student);
            }

            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            // Add success message
            TempData["Success"] = $"Student {student.FirstName} {student.LastName} has been successfully deleted.";

            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException ex)
        {
            // Log the error (uncomment to enable)
            // _logger.LogError(ex, "Error deleting student");

            ModelState.AddModelError("", "Unable to delete student. The record may be referenced by other records.");

            // Reload the student to show in view
            var student = await _context.Students.FindAsync(id);
            return View(student);
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", "An error occurred while deleting the student record.");

            var student = await _context.Students.FindAsync(id);
            return View(student);
        }
    }

    // Soft Delete (alternative to hard delete)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> SoftDelete(int id)
    {
        try
        {
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }

            // Mark as inactive instead of deleting
            student.IsActive = false;
            student.UpdatedDate = DateTime.Now;

            _context.Update(student);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"Student {student.FirstName} {student.LastName} has been deactivated.";

            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = "An error occurred while deactivating the student.";
            return RedirectToAction(nameof(Index));
        }
    }

    // Bulk Delete (delete multiple records)
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> BulkDelete(int[] selectedIds)
    {
        if (selectedIds == null || selectedIds.Length == 0)
        {
            TempData["Error"] = "No students selected for deletion.";
            return RedirectToAction(nameof(Index));
        }

        try
        {
            var studentsToDelete = await _context.Students
                .Where(s => selectedIds.Contains(s.Id))
                .ToListAsync();

            // Check for related records
            var studentsWithEnrollments = await _context.Enrollments
                .Where(e => selectedIds.Contains(e.StudentId))
                .Select(e => e.StudentId)
                .Distinct()
                .ToListAsync();

            if (studentsWithEnrollments.Any())
            {
                var blockedStudents = studentsToDelete
                    .Where(s => studentsWithEnrollments.Contains(s.Id))
                    .Select(s => $"{s.FirstName} {s.LastName}")
                    .ToList();

                TempData["Error"] = $"Cannot delete students with existing enrollments: {string.Join(", ", blockedStudents)}";
                return RedirectToAction(nameof(Index));
            }

            _context.Students.RemoveRange(studentsToDelete);
            await _context.SaveChangesAsync();

            TempData["Success"] = $"{studentsToDelete.Count} student(s) have been successfully deleted.";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = "An error occurred during bulk deletion.";
            return RedirectToAction(nameof(Index));
        }
    }
}
```

### Create View (Create.cshtml)

```cshtml
@model Student
@{
    ViewData["Title"] = "Add New Student";
}

<div class="container">
    <div class="row mb-4">
        <div class="col">
            <h1>Add New Student</h1>
        </div>
        <div class="col-auto">
            <a asp-action="Index" class="btn btn-outline-secondary">Back to List</a>
        </div>
    </div>

    <div class="row">
        <div class="col-md-8">
            <div class="card">
                <div class="card-header">
                    <h5 class="mb-0">Student Information</h5>
                </div>
                <div class="card-body">
                    <form asp-action="Create">
                        <div asp-validation-summary="ModelOnly" class="alert alert-danger"></div>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label asp-for="FirstName" class="form-label"></label>
                                <input asp-for="FirstName" class="form-control" placeholder="Enter first name" />
                                <span asp-validation-for="FirstName" class="text-danger"></span>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label asp-for="LastName" class="form-label"></label>
                                <input asp-for="LastName" class="form-control" placeholder="Enter last name" />
                                <span asp-validation-for="LastName" class="text-danger"></span>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label asp-for="Email" class="form-label"></label>
                                <input asp-for="Email" class="form-control" placeholder="student@example.com" />
                                <span asp-validation-for="Email" class="text-danger"></span>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label asp-for="Phone" class="form-label"></label>
                                <input asp-for="Phone" class="form-control" placeholder="1234567890" />
                                <span asp-validation-for="Phone" class="text-danger"></span>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col-md-6 mb-3">
                                <label asp-for="DateOfBirth" class="form-label"></label>
                                <input asp-for="DateOfBirth" class="form-control" type="date" />
                                <span asp-validation-for="DateOfBirth" class="text-danger"></span>
                                <small class="form-text text-muted">Student must be between 15-100 years old</small>
                            </div>
                            <div class="col-md-6 mb-3">
                                <label asp-for="Course" class="form-label"></label>
                                <select asp-for="Course" class="form-select">
                                    <option value="">Select Course</option>
                                    <option value="Computer Science">Computer Science</option>
                                    <option value="Information Technology">Information Technology</option>
                                    <option value="Electronics">Electronics</option>
                                    <option value="Mechanical">Mechanical</option>
                                    <option value="Civil">Civil</option>
                                    <option value="Chemical">Chemical</option>
                                </select>
                                <span asp-validation-for="Course" class="text-danger"></span>
                            </div>
                        </div>

                        <div class="col-md-6 mb-3">
                            <label asp-for="EnrollmentNumber" class="form-label"></label>
                            <input asp-for="EnrollmentNumber" class="form-control" placeholder="CS2024001" />
                            <span asp-validation-for="EnrollmentNumber" class="text-danger"></span>
                            <small class="form-text text-muted">Unique enrollment number for the student</small>
                        </div>

                        <div class="form-check mb-3">
                            <input asp-for="IsActive" class="form-check-input" type="checkbox" checked disabled />
                            <label asp-for="IsActive" class="form-check-label">
                                Active Student (New students are active by default)
                            </label>
                        </div>

                        <div class="d-grid gap-2 d-md-flex justify-content-md-end">
                            <a asp-action="Index" class="btn btn-outline-secondary">Cancel</a>
                            <button type="submit" class="btn btn-primary">
                                <i class="bi bi-plus-circle"></i> Add Student
                            </button>
                        </div>
                    </form>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card">
                <div class="card-header">
                    <h6 class="mb-0">Help & Guidelines</h6>
                </div>
                <div class="card-body">
                    <h6>Required Fields:</h6>
                    <ul class="small">
                        <li>First Name and Last Name</li>
                        <li>Valid Email Address</li>
                        <li>Enrollment Number (unique)</li>
                        <li>Course Selection</li>
                        <li>Date of Birth</li>
                    </ul>

                    <h6>Validation Rules:</h6>
                    <ul class="small">
                        <li>Email must be unique</li>
                        <li>Enrollment number must be unique</li>
                        <li>Age must be 15-100 years</li>
                        <li>Phone: 10 digits recommended</li>
                    </ul>

                    <div class="alert alert-info small">
                        <strong>Note:</strong> All new students are marked as active by default. You can deactivate students later if needed.
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
}
```

### Delete Confirmation View (Delete.cshtml)

```cshtml
@model Student
@{
    ViewData["Title"] = "Delete Student";
}

<div class="container">
    <div class="row mb-4">
        <div class="col">
            <h1>Delete Student</h1>
        </div>
        <div class="col-auto">
            <a asp-action="Index" class="btn btn-outline-secondary">Back to List</a>
        </div>
    </div>

    @if (ViewBag.HasRelatedRecords == true)
    {
        <div class="alert alert-warning">
            <h5><i class="bi bi-exclamation-triangle"></i> Cannot Delete</h5>
            <p>This student has related records and cannot be deleted. You must first remove the following records:</p>
            <ul>
                @foreach (var enrollment in ViewBag.RelatedRecords)
                {
                    <li><strong>@enrollment.Course.CourseName</strong> - Enrolled on @enrollment.EnrollmentDate.ToString("MMM dd, yyyy")</li>
                }
            </ul>
            <p>Alternatively, you can <strong>deactivate</strong> the student instead of deleting them.</p>
        </div>

        <div class="text-center">
            <form asp-action="SoftDelete" asp-route-id="@Model.Id" method="post">
                <button type="submit" class="btn btn-warning">
                    <i class="bi bi-pause-circle"></i> Deactivate Student
                </button>
                <a asp-action="Index" class="btn btn-secondary">Cancel</a>
            </form>
        </div>
    }
    else
    {
        <div class="alert alert-danger">
            <h5><i class="bi bi-exclamation-triangle-fill"></i> Confirm Deletion</h5>
            <p>Are you sure you want to delete this student? This action cannot be undone.</p>
        </div>

        <div class="card">
            <div class="card-header">
                <h5 class="mb-0">Student Information</h5>
            </div>
            <div class="card-body">
                <dl class="row">
                    <dt class="col-sm-3">Name:</dt>
                    <dd class="col-sm-9">@Model.FirstName @Model.LastName</dd>

                    <dt class="col-sm-3">Email:</dt>
                    <dd class="col-sm-9">@Model.Email</dd>

                    <dt class="col-sm-3">Phone:</dt>
                    <dd class="col-sm-9">@Model.Phone</dd>

                    <dt class="col-sm-3">Course:</dt>
                    <dd class="col-sm-9">@Model.Course</dd>

                    <dt class="col-sm-3">Enrollment Number:</dt>
                    <dd class="col-sm-9">@Model.EnrollmentNumber</dd>

                    <dt class="col-sm-3">Date of Birth:</dt>
                    <dd class="col-sm-9">@Model.DateOfBirth.ToString("MMM dd, yyyy")</dd>

                    <dt class="col-sm-3">Created Date:</dt>
                    <dd class="col-sm-9">@Model.CreatedDate.ToString("MMM dd, yyyy HH:mm")</dd>
                </dl>
            </div>
        </div>

        <form asp-action="DeleteConfirmed" asp-route-id="@Model.Id" method="post" class="mt-4">
            <div class="d-flex justify-content-center gap-2">
                <button type="submit" class="btn btn-danger">
                    <i class="bi bi-trash"></i> Delete Student
                </button>
                <a asp-action="Details" asp-route-id="@Model.Id" class="btn btn-outline-primary">
                    <i class="bi bi-eye"></i> View Details
                </a>
                <a asp-action="Index" class="btn btn-outline-secondary">
                    <i class="bi bi-x-circle"></i> Cancel
                </a>
            </div>
        </form>
    }
</div>
```

### Enhanced Index View with Bulk Operations

```cshtml
@* Add these sections to the existing Index.cshtml view *@

<!-- Bulk Actions Section -->
@if (Model != null && Model.Any())
{
    <div class="row mb-3">
        <div class="col">
            <div class="btn-group" role="group">
                <button type="button" class="btn btn-outline-danger" id="bulkDeleteBtn" disabled>
                    <i class="bi bi-trash"></i> Delete Selected
                </button>
                <button type="button" class="btn btn-outline-secondary" id="selectAllBtn">
                    <i class="bi bi-check-all"></i> Select All
                </button>
                <button type="button" class="btn btn-outline-secondary" id="clearSelectionBtn">
                    <i class="bi bi-x-square"></i> Clear Selection
                </button>
            </div>
        </div>
    </div>
}

<!-- Modified Table with Checkboxes -->
<table class="table table-striped table-hover">
    <thead class="table-dark">
        <tr>
            <th style="width: 50px;">
                <input type="checkbox" id="selectAllCheckbox" class="form-check-input" />
            </th>
            <th>@Html.DisplayNameFor(model => model.FirstName)</th>
            <th>@Html.DisplayNameFor(model => model.LastName)</th>
            <th>@Html.DisplayNameFor(model => model.Email)</th>
            <th>@Html.DisplayNameFor(model => model.Course)</th>
            <th>Actions</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model)
        {
            <tr>
                <td>
                    <input type="checkbox" class="row-checkbox form-check-input" value="@item.Id" />
                </td>
                <td>@Html.DisplayFor(modelItem => item.FirstName)</td>
                <td>@Html.DisplayFor(modelItem => item.LastName)</td>
                <td>@Html.DisplayFor(modelItem => item.Email)</td>
                <td>@Html.DisplayFor(modelItem => item.Course)</td>
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

<!-- Bulk Delete Form (Hidden) -->
<form id="bulkDeleteForm" asp-action="BulkDelete" method="post" style="display: none;">
    <input type="hidden" name="selectedIds" id="selectedIdsInput" />
</form>

@section Scripts {
    <script>
        // Checkbox management
        const selectAllCheckbox = document.getElementById('selectAllCheckbox');
        const rowCheckboxes = document.querySelectorAll('.row-checkbox');
        const bulkDeleteBtn = document.getElementById('bulkDeleteBtn');
        const selectedIdsInput = document.getElementById('selectedIdsInput');
        const bulkDeleteForm = document.getElementById('bulkDeleteForm');

        // Update bulk delete button state
        function updateBulkDeleteButton() {
            const selectedCount = document.querySelectorAll('.row-checkbox:checked').length;
            bulkDeleteBtn.disabled = selectedCount === 0;
            bulkDeleteBtn.innerHTML = `<i class="bi bi-trash"></i> Delete Selected (${selectedCount})`;
        }

        // Select all functionality
        selectAllCheckbox.addEventListener('change', function() {
            rowCheckboxes.forEach(checkbox => {
                checkbox.checked = this.checked;
            });
            updateBulkDeleteButton();
        });

        // Individual checkbox changes
        rowCheckboxes.forEach(checkbox => {
            checkbox.addEventListener('change', function() {
                // Update select all checkbox state
                const allChecked = Array.from(rowCheckboxes).every(cb => cb.checked);
                const someChecked = Array.from(rowCheckboxes).some(cb => cb.checked);
                selectAllCheckbox.checked = allChecked;
                selectAllCheckbox.indeterminate = someChecked && !allChecked;

                updateBulkDeleteButton();
            });
        });

        // Select All button
        document.getElementById('selectAllBtn').addEventListener('click', function() {
            rowCheckboxes.forEach(checkbox => checkbox.checked = true);
            selectAllCheckbox.checked = true;
            updateBulkDeleteButton();
        });

        // Clear Selection button
        document.getElementById('clearSelectionBtn').addEventListener('click', function() {
            rowCheckboxes.forEach(checkbox => checkbox.checked = false);
            selectAllCheckbox.checked = false;
            updateBulkDeleteButton();
        });

        // Bulk Delete functionality
        bulkDeleteBtn.addEventListener('click', function() {
            const selectedIds = Array.from(document.querySelectorAll('.row-checkbox:checked'))
                .map(cb => parseInt(cb.value));

            if (selectedIds.length === 0) {
                alert('Please select at least one student to delete.');
                return;
            }

            if (confirm(`Are you sure you want to delete ${selectedIds.length} student(s)? This action cannot be undone.`)) {
                selectedIdsInput.value = JSON.stringify(selectedIds);
                bulkDeleteForm.submit();
            }
        });

        initializeCheckboxes();
    </script>
}
```

## 5. Use Cases

- **Student Management**: Enrolling new students and removing graduates/transferred students
- **E-commerce**: Adding new products and discontinuing obsolete inventory
- **HR Systems**: Hiring new employees and processing departures
- **Content Management**: Publishing new articles and removing outdated content
- **Inventory Management**: Adding new stock items and removing expired/damaged goods
- **Customer Management**: Onboarding new clients and removing inactive accounts
- **Event Management**: Creating new events and canceling scheduled events

## 6. Mini Practice Task

1. **Basic Create/Delete**:

   - Implement Create form for a Product entity with validation
   - Add Delete confirmation with proper checks for related records
   - Include success/error messages using TempData

2. **Enhanced Validation**:

   - Add custom validation rules (unique product codes, price ranges)
   - Implement client-side validation with JavaScript
   - Add form auto-save functionality

3. **Advanced Features**:
   - Implement bulk delete with checkbox selection
   - Add soft delete functionality (deactivate instead of permanent delete)
   - Create audit trail for insert/delete operations
   - Add file upload support during record creation (profile pictures, product images)
