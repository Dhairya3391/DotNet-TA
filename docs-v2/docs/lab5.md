---
title: Lab 5 - Inheritance, Interface & Abstraction
sidebar_position: 5
---

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

### Generic Interface Example
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

// Generic Repository Interface with <T>
public interface IRepository<T> where T : class
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
    IQueryable<T> Query();
}

// Generic Base Entity with Common Properties
public abstract class BaseEntity
{
    public int Id { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }
    public bool IsActive { get; set; } = true;
}

// Generic In-Memory Repository Implementation
public class InMemoryRepository<T> : IRepository<T> where T : BaseEntity, new()
{
    private readonly List<T> _entities = new List<T>();
    private int _nextId = 1;

    public T GetById(int id) => _entities.FirstOrDefault(e => e.Id == id);

    public IEnumerable<T> GetAll() => _entities.Where(e => e.IsActive);

    public void Add(T entity)
    {
        entity.Id = _nextId++;
        entity.CreatedDate = DateTime.Now;
        _entities.Add(entity);
    }

    public void Update(T entity)
    {
        var existing = GetById(entity.Id);
        if (existing != null)
        {
            entity.UpdatedDate = DateTime.Now;
            _entities[_entities.IndexOf(existing)] = entity;
        }
    }

    public void Delete(int id)
    {
        var entity = GetById(id);
        if (entity != null)
        {
            entity.IsActive = false;
            entity.UpdatedDate = DateTime.Now;
        }
    }

    public IQueryable<T> Query() => _entities.AsQueryable();
}

// Domain Models with Inheritance
public abstract class Person : BaseEntity
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }

    public string FullName => $"{FirstName} {LastName}";

    // Abstract method: must be implemented by derived classes
    public abstract string GetRoleDescription();
}

public class Student : Person
{
    public string StudentNumber { get; set; }
    public string Major { get; set; }
    public double GPA { get; set; }

    public override string GetRoleDescription() => $"Student studying {Major}";
}

public class Teacher : Person
{
    public string EmployeeNumber { get; set; }
    public string Department { get; set; }
    public decimal Salary { get; set; }

    public override string GetRoleDescription() => $"Teacher in {Department} department";
}

// Generic Service Interface
public interface IService<T> where T : BaseEntity
{
    T GetById(int id);
    IEnumerable<T> GetAll();
    void Create(T entity);
    void Update(T entity);
    void Delete(int id);
}

// Generic Service Implementation
public class BaseService<T> : IService<T> where T : BaseEntity, new()
{
    protected readonly IRepository<T> _repository;

    public BaseService(IRepository<T> repository)
    {
        _repository = repository;
    }

    public virtual T GetById(int id) => _repository.GetById(id);

    public virtual IEnumerable<T> GetAll() => _repository.GetAll();

    public virtual void Create(T entity)
    {
        // Business logic before creation
        _repository.Add(entity);
    }

    public virtual void Update(T entity)
    {
        // Business logic before update
        _repository.Update(entity);
    }

    public virtual void Delete(int id)
    {
        // Business logic before deletion
        _repository.Delete(id);
    }
}

// Specialized Service for Students
public class StudentService : BaseService<Student>
{
    public StudentService(IRepository<Student> repository) : base(repository) { }

    // Additional student-specific methods
    public IEnumerable<Student> GetStudentsByMajor(string major)
    {
        return _repository.Query()
            .Where(s => s.Major == major && s.IsActive)
            .ToList();
    }

    public IEnumerable<Student> GetHonorStudents()
    {
        return _repository.Query()
            .Where(s => s.GPA >= 3.5 && s.IsActive)
            .ToList();
    }
}

// Generic Logger Interface
public interface ILogger<T>
{
    void LogInfo(string message);
    void LogError(string message, Exception ex = null);
    void LogWarning(string message);
}

