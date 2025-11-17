# Class, Object, Constructors & Exception Handling

## 1. Description
A class is a blueprint for objects. Objects are instances of classes that hold data (fields/properties) and behavior (methods). Constructors initialize new objects. Exception handling (`try`, `catch`, `finally`, `throw`) manages runtime errors in a controlled way.

## 2. Why It Is Important
Classes and objects are the core of object-oriented programming. Constructors ensure objects start in a valid state. Exception handling prevents crashes and lets you provide helpful error messages or recovery steps.

## 3. Real-World Examples
- Define a `Customer` class with properties and methods.
- Use constructors to set defaults (e.g., an empty cart).
- Catch file I/O exceptions when reading configuration files.

## 4. Syntax & Explanation
```csharp
using System;
using System.IO;

class Customer
{
    public string Name { get; }
    public int Age { get; set; }

    // Constructor
    public Customer(string name, int age)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be empty", nameof(name));

        Name = name;
        Age = age;
    }

    public void Print()
    {
        Console.WriteLine($"Customer: {Name}, Age: {Age}");
    }
}

class Program
{
    static void Main()
    {
        try
        {
            var c = new Customer("Sam", 30);
            c.Print();

            // Example: read a file that might not exist
            var text = File.ReadAllText("nonexistent.txt");
            Console.WriteLine(text);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Invalid argument: {ex.Message}");
        }
        catch (IOException ex)
        {
            Console.WriteLine($"I/O error: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Cleaning up resources or final steps.");
        }
    }
}
```

## 5. Use Cases
- Modeling domain entities (`Order`, `Product`, `User`).
- Ensuring valid initialization with constructors.
- Handling errors (file I/O, network failures, parsing errors).

## 6. Mini Practice Task
1. Create a `Product` class with `Name`, `Price` and a constructor that validates `Price >= 0`.
2. Write code that attempts to open a file and handles `FileNotFoundException` specifically.
