# Lab 12: Complete CRUD Operations with Stored Procedures

## Overview

This lab demonstrates comprehensive CRUD (Create, Read, Update, Delete) operations using ADO.NET and SQL Server stored procedures. The application implements a complete employee management system with a custom Employee table featuring 10+ columns as required by the lab specifications.

## Features Implemented

### Complete CRUD Operations
- **Create**: Insert new records with validation and identity return
- **Read**: Retrieve all records, specific records by ID, and search functionality
- **Update**: Modify existing records with proper validation
- **Delete**: Soft delete pattern with business rule validation

### Database Schema
- **Country**: CountryID, CountryName, CountryCode, IsActive, CreatedDate, UpdatedDate
- **State**: StateID, StateName, StateCode, CountryID (FK), IsActive, CreatedDate, UpdatedDate
- **City**: CityID, CityName, CityCode, StateID (FK), IsActive, CreatedDate, UpdatedDate
- **Employee**: EmployeeID, EmployeeCode, FirstName, LastName, Email, PhoneNumber, Department, Position, Salary, HireDate, IsActive, CreatedDate, UpdatedDate

### Custom Employee Table (10+ Columns)
The Employee table demonstrates a real-world business entity with comprehensive fields:
- **Identification**: EmployeeID, EmployeeCode
- **Personal**: FirstName, LastName, Email, PhoneNumber
- **Professional**: Department, Position, Salary, HireDate
- **System**: IsActive, CreatedDate, UpdatedDate

## Stored Procedures Created

### CRUD Procedures (12 total)
1. **Country CRUD**: PR_Country_Insert, PR_Country_Update, PR_Country_Delete
2. **State CRUD**: PR_State_Insert, PR_State_Update, PR_State_Delete
3. **City CRUD**: PR_City_Insert, PR_City_Update, PR_City_Delete
4. **Employee CRUD**: PR_Employee_Insert, PR_Employee_Update, PR_Employee_Delete

### Select Procedures (2 additional)
- PR_Country_SelectAll
- PR_Employee_SelectAll

## Setup Instructions

### Prerequisites
- SQL Server (LocalDB, SQL Server Express, or full SQL Server)
- Visual Studio 2022 or VS Code
- .NET 8 SDK

### Database Setup

1. **Connect to SQL Server**
   ```bash
   sqlcmd -S localhost -E
   ```
   or use SQL Server Management Studio (SSMS)

2. **Execute Database Creation Script**
   ```sql
   -- Run: DatabaseScripts/01_CreateAddressBookDatabase.sql
   -- Creates database, tables, relationships, and sample data
   ```

3. **Execute Stored Procedures Script**
   ```sql
   -- Run: DatabaseScripts/02_CreateStoredProcedures.sql
   -- Creates all 14 stored procedures with error handling
   ```

4. **Verify Setup**
   ```sql
   USE AddressBook;
   EXEC PR_Employee_SelectAll;
   -- Should return 8 sample employee records
   ```

### Application Setup

1. **Update Connection String**
   - Edit `appsettings.json` if needed:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER;Database=AddressBook;Trusted_Connection=True;TrustServerCertificate=True;"
     }
   }
   ```

2. **Build and Run**
   ```bash
   dotnet build
   dotnet run
   ```

3. **Access the Application**
   - Navigate to `https://localhost:XXXX`
   - Default port is usually 5001 or 7XXX

## Application Structure

### Models
- **Employee**: Complete employee entity with validation and computed properties
- **Country, State, City**: Location entities with audit fields
- **CRUDResult**: Standardized result handling for stored procedure operations

### Data Layer
- **DatabaseHelper**: Core ADO.NET operations with comprehensive error handling
- **EmployeeRepository**: Employee-specific CRUD operations
- Repository pattern implementation with dependency injection

### Controllers
- **EmployeeController**: Full CRUD operations with testing capabilities
- Proper error handling and user feedback
- Search and filtering functionality

