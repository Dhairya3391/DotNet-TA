---
title: Lab 12 - Prepare Stored Procedure for Insert, Update and Delete Command
sidebar_position: 12
---
# Prepare Stored Procedure for Insert, Update and Delete Command

## 1. Description
Stored procedures for Insert, Update, and Delete operations encapsulate data modification logic within the database. These procedures provide a secure, efficient way to modify data while maintaining data integrity, implementing business rules, and improving performance through pre-compilation and reduced network traffic.

## 2. Why It Is Important
Stored procedures for CRUD operations are essential for maintaining data consistency, security, and performance. They centralize business logic, prevent SQL injection attacks, improve performance through execution plan reuse, and provide a consistent interface for data modifications across different applications.

## 3. Real-World Examples
- Student management system inserting new student records, updating contact information, and deleting graduated students
- E-commerce platform adding products, updating inventory levels, and removing discontinued items
- Healthcare application registering patients, updating medical records, and archiving old records
- Banking system creating accounts, updating balances, and closing inactive accounts
- Inventory management adding stock items, updating quantities, and removing expired products
- HR management hiring employees, updating salary information, and processing resignations

## 4. Syntax & Explanation

### Insert Stored Procedures

```sql
-- Insert Country
CREATE PROCEDURE usp_Country_Insert
    @CountryName NVARCHAR(100),
    @CountryCode NVARCHAR(10),
    @NewId INT OUTPUT
AS
BEGIN
    INSERT INTO Country (CountryName, CountryCode, CreatedDate)
    VALUES (@CountryName, @CountryCode, GETDATE());

    SET @NewId = SCOPE_IDENTITY();
END;

-- Insert State
CREATE PROCEDURE usp_State_Insert
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10),
    @CountryId INT,
    @NewId INT OUTPUT
AS
BEGIN
    INSERT INTO State (StateName, StateCode, CountryId, CreatedDate)
    VALUES (@StateName, @StateCode, @CountryId, GETDATE());

    SET @NewId = SCOPE_IDENTITY();
END;

-- Insert City
CREATE PROCEDURE usp_City_Insert
    @CityName NVARCHAR(100),
    @CityCode NVARCHAR(10),
    @StateId INT,
    @PinCode NVARCHAR(10),
    @NewId INT OUTPUT
AS
BEGIN
    INSERT INTO City (CityName, CityCode, StateId, PinCode, CreatedDate)
    VALUES (@CityName, @CityCode, @StateId, @PinCode, GETDATE());

    SET @NewId = SCOPE_IDENTITY();
END;
```

### Update Stored Procedures

```sql
-- Update Country
CREATE PROCEDURE usp_Country_Update
    @CountryId INT,
    @CountryName NVARCHAR(100),
    @CountryCode NVARCHAR(10)
AS
BEGIN
    UPDATE Country
    SET CountryName = @CountryName,
        CountryCode = @CountryCode,
        UpdatedDate = GETDATE()
    WHERE CountryId = @CountryId;
END;

-- Update State
CREATE PROCEDURE usp_State_Update
    @StateId INT,
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10),
    @CountryId INT
AS
BEGIN
    UPDATE State
    SET StateName = @StateName,
        StateCode = @StateCode,
        CountryId = @CountryId,
        UpdatedDate = GETDATE()
    WHERE StateId = @StateId;
END;

-- Update City
CREATE PROCEDURE usp_City_Update
    @CityId INT,
    @CityName NVARCHAR(100),
    @CityCode NVARCHAR(10),
    @StateId INT,
    @PinCode NVARCHAR(10)
AS
BEGIN
    UPDATE City
    SET CityName = @CityName,
        CityCode = @CityCode,
        StateId = @StateId,
        PinCode = @PinCode,
        UpdatedDate = GETDATE()
    WHERE CityId = @CityId;
END;
```

### Delete Stored Procedures

```sql
-- Delete Country
CREATE PROCEDURE usp_Country_Delete
    @CountryId INT
AS
BEGIN
    DELETE FROM Country WHERE CountryId = @CountryId;
END;

-- Delete State
CREATE PROCEDURE usp_State_Delete
    @StateId INT
AS
BEGIN
    DELETE FROM State WHERE StateId = @StateId;
END;

-- Delete City
CREATE PROCEDURE usp_City_Delete
    @CityId INT
AS
BEGIN
    DELETE FROM City WHERE CityId = @CityId;
END;
```

