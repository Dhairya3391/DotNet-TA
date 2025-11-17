# Class, Object, Constructors & Exception Handling

## 1. Description
A class is a blueprint for objects. Objects are instances of classes that hold data (fields/properties) and behavior (methods). Constructors initialize new objects. Exception handling (`try`, `catch`, `finally`, `throw`) manages runtime errors in a controlled way.

Think of a class as a cookie cutter (blueprint) and objects as the actual cookies (instances) created from that cutter. Each cookie can have different flavors (property values) but they all follow the same shape (class structure).

## 2. Why It Is Important
Classes and objects are the core of object-oriented programming. Constructors ensure objects start in a valid state. Exception handling prevents crashes and lets you provide helpful error messages or recovery steps.

**Why This Matters**:
- **Code Organization**: Classes group related data and behavior together
- **Reusability**: Create multiple objects from the same class
- **Data Validation**: Constructors ensure objects are always in a valid state
- **Error Recovery**: Exception handling allows graceful failure instead of crashes
- **Maintainability**: Changes to class logic automatically apply to all instances
- **Real-World Modeling**: Classes represent real entities (customers, products, orders)

## 3. Real-World Examples
- **E-commerce**: `Product`, `Customer`, `Order`, `ShoppingCart` classes with validation
- **Banking**: `BankAccount` class with balance, deposit/withdraw methods, and overdraft exceptions
- **School System**: `Student`, `Course`, `Grade` classes with enrollment logic
- **File Processing**: Use exceptions to handle missing files, permission errors gracefully
- **API Integration**: Handle network timeouts, invalid responses with try-catch
- **User Authentication**: Throw exceptions for invalid credentials, expired sessions

## 4. Syntax & Explanation

