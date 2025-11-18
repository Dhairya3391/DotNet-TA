-- =====================================================
-- Lab 12: AddressBook Database Creation Script
-- =====================================================
-- This script creates the AddressBook database with
-- Country, State, City, and Employee tables for Lab 12
-- =====================================================

-- Create Database
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'AddressBook')
BEGIN
    CREATE DATABASE AddressBook;
END
GO

USE AddressBook;
GO

-- =====================================================
-- Create Country Table
-- =====================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Country' and xtype='U')
BEGIN
    CREATE TABLE Country (
        CountryID INT IDENTITY(1,1) PRIMARY KEY,
        CountryName NVARCHAR(100) NOT NULL UNIQUE,
        CountryCode NVARCHAR(10) NOT NULL UNIQUE,
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME NULL
    );

    PRINT 'Country table created successfully';
END
ELSE
BEGIN
    PRINT 'Country table already exists';
END
GO

-- =====================================================
-- Create State Table
-- =====================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='State' and xtype='U')
BEGIN
    CREATE TABLE State (
        StateID INT IDENTITY(1,1) PRIMARY KEY,
        StateName NVARCHAR(100) NOT NULL,
        StateCode NVARCHAR(10) NOT NULL,
        CountryID INT NOT NULL FOREIGN KEY REFERENCES Country(CountryID),
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME NULL,
        CONSTRAINT UQ_State_Country_Name UNIQUE (CountryID, StateName)
    );

    PRINT 'State table created successfully';
END
ELSE
BEGIN
    PRINT 'State table already exists';
END
GO

-- =====================================================
-- Create City Table
-- =====================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='City' and xtype='U')
BEGIN
    CREATE TABLE City (
        CityID INT IDENTITY(1,1) PRIMARY KEY,
        CityName NVARCHAR(100) NOT NULL,
        CityCode NVARCHAR(10) NOT NULL,
        StateID INT NOT NULL FOREIGN KEY REFERENCES State(StateID),
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME NULL,
        CONSTRAINT UQ_City_State_City UNIQUE (StateID, CityName)
    );

    PRINT 'City table created successfully';
END
ELSE
BEGIN
    PRINT 'City table already exists';
END
GO

-- =====================================================
-- Create Employee Table (Custom Table for Lab 12)
-- =====================================================
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='Employee' and xtype='U')
BEGIN
    CREATE TABLE Employee (
        EmployeeID INT IDENTITY(1,1) PRIMARY KEY,
        EmployeeCode NVARCHAR(20) NOT NULL UNIQUE,
        FirstName NVARCHAR(50) NOT NULL,
        LastName NVARCHAR(50) NOT NULL,
        Email NVARCHAR(100) NOT NULL UNIQUE,
        PhoneNumber NVARCHAR(20) NULL,
        Department NVARCHAR(50) NOT NULL,
        Position NVARCHAR(50) NOT NULL,
        Salary DECIMAL(18,2) NOT NULL,
        HireDate DATE NOT NULL,
        IsActive BIT DEFAULT 1,
        CreatedDate DATETIME DEFAULT GETDATE(),
        UpdatedDate DATETIME NULL,
        CONSTRAINT CHK_Email_Format CHECK (Email LIKE '%@%.%'),
        CONSTRAINT CHK_Salary_Positive CHECK (Salary > 0),
        CONSTRAINT CHK_HireDate_Valid CHECK (HireDate <= GETDATE())
    );

    PRINT 'Employee table created successfully';
END
ELSE
BEGIN
    PRINT 'Employee table already exists';
END
GO

-- =====================================================
-- Insert Sample Data
-- =====================================================

-- Clear existing data to avoid duplicates
DELETE FROM Employee;
DELETE FROM City;
DELETE FROM State;
DELETE FROM Country;
GO

-- Insert Countries
INSERT INTO Country (CountryName, CountryCode, IsActive) VALUES
('India', 'IN', 1),
('United States', 'US', 1),
('United Kingdom', 'UK', 1),
('Canada', 'CA', 1),
('Australia', 'AU', 1);
PRINT 'Sample countries inserted';
GO

