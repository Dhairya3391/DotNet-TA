# Login and User Registration Operation

## 1. Description
Login functionality authenticates users by verifying their credentials (username/email and password) against stored user data. User Registration creates new user accounts by collecting user information, validating input, hashing passwords, and storing user data securely. Both operations are fundamental to application security and user management.

## 2. Why It Is Important
Authentication and registration are critical security features that protect sensitive data and provide personalized user experiences. Proper implementation ensures only authorized users can access protected resources, prevents unauthorized access, and maintains data integrity. Registration enables user onboarding and account management for multi-user applications.

## 3. Real-World Examples
- Student management system with student and staff login portals
- E-commerce platform with customer registration and login
- Healthcare application with patient and provider authentication
- Banking system with secure customer login and account creation
- Enterprise applications with employee authentication and role-based access
- Social media platforms with user registration and secure login

## 4. Syntax & Explanation

### User Authentication Models

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

// Custom User model extending IdentityUser
public class ApplicationUser : IdentityUser
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }

    [Required]
    [StringLength(100)]
    public string LastName { get; set; }

    [DataType(DataType.Date)]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(200)]
    public string Address { get; set; }

    [StringLength(20)]
    public string PhoneNumber { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; } = DateTime.Now;

    public DateTime? LastLoginDate { get; set; }

    public string? ProfilePicture { get; set; }

    // Navigation properties
    public virtual ICollection<StudentProfile> StudentProfiles { get; set; }
}

// Student profile linked to user account
public class StudentProfile
{
    public int Id { get; set; }
    public string UserId { get; set; }
    public virtual ApplicationUser User { get; set; }

    [StringLength(20)]
    public string EnrollmentNumber { get; set; }

    [StringLength(50)]
    public string Course { get; set; }

    public int AcademicYear { get; set; }
    public bool IsActive { get; set; } = true;
}

// Login view model
public class LoginViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Display(Name = "Remember me")]
    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }
}

// Registration view model
public class RegisterViewModel
{
    [Required(ErrorMessage = "First name is required")]
    [StringLength(100, ErrorMessage = "First name cannot be longer than 100 characters")]
    [Display(Name = "First Name")]
    public string FirstName { get; set; }

    [Required(ErrorMessage = "Last name is required")]
    [StringLength(100, ErrorMessage = "Last name cannot be longer than 100 characters")]
    [Display(Name = "Last Name")]
    public string LastName { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    [Display(Name = "Email")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be at least 8 characters long")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please confirm your password")]
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
    public string ConfirmPassword { get; set; }

    [DataType(DataType.Date)]
    [Display(Name = "Date of Birth")]
    public DateTime? DateOfBirth { get; set; }

    [StringLength(200)]
    [Display(Name = "Address")]
    public string Address { get; set; }

    [StringLength(20)]
    [Phone(ErrorMessage = "Please enter a valid phone number")]
    [Display(Name = "Phone Number")]
    public string PhoneNumber { get; set; }

    [Display(Name = "Enrollment Number")]
    [StringLength(20)]
    public string? EnrollmentNumber { get; set; }

    [Display(Name = "Course")]
    [StringLength(50)]
    public string? Course { get; set; }

    public bool AgreeToTerms { get; set; }
}

// External login model
public class ExternalLoginViewModel
{
    public string Provider { get; set; }
    public string? ReturnUrl { get; set; }
}

// Reset password models
public class ForgotPasswordViewModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; }
}

