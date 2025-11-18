-- =====================================================
-- Lab 12: Complete CRUD Stored Procedures
-- =====================================================
-- This script creates all stored procedures required for Lab 12:
-- Insert, Update, Delete operations for Country, State, City, and Employee tables
-- =====================================================

USE AddressBook;
GO

-- =====================================================
-- Country CRUD Stored Procedures
-- =====================================================

-- Procedure: PR_Country_Insert
-- Inserts a new country
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_Country_Insert' and xtype='P')
BEGIN
    DROP PROCEDURE PR_Country_Insert;
    PRINT 'Dropped existing PR_Country_Insert';
END
GO

CREATE PROCEDURE PR_Country_Insert
    @CountryName NVARCHAR(100),
    @CountryCode NVARCHAR(10),
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO Country (CountryName, CountryCode, IsActive)
        VALUES (@CountryName, @CountryCode, @IsActive);

        SELECT SCOPE_IDENTITY() AS CountryID, 'Country inserted successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS CountryID;
    END CATCH
END
GO
PRINT 'Created PR_Country_Insert';
GO

-- Procedure: PR_Country_Update
-- Updates an existing country
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_Country_Update' and xtype='P')
BEGIN
    DROP PROCEDURE PR_Country_Update;
    PRINT 'Dropped existing PR_Country_Update';
END
GO

CREATE PROCEDURE PR_Country_Update
    @CountryID INT,
    @CountryName NVARCHAR(100),
    @CountryCode NVARCHAR(10),
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE Country
        SET CountryName = @CountryName,
            CountryCode = @CountryCode,
            IsActive = @IsActive,
            UpdatedDate = GETDATE()
        WHERE CountryID = @CountryID;

        SELECT @@ROWCOUNT AS RowsAffected, 'Country updated successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS RowsAffected;
    END CATCH
END
GO
PRINT 'Created PR_Country_Update';
GO

-- Procedure: PR_Country_Delete
-- Deletes a country (soft delete by setting IsActive = 0)
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_Country_Delete' and xtype='P')
BEGIN
    DROP PROCEDURE PR_Country_Delete;
    PRINT 'Dropped existing PR_Country_Delete';
END
GO

CREATE PROCEDURE PR_Country_Delete
    @CountryID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if country has states
        IF EXISTS (SELECT 1 FROM State WHERE CountryID = @CountryID AND IsActive = 1)
        BEGIN
            SELECT 0 AS RowsAffected, 'Cannot delete country. It has associated states.' AS Message;
            RETURN;
        END

        UPDATE Country
        SET IsActive = 0,
            UpdatedDate = GETDATE()
        WHERE CountryID = @CountryID;

        SELECT @@ROWCOUNT AS RowsAffected, 'Country deleted successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS RowsAffected;
    END CATCH
END
GO
PRINT 'Created PR_Country_Delete';
GO

-- =====================================================
-- State CRUD Stored Procedures
-- =====================================================

-- Procedure: PR_State_Insert
-- Inserts a new state
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_State_Insert' and xtype='P')
BEGIN
    DROP PROCEDURE PR_State_Insert;
    PRINT 'Dropped existing PR_State_Insert';
END
GO

CREATE PROCEDURE PR_State_Insert
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10),
    @CountryID INT,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO State (StateName, StateCode, CountryID, IsActive)
        VALUES (@StateName, @StateCode, @CountryID, @IsActive);

        SELECT SCOPE_IDENTITY() AS StateID, 'State inserted successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS StateID;
    END CATCH
END
GO
PRINT 'Created PR_State_Insert';
GO

-- Procedure: PR_State_Update
-- Updates an existing state
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_State_Update' and xtype='P')
BEGIN
    DROP PROCEDURE PR_State_Update;
    PRINT 'Dropped existing PR_State_Update';
END
GO

CREATE PROCEDURE PR_State_Update
    @StateID INT,
    @StateName NVARCHAR(100),
    @StateCode NVARCHAR(10),
    @CountryID INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE State
        SET StateName = @StateName,
            StateCode = @StateCode,
            CountryID = @CountryID,
            IsActive = @IsActive,
            UpdatedDate = GETDATE()
        WHERE StateID = @StateID;

        SELECT @@ROWCOUNT AS RowsAffected, 'State updated successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS RowsAffected;
    END CATCH
END
GO
PRINT 'Created PR_State_Update';
GO

-- Procedure: PR_State_Delete
-- Deletes a state (soft delete)
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_State_Delete' and xtype='P')
BEGIN
    DROP PROCEDURE PR_State_Delete;
    PRINT 'Dropped existing PR_State_Delete';
