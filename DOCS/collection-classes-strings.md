# Collection Classes & Strings

## 1. Description
Collection classes (like `List<T>`, `Dictionary<TKey, TValue>`, `Queue<T>`, `Stack<T>`) store and manage groups of objects. Strings are sequences of characters (`string`) with many helper methods for manipulation and formatting.

## 2. Why It Is Important
Most applications process collections of data (lists of users, lookup maps, message queues). Efficient use of collections and proper string handling are essential for performance and correctness.

## 3. Real-World Examples
- Store product catalog in `List<Product>`, shopping cart items in `Dictionary<int, int>` (productId -> quantity)
- Use `Queue<Order>` for order processing (first-come, first-served)
- Parse CSV files, validate email formats, format reports

## 4. Syntax & Explanation
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // List - shopping cart
        var cart = new List<string> { "Laptop", "Mouse" };
        cart.Add("Keyboard");
        Console.WriteLine($"Items: {cart.Count}");

        // Dictionary - product prices (fast lookups)
        var prices = new Dictionary<string, decimal>
        {
            ["Laptop"] = 999.99m,
            ["Mouse"] = 29.99m
        };
        if (prices.TryGetValue("Laptop", out var price))
            Console.WriteLine($"Laptop costs: {price:C}");

        // Queue - order processing (FIFO)
        var orders = new Queue<int>();
        orders.Enqueue(1001);  // First order
        orders.Enqueue(1002);  // Second order
        Console.WriteLine($"Processing order: {orders.Dequeue()}");  // 1001

        // String manipulation
        string email = "  user@Example.COM  ";
        email = email.Trim().ToLower();  // "user@example.com"
        
        string csvLine = "John,Doe,30";
        string[] fields = csvLine.Split(',');  // ["John", "Doe", "30"]
        
        var names = new[] { "Alice", "Bob", "Charlie" };
        string formatted = string.Join("; ", names);  // "Alice; Bob; Charlie"
        Console.WriteLine(formatted);
    }
}
```

## 5. Use Cases
- Holding query results and processing them with LINQ.
- Fast lookup tables with `Dictionary`.
- Message buffering with `Queue` or `Stack`.
- Formatting output, parsing user input, and building CSV lines.

## 6. Mini Practice Task
1. Read a comma-separated line of integers and parse into a `List<int>`, then print the sorted list.
2. Given a list of names, produce a single string with names separated by semicolons.