### Example 1: Complete Bank Account System with Validation
```csharp
using System;

// ===== CLASS Definition with Properties and Methods =====
public class BankAccount
{
    // ===== PROPERTIES (encapsulate data) =====
    // Auto-implemented properties
    public string AccountNumber { get; private set; }  // Private setter - can't be changed from outside
    public string AccountHolderName { get; set; }
    
    // Backing field for property with logic
    private decimal _balance;
    public decimal Balance 
    { 
        get { return _balance; }
        private set { _balance = value; }  // Private - balance can only change via Deposit/Withdraw
    }
    
    public DateTime CreatedDate { get; private set; }
    public bool IsActive { get; set; }
    
    // Read-only property (no setter)
    public string AccountType { get; }
    
    // Static property - shared across all instances
    public static int TotalAccountsCreated { get; private set; }
    
    // ===== CONSTRUCTORS =====
    
    // Default constructor (parameterless)
    public BankAccount()
    {
        AccountNumber = GenerateAccountNumber();
        AccountType = "Savings";
        CreatedDate = DateTime.Now;
        IsActive = true;
        TotalAccountsCreated++;
    }
    
    // Parameterized constructor
    public BankAccount(string accountHolderName, string accountType, decimal initialDeposit)
    {
        // Validation in constructor
        if (string.IsNullOrWhiteSpace(accountHolderName))
            throw new ArgumentException("Account holder name is required", nameof(accountHolderName));
        
        if (initialDeposit < 0)
            throw new ArgumentException("Initial deposit cannot be negative", nameof(initialDeposit));
        
        if (initialDeposit < 100)
            throw new ArgumentException("Minimum initial deposit is $100", nameof(initialDeposit));
        
        AccountNumber = GenerateAccountNumber();
        AccountHolderName = accountHolderName;
        AccountType = accountType;
        _balance = initialDeposit;
        CreatedDate = DateTime.Now;
        IsActive = true;
        TotalAccountsCreated++;
        
        Console.WriteLine($"✓ Account created: {AccountNumber} for {AccountHolderName}");
    }
    
    // Constructor chaining (calling another constructor)
    public BankAccount(string accountHolderName, decimal initialDeposit) 
        : this(accountHolderName, "Savings", initialDeposit)
    {
        // Calls the main constructor with default "Savings" account type
    }
    
    // ===== METHODS (behavior) =====
    
    public void Deposit(decimal amount)
    {
        // Method-level validation
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive", nameof(amount));
        
        if (!IsActive)
            throw new InvalidOperationException("Cannot deposit to inactive account");
        
        _balance += amount;
        Console.WriteLine($"✓ Deposited {amount:C} | New balance: {Balance:C}");
    }
    
    public void Withdraw(decimal amount)
    {
        // Multiple validation checks
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive", nameof(amount));
        
        if (!IsActive)
            throw new InvalidOperationException("Cannot withdraw from inactive account");
        
        if (amount > _balance)
            throw new InvalidOperationException($"Insufficient funds. Balance: {Balance:C}, Requested: {amount:C}");
        
        _balance -= amount;
        Console.WriteLine($"✓ Withdrawn {amount:C} | New balance: {Balance:C}");
    }
    
    public void Transfer(BankAccount toAccount, decimal amount)
    {
        if (toAccount == null)
            throw new ArgumentNullException(nameof(toAccount));
        
        // Validate before making any changes
        if (amount > _balance)
            throw new InvalidOperationException("Insufficient funds for transfer");
        
        // Perform transfer
        this.Withdraw(amount);
        toAccount.Deposit(amount);
        
        Console.WriteLine($"✓ Transferred {amount:C} to {toAccount.AccountHolderName}");
    }
    
    public void DisplayAccountInfo()
    {
        Console.WriteLine($"\n--- Account Information ---");
        Console.WriteLine($"Account #: {AccountNumber}");
        Console.WriteLine($"Holder: {AccountHolderName}");
        Console.WriteLine($"Type: {AccountType}");
        Console.WriteLine($"Balance: {Balance:C}");
        Console.WriteLine($"Created: {CreatedDate:yyyy-MM-dd}");
        Console.WriteLine($"Status: {(IsActive ? "Active" : "Inactive")}");
    }
    
    // Private helper method (not accessible from outside)
    private string GenerateAccountNumber()
    {
        return $"ACC{DateTime.Now:yyyyMMddHHmmss}{new Random().Next(1000, 9999)}";
    }
    
    // Static method (called on class, not instance)
    public static void DisplayBankStatistics()
    {
        Console.WriteLine($"\n=== Bank Statistics ===");
        Console.WriteLine($"Total Accounts Created: {TotalAccountsCreated}");
    }
}

// ===== EXCEPTION HANDLING Demo =====
class BankingDemo
{
    static void Main()
    {
        Console.WriteLine("===== Bank Account Management System =====\n");
        
        // ===== Creating Objects (Instances) =====
        try
        {
            // Using parameterized constructor
            var account1 = new BankAccount("Alice Johnson", "Checking", 1000m);
            var account2 = new BankAccount("Bob Smith", "Savings", 500m);
            
            // Using constructor chaining (defaults to Savings)
            var account3 = new BankAccount("Charlie Brown", 250m);
            
            account1.DisplayAccountInfo();
            account2.DisplayAccountInfo();
            
            // ===== Normal Operations =====
            Console.WriteLine("\n===== Performing Transactions =====");
            
            account1.Deposit(500m);
            account1.Withdraw(200m);
            account1.Transfer(account2, 300m);
            
            // ===== Handling Invalid Operations =====
            Console.WriteLine("\n===== Testing Error Scenarios =====");
            
            // Scenario 1: Insufficient funds
            try
            {
                Console.WriteLine("\nAttempting to withdraw more than balance...");
                account2.Withdraw(10000m);  // This will throw exception
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"❌ Transaction failed: {ex.Message}");
            }
            
            // Scenario 2: Invalid deposit amount
            try
            {
                Console.WriteLine("\nAttempting to deposit negative amount...");
                account1.Deposit(-50m);  // This will throw exception
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ Invalid input: {ex.Message}");
                Console.WriteLine($"   Parameter: {ex.ParamName}");
            }
            
            // Scenario 3: Creating account with low initial deposit
            try
            {
                Console.WriteLine("\nAttempting to create account with $50 initial deposit...");
                var invalidAccount = new BankAccount("Test User", "Savings", 50m);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"❌ Account creation failed: {ex.Message}");
            }
            
            // Scenario 4: Inactive account operations
            try
            {
                Console.WriteLine("\nAttempting operations on inactive account...");
                account3.IsActive = false;  // Deactivate account
                account3.Deposit(100m);  // This will throw exception
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"❌ Operation failed: {ex.Message}");
            }
            
            // ===== Multiple Exception Types with Finally =====
            Console.WriteLine("\n===== File Operations with Exception Handling =====");
            
            string logFile = "transaction_log.txt";
            System.IO.StreamWriter writer = null;
            
            try
            {
                // This might throw if file path is invalid or permissions issue
                writer = new System.IO.StreamWriter(logFile);
                writer.WriteLine($"Transaction log for {DateTime.Now:yyyy-MM-dd}");
                writer.WriteLine($"Account: {account1.AccountNumber}");
                writer.WriteLine($"Balance: {account1.Balance:C}");
                
                Console.WriteLine($"✓ Log written to {logFile}");
            }
            catch (System.IO.IOException ex)
            {
                Console.WriteLine($"❌ File error: {ex.Message}");
            }
            catch (UnauthorizedAccessException ex)
            {
                Console.WriteLine($"❌ Permission denied: {ex.Message}");
            }
            finally
            {
                // ALWAYS executes - perfect for cleanup
                writer?.Dispose();  // ?. = null-conditional operator
                Console.WriteLine("✓ File resources cleaned up");
            }
            
            // ===== Using Statement (automatic cleanup) =====
            Console.WriteLine("\n===== Better File Handling with 'using' Statement =====");
            
            try
            {
                // 'using' automatically calls Dispose() even if exception occurs
                using (var logWriter = new System.IO.StreamWriter("summary.txt"))
                {
                    logWriter.WriteLine("Account Summary");
                    logWriter.WriteLine($"Total Accounts: {BankAccount.TotalAccountsCreated}");
                }
                Console.WriteLine("✓ Summary file created and closed automatically");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
            
            // ===== Custom Exception Handling =====
            Console.WriteLine("\n===== Processing Multiple Transactions =====");
            
            var transactions = new[] { 100m, -50m, 200m, 999999m, 75m };
            int successCount = 0;
            int failureCount = 0;
            
            foreach (var amount in transactions)
            {
                try
                {
                    account1.Deposit(amount);
                    successCount++;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"❌ Failed to deposit {amount:C}: {ex.Message}");
                    failureCount++;
                    // Continue processing other transactions
                }
            }
            
            Console.WriteLine($"\n✓ Successful transactions: {successCount}");
            Console.WriteLine($"❌ Failed transactions: {failureCount}");
            
            // ===== Final State =====
            Console.WriteLine("\n===== Final Account States =====");
            account1.DisplayAccountInfo();
            account2.DisplayAccountInfo();
            
            // Static method call
            BankAccount.DisplayBankStatistics();
        }
        catch (Exception ex)
        {
            // Catch-all for any unhandled exceptions
            Console.WriteLine($"\n❌ CRITICAL ERROR: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
        finally
        {
            Console.WriteLine("\n===== Program Completed =====");
        }
    }
}
```

