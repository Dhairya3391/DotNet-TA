# Method Overloading

## 1. Description
**Method overloading** is a feature that allows a class to have multiple methods with the **same name**, but with **different parameter lists**. The difference can be in the number of parameters, the type of parameters, or both. The return type of the method does not play a role in method overloading.

## 2. Why It Is Important
Overloading provides a way to create more intuitive and readable APIs. It allows you to offer different ways to call a method depending on the available input, without having to invent different method names for similar operations. This reduces the cognitive load on the developer using your class.

## 3. Real-World Examples
- The `Console.WriteLine()` method is a perfect example. You can call it with a string, an integer, a double, a boolean, and many other data types. It's the same method name, but overloaded to handle different types.
- A `Calculator` class might have an `Add` method that is overloaded to work with both integers (`Add(int a, int b)`) and doubles (`Add(double a, double b)`).
- A method to create a `User` profile might be overloaded: one version could take just a username, while another could take a username and a profile picture URL.

## 4. Syntax & Explanation
```csharp
using System;

class MathUtils
{
    // Overloaded 'Add' methods
    
    // 1. Adds two integers
    public int Add(int a, int b)
    {
        Console.WriteLine("Called Add(int, int)");
        return a + b;
    }

    // 2. Adds two doubles
    public double Add(double a, double b)
    {
        Console.WriteLine("Called Add(double, double)");
        return a + b;
    }

    // 3. Adds three integers
    public int Add(int a, int b, int c)
    {
        Console.WriteLine("Called Add(int, int, int)");
        return a + b + c;
    }
}

class Program
{
    static void Main()
    {
        var math = new MathUtils();
        
        // The compiler automatically chooses the correct overload based on the arguments
        Console.WriteLine(math.Add(1, 2));       // Calls overload #1
        Console.WriteLine(math.Add(1.5, 2.3));   // Calls overload #2
        Console.WriteLine(math.Add(1, 2, 3));    // Calls overload #3
    }
}
```

## 5. Use Cases
- Providing default values for parameters by creating an overload with fewer arguments.
- Handling different data types for the same logical operation (e.g., adding numbers, printing values).
- Creating more flexible and convenient APIs for other developers.

## 6. Mini Practice Task
1. Create a `Logger` class.
2. Inside the `Logger` class, create two overloaded methods named `Log`.
3. The first `Log` method should accept a single `string` parameter (the message).
4. The second `Log` method should accept a `string` (the message) and a `string` for the log level (e.g., "INFO", "WARNING", "ERROR").
5. Both methods should print the log message to the console, but the second one should also include the log level.
6. Test both methods from your `Main` method.
