# Conditions & Looping

## 1. Description
Conditions (like `if`, `else`, and `switch`) let a program choose behavior based on values. Looping constructs (`for`, `while`, `do-while`, `foreach`) repeat actions until a condition is met. Together, they control the flow of your program.

## 2. Why It Is Important
Branching and repetition are essential: they let programs react to input, iterate collections, and perform repeated tasks such as processing lists or retrying operations.

## 3. Real-World Examples
- Validating user input and responding with different error messages
- Iterating over a list of orders to calculate totals
- Retrying a failed operation up to 3 times
- Building menu-driven console applications

## 4. Syntax & Explanation
```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        // if / else
        int score = 85;
        if (score >= 90)
            Console.WriteLine("Grade: A");
        else if (score >= 75)
            Console.WriteLine("Grade: B");
        else
            Console.WriteLine("Grade: C or below");

        // switch
        string command = "start";
        switch (command)
        {
            case "start":
                Console.WriteLine("Started");
                break;
            case "stop":
                Console.WriteLine("Stopped");
                break;
            default:
                Console.WriteLine("Unknown command");
                break;
        }

        // for loop - process orders
        decimal[] prices = { 29.99m, 15.50m, 99.00m };
        decimal total = 0;
        for (int i = 0; i < prices.Length; i++)
        {
            total += prices[i];
            Console.WriteLine($"Item {i + 1}: {prices[i]:C}");
        }
        Console.WriteLine($"Total: {total:C}");

        // while loop - retry logic
        int attempts = 0;
        bool success = false;
        while (attempts < 3 && !success)
        {
            attempts++;
            Console.WriteLine($"Attempt {attempts}...");
            // success = TryOperation(); // placeholder
        }

        // foreach loop
        var names = new List<string> { "Alice", "Bob", "Charlie" };
        foreach (var name in names)
            Console.WriteLine($"Hello {name}");
    }
}
```

## 5. Use Cases
- Looping through collections (files, database rows, API results)
- Implementing retry logic and timeouts
- Menu handling and state machines in console apps

## 6. Mini Practice Task
1. Write a program that prints all even numbers from 1 to 20 using a `for` loop.
2. Read numbers until the user types `0`, then print the sum (use `while`).
