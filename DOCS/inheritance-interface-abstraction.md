# Inheritance, Interface & Abstraction

## 1. Description
Inheritance lets a class (derived) reuse and extend another class (base). Interfaces declare contracts (method/property signatures) without implementation. Abstraction hides implementation details and exposes a simple interface for users.

Think of inheritance as a family tree: children inherit traits from parents but can also have their own unique characteristics. Interfaces are like contracts or job descriptions - they define what must be done, but not how to do it.

## 2. Why It Is Important
These concepts enable code reuse, polymorphism, and separation of concerns. Interfaces make code testable and decoupled from concrete implementations; inheritance provides a way to share common behavior.

**Key Benefits**:
- **Code Reuse**: Write common functionality once in base class
- **Polymorphism**: Treat different objects uniformly through common interface/base class
- **Maintainability**: Changes to base class automatically apply to all derived classes
- **Flexibility**: Swap implementations easily with interfaces
- **Testability**: Use interfaces to create mock objects for unit testing
- **Design Patterns**: Essential for many design patterns (Strategy, Factory, Repository)

## 3. Real-World Examples
- **Payment Processing**: `IPaymentProcessor` interface with `CreditCardProcessor`, `PayPalProcessor`, `BitcoinProcessor` implementations
- **Data Access**: `IRepository<T>` interface with `SqlRepository`, `MongoRepository`, `FileRepository` implementations
- **Notification System**: Base `Notification` class with `EmailNotification`, `SMSNotification`, `PushNotification` subclasses
- **Shape Hierarchy**: `Shape` base class with `Circle`, `Rectangle`, `Triangle` - all can calculate area differently
- **Logging System**: `ILogger` interface allows swapping between console, file, database loggers without changing application code
- **Employee System**: Base `Employee` class with `FullTimeEmployee`, `PartTimeEmployee`, `Contractor` subclasses with different pay calculations

## 4. Syntax & Explanation
```csharp
using System;

interface ILogger
{
    void Log(string message);
}

class ConsoleLogger : ILogger
{
    public void Log(string message) => Console.WriteLine($"[Console] {message}");
}

abstract class Animal
{
    public string Name { get; set; }
    public Animal(string name) => Name = name;

    // Abstract method: must be implemented by derived classes
    public abstract void Speak();
}

class Dog : Animal
{
    public Dog(string name) : base(name) { }

    public override void Speak() => Console.WriteLine($"{Name} says: Woof!");
}

class Program
{
    static void Main()
    {
        ILogger logger = new ConsoleLogger();
        logger.Log("Starting app");

        Animal pet = new Dog("Rex");
        pet.Speak();
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
