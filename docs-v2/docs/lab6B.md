---
title: Lab 6B - String Manipulation
sidebar_position: 6.2
---

# String Manipulation

## 1. Description
A **string** in C# is an object that represents a sequence of characters. It's an immutable type, which means that once a string object is created, its value cannot be changed. Operations that appear to modify a string actually create a new string object.

The `System.String` class provides a rich set of methods for creating, manipulating, and comparing strings.

## 2. Why It Is Important
Text processing is a fundamental part of almost every software application. Whether you are handling user input, generating reports, parsing data from a file, or building a user interface, you will be working with strings. Knowing how to effectively manipulate strings is essential for writing correct and efficient code.

## 3. Real-World Examples
- **User Input:** Trimming whitespace from user input, converting it to uppercase or lowercase, and checking if it's empty.
- **Data Parsing:** Splitting a comma-separated value (CSV) string into individual fields.
- **Formatting:** Creating formatted output for display to the user, like `Hello, {username}!`.
- **Validation:** Checking if a string contains a certain character or matches a specific pattern (e.g., a valid email address).

## 4. Syntax & Explanation
```csharp
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        // --- Common String Operations ---

        // Trimming whitespace
        string name = "  Alice  ";
        string trimmedName = name.Trim(); // Result: "Alice"
        Console.WriteLine($"Original: '{name}', Trimmed: '{trimmedName}'");

        // Changing case
        string message = "Hello World";
        string upperMessage = message.ToUpper(); // Result: "HELLO WORLD"
        string lowerMessage = message.ToLower(); // Result: "hello world"
        Console.WriteLine($"Uppercase: {upperMessage}");

        // Substring and Length
        string sentence = "The quick brown fox";
        string sub = sentence.Substring(4, 5); // Starts at index 4, takes 5 chars. Result: "quick"
        Console.WriteLine($"Substring: {sub}");
        Console.WriteLine($"Length of sentence: {sentence.Length}");

        // Replacing text
        string replaced = sentence.Replace("fox", "cat"); // Result: "The quick brown cat"
        Console.WriteLine($"Replaced: {replaced}");
        
        // --- Formatting Strings ---

        // String Interpolation (the recommended way)
        string user = "Bob";
        int score = 95;
        string formatted = $"User: {user}, Score: {score}";
        Console.WriteLine(formatted);

        // --- Splitting and Joining ---

        // Splitting a string into an array
        string csvLine = "one,two,three,four";
        string[] parts = csvLine.Split(',');
        Console.WriteLine("Split parts:");
        foreach (var part in parts)
        {
            Console.WriteLine(part.Trim());
        }

        // Joining an array of strings into a single string
        var words = new[] { "apple", "banana", "cherry" };
        string joined = string.Join(" | ", words); // Result: "apple | banana | cherry"
        Console.WriteLine($"Joined string: {joined}");
        
        // --- Checking for null or empty strings ---
        string emptyStr = "";
        string whitespaceStr = "   ";
        string nullStr = null;

        Console.WriteLine($"Is emptyStr null or empty? {string.IsNullOrEmpty(emptyStr)}"); // True
        Console.WriteLine($"Is whitespaceStr null or whitespace? {string.IsNullOrWhiteSpace(whitespaceStr)}"); // True
        Console.WriteLine($"Is nullStr null or empty? {string.IsNullOrEmpty(nullStr)}"); // True
    }
}
```

## 5. Use Cases
- **Parsing data:** Reading data from files, web services, or user input and extracting the information you need.
- **Building UI:** Constructing the text to display in labels, buttons, and messages.
- **Generating reports:** Creating formatted text or CSV files.
- **Data validation:** Ensuring that user input meets certain criteria (e.g., is not empty, has a certain length).

## 6. Mini Practice Task
1. Ask the user to enter their full name (first and last name separated by a space).
2. Split the full name into two parts: the first name and the last name.
3. Convert both the first name and the last name to uppercase.
4. Print out the initials of the user (the first letter of the first name and the first letter of the last name).
5. Create a welcome message in the format: "Welcome, LASTNAME, FIRSTNAME!".
