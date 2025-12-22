---
title: Lab 3A - Class, Object, & Constructors
sidebar_position: 3.1
---

# Class, Object, & Constructors

## 1. Description
A **class** is a blueprint for creating objects. It defines a set of properties (data) and methods (behaviors) that objects of that type will have. An **object** is an instance of a class, a concrete entity created from the blueprint. A **constructor** is a special method in a class that is called when a new object is created, used to initialize the object's properties.

## 2. Why It Is Important
Classes and objects are the fundamental concepts of Object-Oriented Programming (OOP). They allow you to model real-world entities, organize your code into logical units, and create reusable and maintainable code. Constructors ensure that objects are created in a valid and predictable state.

## 3. Real-World Examples
- A `Car` class can define properties like `Color`, `Model`, and `Speed`, and methods like `Accelerate()` and `Brake()`. Each individual car is an object of the `Car` class.
- A `User` class in a social media application could have properties like `Username`, `Email`, and a list of `Posts`.
- When creating a new `User` object, a constructor would ensure that the `Username` and `Email` are provided and are not empty.

## 4. Syntax & Explanation
```csharp
using System;

// Defining a class named 'Customer'
class Customer
{
    // Properties
    public string Name { get; }
    public int Age { get; set; }

    // Constructor
    public Customer(string name, int age)
    {
        // Basic validation in the constructor
        if (string.IsNullOrWhiteSpace(name))
        {
            // Throw an exception if the name is not valid
            throw new ArgumentException("Name cannot be empty", nameof(name));
        }

        Name = name;
        Age = age;
    }

    // Method
    public void Print()
    {
        Console.WriteLine($"Customer: {Name}, Age: {Age}");
    }
}

class Program
{
    static void Main()
    {
        // Creating an object of the Customer class
        var customer1 = new Customer("Sam", 30);
        customer1.Print(); // Output: Customer: Sam, Age: 30

        // Creating another object
        var customer2 = new Customer("Alice", 25);
        customer2.Age = 26; // Modifying a property
        customer2.Print(); // Output: Customer: Alice, Age: 26
    }
}
```

## 5. Use Cases
- Modeling real-world entities in your application, such as `Product`, `Order`, `Employee`, etc.
- Encapsulating data and behavior into a single unit.
- Creating complex data structures.
- Ensuring that objects are always created in a valid state using constructors.

## 6. Mini Practice Task
1. Create a `Product` class with properties for `Name` (string) and `Price` (decimal).
2. Add a constructor to the `Product` class that accepts the name and price as parameters and initializes the properties.
3. In the constructor, add a validation to ensure that the `Price` is not negative. If it is, throw an `ArgumentException`.
4. Create a few instances of the `Product` class and print their details to the console.
