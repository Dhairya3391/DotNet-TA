# Variables, Data Types, Operators

## 1. Description
Variables are named storage locations for values. Data types specify what kind of values can be stored (integers, text, booleans, decimals, etc.). Operators are symbols or keywords that perform operations on values (for arithmetic, comparison, logical operations and more).

## 2. Why It Is Important
Every program stores and manipulates data. Knowing variables, types, and operators helps you write correct logic, avoid type-related bugs, and choose the right representations (for example, using `decimal` for money instead of `double`).

## 3. Real-World Examples
- Track product quantity (`int`), price (`decimal`), name (`string`), in-stock status (`bool`)
- Calculate order totals, apply discounts, and determine shipping costs
- Store customer information and validate ages or account balances

## 4. Syntax & Explanation
**Example: Shopping Cart Calculator**
```csharp
using System;

class Program
{
    static void Main()
    {
        // Variables & Data Types
        int quantity = 3;
        decimal price = 29.99m;        // Use 'm' suffix for decimal (for money!)
        string product = "Laptop";
        bool inStock = true;

        // Operators
        decimal subtotal = price * quantity;      // Arithmetic: 89.97
        decimal tax = subtotal * 0.08m;           // 7.20
        decimal total = subtotal + tax;           // 97.17
        
        bool canBuy = inStock && total > 0;       // Logical AND
        bool needsReview = total > 100 || quantity > 5; // Logical OR

        // Type conversion
        double avg = (double)total / quantity;    // Explicit cast
        if (int.TryParse("10", out int num))      // Safe parsing
            Console.WriteLine($"Parsed: {num}");

        // Output
        Console.WriteLine($"Product: {product}");
        Console.WriteLine($"Subtotal: {subtotal:C}");  // :C for currency format
        Console.WriteLine($"Tax: {tax:C}");
        Console.WriteLine($"Total: {total:C}");
    }
}
```

## 5. Use Cases
- Calculations (totals, averages, balances)
- Flags to control flow (like `isAuthenticated`, `isAdmin`)
- Parsing and converting user input (strings ➜ numbers)
- Validations, comparisons, and conditional logic

**Key Tip**: Always use `decimal` for money (not `double`)! Use `TryParse()` for safe string-to-number conversion.

## 6. Mini Practice Task
1. Write a program that calculates a restaurant bill: meal price + 18% tip + 8% tax.
2. Create a small snippet that reads two integers, prints their sum and floating-point average.
