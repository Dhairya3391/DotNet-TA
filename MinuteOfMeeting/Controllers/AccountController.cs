using Microsoft.AspNetCore.Mvc;
using MinuteOfMeeting.Helpers;
using MinuteOfMeeting.Models;
using MinuteOfMeeting.DAL;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MinuteOfMeeting.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account/Login
        public IActionResult Login()
        {
            // Redirect to dashboard if already logged in
            if (SessionHelper.IsUserLoggedIn(HttpContext))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User user = UserDAL.AuthenticateUser(model.Username, model.Password);

                    if (user != null)
                    {
                        UserDAL.UpdateLastLogin(user.UserID, DateTime.Now);
                        SessionHelper.SetUserSession(HttpContext, user);
                        TempData["Success"] = $"Welcome back, {user.Username}!";
                        return RedirectToAction("Index", "Dashboard");
                    }

                    ModelState.AddModelError("", "Invalid username or password");
                    TempData["Error"] = "Invalid login credentials. Please try again.";
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred during login. Please try again.");
                    TempData["Error"] = "Login failed: " + ex.Message;
                }
            }

            return View(model);
        }

        // GET: Account/Register
        public IActionResult Register()
        {
            // Redirect to dashboard if already logged in
            if (SessionHelper.IsUserLoggedIn(HttpContext))
            {
                return RedirectToAction("Index", "Dashboard");
            }

            PopulateStaffDropdown();
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(RegisterViewModel model)
        {
            try
            {
                PopulateStaffDropdown(model.StaffID);

                if (ModelState.IsValid)
                {
                    // Check if username already exists
                    if (UserDAL.CheckUsernameExists(model.Username))
                    {
                        ModelState.AddModelError("Username", "Username already exists. Please choose a different one.");
                        return View(model);
                    }

                    // Check if email already exists (if staff is selected)
                    if (model.StaffID.HasValue)
                    {
                        DataTable staffDt = StaffDAL.SelectByPK(model.StaffID.Value);
                        if (staffDt.Rows.Count > 0)
                        {
                            string email = staffDt.Rows[0]["EmailAddress"].ToString();
                            if (UserDAL.CheckEmailExists(email))
                            {
                                ModelState.AddModelError("StaffID", "A user account already exists for this staff member.");
                                return View(model);
                            }
                        }
                    }

                    // Create new user
                    User user = new User
                    {
                        Username = model.Username,
                        Password = model.Password,
                        Role = model.Role,
                        StaffID = model.StaffID,
                        IsActive = true,
                        Created = DateTime.Now
                    };

                    int userId = UserDAL.Insert(user);

                    if (userId > 0)
                    {
                        TempData["Success"] = "Registration successful! You can now login.";
                        return RedirectToAction("Login");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Registration failed. Please try again.");
                        TempData["Error"] = "Unable to create user account.";
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627) // Unique constraint violation
                {
                    ModelState.AddModelError("Username", "Username already exists.");
                }
                else
                {
                    ModelState.AddModelError("", "Database error: " + ex.Message);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred during registration. Please try again.");
                TempData["Error"] = "Registration failed: " + ex.Message;
            }

            return View(model);
        }

        // GET: Account/Logout
        public IActionResult Logout()
        {
            try
            {
                SessionHelper.ClearSession(HttpContext);
                TempData["Success"] = "You have been logged out successfully.";
            }
            catch (Exception ex)
            {
                TempData["Warning"] = "An error occurred during logout: " + ex.Message;
            }

            return RedirectToAction("Login");
        }

        // GET: Account/Profile
        [SessionAuthorize]
        public IActionResult Profile()
        {
            try
            {
                int userId = SessionHelper.GetUserID(HttpContext);
                User user = UserDAL.SelectByPK(userId);

                if (user == null)
                {
                    TempData["Error"] = "User not found.";
                    return RedirectToAction("Login");
                }

                // Get staff details if linked
                StaffProfileViewModel model = new StaffProfileViewModel();
                model.UserID = user.UserID;
                model.Username = user.Username;
                model.Role = user.Role;
                model.IsActive = user.IsActive;
                model.LastLogin = user.LastLogin;
                model.Created = user.Created;

                if (user.StaffID.HasValue)
                {
                    DataTable staffDt = StaffDAL.SelectByPK(user.StaffID.Value);
                    if (staffDt.Rows.Count > 0)
                    {
                        DataRow row = staffDt.Rows[0];
                        model.StaffID = user.StaffID.Value;
                        model.StaffName = row["StaffName"].ToString();
                        model.DepartmentName = row["DepartmentName"].ToString();
                        model.MobileNo = row["MobileNo"].ToString();
                        model.EmailAddress = row["EmailAddress"].ToString();
                    }
                }

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["Error"] = "Error loading profile: " + ex.Message;
                return RedirectToAction("Index", "Dashboard");
            }
        }

        // GET: Account/ChangePassword
        [SessionAuthorize]
        public IActionResult ChangePassword()
        {
            return View();
        }

        // POST: Account/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        [SessionAuthorize]
        public IActionResult ChangePassword(ChangePasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int userId = SessionHelper.GetUserID(HttpContext);
                    string currentUsername = SessionHelper.GetUsername(HttpContext);

                    // Verify current password
                    User currentUser = UserDAL.AuthenticateUser(currentUsername, model.CurrentPassword);
                    if (currentUser == null)
                    {
                        ModelState.AddModelError("CurrentPassword", "Current password is incorrect.");
                        return View(model);
                    }

                    int rowsAffected = UserDAL.UpdatePassword(userId, model.CurrentPassword, model.NewPassword);

                    if (rowsAffected > 0)
                    {
                        TempData["Success"] = "Password changed successfully!";
                        return RedirectToAction("Profile");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Failed to update password. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred: " + ex.Message);
                }
            }

            return View(model);
        }

        // GET: Account/AccessDenied
        public IActionResult AccessDenied()
        {
            return View();
        }

        // AJAX: Check username availability
        [HttpPost]
        public IActionResult CheckUsername(string username)
        {
            try
            {
                bool exists = UserDAL.CheckUsernameExists(username);
                return Json(new { available = !exists });
            }
            catch
            {
                return Json(new { available = false, error = true });
            }
        }

        private void PopulateStaffDropdown(int? selectedStaffId = null)
        {
            try
            {
                DataTable staffDt = StaffDAL.SelectAll();
                var staffItems = staffDt.AsEnumerable()
                    .Select(row =>
                    {
                        string department = row.Table.Columns.Contains("DepartmentName")
                            ? row["DepartmentName"].ToString()
                            : string.Empty;

                        string staffName = row["StaffName"].ToString();
                        string displayText = string.IsNullOrWhiteSpace(department)
                            ? staffName
                            : $"{staffName} ({department})";

                        return new SelectListItem
                        {
                            Value = row["StaffID"].ToString(),
                            Text = displayText,
                            Selected = selectedStaffId.HasValue && row.Field<int>("StaffID") == selectedStaffId.Value
                        };
                    })
                    .ToList();

                ViewBag.Departments = staffItems;
            }
            catch
            {
                ViewBag.Departments = new List<SelectListItem>();
            }
        }
    }

    // View Models for Account
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Username is required")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Username must be between 3 and 50 characters")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessage = "Password and confirmation password do not match")]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Role is required")]
        [Display(Name = "User Role")]
        public string Role { get; set; }

        [Display(Name = "Link to Staff Member")]
        public int? StaffID { get; set; }
    }

    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Current Password")]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = "New password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters long")]
        [DataType(DataType.Password)]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "Confirm new password is required")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm New Password")]
        [Compare("NewPassword", ErrorMessage = "New password and confirmation password do not match")]
        public string ConfirmNewPassword { get; set; }
    }

    public class StaffProfileViewModel
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLogin { get; set; }
        public DateTime Created { get; set; }

        // Staff details (if linked)
        public int? StaffID { get; set; }
        public string StaffName { get; set; }
        public string DepartmentName { get; set; }
        public string MobileNo { get; set; }
        public string EmailAddress { get; set; }
    }
}