END
GO

CREATE PROCEDURE PR_State_Delete
    @StateID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if state has cities
        IF EXISTS (SELECT 1 FROM City WHERE StateID = @StateID AND IsActive = 1)
        BEGIN
            SELECT 0 AS RowsAffected, 'Cannot delete state. It has associated cities.' AS Message;
            RETURN;
        END

        UPDATE State
        SET IsActive = 0,
            UpdatedDate = GETDATE()
        WHERE StateID = @StateID;

        SELECT @@ROWCOUNT AS RowsAffected, 'State deleted successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS RowsAffected;
    END CATCH
END
GO
PRINT 'Created PR_State_Delete';
GO

-- =====================================================
-- City CRUD Stored Procedures
-- =====================================================

-- Procedure: PR_City_Insert
-- Inserts a new city
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_City_Insert' and xtype='P')
BEGIN
    DROP PROCEDURE PR_City_Insert;
    PRINT 'Dropped existing PR_City_Insert';
END
GO

CREATE PROCEDURE PR_City_Insert
    @CityName NVARCHAR(100),
    @CityCode NVARCHAR(10),
    @StateID INT,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO City (CityName, CityCode, StateID, IsActive)
        VALUES (@CityName, @CityCode, @StateID, @IsActive);

        SELECT SCOPE_IDENTITY() AS CityID, 'City inserted successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS CityID;
    END CATCH
END
GO
PRINT 'Created PR_City_Insert';
GO

-- Procedure: PR_City_Update
-- Updates an existing city
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_City_Update' and xtype='P')
BEGIN
    DROP PROCEDURE PR_City_Update;
    PRINT 'Dropped existing PR_City_Update';
END
GO

CREATE PROCEDURE PR_City_Update
    @CityID INT,
    @CityName NVARCHAR(100),
    @CityCode NVARCHAR(10),
    @StateID INT,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE City
        SET CityName = @CityName,
            CityCode = @CityCode,
            StateID = @StateID,
            IsActive = @IsActive,
            UpdatedDate = GETDATE()
        WHERE CityID = @CityID;

        SELECT @@ROWCOUNT AS RowsAffected, 'City updated successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS RowsAffected;
    END CATCH
END
GO
PRINT 'Created PR_City_Update';
GO

-- Procedure: PR_City_Delete
-- Deletes a city (soft delete)
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_City_Delete' and xtype='P')
BEGIN
    DROP PROCEDURE PR_City_Delete;
    PRINT 'Dropped existing PR_City_Delete';
END
GO

CREATE PROCEDURE PR_City_Delete
    @CityID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE City
        SET IsActive = 0,
            UpdatedDate = GETDATE()
        WHERE CityID = @CityID;

        SELECT @@ROWCOUNT AS RowsAffected, 'City deleted successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS RowsAffected;
    END CATCH
END
GO
PRINT 'Created PR_City_Delete';
GO

-- =====================================================
-- Employee CRUD Stored Procedures (Custom Table)
-- =====================================================

-- Procedure: PR_Employee_Insert
-- Inserts a new employee
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_Employee_Insert' and xtype='P')
BEGIN
    DROP PROCEDURE PR_Employee_Insert;
    PRINT 'Dropped existing PR_Employee_Insert';
END
GO

CREATE PROCEDURE PR_Employee_Insert
    @EmployeeCode NVARCHAR(20),
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @Department NVARCHAR(50),
    @Position NVARCHAR(50),
    @Salary DECIMAL(18,2),
    @HireDate DATE,
    @IsActive BIT = 1
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        INSERT INTO Employee (EmployeeCode, FirstName, LastName, Email, PhoneNumber,
                             Department, Position, Salary, HireDate, IsActive)
        VALUES (@EmployeeCode, @FirstName, @LastName, @Email, @PhoneNumber,
                @Department, @Position, @Salary, @HireDate, @IsActive);

        SELECT SCOPE_IDENTITY() AS EmployeeID, 'Employee inserted successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS EmployeeID;
    END CATCH
END
GO
PRINT 'Created PR_Employee_Insert';
GO

-- Procedure: PR_Employee_Update
-- Updates an existing employee
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_Employee_Update' and xtype='P')
BEGIN
    DROP PROCEDURE PR_Employee_Update;
    PRINT 'Dropped existing PR_Employee_Update';
END
GO

