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
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if country already exists
        IF EXISTS (SELECT 1 FROM Country WHERE CountryName = @CountryName)
        BEGIN
            RAISERROR('Country already exists', 16, 1);
            RETURN -1;
        END

        -- Insert new country
        INSERT INTO Country (CountryName, CountryCode, CreatedDate)
        VALUES (@CountryName, @CountryCode, GETDATE());

        -- Return the new ID
        SET @NewId = SCOPE_IDENTITY();

        -- Return success
        RETURN 0;
    END TRY
    BEGIN CATCH
        -- Log error and return error code
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        DECLARE @ErrorSeverity INT = ERROR_SEVERITY();
        DECLARE @ErrorState INT = ERROR_STATE();

        RAISERROR(@ErrorMessage, @ErrorSeverity, @ErrorState);
        RETURN -1;
    END CATCH
END;

-- Insert State
CREATE PROCEDURE usp_State_Insert
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10),
    @CountryId INT,
    @NewId INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validate CountryId
        IF NOT EXISTS (SELECT 1 FROM Country WHERE CountryId = @CountryId)
        BEGIN
            RAISERROR('Invalid CountryId', 16, 1);
            RETURN -1;
        END

        -- Check if state already exists for this country
        IF EXISTS (SELECT 1 FROM State WHERE StateName = @StateName AND CountryId = @CountryId)
        BEGIN
            RAISERROR('State already exists for this country', 16, 1);
            RETURN -1;
        END

        -- Insert new state
        INSERT INTO State (StateName, StateCode, CountryId, CreatedDate)
        VALUES (@StateName, @StateCode, @CountryId, GETDATE());

        -- Return the new ID
        SET @NewId = SCOPE_IDENTITY();

        -- Return success
        RETURN 0;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
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
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validate StateId
        IF NOT EXISTS (SELECT 1 FROM State WHERE StateId = @StateId)
        BEGIN
            RAISERROR('Invalid StateId', 16, 1);
            RETURN -1;
        END

        -- Check if city already exists for this state
        IF EXISTS (SELECT 1 FROM City WHERE CityName = @CityName AND StateId = @StateId)
        BEGIN
            RAISERROR('City already exists for this state', 16, 1);
            RETURN -1;
        END

        -- Insert new city
        INSERT INTO City (CityName, CityCode, StateId, PinCode, CreatedDate)
        VALUES (@CityName, @CityCode, @StateId, @PinCode, GETDATE());

        -- Return the new ID
        SET @NewId = SCOPE_IDENTITY();

        -- Return success
        RETURN 0;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
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
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if country exists
        IF NOT EXISTS (SELECT 1 FROM Country WHERE CountryId = @CountryId)
        BEGIN
            RAISERROR('Country not found', 16, 1);
            RETURN -1;
        END

        -- Check for duplicate name (excluding current record)
        IF EXISTS (SELECT 1 FROM Country WHERE CountryName = @CountryName AND CountryId <> @CountryId)
        BEGIN
            RAISERROR('Country name already exists', 16, 1);
            RETURN -1;
        END

        -- Update country
        UPDATE Country
        SET CountryName = @CountryName,
            CountryCode = @CountryCode,
            UpdatedDate = GETDATE()
        WHERE CountryId = @CountryId;

        -- Return success
        RETURN 0;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
END;