public class ResetPasswordViewModel
{
    public string Email { get; set; }
    public string Token { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [StringLength(100, MinimumLength = 8)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required(ErrorMessage = "Please confirm your password")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
    public string ConfirmPassword { get; set; }
}
```

### Database Context with Identity

```csharp
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

public class AppDbContext : IdentityDbContext<ApplicationUser>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<StudentProfile> StudentProfiles { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure ApplicationUser
        builder.Entity<ApplicationUser>(entity =>
        {
            entity.Property(e => e.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.LastName).IsRequired().HasMaxLength(100);
            entity.Property(e => e.PhoneNumber).HasMaxLength(20);
            entity.Property(e => e.Address).HasMaxLength(200);
            entity.Property(e => e.ProfilePicture).HasMaxLength(255);
        });

        // Configure StudentProfile
        builder.Entity<StudentProfile>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.EnrollmentNumber).HasMaxLength(20);
            entity.Property(e => e.Course).HasMaxLength(50);
            entity.HasOne(e => e.User)
                  .WithMany(u => u.StudentProfiles)
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
```

### Authentication Controller

```csharp
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using YourProjectName.Data;
using YourProjectName.Models;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ILogger<AccountController> _logger;
    private readonly AppDbContext _context;
    private readonly IEmailSender _emailSender;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ILogger<AccountController> logger,
        AppDbContext context,
        IEmailSender emailSender)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _logger = logger;
        _context = context;
        _emailSender = emailSender;
    }

    // GET: Account/Login
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Login(string? returnUrl = null)
    {
        // Clear the existing external cookie to ensure a clean login process
        HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: Account/Login
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            // Find user by email
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);
            }

            // Check if user is active
            if (!user.IsActive)
            {
                ModelState.AddModelError(string.Empty, "Your account has been deactivated. Please contact administrator.");
                return View(model);
            }

            // Attempt to sign in
            var result = await _signInManager.PasswordSignInAsync(
                user, model.Password, model.RememberMe, lockoutOnFailure: true);

            if (result.Succeeded)
            {
                // Update last login date
                user.LastLoginDate = DateTime.Now;
                await _userManager.UpdateAsync(user);

                _logger.LogInformation("User {Email} logged in", user.Email);

                // Check if URL is local to prevent open redirect attacks
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }

            if (result.IsLockedOut)
            {
                _logger.LogWarning("User account {Email} locked out", user.Email);
                ModelState.AddModelError(string.Empty, "Account has been locked out due to multiple failed login attempts. Please try again later.");
                return View(model);
            }

            if (result.IsNotAllowed)
            {
                ModelState.AddModelError(string.Empty, "Account is not allowed to sign in. Please verify your email.");
                return View(model);
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    // GET: Account/Register
    [HttpGet]
    [AllowAnonymous]
    public IActionResult Register(string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    // POST: Account/Register
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string? returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl;

        if (ModelState.IsValid)
        {
            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            if (existingUser != null)
            {
                ModelState.AddModelError("Email", "An account with this email already exists.");
                return View(model);
            }

            // Create new user
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Address = model.Address,
                PhoneNumber = model.PhoneNumber,
                IsActive = true,
                CreatedDate = DateTime.Now
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                _logger.LogInformation("User {Email} created a new account", model.Email);

                // Add user role (default role)
                await _userManager.AddToRoleAsync(user, "User");

                // Create student profile if enrollment data provided
                if (!string.IsNullOrEmpty(model.EnrollmentNumber) && !string.IsNullOrEmpty(model.Course))
                {
                    var studentProfile = new StudentProfile
                    {
                        UserId = user.Id,
                        EnrollmentNumber = model.EnrollmentNumber,
                        Course = model.Course,
                        AcademicYear = DateTime.Now.Year,
                        IsActive = true
                    };

                    _context.StudentProfiles.Add(studentProfile);
                    await _context.SaveChangesAsync();
                }

                // Generate email confirmation token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var confirmationLink = Url.Action(
                    "ConfirmEmail", "Account",
                    new { userId = user.Id, token = token },
                    protocol: HttpContext.Scheme);

                // Send confirmation email (implementation depends on your email service)
                await _emailSender.SendEmailAsync(
                    model.Email,
                    "Confirm your email",
                    $"Please confirm your account by <a href='{confirmationLink}'>clicking here</a>.");

                // Sign in user (optional - you might require email confirmation first)
                await _signInManager.SignInAsync(user, isPersistent: false);

                _logger.LogInformation("User {Email} created and signed in", model.Email);

                // Redirect to confirmation required page or directly to return URL
                return RedirectToAction("RegisterConfirmation", new { email = model.Email, returnUrl = returnUrl });
            }

            // Add errors to ModelState
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // If we got this far, something failed, redisplay form
        return View(model);
    }

    // GET: Account/ConfirmEmail
    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ConfirmEmail(string userId, string token)
    {
        if (userId == null || token == null)
        {
            return RedirectToAction("Index", "Home");
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user == null)
        {
            return NotFound($"Unable to load user with ID '{userId}'.");
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            return View();
        }

        ModelState.AddModelError(string.Empty, "Error confirming your email.");
        return View("Error");
    }

    // GET: Account/RegisterConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult RegisterConfirmation(string email, string? returnUrl = null)
    {
        if (string.IsNullOrEmpty(email))
        {
            return RedirectToAction("Index", "Home");
        }

        ViewData["ReturnUrl"] = returnUrl;
        ViewData["Email"] = email;
        return View();
    }

    // POST: Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        _logger.LogInformation("User logged out");
        return RedirectToAction("Index", "Home");
    }

    // GET: Account/ForgotPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPassword()
    {
        return View();
    }

    // POST: Account/ForgotPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
    {
        if (ModelState.IsValid)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || !(await _userManager.IsEmailConfirmedAsync(user)))
            {
                // Don't reveal that the user does not exist or is not confirmed
                return RedirectToAction("ForgotPasswordConfirmation");
            }

            // Generate password reset token
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var resetLink = Url.Action(
                "ResetPassword", "Account",
                new { email = model.Email, token = token },
                protocol: HttpContext.Scheme);

            // Send password reset email
            await _emailSender.SendEmailAsync(
                model.Email,
                "Reset Password",
                $"Please reset your password by <a href='{resetLink}'>clicking here</a>.");

            return RedirectToAction("ForgotPasswordConfirmation");
        }

        return View(model);
    }

    // GET: Account/ResetPassword
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPassword(string token = null)
    {
        if (token == null)
        {
            return BadRequest("A token must be supplied for password reset.");
        }

        var model = new ResetPasswordViewModel { Token = token };
        return View(model);
    }

    // POST: Account/ResetPassword
    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var user = await _userManager.FindByEmailAsync(model.Email);
        if (user == null)
        {
            // Don't reveal that the user does not exist
            return RedirectToAction("ResetPasswordConfirmation");
        }

        var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("ResetPasswordConfirmation");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    // GET: Account/ForgotPasswordConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ForgotPasswordConfirmation()
    {
        return View();
    }

    // GET: Account/ResetPasswordConfirmation
    [HttpGet]
    [AllowAnonymous]
    public IActionResult ResetPasswordConfirmation()
    {
        return View();
    }

    // Access Denied page
    [HttpGet]
    public IActionResult AccessDenied()
    {
        return View();
    }

    private IActionResult RedirectToLocal(string returnUrl)
    {
        if (Url.IsLocalUrl(returnUrl))
        {
            return Redirect(returnUrl);
        }
        else
        {
            return RedirectToAction("Index", "Home");
        }
    }
}
```

### Login View (Login.cshtml)

```cshtml
@model LoginViewModel
@{
    ViewData["Title"] = "Login";
    Layout = "_AuthLayout";
}

