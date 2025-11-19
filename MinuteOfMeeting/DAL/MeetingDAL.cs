using System.Data.SqlClient;
using System.Data;
using MinuteOfMeeting.Models;

namespace MinuteOfMeeting.DAL
{
    /// <summary>
    /// Meeting Data Access Layer
    /// Handles all database operations for Meeting entity
    /// </summary>
    public class MeetingDAL
    {
        /// <summary>
        /// Get all meetings with related data
        /// </summary>
        /// <returns>DataTable with all meetings</returns>
        public static DataTable SelectAll()
        {
            return DBHelper.ExecuteProcedure("PR_Meeting_SelectAll");
        }

        /// <summary>
        /// Get meeting by ID
        /// </summary>
        /// <param name="meetingID">Meeting ID</param>
        /// <returns>DataTable with meeting details</returns>
        public static DataTable SelectByPK(int meetingID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingID", SqlDbType.Int) { Value = meetingID }
            };

            return DBHelper.ExecuteProcedure("PR_Meeting_SelectByPK", parameters);
        }

        /// <summary>
        /// Get meetings with filters
        /// </summary>
        /// <param name="startDate">Start date filter</param>
        /// <param name="endDate">End date filter</param>
        /// <param name="meetingTypeID">Meeting type filter</param>
        /// <param name="meetingVenueID">Venue filter</param>
        /// <param name="departmentID">Department filter</param>
        /// <param name="searchKeyword">Search keyword</param>
        /// <param name="status">Status filter</param>
        /// <returns>DataTable with filtered meetings</returns>
        public static DataTable SelectWithFilters(
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? meetingTypeID = null,
            int? meetingVenueID = null,
            int? departmentID = null,
            string searchKeyword = null,
            string status = null)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = (object?)startDate ?? DBNull.Value },
                new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = (object?)endDate ?? DBNull.Value },
                new SqlParameter("@MeetingTypeID", SqlDbType.Int) { Value = (object?)meetingTypeID ?? DBNull.Value },
                new SqlParameter("@MeetingVenueID", SqlDbType.Int) { Value = (object?)meetingVenueID ?? DBNull.Value },
                new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = (object?)departmentID ?? DBNull.Value },
                new SqlParameter("@SearchKeyword", SqlDbType.NVarChar, 250) { Value = (object?)searchKeyword ?? DBNull.Value },
                new SqlParameter("@Status", SqlDbType.NVarChar, 20) { Value = (object?)status ?? DBNull.Value }
            };

            return DBHelper.ExecuteProcedure("PR_Meeting_SelectWithFilters", parameters);
        }

        /// <summary>
        /// Get upcoming meetings
        /// </summary>
        /// <param name="topCount">Number of meetings to return</param>
        /// <returns>DataTable with upcoming meetings</returns>
        public static DataTable SelectUpcoming(int topCount = 10)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TopCount", SqlDbType.Int) { Value = topCount }
            };

            return DBHelper.ExecuteProcedure("PR_Meeting_SelectUpcoming", parameters);
        }

        /// <summary>
        /// Get completed meetings
        /// </summary>
        /// <param name="topCount">Number of meetings to return</param>
        /// <returns>DataTable with completed meetings</returns>
        public static DataTable SelectCompleted(int topCount = 10)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@TopCount", SqlDbType.Int) { Value = topCount }
            };

            return DBHelper.ExecuteProcedure("PR_Meeting_SelectCompleted", parameters);
        }

        /// <summary>
        /// Get cancelled meetings
        /// </summary>
        /// <returns>DataTable with cancelled meetings</returns>
        public static DataTable SelectCancelled()
        {
            return DBHelper.ExecuteProcedure("PR_Meeting_SelectCancelled");
        }

        /// <summary>
        /// Insert new meeting
        /// </summary>
        /// <param name="model">Meeting model</param>
        /// <returns>Newly created Meeting ID</returns>
        public static int Insert(Meeting model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingDate", SqlDbType.DateTime) { Value = model.MeetingDate },
                new SqlParameter("@MeetingVenueID", SqlDbType.Int) { Value = model.MeetingVenueID },
                new SqlParameter("@MeetingTypeID", SqlDbType.Int) { Value = model.MeetingTypeID },
                new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = model.DepartmentID },
                new SqlParameter("@MeetingDescription", SqlDbType.NVarChar, 250) { Value = (object?)model.MeetingDescription ?? DBNull.Value },
                new SqlParameter("@DocumentPath", SqlDbType.NVarChar, 250) { Value = (object?)model.DocumentPath ?? DBNull.Value },
                new SqlParameter("@Created", SqlDbType.DateTime) { Value = model.Created },
                new SqlParameter("@Modified", SqlDbType.DateTime) { Value = model.Modified }
            };

            var outputParameters = new Dictionary<string, object>
            {
                { "@MeetingID", null }
            };

            DBHelper.ExecuteNonQueryWithOutput("PR_Meeting_Insert", outputParameters, parameters);

            return Convert.ToInt32(outputParameters["@MeetingID"]);
        }

        /// <summary>
        /// Update existing meeting
        /// </summary>
        /// <param name="model">Meeting model with updated data</param>
        /// <returns>Number of rows affected</returns>
        public static int Update(Meeting model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingID", SqlDbType.Int) { Value = model.MeetingID },
                new SqlParameter("@MeetingDate", SqlDbType.DateTime) { Value = model.MeetingDate },
                new SqlParameter("@MeetingVenueID", SqlDbType.Int) { Value = model.MeetingVenueID },
                new SqlParameter("@MeetingTypeID", SqlDbType.Int) { Value = model.MeetingTypeID },
                new SqlParameter("@DepartmentID", SqlDbType.Int) { Value = model.DepartmentID },
                new SqlParameter("@MeetingDescription", SqlDbType.NVarChar, 250) { Value = (object?)model.MeetingDescription ?? DBNull.Value },
                new SqlParameter("@DocumentPath", SqlDbType.NVarChar, 250) { Value = (object?)model.DocumentPath ?? DBNull.Value },
                new SqlParameter("@Modified", SqlDbType.DateTime) { Value = model.Modified }
            };

            return DBHelper.ExecuteNonQuery("PR_Meeting_Update", parameters);
        }

        /// <summary>
        /// Cancel meeting
        /// </summary>
        /// <param name="meetingID">Meeting ID</param>
        /// <param name="cancellationReason">Cancellation reason</param>
        /// <param name="cancellationDateTime">Cancellation date and time</param>
        /// <returns>Number of rows affected</returns>
        public static int Cancel(int meetingID, string cancellationReason, DateTime? cancellationDateTime = null)
        {
            DateTime cancelTime = cancellationDateTime ?? DateTime.Now;
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingID", SqlDbType.Int) { Value = meetingID },
                new SqlParameter("@CancellationReason", SqlDbType.NVarChar, 250) { Value = cancellationReason },
                new SqlParameter("@CancellationDateTime", SqlDbType.DateTime) { Value = cancelTime }
            };

            return DBHelper.ExecuteNonQuery("PR_Meeting_Cancel", parameters);
        }

        /// <summary>
        /// Delete meeting
        /// </summary>
        /// <param name="meetingID">Meeting ID to delete</param>
        /// <returns>Document path for cleanup</returns>
        public static int Delete(int meetingID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingID", SqlDbType.Int) { Value = meetingID }
            };

            return DBHelper.ExecuteNonQuery("PR_Meeting_Delete", parameters);
        }

        /// <summary>
        /// Check for meeting conflicts
        /// </summary>
        /// <param name="meetingVenueID">Venue ID</param>
        /// <param name="meetingDate">Meeting date and time</param>
        /// <param name="excludeMeetingID">Meeting ID to exclude (for updates)</param>
        /// <returns>Tuple indicating if conflict exists and conflict details</returns>
        public static (bool HasConflict, int ConflictMeetingID, string ConflictDescription) CheckConflict(
            int meetingVenueID, DateTime meetingDate, int? excludeMeetingID = null)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingVenueID", SqlDbType.Int) { Value = meetingVenueID },
                new SqlParameter("@MeetingDate", SqlDbType.DateTime) { Value = meetingDate },
                new SqlParameter("@ExcludeMeetingID", SqlDbType.Int) { Value = (object?)excludeMeetingID ?? DBNull.Value }
            };

            var outputParameters = new Dictionary<string, object>
            {
                { "@HasConflict", null },
                { "@ConflictMeetingID", null },
                { "@ConflictMeetingDescription", null }
            };

            DBHelper.ExecuteNonQueryWithOutput("PR_Meeting_CheckConflict", outputParameters, parameters);

            bool hasConflict = Convert.ToBoolean(outputParameters["@HasConflict"]);
            int conflictMeetingID = outputParameters["@ConflictMeetingID"] != null ? Convert.ToInt32(outputParameters["@ConflictMeetingID"]) : 0;
            string conflictDescription = outputParameters["@ConflictMeetingDescription"]?.ToString() ?? string.Empty;

            return (hasConflict, conflictMeetingID, conflictDescription);
        }

        /// <summary>
        /// Get meeting count
        /// </summary>
        /// <returns>Total number of meetings</returns>
        public static int GetCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM MOM_Meetings", conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Get meeting count by status
        /// </summary>
        /// <param name="status">Meeting status</param>
        /// <returns>Number of meetings with specified status</returns>
        public static int GetCountByStatus(string status)
        {
            string query = status.ToLower() switch
            {
                "upcoming" => "SELECT COUNT(*) FROM MOM_Meetings WHERE MeetingDate >= GETDATE() AND IsCancelled = 0",
                "completed" => "SELECT COUNT(*) FROM MOM_Meetings WHERE MeetingDate < GETDATE() AND IsCancelled = 0",
                "cancelled" => "SELECT COUNT(*) FROM MOM_Meetings WHERE IsCancelled = 1",
                _ => "SELECT COUNT(*) FROM MOM_Meetings"
            };

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Get meetings by date range
        /// </summary>
        /// <param name="startDate">Start date</param>
        /// <param name="endDate">End date</param>
        /// <returns>DataTable with meetings in date range</returns>
        public static DataTable GetByDateRange(DateTime startDate, DateTime endDate)
        {
            string query = @"
                SELECT m.MeetingID, m.MeetingDate, m.MeetingDescription, m.IsCancelled,
                       mv.MeetingVenueName, mt.MeetingTypeName, d.DepartmentName
                FROM MOM_Meetings m
                INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
                INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
                INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
                WHERE CAST(m.MeetingDate AS DATE) BETWEEN CAST(@StartDate AS DATE) AND CAST(@EndDate AS DATE)
                ORDER BY m.MeetingDate";

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@StartDate", startDate);
                    cmd.Parameters.AddWithValue("@EndDate", endDate);

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
        /// Search meetings
        /// </summary>
        /// <param name="searchTerm">Search term</param>
        /// <returns>DataTable with matching meetings</returns>
        public static DataTable Search(string searchTerm)
        {
            string query = @"
                SELECT m.MeetingID, m.MeetingDate, m.MeetingDescription, m.IsCancelled,
                       mv.MeetingVenueName, mt.MeetingTypeName, d.DepartmentName
                FROM MOM_Meetings m
                INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
                INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
                INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
                WHERE m.MeetingDescription LIKE @SearchTerm
                   OR mv.MeetingVenueName LIKE @SearchTerm
                   OR mt.MeetingTypeName LIKE @SearchTerm
                   OR d.DepartmentName LIKE @SearchTerm
                ORDER BY m.MeetingDate DESC";

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
        /// Alias for GetByDateRange used by controllers expecting SelectByDateRange
        /// </summary>
        public static DataTable SelectByDateRange(DateTime startDate, DateTime endDate)
        {
            return GetByDateRange(startDate, endDate);
        }
    }
}