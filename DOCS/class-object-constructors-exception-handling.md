# Class, Object, Constructors & Exception Handling

## 1. Description
A class is a blueprint for objects. Objects are instances of classes that hold data (fields/properties) and behavior (methods). Constructors initialize new objects. Exception handling (`try`, `catch`, `finally`, `throw`) manages runtime errors in a controlled way.

## 2. Why It Is Important
Classes and objects are the core of object-oriented programming. Constructors ensure objects start in a valid state. Exception handling prevents crashes and lets you provide helpful error messages or recovery steps.

## 3. Real-World Examples
- Model a `BankAccount` class with balance, deposit/withdraw methods, overdraft validation
- Use constructors to ensure minimum deposit requirement (e.g., $100)
- Handle file I/O exceptions, invalid input, insufficient funds scenarios

## 4. Syntax & Explanation
```csharp
using System;
using System.IO;

class BankAccount
{
    public string AccountHolder { get; }
    private decimal balance;

    // Constructor with validation
    public BankAccount(string holder, decimal initialDeposit)
    {
        if (string.IsNullOrWhiteSpace(holder))
            throw new ArgumentException("Account holder name required");
        if (initialDeposit < 100)
            throw new ArgumentException("Minimum deposit is $100");

        AccountHolder = holder;
        balance = initialDeposit;
    }

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Amount must be positive");
        balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount > balance)
            throw new InvalidOperationException("Insufficient funds");
        balance -= amount;
    }

    public void PrintStatement()
    {
        Console.WriteLine($"Account: {AccountHolder}, Balance: {balance:C}");
    }
}

class Program
{
    static void Main()
    {
        try
        {
            var account = new BankAccount("John Doe", 500);
            account.Deposit(100);
            account.Withdraw(50);
            account.PrintStatement();

            // This will throw - insufficient funds
            account.Withdraw(1000);
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine($"Validation error: {ex.Message}");
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Operation failed: {ex.Message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unexpected error: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("Transaction complete.");
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
