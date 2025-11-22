# Inheritance

## 1. Description
**Inheritance** is a fundamental principle of Object-Oriented Programming (OOP) that allows a class to inherit properties and methods from another class. The class that is being inherited from is called the **base class** (or superclass), and the class that inherits is called the **derived class** (or subclass).

The derived class can reuse the code from the base class, extend it with new functionality, and modify its behavior (through method overriding).

## 2. Why It Is Important
Inheritance is a powerful mechanism for code reuse and for establishing relationships between classes. It allows you to:
- **Avoid Code Duplication:** Common functionality can be placed in a base class and reused across multiple derived classes.
- **Create a Logical Hierarchy:** You can model "is-a" relationships (e.g., a `Dog` is an `Animal`, a `Student` is a `Person`).
- **Enable Polymorphism:** Inheritance is a prerequisite for polymorphism, which allows you to treat objects of different derived classes as objects of the base class.

## 3. Real-World Examples
- A `Vehicle` base class can have properties like `Speed` and `Color`. Derived classes like `Car`, `Truck`, and `Motorcycle` can inherit these properties and add their own specific ones (e.g., `NumberOfDoors` for a `Car`).
- In a UI framework, you might have a base `Control` class with properties like `Position` and `Size`, and derived classes like `Button`, `TextBox`, and `Checkbox`.
- A `Person` class can be a base class for `Student`, `Teacher`, and `Administrator` classes in a university management system.

## 4. Syntax & Explanation
```csharp
using System;

// Base class
public class Person
{
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    public void Greet()
    {
        Console.WriteLine($"Hello, my name is {FullName}.");
    }
}

// Derived class 'Student' inherits from 'Person'
public class Student : Person
{
    public string StudentNumber { get; set; }
    public string Major { get; set; }
}

// Derived class 'Teacher' also inherits from 'Person'
public class Teacher : Person
{
    public string EmployeeNumber { get; set; }
    public string Department { get; set; }
}

class Program
{
    static void Main()
    {
        // Create a Student object
        var student = new Student
        {
            FirstName = "Alice",
            LastName = "Smith",
            Major = "Computer Science"
        };

        // Create a Teacher object
        var teacher = new Teacher
        {
            FirstName = "Bob",
            LastName = "Johnson",
            Department = "Physics"
        };
        
        // Both 'student' and 'teacher' have access to the properties and methods of the 'Person' class.
        Console.WriteLine(student.FullName); // Inherited from Person
        student.Greet();                     // Inherited from Person

        Console.WriteLine(teacher.FullName); // Inherited from Person
        teacher.Greet();                     // Inherited from Person
    }
}
```

## 5. Use Cases
- **Sharing common code:** When you have multiple classes that share the same set of properties or methods.
- **Creating specialized versions of a class:** A derived class is often a more specialized version of the base class.
- **Framework development:** Base classes in frameworks often provide core functionality that developers can extend to fit their specific needs.

## 6. Mini Practice Task
1. Create a base class called `Animal` with a `Name` property and a method `Eat()` that prints a generic message like "{Name} is eating."
2. Create two derived classes, `Dog` and `Cat`, that inherit from `Animal`.
3. Add a `Bark()` method to the `Dog` class and a `Meow()` method to the `Cat` class.
4. In your `Main` method, create instances of `Dog` and `Cat`, set their names, and call both their specific methods (`Bark`/`Meow`) and the inherited method (`Eat`).
