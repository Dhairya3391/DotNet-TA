using System.Data.SqlClient;
using System.Data;
using MinuteOfMeeting.Models;

namespace MinuteOfMeeting.DAL
{
    /// <summary>
    /// Meeting Venue Data Access Layer
    /// Handles all database operations for MeetingVenue entity
    /// </summary>
    public class MeetingVenueDAL
    {
        /// <summary>
        /// Get all meeting venues
        /// </summary>
        /// <returns>DataTable with all meeting venues</returns>
        public static DataTable SelectAll()
        {
            return DBHelper.ExecuteProcedure("PR_MeetingVenue_SelectAll");
        }

        /// <summary>
        /// Get meeting venue by ID
        /// </summary>
        /// <param name="meetingVenueID">Meeting Venue ID</param>
        /// <returns>DataTable with venue details</returns>
        public static DataTable SelectByPK(int meetingVenueID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingVenueID", SqlDbType.Int) { Value = meetingVenueID }
            };

            return DBHelper.ExecuteProcedure("PR_MeetingVenue_SelectByPK", parameters);
        }

        /// <summary>
        /// Get meeting venues for dropdown (ID and Name only)
        /// </summary>
        /// <returns>DataTable with venue ID and name</returns>
        public static DataTable SelectForDropdown()
        {
            return DBHelper.ExecuteProcedure("PR_MeetingVenue_SelectForDropdown");
        }

        /// <summary>
        /// Insert new meeting venue
        /// </summary>
        /// <param name="model">MeetingVenue model</param>
        /// <returns>Newly created Meeting Venue ID</returns>
        public static int Insert(MeetingVenue model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingVenueName", SqlDbType.NVarChar, 100) { Value = model.MeetingVenueName },
                new SqlParameter("@Created", SqlDbType.DateTime) { Value = model.Created },
                new SqlParameter("@Modified", SqlDbType.DateTime) { Value = model.Modified }
            };

            var outputParameters = new Dictionary<string, object>
            {
                { "@MeetingVenueID", null }
            };

            DBHelper.ExecuteNonQueryWithOutput("PR_MeetingVenue_Insert", outputParameters, parameters);

