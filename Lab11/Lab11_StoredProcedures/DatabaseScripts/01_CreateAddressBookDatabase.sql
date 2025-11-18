-- =====================================================
-- Lab 11: AddressBook Database Creation Script
-- =====================================================
-- This script creates the AddressBook database with
-- Country, State, and City tables for Lab 11
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
        CountryCode NVARCHAR(10) NOT NULL UNIQUE
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
        StateName NVARCHAR(100) NOT NULL UNIQUE,
        StateCode NVARCHAR(10) NOT NULL UNIQUE,
        CountryID INT NOT NULL FOREIGN KEY REFERENCES Country(CountryID)
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
        -- Ensure unique city name within a state
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
-- Insert Sample Data
-- =====================================================

-- Clear existing data to avoid duplicates
DELETE FROM City;
DELETE FROM State;
DELETE FROM Country;
GO

-- Insert Countries
INSERT INTO Country (CountryName, CountryCode) VALUES
('India', 'IN'),
('United States', 'US'),
('United Kingdom', 'UK'),
('Canada', 'CA'),
('Australia', 'AU');
PRINT 'Sample countries inserted';
GO

-- Insert States for India
INSERT INTO State (StateName, StateCode, CountryID) VALUES
('Gujarat', 'GJ', 1),
('Maharashtra', 'MH', 1),
('Karnataka', 'KA', 1),
('Delhi', 'DL', 1),
('Tamil Nadu', 'TN', 1);
PRINT 'Indian states inserted';
GO

-- Insert States for United States
INSERT INTO State (StateName, StateCode, CountryID) VALUES
('California', 'CA', 2),
('Texas', 'TX', 2),
('New York', 'NY', 2),
('Florida', 'FL', 2),
('Illinois', 'IL', 2);
PRINT 'US states inserted';
GO

-- Insert Cities for Gujarat, India
INSERT INTO City (CityName, CityCode, StateID) VALUES
('Ahmedabad', 'AMD', 1),
('Surat', 'SUR', 1),
('Vadodara', 'VAD', 1),
('Rajkot', 'RJK', 1),
('Gandhinagar', 'GND', 1);
GO

-- Insert Cities for Maharashtra, India
INSERT INTO City (CityName, CityCode, StateID) VALUES
('Mumbai', 'MUM', 2),
('Pune', 'PUN', 2),
('Nagpur', 'NGP', 2),
('Nashik', 'NSK', 2),
('Thane', 'THN', 2);
GO

-- Insert Cities for Karnataka, India
INSERT INTO City (CityName, CityCode, StateID) VALUES
('Bangalore', 'BLR', 3),
('Mysore', 'MYS', 3),
('Hubli', 'HBL', 3),
('Mangalore', 'MNG', 3),
('Belgaum', 'BLG', 3);
GO

-- Insert Cities for Delhi, India
INSERT INTO City (CityName, CityCode, StateID) VALUES
('New Delhi', 'NDL', 4),
('North Delhi', 'NDH', 4),
('South Delhi', 'SDH', 4),
('East Delhi', 'EDH', 4),
('West Delhi', 'WDH', 4);
GO

-- Insert Cities for Tamil Nadu, India
INSERT INTO City (CityName, CityCode, StateID) VALUES
('Chennai', 'CHN', 5),
('Coimbatore', 'CBE', 5),
('Madurai', 'MDU', 5),
('Tiruchirappalli', 'TZY', 5),
('Salem', 'SLM', 5);
GO

-- Insert Cities for California, US
INSERT INTO City (CityName, CityCode, StateID) VALUES
('Los Angeles', 'LAX', 6),
('San Francisco', 'SFO', 6),
('San Diego', 'SAN', 6),
('Sacramento', 'SAC', 6),
('San Jose', 'SJC', 6);
GO

-- Insert Cities for Texas, US
INSERT INTO City (CityName, CityCode, StateID) VALUES
('Houston', 'HOU', 7),
('Austin', 'AUS', 7),
('Dallas', 'DAL', 7),
('San Antonio', 'SAT', 7),
('Fort Worth', 'FTW', 7);
GO

PRINT 'Sample data insertion completed';
PRINT '================================================';
PRINT 'AddressBook database setup completed successfully!';
PRINT '================================================';
GO

-- Display summary
SELECT 'Countries' as TableName, COUNT(*) as RecordCount FROM Country
UNION ALL
SELECT 'States', COUNT(*) FROM State
UNION ALL
SELECT 'Cities', COUNT(*) FROM City;
GO