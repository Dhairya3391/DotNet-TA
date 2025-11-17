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

### Example 1: Employee Management System (Inheritance & Polymorphism)
```csharp
using System;
using System.Collections.Generic;

// ===== BASE (PARENT) CLASS =====
public abstract class Employee
{
    // Common properties for all employees
    public int EmployeeId { get; set; }
    public string Name { get; set; }
    public string Email { get; set; }
    public DateTime HireDate { get; set; }
    
    // Protected - accessible to derived classes but not external code
    protected decimal baseSalary;
    
    // Constructor
    public Employee(int id, string name, string email, decimal baseSalary)
    {
        EmployeeId = id;
        Name = name;
        Email = email;
        this.baseSalary = baseSalary;
        HireDate = DateTime.Now;
    }
    
    // Abstract method - MUST be implemented by derived classes
    public abstract decimal CalculateMonthlySalary();
    
    // Abstract method for bonuses
    public abstract decimal CalculateBonus();
    
    // Virtual method - CAN be overridden by derived classes (optional)
    public virtual void DisplayInfo()
    {
        Console.WriteLine($"ID: {EmployeeId} | Name: {Name}");
        Console.WriteLine($"Email: {Email} | Hired: {HireDate:yyyy-MM-dd}");
    }
    
    // Regular method - shared by all employees
    public int GetYearsOfService()
    {
        return DateTime.Now.Year - HireDate.Year;
    }
}

// ===== DERIVED (CHILD) CLASS #1 =====
public class FullTimeEmployee : Employee
{
    public decimal AnnualSalary { get; set; }
    public int VacationDays { get; set; }
    
    // Constructor - calls base class constructor
    public FullTimeEmployee(int id, string name, string email, decimal annualSalary)
        : base(id, name, email, annualSalary)  // Call parent constructor
    {
        AnnualSalary = annualSalary;
        VacationDays = 20;  // Default vacation days
    }
    
    // Implementing abstract method (required)
    public override decimal CalculateMonthlySalary()
    {
        return AnnualSalary / 12;
    }
    
    // Implementing abstract method
    public override decimal CalculateBonus()
    {
        // Full-time gets 10% annual bonus
        return AnnualSalary * 0.10m;
    }
    
    // Overriding virtual method (optional)
    public override void DisplayInfo()
    {
        base.DisplayInfo();  // Call parent version first
        Console.WriteLine($"Type: Full-Time");
        Console.WriteLine($"Annual Salary: {AnnualSalary:C}");
        Console.WriteLine($"Monthly Salary: {CalculateMonthlySalary():C}");
        Console.WriteLine($"Vacation Days: {VacationDays}");
    }
}

// ===== DERIVED CLASS #2 =====
public class PartTimeEmployee : Employee
{
    public decimal HourlyRate { get; set; }
    public int HoursPerWeek { get; set; }
    
    public PartTimeEmployee(int id, string name, string email, decimal hourlyRate, int hoursPerWeek)
        : base(id, name, email, hourlyRate * hoursPerWeek * 4)  // Estimate base
    {
        HourlyRate = hourlyRate;
        HoursPerWeek = hoursPerWeek;
    }
    
    public override decimal CalculateMonthlySalary()
    {
        // Part-time: hourly rate × hours per week × 4 weeks
        return HourlyRate * HoursPerWeek * 4;
    }
    
    public override decimal CalculateBonus()
    {
        // Part-time gets smaller bonus - 5% of annual equivalent
        return (HourlyRate * HoursPerWeek * 52) * 0.05m;
    }
    
    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Type: Part-Time");
        Console.WriteLine($"Hourly Rate: {HourlyRate:C}");
        Console.WriteLine($"Hours/Week: {HoursPerWeek}");
        Console.WriteLine($"Monthly Salary: {CalculateMonthlySalary():C}");
    }
}

// ===== DERIVED CLASS #3 =====
public class Contractor : Employee
{
    public decimal ProjectRate { get; set; }
    public DateTime ContractEndDate { get; set; }
    
    public Contractor(int id, string name, string email, decimal projectRate, DateTime endDate)
        : base(id, name, email, projectRate)
    {
        ProjectRate = projectRate;
        ContractEndDate = endDate;
    }
    
    public override decimal CalculateMonthlySalary()
    {
        return ProjectRate;  // Flat monthly project rate
    }
    
    public override decimal CalculateBonus()
    {
        // Contractors don't get bonuses
        return 0;
    }
    
    public override void DisplayInfo()
    {
        base.DisplayInfo();
        Console.WriteLine($"Type: Contractor");
        Console.WriteLine($"Project Rate: {ProjectRate:C}/month");
        Console.WriteLine($"Contract Ends: {ContractEndDate:yyyy-MM-dd}");
        
        if (ContractEndDate < DateTime.Now)
        {
            Console.WriteLine("⚠ Contract EXPIRED");
        }
    }
}

// ===== DEMO: POLYMORPHISM =====
class InheritanceDemo
{
    static void Main()
    {
        Console.WriteLine("===== Employee Management System =====\n");
        
        // ===== Creating different types of employees =====
        var employees = new List<Employee>
        {
            new FullTimeEmployee(1, "Alice Johnson", "alice@company.com", 75000m),
            new PartTimeEmployee(2, "Bob Smith", "bob@company.com", 25m, 20),
            new Contractor(3, "Charlie Brown", "charlie@freelance.com", 8000m, DateTime.Now.AddMonths(6)),
            new FullTimeEmployee(4, "Diana Prince", "diana@company.com", 95000m),
            new PartTimeEmployee(5, "Eve Wilson", "eve@company.com", 30m, 15)
        };
        
        // ===== POLYMORPHISM: Treating all employees uniformly =====
        decimal totalMonthlyCost = 0;
        decimal totalAnnualBonus = 0;
        
        foreach (Employee emp in employees)  // Employee is the base type
        {
            Console.WriteLine("\n" + new string('-', 50));
            emp.DisplayInfo();  // Calls appropriate override
            
            decimal monthly = emp.CalculateMonthlySalary();  // Calls appropriate implementation
            decimal bonus = emp.CalculateBonus();
            
            totalMonthlyCost += monthly;
            totalAnnualBonus += bonus;
            
            Console.WriteLine($"Bonus: {bonus:C}");
            Console.WriteLine($"Years of Service: {emp.GetYearsOfService()}");
        }
        
        // ===== Summary =====
        Console.WriteLine("\n" + new string('=', 50));
        Console.WriteLine("===== PAYROLL SUMMARY =====");
        Console.WriteLine($"Total Employees: {employees.Count}");
        Console.WriteLine($"Total Monthly Cost: {totalMonthlyCost:C}");
        Console.WriteLine($"Total Annual Bonuses: {totalAnnualBonus:C}");
        Console.WriteLine($"Total Annual Cost: {(totalMonthlyCost * 12 + totalAnnualBonus):C}");
        
        // ===== Type Checking and Casting =====
        Console.WriteLine("\n===== Full-Time Employees Only =====");
        foreach (Employee emp in employees)
        {
            // Check if employee is full-time
            if (emp is FullTimeEmployee fullTime)  // Pattern matching (C# 7+)
            {
                Console.WriteLine($"{fullTime.Name}: {fullTime.VacationDays} vacation days");
            }
        }
    }
}
```

