-- =====================================================
-- Lab 11: Stored Procedures for AddressBook Database
-- =====================================================
-- This script creates all stored procedures required
-- for Lab 11: SelectAll and SelectByPK operations
-- =====================================================

USE AddressBook;
GO

-- =====================================================
-- Country Stored Procedures
-- =====================================================

-- Procedure: PR_Country_SelectAll
-- Returns all countries
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_Country_SelectAll' and xtype='P')
BEGIN
    DROP PROCEDURE PR_Country_SelectAll;
    PRINT 'Dropped existing PR_Country_SelectAll';
END
GO

CREATE PROCEDURE PR_Country_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CountryID,
        CountryName,
        CountryCode
    FROM Country
    ORDER BY CountryName;
END
GO
PRINT 'Created PR_Country_SelectAll';
GO

-- Procedure: PR_Country_SelectByPK
-- Returns a specific country by ID
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_Country_SelectByPK' and xtype='P')
BEGIN
    DROP PROCEDURE PR_Country_SelectByPK;
    PRINT 'Dropped existing PR_Country_SelectByPK';
END
GO

CREATE PROCEDURE PR_Country_SelectByPK
    @CountryID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        CountryID,
        CountryName,
        CountryCode
    FROM Country
    WHERE CountryID = @CountryID;
END
GO
PRINT 'Created PR_Country_SelectByPK';
GO

-- =====================================================
-- State Stored Procedures
-- =====================================================

-- Procedure: PR_State_SelectAll
-- Returns all states with country information
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_State_SelectAll' and xtype='P')
BEGIN
    DROP PROCEDURE PR_State_SelectAll;
    PRINT 'Dropped existing PR_State_SelectAll';
END
GO

CREATE PROCEDURE PR_State_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.StateID,
        s.StateName,
        s.StateCode,
        s.CountryID,
        c.CountryName
    FROM State s
    INNER JOIN Country c ON s.CountryID = c.CountryID
    ORDER BY c.CountryName, s.StateName;
END
GO
PRINT 'Created PR_State_SelectAll';
GO

-- Procedure: PR_State_SelectByPK
-- Returns a specific state by ID with country information
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_State_SelectByPK' and xtype='P')
BEGIN
    DROP PROCEDURE PR_State_SelectByPK;
    PRINT 'Dropped existing PR_State_SelectByPK';
END
GO

CREATE PROCEDURE PR_State_SelectByPK
    @StateID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.StateID,
        s.StateName,
        s.StateCode,
        s.CountryID,
        c.CountryName
    FROM State s
    INNER JOIN Country c ON s.CountryID = c.CountryID
    WHERE s.StateID = @StateID;
END
GO
PRINT 'Created PR_State_SelectByPK';
GO

-- =====================================================
-- City Stored Procedures
-- =====================================================

-- Procedure: PR_City_SelectAll
-- Returns all cities with state and country information
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_City_SelectAll' and xtype='P')
BEGIN
    DROP PROCEDURE PR_City_SelectAll;
    PRINT 'Dropped existing PR_City_SelectAll';
END
GO

CREATE PROCEDURE PR_City_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ci.CityID,
        ci.CityName,
        ci.CityCode,
        ci.StateID,
        s.StateName,
        s.CountryID,
        c.CountryName
    FROM City ci
    INNER JOIN State s ON ci.StateID = s.StateID
    INNER JOIN Country c ON s.CountryID = c.CountryID
    ORDER BY c.CountryName, s.StateName, ci.CityName;
END
GO
PRINT 'Created PR_City_SelectAll';
GO

-- Procedure: PR_City_SelectByPK
-- Returns a specific city by ID with state and country information
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_City_SelectByPK' and xtype='P')
BEGIN
    DROP PROCEDURE PR_City_SelectByPK;
    PRINT 'Dropped existing PR_City_SelectByPK';
END
GO

CREATE PROCEDURE PR_City_SelectByPK
    @CityID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ci.CityID,
        ci.CityName,
        ci.CityCode,
        ci.StateID,
        s.StateName,
        s.CountryID,
        c.CountryName
    FROM City ci
    INNER JOIN State s ON ci.StateID = s.StateID
    INNER JOIN Country c ON s.CountryID = c.CountryID
    WHERE ci.CityID = @CityID;
END
GO
PRINT 'Created PR_City_SelectByPK';
GO

-- =====================================================
-- Additional Lab 11 Requirement Procedures
-- =====================================================

