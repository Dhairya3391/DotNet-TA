using System.Data;
using System.Data.SqlClient;
using MinuteOfMeeting.Models;

namespace MinuteOfMeeting.DAL
{
    /// <summary>
    /// Meeting Member Data Access Layer
    /// Handles attendance and participation tracking
    /// </summary>
    public class MeetingMemberDAL
    {
        /// <summary>
        /// Select all meeting members for a specific meeting
        /// </summary>
        public static DataTable SelectByMeeting(int meetingId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_SelectByMeeting", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MeetingID", meetingId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Select all meetings for a specific staff member
        /// </summary>
        public static DataTable SelectByStaff(int staffId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_SelectByStaff", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StaffID", staffId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Insert a new meeting member record
        /// </summary>
        public static int Insert(MeetingMember model)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_Insert", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MeetingID", model.MeetingID);
                cmd.Parameters.AddWithValue("@StaffID", model.StaffID);
                cmd.Parameters.AddWithValue("@IsPresent", model.IsPresent);
                cmd.Parameters.AddWithValue("@Remarks", model.Remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Created", DateTime.Now);
                cmd.Parameters.AddWithValue("@Modified", DateTime.Now);

                SqlParameter outputParam = new SqlParameter("@MeetingMemberID", SqlDbType.Int);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.ToInt32(outputParam.Value);
            }
        }

        /// <summary>
        /// Update attendance status for a meeting member
        /// </summary>
        public static bool UpdateAttendance(int meetingMemberId, bool isPresent, string remarks)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_UpdateAttendance", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MeetingMemberID", meetingMemberId);
                cmd.Parameters.AddWithValue("@IsPresent", isPresent);
                cmd.Parameters.AddWithValue("@Remarks", remarks ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@Modified", DateTime.Now);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Delete a meeting member record
        /// </summary>
        public static bool Delete(int meetingMemberId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_Delete", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MeetingMemberID", meetingMemberId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Delete all meeting members for a specific meeting
        /// </summary>
        public static bool DeleteByMeeting(int meetingId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_DeleteByMeeting", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MeetingID", meetingId);

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Get attendance summary for a meeting
        /// </summary>
        public static DataTable GetAttendanceSummary(int meetingId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_GetAttendanceSummary", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@MeetingID", meetingId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Get staff participation statistics
        /// </summary>
        public static DataTable GetStaffParticipation(int staffId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_GetStaffParticipation", conn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StaffID", staffId);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Check if staff member is already invited to a meeting
        /// </summary>
        public static bool IsStaffInvited(int meetingId, int staffId)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_IsStaffInvited", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MeetingID", meetingId);
                cmd.Parameters.AddWithValue("@StaffID", staffId);

                SqlParameter outputParam = new SqlParameter("@IsInvited", SqlDbType.Bit);
                outputParam.Direction = ParameterDirection.Output;
                cmd.Parameters.Add(outputParam);

                conn.Open();
                cmd.ExecuteNonQuery();
                return Convert.ToBoolean(outputParam.Value);
            }
        }

        /// <summary>
        /// Bulk insert meeting members using table-valued parameter
        /// </summary>
        public static bool BulkInsert(int meetingId, DataTable staffMembersTable)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_BulkInsert", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@MeetingID", meetingId);
                SqlParameter tvpParam = cmd.Parameters.AddWithValue("@StaffMembers", staffMembersTable);
                tvpParam.SqlDbType = SqlDbType.Structured;
                tvpParam.TypeName = "dbo.StaffMemberTableType";

                conn.Open();
                int rowsAffected = cmd.ExecuteNonQuery();
                return rowsAffected > 0;
            }
        }

        /// <summary>
        /// Get attendance rate for a department
        /// </summary>
        public static double GetDepartmentAttendanceRate(int departmentId, DateTime startDate, DateTime endDate)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_GetDepartmentAttendanceRate", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

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

        /// <summary>
        /// Get attendance summary for date range
        /// </summary>
        public static DataTable GetAttendanceSummary(DateTime startDate, DateTime endDate)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_GetAttendanceSummary", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@StartDate", startDate);
                cmd.Parameters.AddWithValue("@EndDate", endDate);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Get staff participation statistics
        /// </summary>
        public static DataTable GetStaffParticipationStats(int topCount = 10)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_GetStaffParticipationStats", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TopCount", topCount);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        /// <summary>
        /// Get monthly attendance trend
        /// </summary>
        public static DataTable GetMonthlyAttendanceTrend(int months = 12)
        {
            using (SqlConnection conn = DBHelper.GetConnection())
            {
                SqlCommand cmd = new SqlCommand("PR_MeetingMember_GetMonthlyTrend", conn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Months", months);

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }
    }
}