### Example 2: Payment Processing System (Interfaces)
```csharp
using System;

// ===== INTERFACE DEFINITION =====
// Interface = contract that implementing classes must follow
public interface IPaymentProcessor
{
    // Method signatures only - no implementation
    bool ProcessPayment(decimal amount, string accountInfo);
    bool RefundPayment(string transactionId, decimal amount);
    string GetPaymentMethod();
}

// Optional: Interface with properties
public interface IPaymentValidator
{
    bool ValidateAccount(string accountInfo);
    decimal GetTransactionFee(decimal amount);
}

// ===== IMPLEMENTATION #1: Credit Card =====
public class CreditCardProcessor : IPaymentProcessor, IPaymentValidator
{
    private const decimal FeePercentage = 0.029m;  // 2.9% fee
    
    public bool ProcessPayment(decimal amount, string accountInfo)
    {
        Console.WriteLine($"\n[Credit Card] Processing ${amount:F2}");
        
        if (!ValidateAccount(accountInfo))
        {
            Console.WriteLine("❌ Invalid credit card number");
            return false;
        }
        
        decimal fee = GetTransactionFee(amount);
        Console.WriteLine($"  Transaction Fee: ${fee:F2}");
        Console.WriteLine($"  Total Charge: ${amount + fee:F2}");
        Console.WriteLine("✓ Payment processed successfully");
        return true;
    }
    
    public bool RefundPayment(string transactionId, decimal amount)
    {
        Console.WriteLine($"\n[Credit Card] Refunding ${amount:F2} for transaction {transactionId}");
        Console.WriteLine("✓ Refund will appear in 3-5 business days");
        return true;
    }
    
    public string GetPaymentMethod()
    {
        return "Credit Card";
    }
    
    public bool ValidateAccount(string cardNumber)
    {
        // Simple validation: 16 digits
        return cardNumber?.Length == 16 && long.TryParse(cardNumber, out _);
    }
    
    public decimal GetTransactionFee(decimal amount)
    {
        return amount * FeePercentage;
    }
}

// ===== IMPLEMENTATION #2: PayPal =====
public class PayPalProcessor : IPaymentProcessor, IPaymentValidator
{
    public bool ProcessPayment(decimal amount, string accountInfo)
    {
        Console.WriteLine($"\n[PayPal] Processing ${amount:F2}");
        
        if (!ValidateAccount(accountInfo))
        {
            Console.WriteLine("❌ Invalid PayPal email");
            return false;
        }
        
        decimal fee = GetTransactionFee(amount);
        Console.WriteLine($"  PayPal Fee: ${fee:F2}");
        Console.WriteLine("✓ Payment sent via PayPal");
        return true;
    }
    
    public bool RefundPayment(string transactionId, decimal amount)
    {
        Console.WriteLine($"\n[PayPal] Instant refund of ${amount:F2}");
        Console.WriteLine("✓ Refund completed");
        return true;
    }
    
    public string GetPaymentMethod()
    {
        return "PayPal";
    }
    
    public bool ValidateAccount(string email)
    {
        return email?.Contains("@") ?? false;
    }
    
    public decimal GetTransactionFee(decimal amount)
    {
        return amount * 0.034m + 0.30m;  // 3.4% + $0.30
    }
}

// ===== IMPLEMENTATION #3: Bank Transfer =====
public class BankTransferProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount, string accountInfo)
    {
        Console.WriteLine($"\n[Bank Transfer] Processing ${amount:F2}");
        Console.WriteLine($"  To Account: {accountInfo}");
        Console.WriteLine("✓ Transfer initiated (2-3 business days)");
        return true;
    }
    
    public bool RefundPayment(string transactionId, decimal amount)
    {
        Console.WriteLine($"\n[Bank Transfer] Reversing ${amount:F2}");
        Console.WriteLine("✓ Reversal initiated");
        return true;
    }
    
    public string GetPaymentMethod()
    {
        return "Bank Transfer";
    }
}

// ===== IMPLEMENTATION #4: Cryptocurrency =====
public class CryptoProcessor : IPaymentProcessor
{
    public bool ProcessPayment(decimal amount, string walletAddress)
    {
        Console.WriteLine($"\n[Cryptocurrency] Processing ${amount:F2}");
        Console.WriteLine($"  To Wallet: {walletAddress}");
        Console.WriteLine($"  Network Fee: $2.50");
        Console.WriteLine("✓ Transaction broadcast to blockchain");
        return true;
    }
    
    public bool RefundPayment(string transactionId, decimal amount)
    {
        Console.WriteLine($"\n[Cryptocurrency] Sending ${amount:F2} back");
        Console.WriteLine("⚠ Irreversible - sending new transaction");
        return true;
    }
    
    public string GetPaymentMethod()
    {
        return "Cryptocurrency";
    }
}

// ===== USING INTERFACES FOR FLEXIBILITY =====
public class CheckoutService
{
    private IPaymentProcessor paymentProcessor;
    
    // Dependency Injection - accept any payment processor
    public CheckoutService(IPaymentProcessor processor)
    {
        this.paymentProcessor = processor;
    }
    
    // Can change payment processor at runtime
    public void SetPaymentProcessor(IPaymentProcessor processor)
    {
        this.paymentProcessor = processor;
    }
    
    public bool ProcessOrder(decimal amount, string accountInfo)
    {
        Console.WriteLine($"\n===== Processing Order =====");
        Console.WriteLine($"Amount: ${amount:F2}");
        Console.WriteLine($"Payment Method: {paymentProcessor.GetPaymentMethod()}");
        
        bool success = paymentProcessor.ProcessPayment(amount, accountInfo);
        
        if (success)
        {
            Console.WriteLine("✓ Order completed!");
        }
        else
        {
            Console.WriteLine("❌ Order failed!");
        }
        
        return success;
    }
}

// ===== DEMO =====
class InterfaceDemo
{
    static void Main()
    {
        Console.WriteLine("===== Payment Processing System =====");
        
        // ===== Create different payment processors =====
        var creditCard = new CreditCardProcessor();
        var paypal = new PayPalProcessor();
        var bankTransfer = new BankTransferProcessor();
        var crypto = new CryptoProcessor();
        
        // ===== Using polymorphism with interfaces =====
        var processors = new List<IPaymentProcessor>
        {
            creditCard, paypal, bankTransfer, crypto
        };
        
        Console.WriteLine("\nAvailable Payment Methods:");
        foreach (var processor in processors)
        {
            Console.WriteLine($"  - {processor.GetPaymentMethod()}");
        }
        
        // ===== Checkout Service (Dependency Injection) =====
        var checkout = new CheckoutService(creditCard);
        
        // Process with credit card
        checkout.ProcessOrder(150.00m, "1234567890123456");
        
        // Switch to PayPal at runtime
        checkout.SetPaymentProcessor(paypal);
        checkout.ProcessOrder(200.00m, "customer@email.com");
        
        // Switch to crypto
        checkout.SetPaymentProcessor(crypto);
        checkout.ProcessOrder(500.00m, "0x742d35Cc6634C0532925a3b844Bc9e7595f0bEb");
        
        // ===== Refund Scenario =====
        Console.WriteLine("\n===== Processing Refund =====");
        paypal.RefundPayment("TXN-12345", 200.00m);
    }
}
```