<div class="row justify-content-center">
    <div class="col-md-6 col-lg-4">
        <div class="card shadow">
            <div class="card-body p-4">
                <div class="text-center mb-4">
                    <h2 class="fw-bold">Welcome Back</h2>
                    <p class="text-muted">Sign in to your account</p>
                </div>

                <form asp-action="Login" method="post">
                    <div asp-validation-summary="ModelOnly" class="alert alert-danger"></div>

                    <input type="hidden" asp-for="ReturnUrl" />

                    <div class="mb-3">
                        <label asp-for="Email" class="form-label"></label>
                        <div class="input-group">
                            <span class="input-group-text">
                                <i class="bi bi-envelope"></i>
                            </span>
                            <input asp-for="Email" class="form-control" placeholder="Enter your email" />
                        </div>
                        <span asp-validation-for="Email" class="text-danger"></span>
                    </div>

                    <div class="mb-3">
                        <label asp-for="Password" class="form-label"></label>
                        <div class="input-group">
                            <span class="input-group-text">
                                <i class="bi bi-lock"></i>
                            </span>
                            <input asp-for="Password" class="form-control" placeholder="Enter your password" />
                            <button class="btn btn-outline-secondary" type="button" id="togglePassword">
                                <i class="bi bi-eye" id="passwordIcon"></i>
                            </button>
                        </div>
                        <span asp-validation-for="Password" class="text-danger"></span>
                    </div>

                    <div class="mb-3 form-check">
                        <input asp-for="RememberMe" class="form-check-input" />
                        <label asp-for="RememberMe" class="form-check-label">
                            Remember me
                        </label>
                    </div>

                    <div class="d-grid">
                        <button type="submit" class="btn btn-primary btn-lg">
                            <i class="bi bi-box-arrow-in-right"></i> Sign In
                        </button>
                    </div>
                </form>

                <div class="mt-4 text-center">
                    <p class="mb-0">
                        <a asp-action="ForgotPassword" class="text-decoration-none">
                            Forgot your password?
                        </a>
                    </p>
                </div>

                @if (User.Identity?.IsAuthenticated != true)
                {
                    <div class="mt-4">
                        <div class="text-center mb-3">
                            <span class="text-muted">Or continue with</span>
                        </div>
                        <div class="d-grid gap-2">
                            <a class="btn btn-outline-primary" href="@Url.Action("ExternalLogin", "Account", new { provider = "Google", returnUrl = Model.ReturnUrl })">
                                <i class="bi bi-google"></i> Continue with Google
                            </a>
                            <a class="btn btn-outline-secondary" href="@Url.Action("ExternalLogin", "Account", new { provider = "Microsoft", returnUrl = Model.ReturnUrl })">
                                <i class="bi bi-microsoft"></i> Continue with Microsoft
                            </a>
                        </div>
                    </div>
                }

                <div class="mt-4 text-center">
                    <p class="text-muted">
                        Don't have an account?
                        <a asp-action="Register" asp-route-returnUrl="@Model.ReturnUrl" class="text-decoration-none">
                            Sign up here
                        </a>
                    </p>
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
        // Toggle password visibility
        document.getElementById('togglePassword').addEventListener('click', function() {
            const passwordInput = document.getElementById('Password');
            const passwordIcon = document.getElementById('passwordIcon');

            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                passwordIcon.className = 'bi bi-eye-slash';
            } else {
                passwordInput.type = 'password';
                passwordIcon.className = 'bi bi-eye';
            }
        });

        // Auto-focus on email field
        document.addEventListener('DOMContentLoaded', function() {
            document.getElementById('Email').focus();
        });
    </script>
}
```

### Registration View (Register.cshtml)

```cshtml
@model RegisterViewModel
@{
    ViewData["Title"] = "Register";
    Layout = "_AuthLayout";
}

