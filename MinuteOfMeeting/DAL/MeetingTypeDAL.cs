using System.Data.SqlClient;
using System.Data;
using MinuteOfMeeting.Models;

namespace MinuteOfMeeting.DAL
{
    /// <summary>
    /// Meeting Type Data Access Layer
    /// Handles all database operations for MeetingType entity
    /// </summary>
    public class MeetingTypeDAL
    {
        /// <summary>
        /// Get all meeting types
        /// </summary>
        /// <returns>DataTable with all meeting types</returns>
        public static DataTable SelectAll()
        {
            return DBHelper.ExecuteProcedure("PR_MeetingType_SelectAll");
        }

        /// <summary>
        /// Get meeting type by ID
        /// </summary>
        /// <param name="meetingTypeID">Meeting Type ID</param>
        /// <returns>DataTable with meeting type details</returns>
        public static DataTable SelectByPK(int meetingTypeID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingTypeID", SqlDbType.Int) { Value = meetingTypeID }
            };

            return DBHelper.ExecuteProcedure("PR_MeetingType_SelectByPK", parameters);
        }

        /// <summary>
        /// Get meeting types for dropdown (ID and Name only)
        /// </summary>
        /// <returns>DataTable with meeting type ID and name</returns>
        public static DataTable SelectForDropdown()
        {
            return DBHelper.ExecuteProcedure("PR_MeetingType_SelectForDropdown");
        }

        /// <summary>
        /// Insert new meeting type
        /// </summary>
        /// <param name="model">MeetingType model</param>
        /// <returns>Newly created Meeting Type ID</returns>
        public static int Insert(MeetingType model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingTypeName", SqlDbType.NVarChar, 100) { Value = model.MeetingTypeName },
                new SqlParameter("@Remarks", SqlDbType.NVarChar, 100) { Value = model.Remarks },
                new SqlParameter("@Created", SqlDbType.DateTime) { Value = model.Created },
                new SqlParameter("@Modified", SqlDbType.DateTime) { Value = model.Modified }
            };

            // Output parameter for the new ID
            var outputParameters = new Dictionary<string, object>
            {
                { "@MeetingTypeID", null }
            };

            DBHelper.ExecuteNonQueryWithOutput("PR_MeetingType_Insert", outputParameters, parameters);

            return Convert.ToInt32(outputParameters["@MeetingTypeID"]);
        }

        /// <summary>
        /// Update existing meeting type
        /// </summary>
        /// <param name="model">MeetingType model with updated data</param>
        /// <returns>Number of rows affected</returns>
        public static int Update(MeetingType model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingTypeID", SqlDbType.Int) { Value = model.MeetingTypeID },
                new SqlParameter("@MeetingTypeName", SqlDbType.NVarChar, 100) { Value = model.MeetingTypeName },
                new SqlParameter("@Remarks", SqlDbType.NVarChar, 100) { Value = model.Remarks },
                new SqlParameter("@Modified", SqlDbType.DateTime) { Value = model.Modified }
            };

            return DBHelper.ExecuteNonQuery("PR_MeetingType_Update", parameters);
        }

        /// <summary>
        /// Delete meeting type
        /// </summary>
        /// <param name="meetingTypeID">Meeting Type ID to delete</param>
        /// <returns>Number of rows affected</returns>
        public static int Delete(int meetingTypeID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingTypeID", SqlDbType.Int) { Value = meetingTypeID }
            };

            return DBHelper.ExecuteNonQuery("PR_MeetingType_Delete", parameters);
        }

        /// <summary>
        /// Check if meeting type name already exists
        /// </summary>
        /// <param name="meetingTypeName">Meeting Type Name</param>
        /// <param name="excludeMeetingTypeID">ID to exclude (for update scenarios)</param>
        /// <returns>True if name exists, false otherwise</returns>
        public static bool CheckMeetingTypeNameExists(string meetingTypeName, int? excludeMeetingTypeID = null)
        {
            return CheckNameExistsInternal(meetingTypeName, excludeMeetingTypeID);
        }

        // Backward compatible helper for any legacy callers
        public static bool CheckNameExists(string meetingTypeName, int? excludeMeetingTypeID = null)
        {
            return CheckNameExistsInternal(meetingTypeName, excludeMeetingTypeID);
        }

        private static bool CheckNameExistsInternal(string meetingTypeName, int? excludeMeetingTypeID)
        {
            string query = "SELECT COUNT(*) FROM MOM_MeetingType WHERE MeetingTypeName = @MeetingTypeName";

            if (excludeMeetingTypeID.HasValue)
            {
                query += " AND MeetingTypeID != @ExcludeMeetingTypeID";
            }

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MeetingTypeName", meetingTypeName);

                    if (excludeMeetingTypeID.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@ExcludeMeetingTypeID", excludeMeetingTypeID.Value);
                    }

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Check if meeting type is in use by any meetings
        /// </summary>
        /// <param name="meetingTypeID">Meeting Type ID</param>
        /// <returns>True if in use, false otherwise</returns>
        public static bool CheckInUse(int meetingTypeID)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM MOM_Meetings WHERE MeetingTypeID = @MeetingTypeID", conn))
                {
                    cmd.Parameters.AddWithValue("@MeetingTypeID", meetingTypeID);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Get meeting type count
        /// </summary>
        /// <returns>Total number of meeting types</returns>
        public static int GetCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM MOM_MeetingType", conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }
    }
}