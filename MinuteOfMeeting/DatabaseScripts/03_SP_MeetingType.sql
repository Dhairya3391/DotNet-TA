/*
 * Database: MOM_Database
 * Purpose: Stored Procedures for MOM_MeetingType table
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

USE MOM_Database;
GO

-- =============================================
-- SP 1: Select All Meeting Types
-- =============================================
IF OBJECT_ID('PR_MeetingType_SelectAll', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingType_SelectAll;
GO

CREATE PROCEDURE PR_MeetingType_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        MeetingTypeID,
        MeetingTypeName,
        Remarks,
        Created,
        Modified
    FROM MOM_MeetingType
    ORDER BY MeetingTypeName;
END
GO

PRINT 'Procedure PR_MeetingType_SelectAll created';
GO

-- =============================================
-- SP 2: Select Meeting Type By Primary Key
-- =============================================
IF OBJECT_ID('PR_MeetingType_SelectByPK', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingType_SelectByPK;
GO

CREATE PROCEDURE PR_MeetingType_SelectByPK
    @MeetingTypeID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        MeetingTypeID,
        MeetingTypeName,
        Remarks,
        Created,
        Modified
    FROM MOM_MeetingType
    WHERE MeetingTypeID = @MeetingTypeID;
END
GO

PRINT 'Procedure PR_MeetingType_SelectByPK created';
GO

-- =============================================
-- SP 3: Insert New Meeting Type
-- =============================================
IF OBJECT_ID('PR_MeetingType_Insert', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingType_Insert;
GO

CREATE PROCEDURE PR_MeetingType_Insert
    @MeetingTypeName NVARCHAR(100),
    @Remarks NVARCHAR(100),
    @Created DATETIME,
    @Modified DATETIME,
    @MeetingTypeID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check for duplicate name
        IF EXISTS (SELECT 1 FROM MOM_MeetingType WHERE MeetingTypeName = @MeetingTypeName)
        BEGIN
            RAISERROR('Meeting type with this name already exists', 16, 1);
            RETURN;
        END

        INSERT INTO MOM_MeetingType
        (
            MeetingTypeName,
            Remarks,
            Created,
            Modified
        )
        VALUES
        (
            @MeetingTypeName,
            @Remarks,
            @Created,
            @Modified
        );

        SET @MeetingTypeID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_MeetingType_Insert created';
GO

-- =============================================
-- SP 4: Update Meeting Type
-- =============================================
IF OBJECT_ID('PR_MeetingType_Update', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingType_Update;
GO

CREATE PROCEDURE PR_MeetingType_Update
    @MeetingTypeID INT,
    @MeetingTypeName NVARCHAR(100),
    @Remarks NVARCHAR(100),
    @Modified DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if record exists
        IF NOT EXISTS (SELECT 1 FROM MOM_MeetingType WHERE MeetingTypeID = @MeetingTypeID)
        BEGIN
            RAISERROR('Meeting type not found', 16, 1);
            RETURN;
        END

        -- Check for duplicate name (excluding current record)
        IF EXISTS (
            SELECT 1 FROM MOM_MeetingType
            WHERE MeetingTypeName = @MeetingTypeName
            AND MeetingTypeID != @MeetingTypeID
        )
        BEGIN
            RAISERROR('Meeting type with this name already exists', 16, 1);
            RETURN;
        END

        UPDATE MOM_MeetingType
        SET
            MeetingTypeName = @MeetingTypeName,
            Remarks = @Remarks,
            Modified = @Modified
        WHERE MeetingTypeID = @MeetingTypeID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_MeetingType_Update created';
GO

-- =============================================
-- SP 5: Delete Meeting Type
-- =============================================
IF OBJECT_ID('PR_MeetingType_Delete', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingType_Delete;
GO

CREATE PROCEDURE PR_MeetingType_Delete
    @MeetingTypeID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if record exists
        IF NOT EXISTS (SELECT 1 FROM MOM_MeetingType WHERE MeetingTypeID = @MeetingTypeID)
        BEGIN
            RAISERROR('Meeting type not found', 16, 1);
            RETURN;
        END

        -- Check if meeting type is being used in any meetings
        IF EXISTS (SELECT 1 FROM MOM_Meetings WHERE MeetingTypeID = @MeetingTypeID)
        BEGIN
            RAISERROR('Cannot delete meeting type. It is being used in existing meetings', 16, 1);
            RETURN;
        END

        DELETE FROM MOM_MeetingType
        WHERE MeetingTypeID = @MeetingTypeID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_MeetingType_Delete created';
GO

-- =============================================
-- SP 6: Select Meeting Types for Dropdown
-- Returns ID and Name only for dropdown lists
-- =============================================
IF OBJECT_ID('PR_MeetingType_SelectForDropdown', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingType_SelectForDropdown;
GO

CREATE PROCEDURE PR_MeetingType_SelectForDropdown
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        MeetingTypeID,
        MeetingTypeName
    FROM MOM_MeetingType
    ORDER BY MeetingTypeName;
END
GO

PRINT 'Procedure PR_MeetingType_SelectForDropdown created';
GO

PRINT '========================================';
PRINT 'All MeetingType stored procedures created!';
PRINT '========================================';
GO