-- Procedure: PR_City_SelectByName
-- Filter cities by name (supports partial search)
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_City_SelectByName' and xtype='P')
BEGIN
    DROP PROCEDURE PR_City_SelectByName;
    PRINT 'Dropped existing PR_City_SelectByName';
END
GO

CREATE PROCEDURE PR_City_SelectByName
    @CityName NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ci.CityID,
        ci.CityName,
        ci.CityCode,
        ci.StateID,
        s.StateName,
        s.CountryID,
        c.CountryName
    FROM City ci
    INNER JOIN State s ON ci.StateID = s.StateID
    INNER JOIN Country c ON s.CountryID = c.CountryID
    WHERE ci.CityName LIKE '%' + @CityName + '%'
    ORDER BY ci.CityName;
END
GO
PRINT 'Created PR_City_SelectByName';
GO

-- Procedure: PR_City_SelectByState
-- Display cities by state
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_City_SelectByState' and xtype='P')
BEGIN
    DROP PROCEDURE PR_City_SelectByState;
    PRINT 'Dropped existing PR_City_SelectByState';
END
GO

CREATE PROCEDURE PR_City_SelectByState
    @StateID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        ci.CityID,
        ci.CityName,
        ci.CityCode,
        ci.StateID,
        s.StateName,
        s.CountryID,
        c.CountryName
    FROM City ci
    INNER JOIN State s ON ci.StateID = s.StateID
    INNER JOIN Country c ON s.CountryID = c.CountryID
    WHERE ci.StateID = @StateID
    ORDER BY ci.CityName;
END
GO
PRINT 'Created PR_City_SelectByState';
GO

-- Procedure: PR_State_SelectWithCityCount
-- Display states with city count by country
IF EXISTS (SELECT * FROM sysobjects WHERE name='PR_State_SelectWithCityCount' and xtype='P')
BEGIN
    DROP PROCEDURE PR_State_SelectWithCityCount;
    PRINT 'Dropped existing PR_State_SelectWithCityCount';
END
GO

CREATE PROCEDURE PR_State_SelectWithCityCount
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        s.StateID,
        s.StateName,
        s.StateCode,
        s.CountryID,
        c.CountryName,
        ISNULL(CityCount.CityCount, 0) AS CityCount
    FROM State s
    INNER JOIN Country c ON s.CountryID = c.CountryID
    LEFT JOIN (
        SELECT StateID, COUNT(*) AS CityCount
        FROM City
        GROUP BY StateID
    ) CityCount ON s.StateID = CityCount.StateID
    ORDER BY c.CountryName, s.StateName;
END
GO
PRINT 'Created PR_State_SelectWithCityCount';
GO

-- =====================================================
-- Test Procedures
-- =====================================================

PRINT '================================================';
PRINT 'Testing Stored Procedures';
PRINT '================================================';

-- Test Country procedures
PRINT 'Testing PR_Country_SelectAll:';
EXEC PR_Country_SelectAll;
PRINT '';

PRINT 'Testing PR_Country_SelectByPK (ID=1):';
EXEC PR_Country_SelectByPK @CountryID = 1;
PRINT '';

-- Test State procedures
PRINT 'Testing PR_State_SelectAll:';
EXEC PR_State_SelectAll;
PRINT '';

PRINT 'Testing PR_State_SelectByPK (ID=1):';
EXEC PR_State_SelectByPK @StateID = 1;
PRINT '';

-- Test City procedures
PRINT 'Testing PR_City_SelectAll (first 5 records):';
SELECT TOP 5 * FROM (
    EXEC PR_City_SelectAll
) AS CityData;
PRINT '';

PRINT 'Testing PR_City_SelectByPK (ID=1):';
EXEC PR_City_SelectByPK @CityID = 1;
PRINT '';

-- Test Lab 11 specific procedures
PRINT 'Testing PR_City_SelectByName (searching for ''Ahmedabad''):';
EXEC PR_City_SelectByName @CityName = 'Ahmedabad';
PRINT '';

PRINT 'Testing PR_City_SelectByState (StateID=1):';
EXEC PR_City_SelectByState @StateID = 1;
PRINT '';

PRINT 'Testing PR_State_SelectWithCityCount:';
EXEC PR_State_SelectWithCityCount;
PRINT '';

PRINT '================================================';
PRINT 'All stored procedures created and tested successfully!';
PRINT '================================================';
GO