---
title: Lab 5B - Interfaces
sidebar_position: 5.2
---

# Interfaces

## 1. Description
An **interface** is a contract that defines a set of method and property signatures. It specifies *what* a class can do, but not *how* it does it. A class or struct that **implements** an interface must provide an implementation for all the members defined in that interface.

An interface contains no implementation code, only the declarations of its members.

## 2. Why It Is Important
Interfaces are a cornerstone of modern software design, particularly for achieving loose coupling and dependency inversion. They are crucial for:
- **Decoupling:** Interfaces allow you to separate the definition of a service from its implementation. This means you can change the implementation without affecting the code that uses it.
- **Testability:** When a class depends on an interface, you can easily provide a "mock" or "fake" implementation of that interface for unit testing purposes.
- **Polymorphism:** A class can implement multiple interfaces, allowing it to be treated in different ways depending on the context. This is a form of polymorphism that is more flexible than single inheritance.
- **Pluggable Architectures:** You can swap out different implementations of an interface, allowing for a plug-in-like architecture.

## 3. Real-World Examples
- An `ILogger` interface can define a `Log(string message)` method. You can then have a `ConsoleLogger`, a `FileLogger`, and a `DatabaseLogger`, each implementing the `ILogger` interface. Your application can then be configured to use any of these loggers without changing the core application logic.
- An `IRepository<T>` interface in a data access layer can define methods like `GetAll()`, `GetById(int id)`, `Add(T entity)`, etc. You can have an `EntityFrameworkRepository` that talks to a SQL database, or an `InMemoryRepository` for testing.
- The `.NET` base class library is full of interfaces, such as `IEnumerable` (which allows a collection to be iterated with a `foreach` loop) and `IDisposable` (which provides a mechanism for releasing unmanaged resources).

## 4. Syntax & Explanation
```csharp
using System;
using System.Collections.Generic;

// Interface definition
public interface IRepository<T>
{
    // Method signatures that any implementing class must provide
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
}

// A simple entity class
public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
}

// A concrete implementation of the interface for 'User'
public class InMemoryUserRepository : IRepository<User>
{
    private readonly List<User> _users = new List<User>();
    private int _nextId = 1;

    public void Add(User entity)
    {
        entity.Id = _nextId++;
        _users.Add(entity);
        Console.WriteLine($"User '{entity.Name}' added to in-memory repository.");
    }

    public IEnumerable<User> GetAll()
    {
        return _users;
    }

    public User GetById(int id)
    {
        return _users.Find(u => u.Id == id);
    }
}

// Another implementation, maybe for a database
public class DatabaseUserRepository : IRepository<User>
{
    public void Add(User entity)
    {
        // Code to add the user to a database would go here
        Console.WriteLine($"User '{entity.Name}' added to the database.");
    }

    public IEnumerable<User> GetAll()
    {
        // Code to retrieve all users from the database
        Console.WriteLine("Getting all users from the database.");
        return new List<User>(); // Return a dummy list
    }

    public User GetById(int id)
    {
        // Code to get a user by ID from the database
        Console.WriteLine($"Getting user with ID {id} from the database.");
        return null; // Return a dummy user
    }
}


class Program
{
    static void Main()
    {
        // We code against the interface, not the concrete class.
        // This allows us to easily switch the implementation.
        IRepository<User> userRepository = new InMemoryUserRepository();
        // IRepository<User> userRepository = new DatabaseUserRepository(); // We can swap this out!

        userRepository.Add(new User { Name = "Alice" });
        userRepository.Add(new User { Name = "Bob" });

        foreach (var user in userRepository.GetAll())
        {
            Console.WriteLine($"Found user: {user.Name}");
        }
    }
}
```

## 5. Use Cases
- **Dependency Injection:** Interfaces are the standard way to define services that are injected into other classes.
- **Defining a common API for a set of classes:** Any class that needs to be "loggable" could implement an `ILoggable` interface.
- **Unit Testing:** Creating mock implementations of interfaces to isolate the class you are testing.

## 6. Mini Practice Task
1. Define an interface called `IShape` with a single method signature: `double CalculateArea();`.
2. Create two classes, `Circle` and `Rectangle`, that both implement the `IShape` interface.
3. The `Circle` class should have a `Radius` property, and the `Rectangle` class should have `Width` and `Height` properties.
4. Implement the `CalculateArea()` method in both classes with the correct formula.
5. In your `Main` method, create a list of `IShape` objects and add instances of both `Circle` and `Rectangle` to it.
6. Loop through the list and print the area of each shape.