<div class="row justify-content-center">
    <div class="col-md-8 col-lg-6">
        <div class="card shadow">
            <div class="card-body p-4">
                <div class="text-center mb-4">
                    <h2 class="fw-bold">Create Account</h2>
                    <p class="text-muted">Join our learning platform today</p>
                </div>

                <form asp-action="Register" method="post">
                    <div asp-validation-summary="ModelOnly" class="alert alert-danger"></div>

                    <input type="hidden" asp-for="ReturnUrl" />

                    <!-- Personal Information -->
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <label asp-for="FirstName" class="form-label"></label>
                            <input asp-for="FirstName" class="form-control" placeholder="First name" />
                            <span asp-validation-for="FirstName" class="text-danger"></span>
                        </div>
                        <div class="col-md-6">
                            <label asp-for="LastName" class="form-label"></label>
                            <input asp-for="LastName" class="form-control" placeholder="Last name" />
                            <span asp-validation-for="LastName" class="text-danger"></span>
                        </div>
                    </div>

                    <div class="mb-3">
                        <label asp-for="Email" class="form-label"></label>
                        <div class="input-group">
                            <span class="input-group-text">
                                <i class="bi bi-envelope"></i>
                            </span>
                            <input asp-for="Email" class="form-control" placeholder="your.email@example.com" />
                        </div>
                        <span asp-validation-for="Email" class="text-danger"></span>
                    </div>

                    <div class="mb-3">
                        <label asp-for="PhoneNumber" class="form-label"></label>
                        <div class="input-group">
                            <span class="input-group-text">
                                <i class="bi bi-telephone"></i>
                            </span>
                            <input asp-for="PhoneNumber" class="form-control" placeholder="1234567890" />
                        </div>
                        <span asp-validation-for="PhoneNumber" class="text-danger"></span>
                    </div>

                    <div class="mb-3">
                        <label asp-for="DateOfBirth" class="form-label"></label>
                        <input asp-for="DateOfBirth" class="form-control" type="date" />
                        <span asp-validation-for="DateOfBirth" class="text-danger"></span>
                    </div>

                    <div class="mb-3">
                        <label asp-for="Address" class="form-label"></label>
                        <textarea asp-for="Address" class="form-control" rows="2" placeholder="Your address"></textarea>
                        <span asp-validation-for="Address" class="text-danger"></span>
                    </div>

                    <!-- Password Section -->
                    <hr class="my-4">
                    <h5 class="mb-3">Password Setup</h5>

                    <div class="row mb-3">
                        <div class="col-md-6">
                            <label asp-for="Password" class="form-label"></label>
                            <div class="input-group">
                                <span class="input-group-text">
                                    <i class="bi bi-lock"></i>
                                </span>
                                <input asp-for="Password" class="form-control" placeholder="Create password" />
                                <button class="btn btn-outline-secondary" type="button" onclick="togglePassword('Password')">
                                    <i class="bi bi-eye"></i>
                                </button>
                            </div>
                            <span asp-validation-for="Password" class="text-danger"></span>
                            <div class="form-text">Password must be at least 8 characters long.</div>
                        </div>
                        <div class="col-md-6">
                            <label asp-for="ConfirmPassword" class="form-label"></label>
                            <div class="input-group">
                                <span class="input-group-text">
                                    <i class="bi bi-lock-fill"></i>
                                </span>
                                <input asp-for="ConfirmPassword" class="form-control" placeholder="Confirm password" />
                                <button class="btn btn-outline-secondary" type="button" onclick="togglePassword('ConfirmPassword')">
                                    <i class="bi bi-eye"></i>
                                </button>
                            </div>
                            <span asp-validation-for="ConfirmPassword" class="text-danger"></span>
                        </div>
                    </div>

                    <!-- Student Information (Optional) -->
                    <hr class="my-4">
                    <h5 class="mb-3">Student Information (Optional)</h5>

                    <div class="row mb-3">
                        <div class="col-md-6">
                            <label asp-for="EnrollmentNumber" class="form-label"></label>
                            <input asp-for="EnrollmentNumber" class="form-control" placeholder="e.g., CS2024001" />
                            <span asp-validation-for="EnrollmentNumber" class="text-danger"></span>
                        </div>
                        <div class="col-md-6">
                            <label asp-for="Course" class="form-label"></label>
                            <select asp-for="Course" class="form-select">
                                <option value="">Select Course</option>
                                <option value="Computer Science">Computer Science</option>
                                <option value="Information Technology">Information Technology</option>
                                <option value="Electronics">Electronics</option>
                                <option value="Mechanical">Mechanical</option>
                                <option value="Civil">Civil</option>
                            </select>
                            <span asp-validation-for="Course" class="text-danger"></span>
                        </div>
                    </div>

                    <!-- Terms and Conditions -->
                    <div class="mb-4">
                        <div class="form-check">
                            <input asp-for="AgreeToTerms" class="form-check-input" />
                            <label asp-for="AgreeToTerms" class="form-check-label">
                                I agree to the <a href="#" class="text-decoration-none">Terms of Service</a>
                                and <a href="#" class="text-decoration-none">Privacy Policy</a>
                            </label>
                        </div>
                        <span asp-validation-for="AgreeToTerms" class="text-danger"></span>
                    </div>

                    <div class="d-grid">
                        <button type="submit" class="btn btn-primary btn-lg">
                            <i class="bi bi-person-plus"></i> Create Account
                        </button>
                    </div>
                </form>

                <div class="mt-4 text-center">
                    <p class="text-muted">
                        Already have an account?
                        <a asp-action="Login" asp-route-returnUrl="@Model.ReturnUrl" class="text-decoration-none">
                            Sign in here
                        </a>
                    </p>
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
        // Toggle password visibility
        function togglePassword(fieldId) {
            const passwordInput = document.getElementById(fieldId);
            const button = passwordInput.nextElementSibling;
            const icon = button.querySelector('i');

            if (passwordInput.type === 'password') {
                passwordInput.type = 'text';
                icon.className = 'bi bi-eye-slash';
            } else {
                passwordInput.type = 'password';
                icon.className = 'bi bi-eye';
            }
        }

        // Password strength indicator
        document.getElementById('Password').addEventListener('input', function() {
            const password = this.value;
            const strength = calculatePasswordStrength(password);
            updatePasswordStrengthIndicator(strength);
        });

        function calculatePasswordStrength(password) {
            if (password.length === 0) return 0;

            let strength = 0;
            if (password.length >= 8) strength++;
            if (password.length >= 12) strength++;
            if (/[a-z]/.test(password) && /[A-Z]/.test(password)) strength++;
            if (/\d/.test(password)) strength++;
            if (/[^a-zA-Z\d]/.test(password)) strength++;

            return strength;
        }

        function updatePasswordStrengthIndicator(strength) {
            const indicator = document.getElementById('passwordStrength');
            if (!indicator) {
                const passwordField = document.getElementById('Password');
                const strengthDiv = document.createElement('div');
                strengthDiv.id = 'passwordStrength';
                strengthDiv.className = 'mt-2';
                passwordField.parentNode.appendChild(strengthDiv);
            }

            const colors = ['danger', 'warning', 'warning', 'info', 'success'];
            const texts = ['Very Weak', 'Weak', 'Fair', 'Good', 'Strong'];

            indicator.innerHTML = `
                <div class="progress" style="height: 5px;">
                    <div class="progress-bar bg-${colors[strength]}" style="width: ${(strength + 1) * 20}%"></div>
                </div>
                <small class="text-${colors[strength]}">Password strength: ${texts[strength]}</small>
            `;
        }

        // Auto-focus on first field
        document.addEventListener('DOMContentLoaded', function() {
            document.getElementById('FirstName').focus();
        });
    </script>
}
```

### Authentication Layout (_AuthLayout.cshtml)

```html
<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - Student Management System</title>
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/css/bootstrap.min.css" rel="stylesheet">
    <link href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.11.0/font/bootstrap-icons.css" rel="stylesheet">
    <link rel="stylesheet" href="~/css/auth.css" />
