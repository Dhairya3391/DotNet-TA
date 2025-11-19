using Microsoft.AspNetCore.Http;
using MinuteOfMeeting.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MinuteOfMeeting.Helpers
{
    /// <summary>
    /// Session Helper Class
    /// Manages user session data throughout the application
    /// </summary>
    public static class SessionHelper
    {
        // Session Keys
        private const string USER_ID_KEY = "UserID";
        private const string USERNAME_KEY = "Username";
        private const string ROLE_KEY = "Role";
        private const string STAFF_ID_KEY = "StaffID";
        private const string STAFF_NAME_KEY = "StaffName";
        private const string LOGIN_TIME_KEY = "LoginTime";

        /// <summary>
        /// Set user session after successful login
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="user">User object</param>
        public static void SetUserSession(HttpContext context, User user)
        {
            context.Session.SetInt32(USER_ID_KEY, user.UserID);
            context.Session.SetString(USERNAME_KEY, user.Username);
            context.Session.SetString(ROLE_KEY, user.Role);

            if (user.StaffID.HasValue)
            {
                context.Session.SetInt32(STAFF_ID_KEY, user.StaffID.Value);
            }

            if (!string.IsNullOrEmpty(user.StaffName))
            {
                context.Session.SetString(STAFF_NAME_KEY, user.StaffName);
            }

            context.Session.SetString(LOGIN_TIME_KEY, DateTime.Now.ToString());
        }

        /// <summary>
        /// Check if user is logged in
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>True if logged in, false otherwise</returns>
        public static bool IsUserLoggedIn(HttpContext context)
        {
            return context.Session.GetInt32(USER_ID_KEY).HasValue;
        }

        /// <summary>
        /// Get current user ID from session
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>User ID or 0 if not logged in</returns>
        public static int GetUserID(HttpContext context)
        {
            return context.Session.GetInt32(USER_ID_KEY) ?? 0;
        }

        /// <summary>
        /// Get current username from session
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>Username or empty string if not logged in</returns>
        public static string GetUsername(HttpContext context)
        {
            return context.Session.GetString(USERNAME_KEY) ?? string.Empty;
        }

        /// <summary>
        /// Get current user role from session
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>User role or empty string if not logged in</returns>
        public static string GetUserRole(HttpContext context)
        {
            return context.Session.GetString(ROLE_KEY) ?? string.Empty;
        }

        /// <summary>
        /// Get current staff ID from session
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>Staff ID or null if not available</returns>
        public static int? GetStaffID(HttpContext context)
        {
            return context.Session.GetInt32(STAFF_ID_KEY);
        }

        /// <summary>
        /// Get current staff name from session
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>Staff name or empty string if not available</returns>
        public static string GetStaffName(HttpContext context)
        {
            return context.Session.GetString(STAFF_NAME_KEY) ?? string.Empty;
        }

        /// <summary>
        /// Get login time from session
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>Login time or null if not available</returns>
        public static DateTime? GetLoginTime(HttpContext context)
        {
            string loginTimeString = context.Session.GetString(LOGIN_TIME_KEY);
            if (DateTime.TryParse(loginTimeString, out DateTime loginTime))
            {
                return loginTime;
            }
            return null;
        }

        /// <summary>
        /// Check if user has specific role
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="requiredRole">Required role</param>
        /// <returns>True if user has required role, false otherwise</returns>
        public static bool HasRole(HttpContext context, string requiredRole)
        {
            string userRole = GetUserRole(context);
            return string.Equals(userRole, requiredRole, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Check if user has admin role
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>True if admin, false otherwise</returns>
        public static bool IsAdmin(HttpContext context)
        {
            return HasRole(context, "Admin");
        }

        /// <summary>
        /// Check if user has organizer role
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>True if organizer, false otherwise</returns>
        public static bool IsOrganizer(HttpContext context)
        {
            return HasRole(context, "Organizer");
        }

        /// <summary>
        /// Check if user has staff role
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>True if staff, false otherwise</returns>
        public static bool IsStaff(HttpContext context)
        {
            return HasRole(context, "Staff");
        }

        /// <summary>
        /// Check if user has admin or organizer role (can create/edit meetings)
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>True if can manage meetings, false otherwise</returns>
        public static bool CanManageMeetings(HttpContext context)
        {
            return IsAdmin(context) || IsOrganizer(context);
        }

        /// <summary>
        /// Check if session has expired (30 minutes timeout)
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>True if expired, false otherwise</returns>
        public static bool IsSessionExpired(HttpContext context)
        {
            DateTime? loginTime = GetLoginTime(context);
            if (!loginTime.HasValue)
            {
                return true;
            }

            TimeSpan sessionDuration = DateTime.Now - loginTime.Value;
            return sessionDuration.TotalMinutes > 30; // 30 minutes timeout
        }

        /// <summary>
        /// Set temporary data in session
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="key">Session key</param>
        /// <param name="value">Value to store</param>
        public static void SetTempData(HttpContext context, string key, string value)
        {
            context.Session.SetString($"Temp_{key}", value);
        }

        /// <summary>
        /// Get temporary data from session
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="key">Session key</param>
        /// <returns>Stored value or empty string if not found</returns>
        public static string GetTempData(HttpContext context, string key)
        {
            return context.Session.GetString($"Temp_{key}") ?? string.Empty;
        }

        /// <summary>
        /// Remove temporary data from session
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <param name="key">Session key</param>
        public static void RemoveTempData(HttpContext context, string key)
        {
            context.Session.Remove($"Temp_{key}");
        }

        /// <summary>
        /// Clear all session data (logout)
        /// </summary>
        /// <param name="context">HttpContext</param>
        public static void ClearSession(HttpContext context)
        {
            context.Session.Clear();
        }

        /// <summary>
        /// Refresh session timeout (keep alive)
        /// </summary>
        /// <param name="context">HttpContext</param>
        public static void RefreshSession(HttpContext context)
        {
            context.Session.SetString(LOGIN_TIME_KEY, DateTime.Now.ToString());
        }

        /// <summary>
        /// Get session information for display
        /// </summary>
        /// <param name="context">HttpContext</param>
        /// <returns>Session info object</returns>
        public static object GetSessionInfo(HttpContext context)
        {
            return new
            {
                UserID = GetUserID(context),
                Username = GetUsername(context),
                Role = GetUserRole(context),
                StaffName = GetStaffName(context),
                LoginTime = GetLoginTime(context),
                IsLoggedIn = IsUserLoggedIn(context),
                CanManageMeetings = CanManageMeetings(context),
                IsExpired = IsSessionExpired(context)
            };
        }
    }

    /// <summary>
    /// Custom Authorization Attribute for Session-based Authentication (Alias)
    /// </summary>
    public class SessionAuthorizeAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public SessionAuthorizeAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            // Check if user is logged in
            if (!SessionHelper.IsUserLoggedIn(httpContext))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Check if session has expired
            if (SessionHelper.IsSessionExpired(httpContext))
            {
                SessionHelper.ClearSession(httpContext);
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Check role-based access if roles are specified
            if (_allowedRoles != null && _allowedRoles.Length > 0)
            {
                string userRole = SessionHelper.GetUserRole(httpContext);
                if (!Array.Exists(_allowedRoles, role => role.Equals(userRole, StringComparison.OrdinalIgnoreCase)))
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                    return;
                }
            }

            // Refresh session timeout
            SessionHelper.RefreshSession(httpContext);

            base.OnActionExecuting(context);
        }
    }

    /// <summary>
    /// Custom Authorization Attribute for Session-based Authentication
    /// </summary>
    public class AuthorizeSessionAttribute : ActionFilterAttribute
    {
        private readonly string[] _allowedRoles;

        public AuthorizeSessionAttribute(params string[] allowedRoles)
        {
            _allowedRoles = allowedRoles;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var httpContext = context.HttpContext;

            // Check if user is logged in
            if (!SessionHelper.IsUserLoggedIn(httpContext))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Check if session has expired
            if (SessionHelper.IsSessionExpired(httpContext))
            {
                SessionHelper.ClearSession(httpContext);
                context.Result = new RedirectToActionResult("Login", "Account", null);
                return;
            }

            // Check role-based access if roles are specified
            if (_allowedRoles != null && _allowedRoles.Length > 0)
            {
                string userRole = SessionHelper.GetUserRole(httpContext);
                if (!Array.Exists(_allowedRoles, role => role.Equals(userRole, StringComparison.OrdinalIgnoreCase)))
                {
                    context.Result = new RedirectToActionResult("AccessDenied", "Account", null);
                    return;
                }
            }

            // Refresh session timeout
            SessionHelper.RefreshSession(httpContext);

            base.OnActionExecuting(context);
        }
    }
}