/*
 * Database: MOM_Database
 * Purpose: Stored Procedures for MOM_Meetings table
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

USE MOM_Database;
GO

-- =============================================
-- SP 1: Select All Meetings with Related Data
-- =============================================
IF OBJECT_ID('PR_Meeting_SelectAll', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_SelectAll;
GO

CREATE PROCEDURE PR_Meeting_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.MeetingID,
        m.MeetingDate,
        m.MeetingVenueID,
        m.MeetingTypeID,
        m.DepartmentID,
        m.MeetingDescription,
        m.DocumentPath,
        m.Created,
        m.Modified,
        m.IsCancelled,
        m.CancellationDateTime,
        m.CancellationReason,
        mv.MeetingVenueName,
        mt.MeetingTypeName,
        d.DepartmentName,
        -- Format meeting date for display
        FORMAT(m.MeetingDate, 'yyyy-MM-dd HH:mm') AS MeetingDateTime,
        -- Meeting status
        CASE
            WHEN m.IsCancelled = 1 THEN 'Cancelled'
            WHEN m.MeetingDate < GETDATE() THEN 'Completed'
            ELSE 'Upcoming'
        END AS MeetingStatus,
        -- Count of attendees
        ISNULL(mm.AttendeeCount, 0) AS AttendeeCount
    FROM MOM_Meetings m
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    LEFT JOIN (
        SELECT MeetingID, COUNT(*) AS AttendeeCount
        FROM MOM_MeetingMember
        GROUP BY MeetingID
    ) mm ON m.MeetingID = mm.MeetingID
    ORDER BY m.MeetingDate DESC;
END
GO

PRINT 'Procedure PR_Meeting_SelectAll created';
GO

-- =============================================
-- SP 2: Select Meeting By Primary Key
-- =============================================
IF OBJECT_ID('PR_Meeting_SelectByPK', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_SelectByPK;
GO

CREATE PROCEDURE PR_Meeting_SelectByPK
    @MeetingID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.MeetingID,
        m.MeetingDate,
        m.MeetingVenueID,
        m.MeetingTypeID,
        m.DepartmentID,
        m.MeetingDescription,
        m.DocumentPath,
        m.Created,
        m.Modified,
        m.IsCancelled,
        m.CancellationDateTime,
        m.CancellationReason,
        mv.MeetingVenueName,
        mt.MeetingTypeName,
        d.DepartmentName,
        FORMAT(m.MeetingDate, 'yyyy-MM-dd HH:mm') AS MeetingDateTime,
        CASE
            WHEN m.IsCancelled = 1 THEN 'Cancelled'
            WHEN m.MeetingDate < GETDATE() THEN 'Completed'
            ELSE 'Upcoming'
        END AS MeetingStatus
    FROM MOM_Meetings m
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    WHERE m.MeetingID = @MeetingID;
END
GO

PRINT 'Procedure PR_Meeting_SelectByPK created';
GO

-- =============================================
-- SP 3: Select Meetings with Filters
-- For advanced search and filtering
-- =============================================
IF OBJECT_ID('PR_Meeting_SelectWithFilters', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_SelectWithFilters;
GO

CREATE PROCEDURE PR_Meeting_SelectWithFilters
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL,
    @MeetingTypeID INT = NULL,
    @MeetingVenueID INT = NULL,
    @DepartmentID INT = NULL,
    @SearchKeyword NVARCHAR(250) = NULL,
    @Status NVARCHAR(20) = NULL -- 'Upcoming', 'Completed', 'Cancelled'
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.MeetingID,
        m.MeetingDate,
        m.MeetingVenueID,
        m.MeetingTypeID,
        m.DepartmentID,
        m.MeetingDescription,
        m.DocumentPath,
        m.Created,
        m.Modified,
        m.IsCancelled,
        m.CancellationDateTime,
        m.CancellationReason,
        mv.MeetingVenueName,
        mt.MeetingTypeName,
        d.DepartmentName,
        FORMAT(m.MeetingDate, 'yyyy-MM-dd HH:mm') AS MeetingDateTime,
        CASE
            WHEN m.IsCancelled = 1 THEN 'Cancelled'
            WHEN m.MeetingDate < GETDATE() THEN 'Completed'
            ELSE 'Upcoming'
        END AS MeetingStatus,
        ISNULL(mm.AttendeeCount, 0) AS AttendeeCount
    FROM MOM_Meetings m
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    LEFT JOIN (
        SELECT MeetingID, COUNT(*) AS AttendeeCount
        FROM MOM_MeetingMember
        GROUP BY MeetingID
    ) mm ON m.MeetingID = mm.MeetingID
    WHERE
        (@StartDate IS NULL OR CAST(m.MeetingDate AS DATE) >= CAST(@StartDate AS DATE)) AND
        (@EndDate IS NULL OR CAST(m.MeetingDate AS DATE) <= CAST(@EndDate AS DATE)) AND
        (@MeetingTypeID IS NULL OR m.MeetingTypeID = @MeetingTypeID) AND
        (@MeetingVenueID IS NULL OR m.MeetingVenueID = @MeetingVenueID) AND
        (@DepartmentID IS NULL OR m.DepartmentID = @DepartmentID) AND
        (@SearchKeyword IS NULL OR
         m.MeetingDescription LIKE '%' + @SearchKeyword + '%' OR
         mv.MeetingVenueName LIKE '%' + @SearchKeyword + '%' OR
         mt.MeetingTypeName LIKE '%' + @SearchKeyword + '%' OR
         d.DepartmentName LIKE '%' + @SearchKeyword + '%') AND
        (@Status IS NULL OR
         (@Status = 'Upcoming' AND m.MeetingDate >= GETDATE() AND m.IsCancelled = 0) OR
         (@Status = 'Completed' AND m.MeetingDate < GETDATE() AND m.IsCancelled = 0) OR
         (@Status = 'Cancelled' AND m.IsCancelled = 1))
    ORDER BY m.MeetingDate DESC;
END
GO

PRINT 'Procedure PR_Meeting_SelectWithFilters created';
GO

-- =============================================
-- SP 4: Select Upcoming Meetings
-- =============================================
IF OBJECT_ID('PR_Meeting_SelectUpcoming', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_SelectUpcoming;
GO

CREATE PROCEDURE PR_Meeting_SelectUpcoming
    @TopCount INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@TopCount)
        m.MeetingID,
        m.MeetingDate,
        m.MeetingDescription,
        mv.MeetingVenueName,
        mt.MeetingTypeName,
        d.DepartmentName,
        FORMAT(m.MeetingDate, 'yyyy-MM-dd HH:mm') AS MeetingDateTime,
        ISNULL(mm.AttendeeCount, 0) AS AttendeeCount
    FROM MOM_Meetings m
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    LEFT JOIN (
        SELECT MeetingID, COUNT(*) AS AttendeeCount
        FROM MOM_MeetingMember
        GROUP BY MeetingID
    ) mm ON m.MeetingID = mm.MeetingID
    WHERE m.MeetingDate >= GETDATE()
        AND m.IsCancelled = 0
    ORDER BY m.MeetingDate ASC;
END
GO

PRINT 'Procedure PR_Meeting_SelectUpcoming created';
GO

-- =============================================
-- SP 5: Select Completed Meetings
-- =============================================
IF OBJECT_ID('PR_Meeting_SelectCompleted', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_SelectCompleted;
GO

CREATE PROCEDURE PR_Meeting_SelectCompleted
    @TopCount INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@TopCount)
        m.MeetingID,
        m.MeetingDate,
        m.MeetingDescription,
        mv.MeetingVenueName,
        mt.MeetingTypeName,
        d.DepartmentName,
        FORMAT(m.MeetingDate, 'yyyy-MM-dd HH:mm') AS MeetingDateTime,
        ISNULL(mm.AttendeeCount, 0) AS AttendeeCount
    FROM MOM_Meetings m
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    LEFT JOIN (
        SELECT MeetingID, COUNT(*) AS AttendeeCount
        FROM MOM_MeetingMember
        GROUP BY MeetingID
    ) mm ON m.MeetingID = mm.MeetingID
    WHERE m.MeetingDate < GETDATE()
        AND m.IsCancelled = 0
    ORDER BY m.MeetingDate DESC;
END
GO

PRINT 'Procedure PR_Meeting_SelectCompleted created';
GO

-- =============================================
-- SP 6: Select Cancelled Meetings
-- =============================================
IF OBJECT_ID('PR_Meeting_SelectCancelled', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_SelectCancelled;
GO

CREATE PROCEDURE PR_Meeting_SelectCancelled
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        m.MeetingID,
        m.MeetingDate,
        m.MeetingDescription,
        m.CancellationDateTime,
        m.CancellationReason,
        mv.MeetingVenueName,
        mt.MeetingTypeName,
        d.DepartmentName,
        FORMAT(m.MeetingDate, 'yyyy-MM-dd HH:mm') AS MeetingDateTime
    FROM MOM_Meetings m
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    WHERE m.IsCancelled = 1
    ORDER BY m.CancellationDateTime DESC;
END
GO

PRINT 'Procedure PR_Meeting_SelectCancelled created';
GO

-- =============================================
-- SP 7: Insert New Meeting
-- =============================================
IF OBJECT_ID('PR_Meeting_Insert', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_Insert;
GO

CREATE PROCEDURE PR_Meeting_Insert
    @MeetingDate DATETIME,
    @MeetingVenueID INT,
    @MeetingTypeID INT,
    @DepartmentID INT,
    @MeetingDescription NVARCHAR(250),
    @DocumentPath NVARCHAR(250),
    @Created DATETIME,
    @Modified DATETIME,
    @MeetingID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Validate foreign keys
        IF NOT EXISTS (SELECT 1 FROM MOM_MeetingVenue WHERE MeetingVenueID = @MeetingVenueID)
        BEGIN
            RAISERROR('Invalid meeting venue', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM MOM_MeetingType WHERE MeetingTypeID = @MeetingTypeID)
        BEGIN
            RAISERROR('Invalid meeting type', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM MOM_Department WHERE DepartmentID = @DepartmentID)
        BEGIN
            RAISERROR('Invalid department', 16, 1);
            RETURN;
        END

        INSERT INTO MOM_Meetings
        (
            MeetingDate,
            MeetingVenueID,
            MeetingTypeID,
            DepartmentID,
            MeetingDescription,
            DocumentPath,
            Created,
            Modified,
            IsCancelled
        )
        VALUES
        (
            @MeetingDate,
            @MeetingVenueID,
            @MeetingTypeID,
            @DepartmentID,
            @MeetingDescription,
            @DocumentPath,
            @Created,
            @Modified,
            0 -- Not cancelled by default
        );

        SET @MeetingID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_Meeting_Insert created';
GO

-- =============================================
-- SP 8: Update Meeting
-- =============================================
IF OBJECT_ID('PR_Meeting_Update', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_Update;
GO

CREATE PROCEDURE PR_Meeting_Update
    @MeetingID INT,
    @MeetingDate DATETIME,
    @MeetingVenueID INT,
    @MeetingTypeID INT,
    @DepartmentID INT,
    @MeetingDescription NVARCHAR(250),
    @DocumentPath NVARCHAR(250),
    @Modified DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if meeting exists
        IF NOT EXISTS (SELECT 1 FROM MOM_Meetings WHERE MeetingID = @MeetingID)
        BEGIN
            RAISERROR('Meeting not found', 16, 1);
            RETURN;
        END

        -- Check if meeting is cancelled (cancelled meetings cannot be updated)
        IF EXISTS (SELECT 1 FROM MOM_Meetings WHERE MeetingID = @MeetingID AND IsCancelled = 1)
        BEGIN
            RAISERROR('Cannot update cancelled meeting', 16, 1);
            RETURN;
        END

        -- Validate foreign keys
        IF NOT EXISTS (SELECT 1 FROM MOM_MeetingVenue WHERE MeetingVenueID = @MeetingVenueID)
        BEGIN
            RAISERROR('Invalid meeting venue', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM MOM_MeetingType WHERE MeetingTypeID = @MeetingTypeID)
        BEGIN
            RAISERROR('Invalid meeting type', 16, 1);
            RETURN;
        END

        IF NOT EXISTS (SELECT 1 FROM MOM_Department WHERE DepartmentID = @DepartmentID)
        BEGIN
            RAISERROR('Invalid department', 16, 1);
            RETURN;
        END

        UPDATE MOM_Meetings
        SET
            MeetingDate = @MeetingDate,
            MeetingVenueID = @MeetingVenueID,
            MeetingTypeID = @MeetingTypeID,
            DepartmentID = @DepartmentID,
            MeetingDescription = @MeetingDescription,
            DocumentPath = @DocumentPath,
            Modified = @Modified
        WHERE MeetingID = @MeetingID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_Meeting_Update created';
GO

-- =============================================
-- SP 9: Cancel Meeting
-- =============================================
IF OBJECT_ID('PR_Meeting_Cancel', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_Cancel;
GO

CREATE PROCEDURE PR_Meeting_Cancel
    @MeetingID INT,
    @CancellationReason NVARCHAR(250),
    @CancellationDateTime DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if meeting exists
        IF NOT EXISTS (SELECT 1 FROM MOM_Meetings WHERE MeetingID = @MeetingID)
        BEGIN
            RAISERROR('Meeting not found', 16, 1);
            RETURN;
        END

        -- Check if meeting is already cancelled
        IF EXISTS (SELECT 1 FROM MOM_Meetings WHERE MeetingID = @MeetingID AND IsCancelled = 1)
        BEGIN
            RAISERROR('Meeting is already cancelled', 16, 1);
            RETURN;
        END

        -- Check if meeting is in the past (past meetings cannot be cancelled)
        IF EXISTS (SELECT 1 FROM MOM_Meetings WHERE MeetingID = @MeetingID AND MeetingDate < GETDATE())
        BEGIN
            RAISERROR('Cannot cancel past meeting', 16, 1);
            RETURN;
        END

        UPDATE MOM_Meetings
        SET
            IsCancelled = 1,
            CancellationReason = @CancellationReason,
            CancellationDateTime = @CancellationDateTime,
            Modified = @CancellationDateTime
        WHERE MeetingID = @MeetingID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_Meeting_Cancel created';
GO

-- =============================================
-- SP 10: Delete Meeting
-- (Only allows deletion of meetings without attendees)
-- =============================================
IF OBJECT_ID('PR_Meeting_Delete', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_Delete;
GO

CREATE PROCEDURE PR_Meeting_Delete
    @MeetingID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if meeting exists
        IF NOT EXISTS (SELECT 1 FROM MOM_Meetings WHERE MeetingID = @MeetingID)
        BEGIN
            RAISERROR('Meeting not found', 16, 1);
            RETURN;
        END

        -- Check if meeting has attendees
        IF EXISTS (SELECT 1 FROM MOM_MeetingMember WHERE MeetingID = @MeetingID)
        BEGIN
            RAISERROR('Cannot delete meeting with attendees. Remove attendees first', 16, 1);
            RETURN;
        END

        -- Delete any associated document
        DECLARE @DocumentPath NVARCHAR(250);
        SELECT @DocumentPath = DocumentPath FROM MOM_Meetings WHERE MeetingID = @MeetingID;

        DELETE FROM MOM_Meetings
        WHERE MeetingID = @MeetingID;

        -- Return document path for cleanup (if needed)
        SELECT @DocumentPath AS DocumentPath;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_Meeting_Delete created';
GO

-- =============================================
-- SP 11: Check Meeting Conflict
-- For venue/time conflict detection
-- =============================================
IF OBJECT_ID('PR_Meeting_CheckConflict', 'P') IS NOT NULL
    DROP PROCEDURE PR_Meeting_CheckConflict;
GO

CREATE PROCEDURE PR_Meeting_CheckConflict
    @MeetingVenueID INT,
    @MeetingDate DATETIME,
    @ExcludeMeetingID INT = NULL,
    @HasConflict BIT OUTPUT,
    @ConflictMeetingID INT OUTPUT,
    @ConflictMeetingDescription NVARCHAR(250) OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    -- Check for venue/time conflicts (1-hour window)
    SELECT TOP 1
        @ConflictMeetingID = m.MeetingID,
        @ConflictMeetingDescription = m.MeetingDescription
    FROM MOM_Meetings m
    WHERE m.MeetingVenueID = @MeetingVenueID
        AND m.IsCancelled = 0
        AND (
            -- Check if new meeting time conflicts with existing meeting
            -- Consider 1-hour buffer for each meeting
            (@MeetingDate BETWEEN m.MeetingDate AND DATEADD(HOUR, 1, m.MeetingDate))
            OR
            (DATEADD(HOUR, 1, @MeetingDate) BETWEEN m.MeetingDate AND DATEADD(HOUR, 1, m.MeetingDate))
        )
        AND (@ExcludeMeetingID IS NULL OR m.MeetingID != @ExcludeMeetingID);

    IF @ConflictMeetingID IS NOT NULL
        SET @HasConflict = 1;
    ELSE
        SET @HasConflict = 0;
END
GO

PRINT 'Procedure PR_Meeting_CheckConflict created';
GO

PRINT '========================================';
PRINT 'All Meeting stored procedures created!';
PRINT '========================================';
GO