-- Insert States for India
INSERT INTO State (StateName, StateCode, CountryID, IsActive) VALUES
('Gujarat', 'GJ', 1, 1),
('Maharashtra', 'MH', 1, 1),
('Karnataka', 'KA', 1, 1),
('Delhi', 'DL', 1, 1),
('Tamil Nadu', 'TN', 1, 1);
PRINT 'Indian states inserted';
GO

-- Insert States for United States
INSERT INTO State (StateName, StateCode, CountryID, IsActive) VALUES
('California', 'CA', 2, 1),
('Texas', 'TX', 2, 1),
('New York', 'NY', 2, 1),
('Florida', 'FL', 2, 1),
('Illinois', 'IL', 2, 1);
PRINT 'US states inserted';
GO

-- Insert Cities for Gujarat, India
INSERT INTO City (CityName, CityCode, StateID, IsActive) VALUES
('Ahmedabad', 'AMD', 1, 1),
('Surat', 'SUR', 1, 1),
('Vadodara', 'VAD', 1, 1),
('Rajkot', 'RJK', 1, 1),
('Gandhinagar', 'GND', 1, 1);
GO

-- Insert Cities for Maharashtra, India
INSERT INTO City (CityName, CityCode, StateID, IsActive) VALUES
('Mumbai', 'MUM', 2, 1),
('Pune', 'PUN', 2, 1),
('Nagpur', 'NGP', 2, 1),
('Nashik', 'NSK', 2, 1),
('Thane', 'THN', 2, 1);
GO

-- Insert Cities for Karnataka, India
INSERT INTO City (CityName, CityCode, StateID, IsActive) VALUES
('Bangalore', 'BLR', 3, 1),
('Mysore', 'MYS', 3, 1),
('Hubli', 'HBL', 3, 1),
('Mangalore', 'MNG', 3, 1),
('Belgaum', 'BLG', 3, 1);
GO

-- Insert Sample Employees
INSERT INTO Employee (EmployeeCode, FirstName, LastName, Email, PhoneNumber, Department, Position, Salary, HireDate, IsActive) VALUES
('EMP001', 'John', 'Smith', 'john.smith@company.com', '9876543210', 'IT', 'Software Developer', 75000.00, '2022-01-15', 1),
('EMP002', 'Sarah', 'Johnson', 'sarah.johnson@company.com', '9876543211', 'HR', 'HR Manager', 85000.00, '2021-06-10', 1),
('EMP003', 'Michael', 'Brown', 'michael.brown@company.com', '9876543212', 'Finance', 'Accountant', 65000.00, '2022-03-20', 1),
('EMP004', 'Emily', 'Davis', 'emily.davis@company.com', '9876543213', 'IT', 'Project Manager', 90000.00, '2020-11-05', 1),
('EMP005', 'David', 'Wilson', 'david.wilson@company.com', '9876543214', 'Sales', 'Sales Executive', 55000.00, '2022-07-12', 1),
('EMP006', 'Lisa', 'Anderson', 'lisa.anderson@company.com', '9876543215', 'Marketing', 'Marketing Manager', 80000.00, '2021-09-18', 1),
('EMP007', 'James', 'Taylor', 'james.taylor@company.com', '9876543216', 'IT', 'Senior Developer', 95000.00, '2019-04-22', 1),
('EMP008', 'Jennifer', 'Thomas', 'jennifer.thomas@company.com', '9876543217', 'HR', 'Recruiter', 60000.00, '2022-05-30', 1);
PRINT 'Sample employees inserted';
GO

PRINT '================================================';
PRINT 'AddressBook database setup completed successfully!';
PRINT '================================================';
GO

-- Display summary
SELECT 'Countries' as TableName, COUNT(*) as RecordCount FROM Country
UNION ALL
SELECT 'States', COUNT(*) FROM State
UNION ALL
SELECT 'Cities', COUNT(*) FROM City
UNION ALL
SELECT 'Employees', COUNT(*) FROM Employee;
GO