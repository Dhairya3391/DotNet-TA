# Lab 12: Stored Procedures Explanation

## Overview

This document explains all stored procedures created for Lab 12, which demonstrates complete CRUD (Create, Read, Update, Delete) operations using ADO.NET and SQL Server stored procedures.

## Database Schema

### Tables Created

#### 1. Country Table
```sql
CREATE TABLE Country (
    CountryID INT IDENTITY(1,1) PRIMARY KEY,
    CountryName NVARCHAR(100) NOT NULL UNIQUE,
    CountryCode NVARCHAR(10) NOT NULL UNIQUE,
    IsActive BIT DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    UpdatedDate DATETIME NULL
);
```

#### 2. State Table
```sql
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
```

#### 3. City Table
```sql
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
```

#### 4. Employee Table (Custom Table - 6+ columns)
```sql
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
```

## Stored Procedures

### Country CRUD Procedures

#### 1. PR_Country_Insert
**Purpose**: Creates a new country record.

**Parameters**:
- `@CountryName` (NVARCHAR(100)): Name of the country
- `@CountryCode` (NVARCHAR(10)): Country code (e.g., 'IN', 'US')
- `@IsActive` (BIT): Status flag (default: 1)

**Returns**:
- `CountryID`: ID of the newly inserted country
- `Message`: Success or error message

**Key Features**:
- Error handling with TRY-CATCH blocks
- Returns identity value of inserted record
- Validates constraints automatically

**Example Usage**:
```sql
EXEC PR_Country_Insert 'Germany', 'DE', 1;
```

#### 2. PR_Country_Update
**Purpose**: Updates an existing country record.

**Parameters**:
- `@CountryID` (INT): ID of the country to update
- `@CountryName` (NVARCHAR(100)): Updated country name
- `@CountryCode` (NVARCHAR(10)): Updated country code
- `@IsActive` (BIT): Updated status

**Returns**:
- `RowsAffected`: Number of rows updated (0 or 1)
- `Message`: Success or error message

**Key Features**:
- Automatically updates `UpdatedDate` timestamp
- Returns count of affected rows
- Handles foreign key constraints

**Example Usage**:
```sql
EXEC PR_Country_Update 1, 'Bharat', 'BH', 1;
```

#### 3. PR_Country_Delete
**Purpose**: Soft-deletes a country by setting IsActive to 0.

**Parameters**:
- `@CountryID` (INT): ID of the country to delete

**Returns**:
- `RowsAffected`: Number of rows deleted
- `Message`: Success or error message

**Key Features**:
- **Soft Delete**: Sets IsActive = 0 instead of hard deletion
- **Business Rule Validation**: Prevents deletion if country has associated states
- **Data Integrity**: Maintains referential integrity

**Example Usage**:
```sql
EXEC PR_Country_Delete 1;
```

### State CRUD Procedures

#### 4. PR_State_Insert
**Purpose**: Creates a new state record.

**Parameters**:
- `@StateName` (NVARCHAR(100)): State name
- `@StateCode` (NVARCHAR(10)): State code
- `@CountryID` (INT): Foreign key to Country table
- `@IsActive` (BIT): Status flag

**Returns**:
- `StateID`: ID of newly inserted state
- `Message`: Success or error message

**Key Features**:
- Validates foreign key relationship
- Enforces unique constraint on (CountryID, StateName)
- Auto-generates timestamps

#### 5. PR_State_Update
**Purpose**: Updates an existing state record.

**Parameters**:
- `@StateID` (INT): ID of state to update
- `@StateName` (NVARCHAR(100)): Updated state name
- `@StateCode` (NVARCHAR(10)): Updated state code
- `@CountryID` (INT): Updated country reference
- `@IsActive` (BIT): Updated status

**Returns**:
- `RowsAffected`: Number of rows updated
- `Message`: Success or error message

#### 6. PR_State_Delete
**Purpose**: Soft-deletes a state record.

**Parameters**:
- `@StateID` (INT): ID of state to delete

**Returns**:
- `RowsAffected`: Number of rows deleted
- `Message`: Success or error message

**Key Features**:
- **Business Rule**: Prevents deletion if state has cities
- **Cascade Logic**: Maintains data hierarchy integrity

### City CRUD Procedures

#### 7. PR_City_Insert
**Purpose**: Creates a new city record.

**Parameters**:
- `@CityName` (NVARCHAR(100)): City name
- `@CityCode` (NVARCHAR(10)): City code
- `@StateID` (INT): Foreign key to State table
- `@IsActive` (BIT): Status flag

**Returns**:
- `CityID`: ID of newly inserted city
- `Message`: Success or error message

#### 8. PR_City_Update
**Purpose**: Updates an existing city record.

**Parameters**:
- `@CityID` (INT): ID of city to update
- `@CityName` (NVARCHAR(100)): Updated city name
- `@CityCode` (NVARCHAR(10)): Updated city code
- `@StateID` (INT): Updated state reference
- `@IsActive` (BIT): Updated status

**Returns**:
- `RowsAffected`: Number of rows updated
- `Message`: Success or error message

#### 9. PR_City_Delete
**Purpose**: Soft-deletes a city record.

**Parameters**:
- `@CityID` (INT): ID of city to delete

