using System.Data;
using System.Data.SqlClient;
using Lab12_CRUDOperations.Models;

namespace Lab12_CRUDOperations.Data
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

        // Execute stored procedure and return scalar value
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

        // Execute stored procedure and return CRUD result
        public CRUDResult ExecuteCRUDProcedure(string procedureName, params SqlParameter[] parameters)
        {
            try
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

                        // Execute and read results
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Check if this is an error result
                                if (reader.GetOrdinal("ErrorNumber") >= 0 && !reader.IsDBNull(reader.GetOrdinal("ErrorNumber")))
                                {
                                    return CRUDResult.ErrorResult(
                                        reader.GetString(reader.GetOrdinal("ErrorMessage")),
                                        reader.GetInt32(reader.GetOrdinal("ErrorNumber")).ToString()
                                    );
                                }

                                // Success result
                                string message = reader.GetString(reader.GetOrdinal("Message"));
                                int rowsAffected = reader.GetInt32(reader.GetOrdinal("RowsAffected"));

                                // Check for generated ID (for insert operations)
                                int? generatedId = null;
                                if (reader.GetOrdinal("EmployeeID") >= 0 && !reader.IsDBNull(reader.GetOrdinal("EmployeeID")))
                                {
                                    generatedId = reader.GetInt32(reader.GetOrdinal("EmployeeID"));
                                }
                                else if (reader.GetOrdinal("CountryID") >= 0 && !reader.IsDBNull(reader.GetOrdinal("CountryID")))
                                {
                                    generatedId = reader.GetInt32(reader.GetOrdinal("CountryID"));
                                }
                                else if (reader.GetOrdinal("StateID") >= 0 && !reader.IsDBNull(reader.GetOrdinal("StateID")))
                                {
                                    generatedId = reader.GetInt32(reader.GetOrdinal("StateID"));
                                }
                                else if (reader.GetOrdinal("CityID") >= 0 && !reader.IsDBNull(reader.GetOrdinal("CityID")))
                                {
                                    generatedId = reader.GetInt32(reader.GetOrdinal("CityID"));
                                }

                                return CRUDResult.SuccessResult(message, rowsAffected, generatedId);
                            }
                        }
                    }
                }
            }
            catch (SqlException ex)
            {
                return CRUDResult.ErrorResult(ex.Message, ex.Number.ToString());
            }
            catch (Exception ex)
            {
                return CRUDResult.ErrorResult(ex.Message);
            }

            return CRUDResult.ErrorResult("Unknown error occurred");
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

        // Create SqlParameter with direction
        public SqlParameter CreateParameter(string name, object value, SqlDbType dbType, ParameterDirection direction)
        {
            return new SqlParameter
            {
                ParameterName = name,
                Value = value ?? DBNull.Value,
                SqlDbType = dbType,
                Direction = direction
            };
        }
    }
}