CREATE PROCEDURE PR_Employee_Update
    @EmployeeID INT,
    @EmployeeCode NVARCHAR(20),
    @FirstName NVARCHAR(50),
    @LastName NVARCHAR(50),
    @Email NVARCHAR(100),
    @PhoneNumber NVARCHAR(20),
    @Department NVARCHAR(50),
    @Position NVARCHAR(50),
    @Salary DECIMAL(18,2),
    @HireDate DATE,
    @IsActive BIT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE Employee
        SET EmployeeCode = @EmployeeCode,
            FirstName = @FirstName,
            LastName = @LastName,
            Email = @Email,
            PhoneNumber = @PhoneNumber,
            Department = @Department,
            Position = @Position,
            Salary = @Salary,
            HireDate = @HireDate,
            IsActive = @IsActive,
            UpdatedDate = GETDATE()
        WHERE EmployeeID = @EmployeeID;

        SELECT @@ROWCOUNT AS RowsAffected, 'Employee updated successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS RowsAffected;
    END CATCH
END
GO
PRINT 'Created PR_Employee_Update';
GO

-- Procedure: PR_Employee_Delete
-- Deletes an employee (soft delete)
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_Employee_Delete' and xtype='P')
BEGIN
    DROP PROCEDURE PR_Employee_Delete;
    PRINT 'Dropped existing PR_Employee_Delete';
END
GO

CREATE PROCEDURE PR_Employee_Delete
    @EmployeeID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        UPDATE Employee
        SET IsActive = 0,
            UpdatedDate = GETDATE()
        WHERE EmployeeID = @EmployeeID;

        SELECT @@ROWCOUNT AS RowsAffected, 'Employee deleted successfully' AS Message;
    END TRY
    BEGIN CATCH
        SELECT ERROR_NUMBER() AS ErrorNumber,
               ERROR_MESSAGE() AS ErrorMessage,
               0 AS RowsAffected;
    END CATCH
END
GO
PRINT 'Created PR_Employee_Delete';
GO

-- =====================================================
-- Select Procedures (for reference and testing)
-- =====================================================

-- Procedure: PR_Country_SelectAll
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_Country_SelectAll' and xtype='P')
BEGIN
    DROP PROCEDURE PR_Country_SelectAll;
END
GO

CREATE PROCEDURE PR_Country_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT CountryID, CountryName, CountryCode, IsActive, CreatedDate, UpdatedDate
    FROM Country
    ORDER BY CountryName;
END
GO

-- Procedure: PR_Employee_SelectAll
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_Employee_SelectAll' and xtype='P')
BEGIN
    DROP PROCEDURE PR_Employee_SelectAll;
END
GO

CREATE PROCEDURE PR_Employee_SelectAll
AS
BEGIN
    SET NOCOUNT ON;
    SELECT EmployeeID, EmployeeCode, FirstName, LastName, Email, PhoneNumber,
           Department, Position, Salary, HireDate, IsActive, CreatedDate, UpdatedDate
    FROM Employee
    ORDER BY LastName, FirstName;
END
GO

PRINT '================================================';
PRINT 'All CRUD stored procedures created successfully!';
PRINT '================================================';
GO

-- Test procedures
PRINT 'Testing Employee CRUD Procedures:';
PRINT '';

-- Test Insert
PRINT '1. Testing PR_Employee_Insert:';
DECLARE @Result TABLE (EmployeeID INT, Message NVARCHAR(MAX));
INSERT INTO @Result EXEC PR_Employee_Insert 'TEST001', 'Test', 'User', 'test@example.com', '1234567890', 'IT', 'Tester', 50000.00, GETDATE(), 1;
SELECT * FROM @Result;
PRINT '';

-- Test Update
PRINT '2. Testing PR_Employee_Update (assuming EmployeeID = 9):';
DECLARE @UpdateResult TABLE (RowsAffected INT, Message NVARCHAR(MAX));
INSERT INTO @UpdateResult EXEC PR_Employee_Update 9, 'TEST001', 'Test', 'User Updated', 'test.updated@example.com', '1234567890', 'IT', 'Senior Tester', 55000.00, GETDATE(), 1;
SELECT * FROM @UpdateResult;
PRINT '';

-- Test Delete
PRINT '3. Testing PR_Employee_Delete (assuming EmployeeID = 9):';
DECLARE @DeleteResult TABLE (RowsAffected INT, Message NVARCHAR(MAX));
INSERT INTO @DeleteResult EXEC PR_Employee_Delete 9;
SELECT * FROM @DeleteResult;
PRINT '';

PRINT 'CRUD procedure testing completed!';