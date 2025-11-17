# Lab 04: Method Overloading, Overriding & Access Modifiers

This lab demonstrates core Object-Oriented Programming concepts in C#: method overloading, method overriding, and access modifiers.

## Overview

**Course**: 2301CS412 - ASP.NET Core
**Semester**: IV, 2025-26
**Institution**: Darshan University - Computer Science & Engineering

## Programs Implemented

### A Tasks (Basic Level)

#### 1. Lab04_A01_Calculator
- **Concept**: Method Overloading
- **Description**: Calculator class with overloaded `Add()` methods
- **Overloads**:
  - `Add(int, int)` - Adds two integers
  - `Add(int, int, int)` - Adds three integers
  - `Add(double, double)` - Adds two doubles
- **Key Learning**: Compile-time polymorphism, same method name with different parameters

#### 2. Lab04_A02_EmployeeOverloading
- **Concept**: Method Overloading with objects
- **Description**: Employee class with overloaded `DisplayInfo()` methods
- **Overloads**:
  - `DisplayInfo(string name)` - Shows name only
  - `DisplayInfo(string name, int age)` - Shows name and age
  - `DisplayInfo(string name, int age, double salary)` - Shows complete information
- **Key Learning**: Flexible method signatures for different use cases

#### 3. Lab04_A03_AccessModifiers
- **Concept**: Access Modifiers (public, private, protected, internal)
- **Description**: Person class demonstrating all access levels
- **Features**:
  - Public members (accessible everywhere)
  - Private members (only within class)
  - Protected members (accessible in derived classes)
  - Internal members (accessible within assembly)
  - Employee derived class showing protected access
- **Key Learning**: Encapsulation and data hiding principles

### B Tasks (Intermediate Level)

#### 4. Lab04_B01_AnimalSound
- **Concept**: Method Overriding (Runtime Polymorphism)
- **Description**: Animal base class with `virtual Sound()` method
- **Classes**:
  - Animal (base class)
  - Dog (overrides Sound() - "Woof!")
  - Cat (overrides Sound() - "Meow!")
  - Cow (overrides Sound() - "Moo!")
- **Key Learning**: Virtual/override keywords, runtime polymorphism, base class references

#### 5. Lab04_B02_ShapeArea
- **Concept**: Abstract classes and method overriding
- **Description**: Shape base class with abstract `CalculateArea()` and `CalculatePerimeter()`
- **Classes**:
  - Shape (abstract base class)
  - Circle (πr²)
  - Rectangle (length × width)
  - Triangle (0.5 × base × height)
  - Square (side²)
- **Key Learning**: Abstract methods, polymorphic collections, practical calculations

### C Tasks (Advanced Level)

#### 6. Lab04_C01_BankTransaction
- **Concept**: Method Overloading with encapsulation
- **Description**: Bank transaction system with private fields and public methods
- **Features**:
  - BankAccount class (encapsulated balance)
  - BankTransaction class with overloaded `Transfer()` methods:
    - `Transfer(from, to, amount)` - Simple transfer
    - `Transfer(from, to, amount, description)` - Transfer with description
    - `Transfer(from, to, amount, description, fee)` - Transfer with transaction fee
  - Transaction history tracking
  - Validation and error handling
- **Key Learning**: Encapsulation, method overloading in real-world scenarios

#### 7. Lab04_C02_LibraryItem
- **Concept**: Inheritance with access modifiers
- **Description**: Library management system with multiple item types
- **Classes**:
  - LibraryItem (base class) - protected, private, internal fields
  - Book - ISBN, pages, genre
  - Magazine - issue number, month, publisher
  - DVD - duration, director, rating
- **Features**:
  - Check-out/return functionality
  - Different late fee calculations per item type
  - Access modifier demonstrations
- **Key Learning**: Inheritance hierarchies, access control, polymorphic behavior

#### 8. Lab04_C03_BillingSystem
- **Concept**: Method Overriding with business logic
- **Description**: Customer billing system with tiered discounts
- **Classes**:
  - Customer (abstract base class)
  - RegularCustomer - 0% discount
  - PremiumCustomer - 15% discount
  - VIPCustomer - 25% discount + loyalty points
- **Features**:
  - Abstract `CalculateBill()` method
  - Different discount strategies
  - Purchase tracking and savings calculation
  - Cost comparison analysis
