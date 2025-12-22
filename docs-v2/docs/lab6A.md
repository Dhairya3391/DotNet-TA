---
title: Lab 6A - Collection Classes
sidebar_position: 6.1
---

# Collection Classes

## 1. Description
**Collection classes** are specialized classes designed to store, manage, and manipulate groups of objects. They provide efficient ways to handle tasks like adding, removing, finding, and iterating over items.

Some of the most common collection classes in C# include:
- `List<T>`: A dynamic array that can grow or shrink in size. Good for ordered data that you access by index.
- `Dictionary<TKey, TValue>`: A collection of key-value pairs. Provides very fast lookups based on the key.
- `Queue<T>`: A "First-In, First-Out" (FIFO) collection. You add items to the back and remove them from the front, like a waiting line.
- `Stack<T>`: A "Last-In, First-Out" (LIFO) collection. You add items to the top and remove them from the top, like a stack of plates.
- `HashSet<T>`: A collection of unique items. It's very fast for checking if an item is present in the set.

## 2. Why It Is Important
Almost every application needs to work with groups of data. Choosing the right collection class for the job is crucial for both performance and readability. Using a `Dictionary` for lookups is much faster than searching through a `List`, and using a `Queue` makes it clear that you are processing items in a specific order.

## 3. Real-World Examples
- A `List<Customer>` to hold a list of customers retrieved from a database.
- A `Dictionary<string, Product>` to cache product information, where the key is the product ID.
- A `Queue<string>` to manage a list of print jobs to be processed in order.
- A `Stack<string>` to implement an "undo" feature in an application.
- A `HashSet<string>` to keep track of all the unique visitors to a website.

## 4. Syntax & Explanation
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main()
    {
        // --- List<T> ---
        // A dynamic array of integers.
        var numbers = new List<int> { 1, 10, 5, 2 };
        numbers.Add(6); // Add an item to the end
        numbers.Sort(); // Sorts the list in place
        Console.WriteLine("Numbers in the list:");
        foreach (var num in numbers)
        {
            Console.WriteLine(num);
        }
        Console.WriteLine($"The number at index 2 is {numbers[2]}");


        // --- Dictionary<TKey, TValue> ---
        // A collection of key-value pairs for fast lookups.
        var userScores = new Dictionary<string, int>();
        userScores["Alice"] = 100;
        userScores["Bob"] = 85;

        if (userScores.TryGetValue("Alice", out int score))
        {
            Console.WriteLine($"Alice's score is: {score}");
        }

        // --- Queue<T> ---
        // A First-In, First-Out (FIFO) collection.
        var taskQueue = new Queue<string>();
        taskQueue.Enqueue("Task 1: Process payment");
        taskQueue.Enqueue("Task 2: Send email");
        Console.WriteLine("Processing task: " + taskQueue.Dequeue()); // Dequeues "Task 1"

        // --- Stack<T> ---
        // A Last-In, First-Out (LIFO) collection.
        var historyStack = new Stack<string>();
        historyStack.Push("Visited page A");
        historyStack.Push("Visited page B");
        Console.WriteLine("Last visited page: " + historyStack.Pop()); // Pops "Visited page B"
    }
}
```

## 5. Use Cases
- **`List<T>`**: Use when you need an ordered collection of items that you can access by index. This is the most common general-purpose collection.
- **`Dictionary<TKey, TValue>`**: Use when you need to store and retrieve items based on a unique key. Excellent for lookups, caches, and maps.
- **`Queue<T>`**: Use for processing items in the order they were added, such as message queues, task schedulers, or breadth-first searches.
- **`Stack<T>`**: Use for scenarios where you need to access the most recently added item first, such as implementing undo/redo functionality or depth-first searches.

## 6. Mini Practice Task
1. Create a `Dictionary<string, double>` to store the prices of products. Add a few products and their prices.
2. Ask the user to enter a product name and then print out the price of that product. If the product is not in the dictionary, print a "not found" message.
3. Create a `List<string>` to represent a shopping cart. Allow the user to add a few items to the cart.
4. Finally, loop through the shopping cart and print out all the items.
