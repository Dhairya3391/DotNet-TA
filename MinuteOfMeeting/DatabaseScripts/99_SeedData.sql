/*
 * Database: MOM_Database
 * Purpose: Insert sample data for testing and demonstration
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

USE MOM_Database;
GO

PRINT '========================================';
PRINT 'Starting Seed Data Insertion...';
PRINT '========================================';
GO

-- =============================================
-- Insert Meeting Types
-- =============================================
PRINT 'Inserting Meeting Types...';

INSERT INTO MOM_MeetingType (MeetingTypeName, Remarks, Created, Modified) VALUES
('Team Meeting', 'Regular team sync and status updates', GETDATE(), GETDATE()),
('Review Meeting', 'Project reviews and retrospectives', GETDATE(), GETDATE()),
('Planning Meeting', 'Strategic planning and project planning', GETDATE(), GETDATE()),
('Training Session', 'Skill development and training programs', GETDATE(), GETDATE()),
('Client Meeting', 'Client discussions and presentations', GETDATE(), GETDATE()),
('Interview', 'Candidate interviews and evaluations', GETDATE(), GETDATE()),
('Board Meeting', 'Board of directors and governance meetings', GETDATE(), GETDATE()),
('Brainstorming', 'Creative sessions and idea generation', GETDATE(), GETDATE());
GO

-- =============================================
-- Insert Departments
-- =============================================
PRINT 'Inserting Departments...';

INSERT INTO MOM_Department (DepartmentName, Created, Modified) VALUES
('Information Technology', GETDATE(), GETDATE()),
('Human Resources', GETDATE(), GETDATE()),
('Finance', GETDATE(), GETDATE()),
('Marketing', GETDATE(), GETDATE()),
('Operations', GETDATE(), GETDATE()),
('Research & Development', GETDATE(), GETDATE()),
('Customer Service', GETDATE(), GETDATE()),
('Sales', GETDATE(), GETDATE());
GO

-- =============================================
-- Insert Meeting Venues
-- =============================================
PRINT 'Inserting Meeting Venues...';

INSERT INTO MOM_MeetingVenue (MeetingVenueName, Created, Modified) VALUES
('Conference Room A', GETDATE(), GETDATE()),
('Conference Room B', GETDATE(), GETDATE()),
('Training Room 1', GETDATE(), GETDATE()),
('Training Room 2', GETDATE(), GETDATE()),
('Board Room', GETDATE(), GETDATE()),
('Auditorium', GETDATE(), GETDATE()),
('Cafeteria', GETDATE(), GETDATE()),
('Virtual Meeting - Zoom', GETDATE(), GETDATE()),
('Virtual Meeting - Teams', GETDATE(), GETDATE()),
('Outdoor Garden', GETDATE(), GETDATE());
GO

-- =============================================
-- Insert Staff Members
-- =============================================
PRINT 'Inserting Staff Members...';

INSERT INTO MOM_Staff (DepartmentID, StaffName, MobileNo, EmailAddress, Remarks, Created, Modified) VALUES
-- IT Department
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Information Technology'), 'John Smith', '555-0101', 'john.smith@company.com', 'Senior Software Developer', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Information Technology'), 'Emily Johnson', '555-0102', 'emily.j@company.com', 'Database Administrator', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Information Technology'), 'Michael Chen', '555-0103', 'm.chen@company.com', 'System Administrator', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Information Technology'), 'Sarah Williams', '555-0104', 'sarah.w@company.com', 'UI/UX Designer', GETDATE(), GETDATE()),

-- Human Resources
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Human Resources'), 'Patricia Brown', '555-0201', 'patricia.b@company.com', 'HR Manager', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Human Resources'), 'Robert Davis', '555-0202', 'robert.d@company.com', 'HR Specialist', GETDATE(), GETDATE()),

-- Finance
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Finance'), 'James Wilson', '555-0301', 'james.w@company.com', 'Finance Director', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Finance'), 'Jennifer Garcia', '555-0302', 'jennifer.g@company.com', 'Accountant', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Finance'), 'David Martinez', '555-0303', 'david.m@company.com', 'Financial Analyst', GETDATE(), GETDATE()),

-- Marketing
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Marketing'), 'Maria Rodriguez', '555-0401', 'maria.r@company.com', 'Marketing Manager', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Marketing'), 'Thomas Anderson', '555-0402', 'thomas.a@company.com', 'Marketing Specialist', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Marketing'), 'Lisa Taylor', '555-0403', 'lisa.t@company.com', 'Content Creator', GETDATE(), GETDATE()),

-- Operations
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Operations'), 'Christopher Lee', '555-0501', 'chris.lee@company.com', 'Operations Manager', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Operations'), 'Amanda White', '555-0502', 'amanda.w@company.com', 'Process Analyst', GETDATE(), GETDATE()),

-- R&D
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Research & Development'), 'Daniel Kim', '555-0601', 'daniel.k@company.com', 'R&D Manager', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Research & Development'), 'Sophie Martin', '555-0602', 'sophie.m@company.com', 'Research Scientist', GETDATE(), GETDATE()),

-- Customer Service
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Customer Service'), 'Kevin Thompson', '555-0701', 'kevin.t@company.com', 'Call Center Manager', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Customer Service'), 'Nancy Moore', '555-0702', 'nancy.m@company.com', 'Customer Service Rep', GETDATE(), GETDATE()),

-- Sales
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Sales'), 'Richard Jackson', '555-0801', 'richard.j@company.com', 'Sales Director', GETDATE(), GETDATE()),
((SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Sales'), 'Olivia Martin', '555-0802', 'olivia.m@company.com', 'Sales Executive', GETDATE(), GETDATE());
GO

-- =============================================
-- Insert Users (Accounts for Authentication)
-- =============================================
PRINT 'Inserting Users...';

-- Simple password hashing (not secure for production - just for demo)
INSERT INTO MOM_User (StaffID, Username, Password, Role, IsActive, Created) VALUES
-- Admin accounts
((SELECT StaffID FROM MOM_Staff WHERE EmailAddress = 'john.smith@company.com'), 'admin', 'admin123', 'Admin', 1, GETDATE()),

-- Manager accounts
((SELECT StaffID FROM MOM_Staff WHERE EmailAddress = 'patricia.b@company.com'), 'hrmanager', 'hr123', 'Organizer', 1, GETDATE()),
((SELECT StaffID FROM MOM_Staff WHERE EmailAddress = 'james.w@company.com'), 'financemanager', 'fin123', 'Organizer', 1, GETDATE()),
((SELECT StaffID FROM MOM_Staff WHERE EmailAddress = 'maria.r@company.com'), 'marketingmanager', 'mkt123', 'Organizer', 1, GETDATE()),
((SELECT StaffID FROM MOM_Staff WHERE EmailAddress = 'chris.lee@company.com'), 'operationsmanager', 'ops123', 'Organizer', 1, GETDATE()),

-- Staff accounts
((SELECT StaffID FROM MOM_Staff WHERE EmailAddress = 'emily.j@company.com'), 'itstaff1', '.staff123', 'Staff', 1, GETDATE()),
((SELECT StaffID FROM MOM_Staff WHERE EmailAddress = 'robert.d@company.com'), 'hrstaff1', 'staff123', 'Staff', 1, GETDATE()),
((SELECT StaffID FROM MOM_Staff WHERE EmailAddress = 'jennifer.g@company.com'), 'financestaff1', 'staff123', 'Staff', 1, GETDATE()),
((SELECT StaffID FROM MOM_Staff WHERE EmailAddress = 'thomas.a@company.com'), 'mktstaff1', 'staff123', 'Staff', 1, GETDATE()),
((SELECT StaffID FROM MOM_Staff WHERE EmailAddress = 'daniel.k@company.com'), 'rdstaff1', 'staff123', 'Staff', 1, GETDATE()),

-- Generic account without staff association
(NULL, 'organizer', 'org123', 'Organizer', 1, GETDATE()),
(NULL, 'staff', 'staff123', 'Staff', 1, GETDATE());
GO

-- =============================================
-- Insert Sample Meetings
-- =============================================
PRINT 'Inserting Sample Meetings...';

INSERT INTO MOM_Meetings (
    MeetingDate, MeetingVenueID, MeetingTypeID, DepartmentID,
    MeetingDescription, DocumentPath, Created, Modified
) VALUES
-- Recent meetings (last month)
(DATEADD(DAY, -35, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Conference Room A'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Team Meeting'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Information Technology'),
 'Q1 Project Review and Planning Session', '/uploads/q1-review.pdf', GETDATE(), GETDATE()),

(DATEADD(DAY, -30, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Training Room 1'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Training Session'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Information Technology'),
 'ASP.NET Core MVC Best Practices Training', '/uploads/mvc-training.docx', GETDATE(), GETDATE()),

(DATEADD(DAY, -28, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Board Room'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Board Meeting'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Finance'),
 'Budget Review and Approval Meeting', NULL, GETDATE(), GETDATE()),

-- This week's meetings
(DATEADD(DAY, -3, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Conference Room B'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Team Meeting'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Marketing'),
 'Marketing Campaign Review', '/uploads/marketing-review.pdf', GETDATE(), GETDATE()),

(DATEADD(DAY, -2, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Virtual Meeting - Zoom'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Client Meeting'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Sales'),
 'Client Presentation - New Product Launch', '/uploads/client-presentation.pptx', GETDATE(), GETDATE()),

-- Upcoming meetings
(DATEADD(DAY, 1, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Conference Room A'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Planning Meeting'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Research & Development'),
 'R&D Project Planning Session', NULL, GETDATE(), GETDATE()),

(DATEADD(DAY, 2, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Training Room 2'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Training Session'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Human Resources'),
 'Leadership Development Program', NULL, GETDATE(), GETDATE()),

(DATEADD(DAY, 3, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Conference Room B'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Review Meeting'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Operations'),
 'Process Improvement Review', NULL, GETDATE(), GETDATE()),

(DATEADD(DAY, 5, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Board Room'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Interview'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Information Technology'),
 'Senior Developer Interview - Candidate 1', NULL, GETDATE(), GETDATE()),

(DATEADD(DAY, 7, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Outdoor Garden'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Brainstorming'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Marketing'),
 'Creative Brainstorming - Next Campaign Ideas', NULL, GETDATE(), GETDATE()),

(DATEADD(DAY, 10, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Virtual Meeting - Teams'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Team Meeting'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Customer Service'),
 'Monthly Customer Service Team Sync', NULL, GETDATE(), GETDATE()),

-- Cancelled meeting
(DATEADD(DAY, -15, GETDATE()), (SELECT MeetingVenueID FROM MOM_MeetingVenue WHERE MeetingVenueName = 'Auditorium'),
 (SELECT MeetingTypeID FROM MOM_MeetingType WHERE MeetingTypeName = 'Training Session'),
 (SELECT DepartmentID FROM MOM_Department WHERE DepartmentName = 'Operations'),
 'Annual Safety Training', NULL, GETDATE(), DATEADD(DAY, -10, GETDATE()));

-- Update cancelled meetings
UPDATE MOM_Meetings
SET IsCancelled = 1, CancellationDateTime = DATEADD(DAY, -10, GETDATE()),
    CancellationReason = 'Venue maintenance emergency'
WHERE MeetingDescription = 'Annual Safety Training';
GO

-- =============================================
-- Insert Meeting Members (Attendance)
-- =============================================
PRINT 'Inserting Meeting Members...';

-- Helper: Insert attendance for each meeting
DECLARE @MeetingID INT;
DECLARE @StaffIDs NVARCHAR(MAX);

-- IT Department meetings
SET @MeetingID = (SELECT MeetingID FROM MOM_Meetings WHERE MeetingDescription = 'Q1 Project Review and Planning Session');
SET @StaffIDs = '1,2,3,4'; -- John, Emily, Michael, Sarah
EXEC PR_MeetingMember_BulkInsert @MeetingID, @StaffIDs, GETDATE(), GETDATE();

-- Mark as present (since this was in the past)
UPDATE MOM_MeetingMember SET IsPresent = 1, Modified = GETDATE()
WHERE StaffID IN (1, 2, 4) AND MeetingID = @MeetingID; -- John, Emily, Sarah attended

-- Training session
SET @MeetingID = (SELECT MeetingID FROM MOM_Meetings WHERE MeetingDescription = 'ASP.NET Core MVC Best Practices Training');
SET @StaffIDs = '1,2,3,4,15';
EXEC PR_MeetingMember_BulkInsert @MeetingID, @StaffIDs, GETDATE(), GETDATE();

UPDATE MOM_MeetingMember SET IsPresent = 1, Modified = GETDATE()
WHERE StaffID IN (1, 2, 3, 4) AND MeetingID = @MeetingID;

-- Board meeting
SET @MeetingID = (SELECT MeetingID FROM MOM_Meetings WHERE MeetingDescription = 'Budget Review and Approval Meeting');
SET @StaffIDs = '7,8,9';
EXEC PR_MeetingMember_BulkInsert @MeetingID, @StaffIDs, GETDATE(), GETDATE();

UPDATE MOM_MeetingMember SET IsPresent = 1, Modified = GETDATE()
WHERE StaffID IN (7, 8, 9) AND MeetingID = @MeetingID;

-- Marketing meeting
SET @MeetingID = (SELECT MeetingID FROM MOM_Meetings WHERE MeetingDescription = 'Marketing Campaign Review');
SET @StaffIDs = '11,12,13';
EXEC PR_MeetingMember_BulkInsert @MeetingID, @StaffIDs, GETDATE(), GETDATE();

UPDATE MOM_MeetingMember SET IsPresent = 1, Modified = GETDATE()
WHERE StaffID IN (11, 12, 13) AND MeetingID = @MeetingID;

-- Client presentation
SET @MeetingID = (SELECT MeetingID FROM MOM_Meetings WHERE MeetingDescription = 'Client Presentation - New Product Launch');
SET @StaffIDs = '17,18';
EXEC PR_MeetingMember_BulkInsert @MeetingID, @StaffIDs, GETDATE(), GETDATE();

UPDATE MOM_MeetingMember SET IsPresent = 1, Modified = GETDATE()
WHERE StaffID IN (17, 18) AND MeetingID = @MeetingID;

-- Upcoming meetings (just invited, not marked present yet)
SET @MeetingID = (SELECT MeetingID FROM MOM_Meetings WHERE MeetingDescription = 'R&D Project Planning Session');
SET @StaffIDs = '15,16';
EXEC PR_MeetingMember_BulkInsert @MeetingID, @StaffIDs, GETDATE(), GETDATE();

SET @MeetingID = (SELECT MeetingID FROM MOM_Meetings WHERE MeetingDescription = 'Leadership Development Program');
SET @StaffIDs = '5,6,11,14';
EXEC PR_MeetingMember_BulkInsert @MeetingID, @StaffIDs, GETDATE(), GETDATE();

SET @MeetingID = (SELECT MeetingID FROM MOM_Meetings WHERE MeetingDescription = 'Process Improvement Review');
SET @StaffIDs = '14,15';
EXEC PR_MeetingMember_BulkInsert @MeetingID, @StaffIDs, GETDATE(), GETDATE();

SET @MeetingID = (SELECT MeetingID FROM MOM_Meetings WHERE MeetingDescription = 'Creative Brainstorming - Next Campaign Ideas');
SET @StaffIDs = '11,12,13,17';
EXEC PR_MeetingMember_BulkInsert @MeetingID, @StaffIDs, GETDATE(), GETDATE();

SET @MeetingID = (SELECT MeetingID FROM MOM_Meetings WHERE MeetingDescription = 'Monthly Customer Service Team Sync');
SET @StaffIDs = '15,16';
EXEC PR_MeetingMember_BulkInsert @MeetingID, @StaffIDs, GETDATE(), GETDATE();

GO

PRINT '========================================';
PRINT 'Seed Data Insertion Complete!';
PRINT '========================================';
PRINT '';
PRINT 'Default login credentials:';
PRINT '--------------------------------------------------';
PRINT 'Admin:      username=admin,      password=admin123';
PRINT 'Organizer:  username=organizer,  password=org123';
PRINT 'Staff:      username=staff,      password=staff123';
PRINT 'HR Manager: username=hrmanager,  password=hr123';
PRINT 'Finance:    username=financemanager, password=fin123';
PRINT 'Marketing:  username=marketingmanager, password=mkt123';
PRINT '--------------------------------------------------';
GO