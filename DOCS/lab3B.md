# Exception Handling

## 1. Description
**Exception handling** is a mechanism to deal with runtime errors in a controlled and predictable manner. It allows you to "try" a block of code that might fail, and if it does, you can "catch" the specific error (exception) and execute code to handle it. The `finally` block allows you to execute code regardless of whether an error occurred, which is useful for cleanup tasks.

## 2. Why It Is Important
Without exception handling, your program would crash whenever an unexpected error occurs. Proper exception handling makes your application more robust and user-friendly. It allows you to recover from errors, provide meaningful error messages to the user, and ensure that critical resources (like files or network connections) are properly closed.

## 3. Real-World Examples
- Reading a file from the disk: The file might not exist (`FileNotFoundException`), or you might not have permission to read it (`SecurityException`).
- Calling a web API: The network connection might be down (`HttpRequestException`), or the server might return an error.
- Parsing user input: The user might enter text when a number is expected (`FormatException`).

## 4. Syntax & Explanation
```csharp
using System;
using System.IO;

class Program
{
    static void Main()
    {
        // The try-catch-finally block
        try
        {
            // Code that might throw an exception
            string text = File.ReadAllText("nonexistent.txt");
            Console.WriteLine(text);
        }
        catch (FileNotFoundException ex)
        {
            // Handle the specific case where the file is not found
            Console.WriteLine($"Error: The file was not found. ({ex.Message})");
        }
        catch (IOException ex)
        {
            // Handle other I/O related errors
            Console.WriteLine($"An I/O error occurred: {ex.Message}");
        }
        catch (Exception ex)
        {
            // A general catch block for any other exceptions
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
        }
        finally
        {
            // This block always executes, whether an exception occurred or not
            Console.WriteLine("This is the finally block, used for cleanup.");
        }
    }
}
```

## 5. Use Cases
- **Graceful Error Recovery:** Instead of crashing, you can catch an exception and try an alternative action, or inform the user about the problem.
- **Resource Management:** Using `finally` (or the `using` statement, which is a shortcut for a `try`/`finally` block) to ensure that resources like file streams, database connections, and network sockets are always closed, even if an error occurs.
- **Logging and Debugging:** Catching exceptions to log detailed error information, which can be invaluable for debugging.

## 6. Mini Practice Task
1. Write a program that asks the user to enter a number and then parses it to an integer using `int.Parse()`.
2. Wrap the `int.Parse()` call in a `try-catch` block to handle the `FormatException` that occurs if the user enters non-numeric text.
3. In the `catch` block, print a user-friendly error message.
4. Add a `finally` block to print a message like "Parsing attempt finished."