-- Update State
CREATE PROCEDURE usp_State_Update
    @StateId INT,
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10),
    @CountryId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if state exists
        IF NOT EXISTS (SELECT 1 FROM State WHERE StateId = @StateId)
        BEGIN
            RAISERROR('State not found', 16, 1);
            RETURN -1;
        END

        -- Validate CountryId
        IF NOT EXISTS (SELECT 1 FROM Country WHERE CountryId = @CountryId)
        BEGIN
            RAISERROR('Invalid CountryId', 16, 1);
            RETURN -1;
        END

        -- Check for duplicate name (excluding current record)
        IF EXISTS (SELECT 1 FROM State WHERE StateName = @StateName AND CountryId = @CountryId AND StateId <> @StateId)
        BEGIN
            RAISERROR('State name already exists for this country', 16, 1);
            RETURN -1;
        END

        -- Update state
        UPDATE State
        SET StateName = @StateName,
            StateCode = @StateCode,
            CountryId = @CountryId,
            UpdatedDate = GETDATE()
        WHERE StateId = @StateId;

        -- Return success
        RETURN 0;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
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
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if city exists
        IF NOT EXISTS (SELECT 1 FROM City WHERE CityId = @CityId)
        BEGIN
            RAISERROR('City not found', 16, 1);
            RETURN -1;
        END

        -- Validate StateId
        IF NOT EXISTS (SELECT 1 FROM State WHERE StateId = @StateId)
        BEGIN
            RAISERROR('Invalid StateId', 16, 1);
            RETURN -1;
        END

        -- Check for duplicate name (excluding current record)
        IF EXISTS (SELECT 1 FROM City WHERE CityName = @CityName AND StateId = @StateId AND CityId <> @CityId)
        BEGIN
            RAISERROR('City name already exists for this state', 16, 1);
            RETURN -1;
        END

        -- Update city
        UPDATE City
        SET CityName = @CityName,
            CityCode = @CityCode,
            StateId = @StateId,
            PinCode = @PinCode,
            UpdatedDate = GETDATE()
        WHERE CityId = @CityId;

        -- Return success
        RETURN 0;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
END;
```

### Delete Stored Procedures

```sql
-- Delete Country
CREATE PROCEDURE usp_Country_Delete
    @CountryId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if country exists
        IF NOT EXISTS (SELECT 1 FROM Country WHERE CountryId = @CountryId)
        BEGIN
            RAISERROR('Country not found', 16, 1);
            RETURN -1;
        END

        -- Check if country has associated states
        IF EXISTS (SELECT 1 FROM State WHERE CountryId = @CountryId)
        BEGIN
            RAISERROR('Cannot delete country. Associated states exist.', 16, 1);
            RETURN -1;
        END

        -- Delete country (soft delete by setting IsActive flag)
        UPDATE Country
        SET IsActive = 0,
            UpdatedDate = GETDATE(),
            DeletedDate = GETDATE()
        WHERE CountryId = @CountryId;

        -- Return success
        RETURN 0;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
END;

-- Delete State
CREATE PROCEDURE usp_State_Delete
    @StateId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if state exists
        IF NOT EXISTS (SELECT 1 FROM State WHERE StateId = @StateId)
        BEGIN
            RAISERROR('State not found', 16, 1);
            RETURN -1;
        END

        -- Check if state has associated cities
        IF EXISTS (SELECT 1 FROM City WHERE StateId = @StateId)
        BEGIN
            RAISERROR('Cannot delete state. Associated cities exist.', 16, 1);
            RETURN -1;
        END

        -- Delete state (soft delete)
        UPDATE State
        SET IsActive = 0,
            UpdatedDate = GETDATE(),
            DeletedDate = GETDATE()
        WHERE StateId = @StateId;

        -- Return success
        RETURN 0;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
END;

-- Delete City
CREATE PROCEDURE usp_City_Delete
    @CityId INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if city exists
        IF NOT EXISTS (SELECT 1 FROM City WHERE CityId = @CityId)
        BEGIN
            RAISERROR('City not found', 16, 1);
            RETURN -1;
        END

        -- Check if city has associated records (e.g., addresses, contacts)
        IF EXISTS (SELECT 1 FROM Address WHERE CityId = @CityId)
        BEGIN
            RAISERROR('Cannot delete city. Associated addresses exist.', 16, 1);
            RETURN -1;
        END

        -- Delete city (soft delete)
        UPDATE City
        SET IsActive = 0,
            UpdatedDate = GETDATE(),
            DeletedDate = GETDATE()
        WHERE CityId = @CityId;

        -- Return success
        RETURN 0;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
        RETURN -1;
    END CATCH
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