// Generic Logger Implementation
public class ConsoleLogger<T> : ILogger<T>
{
    public void LogInfo(string message)
    {
        Console.WriteLine($"[INFO] [{typeof(T).Name}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }

    public void LogError(string message, Exception ex = null)
    {
        Console.WriteLine($"[ERROR] [{typeof(T).Name}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
        if (ex != null) Console.WriteLine($"Exception: {ex.Message}");
    }

    public void LogWarning(string message)
    {
        Console.WriteLine($"[WARN] [{typeof(T).Name}] {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}");
    }
}

// Generic Validation Interface
public interface IValidator<T>
{
    ValidationResult Validate(T entity);
}

public class ValidationResult
{
    public bool IsValid { get; set; }
    public List<string> Errors { get; set; } = new List<string>();
}

// Student Validator Implementation
public class StudentValidator : IValidator<Student>
{
    public ValidationResult Validate(Student student)
    {
        var result = new ValidationResult { IsValid = true };

        if (string.IsNullOrWhiteSpace(student.FirstName))
        {
            result.IsValid = false;
            result.Errors.Add("First name is required");
        }

        if (string.IsNullOrWhiteSpace(student.Email) || !student.Email.Contains("@"))
        {
            result.IsValid = false;
            result.Errors.Add("Valid email is required");
        }

        if (student.GPA < 0 || student.GPA > 4.0)
        {
            result.IsValid = false;
            result.Errors.Add("GPA must be between 0 and 4.0");
        }

        return result;
    }
}

class Program
{
    static void Main()
    {
        // Setup dependencies
        var studentRepository = new InMemoryRepository<Student>();
        var teacherRepository = new InMemoryRepository<Teacher>();
        var studentLogger = new ConsoleLogger<Student>();
        var studentValidator = new StudentValidator();

        var studentService = new StudentService(studentRepository);

        // Create and validate a student
        var student = new Student
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@university.edu",
            StudentNumber = "STU001",
            Major = "Computer Science",
            GPA = 3.8
        };

        // Validate before creating
        var validationResult = studentValidator.Validate(student);
        if (validationResult.IsValid)
        {
            studentService.Create(student);
            studentLogger.LogInfo($"Student {student.FullName} created successfully");
        }
        else
        {
            studentLogger.LogError("Validation failed", null);
            foreach (var error in validationResult.Errors)
            {
                Console.WriteLine($"Error: {error}");
            }
        }

        // Demonstrate polymorphism
        Person[] people = {
            new Student { FirstName = "Alice", LastName = "Smith", Major = "Mathematics" },
            new Teacher { FirstName = "Bob", LastName = "Johnson", Department = "Physics" }
        };

        foreach (var person in people)
        {
            Console.WriteLine($"{person.FullName}: {person.GetRoleDescription()}");
        }

        // Generic operations
        Console.WriteLine("\nAll Students:");
        foreach (var s in studentService.GetAll())
        {
            Console.WriteLine($"{s.FullName} - {s.Major} (GPA: {s.GPA})");
        }
    }
}
```

## 5. Use Cases
- **Generic Data Access**: `IRepository<T>` pattern for working with any entity type
- **Pluggable Services**: Generic logging `ILogger<T>` that can be swapped between console, file, or database loggers
- **Validation Framework**: `IValidator<T>` for consistent validation across different entity types
- **Business Logic Layers**: Generic `IService<T>` base class with specialized implementations
- **Domain Models**: Base entity classes with common properties (Id, CreatedDate, IsActive) inherited by specific entities
- **Polymorphic Operations**: Processing different entity types through common interfaces
- **Testing**: Mock implementations using interfaces for unit testing
- **Dependency Injection**: Generic interfaces make it easy to inject different implementations

## 6. Mini Practice Task
1. **Generic Repository Pattern**:
   - Implement `IRepository<T>` with `Add(T)`, `GetAll()`, `Find(Func<T, bool> predicate)` methods
   - Create both `InMemoryRepository<T>` and `DatabaseRepository<T>` implementations
   - Add constraint `where T : BaseEntity` to ensure all entities have common properties

2. **Abstract Base Classes with Generics**:
   - Create an abstract `EntityService<T>` base class that depends on `IRepository<T>`
   - Implement concrete `UserService<User>` and `ProductService<Product>` classes
   - Add virtual methods that can be overridden for specific business logic

3. **Generic Validation Framework**:
   - Define `IValidator<T>` interface with `Validate(T entity)` method
   - Create validation rules for different entity types (UserValidator, ProductValidator)
   - Implement a `ValidationEngine<T>` that can run multiple validators

4. **Advanced Generic Interfaces**:
   - Create `IRepository<T, TKey>` where TKey is the type of the primary key
   - Implement `ICacheService<T>` with methods like `GetOrAdd(string key, Func<T> factory)`
   - Design `IEventPublisher<TEvent>` for publishing different types of events
