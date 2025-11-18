using Lab12_CRUDOperations.Models;
using System.Data;

namespace Lab12_CRUDOperations.Data
{
    public class EmployeeRepository
    {
        private readonly DatabaseHelper _dbHelper;

        public EmployeeRepository(DatabaseHelper dbHelper)
        {
            _dbHelper = dbHelper;
        }

        // Create: Insert new employee
        public CRUDResult InsertEmployee(Employee employee)
        {
            var parameters = new[]
            {
                _dbHelper.CreateParameter("@EmployeeCode", employee.EmployeeCode, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@FirstName", employee.FirstName, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@LastName", employee.LastName, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@Email", employee.Email, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@Department", employee.Department, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@Position", employee.Position, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@Salary", employee.Salary, SqlDbType.Decimal),
                _dbHelper.CreateParameter("@HireDate", employee.HireDate, SqlDbType.Date),
                _dbHelper.CreateParameter("@IsActive", employee.IsActive, SqlDbType.Bit)
            };

            return _dbHelper.ExecuteCRUDProcedure("PR_Employee_Insert", parameters);
        }

        // Read: Get all employees
        public List<Employee> GetAllEmployees()
        {
            List<Employee> employees = new List<Employee>();
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_Employee_SelectAll");

            foreach (DataRow row in dt.Rows)
            {
                employees.Add(MapDataRowToEmployee(row));
            }

            return employees;
        }

        // Read: Get employee by ID
        public Employee? GetEmployeeById(int employeeId)
        {
            var parameter = _dbHelper.CreateParameter("@EmployeeID", employeeId, SqlDbType.Int);
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_Employee_SelectByPK", parameter);

            if (dt.Rows.Count > 0)
            {
                return MapDataRowToEmployee(dt.Rows[0]);
            }

            return null;
        }

        // Update: Update existing employee
        public CRUDResult UpdateEmployee(Employee employee)
        {
            var parameters = new[]
            {
                _dbHelper.CreateParameter("@EmployeeID", employee.EmployeeID, SqlDbType.Int),
                _dbHelper.CreateParameter("@EmployeeCode", employee.EmployeeCode, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@FirstName", employee.FirstName, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@LastName", employee.LastName, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@Email", employee.Email, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@PhoneNumber", employee.PhoneNumber ?? (object)DBNull.Value, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@Department", employee.Department, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@Position", employee.Position, SqlDbType.NVarChar),
                _dbHelper.CreateParameter("@Salary", employee.Salary, SqlDbType.Decimal),
                _dbHelper.CreateParameter("@HireDate", employee.HireDate, SqlDbType.Date),
                _dbHelper.CreateParameter("@IsActive", employee.IsActive, SqlDbType.Bit)
            };

            return _dbHelper.ExecuteCRUDProcedure("PR_Employee_Update", parameters);
        }

        // Delete: Soft delete employee
        public CRUDResult DeleteEmployee(int employeeId)
        {
            var parameter = _dbHelper.CreateParameter("@EmployeeID", employeeId, SqlDbType.Int);
            return _dbHelper.ExecuteCRUDProcedure("PR_Employee_Delete", parameter);
        }

        // Search employees by name
        public List<Employee> SearchEmployees(string searchTerm)
        {
            List<Employee> employees = new List<Employee>();
            var parameter = _dbHelper.CreateParameter("@SearchTerm", searchTerm, SqlDbType.NVarChar);
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_Employee_SearchByName", parameter);

            foreach (DataRow row in dt.Rows)
            {
                employees.Add(MapDataRowToEmployee(row));
            }

            return employees;
        }

        // Get employees by department
        public List<Employee> GetEmployeesByDepartment(string department)
        {
            List<Employee> employees = new List<Employee>();
            var parameter = _dbHelper.CreateParameter("@Department", department, SqlDbType.NVarChar);
            DataTable dt = _dbHelper.ExecuteStoredProcedure("PR_Employee_SelectByDepartment", parameter);

            foreach (DataRow row in dt.Rows)
            {
                employees.Add(MapDataRowToEmployee(row));
            }

            return employees;
        }

        // Helper method to map DataRow to Employee object
        private Employee MapDataRowToEmployee(DataRow row)
        {
            return new Employee
            {
                EmployeeID = Convert.ToInt32(row["EmployeeID"]),
                EmployeeCode = row["EmployeeCode"].ToString(),
                FirstName = row["FirstName"].ToString(),
                LastName = row["LastName"].ToString(),
                Email = row["Email"].ToString(),
                PhoneNumber = row["PhoneNumber"] != DBNull.Value ? row["PhoneNumber"].ToString() : null,
                Department = row["Department"].ToString(),
                Position = row["Position"].ToString(),
                Salary = Convert.ToDecimal(row["Salary"]),
                HireDate = Convert.ToDateTime(row["HireDate"]),
                IsActive = Convert.ToBoolean(row["IsActive"]),
                CreatedDate = Convert.ToDateTime(row["CreatedDate"]),
                UpdatedDate = row["UpdatedDate"] != DBNull.Value ? Convert.ToDateTime(row["UpdatedDate"]) : null
            };
        }
    }
}