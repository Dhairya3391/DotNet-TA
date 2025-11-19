/*
 * Database: MOM_Database
 * Purpose: Stored Procedures for MOM_Department table
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

USE MOM_Database;
GO

-- =============================================
-- SP 1: Select All Departments
-- =============================================
IF OBJECT_ID('PR_Department_SelectAll', 'P') IS NOT NULL
    DROP PROCEDURE PR_Department_SelectAll;
GO

CREATE PROCEDURE PR_Department_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DepartmentID,
        DepartmentName,
        Created,
        Modified
    FROM MOM_Department
    ORDER BY DepartmentName;
END
GO

PRINT 'Procedure PR_Department_SelectAll created';
GO

-- =============================================
-- SP 2: Select Department By Primary Key
-- =============================================
IF OBJECT_ID('PR_Department_SelectByPK', 'P') IS NOT NULL
    DROP PROCEDURE PR_Department_SelectByPK;
GO

CREATE PROCEDURE PR_Department_SelectByPK
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DepartmentID,
        DepartmentName,
        Created,
        Modified
    FROM MOM_Department
    WHERE DepartmentID = @DepartmentID;
END
GO

PRINT 'Procedure PR_Department_SelectByPK created';
GO

-- =============================================
-- SP 3: Insert New Department
-- =============================================
IF OBJECT_ID('PR_Department_Insert', 'P') IS NOT NULL
    DROP PROCEDURE PR_Department_Insert;
GO

CREATE PROCEDURE PR_Department_Insert
    @DepartmentName NVARCHAR(100),
    @Created DATETIME,
    @Modified DATETIME,
    @DepartmentID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check for duplicate name
        IF EXISTS (SELECT 1 FROM MOM_Department WHERE DepartmentName = @DepartmentName)
        BEGIN
            RAISERROR('Department with this name already exists', 16, 1);
            RETURN;
        END

        INSERT INTO MOM_Department
        (
            DepartmentName,
            Created,
            Modified
        )
        VALUES
        (
            @DepartmentName,
            @Created,
            @Modified
        );

        SET @DepartmentID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_Department_Insert created';
GO

-- =============================================
-- SP 4: Update Department
-- =============================================
IF OBJECT_ID('PR_Department_Update', 'P') IS NOT NULL
    DROP PROCEDURE PR_Department_Update;
GO

CREATE PROCEDURE PR_Department_Update
    @DepartmentID INT,
    @DepartmentName NVARCHAR(100),
    @Modified DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if record exists
        IF NOT EXISTS (SELECT 1 FROM MOM_Department WHERE DepartmentID = @DepartmentID)
        BEGIN
            RAISERROR('Department not found', 16, 1);
            RETURN;
        END

        -- Check for duplicate name (excluding current record)
        IF EXISTS (
            SELECT 1 FROM MOM_Department
            WHERE DepartmentName = @DepartmentName
            AND DepartmentID != @DepartmentID
        )
        BEGIN
            RAISERROR('Department with this name already exists', 16, 1);
            RETURN;
        END

        UPDATE MOM_Department
        SET
            DepartmentName = @DepartmentName,
            Modified = @Modified
        WHERE DepartmentID = @DepartmentID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_Department_Update created';
GO

-- =============================================
-- SP 5: Delete Department
-- =============================================
IF OBJECT_ID('PR_Department_Delete', 'P') IS NOT NULL
    DROP PROCEDURE PR_Department_Delete;
GO

CREATE PROCEDURE PR_Department_Delete
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if record exists
        IF NOT EXISTS (SELECT 1 FROM MOM_Department WHERE DepartmentID = @DepartmentID)
        BEGIN
            RAISERROR('Department not found', 16, 1);
            RETURN;
        END

        -- Check if department is being used in staff records
        IF EXISTS (SELECT 1 FROM MOM_Staff WHERE DepartmentID = @DepartmentID)
        BEGIN
            RAISERROR('Cannot delete department. It has associated staff members', 16, 1);
            RETURN;
        END

        -- Check if department is being used in meetings
        IF EXISTS (SELECT 1 FROM MOM_Meetings WHERE DepartmentID = @DepartmentID)
        BEGIN
            RAISERROR('Cannot delete department. It has associated meetings', 16, 1);
            RETURN;
        END

        DELETE FROM MOM_Department
        WHERE DepartmentID = @DepartmentID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_Department_Delete created';
GO

-- =============================================
-- SP 6: Select Departments for Dropdown
-- =============================================
IF OBJECT_ID('PR_Department_SelectForDropdown', 'P') IS NOT NULL
    DROP PROCEDURE PR_Department_SelectForDropdown;
GO

CREATE PROCEDURE PR_Department_SelectForDropdown
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        DepartmentID,
        DepartmentName
    FROM MOM_Department
    ORDER BY DepartmentName;
END
GO

PRINT 'Procedure PR_Department_SelectForDropdown created';
GO

PRINT '========================================';
PRINT 'All Department stored procedures created!';
PRINT '========================================';
GO