**Expected Output** (partial):
```
===== Bank Account Management System =====

✓ Account created: ACC20241117095234567 for Alice Johnson
✓ Account created: ACC20241117095234892 for Bob Smith
✓ Account created: ACC20241117095235123 for Charlie Brown

--- Account Information ---
Account #: ACC20241117095234567
Holder: Alice Johnson
Type: Checking
Balance: $1,000.00
Created: 2024-11-17
Status: Active

===== Performing Transactions =====
✓ Deposited $500.00 | New balance: $1,500.00
✓ Withdrawn $200.00 | New balance: $1,300.00
✓ Withdrawn $300.00 | New balance: $1,000.00
✓ Deposited $300.00 | New balance: $800.00
✓ Transferred $300.00 to Bob Smith

===== Testing Error Scenarios =====

Attempting to withdraw more than balance...
❌ Transaction failed: Insufficient funds. Balance: $800.00, Requested: $10,000.00

Attempting to deposit negative amount...
❌ Invalid input: Deposit amount must be positive
   Parameter: amount

=== Bank Statistics ===
Total Accounts Created: 3
```

### Exception Handling Best Practices

#### Exception Hierarchy (Catch Most Specific First)
```csharp
try
{
    // risky operation
}
catch (ArgumentNullException ex)      // Most specific
{
    // Handle null argument
}
catch (ArgumentException ex)          // More general
{
    // Handle invalid arguments
}
catch (Exception ex)                  // Catch-all (use sparingly)
{
    // Handle any other exception
}
finally
{
    // Always runs - use for cleanup
}
```

