---
title: Lab 5C - Abstraction
sidebar_position: 5.3
---

# Abstraction

## 1. Description
**Abstraction** is the concept of hiding complex implementation details and exposing only the essential features of an object or system. It simplifies a complex reality by modeling classes appropriate to the problem.

In C#, abstraction is primarily achieved using **abstract classes** and **interfaces**.

An **abstract class** is a class that cannot be instantiated on its own and is meant to be inherited by other classes. It can contain both abstract methods (with no implementation) and regular methods (with implementation).

## 2. Why It Is Important
Abstraction is key to managing complexity in large software systems. It allows you to:
- **Simplify Complex Systems:** By hiding the "how" and showing only the "what," you make your code easier to understand and use.
- **Improve Maintainability:** Since the implementation details are hidden, you can change them without breaking the code that uses the class, as long as the public interface remains the same.
- **Provide a Common Template:** An abstract class can define a common structure and behavior that all derived classes must share, ensuring consistency.

## 3. Real-World Examples
- The driver of a car doesn't need to know how the engine, transmission, or braking systems work internally. They only need to interact with a simple interface: a steering wheel, pedals, and a gear stick. The complexity is abstracted away.
- When you use a method like `Console.WriteLine()`, you don't need to know how the operating system handles writing characters to the screen. You just use the simple, high-level method.
- A `Database` abstract class could define a `Connect()` and a `Query()` method. Subclasses like `SqlServerDatabase` and `OracleDatabase` would then provide the specific implementation for how to connect and query, while the user of the class just calls the abstract methods.

## 4. Syntax & Explanation
```csharp
using System;

// Abstract base class
// The 'abstract' keyword means this class cannot be instantiated directly.
public abstract class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    // A regular method with implementation, inherited by all subclasses.
    public void DisplayName()
    {
        Console.WriteLine($"Name: {FullName}");
    }

    // An abstract method. It has no implementation here.
    // Any class that inherits from Person MUST provide an implementation for this method.
    public abstract string GetRoleDescription();
}

// A concrete class that inherits from the abstract class
public class Student : Person
{
    public string Major { get; set; }

    // We MUST 'override' the abstract method from the base class.
    public override string GetRoleDescription()
    {
        return $"Student studying {Major}";
    }
}

// Another concrete class
public class Teacher : Person
{
    public string Department { get; set; }

    public override string GetRoleDescription()
    {
        return $"Teacher in the {Department} department";
    }
}


class Program
{
    static void Main()
    {
        // This would cause a compile error: Cannot create an instance of the abstract type 'Person'
        // Person p = new Person();

        // We can create instances of the concrete classes
        Person student = new Student { FirstName = "Alice", LastName = "Smith", Major = "Mathematics" };
        Person teacher = new Teacher { FirstName = "Bob", LastName = "Johnson", Department = "Physics" };
        
        student.DisplayName(); // Calling a non-abstract method from the base class
        Console.WriteLine(student.GetRoleDescription()); // Calling the overridden abstract method

        teacher.DisplayName();
        Console.WriteLine(teacher.GetRoleDescription());
    }
}
```

## 5. Use Cases
- **Sharing common code with a required template:** When you want to provide some common implemented methods but also force subclasses to provide their own implementation for other methods.
- **Creating a base for a component framework:** For example, an abstract `Plugin` class that provides basic lifecycle methods but requires each specific plugin to implement a `Run()` method.
- **When you need a class that has some default behavior but is incomplete on its own.**

## 6. Mini Practice Task
1. Create an abstract class named `Shape`.
2. Give it two `public` properties: `Name` (a string) and `Color` (a string).
3. Add an `abstract` method to it called `GetArea()` that returns a `double`.
4. Add a regular (non-abstract) method called `DisplayInfo()` that prints the name and color of the shape.
5. Create two concrete classes, `Square` and `Triangle`, that inherit from `Shape`.
6. Implement the `GetArea()` method in both `Square` and `Triangle` with the correct area calculation logic.
7. In your `Main` method, create instances of `Square` and `Triangle`, call `DisplayInfo()` on them, and print their areas.
