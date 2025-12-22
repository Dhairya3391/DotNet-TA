---
title: Lab 20B - Stored Procedure Naming Conventions
sidebar_position: 20.2
---

# Stored Procedure Naming Conventions

## 1. Description
**Naming conventions** are a set of rules for choosing the character sequence to be used for identifiers (like stored procedures, tables, columns, etc.) in code. A consistent naming convention makes the code more readable, understandable, and maintainable.

For stored procedures, a good naming convention helps to quickly identify the purpose of the procedure and the database object it operates on.

## 2. Why It Is Important
- **Readability:** A consistent naming scheme makes it easier for developers (including your future self) to understand the purpose of a stored procedure at a glance.
- **Maintainability:** When you need to find a stored procedure related to a specific table or action, a good naming convention makes it much faster.
- **Organization:** It helps to group related stored procedures together when they are listed alphabetically.
- **Avoiding Conflicts:** It prevents accidentally giving your stored procedure the same name as a system stored procedure.

## 3. Common Naming Conventions

While there is no single "correct" naming convention, some are more popular and effective than others. A widely used and recommended pattern is:

**`usp_{Schema}_{TableName}_{Action}`**

- **`usp_`**: A prefix to indicate that the object is a user stored procedure. **Note:** Avoid using the `sp_` prefix. In SQL Server, this prefix is reserved for system stored procedures. If a user-created stored procedure starts with `sp_`, SQL Server will first look for it in the master database, which can cause performance issues and unexpected behavior.
- **`{Schema}`**: (Optional but recommended) The schema of the primary table the procedure operates on (e.g., `dbo`, `Sales`, `HR`).
- **`{TableName}`**: The name of the primary database table or entity that the procedure is related to.
- **`{Action}`**: The action that the stored procedure performs. This is often a verb, like `Get`, `Create`, `Update`, `Delete`, `Save`, `Select`, etc.

### Examples

| Stored Procedure Name         | Description                                                |
| ----------------------------- | ---------------------------------------------------------- |
| `usp_Product_GetById`         | Gets a single product by its ID.                           |
| `usp_Product_GetAll`          | Gets all products.                                         |
| `usp_Product_Create`          | Creates a new product.                                     |
| `usp_Product_Update`          | Updates an existing product.                               |
| `usp_Product_Delete`          | Deletes a product.                                         |
| `usp_Order_GetByCustomerId`   | Gets all orders for a specific customer.                   |
| `usp_HR_Employee_Deactivate`  | Deactivates an employee in the HR schema.                  |

### Parameter Naming
- Use the `@` prefix for all parameters (this is required by T-SQL).
- Keep parameter names consistent and descriptive. Often, they will match the column name in the table.
- Example: `@Id`, `@FirstName`, `@Email`.

## 4. Best Practices
- **Be Consistent:** The most important rule is to be consistent. Pick a convention and stick to it throughout your project.
- **Be Descriptive:** The name should clearly describe what the stored procedure does. Avoid overly abbreviated or cryptic names.
- **Use PascalCase:** Use PascalCase (e.g., `GetById`) for the different parts of the name for better readability.
- **Document Your Convention:** If you are working in a team, make sure the naming convention is documented and shared with everyone.

## 5. Mini Practice Task
Based on the `usp_{TableName}_{Action}` naming convention, come up with appropriate names for the following stored procedures:

1. A stored procedure that retrieves all active employees.
2. A stored procedure that updates a customer's shipping address.
3. A stored procedure that deletes an order line item from an order.
4. A stored procedure that searches for products by a keyword.
5. A stored procedure that gets the total sales for a given month.