**Expected Output** (partial):
```
===== Employee Management System =====

--------------------------------------------------
ID: 1 | Name: Alice Johnson
Email: alice@company.com | Hired: 2024-11-17
Type: Full-Time
Annual Salary: $75,000.00
Monthly Salary: $6,250.00
Vacation Days: 20
Bonus: $7,500.00
Years of Service: 0

--------------------------------------------------
ID: 2 | Name: Bob Smith
Email: bob@company.com | Hired: 2024-11-17
Type: Part-Time
Hourly Rate: $25.00
Hours/Week: 20
Monthly Salary: $2,000.00
Bonus: $1,300.00

===== PAYROLL SUMMARY =====
Total Employees: 5
Total Monthly Cost: $30,750.00
Total Annual Bonuses: $18,550.00
Total Annual Cost: $387,550.00

===== Payment Processing System =====

===== Processing Order =====
Amount: $150.00
Payment Method: Credit Card

[Credit Card] Processing $150.00
  Transaction Fee: $4.35
  Total Charge: $154.35
✓ Payment processed successfully
✓ Order completed!
```

## 5. Use Cases
- **Pluggable Services**: Use interfaces for logging, data access, notification systems - swap implementations easily
- **Payment Systems**: Different payment processors (credit card, PayPal, crypto) implementing same interface
- **Domain Hierarchies**: Employee types, product categories, customer tiers sharing common base behavior
- **Testing**: Create mock implementations of interfaces for unit testing without real dependencies
- **Strategy Pattern**: Different algorithms (sorting, pricing, discounting) switchable at runtime
- **Repository Pattern**: Abstract data access - use SQL, NoSQL, file storage interchangeably
- **Notification System**: Email, SMS, push notifications all implementing `INotificationService`

