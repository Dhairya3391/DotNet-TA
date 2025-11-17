# Lab 03 - Classes, Objects, Constructors & Exception Handling

This lab demonstrates fundamental object-oriented programming concepts in C# including class design, constructors, and exception handling.

## Lab Programs Overview

### A-Level Tasks (Basic)

#### 1. Lab03_A01_StudentClass
**Objective**: Create a Student class with properties and display student details.

**Concepts**:
- Class definition with properties
- Object creation and initialization
- Display methods

**Features**:
- Student class with Name, RollNo, Marks properties
- Creates 2 student objects with different data
- DisplayDetails() method for formatted output

**Run Command**:
```bash
cd Lab03_A01_StudentClass
dotnet run
```

---

#### 2. Lab03_A02_RectangleClass
**Objective**: Rectangle class with parameterized constructor to calculate area.

**Concepts**:
- Parameterized constructors
- Private fields and encapsulation
- Method implementation for calculations

**Features**:
- Rectangle class with length and width
- Parameterized constructor
- CalculateArea() method
- Demonstrates 3 different rectangles

**Run Command**:
```bash
cd Lab03_A02_RectangleClass
dotnet run
```

---

#### 3. Lab03_A03_DivideByZero
**Objective**: Accept two numbers, divide them, and handle divide-by-zero exception.

**Concepts**:
- Try-catch blocks
- DivideByZeroException handling
- Input validation
- FormatException handling

**Features**:
- Interactive division calculator
- Handles division by zero gracefully
- Validates numeric input
- Repeatable program with user choice

**Run Command**:
```bash
cd Lab03_A03_DivideByZero
dotnet run
```

---

### B-Level Tasks (Intermediate)

#### 4. Lab03_B01_BankAccount
**Objective**: BankAccount class with deposit/withdrawal methods and exception handling.

**Concepts**:
- Custom exception classes (InsufficientFundsException)
- Encapsulation with private fields
- Business logic validation
- Method implementation with error handling

**Features**:
- BankAccount class with account details
- Deposit() and Withdraw() methods
- Prevents negative balance
- Custom exception for insufficient funds
- Demonstrates successful and failed transactions

**Run Command**:
```bash
cd Lab03_B01_BankAccount
dotnet run
```

---

#### 5. Lab03_B02_ConstructorOverloading
**Objective**: Person class demonstrating constructor overloading.

**Concepts**:
- Constructor overloading (5 different constructors)
- Constructor chaining with 'this' keyword
- Default vs parameterized constructors

**Features**:
- Person class with Name, Age, City, Occupation
- 5 overloaded constructors (0 to 4 parameters)
- Constructor chaining demonstration
- Creates 5 person objects using different constructors

**Run Command**:
```bash
cd Lab03_B02_ConstructorOverloading
dotnet run
```

---

#### 6. Lab03_B03_EmployeeClass
**Objective**: Employee class with parameterized constructor and methods.

**Concepts**:
- Parameterized constructors
- Property encapsulation
- Method implementation
- Formatted output

**Features**:
- Employee class with EmpID, EmpName, Salary
- GetAnnualSalary() calculation
- GiveRaise() method to increase salary
- Displays 3 employees with summary statistics

**Run Command**:
```bash
cd Lab03_B03_EmployeeClass
dotnet run
```

---

### C-Level Tasks (Advanced)

#### 7. Lab03_C01_ShoppingCart
**Objective**: ShoppingCart class with items, calculate total, throw exception for invalid quantity.

**Concepts**:
- Composition (Item class within ShoppingCart)
- Collection management (List<Item>)
- Custom exception (InvalidQuantityException)
- Complex class interactions

**Features**:
- Item class with Name, Price, Quantity
- ShoppingCart class managing multiple items
- AddItem() method with validation
- CalculateTotal() for cart value
- Validates quantity > 0
- Professional tabular output
- Demonstrates exception scenarios

**Run Command**:
```bash
cd Lab03_C01_ShoppingCart
dotnet run
```

---

#### 8. Lab03_C02_CarRental
**Objective**: CarRental class with CalculateRent() method, throw exception if days <= 0.

**Concepts**:
- Business logic implementation
- Property validation
- Custom exception (InvalidRentalDaysException)
- Complex calculations with discount logic

**Features**:
- CarRental class with model, rate, days, customer
- CalculateRent() with tiered discount system:
  - 1-6 days: No discount
  - 7-29 days: 10% discount
  - 30+ days: 20% discount
- Validates rental days > 0
- Displays rental start/end dates
- Multiple rental scenarios demonstrated

**Run Command**:
```bash
cd Lab03_C02_CarRental
dotnet run
```

---

#### 9. Lab03_C03_FlightTicket
**Objective**: FlightTicket class, throw exception if ticket price < Rs. 500.

**Concepts**:
- Constructor validation
- Custom business rule exceptions (InvalidTicketPriceException)
- Property validation with private backing field
- Constructor overloading

**Features**:
- FlightTicket class with passenger and flight details
- Minimum price validation (Rs. 500)
- Two constructors (basic and extended)
- CalculateGST() method (5% GST)
- GetTotalPrice() including GST
- Displays basic and complete ticket formats
- Tests multiple invalid price scenarios

**Run Command**:
```bash
cd Lab03_C03_FlightTicket
dotnet run
```

---

## Key Learning Outcomes

### Classes and Objects
- Defining classes with properties and methods
- Creating and initializing objects
- Encapsulation with private fields and public properties

### Constructors
- Default constructors
- Parameterized constructors
- Constructor overloading
- Constructor chaining with 'this' keyword

### Exception Handling
- Try-catch blocks for error handling
- Standard exceptions (DivideByZeroException, FormatException, ArgumentException)
- Custom exception classes
- Business rule validation with exceptions

### Best Practices Demonstrated
- Meaningful variable and method names
- Comments for teaching purposes
- Formatted console output with borders
- Input validation and error messages
- Separation of concerns (business logic vs display)

---

## Building All Projects

To build all Lab03 projects at once:

```bash
# From Lab03 directory
for dir in Lab03_*/; do
    cd "$dir"
    dotnet build --verbosity quiet
    cd ..
done
```

---

## Testing Individual Programs

Each program is self-contained and can be tested independently:

```bash
cd Lab03_A01_StudentClass
dotnet run
```

---

## Common Patterns Used

1. **Helper Methods**: `PrintHeader()` and `PrintFooter()` for consistent formatting
2. **Exception Handling**: Comprehensive try-catch blocks with specific exception types
3. **Validation**: Input validation at constructor level and method level
4. **Display Methods**: Separate methods for displaying object details
5. **Business Logic**: Calculations and validations in dedicated methods

---

## Notes for Students

- All programs compile without errors
- Some nullable reference warnings are normal in .NET 8 and can be ignored
- Programs use hardcoded examples for demonstration purposes
- Exception handling demonstrates both successful and failure scenarios
- Follow the progression from A-level (basic) to C-level (advanced) tasks

---

## Course Information

**Course**: 2301CS412 - ASP.NET Core
**Semester**: IV (2025-26)
**Department**: Computer Science and Engineering
**Institution**: Darshan University
**Lab**: 03 - Classes, Objects, Constructors & Exception Handling
