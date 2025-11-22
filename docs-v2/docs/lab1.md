---
title: Lab 1 - Variables, Data Types, Operators
sidebar_position: 1
---

# Variables, Data Types, Operators

## 1. Description
Variables are named storage locations for values. Data types specify what kind of values can be stored (integers, text, booleans, decimals, etc.). Operators are symbols or keywords that perform operations on values (for arithmetic, comparison, logical operations and more).

## 2. Why It Is Important
Every program stores and manipulates data. Knowing variables, types, and operators helps you write correct logic, avoid type-related bugs, and choose the right representations (for example, using `decimal` for money instead of `double`).

## 3. Real-World Examples
- Track a player's score in a game (`int`).
- Store a product price and compute totals (`decimal`).
- Hold a user's name (`string`) and status flags (`bool`).
- Compare timestamps or counts to control program flow.

## 4. Syntax & Explanation
```csharp
using System;

class Program
{
    static void Main()
    {
        // Integer variable
        int lives = 3;

        // Floating point, good for measurements
        double temperature = 21.5;

        // Decimal for financial values (better precision for money)
        decimal price = 19.99m;

        // Text and boolean
        string playerName = "Alex";
        bool isGameOver = false;

        // Arithmetic operators
        int extraLives = 2;
        int totalLives = lives + extraLives; // + operator

        // Comparison operator
        bool needsRefill = price > 100m; // false

        // Logical operators
        bool canContinue = !isGameOver && totalLives > 0; // true

        // Type conversion
        double avgLives = (double)totalLives / 3; // explicit cast

        Console.WriteLine($"Player: {playerName}");
        Console.WriteLine($"Total lives: {totalLives}");
        Console.WriteLine($"Average lives: {avgLives:F2}");
        Console.WriteLine($"Can continue: {canContinue}");
    }
}
```

## 5. Use Cases
- Calculations (totals, averages, balances).
- Flags to control flow (like `isAuthenticated`, `isAdmin`).
- Parsing and converting user input (strings ➜ numbers).
- Validations, comparisons, and conditional logic.

## 6. Mini Practice Task
1. Write a program that reads two integers from the console, prints their sum and their floating-point average.
2. Create a small snippet that asks for a price (`decimal`) and prints whether it is above a given threshold.