- **Key Learning**: Abstract classes, strategy pattern, real-world business logic

## Running the Programs

### Run Individual Program
```bash
cd Lab04_A01_Calculator
dotnet run
```

### Build All Programs
```bash
# From Lab04 directory
for dir in Lab04_*/; do
    cd "$dir"
    dotnet build
    cd ..
done
```

### Build and Run Specific Program
```bash
cd Lab04_B01_AnimalSound
dotnet build
dotnet run
```

## Key Concepts Covered

### 1. Method Overloading (Compile-Time Polymorphism)
- Same method name, different parameters
- Parameters differ by: number, type, or order
- Decided at compile time
- Examples: Calculator.Add(), Employee.DisplayInfo()

### 2. Method Overriding (Runtime Polymorphism)
- Base class method marked as `virtual`
- Derived class method marked as `override`
- Decided at runtime based on actual object type
- Examples: Animal.Sound(), Shape.CalculateArea()

### 3. Access Modifiers
- **public**: No restrictions, accessible everywhere
- **private**: Only accessible within the class
- **protected**: Accessible within class and derived classes
- **internal**: Accessible within same assembly

### 4. Abstract Classes and Methods
- Cannot instantiate abstract classes directly
- Abstract methods must be implemented by derived classes
- Used for defining contracts/interfaces
- Examples: Shape, Customer

### 5. Encapsulation
- Private fields with public methods/properties
- Controlled access to data
- Data validation in setters
- Examples: BankAccount, LibraryItem

## Program Complexity

- **A Tasks**: Basic demonstrations, straightforward implementations
- **B Tasks**: Intermediate complexity, polymorphic arrays, runtime behavior
- **C Tasks**: Advanced scenarios, real-world applications, multiple concepts combined

## Educational Features

All programs include:
- Formatted console output with borders
- Clear section headings
- Multiple test cases
- Educational comments
- Key learning points summary
- Practical real-world examples

## Testing

Each program demonstrates:
1. Direct object usage
2. Polymorphic behavior (base class references)
3. Collections/arrays of objects
4. Practical applications
5. Edge cases and validations

## File Structure

```
Lab04/
├── Lab04_A01_Calculator/
│   ├── Program.cs
│   └── Lab04_A01_Calculator.csproj
├── Lab04_A02_EmployeeOverloading/
│   ├── Program.cs
│   └── Lab04_A02_EmployeeOverloading.csproj
├── Lab04_A03_AccessModifiers/
│   ├── Program.cs
│   └── Lab04_A03_AccessModifiers.csproj
├── Lab04_B01_AnimalSound/
│   ├── Program.cs
│   └── Lab04_B01_AnimalSound.csproj
├── Lab04_B02_ShapeArea/
│   ├── Program.cs
│   └── Lab04_B02_ShapeArea.csproj
├── Lab04_C01_BankTransaction/
│   ├── Program.cs
│   └── Lab04_C01_BankTransaction.csproj
├── Lab04_C02_LibraryItem/
│   ├── Program.cs
│   └── Lab04_C02_LibraryItem.csproj
├── Lab04_C03_BillingSystem/
│   ├── Program.cs
│   └── Lab04_C03_BillingSystem.csproj
└── README.md
```

## Learning Outcomes

After completing this lab, students will understand:
1. How to implement method overloading for flexible APIs
2. How to use method overriding for polymorphic behavior
3. When to use different access modifiers
4. How to design inheritance hierarchies
5. The difference between compile-time and runtime polymorphism
6. How to implement encapsulation and data hiding
7. Real-world applications of OOP concepts

## Notes

- All programs are standalone console applications
- Each program compiles without errors or warnings
- Programs use .NET 8 (Latest LTS)
- Code follows C# naming conventions
- Comprehensive examples with multiple test cases
- Educational comments throughout the code

## Additional Resources

- [C# Method Overloading](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/methods#method-overloading)
- [C# Polymorphism](https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/polymorphism)
- [C# Access Modifiers](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/access-modifiers)
- [C# Abstract Classes](https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/abstract-and-sealed-classes-and-class-members)

---

**Lab Completed**: All 8 programs implemented and tested successfully
**Build Status**: All programs compile without errors
