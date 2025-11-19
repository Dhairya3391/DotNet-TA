using System.Data;
using System.Data.SqlClient;

namespace MinuteOfMeeting.DAL
{
    /// <summary>
    /// Dashboard Data Access Layer
    /// Provides statistics and analytics for the dashboard
    /// </summary>
    public class DashboardDAL
    {
        // Basic Statistics
        public static int GetTotalMeetings()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetTotalMeetings", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@TotalMeetings", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetUpcomingMeetingsCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetUpcomingMeetingsCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetCompletedMeetingsCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetCompletedMeetingsCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetCancelledMeetingsCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetCancelledMeetingsCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        // Meeting Lists
        public static DataTable GetRecentMeetings(int count)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetRecentMeetings", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Count", count);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetUpcomingMeetings(int count)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetUpcomingMeetings", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Count", count);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetTodayMeetings()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetTodayMeetings", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Chart Data
        public static DataTable GetMeetingsByType()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetMeetingsByType", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetMeetingsByDepartment()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetMeetingsByDepartment", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetDepartmentParticipation()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetDepartmentParticipation", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetMonthlyMeetingTrend()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetMonthlyMeetingTrend", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Department Statistics
        public static DataTable GetMostActiveDepartments(int count)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetMostActiveDepartments", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Count", count);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetStaffParticipation(int count)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetStaffParticipation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Count", count);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Count Methods
        public static int GetTodayMeetingsCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetTodayMeetingsCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetThisWeekMeetingsCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetThisWeekMeetingsCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetThisMonthMeetingsCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetThisMonthMeetingsCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        // Master Data Counts
        public static int GetTotalStaffCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetTotalStaffCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetTotalDepartmentsCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetTotalDepartmentsCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetTotalVenuesCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetTotalVenuesCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetTotalTypesCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetTotalTypesCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        // Staff-specific methods
        public static int GetStaffTotalMeetings(int staffId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetStaffTotalMeetings", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StaffID", staffId);

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetStaffUpcomingMeetings(int staffId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetStaffUpcomingMeetings", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StaffID", staffId);

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetStaffAttendedMeetings(int staffId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetStaffAttendedMeetings", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StaffID", staffId);

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static double GetStaffAttendanceRate(int staffId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetStaffAttendanceRate", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StaffID", staffId);

                SqlParameter outputParam = new SqlParameter("@AttendanceRate", SqlDbType.Decimal);
                outputParam.Direction = ParameterDirection.Output;
                outputParam.Precision = 5;
                outputParam.Scale = 2;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0.0 : Convert.ToDouble(outputParam.Value);
            }
        }

        public static DataTable GetStaffRecentMeetings(int staffId, int count)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetStaffRecentMeetings", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StaffID", staffId);
                cmd.Parameters.AddWithValue("@Count", count);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetStaffUpcomingMeetingsList(int staffId, int count)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetStaffUpcomingMeetingsList", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StaffID", staffId);
                cmd.Parameters.AddWithValue("@Count", count);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Additional methods for calendar and activities
        public static DataTable GetCalendarData(int year, int month)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetCalendarData", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Year", year);
                cmd.Parameters.AddWithValue("@Month", month);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetRecentActivities(int count)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetRecentActivities", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Count", count);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // Admin-specific methods
        public static int GetTotalUsersCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetTotalUsersCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        public static int GetActiveUsersCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetActiveUsersCount", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter outputParam = new SqlParameter("@Count", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.IsDBNull(outputParam.Value) ? 0 : Convert.ToInt32(outputParam.Value);
            }
        }

        // Utility methods
        public static DataTable GetNotifications(int userId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetNotifications", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@UserID", userId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static bool MarkNotificationRead(int notificationId, int userId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_MarkNotificationRead", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@NotificationID", notificationId);
                cmd.Parameters.AddWithValue("@UserID", userId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        public static string GetSystemUptime()
        {
            // Placeholder: in real scenarios fetch from monitoring table
            return "24 hrs";
        }

        public static DateTime? GetLastBackupTime()
        {
            // Placeholder: could query a maintenance log table
            return DateTime.Now.AddHours(-5);
        }

        public static string GetDatabaseSize()
        {
            return "Unknown";
        }

        public static DataTable GetRecentUserActivities(int count)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetRecentUserActivities", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Count", count);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public static DataTable GetStorageUsage()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_Dashboard_GetStorageUsage", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}