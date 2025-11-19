/*
 * Database: MOM_Database
 * Purpose: Stored Procedures for MOM_User table (Authentication)
 * Author: TA Reference Implementation
 * Date: 2025-01-19
 */

USE MOM_Database;
GO

-- =============================================
-- SP 1: Select User by Username
-- For authentication
-- =============================================
IF OBJECT_ID('PR_User_SelectByUsername', 'P') IS NOT NULL
    DROP PROCEDURE PR_User_SelectByUsername;
GO

CREATE PROCEDURE PR_User_SelectByUsername
    @Username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.UserID,
        u.StaffID,
        u.Username,
        u.Password, -- Note: This should be hashed in production
        u.Role,
        u.IsActive,
        u.LastLogin,
        u.Created,
        s.StaffName,
        s.EmailAddress
    FROM MOM_User u
    LEFT JOIN MOM_Staff s ON u.StaffID = s.StaffID
    WHERE u.Username = @Username
        AND u.IsActive = 1;
END
GO

PRINT 'Procedure PR_User_SelectByUsername created';
GO

-- =============================================
-- SP 2: Select User By Primary Key
-- =============================================
IF OBJECT_ID('PR_User_SelectByPK', 'P') IS NOT NULL
    DROP PROCEDURE PR_User_SelectByPK;
GO

CREATE PROCEDURE PR_User_SelectByPK
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.UserID,
        u.StaffID,
        u.Username,
        u.Password,
        u.Role,
        u.IsActive,
        u.LastLogin,
        u.Created,
        s.StaffName,
        s.EmailAddress,
        d.DepartmentName
    FROM MOM_User u
    LEFT JOIN MOM_Staff s ON u.StaffID = s.StaffID
    LEFT JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
    WHERE u.UserID = @UserID;
END
GO

PRINT 'Procedure PR_User_SelectByPK created';
GO

-- =============================================
-- SP 3: Insert New User (Registration)
-- =============================================
IF OBJECT_ID('PR_User_Insert', 'P') IS NOT NULL
    DROP PROCEDURE PR_User_Insert;
GO

CREATE PROCEDURE PR_User_Insert
    @StaffID INT,
    @Username NVARCHAR(50),
    @Password NVARCHAR(255),
    @Role NVARCHAR(20),
    @Created DATETIME,
    @UserID INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if username already exists
        IF EXISTS (SELECT 1 FROM MOM_User WHERE Username = @Username)
        BEGIN
            RAISERROR('Username already exists', 16, 1);
            RETURN;
        END

        -- Validate role
        IF @Role NOT IN ('Admin', 'Organizer', 'Staff')
        BEGIN
            RAISERROR('Invalid role. Must be Admin, Organizer, or Staff', 16, 1);
            RETURN;
        END

        -- Check if staff exists (if StaffID is provided)
        IF @StaffID IS NOT NULL
        BEGIN
            IF NOT EXISTS (SELECT 1 FROM MOM_Staff WHERE StaffID = @StaffID)
            BEGIN
                RAISERROR('Staff member not found', 16, 1);
                RETURN;
            END

            -- Check if staff member already has a user account
            IF EXISTS (SELECT 1 FROM MOM_User WHERE StaffID = @StaffID)
            BEGIN
                RAISERROR('Staff member already has a user account', 16, 1);
                RETURN;
            END
        END

        INSERT INTO MOM_User
        (
            StaffID,
            Username,
            Password,
            Role,
            IsActive,
            Created
        )
        VALUES
        (
            @StaffID,
            @Username,
            @Password, -- Note: Should be hashed in production
            @Role,
            1, -- Active by default
            @Created
        );

        SET @UserID = SCOPE_IDENTITY();
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_User_Insert created';
GO

-- =============================================
-- SP 4: Update User Profile
-- =============================================
IF OBJECT_ID('PR_User_Update', 'P') IS NOT NULL
    DROP PROCEDURE PR_User_Update;
GO

CREATE PROCEDURE PR_User_Update
    @UserID INT,
    @Username NVARCHAR(50),
    @Role NVARCHAR(20)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if user exists
        IF NOT EXISTS (SELECT 1 FROM MOM_User WHERE UserID = @UserID)
        BEGIN
            RAISERROR('User not found', 16, 1);
            RETURN;
        END

        -- Check for duplicate username (excluding current user)
        IF EXISTS (
            SELECT 1 FROM MOM_User
            WHERE Username = @Username
            AND UserID != @UserID
        )
        BEGIN
            RAISERROR('Username already exists', 16, 1);
            RETURN;
        END

        -- Validate role
        IF @Role NOT IN ('Admin', 'Organizer', 'Staff')
        BEGIN
            RAISERROR('Invalid role. Must be Admin, Organizer, or Staff', 16, 1);
            RETURN;
        END

        UPDATE MOM_User
        SET
            Username = @Username,
            Role = @Role
        WHERE UserID = @UserID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_User_Update created';
GO

-- =============================================
-- SP 5: Update User Password
-- =============================================
IF OBJECT_ID('PR_User_UpdatePassword', 'P') IS NOT NULL
    DROP PROCEDURE PR_User_UpdatePassword;
GO

