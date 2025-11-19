using System.Data.SqlClient;
using System.Data;
using MinuteOfMeeting.Models;

namespace MinuteOfMeeting.DAL
{
    /// <summary>
    /// Department Data Access Layer
    /// Handles all database operations for Department entity
    /// </summary>
    public class DepartmentDAL
    {
        /// <summary>
        /// Get all departments
        /// </summary>
        /// <returns>DataTable with all departments</returns>
        public static DataTable SelectAll()
        {
            return DBHelper.ExecuteProcedure("PR_Department_SelectAll");
        }

        /// <summary>
        /// Get department by ID
        /// </summary>
        /// <param name="departmentID">Department ID</param>
        /// <returns>DataTable with department details</returns>
        public static DataTable SelectByPK(int departmentID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = departmentID }
            };

            return DBHelper.ExecuteProcedure("PR_Department_SelectByPK", parameters);
        }

        /// <summary>
        /// Get departments for dropdown (ID and Name only)
        /// </summary>
        /// <returns>DataTable with department ID and name</returns>
        public static DataTable SelectForDropdown()
        {
            return DBHelper.ExecuteProcedure("PR_Department_SelectForDropdown");
        }

        /// <summary>
        /// Insert new department
        /// </summary>
        /// <param name="model">Department model</param>
        /// <returns>Newly created Department ID</returns>
        public static int Insert(Department model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DepartmentName", SqlDbType.NVarChar, 100) { Value = model.DepartmentName },
                new SqlParameter("@Created", SqlDbType.DateTime) { Value = model.Created },
                new SqlParameter("@Modified", SqlDbType.DateTime) { Value = model.Modified }
            };

            var outputParameters = new Dictionary<string, object>
            {
                { "@DepartmentID", null }
            };

            DBHelper.ExecuteNonQueryWithOutput("PR_Department_Insert", outputParameters, parameters);

            return Convert.ToInt32(outputParameters["@DepartmentID"]);
        }

        /// <summary>
        /// Update existing department
        /// </summary>
        /// <param name="model">Department model with updated data</param>
        /// <returns>Number of rows affected</returns>
        public static int Update(Department model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = model.DepartmentID },
                new SqlParameter("@DepartmentName", SqlDbType.NVarChar, 100) { Value = model.DepartmentName },
                new SqlParameter("@Modified", SqlDbType.DateTime) { Value = model.Modified }
            };

            return DBHelper.ExecuteNonQuery("PR_Department_Update", parameters);
        }

        /// <summary>
        /// Delete department
        /// </summary>
        /// <param name="departmentID">Department ID to delete</param>
        /// <returns>Number of rows affected</returns>
        public static int Delete(int departmentID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = departmentID }
            };

            return DBHelper.ExecuteNonQuery("PR_Department_Delete", parameters);
        }

        /// <summary>
        /// Check if department name already exists
        /// </summary>
        /// <param name="departmentName">Department Name</param>
        /// <param name="excludeDepartmentID">ID to exclude (for update scenarios)</param>
        /// <returns>True if name exists, false otherwise</returns>
        public static bool CheckNameExists(string departmentName, int? excludeDepartmentID = null)
        {
            string query = "SELECT COUNT(*) FROM MOM_Department WHERE DepartmentName = @DepartmentName";

            if (excludeDepartmentID.HasValue)
            {
                query += " AND DepartmentID != @ExcludeDepartmentID";
            }

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentName", departmentName);

                    if (excludeDepartmentID.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@ExcludeDepartmentID", excludeDepartmentID.Value);
                    }

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Check if department is in use by staff members
        /// </summary>
        /// <param name="departmentID">Department ID</param>
        /// <returns>True if in use, false otherwise</returns>
        public static bool CheckInUseByStaff(int departmentID)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM MOM_Staff WHERE DepartmentID = @DepartmentID", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Check if department is in use by meetings
        /// </summary>
        /// <param name="departmentID">Department ID</param>
        /// <returns>True if in use, false otherwise</returns>
        public static bool CheckInUseByMeetings(int departmentID)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM MOM_Meetings WHERE DepartmentID = @DepartmentID", conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentID);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Get department count
        /// </summary>
        /// <returns>Total number of departments</returns>
        public static int GetCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM MOM_Department", conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Wrapper for controllers expecting this method name
        /// </summary>
        public static bool CheckDepartmentNameExists(string departmentName, int? excludeDepartmentID = null)
        {
            return CheckNameExists(departmentName, excludeDepartmentID);
        }

        public static int GetMeetingCountByDepartment(int departmentID)
        {
            const string query = "SELECT COUNT(*) FROM MOM_Meetings WHERE DepartmentID = @DepartmentID";
            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int GetUpcomingMeetingCountByDepartment(int departmentID)
        {
            string query = @"SELECT COUNT(*) FROM MOM_Meetings WHERE DepartmentID = @DepartmentID AND MeetingDate >= GETDATE() AND IsCancelled = 0";
            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DepartmentID", departmentID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
}