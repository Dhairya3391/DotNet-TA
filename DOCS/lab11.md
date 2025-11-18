# Stored Procedures Basics, CRUD Syntax & Naming Conventions

## 1. Description
Stored procedures (SPs) are precompiled routines stored in the database that encapsulate SQL logic. They are commonly used for CRUD (Create, Read, Update, Delete) operations, batch tasks, and enforcing data-layer rules.

## 2. Why It Is Important
SPs can improve performance, centralize business logic in the database, and make certain operations safer by controlling SQL access. Understanding them helps when integrating ASP.NET applications with relational databases.

## 3. Real-World Examples
- `usp_Product_Create` inserts a product and returns the new ID.
- `usp_Product_GetById` returns a product row.
- `usp_Product_Update` updates fields, often returning success/failure.

## 4. Syntax & Explanation
Example SQL Server SPs (T-SQL):

Create (insert):

```sql
CREATE PROCEDURE usp_Product_Create
    @Name NVARCHAR(200),
    @Price DECIMAL(18,2),
    @NewId INT OUTPUT
AS
BEGIN
    INSERT INTO Products (Name, Price) VALUES (@Name, @Price);
    SET @NewId = SCOPE_IDENTITY();
END
```

Read (select):

```sql
CREATE PROCEDURE usp_Product_GetById
    @Id INT
AS
BEGIN
    SELECT Id, Name, Price FROM Products WHERE Id = @Id;
END
```

Update and Delete follow similar patterns; include error handling and transactions as needed.

Calling from C# (ADO.NET example):

```csharp
using System.Data;
using System.Data.SqlClient;

int newId;
using(var conn = new SqlConnection(connString))
using(var cmd = new SqlCommand("usp_Product_Create", conn))
{
    cmd.CommandType = CommandType.StoredProcedure;
    cmd.Parameters.AddWithValue("@Name", "Pen");
    cmd.Parameters.AddWithValue("@Price", 1.20m);
    var outParam = new SqlParameter("@NewId", SqlDbType.Int) { Direction = ParameterDirection.Output };
    cmd.Parameters.Add(outParam);
    conn.Open();
    cmd.ExecuteNonQuery();
    newId = (int)outParam.Value;
}
```

## 5. Use Cases
- Reusable database logic across multiple apps.
- Batch operations and maintenance tasks.
- Complex joins and logic that benefit from execution on the server.

## 6. Mini Practice Task
1. Write a `usp_Customer_Create` stored procedure that inserts a customer and returns its ID.
2. From C#, call `usp_Customer_GetById` and map the result to a `Customer` object.

## Naming Conventions (suggested)
- Prefix stored procedures with `usp_` or `sp_` (use `usp_` to avoid reserved `sp_` behaviors in SQL Server).
- Use `Entity_Action` style: `usp_Product_Create`, `usp_Product_GetById`, `usp_Order_GetByCustomer`.
- Use clear parameter names and include `Output` for OUT params.
