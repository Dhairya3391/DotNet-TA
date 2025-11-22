# Stored Procedures for Insert, Update, and Delete

## 1. Description
This document provides a practical guide to creating and using **stored procedures (SPs)** in SQL Server for the fundamental **CRUD (Create, Read, Update, Delete)** operations. Stored procedures encapsulate SQL logic on the database server, offering significant benefits in security, performance, and maintainability.

- **Insert (Create):** Adds a new record to a table.
- **Update:** Modifies an existing record.
- **Delete:** Removes a record from a table.

## 2. Why It Is Important
Using stored procedures for data modification is a best practice for several reasons:
- **Security:** It helps prevent SQL injection attacks. Applications are given permission to execute the stored procedure, not to perform direct `INSERT`, `UPDATE`, or `DELETE` operations on the tables.
- **Performance:** Stored procedures are pre-compiled and their execution plans are cached by the database, leading to faster performance.
- **Data Integrity:** You can embed business rules and data validation logic directly into the stored procedure, ensuring that no invalid data is ever written to your tables.
- **Maintainability:** If your data modification logic needs to change, you only have to update the stored procedure in one place, rather than updating every application that interacts with the database.
- **Reduced Network Traffic:** The application only needs to send the procedure name and its parameters, not a potentially long and complex SQL query.

## 3. Real-World Examples
- An e-commerce site using a `usp_CreateOrder` stored procedure to ensure that when a new order is created, the inventory is also updated within the same transaction.
- A student management system using a `usp_UpdateStudentAddress` SP to modify a student's address, which might also trigger a logging mechanism for auditing purposes.
- A banking application using a `usp_SoftDeleteAccount` SP to deactivate a bank account instead of permanently deleting it, preserving the transaction history.

## 4. Syntax & Explanation (T-SQL for SQL Server)

This example uses a common database schema with `Country`, `State`, and `City` tables to demonstrate CRUD stored procedures.

### Insert Stored Procedures
These procedures add new records and typically return the ID of the newly created record via an `OUTPUT` parameter.

```sql
-- Insert a new Country
CREATE PROCEDURE usp_Country_Insert
    @CountryName NVARCHAR(100),
    @CountryCode NVARCHAR(10),
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO Country (CountryName, CountryCode, CreatedDate)
    VALUES (@CountryName, @CountryCode, GETDATE());

    SET @NewId = SCOPE_IDENTITY();
END;
GO

-- Insert a new State
CREATE PROCEDURE usp_State_Insert
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10),
    @CountryId INT,
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;
    INSERT INTO State (StateName, StateCode, CountryId, CreatedDate)
    VALUES (@StateName, @StateCode, @CountryId, GETDATE());

    SET @NewId = SCOPE_IDENTITY();
END;
GO
```

### Update Stored Procedures
These procedures find an existing record by its primary key and modify its data.

```sql
-- Update an existing Country
CREATE PROCEDURE usp_Country_Update
    @CountryId INT,
    @CountryName NVARCHAR(100),
    @CountryCode NVARCHAR(10)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE Country
    SET CountryName = @CountryName,
        CountryCode = @CountryCode,
        UpdatedDate = GETDATE()
    WHERE CountryId = @CountryId;
END;
GO

-- Update an existing State
CREATE PROCEDURE usp_State_Update
    @StateId INT,
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10),
    @CountryId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE State
    SET StateName = @StateName,
        StateCode = @StateCode,
        CountryId = @CountryId,
        UpdatedDate = GETDATE()
    WHERE StateId = @StateId;
END;
GO
```

### Delete Stored Procedures
These procedures remove records. It's often safer to implement a "soft delete" (marking a record as inactive) rather than a "hard delete" (permanently removing the record).

```sql
-- Hard Delete: Permanently removes the record
CREATE PROCEDURE usp_Country_Delete
    @CountryId INT
AS
BEGIN
    SET NOCOUNT ON;
    -- Be cautious: this permanently removes the data.
    -- You might want to check for related records in other tables first.
    DELETE FROM Country WHERE CountryId = @CountryId;
END;
GO

-- Soft Delete: Marks the record as inactive
CREATE PROCEDURE usp_State_Deactivate
    @StateId INT
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE State
    SET IsActive = 0, -- Assuming an 'IsActive' bit column exists
        UpdatedDate = GETDATE()
    WHERE StateId = @StateId;
END;
GO
```

### C# Implementation for Calling Stored Procedures (ADO.NET)
```csharp
using Microsoft.Data.SqlClient;
using System.Data;

public class CountryRepository
{
    private readonly string _connectionString;

    public CountryRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<int> InsertCountryAsync(string countryName, string countryCode)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("usp_Country_Insert", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CountryName", countryName);
                command.Parameters.AddWithValue("@CountryCode", countryCode);
                
                var newIdParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
                command.Parameters.Add(newIdParam);

                await command.ExecuteNonQueryAsync();
                return (int)newIdParam.Value;
            }
        }
    }

    public async Task UpdateCountryAsync(int countryId, string countryName, string countryCode)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("usp_Country_Update", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CountryId", countryId);
                command.Parameters.AddWithValue("@CountryName", countryName);
                command.Parameters.AddWithValue("@CountryCode", countryCode);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
    
    public async Task DeleteCountryAsync(int countryId)
    {
        using (var connection = new SqlConnection(_connectionString))
        {
            await connection.OpenAsync();
            using (var command = new SqlCommand("usp_Country_Delete", connection))
            {
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CountryId", countryId);
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
```

## 5. Mini Practice Task
1.  **Create a Table:** Create a simple `Departments` table with columns for `DepartmentId`, `Name`, and `IsActive`.
2.  **Write Insert SP:** Create a stored procedure `usp_Department_Insert` that adds a new department and returns its new `DepartmentId`.
3.  **Write Update SP:** Create `usp_Department_Update` to change the `Name` of an existing department.
4.  **Write "Soft Delete" SP:** Create `usp_Department_Deactivate` to set the `IsActive` flag to `0` for a given `DepartmentId`.
5.  **Test:** Execute each stored procedure in your database management tool to verify they work correctly.