#### Common Exception Types
| Exception | When to Use | Example |
|-----------|-------------|---------|
| `ArgumentException` | Invalid method arguments | Negative price, empty string |
| `ArgumentNullException` | Null argument | `if (obj == null) throw...` |
| `InvalidOperationException` | Invalid state | Withdraw from closed account |
| `FormatException` | Parse failures | Invalid date/number format |
| `IOException` | File operations | File not found, permission denied |
| `NullReferenceException` | Accessing null object | Don't throw manually! |

## 5. Use Cases
- **Domain Modeling**: Create classes for `Order`, `Product`, `User`, `Invoice` that model business entities
- **Data Validation**: Use constructors to ensure objects are always in valid state
- **Error Handling**: Gracefully handle file I/O, network failures, database errors, parsing errors
- **Business Logic**: Encapsulate business rules in methods (e.g., CalculateDiscount, ValidateOrder)
- **State Management**: Track object state (IsActive, IsPaid, IsShipped)
- **Resource Management**: Use finally or using statements to clean up files, connections, streams

## 6. Common Pitfalls & Best Practices

### ❌ Common Mistakes:
```csharp
// WRONG: Public fields instead of properties
public class Product
{
    public decimal Price;  // Anyone can set negative price!
}

// WRONG: No validation in constructor
public class Order
{
    public Order(decimal total)
    {
        Total = total;  // Accepts negative!
    }
}

// WRONG: Catching Exception without re-throwing or handling
try
{
    CriticalOperation();
}
catch (Exception)
{
    // Silent failure - very bad!
}

// WRONG: Using exceptions for flow control
try
{
    int value = int.Parse(userInput);
}
catch
{
    value = 0;  // Use TryParse instead!
}
```

### ✅ Best Practices:
```csharp
// CORRECT: Use properties with validation
public class Product
{
    private decimal _price;
    public decimal Price
    {
        get => _price;
        set
        {
            if (value < 0)
                throw new ArgumentException("Price cannot be negative");
            _price = value;
        }
    }
}

// CORRECT: Validate in constructor
public class Order
{
    public decimal Total { get; private set; }
    
    public Order(decimal total)
    {
        if (total < 0)
            throw new ArgumentException("Total cannot be negative", nameof(total));
        Total = total;
    }
}

// CORRECT: Log and re-throw or handle properly
try
{
    CriticalOperation();
}
catch (Exception ex)
{
    Logger.Error("Critical operation failed", ex);
    throw;  // Re-throw to let caller handle
}

// CORRECT: Use TryParse for parsing
if (int.TryParse(userInput, out int value))
{
    // Use value
}
else
{
    Console.WriteLine("Invalid input");
}

// CORRECT: Use 'using' for IDisposable resources
using (var file = File.OpenRead("data.txt"))
{
    // File automatically closed even if exception occurs
}
```

