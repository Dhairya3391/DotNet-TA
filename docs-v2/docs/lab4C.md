---
title: Lab 4C - Access Modifiers
sidebar_position: 4.3
---

# Access Modifiers

## 1. Description
**Access modifiers** are keywords that set the visibility or accessibility level of types (like classes) and members (like methods and properties). They are a key part of encapsulation in Object-Oriented Programming.

There are four primary access modifiers in C#:
- `public`: The member is accessible from anywhere, without restrictions.
- `private`: The member is only accessible from within the same class.
- `protected`: The member is accessible from within the same class and from classes that derive from it.
- `internal`: The member is only accessible from within the same assembly (a project in Visual Studio).
- `protected internal`: A combination of `protected` and `internal`.

## 2. Why It Is Important
Access modifiers are crucial for creating robust and maintainable code. They allow you to:
- **Enforce Encapsulation:** By hiding the internal implementation details of a class (`private` members) and exposing only what is necessary (`public` members), you create a clear boundary. This prevents other parts of the code from depending on implementation details that might change.
- **Improve Security:** You can prevent unauthorized access to sensitive data or critical methods.
- **Create Clear APIs:** `public` members form the public contract of your class. This makes it easier for other developers to understand how to use your class correctly.

## 3. Real-World Examples
- A `BankAccount` class would have a `public` `Deposit()` method and a `public` `Withdraw()` method. However, the `balance` field would be `private` to prevent direct modification from outside the class. The only way to change the balance is through the public methods, which can enforce rules (like not allowing a negative balance).
- A base class might have a `protected` helper method that is intended for use by its subclasses but not by the general public.
- A library project might use the `internal` modifier for utility classes that are only meant to be used within the library itself and not exposed to the applications that use the library.

## 4. Syntax & Explanation
```csharp
using System;

public class BankAccount
{
    // 'private' field: Can only be accessed by code inside this class.
    private decimal balance;

    // 'public' constructor: Can be called from anywhere.
    public BankAccount(decimal initialBalance)
    {
        balance = initialBalance;
    }

    // 'public' method: Part of the class's public API.
    public void Deposit(decimal amount)
    {
        if (amount > 0)
        {
            balance += amount;
        }
    }

    // 'public' method.
    public void Withdraw(decimal amount)
    {
        if (amount > 0 && amount <= balance)
        {
            balance -= amount;
        }
    }
    
    // 'public' method to get the current balance without allowing direct modification.
    public decimal GetBalance()
    {
        return balance;
    }
    
    // 'private' helper method: Not accessible from outside.
    private void LogTransaction(string message)
    {
        Console.WriteLine($"[LOG]: {message}");
    }
}

class Program
{
    static void Main()
    {
        var account = new BankAccount(100);
        
        // These are allowed because the methods are public
        account.Deposit(50);
        account.Withdraw(20);
        
        // This would cause a compile error because 'balance' is private
        // account.balance = 1000000; 

        Console.WriteLine($"Current Balance: {account.GetBalance()}");
    }
}
```

## 5. Use Cases
- **Hiding Implementation Details:** Use `private` for any fields or methods that are not essential for the outside world to know about. This is the default level of encapsulation and should be your starting point.
- **Creating Public APIs:** Use `public` for the methods, properties, and constructors that you want to expose to other parts of your application or to other developers.
- **Extensibility for Subclasses:** Use `protected` when you want to provide a hook for subclasses to extend or modify the behavior of your class, without making it public to everyone.

## 6. Mini Practice Task
1. Create a `Person` class.
2. Add a `private` field for `_age`.
3. Add a `public` method `SetAge(int age)` that sets the `_age` field, but only if the provided age is between 0 and 120.
4. Add a `public` method `GetAge()` that returns the value of the `_age` field.
5. In your `Main` method, create a `Person` object and try to set a valid age and an invalid age, and print the result.