**Returns**:
- `RowsAffected`: Number of rows deleted
- `Message`: Success or error message

### Employee CRUD Procedures (Custom Table)

#### 10. PR_Employee_Insert
**Purpose**: Creates a new employee record (demonstrates custom table operations).

**Parameters**:
- `@EmployeeCode` (NVARCHAR(20)): Unique employee code
- `@FirstName` (NVARCHAR(50)): First name
- `@LastName` (NVARCHAR(50)): Last name
- `@Email` (NVARCHAR(100)): Email address
- `@PhoneNumber` (NVARCHAR(20)): Phone number (optional)
- `@Department` (NVARCHAR(50)): Department name
- `@Position` (NVARCHAR(50)): Job position
- `@Salary` (DECIMAL(18,2)): Salary amount
- `@HireDate` (DATE): Date of hiring
- `@IsActive` (BIT): Employment status

**Returns**:
- `EmployeeID`: ID of newly inserted employee
- `Message`: Success or error message

**Key Features**:
- **Data Validation**: Enforces CHECK constraints
- **Business Rules**: Email format validation, positive salary
- **Audit Trail**: Automatic timestamp creation

#### 11. PR_Employee_Update
**Purpose**: Updates an existing employee record.

**Parameters**:
- All employee fields including EmployeeID for identification

**Returns**:
- `RowsAffected`: Number of rows updated
- `Message`: Success or error message

**Key Features**:
- **Comprehensive Update**: Updates all employee fields
- **Audit Trail**: Updates ModifiedDate timestamp
- **Data Integrity**: Maintains all constraints

#### 12. PR_Employee_Delete
**Purpose**: Soft-deletes an employee record.

**Parameters**:
- `@EmployeeID` (INT): ID of employee to delete

**Returns**:
- `RowsAffected`: Number of rows deleted
- `Message`: Success or error message

**Key Features**:
- **Soft Delete**: Preserves employee history
- **Reversible**: Can be reactivated if needed

### Select Procedures (Reference)

#### 13. PR_Country_SelectAll
**Purpose**: Retrieves all active countries.

#### 14. PR_Employee_SelectAll
**Purpose**: Retrieves all employees with full details.

## Design Patterns and Best Practices

### 1. Error Handling
All procedures implement comprehensive error handling:
```sql
BEGIN TRY
    -- Main operation
END TRY
BEGIN CATCH
    SELECT ERROR_NUMBER() AS ErrorNumber,
           ERROR_MESSAGE() AS ErrorMessage;
END CATCH
```

### 2. Soft Delete Pattern
Instead of hard deletion, we use soft delete:
- Sets `IsActive = 0`
- Preserves data for audit and reporting
- Allows recovery if needed
- Maintains referential integrity

### 3. Business Rule Validation
Procedures enforce business rules:
- Prevent deletion of parent records with child dependencies
- Validate data constraints
- Return meaningful error messages

### 4. Audit Trail
Automatic tracking of data changes:
- `CreatedDate`: Set on record creation
- `UpdatedDate`: Set on record modification
- `IsActive`: For soft delete tracking

### 5. Return Value Consistency
All procedures return consistent result sets:
- Success: Affected rows/counters + success message
- Error: Error details + failure message

## Security Considerations

### 1. SQL Injection Prevention
- Uses parameterized queries
- No dynamic SQL construction
- Proper parameter typing

### 2. Data Validation
- CHECK constraints at database level
- Business rule validation in procedures
- Referential integrity enforcement

### 3. Access Control
- Procedures can be granted specific permissions
- Users don't need direct table access
- Controlled data manipulation

## Performance Optimization

### 1. Indexing Strategy
- Primary keys automatically indexed
- Unique constraints on business keys
- Foreign key relationships indexed

### 2. Efficient Queries
- SET NOCOUNT ON reduces network traffic
- Specific field selection (no SELECT *)
- Optimized WHERE clauses

### 3. Transaction Management
- Individual operations are atomic
- Error handling ensures consistency
- Rollback on failure

## Testing and Validation

### 1. Sample Data
- Pre-populated with meaningful test data
- Covers various scenarios
- Includes edge cases

### 2. Procedure Testing
- Built-in test execution in scripts
- Verification of return values
- Error condition testing

### 3. Integration Testing
- Tests foreign key relationships
- Validates business rules
- Checks constraint enforcement

## Usage in ADO.NET

### 1. Connection Management
```csharp
using (SqlConnection connection = new SqlConnection(connectionString))
{
    // Procedure execution
}
```

### 2. Parameter Configuration
```csharp
SqlCommand command = new SqlCommand("PR_Employee_Insert", connection);
command.CommandType = CommandType.StoredProcedure;
command.Parameters.AddWithValue("@FirstName", firstName);
```

### 3. Result Handling
```csharp
SqlDataReader reader = command.ExecuteReader();
while (reader.Read())
{
    // Process results
}
```

## Conclusion

These stored procedures demonstrate:
- **Complete CRUD Operations** for all tables
- **Business Logic Implementation** with validation
- **Error Handling** and user feedback
- **Data Integrity** maintenance
- **Performance Optimization** techniques
- **Security Best Practices**

The implementation follows industry standards and provides a solid foundation for enterprise-level database operations using ADO.NET and stored procedures.