### Views
- Complete CRUD interface for Employee management
- Responsive Bootstrap design
- Form validation and user feedback

## Key Features Demonstrated

### 1. Complete CRUD Operations
- **Insert**: `PR_Employee_Insert` - Creates new employees with validation
- **Update**: `PR_Employee_Update` - Updates existing employee records
- **Delete**: `PR_Employee_Delete` - Soft deletes with business rules
- **Select**: `PR_Employee_SelectAll` - Retrieves employee data

### 2. Advanced Stored Procedure Features
- **Error Handling**: TRY-CATCH blocks with detailed error messages
- **Business Rules**: Prevents deletion of parent records with dependencies
- **Soft Delete**: Maintains data integrity while hiding inactive records
- **Audit Trail**: Automatic CreatedDate and UpdatedDate tracking
- **Parameter Validation**: Input validation and constraint enforcement

### 3. ADO.NET Best Practices
- **Connection Management**: Proper using statements for resource disposal
- **Parameterized Queries**: Prevents SQL injection attacks
- **Error Handling**: Comprehensive exception handling with user-friendly messages
- **Performance**: Efficient data access with proper indexing

### 4. ASP.NET Core MVC Features
- **Repository Pattern**: Clean separation of data access logic
- **Dependency Injection**: Proper service registration and lifetime management
- **Model Validation**: Server-side validation with Data Annotations
- **Error Handling**: Global error handling and user feedback

## Testing the Application

### 1. Basic CRUD Operations
- Navigate to `/Employee` to view all employees
- Click "Add New Employee" to test Create operation
- Click "Edit" on any employee to test Update operation
- Click "Delete" to test soft delete functionality

### 2. Advanced Testing
- Visit `/Employee/TestCRUD` for automated procedure testing
- Test error scenarios (duplicate emails, invalid data)
- Verify business rule enforcement

### 3. Database Validation
```sql
-- Test stored procedures directly
EXEC PR_Employee_Insert 'TEST001', 'Test', 'User', 'test@example.com', '1234567890', 'IT', 'Tester', 50000.00, GETDATE(), 1;
EXEC PR_Employee_SelectAll;
EXEC PR_Employee_Update 9, 'TEST001', 'Updated', 'User', 'updated@example.com', '1234567890', 'IT', 'Senior Tester', 55000.00, GETDATE(), 1;
EXEC PR_Employee_Delete 9;
```

## Database Scripts

### 1. Database Creation (`01_CreateAddressBookDatabase.sql`)
- Creates AddressBook database
- Creates Country, State, City, and Employee tables
- Establishes relationships and constraints
- Populates sample data for testing

### 2. Stored Procedures (`02_CreateStoredProcedures.sql`)
- Creates all CRUD stored procedures
- Implements comprehensive error handling
- Includes business rule validation
- Provides built-in testing examples

### 3. Documentation (`StoredProcedures_Explained.md`)
- Detailed explanation of all stored procedures
- Design patterns and best practices
- Security considerations
- Performance optimization techniques

## Learning Objectives

After completing this lab, students should understand:

### Database Concepts
1. **Stored Procedure Development**: Creating and managing complex stored procedures
2. **CRUD Operations**: Implementing complete data manipulation operations
3. **Business Logic**: Embedding business rules in database procedures
4. **Error Handling**: Comprehensive error handling in stored procedures
5. **Data Integrity**: Maintaining referential integrity and constraints

### ADO.NET Concepts
1. **Connection Management**: Proper SqlConnection lifecycle management
2. **Parameterized Queries**: Preventing SQL injection with SqlParameter
3. **Command Execution**: SqlCommand with stored procedures
4. **Data Reading**: SqlDataReader and SqlDataAdapter usage
5. **Exception Handling**: Handling database exceptions gracefully

