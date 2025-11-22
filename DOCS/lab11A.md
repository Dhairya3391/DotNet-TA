# Stored Procedures Basics & CRUD Syntax

## 1. Description
A **Stored Procedure** (SP) is a precompiled set of one or more SQL statements that are stored in a database. It can be executed by calling its name, much like a function in a programming language. Stored procedures can accept input parameters and return output parameters, result sets, or a status value.

They are commonly used to encapsulate the logic for **CRUD** (Create, Read, Update, Delete) operations.

## 2. Why It Is Important
Using stored procedures offers several key benefits:
- **Performance:** SPs are compiled once and stored in the database, leading to faster execution as the query plan is reused.
- **Security:** They help prevent SQL injection attacks because the parameters are treated as data, not as executable code. You can also grant permissions to execute a stored procedure without granting permissions to the underlying tables.
- **Centralized Logic:** Business logic can be encapsulated within the stored procedure. If the logic changes, you only need to update the SP, not all the applications that use it.
- **Reduced Network Traffic:** Instead of sending complex queries over the network, an application only needs to send the name of the SP and its parameters, which can reduce network load.
- **Maintainability:** They help in organizing the database code and making it more modular.

## 3. Real-World Examples
- A `usp_CreateOrder` stored procedure that takes order details as input, creates a new order, and updates the inventory in a single transaction.
- A `usp_GetCustomerDetails` SP that takes a `CustomerId` and returns the customer's information along with their recent orders.
- A nightly batch process that calls a stored procedure like `usp_ArchiveOldData` to move old records to an archive table.

## 4. Syntax & Explanation (T-SQL for SQL Server)

### Create (Insert)
This stored procedure inserts a new product and returns the ID of the new record using an `OUTPUT` parameter.
```sql
CREATE PROCEDURE usp_Product_Create
    -- Input parameters
    @Name NVARCHAR(200),
    @Price DECIMAL(18, 2),
    
    -- Output parameter
    @NewId INT OUTPUT
AS
BEGIN
    -- The SET NOCOUNT ON statement prevents the sending of DONE_IN_PROC messages for each statement in a stored procedure.
    SET NOCOUNT ON;

    INSERT INTO Products (Name, Price, CreatedDate)
    VALUES (@Name, @Price, GETDATE());

    -- SCOPE_IDENTITY() returns the last identity value inserted into an identity column in the same scope.
    SET @NewId = SCOPE_IDENTITY();
END
```

### Read (Select)
This SP retrieves a single product by its ID.
```sql
CREATE PROCEDURE usp_Product_GetById
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT Id, Name, Price, CreatedDate
    FROM Products
    WHERE Id = @Id;
END
```

### Update
This SP updates an existing product's name and price.
```sql
CREATE PROCEDURE usp_Product_Update
    @Id INT,
    @Name NVARCHAR(200),
    @Price DECIMAL(18, 2)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Products
    SET
        Name = @Name,
        Price = @Price,
        UpdatedDate = GETDATE()
    WHERE
        Id = @Id;
END
```

### Delete
This SP deletes a product by its ID.
```sql
CREATE PROCEDURE usp_Product_Delete
    @Id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Products
    WHERE Id = @Id;
END
```

### Calling a Stored Procedure from C# (using ADO.NET)
```csharp
using System.Data;
using System.Data.SqlClient;

// Assume 'connectionString' is defined elsewhere

int newProductId;
using (var connection = new SqlConnection(connectionString))
{
    using (var command = new SqlCommand("usp_Product_Create", connection))
    {
        // Specify that the command is a stored procedure
        command.CommandType = CommandType.StoredProcedure;

        // Add input parameters
        command.Parameters.AddWithValue("@Name", "New Gadget");
        command.Parameters.AddWithValue("@Price", 29.99m);

        // Add the output parameter
        var outParam = new SqlParameter("@NewId", SqlDbType.Int)
        {
            Direction = ParameterDirection.Output
        };
        command.Parameters.Add(outParam);

        // Open the connection and execute the command
        connection.Open();
        command.ExecuteNonQuery();

        // Get the value of the output parameter
        newProductId = (int)outParam.Value;
    }
}

Console.WriteLine($"New product created with ID: {newProductId}");
```

## 5. Mini Practice Task
1. Create a simple `Customers` table with columns for `Id`, `FirstName`, `LastName`, and `Email`.
2. Write a stored procedure named `usp_Customer_Create` that takes `FirstName`, `LastName`, and `Email` as input and inserts a new customer.
3. Write a stored procedure named `usp_Customer_GetByEmail` that takes an `Email` as input and returns the matching customer's record.
4. Test both of your stored procedures using SQL Server Management Studio (SSMS) or a similar tool.
5. (Bonus) Write C# code to call your `usp_Customer_Create` stored procedure.
