/*
 * Database: MOM_Database (Minutes of Meeting)
 * Purpose: Create database for Meeting Management System
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

-- Drop database if exists (use with caution in production)
IF EXISTS (SELECT name FROM sys.databases WHERE name = N'MOM_Database')
BEGIN
    ALTER DATABASE MOM_Database SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE MOM_Database;
END
GO

-- Create new database
CREATE DATABASE MOM_Database;
GO

-- Use the database
USE MOM_Database;
GO

PRINT 'Database MOM_Database created successfully';
GO
