# Inheritance, Interface & Abstraction

## 1. Description
Inheritance lets a class (derived) reuse and extend another class (base). Interfaces declare contracts (method/property signatures) without implementation. Abstraction hides implementation details and exposes a simple interface for users.

## 2. Why It Is Important
These concepts enable code reuse, polymorphism, and separation of concerns. Interfaces make code testable and decoupled from concrete implementations; inheritance provides a way to share common behavior.

## 3. Real-World Examples
- Implement `IRepository<T>` to abstract data access (file, memory, database).
- Base `Animal` class with derived `Dog` and `Cat` classes.
- Use an interface for `ILogger` so different logging systems can be swapped.

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
