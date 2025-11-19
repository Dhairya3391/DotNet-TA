using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace MinuteOfMeeting.DAL
{
    /// <summary>
    /// Database Helper Class
    /// Provides database connection management
    /// </summary>
    public static class DBHelper
    {
        /// <summary>
        /// Gets the database connection string from appsettings.json
        /// </summary>
        private static string ConnectionString
        {
            get
            {
                var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json");

                var configuration = builder.Build();
                return configuration.GetConnectionString("DefaultConnection");
            }
        }

        /// <summary>
        /// Creates and returns a new SQL connection
        /// </summary>
        /// <returns>SqlConnection instance</returns>
        public static SqlConnection GetConnection()
        {
            return new SqlConnection(ConnectionString);
        }

        /// <summary>
        /// Tests database connection
        /// </summary>
        /// <returns>True if connection is successful, false otherwise</returns>
        public static bool TestConnection()
        {
            try
            {
                using (SqlConnection conn = GetConnection())
                {
                    conn.Open();
                    return conn.State == System.Data.ConnectionState.Open;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns a DataTable
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure</param>
        /// <param name="parameters">Optional parameters</param>
        /// <returns>DataTable with results</returns>
        public static System.Data.DataTable ExecuteProcedure(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                    {
                        System.Data.DataTable dt = new System.Data.DataTable();
                        adapter.Fill(dt);
                        return dt;
                    }
                }
            }
        }

        /// <summary>
        /// Executes a non-query stored procedure (INSERT, UPDATE, DELETE)
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Number of rows affected</returns>
        public static int ExecuteNonQuery(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure and returns a single value
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure</param>
        /// <param name="parameters">Parameters</param>
        /// <returns>Single value result</returns>
        public static object ExecuteScalar(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    conn.Open();
                    return cmd.ExecuteScalar();
                }
            }
        }

        /// <summary>
        /// Executes a stored procedure with output parameters
        /// </summary>
        /// <param name="procedureName">Name of the stored procedure</param>
        /// <param name="outputParameters">Output parameters to populate</param>
        /// <param name="parameters">Input parameters</param>
        /// <returns>Number of rows affected</returns>
        public static int ExecuteNonQueryWithOutput(string procedureName, Dictionary<string, object> outputParameters, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = GetConnection())
            {
                using (SqlCommand cmd = new SqlCommand(procedureName, conn))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    if (parameters != null && parameters.Length > 0)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    // Add output parameters
                    if (outputParameters != null)
                    {
                        foreach (var param in outputParameters)
                        {
                            SqlParameter outParam = new SqlParameter(param.Key, SqlDbType.Int);
                            outParam.Direction = ParameterDirection.Output;
                            cmd.Parameters.Add(outParam);
                        }
                    }

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();

                    // Populate output parameters
                    if (outputParameters != null)
                    {
                        int index = 0;
                        foreach (var key in outputParameters.Keys.ToList())
                        {
                            outputParameters[key] = cmd.Parameters[parameters.Length + index].Value;
                            index++;
                        }
                    }

                    return rowsAffected;
                }
            }
        }
    }
}