CREATE PROCEDURE PR_User_UpdatePassword
    @UserID INT,
    @OldPassword NVARCHAR(255),
    @NewPassword NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if user exists
        IF NOT EXISTS (SELECT 1 FROM MOM_User WHERE UserID = @UserID)
        BEGIN
            RAISERROR('User not found', 16, 1);
            RETURN;
        END

        -- Verify old password (simple check - in production, use proper hashing)
        IF NOT EXISTS (
            SELECT 1 FROM MOM_User
            WHERE UserID = @UserID AND Password = @OldPassword
        )
        BEGIN
            RAISERROR('Current password is incorrect', 16, 1);
            RETURN;
        END

        UPDATE MOM_User
        SET Password = @NewPassword
        WHERE UserID = @UserID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_User_UpdatePassword created';
GO

-- =============================================
-- SP 6: Update Last Login
-- Called after successful login
-- =============================================
IF OBJECT_ID('PR_User_UpdateLastLogin', 'P') IS NOT NULL
    DROP PROCEDURE PR_User_UpdateLastLogin;
GO

CREATE PROCEDURE PR_User_UpdateLastLogin
    @UserID INT,
    @LastLogin DATETIME
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if user exists
        IF NOT EXISTS (SELECT 1 FROM MOM_User WHERE UserID = @UserID)
        BEGIN
            RAISERROR('User not found', 16, 1);
            RETURN;
        END

        UPDATE MOM_User
        SET LastLogin = @LastLogin
        WHERE UserID = @UserID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_User_UpdateLastLogin created';
GO

-- =============================================
-- SP 7: Deactivate User
-- Soft delete - mark as inactive
-- =============================================
IF OBJECT_ID('PR_User_Deactivate', 'P') IS NOT NULL
    DROP PROCEDURE PR_User_Deactivate;
GO

CREATE PROCEDURE PR_User_Deactivate
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if user exists
        IF NOT EXISTS (SELECT 1 FROM MOM_User WHERE UserID = @UserID)
        BEGIN
            RAISERROR('User not found', 16, 1);
            RETURN;
        END

        -- Prevent deactivation of the last admin user
        DECLARE @AdminCount INT;
        SELECT @AdminCount = COUNT(*) FROM MOM_User WHERE Role = 'Admin' AND IsActive = 1;

        IF @AdminCount <= 1 AND EXISTS (
            SELECT 1 FROM MOM_User WHERE UserID = @UserID AND Role = 'Admin' AND IsActive = 1
        )
        BEGIN
            RAISERROR('Cannot deactivate the last admin user', 16, 1);
            RETURN;
        END

        UPDATE MOM_User
        SET IsActive = 0
        WHERE UserID = @UserID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_User_Deactivate created';
GO

-- =============================================
-- SP 8: Activate User
-- Reactivate a deactivated user
-- =============================================
IF OBJECT_ID('PR_User_Activate', 'P') IS NOT NULL
    DROP PROCEDURE PR_User_Activate;
GO

CREATE PROCEDURE PR_User_Activate
    @UserID INT
AS
BEGIN
    SET NOCOUNT ON;

    BEGIN TRY
        -- Check if user exists
        IF NOT EXISTS (SELECT 1 FROM MOM_User WHERE UserID = @UserID)
        BEGIN
            RAISERROR('User not found', 16, 1);
            RETURN;
        END

        -- Check if user is already active
        IF EXISTS (SELECT 1 FROM MOM_User WHERE UserID = @UserID AND IsActive = 1)
        BEGIN
            RAISERROR('User is already active', 16, 1);
            RETURN;
        END

        UPDATE MOM_User
        SET IsActive = 1
        WHERE UserID = @UserID;
    END TRY
    BEGIN CATCH
        DECLARE @ErrorMessage NVARCHAR(4000) = ERROR_MESSAGE();
        RAISERROR(@ErrorMessage, 16, 1);
    END CATCH
END
GO

PRINT 'Procedure PR_User_Activate created';
GO

-- =============================================
-- SP 9: Select All Users
-- For admin user management
-- =============================================
IF OBJECT_ID('PR_User_SelectAll', 'P') IS NOT NULL
    DROP PROCEDURE PR_User_SelectAll;
GO

CREATE PROCEDURE PR_User_SelectAll
AS
BEGIN
    SET NOCOUNT ON;

    SELECT
        u.UserID,
        u.StaffID,
        u.Username,
        u.Role,
        u.IsActive,
        u.LastLogin,
        u.Created,
        s.StaffName,
        s.EmailAddress,
        d.DepartmentName,
        CASE
            WHEN u.LastLogin IS NULL THEN 'Never'
            ELSE DATEDIFF(DAY, u.LastLogin, GETDATE())
        END AS DaysSinceLastLogin
    FROM MOM_User u
    LEFT JOIN MOM_Staff s ON u.StaffID = s.StaffID
    LEFT JOIN MOM_Department d ON s.DepartmentID = d.DepartmentID
    ORDER BY u.Created DESC;
END
GO

PRINT 'Procedure PR_User_SelectAll created';
GO

-- =============================================
-- SP 10: Check Username Exists
-- For registration validation
-- =============================================
IF OBJECT_ID('PR_User_CheckUsernameExists', 'P') IS NOT NULL
    DROP PROCEDURE PR_User_CheckUsernameExists;
GO

CREATE PROCEDURE PR_User_CheckUsernameExists
    @Username NVARCHAR(50),
    @ExcludeUserID INT = NULL
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) AS UsernameExists
    FROM MOM_User
    WHERE Username = @Username
        AND (@ExcludeUserID IS NULL OR UserID != @ExcludeUserID);
END
GO

PRINT 'Procedure PR_User_CheckUsernameExists created';
GO

PRINT '========================================';
PRINT 'All User stored procedures created!';
PRINT '========================================';
GO