## 6. Common Pitfalls & Best Practices

### ❌ Common Mistakes:
```csharp
// WRONG: Using inheritance for code reuse when composition is better
public class UserService : DatabaseConnection  // Bad - UserService "is-a" DatabaseConnection?
{
}

// WRONG: Deep inheritance hierarchies (hard to maintain)
Vehicle -> MotorVehicle -> FourWheelerVehicle -> PassengerCar -> Sedan
// Too many levels!

// WRONG: Not using virtual/override correctly
public class Base
{
    public void DoSomething() { }  // Not virtual - can't be overridden!
}

// WRONG: Interface with too many methods (violates Interface Segregation)
public interface IEmployee
{
    void Work();
    void GetPaid();
    void ManageTeam();      // Not all employees manage
    void ApproveExpenses(); // Not all employees approve
}
```

### ✅ Best Practices:
```csharp
// CORRECT: Favor composition over inheritance
public class UserService
{
    private readonly IDatabaseConnection _db;  // HAS-A relationship
    
    public UserService(IDatabaseConnection db)
    {
        _db = db;
    }
}

// CORRECT: Keep inheritance hierarchies shallow (2-3 levels max)
Vehicle -> Car -> SportsCar  // Much better!

// CORRECT: Mark methods virtual if they should be overrideable
public class Base
{
    public virtual void DoSomething() { }  // Can be overridden
}

public class Derived : Base
{
    public override void DoSomething() { }  // Override with new behavior
}

// CORRECT: Split large interfaces (Interface Segregation Principle)
public interface IWorker
{
    void Work();
    void GetPaid();
}

public interface IManager : IWorker
{
    void ManageTeam();
    void ApproveExpenses();
}

// CORRECT: Program to interfaces, not implementations
public class OrderService
{
    private readonly IPaymentProcessor _processor;  // Not CreditCardProcessor
    
    public OrderService(IPaymentProcessor processor)
    {
        _processor = processor;  // Can accept ANY payment processor
    }
}
```

