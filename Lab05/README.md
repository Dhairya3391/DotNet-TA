# Lab 05 - Inheritance, Interface & Abstraction

This lab demonstrates core Object-Oriented Programming concepts in C# including inheritance, interfaces, and abstraction.

## Programs Implemented

### A-Level Tasks (Basic)

#### 1. Lab05_A01_AnimalInheritance
**Concept**: Single Inheritance
- Base class: `Animal` with `Eat()` method
- Derived class: `Dog` with `Bark()` method
- Demonstrates how Dog inherits from Animal and adds its own functionality

#### 2. Lab05_A02_VehicleInheritance
**Concept**: Multilevel Inheritance
- Hierarchy: `Vehicle` → `Car` → `ElectricCar`
- Each level adds its own properties and methods
- Demonstrates virtual method overriding with `DisplayType()`

#### 3. Lab05_A03_ShapePolymorphism
**Concept**: Runtime Polymorphism
- Base class: `Shape` with virtual `Area()` method
- Derived classes: `Circle`, `Rectangle`, `Triangle` override `Area()`
- Demonstrates polymorphic behavior using base class reference

### B-Level Tasks (Intermediate)

#### 4. Lab05_B01_ApplianceAbstraction
**Concept**: Abstract Classes
- Abstract class: `Appliance` with abstract methods `TurnOn()` and `TurnOff()`
- Concrete classes: `Fan`, `Light`, `AirConditioner` implement abstract methods
- Demonstrates abstraction and hiding implementation details

#### 5. Lab05_B02_PrintableInterface
**Concept**: Interface Implementation
- Interface: `IPrintable` with `PrintDetails()` method
- Classes: `Book`, `Magazine`, `Newspaper` implement IPrintable
- Demonstrates contract-based programming with interfaces

#### 6. Lab05_B03_RobotInterface
**Concept**: Multiple Interface Implementation
- Interfaces: `IMovable` (Move, Stop) and `ISound` (MakeSound, Mute)
- Classes: `Robot` and `Drone` implement both interfaces
- Demonstrates C#'s support for multiple interface implementation

### C-Level Tasks (Complex)

#### 7. Lab05_C01_PaymentSystem
**Concept**: Abstract Classes + Exception Handling
- Abstract class: `Payment` with abstract `MakePayment()` method
- Derived classes: `CreditCardPayment`, `UPIPayment`, `NetBankingPayment`
- Custom exception: `InvalidPaymentException` for amount < Rs. 100
- Demonstrates abstraction with business logic validation

#### 8. Lab05_C02_EmployeeBonus
**Concept**: Polymorphism with Business Logic
- Abstract class: `Employee` with abstract `CalculateBonus()` method
- Derived classes:
  - `Manager`: 20% base bonus (+ team size bonus)
  - `Developer`: 10% base bonus (+ experience/project bonus)
  - `Intern`: 5% bonus
- Demonstrates polymorphic calculations with different business rules

#### 9. Lab05_C03_VehicleRental
**Concept**: Interface with Complex Business Logic
- Interface: `IRentable` with `CalculateRent()` and `DisplayDetails()`
- Classes: `Car`, `Bike`, `Scooter` with different rental pricing:
  - Cars: 10% off (7+ days), 20% off (30+ days), AC charges
  - Bikes: 5% off (5+ days), 15% off (15+ days), helmet charges
  - Scooters: Electric discount, long-term rental discount
- Demonstrates interface-based design with varied implementations

## Key Concepts Demonstrated

1. **Inheritance Types**:
   - Single inheritance (Animal → Dog)
   - Multilevel inheritance (Vehicle → Car → ElectricCar)
   - Hierarchical inheritance (Shape → Circle/Rectangle/Triangle)

2. **Polymorphism**:
   - Method overriding with `virtual` and `override` keywords
   - Runtime polymorphism using base class references
   - Interface-based polymorphism

3. **Abstraction**:
   - Abstract classes with abstract methods
   - Hiding implementation details
   - Enforcing method implementation in derived classes

4. **Interfaces**:
   - Defining contracts for classes
   - Multiple interface implementation
   - Interface-based design patterns

5. **Exception Handling**:
   - Custom exception classes
   - Try-catch blocks for error handling
   - Validation with exceptions

## Running the Programs

### Individual Program:
```bash
cd Lab05_A01_AnimalInheritance
dotnet run
```

### Build All Programs:
```bash
# From Lab05 directory
for dir in Lab05_*/; do
    echo "Building $dir"
    cd "$dir"
    dotnet build
    cd ..
done
```

### Run All Programs:
```bash
# From Lab05 directory
for dir in Lab05_*/; do
    echo "Running $dir"
    cd "$dir"
    dotnet run
    echo "Press Enter to continue..."
    read
    cd ..
done
```

## Program Structure

All programs follow a consistent structure:
- Clear header with program name and concept
- Well-commented code explaining OOP concepts
- Formatted console output with borders
- Demonstration of inheritance/interface/abstraction
- Educational summary of concepts at the end

## Educational Value

These programs are designed to teach:
- How to design class hierarchies
- When to use inheritance vs. interfaces
- When to use abstract classes vs. concrete classes
- Real-world applications of OOP concepts
- Best practices for code organization and reusability

## Output Features

- Professional formatted console output
- Box-drawing characters for visual appeal
- Clear section separators
- Detailed explanations of concepts
- Multiple examples demonstrating each concept
- Exception handling demonstrations where applicable
