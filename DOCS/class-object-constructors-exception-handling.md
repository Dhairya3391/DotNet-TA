# Class, Object, Constructors & Exception Handling

## 1. Description
A class is a blueprint for objects. Objects are instances of classes that hold data (fields/properties) and behavior (methods). Constructors initialize new objects. Exception handling (`try`, `catch`, `finally`, `throw`) manages runtime errors in a controlled way.

Think of a class as a cookie cutter (blueprint) and objects as the actual cookies (instances) created from that cutter. Each cookie can have different flavors (property values) but they all follow the same shape (class structure).

## 2. Why It Is Important
Classes and objects are the core of object-oriented programming. Constructors ensure objects start in a valid state. Exception handling prevents crashes and lets you provide helpful error messages or recovery steps.

**Why This Matters**:
- **Code Organization**: Classes group related data and behavior together
- **Reusability**: Create multiple objects from the same class
- **Data Validation**: Constructors ensure objects are always in a valid state
- **Error Recovery**: Exception handling allows graceful failure instead of crashes
- **Maintainability**: Changes to class logic automatically apply to all instances
- **Real-World Modeling**: Classes represent real entities (customers, products, orders)

## 3. Real-World Examples
- **E-commerce**: `Product`, `Customer`, `Order`, `ShoppingCart` classes with validation
- **Banking**: `BankAccount` class with balance, deposit/withdraw methods, and overdraft exceptions
- **School System**: `Student`, `Course`, `Grade` classes with enrollment logic
- **File Processing**: Use exceptions to handle missing files, permission errors gracefully
- **API Integration**: Handle network timeouts, invalid responses with try-catch
- **User Authentication**: Throw exceptions for invalid credentials, expired sessions

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
