using System.Data;
using System.Data.SqlClient;

namespace Lab11_StoredProcedures.Data
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        }

        // Execute stored procedure and return DataTable
        public DataTable ExecuteStoredProcedure(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    // Add parameters
                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);
                        return dataTable;
                    }
                }
            }
        }

        // Execute stored procedure and return single value
        public object ExecuteScalar(string procedureName, params SqlParameter[] parameters)
        {
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                using (SqlCommand command = new SqlCommand(procedureName, connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    if (parameters != null)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    connection.Open();
                    return command.ExecuteScalar();
                }
            }
        }

        // Create SqlParameter helper
        public SqlParameter CreateParameter(string name, object value, SqlDbType dbType)
        {
            return new SqlParameter
            {
                ParameterName = name,
                Value = value ?? DBNull.Value,
                SqlDbType = dbType
            };
        }
    }
}