            return Convert.ToInt32(outputParameters["@MeetingVenueID"]);
        }

        /// <summary>
        /// Update existing meeting venue
        /// </summary>
        /// <param name="model">MeetingVenue model with updated data</param>
        /// <returns>Number of rows affected</returns>
        public static int Update(MeetingVenue model)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingVenueID", SqlDbType.Int) { Value = model.MeetingVenueID },
                new SqlParameter("@MeetingVenueName", SqlDbType.NVarChar, 100) { Value = model.MeetingVenueName },
                new SqlParameter("@Modified", SqlDbType.DateTime) { Value = model.Modified }
            };

            return DBHelper.ExecuteNonQuery("PR_MeetingVenue_Update", parameters);
        }

        /// <summary>
        /// Delete meeting venue
        /// </summary>
        /// <param name="meetingVenueID">Meeting Venue ID to delete</param>
        /// <returns>Number of rows affected</returns>
        public static int Delete(int meetingVenueID)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingVenueID", SqlDbType.Int) { Value = meetingVenueID }
            };

            return DBHelper.ExecuteNonQuery("PR_MeetingVenue_Delete", parameters);
        }

        /// <summary>
        /// Check if venue name already exists
        /// </summary>
        /// <param name="venueName">Venue Name</param>
        /// <param name="excludeVenueID">ID to exclude (for update scenarios)</param>
        /// <returns>True if name exists, false otherwise</returns>
        public static bool CheckVenueNameExists(string venueName, int? excludeVenueID = null)
        {
            return CheckNameExistsInternal(venueName, excludeVenueID);
        }

        // Legacy helper retained for any existing callers
        public static bool CheckNameExists(string venueName, int? excludeVenueID = null)
        {
            return CheckNameExistsInternal(venueName, excludeVenueID);
        }

        private static bool CheckNameExistsInternal(string venueName, int? excludeVenueID)
        {
            string query = "SELECT COUNT(*) FROM MOM_MeetingVenue WHERE MeetingVenueName = @MeetingVenueName";

            if (excludeVenueID.HasValue)
            {
                query += " AND MeetingVenueID != @ExcludeMeetingVenueID";
            }

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@MeetingVenueName", venueName);

                    if (excludeVenueID.HasValue)
                    {
                        cmd.Parameters.AddWithValue("@ExcludeMeetingVenueID", excludeVenueID.Value);
                    }

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Check if venue is in use by meetings
        /// </summary>
        /// <param name="meetingVenueID">Meeting Venue ID</param>
        /// <returns>True if in use, false otherwise</returns>
        public static bool CheckInUse(int meetingVenueID)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(
                    "SELECT COUNT(*) FROM MOM_Meetings WHERE MeetingVenueID = @MeetingVenueID", conn))
                {
                    cmd.Parameters.AddWithValue("@MeetingVenueID", meetingVenueID);

                    conn.Open();
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        /// <summary>
        /// Check venue availability (conflict detection)
        /// </summary>
        /// <param name="meetingVenueID">Venue ID</param>
        /// <param name="meetingDate">Meeting date and time</param>
        /// <param name="excludeMeetingID">Meeting ID to exclude (for updates)</param>
        /// <returns>True if conflict exists, false otherwise</returns>
        public static bool CheckAvailabilityConflict(int meetingVenueID, DateTime meetingDate, int? excludeMeetingID = null)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@MeetingVenueID", SqlDbType.Int) { Value = meetingVenueID },
                new SqlParameter("@MeetingDate", SqlDbType.DateTime) { Value = meetingDate },
                new SqlParameter("@ExcludeMeetingID", SqlDbType.Int) { Value = (object?)excludeMeetingID ?? DBNull.Value }
            };

            var outputParameters = new Dictionary<string, object>
            {
                { "@HasConflict", null }
            };

            DBHelper.ExecuteNonQueryWithOutput("PR_MeetingVenue_CheckAvailability", outputParameters, parameters);

            return Convert.ToBoolean(outputParameters["@HasConflict"]);
        }

        /// <summary>
        /// Get venue count
        /// </summary>
        /// <returns>Total number of venues</returns>
        public static int GetCount()
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM MOM_MeetingVenue", conn))
                {
                    conn.Open();
                    return Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
        }

        /// <summary>
        /// Get most used venues (by meeting count)
        /// </summary>
        /// <param name="topCount">Number of top venues to return</param>
        /// <returns>DataTable with venue usage statistics</returns>
        public static DataTable GetMostUsedVenues(int topCount = 5)
        {
            string query = @"
                SELECT TOP (@TopCount)
                    mv.MeetingVenueID,
                    mv.MeetingVenueName,
                    COUNT(m.MeetingID) AS MeetingCount,
                    MAX(m.MeetingDate) AS LastMeetingDate
                FROM MOM_MeetingVenue mv
                LEFT JOIN MOM_Meetings m ON mv.MeetingVenueID = m.MeetingVenueID AND m.IsCancelled = 0
                GROUP BY mv.MeetingVenueID, mv.MeetingVenueName
                HAVING COUNT(m.MeetingID) > 0
                ORDER BY MeetingCount DESC";

            using (SqlConnection conn = DBHelper.GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@TopCount", topCount);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        public static bool CheckAvailability(int meetingVenueID, DateTime meetingDate, int? excludeMeetingID = null)
        {
            return !CheckAvailabilityConflict(meetingVenueID, meetingDate, excludeMeetingID);
        }

        public static DataTable GetVenueSchedule(int meetingVenueID, DateTime? startDate = null, DateTime? endDate = null)
        {
            string query = @"
                SELECT m.MeetingID, m.MeetingDate, m.MeetingDescription, d.DepartmentName, mt.MeetingTypeName
                FROM MOM_Meetings m
                INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
                INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
                WHERE m.MeetingVenueID = @MeetingVenueID
                  AND (@StartDate IS NULL OR m.MeetingDate >= @StartDate)
                  AND (@EndDate IS NULL OR m.MeetingDate <= @EndDate)
                ORDER BY m.MeetingDate";

            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MeetingVenueID", meetingVenueID);
                cmd.Parameters.AddWithValue("@StartDate", (object?)startDate ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@EndDate", (object?)endDate ?? DBNull.Value);

                using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }

        public static int GetTotalMeetingCount(int meetingVenueID)
        {
            const string query = "SELECT COUNT(*) FROM MOM_Meetings WHERE MeetingVenueID = @MeetingVenueID";
            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MeetingVenueID", meetingVenueID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int GetUpcomingMeetingCount(int meetingVenueID)
        {
            const string query = "SELECT COUNT(*) FROM MOM_Meetings WHERE MeetingVenueID = @MeetingVenueID AND MeetingDate >= GETDATE() AND IsCancelled = 0";
            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MeetingVenueID", meetingVenueID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static int GetThisWeekMeetingCount(int meetingVenueID)
        {
            const string query = @"
                SELECT COUNT(*) FROM MOM_Meetings
                WHERE MeetingVenueID = @MeetingVenueID
                  AND MeetingDate >= DATEADD(DAY, 1 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
                  AND MeetingDate < DATEADD(DAY, 8 - DATEPART(WEEKDAY, GETDATE()), CAST(GETDATE() AS DATE))
                  AND IsCancelled = 0";

            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MeetingVenueID", meetingVenueID);
                conn.Open();
                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }

        public static double GetUtilizationRate(int meetingVenueID)
        {
            // Simple utilization metric: meetings scheduled this month
            string query = @"
                SELECT CASE WHEN COUNT(*) = 0 THEN 0
                            ELSE (COUNT(*) * 1.0) END
                FROM MOM_Meetings
                WHERE MeetingVenueID = @MeetingVenueID
                  AND MONTH(MeetingDate) = MONTH(GETDATE())
                  AND YEAR(MeetingDate) = YEAR(GETDATE())
                  AND IsCancelled = 0";

            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MeetingVenueID", meetingVenueID);
                conn.Open();
                object result = cmd.ExecuteScalar();
                return result == DBNull.Value ? 0 : Convert.ToDouble(result);
            }
        }

        public static DataTable GetVenueAvailabilityCalendar(int meetingVenueID, DateTime startDate, DateTime endDate)
        {
            string query = @"
                SELECT MeetingDate, MeetingDescription, DepartmentID, MeetingTypeID
                FROM MOM_Meetings
                WHERE MeetingVenueID = @MeetingVenueID
                  AND MeetingDate BETWEEN @StartDate AND @EndDate
                ORDER BY MeetingDate";

            using (SqlConnection conn = DBHelper.GetConnection())
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@MeetingVenueID", meetingVenueID);
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
}