</head>
<body class="bg-light">
    <nav class="navbar navbar-expand-lg navbar-light bg-white shadow-sm">
        <div class="container">
            <a class="navbar-brand fw-bold" href="/">
                <i class="bi bi-mortarboard-fill text-primary"></i>
                EduPortal
            </a>
            <div class="navbar-nav ms-auto">
                <a class="nav-link" href="/">Home</a>
                <a class="nav-link" href="/About">About</a>
                <a class="nav-link" href="/Contact">Contact</a>
            </div>
        </div>
    </nav>

    <main class="py-5">
        @RenderBody()
    </main>

    <footer class="bg-light py-4 mt-5">
        <div class="container text-center">
            <p class="text-muted mb-0">&copy; 2024 Student Management System. All rights reserved.</p>
        </div>
    </footer>

    <script src="https://cdn.jsdelivr.net/npm/bootstrap@5.3.0/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/lib/jquery/dist/jquery.min.js"></script>
    <script src="~/lib/jquery-validation/dist/jquery.validate.min.js"></script>
    <script src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"></script>
    @RenderSection("Scripts", required: false)
</body>
</html>
```

### Custom CSS for Authentication (auth.css)

```css
.auth-container {
    min-height: 100vh;
    display: flex;
    align-items: center;
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
}