### Constructor Best Practices
1. **Keep constructors simple** - do initialization, not complex logic
2. **Validate all parameters** - throw ArgumentException for invalid input
3. **Use constructor chaining** - avoid duplicating initialization code
4. **Initialize all fields** - don't leave fields with default values unless intentional
5. **Consider factory methods** - for complex creation logic

### Exception Handling Best Practices
1. **Catch specific exceptions** - don't catch Exception unless necessary
2. **Use finally for cleanup** - or use 'using' statement
3. **Don't swallow exceptions** - always log or re-throw
4. **Include helpful messages** - tell user what went wrong and why
5. **Use custom exceptions** - for domain-specific errors
6. **Don't throw NullReferenceException** - use ArgumentNullException instead

## 7. Mini Practice Tasks

### Task 1: Create a Product Inventory System
Build a `Product` class that:
1. Has properties: Id, Name, Price, StockQuantity, Category
2. Constructor validates:
   - Name is not empty
   - Price >= 0
   - StockQuantity >= 0
3. Methods:
   - `AddStock(int quantity)` - adds to stock, throws if quantity <= 0
   - `RemoveStock(int quantity)` - removes from stock, throws if insufficient stock
   - `ApplyDiscount(decimal percentage)` - reduces price, throws if percentage not 0-100
4. Display method that shows all product info

**Test with**:
- Valid product creation
- Invalid price (negative)
- Removing more stock than available

### Task 2: Student Course Enrollment System
Create `Student` and `Course` classes:

**Student class**:
- Properties: StudentId, Name, Email, EnrolledCourses (List<string>)
- Constructor validates email contains '@'
- Methods:
  - `EnrollInCourse(string courseName)` - adds course, max 5 courses
  - `DropCourse(string courseName)` - removes course, throw if not enrolled
  - `DisplayEnrollment()` - shows all courses

**Course class**:
- Properties: CourseCode, Name, Capacity, EnrolledStudents (int)
- Methods:
  - `AddStudent()` - increments enrolled, throws if at capacity
  - `RemoveStudent()` - decrements enrolled

**Exception Scenarios**:
- Enrolling in more than 5 courses
- Enrolling in full course
- Dropping course not enrolled in

### Task 3: Temperature Monitor with Exception Handling
Create a `TemperatureMonitor` class:
1. Properties: CurrentTemp, MinTemp, MaxTemp, Unit (C or F)
2. Constructor sets valid ranges based on unit
3. Method `RecordTemperature(double temp)`:
   - Throws if temp outside safe range (-50 to 50 for C, -58 to 122 for F)
   - Stores temperature in list
4. Method `GetAverage()` - calculates average of all readings
5. Method `ReadFromFile(string filename)`:
   - Read temps from file
   - Use try-catch for FileNotFoundException, FormatException
   - Continue processing even if some lines are invalid

**Expected Output**:
```
✓ Recorded: 22.5°C
✓ Recorded: 25.0°C
❌ Error: Temperature 100.0°C exceeds safe range
✓ Average temperature: 23.8°C
```

### Task 4: Bank Transaction Logger
Enhance the BankAccount class to:
1. Keep a list of all transactions (type, amount, date)
2. Method `GetTransactionHistory()` - returns list
3. Method `ExportToFile(string filename)`:
   - Write all transactions to file
   - Handle IOException, UnauthorizedAccessException
   - Use finally to ensure file is closed
4. Method `ImportFromFile(string filename)`:
   - Read transactions from file
   - Handle file not found
   - Continue processing even if some lines are invalid
   - Return count of successfully imported transactions

**Skills practiced**: Classes, collections, file I/O, exception handling, finally blocks
