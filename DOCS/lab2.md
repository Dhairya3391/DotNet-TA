# Conditions & Looping

## 1. Description
Conditions (like `if`, `else`, and `switch`) let a program choose behavior based on values. Looping constructs (`for`, `while`, `do-while`, `foreach`) repeat actions until a condition is met. Together, they control the flow of your program.

## 2. Why It Is Important
Branching and repetition are essential: they let programs react to input, iterate collections, and perform repeated tasks such as processing lists or retrying operations.

## 3. Real-World Examples
- Validating user input and responding accordingly.
- Iterating over a list of orders to process each one.
- Retrying a network call a fixed number of times.
- Building menu-driven console tools.

## 4. Syntax & Explanation
```csharp
using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        int score = 85;

        // if / else
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

        // for loop
        for (int i = 0; i < 3; i++)
            Console.WriteLine($"for: {i}");

        // while loop
        int n = 3;
        while (n-- > 0)
            Console.WriteLine($"while: {n}");

        // foreach loop
        var names = new List<string> { "Alice", "Bob", "Charlie" };
        foreach (var name in names)
            Console.WriteLine($"Hello {name}");
    }
}
```

## 5. Use Cases
- Looping through collections (files, database rows, API results).
- Implementing retry logic and timeouts.
- Menu handling and state machines in console apps.

## 6. Mini Practice Task
1. Write a program that prints all even numbers from 1 to 20 using a `for` loop.
2. Read numbers until the user types `0`, then print the sum (use `while`).
