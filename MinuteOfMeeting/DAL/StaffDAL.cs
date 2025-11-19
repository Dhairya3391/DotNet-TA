using System.Data.SqlClient;
using System.Data;
using MinuteOfMeeting.Models;

namespace MinuteOfMeeting.DAL
{
    /// <summary>
    /// Staff Data Access Layer
    /// Handles all database operations for Staff entity
    /// </summary>
    public class StaffDAL
    {
        /// <summary>
        /// Get all staff with department names
        /// </summary>
        /// <returns>DataTable with all staff members</returns>
        public static DataTable SelectAll()
        {
            return DBHelper.ExecuteProcedure("PR_Staff_SelectAll");
        }

        /// <summary>
        /// Get staff member by ID
        /// </summary>
        /// <param name="staffID">Staff ID</param>
        /// <returns>DataTable with staff details</returns>
        public static DataTable SelectByPK(int staffID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StaffID", SqlDbType.Int) { Value = staffID }
            };

            return DBHelper.ExecuteProcedure("PR_Staff_SelectByPK", parameters);
        }

        /// <summary>
        /// Get staff members by department
        /// </summary>
        /// <param name="departmentID">Department ID</param>
        /// <returns>DataTable with staff members in specific department</returns>
        public static DataTable SelectByDepartment(int departmentID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = departmentID }
            };

            return DBHelper.ExecuteProcedure("PR_Staff_SelectByDepartment", parameters);
        }

        /// <summary>
        /// Get staff members for dropdown (ID and Name only)
        /// </summary>
        /// <param name="departmentID">Optional department filter</param>
        /// <returns>DataTable with staff ID and display name</returns>
        public static DataTable SelectForDropdown(int? departmentID = null)
        {
            SqlParameter parameter = departmentID.HasValue
                ? new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = departmentID.Value }
                : new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = DBNull.Value };

            return DBHelper.ExecuteProcedure("PR_Staff_SelectForDropdown", parameter);
        }

        /// <summary>
        /// Insert new staff member
        /// </summary>
        /// <param name="model">Staff model</param>
        /// <returns>Newly created Staff ID</returns>
        public static int Insert(Staff model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = model.DepartmentID },
                new SqlParameter("@StaffName", SqlDbType.NVarChar, 50) { Value = model.StaffName },
                new SqlParameter("@MobileNo", SqlDbType.NVarChar, 20) { Value = model.MobileNo },
                new SqlParameter("@EmailAddress", SqlDbType.NVarChar, 50) { Value = model.EmailAddress },
                new SqlParameter("@Remarks", SqlDbType.NVarChar, 250) { Value = (object?)model.Remarks ?? DBNull.Value },
                new SqlParameter("@Created", SqlDbType.DateTime) { Value = model.Created },
                new SqlParameter("@Modified", SqlDbType.DateTime) { Value = model.Modified }
            };

            var outputParameters = new Dictionary<string, object>
            {
                { "@StaffID", null }
            };

            DBHelper.ExecuteNonQueryWithOutput("PR_Staff_Insert", outputParameters, parameters);

            return Convert.ToInt32(outputParameters["@StaffID"]);
        }

        /// <summary>
        /// Update existing staff member
        /// </summary>
        /// <param name="model">Staff model with updated data</param>
        /// <returns>Number of rows affected</returns>
        public static int Update(Staff model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StaffID", SqlDbType.Int) { Value = model.StaffID },
                new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = model.DepartmentID },
                new SqlParameter("@StaffName", SqlDbType.NVarChar, 50) { Value = model.StaffName },
                new SqlParameter("@MobileNo", SqlDbType.NVarChar, 20) { Value = model.MobileNo },
                new SqlParameter("@EmailAddress", SqlDbType.NVarChar, 50) { Value = model.EmailAddress },
                new SqlParameter("@Remarks", SqlDbType.NVarChar, 250) { Value = (object?)model.Remarks ?? DBNull.Value },
                new SqlParameter("@Modified", SqlDbType.DateTime) { Value = model.Modified }
            };

            return DBHelper.ExecuteNonQuery("PR_Staff_Update", parameters);
        }

        /// <summary>
        /// Delete staff member
        /// </summary>
        /// <param name="staffID">Staff ID to delete</param>
        /// <returns>Number of rows affected</returns>
        public static int Delete(int staffID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StaffID", SqlDbType.Int) { Value = staffID }
            };

            return DBHelper.ExecuteNonQuery("PR_Staff_Delete", parameters);
        }

        /// <summary>
        /// Check if email address already exists
        /// </summary>
        /// <param name="emailAddress">Email address</param>
        /// <param name="excludeStaffID">ID to exclude (for update scenarios)</param>
        /// <returns>True if email exists, false otherwise</returns>
        public static bool CheckEmailExists(string emailAddress, int? excludeStaffID = null)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@EmailAddress", SqlDbType.NVarChar, 50) { Value = emailAddress },
                new SqlParameter("@ExcludeStaffID", SqlDbType.Int) { Value = (object?)excludeStaffID ?? DBNull.Value }
            };

            DataTable dt = DBHelper.ExecuteProcedure("PR_Staff_CheckEmailExists", parameters);

            if (dt.Rows.Count > 0)
            {
                return Convert.ToInt32(dt.Rows[0]["EmailExists"]) > 0;
            }

            return false;
        }

        /// <summary>
        /// Check if staff member is in use by meeting members
        /// </summary>
        /// <param name="staffID">Staff ID</param>
        /// <returns>True if in use, false otherwise</returns>
        public static bool CheckInUseByMeetingMembers(int staffID)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM MOM_MeetingMember WHERE StaffID = @StaffID", conn))
                {
                    cmd.Parameters.AddWithValue("@StaffID", staffID);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Check if staff member has a user account
        /// </summary>
        /// <param name="staffID">Staff ID</param>
        /// <returns>True if has user account, false otherwise</returns>
        public static bool CheckHasUserAccount(int staffID)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM MOM_User WHERE StaffID = @StaffID", conn))
                {
                    cmd.Parameters.AddWithValue("@StaffID", staffID);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Get staff count
        /// </summary>
        /// <returns>Total number of staff members</returns>
        public static int GetCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM MOM_Staff", conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Get staff count by department
        /// </summary>
        /// <param name="departmentID">Department ID</param>
        /// <returns>Number of staff members in department</returns>
        public static int GetCountByDepartment(int departmentID)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM MOM_Staff WHERE DepartmentID = @DepartmentID", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Search staff by name or email
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>DataTable with matching staff members</returns>
        public static DataTable Search(string searchTerm)
        {
            string query = @"
                SELECT s.StaffID, s.StaffName, s.EmailAddress, s.MobileNo, d.DepartmentName
                FROM MOM_Staff s
                INNER JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
                WHERE s.StaffName LIKE @SearchTerm
                   OR s.EmailAddress LIKE @SearchTerm
                ORDER BY s.StaffName";

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@SearchTerm", $"%{searchTerm}%");

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Get meetings associated with a staff member
        /// </summary>
        public static DataTable GetStaffMeetings(int staffID)
        {
            string query = @"
                SELECT m.MeetingID, m.MeetingDate, m.MeetingDescription,
                       mt.MeetingTypeName, mv.MeetingVenueName, d.DepartmentName,
                       mm.IsPresent, mm.Remarks
                FROM MOM_MeetingMember mm
                INNER JOIN MOM_Meetings m ON mm.MeetingID = m.MeetingID
                INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
                INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
                INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
                WHERE mm.StaffID = @StaffID
                ORDER BY m.MeetingDate DESC";

            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StaffID", staffID);
                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static int GetStaffMeetingCount(int staffID)
        {
            const string query = "SELECT COUNT(DISTINCT MeetingID) FROM MOM_MeetingMember WHERE StaffID = @StaffID";
            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StaffID", staffID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int GetStaffAttendedMeetingCount(int staffID)
        {
            const string query = "SELECT COUNT(*) FROM MOM_MeetingMember WHERE StaffID = @StaffID AND IsPresent = 1";
            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StaffID", staffID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int GetStaffUpcomingMeetingCount(int staffID)
        {
            string query = @"
                SELECT COUNT(*)
                FROM MOM_MeetingMember mm
                INNER JOIN MOM_Meetings m ON mm.MeetingID = m.MeetingID
                WHERE mm.StaffID = @StaffID AND m.MeetingDate >= GETDATE() AND m.IsCancelled = 0";

            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StaffID", staffID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static double GetStaffAttendanceRate(int staffID)
        {
            const string query = @"
                SELECT CASE WHEN COUNT(*) = 0 THEN 0
                            ELSE (CAST(SUM(CASE WHEN IsPresent = 1 THEN 1 ELSE 0 END) AS FLOAT) / COUNT(*)) * 100 END
                FROM MOM_MeetingMember WHERE StaffID = @StaffID";

            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@StaffID", staffID);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result == DBNull.Value ? 0 : Convert.ToDouble(result);
            }
        }

        public static int GetStaffCountByDepartment(int departmentID)
        {
            return GetCountByDepartment(departmentID);
        }
    }
}