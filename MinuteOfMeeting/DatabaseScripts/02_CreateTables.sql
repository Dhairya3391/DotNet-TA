/*
 * Database: MOM_Database
 * Purpose: Create all tables for Meeting Management System
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

USE MOM_Database;
GO

-- =============================================
-- Table 1: MOM_MeetingType (Master Table)
-- Stores different types of meetings
-- =============================================
IF OBJECT_ID('MOM_MeetingType', 'U') IS NOT NULL
    DROP TABLE MOM_MeetingType;
GO

CREATE TABLE MOM_MeetingType
(
    MeetingTypeID INT PRIMARY KEY IDENTITY(1,1),
    MeetingTypeName NVARCHAR(100) NOT NULL UNIQUE,
    Remarks NVARCHAR(100) NOT NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL
);
GO

PRINT 'Table MOM_MeetingType created successfully';
GO

-- =============================================
-- Table 2: MOM_Department (Master Table)
-- Stores organizational departments
-- =============================================
IF OBJECT_ID('MOM_Department', 'U') IS NOT NULL
    DROP TABLE MOM_Department;
GO

CREATE TABLE MOM_Department
(
    DepartmentID INT PRIMARY KEY IDENTITY(1,1),
    DepartmentName NVARCHAR(100) NOT NULL UNIQUE,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL
);
GO

PRINT 'Table MOM_Department created successfully';
GO

-- =============================================
-- Table 3: MOM_MeetingVenue (Master Table)
-- Stores meeting venues/locations
-- =============================================
IF OBJECT_ID('MOM_MeetingVenue', 'U') IS NOT NULL
    DROP TABLE MOM_MeetingVenue;
GO

CREATE TABLE MOM_MeetingVenue
(
    MeetingVenueID INT PRIMARY KEY IDENTITY(1,1),
    MeetingVenueName NVARCHAR(100) NOT NULL UNIQUE,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL
);
GO

PRINT 'Table MOM_MeetingVenue created successfully';
GO

-- =============================================
-- Table 4: MOM_Staff (Master Table)
-- Stores staff/member information
-- =============================================
IF OBJECT_ID('MOM_Staff', 'U') IS NOT NULL
    DROP TABLE MOM_Staff;
GO

CREATE TABLE MOM_Staff
(
    StaffID INT PRIMARY KEY IDENTITY(1,1),
    DepartmentID INT NOT NULL,
    StaffName NVARCHAR(50) NOT NULL,
    MobileNo NVARCHAR(20) NOT NULL,
    EmailAddress NVARCHAR(50) NOT NULL UNIQUE,
    Remarks NVARCHAR(250) NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    CONSTRAINT FK_Staff_Department FOREIGN KEY (DepartmentID)
        REFERENCES MOM_Department(DepartmentID)
);
GO

PRINT 'Table MOM_Staff created successfully';
GO

-- =============================================
-- Table 5: MOM_Meetings (Transaction Table)
-- Stores meeting records with scheduling information
-- =============================================
IF OBJECT_ID('MOM_Meetings', 'U') IS NOT NULL
    DROP TABLE MOM_Meetings;
GO

CREATE TABLE MOM_Meetings
(
    MeetingID INT PRIMARY KEY IDENTITY(1,1),
    MeetingDate DATETIME NOT NULL,
    MeetingVenueID INT NOT NULL,
    MeetingTypeID INT NOT NULL,
    DepartmentID INT NOT NULL,
    MeetingDescription NVARCHAR(250) NULL,
    DocumentPath NVARCHAR(250) NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    IsCancelled BIT NULL DEFAULT 0,
    CancellationDateTime DATETIME NULL,
    CancellationReason NVARCHAR(250) NULL,
    CONSTRAINT FK_Meeting_Venue FOREIGN KEY (MeetingVenueID)
        REFERENCES MOM_MeetingVenue(MeetingVenueID),
    CONSTRAINT FK_Meeting_Type FOREIGN KEY (MeetingTypeID)
        REFERENCES MOM_MeetingType(MeetingTypeID),
    CONSTRAINT FK_Meeting_Department FOREIGN KEY (DepartmentID)
        REFERENCES MOM_Department(DepartmentID)
);
GO

PRINT 'Table MOM_Meetings created successfully';
GO

-- =============================================
-- Table 6: MOM_MeetingMember (Junction Table)
-- Many-to-many relationship between Meetings and Staff
-- Tracks attendance and participation
-- =============================================
IF OBJECT_ID('MOM_MeetingMember', 'U') IS NOT NULL
    DROP TABLE MOM_MeetingMember;
GO

CREATE TABLE MOM_MeetingMember
(
    MeetingMemberID INT PRIMARY KEY IDENTITY(1,1),
    MeetingID INT NOT NULL,
    StaffID INT NOT NULL,
    IsPresent BIT NOT NULL DEFAULT 0,
    Remarks NVARCHAR(250) NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    Modified DATETIME NOT NULL,
    CONSTRAINT FK_MeetingMember_Meeting FOREIGN KEY (MeetingID)
        REFERENCES MOM_Meetings(MeetingID) ON DELETE CASCADE,
    CONSTRAINT FK_MeetingMember_Staff FOREIGN KEY (StaffID)
        REFERENCES MOM_Staff(StaffID),
    CONSTRAINT UQ_MeetingMember UNIQUE(MeetingID, StaffID) -- Prevent duplicate attendance records
);
GO

PRINT 'Table MOM_MeetingMember created successfully';
GO

-- =============================================
-- Table 7: MOM_User (Authentication Table)
-- Stores user authentication information
-- =============================================
IF OBJECT_ID('MOM_User', 'U') IS NOT NULL
    DROP TABLE MOM_User;
GO

CREATE TABLE MOM_User
(
    UserID INT PRIMARY KEY IDENTITY(1,1),
    StaffID INT NULL, -- Optional link to staff member
    Username NVARCHAR(50) NOT NULL UNIQUE,
    Password NVARCHAR(255) NOT NULL, -- Store hashed passwords
    Role NVARCHAR(20) NOT NULL, -- Admin, Organizer, Staff
    IsActive BIT NOT NULL DEFAULT 1,
    LastLogin DATETIME NULL,
    Created DATETIME NOT NULL DEFAULT GETDATE(),
    CONSTRAINT FK_User_Staff FOREIGN KEY (StaffID)
        REFERENCES MOM_Staff(StaffID),
    CONSTRAINT CK_User_Role CHECK (Role IN ('Admin', 'Organizer', 'Staff'))
);
GO

PRINT 'Table MOM_User created successfully';
GO

-- =============================================
-- Create Indexes for better performance
-- =============================================

-- Index on MeetingDate for faster date-based queries
CREATE NONCLUSTERED INDEX IX_Meeting_Date
    ON MOM_Meetings(MeetingDate);
GO

-- Index on IsCancelled for filtering active meetings
CREATE NONCLUSTERED INDEX IX_Meeting_IsCancelled
    ON MOM_Meetings(IsCancelled);
GO

-- Index on StaffID for faster attendance lookups
CREATE NONCLUSTERED INDEX IX_MeetingMember_Staff
    ON MOM_MeetingMember(StaffID);
GO

-- Index on MeetingID for faster meeting member lookups
CREATE NONCLUSTERED INDEX IX_MeetingMember_Meeting
    ON MOM_MeetingMember(MeetingID);
GO

PRINT 'Indexes created successfully';
GO

PRINT '========================================';
PRINT 'All tables created successfully!';
PRINT '========================================';
GO