### C# Implementation for Calling Stored Procedures

```csharp
using Microsoft.Data.SqlClient;
using System.Data;

public class AddressBookRepository
{
    private readonly string _connectionString;

    public AddressBookRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    // Country Operations
    public async Task<(int CountryId, string Message)> InsertCountryAsync(string countryName, string countryCode)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_Country_Insert", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CountryName", countryName);
                    command.Parameters.AddWithValue("@CountryCode", countryCode);

                    var newIdParam = new SqlParameter("@NewId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(newIdParam);

                    var returnParam = new SqlParameter
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(returnParam);

                    await command.ExecuteNonQueryAsync();

                    int returnValue = (int)returnParam.Value;
                    int newId = (int)newIdParam.Value;

                    if (returnValue == 0)
                    {
                        return (newId, "Country inserted successfully");
                    }
                    else
                    {
                        return (0, "Failed to insert country");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            return (0, $"Error: {ex.Message}");
        }
    }

    public async Task<string> UpdateCountryAsync(int countryId, string countryName, string countryCode)
    {
        try
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

                    var returnParam = new SqlParameter
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(returnParam);

                    await command.ExecuteNonQueryAsync();

                    int returnValue = (int)returnParam.Value;

                    return returnValue == 0 ? "Country updated successfully" : "Failed to update country";
                }
            }
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    public async Task<string> DeleteCountryAsync(int countryId)
    {
        try
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                using (var command = new SqlCommand("usp_Country_Delete", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@CountryId", countryId);

                    var returnParam = new SqlParameter
                    {
                        Direction = ParameterDirection.ReturnValue
                    };
                    command.Parameters.Add(returnParam);

                    await command.ExecuteNonQueryAsync();

                    int returnValue = (int)returnParam.Value;

                    return returnValue == 0 ? "Country deleted successfully" : "Failed to delete country";
                }
            }
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }

    // Similar methods for State and City operations...
}

// Usage in Controller
public class CountryController : Controller
{
    private readonly AddressBookRepository _repository;

    public CountryController(AddressBookRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
    public async Task<IActionResult> Create(string countryName, string countryCode)
    {
        var (countryId, message) = await _repository.InsertCountryAsync(countryName, countryCode);

        if (countryId > 0)
        {
            TempData["Success"] = message;
            return RedirectToAction(nameof(Index));
        }
        else
        {
            TempData["Error"] = message;
            return RedirectToAction(nameof(Create));
        }
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int countryId, string countryName, string countryCode)
    {
        var message = await _repository.UpdateCountryAsync(countryId, countryName, countryCode);

        if (message.Contains("successfully"))
        {
            TempData["Success"] = message;
        }
        else
        {
            TempData["Error"] = message;
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public async Task<IActionResult> Delete(int countryId)
    {
        var message = await _repository.DeleteCountryAsync(countryId);

        if (message.Contains("successfully"))
        {
            TempData["Success"] = message;
        }
        else
        {
            TempData["Error"] = message;
        }

        return RedirectToAction(nameof(Index));
    }
}
```

## 5. Use Cases

- **Address Book Systems**: Managing countries, states, and cities in contact directories
- **E-commerce Platforms**: Product categorization and regional shipping zones
- **Healthcare Systems**: Patient location data and hospital network management
- **Educational Institutions**: Campus locations and regional center management
- **Travel Applications**: Destination management and booking systems
- **Logistics Companies**: Regional distribution centers and route planning
- **Government Services**: Administrative divisions and public service locations

## 6. Mini Practice Task

1. **Basic CRUD Stored Procedures**:
   - Create Insert, Update, and Delete stored procedures for a Products table
   - Include proper validation and error handling
   - Test the procedures using SQL Server Management Studio

2. **Enhanced Stored Procedures**:
   - Add transaction handling for complex operations
   - Implement soft delete functionality
   - Add audit logging for all modifications
   - Include output parameters for returning additional information

3. **Advanced Features**:
   - Create stored procedures for bulk operations (bulk insert, update, delete)
   - Implement conditional updates based on business rules
   - Add table-valued parameters for passing multiple records
   - Create procedures with optional parameters for flexible operations
   - Implement retry logic for handling deadlocks and timeouts