### ASP.NET Core MVC Concepts
1. **Repository Pattern**: Implementing data access repositories
2. **Dependency Injection**: Service registration and lifetime management
3. **Model Validation**: Server-side validation with Data Annotations
4. **CRUD Controllers**: Building complete CRUD controllers
5. **Error Handling**: Application-level error handling and user feedback

## Technical Requirements

- **.NET 8.0**: Latest .NET framework
- **ASP.NET Core MVC**: Web application framework
- **System.Data.SqlClient**: ADO.NET for SQL Server connectivity
- **SQL Server 2019+**: Database server
- **C# 12.0**: Programming language

## Security Best Practices Implemented

### 1. SQL Injection Prevention
- All database operations use parameterized queries
- No dynamic SQL construction
- Proper parameter typing and validation

### 2. Data Validation
- Database-level constraints and CHECK constraints
- Application-level validation with Data Annotations
- Business rule enforcement in stored procedures

### 3. Access Control
- Stored procedures encapsulate data access
- Users don't need direct table permissions
- Controlled data manipulation through procedures

### 4. Audit Trail
- Automatic timestamp tracking for all operations
- Soft delete pattern preserves data history
- Operation logging capabilities

## Performance Optimizations

### 1. Database Design
- Proper indexing on primary and foreign keys
- Unique constraints on business keys
- Optimized queries with specific field selection

### 2. ADO.NET Implementation
- SET NOCOUNT ON reduces network traffic
- Proper connection pooling through using statements
- Efficient data readers for result processing

### 3. Application Architecture
- Repository pattern reduces code duplication
- Dependency injection optimizes service lifetimes
- Caching strategies for frequently accessed data

## Troubleshooting

### Common Issues

1. **Database Connection Error**
   ```
   A network-related or instance-specific error occurred while establishing a connection to SQL Server
   ```
   **Solution**: Check SQL Server service, verify connection string, ensure proper authentication

2. **Stored Procedure Not Found**
   ```
   Could not find stored procedure 'PR_Employee_Insert'
   ```
   **Solution**: Run the stored procedures creation script

3. **Validation Errors**
   ```
   The INSERT statement conflicted with the CHECK constraint
   ```
   **Solution**: Ensure data meets all validation requirements (email format, positive salary, valid hire date)

4. **Business Rule Violations**
   ```
   Cannot delete country. It has associated states.
   ```
   **Solution**: Delete child records first or use cascade delete where appropriate

### Debugging Tips

1. **Check Database Connection**
   ```sql
   -- Test in SSMS
   USE AddressBook;
   SELECT COUNT(*) FROM Employee;
   ```

2. **Verify Stored Procedures**
   ```sql
   -- List all procedures
   SELECT name, create_date, modify_date
   FROM sys.procedures
   WHERE name LIKE 'PR_%'
   ORDER BY name;
   ```

3. **Test Individual Procedures**
   ```sql
   -- Test with known good data
   EXEC PR_Employee_SelectAll;
   ```

## Comparison with Entity Framework

This implementation uses **pure ADO.NET** as required by the course specifications:

| Feature | ADO.NET (This Lab) | Entity Framework |
|---------|-------------------|------------------|
| Control | Maximum control over SQL | Abstracted away |
| Performance | Optimized for specific queries | General optimization |
| Learning Curve | Steeper but more fundamental | Easier but less transparent |
| Debugging | Direct SQL visibility | Abstracted complexity |
| Maintenance | SQL changes require code updates | Model changes update database |

## Extension Ideas

This lab provides a foundation for advanced topics:

1. **Transaction Management**: Implement distributed transactions
2. **Batch Operations**: Bulk insert/update/delete operations
3. **Advanced Searching**: Full-text search and filtering
4. **Reporting**: Complex reporting procedures
5. **Security**: Row-level security and user permissions
6. **Performance**: Query optimization and indexing strategies

---

**Note**: This implementation follows course requirements using pure ADO.NET for direct stored procedure interaction, providing students with fundamental database programming skills essential for enterprise application development.