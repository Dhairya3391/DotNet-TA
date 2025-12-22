---
title: Lab 4B - Method Overriding & Polymorphism
sidebar_position: 4.2
---

# Method Overriding & Polymorphism

## 1. Description
**Method overriding** allows a subclass (or derived class) to provide a specific implementation of a method that is already provided by its superclass (or base class). To override a method, the base class method must be marked as `virtual`, `abstract`, or `override`.

**Polymorphism** is a core OOP concept that allows objects of different classes to be treated as objects of a common superclass. When you call an overridden method on a base class reference, the runtime determines which version of the method to execute based on the object's actual type.

## 2. Why It Is Important
Overriding is the key to enabling polymorphism. This powerful combination allows you to write flexible and extensible code. You can write code that operates on a base class, and it will work correctly with any new classes that derive from that base class, without needing to change the existing code.

## 3. Real-World Examples
- A `Shape` base class has a `virtual Draw()` method. `Circle`, `Square`, and `Triangle` subclasses can all `override` the `Draw()` method to draw themselves correctly. You can then have a list of `Shape` objects and call `Draw()` on each one, and the correct shape will be drawn.
- A game has an `Enemy` base class with a `virtual Attack()` method. Different enemy types like `Goblin` and `Dragon` can override `Attack()` to implement their unique attack behaviors.
- A payment processing system might have a `Payment` base class with a `virtual Process()` method, and subclasses like `CreditCardPayment` and `PayPalPayment` can override it.

## 4. Syntax & Explanation
```csharp
using System;
using System.Collections.Generic;

// Base class
class Logger
{
    // The 'virtual' keyword allows this method to be overridden by derived classes.
    public virtual void Log(string message)
    {
        Console.WriteLine($"[Base Logger] {message}");
    }
}

// Derived class
class FileLogger : Logger
{
    // The 'override' keyword provides a new implementation for the base class method.
    public override void Log(string message)
    {
        Console.WriteLine($"[File Logger] Writing to a file: {message}");
    }
}

// Another derived class
class DatabaseLogger : Logger
{
    public override void Log(string message)
    {
        Console.WriteLine($"[Database Logger] Logging to database: {message}");
    }
}

class Program
{
    static void Main()
    {
        // Polymorphism in action
        // We have a list of Logger objects, but they hold instances of the derived classes.
        List<Logger> loggers = new List<Logger>
        {
            new Logger(),
            new FileLogger(),
            new DatabaseLogger()
        };

        foreach (var logger in loggers)
        {
            // The correct Log() method is called based on the actual object's type at runtime.
            logger.Log("Hello, World!");
        }
    }
}
// Expected Output:
// [Base Logger] Hello, World!
// [File Logger] Writing to a file: Hello, World!
// [Database Logger] Logging to database: Hello, World!
```

## 5. Use Cases
- Implementing different behaviors for subclasses while maintaining a common interface.
- Building plug-in architectures where new functionality can be added by creating new subclasses.
- Creating frameworks where the framework code calls methods on base classes, and the user of the framework provides the implementation in derived classes.

## 6. Mini Practice Task
1. Create a base class `Shape` with a `virtual` method `double Area()` that returns 0.
2. Create two derived classes, `Circle` and `Rectangle`.
3. `Rectangle` should have `Width` and `Height` properties. `Circle` should have a `Radius` property.
4. Override the `Area()` method in both `Circle` and `Rectangle` to calculate and return the correct area. (Area of a circle = π * r^2, Area of a rectangle = width * height).
5. Create a list of `Shape` objects containing both `Circle` and `Rectangle` instances and print out the area of each shape.