.card {
    border: none;
    border-radius: 15px;
}

.form-control:focus {
    border-color: #667eea;
    box-shadow: 0 0 0 0.2rem rgba(102, 126, 234, 0.25);
}

.btn-primary {
    background: linear-gradient(135deg, #667eea 0%, #764ba2 100%);
    border: none;
    border-radius: 8px;
    padding: 12px 24px;
    font-weight: 600;
    transition: all 0.3s ease;
}

.btn-primary:hover {
    transform: translateY(-2px);
    box-shadow: 0 4px 12px rgba(102, 126, 234, 0.4);
}

.input-group-text {
    background-color: #f8f9fa;
    border-right: none;
}

.form-control {
    border-left: none;
}

.alert {
    border-radius: 10px;
    border: none;
}

.text-decoration-none:hover {
    text-decoration: underline !important;
}

/* Password strength indicator */
.progress {
    border-radius: 10px;
}

/* Responsive adjustments */
@media (max-width: 768px) {
    .card-body {
        padding: 2rem 1.5rem !important;
    }
}
```

## 5. Use Cases

- **Educational Platforms**: Student and staff authentication with role-based access
- **E-commerce Applications**: Customer registration and secure login
- **Healthcare Systems**: Patient and provider authentication with HIPAA compliance
- **Banking Applications**: Secure customer login with multi-factor authentication
- **Enterprise Systems**: Employee authentication with Active Directory integration
- **Social Media Platforms**: User registration with email verification and social login
- **Government Services**: Citizen registration and secure access to services

## 6. Mini Practice Task

1. **Basic Authentication Setup**:
   - Configure ASP.NET Core Identity in your application
   - Create simple login and registration forms
   - Implement basic password hashing and validation

2. **Enhanced Security Features**:
   - Add email confirmation for new registrations
   - Implement password reset functionality
   - Add account lockout after failed login attempts
   - Include "Remember Me" functionality

3. **Advanced Authentication Features**:
   - Implement external login (Google, Microsoft)
   - Add two-factor authentication (2FA)
   - Create role-based authorization system
   - Implement audit logging for authentication events
   - Add session management and timeout handling
   - Create user profile management after registration