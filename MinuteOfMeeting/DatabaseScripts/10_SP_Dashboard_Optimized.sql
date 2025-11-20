/*
 * Database: MOM_Database
 * Purpose: OPTIMIZED Dashboard Procedure - Returns all dashboard data in ONE call
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 *
 * PERFORMANCE OPTIMIZATION:
 * Instead of 12 separate database calls, this procedure returns all dashboard data
 * in a single call using multiple result sets. This dramatically improves page load time.
 */

USE MOM_Database;
GO

-- =============================================
-- OPTIMIZED Dashboard Procedure
-- Returns all dashboard data in a single call using multiple result sets
-- This reduces 12 database round-trips to just 1
-- =============================================
IF OBJECT_ID('PR_Dashboard_GetAllData', 'P') IS NOT NULL
    DROP PROCEDURE PR_Dashboard_GetAllData;
GO

CREATE PROCEDURE PR_Dashboard_GetAllData
AS
BEGIN
    SET NOCOUNT ON;

    -- Result Set 1: Basic Statistics (single row with all counts)
    SELECT
        (SELECT COUNT(*) FROM MOM_Meetings) AS TotalMeetings,
        (SELECT COUNT(*) FROM MOM_Meetings WHERE MeetingDate >= GETDATE() AND IsCancelled = 0) AS UpcomingMeetings,
        (SELECT COUNT(*) FROM MOM_Meetings WHERE MeetingDate < GETDATE() AND IsCancelled = 0) AS CompletedMeetings,
        (SELECT COUNT(*) FROM MOM_Meetings WHERE IsCancelled = 1) AS CancelledMeetings;

    -- Result Set 2: Recent Meetings (last 10)
    SELECT TOP 10
        m.MeetingID,
        m.MeetingDate,
        ISNULL(m.MeetingDescription, 'No description') AS MeetingDescription,
        mt.MeetingTypeName,
        mv.MeetingVenueName,
        d.DepartmentName,
        m.IsCancelled,
        (SELECT COUNT(*) FROM MOM_MeetingMember mm WHERE mm.MeetingID = m.MeetingID) AS AttendeeCount
    FROM MOM_Meetings m
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    ORDER BY m.MeetingDate DESC;

    -- Result Set 3: Upcoming Meetings (next 10)
    SELECT TOP 10
        m.MeetingID,
        m.MeetingDate,
        ISNULL(m.MeetingDescription, 'No description') AS MeetingDescription,
        mt.MeetingTypeName,
        mv.MeetingVenueName,
        d.DepartmentName,
        (SELECT COUNT(*) FROM MOM_MeetingMember mm WHERE mm.MeetingID = m.MeetingID) AS AttendeeCount
    FROM MOM_Meetings m
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    WHERE m.MeetingDate >= GETDATE() AND m.IsCancelled = 0
    ORDER BY m.MeetingDate ASC;

    -- Result Set 4: Today's Meetings
    SELECT
        m.MeetingID,
        m.MeetingDate,
        ISNULL(m.MeetingDescription, 'No description') AS MeetingDescription,
        mt.MeetingTypeName,
        mv.MeetingVenueName,
        d.DepartmentName,
        (SELECT COUNT(*) FROM MOM_MeetingMember mm WHERE mm.MeetingID = m.MeetingID) AS AttendeeCount
    FROM MOM_Meetings m
    INNER JOIN MOM_MeetingType mt ON m.MeetingTypeID = mt.MeetingTypeID
    INNER JOIN MOM_MeetingVenue mv ON m.MeetingVenueID = mv.MeetingVenueID
    INNER JOIN MOM_Department d ON m.DepartmentID = d.DepartmentID
    WHERE CAST(m.MeetingDate AS DATE) = CAST(GETDATE() AS DATE) AND m.IsCancelled = 0
    ORDER BY m.MeetingDate ASC;

    -- Result Set 5: Meetings by Type (for pie/doughnut chart)
    SELECT
        mt.MeetingTypeName,
        COUNT(m.MeetingID) AS Count
    FROM MOM_MeetingType mt
    LEFT JOIN MOM_Meetings m ON mt.MeetingTypeID = m.MeetingTypeID AND m.IsCancelled = 0
    GROUP BY mt.MeetingTypeName
    HAVING COUNT(m.MeetingID) > 0
    ORDER BY Count DESC;

    -- Result Set 6: Meetings by Department (for bar chart)
    SELECT
        d.DepartmentName,
        COUNT(m.MeetingID) AS Count
    FROM MOM_Department d
    LEFT JOIN MOM_Meetings m ON d.DepartmentID = m.DepartmentID AND m.IsCancelled = 0
    GROUP BY d.DepartmentName
    HAVING COUNT(m.MeetingID) > 0
    ORDER BY Count DESC;

    -- Result Set 7: Monthly Meeting Trend (last 12 months for line chart)
    SELECT
        FORMAT(m.MeetingDate, 'MMM yyyy') AS Month,
        COUNT(m.MeetingID) AS Count
    FROM MOM_Meetings m
    WHERE m.MeetingDate >= DATEADD(MONTH, -12, GETDATE()) AND m.IsCancelled = 0
    GROUP BY FORMAT(m.MeetingDate, 'yyyy-MM'), FORMAT(m.MeetingDate, 'MMM yyyy')
    ORDER BY FORMAT(m.MeetingDate, 'yyyy-MM');

    -- Result Set 8: Most Active Departments (top 5)
    SELECT TOP 5
        d.DepartmentName,
        COUNT(m.MeetingID) AS MeetingCount
    FROM MOM_Department d
    INNER JOIN MOM_Meetings m ON d.DepartmentID = m.DepartmentID AND m.IsCancelled = 0
    GROUP BY d.DepartmentName
    ORDER BY MeetingCount DESC;

    -- Result Set 9: Staff Participation (top 5 most active staff)
    SELECT TOP 5
        s.StaffName,
        d.DepartmentName,
        COUNT(mm.MeetingMemberID) AS MeetingCount,
        SUM(CASE WHEN mm.IsPresent = 1 THEN 1 ELSE 0 END) AS AttendedCount,
        CASE
            WHEN COUNT(mm.MeetingMemberID) > 0 THEN
                CAST(SUM(CASE WHEN mm.IsPresent = 1 THEN 1 ELSE 0 END) AS DECIMAL(5,2)) / COUNT(mm.MeetingMemberID) * 100
            ELSE 0
        END AS AttendanceRate
    FROM MOM_Staff s
    INNER JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
    LEFT JOIN MOM_MeetingMember mm ON s.StaffID = mm.StaffID
    WHERE mm.MeetingMemberID IS NOT NULL
    GROUP BY s.StaffName, d.DepartmentName
    ORDER BY MeetingCount DESC;

END
GO

PRINT 'Optimized Procedure PR_Dashboard_GetAllData created successfully';
PRINT 'This procedure returns 9 result sets in a single database call';
PRINT 'Result sets: Stats, Recent, Upcoming, Today, ByType, ByDept, MonthlyTrend, ActiveDepts, StaffParticipation';
GO

/*
 * USAGE EXAMPLE:
 *
 * EXEC PR_Dashboard_GetAllData;
 *
 * This will return 9 result sets:
 * 1. Basic statistics (1 row): TotalMeetings, UpcomingMeetings, CompletedMeetings, CancelledMeetings
 * 2. Recent meetings (up to 10 rows)
 * 3. Upcoming meetings (up to 10 rows)
 * 4. Today's meetings (variable rows)
 * 5. Meetings by type (for charts)
 * 6. Meetings by department (for charts)
 * 7. Monthly meeting trend (12 months)
 * 8. Most active departments (top 5)
 * 9. Staff participation (top 5)
 *
 * PERFORMANCE BENEFIT:
 * Before: 12 separate queries = 12 database round-trips
 * After: 1 query with multiple result sets = 1 database round-trip
 * Expected speedup: 5-10x faster initial page load
 */
