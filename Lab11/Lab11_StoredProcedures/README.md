# Lab 11: Stored Procedures (SelectAll and SelectByPK)

## Overview

This lab demonstrates the implementation of ADO.NET with stored procedures in ASP.NET Core MVC. The application performs CRUD operations using stored procedures on an AddressBook database containing Country, State, and City tables.

## Features Implemented

### Core Functionality
- **SelectAll Procedures**: Retrieve all records from each table
- **SelectByPK Procedures**: Retrieve specific records by primary key
- **Filter by City Name**: Search cities using partial name matching
- **Cities by State**: Display all cities belonging to a specific state
- **States with City Count**: Show states along with the number of cities in each

### Database Schema
- **Country**: CountryID, CountryName, CountryCode
- **State**: StateID, StateName, StateCode, CountryID (FK)
- **City**: CityID, CityName, CityCode, StateID (FK)

### Stored Procedures Created
1. `PR_Country_SelectAll` - Returns all countries
2. `PR_Country_SelectByPK` - Returns a specific country by ID
3. `PR_State_SelectAll` - Returns all states with country information
4. `PR_State_SelectByPK` - Returns a specific state by ID
5. `PR_State_SelectWithCityCount` - Returns states with city counts
6. `PR_City_SelectAll` - Returns all cities with state and country information
7. `PR_City_SelectByPK` - Returns a specific city by ID
8. `PR_City_SelectByName` - Filters cities by name (partial search)
9. `PR_City_SelectByState` - Returns cities by state ID

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
   -- Run the script: DatabaseScripts/01_CreateAddressBookDatabase.sql
   -- This will create the AddressBook database and tables
   ```

3. **Execute Stored Procedures Script**
   ```sql
   -- Run the script: DatabaseScripts/02_CreateStoredProcedures.sql
   -- This will create all required stored procedures
   ```

4. **Verify Setup**
   ```sql
   USE AddressBook;
   SELECT COUNT(*) FROM Country;  -- Should return 5
   SELECT COUNT(*) FROM State;    -- Should return 10
   SELECT COUNT(*) FROM City;     -- Should return 25
   ```

### Application Setup

1. **Update Connection String**
   - Edit `appsettings.json`
   - Modify the `DefaultConnection` if needed:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=AddressBook;Trusted_Connection=True;TrustServerCertificate=True;"
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

### Controllers
- `CountryController` - Manages country-related operations
- `StateController` - Manages state-related operations
- `CityController` - Manages city-related operations

### Data Layer
- `DatabaseHelper` - Core ADO.NET database operations
- `CountryRepository` - Country-specific data access
- `StateRepository` - State-specific data access
- `CityRepository` - City-specific data access

### Models
- `Country` - Country entity with validation
- `State` - State entity with navigation properties
- `City` - City entity with navigation properties

## Lab 11 Specific Demonstrations

### 1. Basic Select Operations
- **URL**: `/Country/TestSelectAll` - Tests PR_Country_SelectAll
- **URL**: `/Country/TestSelectByPK/1` - Tests PR_Country_SelectByPK
- **URL**: `/State/TestSelectAll` - Tests PR_State_SelectAll
- **URL**: `/State/TestSelectByPK/1` - Tests PR_State_SelectByPK
- **URL**: `/City/TestSelectAll` - Tests PR_City_SelectAll
- **URL**: `/City/TestSelectByPK/1` - Tests PR_City_SelectByPK

### 2. Lab 11 Requirement Demonstrations
- **Filter by City Name**: `/City/Search?cityName=Ahmedabad`
- **Cities by State**: `/City/ByState/1` (Shows cities for Gujarat)
- **States with City Count**: `/State/StatesWithCityCount`

### 3. Advanced Testing URLs
- **Test City Search**: `/City/TestSearchByName`
- **Test Cities by State**: `/City/TestCitiesByState/1`
- **Test State with City Count**: `/State/TestStateWithCityCount`

## Sample Data

The database is pre-populated with:
- **5 Countries**: India, United States, United Kingdom, Canada, Australia
- **10 States**: 5 Indian states + 5 US states
- **25 Cities**: 5 cities per state

## Key Concepts Demonstrated

### ADO.NET Implementation
- Connection management with `using` statements
- Parameterized queries to prevent SQL injection
- `SqlCommand` with `CommandType.StoredProcedure`
- `SqlDataAdapter` for data retrieval
- Proper exception handling

### Repository Pattern
- Separation of data access logic
- Dependency injection
- Single responsibility principle
- Testable architecture

### MVC Best Practices
- Controller responsibilities
- Model validation with Data Annotations
- Error handling and user feedback
- Clean separation of concerns

## Troubleshooting

### Common Issues

1. **Database Connection Error**
   ```
   A network-related or instance-specific error occurred while establishing a connection to SQL Server
   ```
   **Solution**: Check SQL Server service, verify connection string, ensure SQL Server allows remote connections

2. **Login Failed**
   ```
   Login failed for user 'YOUR_USERNAME'
   ```
   **Solution**: Use `Trusted_Connection=True` for Windows authentication or provide proper credentials

3. **Database Not Found**
   ```
   Cannot open database "AddressBook" requested by the login
   ```
   **Solution**: Run the database creation script first

4. **Stored Procedure Not Found**
   ```
   Could not find stored procedure 'PR_Country_SelectAll'
   ```
   **Solution**: Run the stored procedures creation script

### Debugging Tips

1. **Check Database Connection**
   ```sql
   -- Test connection in SSMS
   USE AddressBook;
   SELECT * FROM Country;
   ```

2. **Verify Stored Procedures**
   ```sql
   -- List all stored procedures
   SELECT name FROM sys.procedures WHERE name LIKE 'PR_%';
   ```

3. **Test Individual Procedures**
   ```sql
   -- Test a specific procedure
   EXEC PR_Country_SelectAll;
   EXEC PR_Country_SelectByPK @CountryID = 1;
   ```

## Learning Objectives

After completing this lab, students should understand:
1. How to create and use stored procedures in SQL Server
2. ADO.NET integration with ASP.NET Core MVC
3. Repository pattern implementation
4. Parameter validation and SQL injection prevention
5. Proper error handling in data access layers
6. Dependency injection for repositories
7. Connection string management
8. DataTable to object mapping

## Next Steps

This lab prepares students for Lab 12, which will cover:
- Insert, Update, Delete stored procedures
- Transaction management
- More complex data operations
- Business logic implementation

## Technical Requirements

- .NET 8.0
- ASP.NET Core MVC
- System.Data.SqlClient (ADO.NET)
- SQL Server 2019 or later
- C# 12.0

---

**Note**: This implementation uses pure ADO.NET as required by the course specifications, avoiding Entity Framework for direct stored procedure interaction.