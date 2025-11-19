/*
 * Database: MOM_Database
 * Purpose: Stored Procedures for MOM_MeetingVenue table
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

USE MOM_Database;
GO

-- =============================================
-- SP 1: Select All Meeting Venues
-- =============================================
IF OBJECT_ID('PR_MeetingVenue_SelectAll', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingVenue_SelectAll;
GO

CREATE PROCEDURE PR_MeetingVenue_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        MeetingVenueID,
        MeetingVenueName,
        Created,
        Modified
    FROM MOM_MeetingVenue
    ORDER BY MeetingVenueName;
END
GO

PRINT 'Procedure PR_MeetingVenue_SelectAll created';
GO

-- =============================================
-- SP 2: Select Meeting Venue By Primary Key
-- =============================================
IF OBJECT_ID('PR_MeetingVenue_SelectByPK', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingVenue_SelectByPK;
GO

CREATE PROCEDURE PR_MeetingVenue_SelectByPK
    @MeetingVenueID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        MeetingVenueID,
        MeetingVenueName,
        Created,
        Modified
    FROM MOM_MeetingVenue
    WHERE MeetingVenueID = @MeetingVenueID;
END
GO

PRINT 'Procedure PR_MeetingVenue_SelectByPK created';
GO

-- =============================================
-- SP 3: Insert New Meeting Venue
-- =============================================
IF OBJECT_ID('PR_MeetingVenue_Insert', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingVenue_Insert;
GO

CREATE PROCEDURE PR_MeetingVenue_Insert
    @MeetingVenueName NVARCHAR(100),
    @Created DATETIME,
    @Modified DATETIME,
    @MeetingVenueID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check for duplicate name
        IF EXISTS (SELECT 1 FROM MOM_MeetingVenue WHERE MeetingVenueName = @MeetingVenueName)
        BEGIN
            RAISERROR('Meeting venue with this name already exists', 16, 1);
            RETURN;
        END

        INSERT INTO MOM_MeetingVenue
        (
            MeetingVenueName,
            Created,
            Modified
        )
        VALUES
        (
            @MeetingVenueName,
            @Created,
            @Modified
        );

        SET @MeetingVenueID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_MeetingVenue_Insert created';
GO

-- =============================================
-- SP 4: Update Meeting Venue
-- =============================================
IF OBJECT_ID('PR_MeetingVenue_Update', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingVenue_Update;
GO

CREATE PROCEDURE PR_MeetingVenue_Update
    @MeetingVenueID INT,
    @MeetingVenueName NVARCHAR(100),
    @Modified DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if record exists
        IF NOT EXISTS (SELECT 1 FROM MOM_MeetingVenue WHERE MeetingVenueID = @MeetingVenueID)
        BEGIN
            RAISERROR('Meeting venue not found', 16, 1);
            RETURN;
        END

        -- Check for duplicate name (excluding current record)
        IF EXISTS (
            SELECT 1 FROM MOM_MeetingVenue
            WHERE MeetingVenueName = @MeetingVenueName
            AND MeetingVenueID != @MeetingVenueID
        )
        BEGIN
            RAISERROR('Meeting venue with this name already exists', 16, 1);
            RETURN;
        END

        UPDATE MOM_MeetingVenue
        SET
            MeetingVenueName = @MeetingVenueName,
            Modified = @Modified
        WHERE MeetingVenueID = @MeetingVenueID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_MeetingVenue_Update created';
GO

-- =============================================
-- SP 5: Delete Meeting Venue
-- =============================================
IF OBJECT_ID('PR_MeetingVenue_Delete', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingVenue_Delete;
GO

CREATE PROCEDURE PR_MeetingVenue_Delete
    @MeetingVenueID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if record exists
        IF NOT EXISTS (SELECT 1 FROM MOM_MeetingVenue WHERE MeetingVenueID = @MeetingVenueID)
        BEGIN
            RAISERROR('Meeting venue not found', 16, 1);
            RETURN;
        END

        -- Check if venue is being used in any meetings
        IF EXISTS (SELECT 1 FROM MOM_Meetings WHERE MeetingVenueID = @MeetingVenueID)
        BEGIN
            RAISERROR('Cannot delete meeting venue. It is being used in existing meetings', 16, 1);
            RETURN;
        END

        DELETE FROM MOM_MeetingVenue
        WHERE MeetingVenueID = @MeetingVenueID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_MeetingVenue_Delete created';
GO

-- =============================================
-- SP 6: Select Meeting Venues for Dropdown
-- =============================================
IF OBJECT_ID('PR_MeetingVenue_SelectForDropdown', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingVenue_SelectForDropdown;
GO

CREATE PROCEDURE PR_MeetingVenue_SelectForDropdown
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        MeetingVenueID,
        MeetingVenueName
    FROM MOM_MeetingVenue
    ORDER BY MeetingVenueName;
END
GO

PRINT 'Procedure PR_MeetingVenue_SelectForDropdown created';
GO

-- =============================================
-- SP 7: Check Venue Availability (Conflict Detection)
-- Checks if venue is already booked for the given date/time
-- =============================================
IF OBJECT_ID('PR_MeetingVenue_CheckAvailability', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingVenue_CheckAvailability;
GO

CREATE PROCEDURE PR_MeetingVenue_CheckAvailability
    @MeetingVenueID INT,
    @MeetingDate DATETIME,
    @ExcludeMeetingID INT = NULL, -- For update scenarios
    @HasConflict BIT OUTPUT,
    @ConflictMeetingID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if venue is booked at the same date and time (within 1 hour window)
    -- Assuming meetings are typically 1 hour long
    SELECT TOP 1
        @ConflictMeetingID = MeetingID
    FROM MOM_Meetings
    WHERE MeetingVenueID = @MeetingVenueID
        AND IsCancelled = 0
        AND (
            -- Check if new meeting time conflicts with existing meeting
            -- Consider a 1-hour buffer for each meeting
            (@MeetingDate BETWEEN MeetingDate AND DATEADD(HOUR, 1, MeetingDate))
            OR
            (DATEADD(HOUR, 1, @MeetingDate) BETWEEN MeetingDate AND DATEADD(HOUR, 1, MeetingDate))
        )
        AND (@ExcludeMeetingID IS NULL OR MeetingID != @ExcludeMeetingID);

    IF @ConflictMeetingID IS NOT NULL
        SET @HasConflict = 1;
    ELSE
        SET @HasConflict = 0;
END
GO

PRINT 'Procedure PR_MeetingVenue_CheckAvailability created';
GO

PRINT '========================================';
PRINT 'All MeetingVenue stored procedures created!';
PRINT '========================================';
GO
