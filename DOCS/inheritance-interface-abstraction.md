# Inheritance, Interface & Abstraction

## 1. Description
Inheritance lets a class (derived) reuse and extend another class (base). Interfaces declare contracts (method/property signatures) without implementation. Abstraction hides implementation details and exposes a simple interface for users.

## 2. Why It Is Important
These concepts enable code reuse, polymorphism, and separation of concerns. Interfaces make code testable and decoupled from concrete implementations; inheritance provides a way to share common behavior.

## 3. Real-World Examples
- Payment processing: `IPaymentProcessor` interface with `CreditCardProcessor`, `PayPalProcessor` implementations
- Employee hierarchy: Base `Employee` class with `FullTimeEmployee`, `Contractor` subclasses (different pay calculations)
- Logging system: `ILogger` interface allows swapping console, file, database loggers

## 4. Syntax & Explanation
```csharp
using System;

// Interface for payment processing
interface IPaymentProcessor
{
    bool ProcessPayment(decimal amount, string accountInfo);
}

class CreditCardProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount, string cardNumber)
    {
        Console.WriteLine($"Processing ${amount} via credit card");
        return true; // Simplified - would validate card, call payment gateway
    }
}

class PayPalProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount, string email)
    {
        Console.WriteLine($"Processing ${amount} via PayPal ({email})");
        return true;
    }
}

// Base class with inheritance
abstract class Employee
{
    public string Name { get; set; }
    public Employee(string name) => Name = name;

    // Abstract - each employee type calculates salary differently
    public abstract decimal CalculateMonthlySalary();
}

class FullTimeEmployee : Employee
{
    public decimal AnnualSalary { get; set; }
    public FullTimeEmployee(string name, decimal annual) : base(name)
    {
        AnnualSalary = annual;
    }

    public override decimal CalculateMonthlySalary() => AnnualSalary / 12;
}

class Contractor : Employee
{
    public decimal HourlyRate { get; set; }
    public int HoursWorked { get; set; }
    
    public Contractor(string name, decimal rate) : base(name)
    {
        HourlyRate = rate;
    }

    public override decimal CalculateMonthlySalary() => HourlyRate * HoursWorked;
}

class Program
{
    static void Main()
    {
        // Polymorphism with interfaces
        IPaymentProcessor processor = new CreditCardProcessor();
        processor.ProcessPayment(99.99m, "1234-5678-9012-3456");

        processor = new PayPalProcessor();  // Swap implementation
        processor.ProcessPayment(49.99m, "user@email.com");

        // Polymorphism with inheritance
        Employee emp1 = new FullTimeEmployee("Alice", 60000);
        Employee emp2 = new Contractor("Bob", 50) { HoursWorked = 160 };

        Console.WriteLine($"{emp1.Name}: ${emp1.CalculateMonthlySalary():F2}");
        Console.WriteLine($"{emp2.Name}: ${emp2.CalculateMonthlySalary():F2}");
    }
}
```

## 5. Use Cases
- Pluggable services (logging, data access) using interfaces.
- Domain models that share behavior (base class with shared code).
- Creating test doubles (mocks/stubs) for unit testing.

## 6. Mini Practice Task
1. Define an `IRepository<T>` interface with `Add(T)`, `GetAll()` methods and implement a simple in-memory repository.
2. Create an abstract `Vehicle` class and concrete `Car`/`Bike` classes that implement an abstract `Drive()` method.
