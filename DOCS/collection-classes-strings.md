# Collection Classes & Strings

## 1. Description
Collection classes (like `List<T>`, `Dictionary<TKey, TValue>`, `Queue<T>`, `Stack<T>`) store and manage groups of objects. Strings are sequences of characters (`string`) with many helper methods for manipulation and formatting.

## 2. Why It Is Important
Most applications process collections of data (lists of users, lookup maps, message queues). Efficient use of collections and proper string handling are essential for performance and correctness.

## 3. Real-World Examples
- Use `List<Order>` to hold orders fetched from a database.
- Use `Dictionary<string, User>` for fast lookups by username.
- Manipulate and format text for display, logging, or file output.

## 4. Syntax & Explanation
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // List
        var numbers = new List<int> { 1, 2, 3, 4, 5 };
        numbers.Add(6);
        Console.WriteLine("Sum: " + numbers.Sum());

        // Dictionary
        var map = new Dictionary<string, string>();
        map["alice"] = "Alice Smith";
        map["bob"] = "Bob Jones";
        if (map.TryGetValue("alice", out var fullName))
            Console.WriteLine(fullName);

        // Queue and Stack
        var queue = new Queue<string>();
        queue.Enqueue("first");
        queue.Enqueue("second");
        Console.WriteLine("Dequeued: " + queue.Dequeue());

        var stack = new Stack<string>();
        stack.Push("bottom");
        stack.Push("top");
        Console.WriteLine("Popped: " + stack.Pop());

        // String examples
        string name = "  Alice  ";
        string trimmed = name.Trim(); // "Alice"
        string upper = trimmed.ToUpper();
        string formatted = $"Hello, {trimmed}! Length={trimmed.Length}";

        Console.WriteLine(upper);
        Console.WriteLine(formatted);

        // Join and split
        var words = new[] { "one", "two", "three" };
        string joined = string.Join(", ", words);
        var parts = joined.Split(',');
        Console.WriteLine(joined);
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
