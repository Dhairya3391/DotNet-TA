using System.Data.SqlClient;
using System.Data;
using MinuteOfMeeting.Models;

namespace MinuteOfMeeting.DAL
{
    /// <summary>
    /// User Data Access Layer
    /// Handles all database operations for User authentication
    /// </summary>
    public class UserDAL
    {
        /// <summary>
        /// Get user data by username
        /// </summary>
        public static DataTable SelectByUsername(string username)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username }
            };

            return DBHelper.ExecuteProcedure("PR_User_SelectByUsername", parameters);
        }

        /// <summary>
        /// Authenticate user by validating username and password
        /// </summary>
        public static User AuthenticateUser(string username, string password)
        {
            DataTable dt = SelectByUsername(username);
            if (dt.Rows.Count == 0)
            {
                return null;
            }

            DataRow row = dt.Rows[0];
            string storedPassword = row["Password"].ToString();

            if (!string.Equals(storedPassword, password))
            {
                return null;
            }

            return MapToUser(row);
        }

        /// <summary>
        /// Get user by ID
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>User object or null if not found</returns>
        public static User SelectByPK(int userID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.Int) { Value = userID }
            };

            DataTable dt = DBHelper.ExecuteProcedure("PR_User_SelectByPK", parameters);
            return dt.Rows.Count > 0 ? MapToUser(dt.Rows[0]) : null;
        }

        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>DataTable with all users</returns>
        public static DataTable SelectAll()
        {
            return DBHelper.ExecuteProcedure("PR_User_SelectAll");
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="model">User model</param>
        /// <returns>Newly created User ID</returns>
        public static int Insert(User model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StaffID", SqlDbType.Int) { Value = (object?)model.StaffID ?? DBNull.Value },
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = model.Username },
                new SqlParameter("@Password", SqlDbType.NVarChar, 255) { Value = model.Password },
                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = model.Role },
                new SqlParameter("@Created", SqlDbType.DateTime) { Value = model.Created }
            };

            var outputParameters = new Dictionary<string, object>
            {
                { "@UserID", null }
            };

            DBHelper.ExecuteNonQueryWithOutput("PR_User_Insert", outputParameters, parameters);

            return Convert.ToInt32(outputParameters["@UserID"]);
        }

        /// <summary>
        /// Update user profile
        /// </summary>
        /// <param name="model">User model with updated data</param>
        /// <returns>Number of rows affected</returns>
        public static int Update(User model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.Int) { Value = model.UserID },
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = model.Username },
                new SqlParameter("@Role", SqlDbType.NVarChar, 20) { Value = model.Role }
            };

            return DBHelper.ExecuteNonQuery("PR_User_Update", parameters);
        }

        /// <summary>
        /// Update user password
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="oldPassword">Current password</param>
        /// <param name="newPassword">New password</param>
        /// <returns>Number of rows affected</returns>
        public static int UpdatePassword(int userID, string oldPassword, string newPassword)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.Int) { Value = userID },
                new SqlParameter("@OldPassword", SqlDbType.NVarChar, 255) { Value = oldPassword },
                new SqlParameter("@NewPassword", SqlDbType.NVarChar, 255) { Value = newPassword }
            };

            return DBHelper.ExecuteNonQuery("PR_User_UpdatePassword", parameters);
        }

        /// <summary>
        /// Update last login time
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <param name="lastLogin">Login time</param>
        /// <returns>Number of rows affected</returns>
        public static int UpdateLastLogin(int userID, DateTime lastLogin)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.Int) { Value = userID },
                new SqlParameter("@LastLogin", SqlDbType.DateTime) { Value = lastLogin }
            };

            return DBHelper.ExecuteNonQuery("PR_User_UpdateLastLogin", parameters);
        }

        /// <summary>
        /// Deactivate user
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>Number of rows affected</returns>
        public static int Deactivate(int userID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.Int) { Value = userID }
            };

            return DBHelper.ExecuteNonQuery("PR_User_Deactivate", parameters);
        }

        /// <summary>
        /// Activate user
        /// </summary>
        /// <param name="userID">User ID</param>
        /// <returns>Number of rows affected</returns>
        public static int Activate(int userID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@UserID", SqlDbType.Int) { Value = userID }
            };

            return DBHelper.ExecuteNonQuery("PR_User_Activate", parameters);
        }

        /// <summary>
        /// Check if username exists
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="excludeUserID">ID to exclude (for update scenarios)</param>
        /// <returns>True if username exists, false otherwise</returns>
        public static bool CheckUsernameExists(string username, int? excludeUserID = null)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@Username", SqlDbType.NVarChar, 50) { Value = username },
                new SqlParameter("@ExcludeUserID", SqlDbType.Int) { Value = (object?)excludeUserID ?? DBNull.Value }
            };

            DataTable dt = DBHelper.ExecuteProcedure("PR_User_CheckUsernameExists", parameters);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["UsernameExists"]) > 0;
            }

            return false;
        }

        /// <summary>
        /// Get active user count
        /// </summary>
        /// <returns>Number of active users</returns>
        public static int GetActiveUserCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM MOM_User WHERE IsActive = 1", conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Check if an email address is already associated with a user account via staff mapping
        /// </summary>
        public static bool CheckEmailExists(string emailAddress)
        {
            const string query = @"
                SELECT COUNT(*)
                FROM MOM_User u
                INNER JOIN MOM_Staff s ON u.StaffID = s.StaffID
                WHERE s.EmailAddress = @EmailAddress";

            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@EmailAddress", emailAddress);
                conn.Open();
                int count = Convert.ToInt32(cmd.ExecuteScalar());
                return count > 0;
            }
        }

        /// <summary>
        /// Get user count by role
        /// </summary>
        /// <param name="role">User role</param>
        /// <returns>Number of users with specified role</returns>
        public static int GetUserCountByRole(string role)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM MOM_User WHERE Role = @Role AND IsActive = 1", conn))
                {
                    cmd.Parameters.AddWithValue("@Role", role);

                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Get users by role
        /// </summary>
        /// <param name="role">User role</param>
        /// <returns>DataTable with users of specified role</returns>
        public static DataTable GetUsersByRole(string role)
        {
            string query = @"
                SELECT u.UserID, u.Username, u.Role, u.IsActive, u.LastLogin, u.Created,
                       s.StaffName, s.EmailAddress, d.DepartmentName
                FROM MOM_User u
                LEFT JOIN MOM_Staff s ON u.StaffID = s.StaffID
                LEFT JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
                WHERE u.Role = @Role
                ORDER BY u.Created DESC";

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Role", role);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        private static User MapToUser(DataRow row)
        {
            if (row == null)
            {
                return null;
            }

            var user = new User
            {
                UserID = row.Field<int>("UserID"),
                StaffID = row.IsNull("StaffID") ? (int?)null : row.Field<int>("StaffID"),
                Username = row["Username"].ToString(),
                Password = row.Table.Columns.Contains("Password") ? row["Password"].ToString() : string.Empty,
                Role = row["Role"].ToString(),
                IsActive = row.Table.Columns.Contains("IsActive") && !row.IsNull("IsActive") && Convert.ToBoolean(row["IsActive"]),
                LastLogin = row.IsNull("LastLogin") ? (DateTime?)null : row.Field<DateTime>("LastLogin"),
                Created = row.IsNull("Created") ? DateTime.MinValue : row.Field<DateTime>("Created"),
                StaffName = row.Table.Columns.Contains("StaffName") ? row["StaffName"].ToString() : null,
                EmailAddress = row.Table.Columns.Contains("EmailAddress") ? row["EmailAddress"].ToString() : null,
                DepartmentName = row.Table.Columns.Contains("DepartmentName") ? row["DepartmentName"].ToString() : null
            };

            return user;
        }
    }
}