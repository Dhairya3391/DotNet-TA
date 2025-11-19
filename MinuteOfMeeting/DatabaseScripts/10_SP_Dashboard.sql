/*
 * Database: MOM_Database
 * Purpose: Dashboard Statistics and Chart Procedures
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

USE MOM_Database;
GO

-- =============================================
-- Dashboard SP 1: Get Overall Statistics
-- Returns total counts for dashboard summary
-- =============================================
IF OBJECT_ID('PR_Dashboard_GetStatistics', 'P') IS NOT NULL
    DROP PROCEDURE PR_Dashboard_GetStatistics;
GO

CREATE PROCEDURE PR_Dashboard_GetStatistics
AS
BEGIN
    SET NOCOUNT ON;

    -- Total meetings
    DECLARE @TotalMeetings INT;
    SELECT @TotalMeetings = COUNT(*) FROM MOM_Meetings;

    -- Upcoming meetings
    DECLARE @UpcomingMeetings INT;
    SELECT @UpcomingMeetings = COUNT(*) FROM MOM_Meetings
    WHERE MeetingDate >= GETDATE() AND IsCancelled = 0;

    -- Completed meetings
    DECLARE @CompletedMeetings INT;
    SELECT @CompletedMeetings = COUNT(*) FROM MOM_Meetings
    WHERE MeetingDate < GETDATE() AND IsCancelled = 0;

    -- Cancelled meetings
    DECLARE @CancelledMeetings INT;
    SELECT @CancelledMeetings = COUNT(*) FROM MOM_Meetings
    WHERE IsCancelled = 1;

    -- Total staff
    DECLARE @TotalStaff INT;
    SELECT @TotalStaff = COUNT(*) FROM MOM_Staff;

    -- Total departments
    DECLARE @TotalDepartments INT;
    SELECT @TotalDepartments = COUNT(*) FROM MOM_Department;

    -- Total venues
    DECLARE @TotalVenues INT;
    SELECT @TotalVenues = COUNT(*) FROM MOM_MeetingVenue;

    -- Total active users
    DECLARE @TotalUsers INT;
    SELECT @TotalUsers = COUNT(*) FROM MOM_User WHERE IsActive = 1;

    -- Today's meetings
    DECLARE @TodaysMeetings INT;
    SELECT @TodaysMeetings = COUNT(*) FROM MOM_Meetings
    WHERE CAST(MeetingDate AS DATE) = CAST(GETDATE() AS DATE) AND IsCancelled = 0;

    -- This week's meetings
    DECLARE @ThisWeekMeetings INT;
    SELECT @ThisWeekMeetings = COUNT(*) FROM MOM_Meetings
    WHERE MeetingDate >= DATEADD(WEEK, DATEDIFF(DAY, 0, GETDATE()), 0)
      AND MeetingDate < DATEADD(WEEK, DATEDIFF(DAY, 0, GETDATE()) + 7, 0)
      AND IsCancelled = 0;

    -- This month's meetings
    DECLARE @ThisMonthMeetings INT;
    SELECT @ThisMonthMeetings = COUNT(*) FROM MOM_Meetings
    WHERE MONTH(MeetingDate) = MONTH(GETDATE())
      AND YEAR(MeetingDate) = YEAR(GETDATE())
      AND IsCancelled = 0;

    SELECT
        @TotalMeetings AS TotalMeetings,
        @UpcomingMeetings AS UpcomingMeetings,
        @CompletedMeetings AS CompletedMeetings,
        @CancelledMeetings AS CancelledMeetings,
        @TotalStaff AS TotalStaff,
        @TotalDepartments AS TotalDepartments,
        @TotalVenues AS TotalVenues,
        @TotalUsers AS TotalUsers,
        @TodaysMeetings AS TodaysMeetings,
        @ThisWeekMeetings AS ThisWeekMeetings,
        @ThisMonthMeetings AS ThisMonthMeetings;
END
GO

PRINT 'Procedure PR_Dashboard_GetStatistics created';
GO

-- =============================================
-- Dashboard SP 2: Get Upcoming Meetings
-- Next N upcoming meetings
-- =============================================
IF OBJECT_ID('PR_Dashboard_GetUpcomingMeetings', 'P') IS NOT NULL
    DROP PROCEDURE PR_Dashboard_GetUpcomingMeetings;
GO

CREATE PROCEDURE PR_Dashboard_GetUpcomingMeetings
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
        DATEDIFF(HOUR, GETDATE(), m.MeetingDate) AS HoursUntilMeeting,
        ISNULL(mm.AttendeeCount, 0) AS AttendeeCount,
        CASE
            WHEN DATEDIFF(HOUR, GETDATE(), m.MeetingDate) <= 1 THEN 'Very Soon'
            WHEN DATEDIFF(HOUR, GETDATE(), m.MeetingDate) <= 24 THEN 'Today'
            WHEN DATEDIFF(DAY, GETDATE(), m.MeetingDate) <= 7 THEN 'This Week'
            ELSE 'Later'
        END AS UrgencyLevel
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

PRINT 'Procedure PR_Dashboard_GetUpcomingMeetings created';
GO

-- =============================================
-- Dashboard SP 3: Get Recent Meetings
-- Last N completed or cancelled meetings
-- =============================================
IF OBJECT_ID('PR_Dashboard_GetRecentMeetings', 'P') IS NOT NULL
    DROP PROCEDURE PR_Dashboard_GetRecentMeetings;
GO

CREATE PROCEDURE PR_Dashboard_GetRecentMeetings
    @TopCount INT = 10
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@TopCount)
        m.MeetingID,
        m.MeetingDate,
        m.MeetingDescription,
        m.IsCancelled,
        m.CancellationDateTime,
        m.CancellationReason,
        mv.MeetingVenueName,
        mt.MeetingTypeName,
        d.DepartmentName,
        FORMAT(m.MeetingDate, 'yyyy-MM-dd HH:mm') AS MeetingDateTime,
        DATEDIFF(DAY, m.MeetingDate, GETDATE()) AS DaysAgo,
        ISNULL(mm.AttendeeCount, 0) AS AttendeeCount,
        ISNULL(mm.PresentCount, 0) AS PresentCount,
        CASE
            WHEN m.IsCancelled = 1 THEN 'Cancelled'
            WHEN m.MeetingDate >= GETDATE() THEN 'Upcoming'
            ELSE 'Completed'
        END AS Status
    FROM MOM_Meetings m
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    LEFT JOIN (
        SELECT MeetingID, COUNT(*) AS AttendeeCount,
               SUM(CASE WHEN IsPresent = 1 THEN 1 ELSE 0 END) AS PresentCount
        FROM MOM_MeetingMember
        GROUP BY MeetingID
    ) mm ON m.MeetingID = mm.MeetingID
    ORDER BY
        CASE WHEN m.IsCancelled = 1 THEN m.CancellationDateTime ELSE m.MeetingDate END DESC;
END
GO

PRINT 'Procedure PR_Dashboard_GetRecentMeetings created';
GO

-- =============================================
-- Dashboard SP 4: Get Meetings by Type
-- For Pie/Bar charts
-- =============================================
IF OBJECT_ID('PR_Dashboard_GetMeetingsByType', 'P') IS NOT NULL
    DROP PROCEDURE PR_Dashboard_GetMeetingsByType;
GO

CREATE PROCEDURE PR_Dashboard_GetMeetingsByType
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        mt.MeetingTypeID,
        mt.MeetingTypeName,
        COUNT(m.MeetingID) AS MeetingCount,
        FORMAT(
            CAST(COUNT(m.MeetingID) AS FLOAT) * 100 /
                NULLIF((SELECT COUNT(*) FROM MOM_Meetings
                        WHERE (@StartDate IS NULL OR MeetingDate >= @StartDate)
                          AND (@EndDate IS NULL OR MeetingDate <= @EndDate)
                          AND IsCancelled = 0), 0),
            'N2'
        ) AS Percentage
    FROM MOM_MeetingType mt
    LEFT JOIN MOM_Meetings m ON mt.MeetingTypeID = m.MeetingTypeID
        AND (@StartDate IS NULL OR m.MeetingDate >= @StartDate)
        AND (@EndDate IS NULL OR m.MeetingDate <= @EndDate)
        AND m.IsCancelled = 0
    GROUP BY mt.MeetingTypeID, mt.MeetingTypeName
    ORDER BY MeetingCount DESC;
END
GO

PRINT 'Procedure PR_Dashboard_GetMeetingsByType created';
GO

-- =============================================
-- Dashboard SP 5: Get Meetings by Department
-- For Pie/Bar charts
-- =============================================
IF OBJECT_ID('PR_Dashboard_GetMeetingsByDepartment', 'P') IS NOT NULL
    DROP PROCEDURE PR_Dashboard_GetMeetingsByDepartment;
GO

CREATE PROCEDURE PR_Dashboard_GetMeetingsByDepartment
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        d.DepartmentID,
        d.DepartmentName,
        COUNT(m.MeetingID) AS MeetingCount,
        FORMAT(
            CAST(COUNT(m.MeetingID) AS FLOAT) * 100 /
                NULLIF((SELECT COUNT(*) FROM MOM_Meetings
                        WHERE (@StartDate IS NULL OR MeetingDate >= @StartDate)
                          AND (@EndDate IS NULL OR MeetingDate <= @EndDate)
                          AND IsCancelled = 0), 0),
            'N2'
        ) AS Percentage
    FROM MOM_Department d
    LEFT JOIN MOM_Meetings m ON d.DepartmentID = m.DepartmentID
        AND (@StartDate IS NULL OR m.MeetingDate >= @StartDate)
        AND (@EndDate IS NULL OR m.MeetingDate <= @EndDate)
        AND m.IsCancelled = 0
    GROUP BY d.DepartmentID, d.DepartmentName
    ORDER BY MeetingCount DESC;
END
GO

PRINT 'Procedure PR_Dashboard_GetMeetingsByDepartment created';
GO

-- =============================================
-- Dashboard SP 6: Get Monthly Meeting Trend
-- For line chart showing meeting trends over months
-- =============================================
IF OBJECT_ID('PR_Dashboard_GetMonthlyMeetingTrend', 'P') IS NOT NULL
    DROP PROCEDURE PR_Dashboard_GetMonthlyMeetingTrend;
GO

CREATE PROCEDURE PR_Dashboard_GetMonthlyMeetingTrend
    @MonthsBack INT = 12
AS
BEGIN
    SET NOCOUNT ON;

    ;WITH Months AS (
        SELECT
            DATEADD(MONTH, -n, 0) AS MonthStart,
            EOMONTH(DATEADD(MONTH, -n, 0)) AS MonthEnd
        FROM (SELECT TOP (@MonthsBack) ROW_NUMBER() OVER (ORDER BY (SELECT NULL)) - 1 AS n
              FROM sys.objects) AS Numbers
    )
    SELECT
        FORMAT(m.MonthStart, 'yyyy-MM') AS YearMonth,
        FORMAT(m.MonthStart, 'MMM yyyy') AS MonthLabel,
        ISNULL(COUNT(meet.MeetingID), 0) AS MeetingCount,
        ISNULL(SUM(CASE WHEN meet.IsCancelled = 1 THEN 1 ELSE 0 END), 0) AS CancelledCount,
        ISNULL(SUM(CASE WHEN meet.IsCancelled = 0 THEN 1 ELSE 0 END), 0) AS CompletedCount
    FROM Months m
    LEFT JOIN MOM_Meetings meet ON meet.MeetingDate >= m.MonthStart
        AND meet.MeetingDate <= m.MonthEnd
    GROUP BY m.MonthStart
    ORDER BY m.MonthStart;
END
GO

PRINT 'Procedure PR_Dashboard_GetMonthlyMeetingTrend created';
GO

-- =============================================
-- Dashboard SP 7: Get Most Active Departments
-- Top departments by meeting count
-- =============================================
IF OBJECT_ID('PR_Dashboard_GetMostActiveDepartments', 'P') IS NOT NULL
    DROP PROCEDURE PR_Dashboard_GetMostActiveDepartments;
GO

CREATE PROCEDURE PR_Dashboard_GetMostActiveDepartments
    @TopCount INT = 5,
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT TOP (@TopCount)
        d.DepartmentID,
        d.DepartmentName,
        COUNT(m.MeetingID) AS MeetingCount,
        COUNT(DISTINCT mm.StaffID) AS UniqueParticipants,
        ISNULL(AVG(
            CASE WHEN mm.MeetingID IS NOT NULL THEN
                CAST(SUM(CASE WHEN mm.IsPresent = 1 THEN 1 ELSE 0 END) AS FLOAT) / COUNT(*) * 100
            ELSE NULL END
        ), 0) AS AvgAttendancePercentage,
        -- Most recent meeting in this department
        (SELECT MAX(MeetingDate) FROM MOM_Meetings
         WHERE DepartmentID = d.DepartmentID AND IsCancelled = 0) AS LastMeetingDate
    FROM MOM_Department d
    LEFT JOIN MOM_Meetings m ON d.DepartmentID = m.DepartmentID
        AND (@StartDate IS NULL OR m.MeetingDate >= @StartDate)
        AND (@EndDate IS NULL OR m.MeetingDate <= @EndDate)
        AND m.IsCancelled = 0
    LEFT JOIN MOM_MeetingMember mm ON m.MeetingID = mm.MeetingID
    GROUP BY d.DepartmentID, d.DepartmentName
    HAVING COUNT(m.MeetingID) > 0
    ORDER BY MeetingCount DESC;
END
GO

PRINT 'Procedure PR_Dashboard_GetMostActiveDepartments created';
GO

-- =============================================
-- Dashboard SP 8: Get Staff Participation Stats
-- Top participants by meeting attendance
-- =============================================
IF OBJECT_ID('PR_Dashboard_GetStaffParticipation', 'P') IS NOT NULL
    DROP PROCEDURE PR_Dashboard_GetStaffParticipation;
GO

CREATE PROCEDURE PR_Dashboard_GetStaffParticipation
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
        COUNT(mm.MeetingMemberID) AS TotalInvitations,
        SUM(CASE WHEN mm.IsPresent = 1 THEN 1 ELSE 0 END) AS Attended,
        SUM(CASE WHEN mm.IsPresent = 0 THEN 1 ELSE 0 END) AS Missed,
        CAST(
            CASE WHEN COUNT(mm.MeetingMemberID) > 0
            THEN CAST(SUM(CASE WHEN mm.IsPresent = 1 THEN 1 ELSE 0 END) AS FLOAT) /
                 COUNT(mm.MeetingMemberID) * 100
            ELSE 0 END
        AS DECIMAL(5,2)) AS AttendancePercentage
    FROM MOM_Staff s
    INNER JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
    LEFT JOIN MOM_MeetingMember mm ON s.StaffID = mm.StaffID
    LEFT JOIN MOM_Meetings m ON mm.MeetingID = m.MeetingID
        AND (@StartDate IS NULL OR m.MeetingDate >= @StartDate)
        AND (@EndDate IS NULL OR m.MeetingDate <= @EndDate)
        AND m.IsCancelled = 0
    WHERE COUNT(mm.MeetingMemberID) > 0
    GROUP BY s.StaffID, s.StaffName, d.DepartmentName
    ORDER BY Attended DESC, AttendancePercentage DESC;
END
GO

PRINT 'Procedure PR_Dashboard_GetStaffParticipation created';
GO

-- =============================================
-- Dashboard SP 9: Get Venue Utilization
-- Most used meeting venues
-- =============================================
IF OBJECT_ID('PR_Dashboard_GetVenueUtilization', 'P') IS NOT NULL
    DROP PROCEDURE PR_Dashboard_GetVenueUtilization;
GO

CREATE PROCEDURE PR_Dashboard_GetVenueUtilization
    @StartDate DATETIME = NULL,
    @EndDate DATETIME = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        mv.MeetingVenueID,
        mv.MeetingVenueName,
        COUNT(m.MeetingID) AS TotalMeetings,
        COUNT(DISTINCT CAST(m.MeetingDate AS DATE)) AS DaysUsed,
        COUNT(DISTINCT d.DepartmentID) AS DepartmentsUsed,
        COUNT(DISTINCT mt.MeetingTypeID) AS MeetingTypesUsed,
        -- Most recent meeting at this venue
        (SELECT MAX(MeetingDate) FROM MOM_Meetings
         WHERE MeetingVenueID = mv.MeetingVenueID AND IsCancelled = 0) AS LastMeetingDate,
        -- Utilization rate ( meetings per day used )
        CASE WHEN COUNT(DISTINCT CAST(m.MeetingDate AS DATE)) > 0
             THEN CAST(COUNT(m.MeetingID) AS FLOAT) / COUNT(DISTINCT CAST(m.MeetingDate AS DATE))
             ELSE 0 END AS MeetingsPerDay
    FROM MOM_MeetingVenue mv
    LEFT JOIN MOM_Meetings m ON mv.MeetingVenueID = m.MeetingVenueID
        AND (@StartDate IS NULL OR m.MeetingDate >= @StartDate)
        AND (@EndDate IS NULL OR m.MeetingDate <= @EndDate)
        AND m.IsCancelled = 0
    LEFT JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    LEFT JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    GROUP BY mv.MeetingVenueID, mv.MeetingVenueName
    HAVING COUNT(m.MeetingID) > 0
    ORDER BY TotalMeetings DESC;
END
GO

PRINT 'Procedure PR_Dashboard_GetVenueUtilization created';
GO

PRINT '========================================';
PRINT 'All Dashboard stored procedures created!';
PRINT '========================================';
GO