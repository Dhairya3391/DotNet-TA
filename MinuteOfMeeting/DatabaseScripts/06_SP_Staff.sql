/*
 * Database: MOM_Database
 * Purpose: Stored Procedures for MOM_Staff table
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

USE MOM_Database;
GO

-- =============================================
-- SP 1: Select All Staff with Department Names
-- =============================================
IF OBJECT_ID('PR_Staff_SelectAll', 'P') IS NOT NULL
    DROP PROCEDURE PR_Staff_SelectAll;
GO

CREATE PROCEDURE PR_Staff_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.StaffID,
        s.DepartmentID,
        s.StaffName,
        s.MobileNo,
        s.EmailAddress,
        s.Remarks,
        s.Created,
        s.Modified,
        d.DepartmentName
    FROM MOM_Staff s
    INNER JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
    ORDER BY s.StaffName;
END
GO

PRINT 'Procedure PR_Staff_SelectAll created';
GO

-- =============================================
-- SP 2: Select Staff By Primary Key
-- =============================================
IF OBJECT_ID('PR_Staff_SelectByPK', 'P') IS NOT NULL
    DROP PROCEDURE PR_Staff_SelectByPK;
GO

CREATE PROCEDURE PR_Staff_SelectByPK
    @StaffID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.StaffID,
        s.DepartmentID,
        s.StaffName,
        s.MobileNo,
        s.EmailAddress,
        s.Remarks,
        s.Created,
        s.Modified,
        d.DepartmentName
    FROM MOM_Staff s
    INNER JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
    WHERE s.StaffID = @StaffID;
END
GO

PRINT 'Procedure PR_Staff_SelectByPK created';
GO

-- =============================================
-- SP 3: Select Staff By Department
-- =============================================
IF OBJECT_ID('PR_Staff_SelectByDepartment', 'P') IS NOT NULL
    DROP PROCEDURE PR_Staff_SelectByDepartment;
GO

CREATE PROCEDURE PR_Staff_SelectByDepartment
    @DepartmentID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.StaffID,
        s.DepartmentID,
        s.StaffName,
        s.MobileNo,
        s.EmailAddress,
        s.Remarks,
        s.Created,
        s.Modified,
        d.DepartmentName
    FROM MOM_Staff s
    INNER JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
    WHERE s.DepartmentID = @DepartmentID
    ORDER BY s.StaffName;
END
GO

PRINT 'Procedure PR_Staff_SelectByDepartment created';
GO

-- =============================================
-- SP 4: Insert New Staff
-- =============================================
IF OBJECT_ID('PR_Staff_Insert', 'P') IS NOT NULL
    DROP PROCEDURE PR_Staff_Insert;
GO

CREATE PROCEDURE PR_Staff_Insert
    @DepartmentID INT,
    @StaffName NVARCHAR(50),
    @MobileNo NVARCHAR(20),
    @EmailAddress NVARCHAR(50),
    @Remarks NVARCHAR(250),
    @Created DATETIME,
    @Modified DATETIME,
    @StaffID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if department exists
        IF NOT EXISTS (SELECT 1 FROM MOM_Department WHERE DepartmentID = @DepartmentID)
        BEGIN
            RAISERROR('Department not found', 16, 1);
            RETURN;
        END

        -- Check for duplicate email
        IF EXISTS (SELECT 1 FROM MOM_Staff WHERE EmailAddress = @EmailAddress)
        BEGIN
            RAISERROR('Staff member with this email address already exists', 16, 1);
            RETURN;
        END

        INSERT INTO MOM_Staff
        (
            DepartmentID,
            StaffName,
            MobileNo,
            EmailAddress,
            Remarks,
            Created,
            Modified
        )
        VALUES
        (
            @DepartmentID,
            @StaffName,
            @MobileNo,
            @EmailAddress,
            @Remarks,
            @Created,
            @Modified
        );

        SET @StaffID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_Staff_Insert created';
GO

-- =============================================
-- SP 5: Update Staff
-- =============================================
IF OBJECT_ID('PR_Staff_Update', 'P') IS NOT NULL
    DROP PROCEDURE PR_Staff_Update;
GO

CREATE PROCEDURE PR_Staff_Update
    @StaffID INT,
    @DepartmentID INT,
    @StaffName NVARCHAR(50),
    @MobileNo NVARCHAR(20),
    @EmailAddress NVARCHAR(50),
    @Remarks NVARCHAR(250),
    @Modified DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if record exists
        IF NOT EXISTS (SELECT 1 FROM MOM_Staff WHERE StaffID = @StaffID)
        BEGIN
            RAISERROR('Staff member not found', 16, 1);
            RETURN;
        END

        -- Check if department exists
        IF NOT EXISTS (SELECT 1 FROM MOM_Department WHERE DepartmentID = @DepartmentID)
        BEGIN
            RAISERROR('Department not found', 16, 1);
            RETURN;
        END

        -- Check for duplicate email (excluding current record)
        IF EXISTS (
            SELECT 1 FROM MOM_Staff
            WHERE EmailAddress = @EmailAddress
            AND StaffID != @StaffID
        )
        BEGIN
            RAISERROR('Staff member with this email address already exists', 16, 1);
            RETURN;
        END

        UPDATE MOM_Staff
        SET
            DepartmentID = @DepartmentID,
            StaffName = @StaffName,
            MobileNo = @MobileNo,
            EmailAddress = @EmailAddress,
            Remarks = @Remarks,
            Modified = @Modified
        WHERE StaffID = @StaffID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_Staff_Update created';
GO

-- =============================================
-- SP 6: Delete Staff
-- =============================================
IF OBJECT_ID('PR_Staff_Delete', 'P') IS NOT NULL
    DROP PROCEDURE PR_Staff_Delete;
GO

CREATE PROCEDURE PR_Staff_Delete
    @StaffID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if record exists
        IF NOT EXISTS (SELECT 1 FROM MOM_Staff WHERE StaffID = @StaffID)
        BEGIN
            RAISERROR('Staff member not found', 16, 1);
            RETURN;
        END

        -- Check if staff member is associated with any meetings
        IF EXISTS (SELECT 1 FROM MOM_MeetingMember WHERE StaffID = @StaffID)
        BEGIN
            RAISERROR('Cannot delete staff member. They have associated meetings', 16, 1);
            RETURN;
        END

        -- Check if staff member has a user account
        IF EXISTS (SELECT 1 FROM MOM_User WHERE StaffID = @StaffID)
        BEGIN
            RAISERROR('Cannot delete staff member. They have an associated user account', 16, 1);
            RETURN;
        END

        DELETE FROM MOM_Staff
        WHERE StaffID = @StaffID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_Staff_Delete created';
GO

-- =============================================
-- SP 7: Select Staff for Dropdown
-- =============================================
IF OBJECT_ID('PR_Staff_SelectForDropdown', 'P') IS NOT NULL
    DROP PROCEDURE PR_Staff_SelectForDropdown;
GO

CREATE PROCEDURE PR_Staff_SelectForDropdown
    @DepartmentID INT = NULL -- Optional filter by department
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.StaffID,
        s.StaffName + ' (' + d.DepartmentName + ')' AS DisplayNameWithDepartment,
        s.StaffName
    FROM MOM_Staff s
    INNER JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
    WHERE (@DepartmentID IS NULL OR s.DepartmentID = @DepartmentID)
    ORDER BY s.StaffName;
END
GO

PRINT 'Procedure PR_Staff_SelectForDropdown created';
GO

-- =============================================
-- SP 8: Check if Email Exists
-- Used for validation before insert/update
-- =============================================
IF OBJECT_ID('PR_Staff_CheckEmailExists', 'P') IS NOT NULL
    DROP PROCEDURE PR_Staff_CheckEmailExists;
GO

CREATE PROCEDURE PR_Staff_CheckEmailExists
    @EmailAddress NVARCHAR(50),
    @ExcludeStaffID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS EmailExists
    FROM MOM_Staff
    WHERE EmailAddress = @EmailAddress
    AND (@ExcludeStaffID IS NULL OR StaffID != @ExcludeStaffID);
END
GO

PRINT 'Procedure PR_Staff_CheckEmailExists created';
GO

PRINT '========================================';
PRINT 'All Staff stored procedures created!';
PRINT '========================================';
GO