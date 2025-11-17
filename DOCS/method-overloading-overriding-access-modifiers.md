# Method Overloading, Overriding & Access Modifiers

## 1. Description
**Method overloading**: defining multiple methods with the same name but different parameters in the same class. **Method overriding**: a derived class provides a different implementation of a virtual method from its base class. **Access modifiers** (`public`, `private`, `protected`, `internal`) control visibility of types and members.

Think of overloading like a restaurant menu - same item name (burger) but different variations (cheese burger, veggie burger, deluxe burger). Overriding is like customizing a recipe - you take the base recipe and make it your own way.

## 2. Why It Is Important
Overloading improves API ergonomics by providing flexible ways to call methods. Overriding enables polymorphism so code can call methods on base types and get derived behavior. Access modifiers enforce encapsulation and help design safe, maintainable classes.

**Key Benefits**:
- **Flexibility**: Overloading lets users call methods with different parameter combinations
- **Readability**: Same method name for similar operations (no need for AddInt, AddDouble, AddString)
- **Polymorphism**: Overriding allows different behavior for different types
- **Security**: Access modifiers prevent unauthorized access to internal implementation
- **Maintainability**: Proper encapsulation makes refactoring safer

## 3. Real-World Examples
- **Method Overloading**: `Console.WriteLine()` accepts string, int, double, object - any type!
- **Constructor Overloading**: Create `DateTime` from year/month/day or from ticks or from string
- **Method Overriding**: Different `ToString()` implementations for different classes
- **Access Modifiers**: Database connection strings are `private`, API methods are `public`
- **Protected Members**: Base validation logic accessible to derived classes but not external code
- **Internal Classes**: Utility classes used only within the same assembly

## 4. Syntax & Explanation
```csharp
using System;

class Logger
{
    public virtual void Log(string message)
    {
        Console.WriteLine($"[Base] {message}");
    }
}

class FileLogger : Logger
{
    public override void Log(string message)
    {
        // Simulate writing to a file (here we print with a tag)
        Console.WriteLine($"[File] {message}");
    }
}

class MathUtils
{
    // Overloaded methods: same name, different parameters
    public int Add(int a, int b) => a + b;
    public double Add(double a, double b) => a + b;
    public int Add(int a, int b, int c) => a + b + c;

    // Access modifiers: private helper
    private void Helper() { /* internal logic */ }
}

class Program
{
    static void Main()
    {
        Logger logger = new FileLogger();
        logger.Log("Hello world"); // Calls override in FileLogger

        var math = new MathUtils();
        Console.WriteLine(math.Add(1, 2));       // 3
        Console.WriteLine(math.Add(1.5, 2.3));   // 3.8
        Console.WriteLine(math.Add(1, 2, 3));    // 6
    }
}
```

## 5. Use Cases
- Provide multiple conveniences for API callers (overloads).
- Implement specific behaviors for derived types (overrides).
- Encapsulate internal logic and expose minimal public surface.

## 6. Mini Practice Task
1. Create a base `Shape` class with a virtual `Area()` method and override it in `Circle` and `Rectangle`.
2. Add overloads of a `Print` method that accept `int`, `double`, and `string`.