### When to Use What?

| Scenario | Use | Example |
|----------|-----|---------|
| "IS-A" relationship | Inheritance | Dog IS-A Animal |
| "CAN-DO" capability | Interface | Customer CAN-DO IPayable |
| Shared implementation | Abstract Base Class | Employee with common fields/methods |
| Multiple capabilities | Multiple Interfaces | Class can implement IComparable, IDisposable |
| No shared code | Interface | ILogger - just contract |
| Need to swap implementations | Interface | IRepository<T> |

### Abstract Class vs Interface

**Use Abstract Class when**:
- Classes share common implementation
- Need to define non-public members
- Want to add methods in future without breaking existing code

**Use Interface when**:
- Unrelated classes need same capability
- Class needs multiple inheritance (C# allows multiple interfaces)
- Want complete flexibility to change implementation

## 7. Mini Practice Tasks

### Task 1: Shape Calculator System
Create a shape hierarchy:
1. Abstract base class `Shape` with:
   - Properties: Name, Color
   - Abstract methods: `CalculateArea()`, `CalculatePerimeter()`
   - Virtual method: `DisplayInfo()`
2. Derived classes:
   - `Circle` (radius)
   - `Rectangle` (width, height)
   - `Triangle` (base, height, side1, side2, side3)
3. Create a list of different shapes
4. Calculate total area of all shapes using polymorphism

**Expected Output**:
```
Circle (Red): Area = 78.54 sq units
Rectangle (Blue): Area = 50.00 sq units
Triangle (Green): Area = 24.00 sq units
Total Area: 152.54 sq units
```

### Task 2: Logger System with Multiple Implementations
Create a logging system:
1. Define `ILogger` interface with:
   - `void Log(string message)`
   - `void LogError(string message)`
   - `void LogWarning(string message)`
2. Implementations:
   - `ConsoleLogger` - writes to console with colors
   - `FileLogger` - appends to a file
   - `MultiLogger` - logs to multiple loggers at once
3. Create a `UserService` class that:
   - Takes ILogger in constructor
   - Uses logger for different operations
4. Demonstrate swapping loggers at runtime

### Task 3: Data Repository Pattern
Implement the repository pattern:
1. Create `IRepository<T>` interface:
   - `void Add(T item)`
   - `T GetById(int id)`
   - `List<T> GetAll()`
   - `void Update(T item)`
   - `void Delete(int id)`
2. Implementations:
   - `InMemoryRepository<T>` - uses List<T>
   - `FileRepository<T>` - simulates file storage
3. Create `Product` class with Id, Name, Price
4. Use both repositories with the same Product class
5. Demonstrate that business logic doesn't care which repository is used

### Task 4: Notification System
Build a flexible notification system:
1. Create `INotificationService` interface:
   - `bool Send(string recipient, string subject, string message)`
   - `string GetServiceName()`
2. Implementations:
   - `EmailNotification` - validates email format
   - `SmsNotification` - validates phone number format
   - `PushNotification` - validates device token
3. Create `NotificationManager` class:
   - Accepts list of INotificationService
   - Method `SendToAll()` that tries all services
   - Tracks success/failure for each service
4. Send a notification through all services and show results

**Expected Output**:
```
===== Sending Notification =====
Subject: Account Created
Message: Welcome to our service!

✓ Email sent to user@email.com
✓ SMS sent to +1234567890
❌ Push failed: Invalid device token

Results: 2 successful, 1 failed
```

### Task 5: Employee Benefits System
Expand the Employee hierarchy:
1. Add `IBenefitsEligible` interface:
   - `decimal GetHealthInsuranceCost()`
   - `decimal Get401kContribution()`
   - `int GetPaidTimeOffDays()`
2. Make `FullTimeEmployee` implement this interface
3. Add Manager class (extends FullTimeEmployee):
   - Additional property: Department
   - Additional benefit: Company car allowance
   - Override bonus calculation (15% instead of 10%)
4. Calculate total company cost including benefits for all employees

**Skills practiced**: Inheritance, interfaces, polymorphism, method overriding, type checking
