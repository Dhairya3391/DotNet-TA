/*
 * Database: MOM_Database
 * Purpose: Stored Procedures for MOM_MeetingMember table (Attendance Tracking)
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

USE MOM_Database;
GO

-- =============================================
-- SP 1: Select Meeting Members by Meeting ID
-- Gets all attendees for a specific meeting
-- =============================================
IF OBJECT_ID('PR_MeetingMember_SelectByMeeting', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingMember_SelectByMeeting;
GO

CREATE PROCEDURE PR_MeetingMember_SelectByMeeting
    @MeetingID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        mm.MeetingMemberID,
        mm.MeetingID,
        mm.StaffID,
        mm.IsPresent,
        mm.Remarks,
        mm.Created,
        mm.Modified,
        s.StaffName,
        s.MobileNo,
        s.EmailAddress,
        d.DepartmentName
    FROM MOM_MeetingMember mm
    INNER JOIN MOM_Staff s ON mm.StaffID = s.StaffID
    INNER JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
    WHERE mm.MeetingID = @MeetingID
    ORDER BY s.StaffName;
END
GO

PRINT 'Procedure PR_MeetingMember_SelectByMeeting created';
GO

-- =============================================
-- SP 2: Select Meeting Members by Staff ID
-- Gets all meetings for a specific staff member
-- =============================================
IF OBJECT_ID('PR_MeetingMember_SelectByStaff', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingMember_SelectByStaff;
GO

CREATE PROCEDURE PR_MeetingMember_SelectByStaff
    @StaffID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        mm.MeetingMemberID,
        mm.MeetingID,
        mm.StaffID,
        mm.IsPresent,
        mm.Remarks,
        mm.Created,
        mm.Modified,
        m.MeetingDate,
        m.MeetingDescription,
        mv.MeetingVenueName,
        mt.MeetingTypeName,
        d.DepartmentName,
        CASE
            WHEN m.IsCancelled = 1 THEN 'Cancelled'
            WHEN m.MeetingDate < GETDATE() THEN 'Completed'
            ELSE 'Upcoming'
        END AS MeetingStatus,
        FORMAT(m.MeetingDate, 'yyyy-MM-dd HH:mm') AS MeetingDateTime
    FROM MOM_MeetingMember mm
    INNER JOIN MOM_Meetings m ON mm.MeetingID = m.MeetingID
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    WHERE mm.StaffID = @StaffID
    ORDER BY m.MeetingDate DESC;
END
GO

PRINT 'Procedure PR_MeetingMember_SelectByStaff created';
GO

-- =============================================
-- SP 3: Insert Meeting Member
-- Add a staff member to a meeting
-- =============================================
IF OBJECT_ID('PR_MeetingMember_Insert', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingMember_Insert;
GO

CREATE PROCEDURE PR_MeetingMember_Insert
    @MeetingID INT,
    @StaffID INT,
    @IsPresent BIT,
    @Remarks NVARCHAR(250),
    @Created DATETIME,
    @Modified DATETIME,
    @MeetingMemberID INT OUTPUT
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

        -- Check if staff exists
        IF NOT EXISTS (SELECT 1 FROM MOM_Staff WHERE StaffID = @StaffID)
        BEGIN
            RAISERROR('Staff member not found', 16, 1);
            RETURN;
        END

        -- Check if staff member is already added to this meeting
        IF EXISTS (
            SELECT 1 FROM MOM_MeetingMember
            WHERE MeetingID = @MeetingID AND StaffID = @StaffID
        )
        BEGIN
            RAISERROR('Staff member is already added to this meeting', 16, 1);
            RETURN;
        END

        INSERT INTO MOM_MeetingMember
        (
            MeetingID,
            StaffID,
            IsPresent,
            Remarks,
            Created,
            Modified
        )
        VALUES
        (
            @MeetingID,
            @StaffID,
            @IsPresent,
            @Remarks,
            @Created,
            @Modified
        );

        SET @MeetingMemberID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_MeetingMember_Insert created';
GO

-- =============================================
-- SP 4: Update Attendance Status
-- Mark staff member as present/absent
-- =============================================
IF OBJECT_ID('PR_MeetingMember_UpdateAttendance', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingMember_UpdateAttendance;
GO

CREATE PROCEDURE PR_MeetingMember_UpdateAttendance
    @MeetingMemberID INT,
    @IsPresent BIT,
    @Remarks NVARCHAR(250),
    @Modified DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if meeting member exists
        IF NOT EXISTS (SELECT 1 FROM MOM_MeetingMember WHERE MeetingMemberID = @MeetingMemberID)
        BEGIN
            RAISERROR('Meeting member not found', 16, 1);
            RETURN;
        END

        -- Check if meeting is completed (attendance can only be marked for completed meetings)
        DECLARE @MeetingDate DATETIME;
        SELECT @MeetingDate = m.MeetingDate
        FROM MOM_MeetingMember mm
        INNER JOIN MOM_Meetings m ON mm.MeetingID = m.MeetingID
        WHERE mm.MeetingMemberID = @MeetingMemberID;

        IF @MeetingDate > GETDATE()
        BEGIN
            RAISERROR('Cannot mark attendance for future meetings', 16, 1);
            RETURN;
        END

        UPDATE MOM_MeetingMember
        SET
            IsPresent = @IsPresent,
            Remarks = @Remarks,
            Modified = @Modified
        WHERE MeetingMemberID = @MeetingMemberID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_MeetingMember_UpdateAttendance created';
GO

-- =============================================
-- SP 5: Delete Meeting Member
-- Remove staff member from a meeting
-- =============================================
IF OBJECT_ID('PR_MeetingMember_Delete', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingMember_Delete;
GO

CREATE PROCEDURE PR_MeetingMember_Delete
    @MeetingMemberID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if meeting member exists
        IF NOT EXISTS (SELECT 1 FROM MOM_MeetingMember WHERE MeetingMemberID = @MeetingMemberID)
        BEGIN
            RAISERROR('Meeting member not found', 16, 1);
            RETURN;
        END

        -- Check if meeting is in progress or completed (cannot remove attendees from past meetings)
        DECLARE @MeetingDate DATETIME;
        SELECT @MeetingDate = m.MeetingDate
        FROM MOM_MeetingMember mm
        INNER JOIN MOM_Meetings m ON mm.MeetingID = m.MeetingID
        WHERE mm.MeetingMemberID = @MeetingMemberID;

        IF @MeetingDate <= GETDATE()
        BEGIN
            RAISERROR('Cannot remove attendees from past or current meetings', 16, 1);
            RETURN;
        END

        DELETE FROM MOM_MeetingMember
        WHERE MeetingMemberID = @MeetingMemberID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_MeetingMember_Delete created';
GO

-- =============================================
-- SP 6: Get Attendance Summary for Meeting
-- Returns count of present, absent, and total attendees
-- =============================================
IF OBJECT_ID('PR_MeetingMember_GetAttendanceSummary', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingMember_GetAttendanceSummary;
GO

CREATE PROCEDURE PR_MeetingMember_GetAttendanceSummary
    @MeetingID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        @MeetingID AS MeetingID,
        COUNT(*) AS TotalInvited,
        SUM(CASE WHEN IsPresent = 1 THEN 1 ELSE 0 END) AS Present,
        SUM(CASE WHEN IsPresent = 0 THEN 1 ELSE 0 END) AS Absent,
        CAST(
            CASE WHEN COUNT(*) > 0
            THEN CAST(SUM(CASE WHEN IsPresent = 1 THEN 1 ELSE 0 END) AS FLOAT) / COUNT(*) * 100
            ELSE 0 END
        AS DECIMAL(5,2)) AS AttendancePercentage
    FROM MOM_MeetingMember
    WHERE MeetingID = @MeetingID;
END
GO

PRINT 'Procedure PR_MeetingMember_GetAttendanceSummary created';
GO

-- =============================================
-- SP 7: Get Staff Participation Statistics
-- Returns meeting participation stats for a staff member
-- =============================================
IF OBJECT_ID('PR_MeetingMember_GetStaffParticipation', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingMember_GetStaffParticipation;
GO

CREATE PROCEDURE PR_MeetingMember_GetStaffParticipation
    @StaffID INT,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        @StaffID AS StaffID,
        s.StaffName,
        COUNT(*) AS TotalMeetings,
        SUM(CASE WHEN mm.IsPresent = 1 THEN 1 ELSE 0 END) AS AttendedMeetings,
        SUM(CASE WHEN mm.IsPresent = 0 THEN 1 ELSE 0 END) AS MissedMeetings,
        CAST(
            CASE WHEN COUNT(*) > 0
            THEN CAST(SUM(CASE WHEN mm.IsPresent = 1 THEN 1 ELSE 0 END) AS FLOAT) / COUNT(*) * 100
            ELSE 0 END
        AS DECIMAL(5,2)) AS AttendancePercentage
    FROM MOM_MeetingMember mm
    INNER JOIN MOM_Staff s ON mm.StaffID = s.StaffID
    INNER JOIN MOM_Meetings m ON mm.MeetingID = m.MeetingID
    WHERE mm.StaffID = @StaffID
        AND (@StartDate IS NULL OR m.MeetingDate >= @StartDate)
        AND (@EndDate IS NULL OR m.MeetingDate <= @EndDate)
        AND m.IsCancelled = 0
    GROUP BY s.StaffName;
END
GO

PRINT 'Procedure PR_MeetingMember_GetStaffParticipation created';
GO

-- =============================================
-- SP 8: Get Top Participants (Staff with most meetings)
-- For dashboard statistics
-- =============================================
IF OBJECT_ID('PR_MeetingMember_GetTopParticipants', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingMember_GetTopParticipants;
GO

CREATE PROCEDURE PR_MeetingMember_GetTopParticipants
    @TopCount INT = 5,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@TopCount)
        s.StaffID,
        s.StaffName,
        d.DepartmentName,
        COUNT(*) AS TotalMeetings,
        SUM(CASE WHEN mm.IsPresent = 1 THEN 1 ELSE 0 END) AS AttendedMeetings,
        CAST(
            CASE WHEN COUNT(*) > 0
            THEN CAST(SUM(CASE WHEN mm.IsPresent = 1 THEN 1 ELSE 0 END) AS FLOAT) / COUNT(*) * 100
            ELSE 0 END
        AS DECIMAL(5,2)) AS AttendancePercentage
    FROM MOM_MeetingMember mm
    INNER JOIN MOM_Staff s ON mm.StaffID = s.StaffID
    INNER JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
    INNER JOIN MOM_Meetings m ON mm.MeetingID = m.MeetingID
    WHERE (@StartDate IS NULL OR m.MeetingDate >= @StartDate)
        AND (@EndDate IS NULL OR m.MeetingDate <= @EndDate)
        AND m.IsCancelled = 0
    GROUP BY s.StaffID, s.StaffName, d.DepartmentName
    ORDER BY TotalMeetings DESC, AttendancePercentage DESC;
END
GO

PRINT 'Procedure PR_MeetingMember_GetTopParticipants created';
GO

-- =============================================
-- SP 9: Bulk Insert Meeting Members
-- Add multiple staff members to a meeting at once
-- =============================================
IF OBJECT_ID('PR_MeetingMember_BulkInsert', 'P') IS NOT NULL
    DROP PROCEDURE PR_MeetingMember_BulkInsert;
GO

CREATE PROCEDURE PR_MeetingMember_BulkInsert
    @MeetingID INT,
    @StaffIDs NVARCHAR(MAX), -- Comma-separated staff IDs
    @Created DATETIME,
    @Modified DATETIME,
    @SuccessCount INT OUTPUT,
    @ErrorCount INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @Success INT = 0;
    DECLARE @Error INT = 0;
    DECLARE @CurrentStaffID INT;
    DECLARE @Pos INT, @NextPos INT;

    -- Check if meeting exists
    IF NOT EXISTS (SELECT 1 FROM MOM_Meetings WHERE MeetingID = @MeetingID)
    BEGIN
        RAISERROR('Meeting not found', 16, 1);
        RETURN;
    END

    -- Process comma-separated staff IDs
    SET @StaffIDs = @StaffIDs + ',';
    SET @Pos = CHARINDEX(',', @StaffIDs, 1);

    IF REPLACE(@StaffIDs, ',', '') <> ''
    BEGIN
        WHILE @Pos > 0
        BEGIN
            SET @CurrentStaffID = LTRIM(RTRIM(SUBSTRING(@StaffIDs, 1, @Pos - 1)));

            IF ISNUMERIC(@CurrentStaffID) = 1
            BEGIN
                BEGIN TRY
                    -- Check if staff exists
                    IF EXISTS (SELECT 1 FROM MOM_Staff WHERE StaffID = @CurrentStaffID)
                    BEGIN
                        -- Check if already added
                        IF NOT EXISTS (
                            SELECT 1 FROM MOM_MeetingMember
                            WHERE MeetingID = @MeetingID AND StaffID = @CurrentStaffID
                        )
                        BEGIN
                            INSERT INTO MOM_MeetingMember
                            (
                                MeetingID,
                                StaffID,
                                IsPresent,
                                Created,
                                Modified
                            )
                            VALUES
                            (
                                @MeetingID,
                                @CurrentStaffID,
                                0, -- Default to not present
                                @Created,
                                @Modified
                            );

                            SET @Success = @Success + 1;
                        END
                    END
                END TRY
                BEGIN CATCH
                    SET @Error = @Error + 1;
                END CATCH
            END

            SET @StaffIDs = SUBSTRING(@StaffIDs, @Pos + 1, LEN(@StaffIDs));
            SET @Pos = CHARINDEX(',', @StaffIDs, 1);
        END
    END

    SET @SuccessCount = @Success;
    SET @ErrorCount = @Error;
END
GO

PRINT 'Procedure PR_MeetingMember_BulkInsert created';
GO

PRINT '========================================';
PRINT 'All MeetingMember stored procedures created!';
PRINT '========================================';
GO