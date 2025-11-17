# ASP.NET Core Lab Solutions

**Course**: 2301CS412 – ASP.NET Core
**Department**: Computer Science and Engineering
**Institution**: Darshan University
**Academic Year**: 2025-26 | Semester IV
**.NET Version**: .NET 9

---

## Repository Overview

This repository contains **complete implementations** for all C# console application labs (Labs 1-6) covering fundamental programming concepts required before ASP.NET Core development.

## Lab Structure

| Lab | Topic | Programs | Status |
|-----|-------|----------|--------|
| [Lab 01](Lab01/) | Variables, Data Types, Operators | 10 | ✅ Complete |
| [Lab 02](Lab02/) | Conditions & Looping | 10 | ✅ Complete |
| [Lab 03](Lab03/) | Classes, Objects, Constructors & Exception Handling | 9 | ✅ Complete |
| [Lab 04](Lab04/) | Method Overloading, Overriding & Access Modifiers | 8 | ✅ Complete |
| [Lab 05](Lab05/) | Inheritance, Interface & Abstraction | 9 | ✅ Complete |
| [Lab 06](Lab06/) | Collection Classes & Strings | 10 | ✅ Complete |

**Total Programs Implemented**: 56

---

## Quick Start

### Prerequisites
- .NET 9 SDK installed
- Terminal/Command Prompt
- Text editor or IDE (VS Code, Visual Studio, Rider)

### Running a Program

```bash
# Navigate to any lab program
cd Lab01/Lab01_A01_PrintDetails

# Run the program
dotnet run
```

### Building All Programs in a Lab

```bash
# Navigate to a lab folder
cd Lab02

# Build all programs
for dir in Lab02_*/; do
    echo "Building $dir..."
    cd "$dir" && dotnet build && cd ..
done
```

---

## Lab Details

### Lab 01: Variables, Data Types, Operators
**Concepts**: Basic I/O, data types, arithmetic operators, type conversion

**Programs**:
- A Tasks: Print details, Two numbers, Hello message
- B Tasks: Temperature converter, Salary calculator, Shape calculator, Grade calculator
- C Tasks: Currency converter, Shopping discount, Cab fare system

**Learning Outcomes**: Console I/O, variables, operators, conditional statements

---

### Lab 02: Conditions & Looping
**Concepts**: If-else, switch, for loops, while loops, do-while

**Programs**:
- A Tasks: Multiplication table, Character counter, Grade calculator
- B Tasks: Sum even/odd numbers, Factorial
- C Tasks: Strong password checker, Prime number, Reverse number, Palindrome, Fibonacci

**Learning Outcomes**: Control flow, looping constructs, validation logic

---

### Lab 03: Classes, Objects, Constructors & Exception Handling
**Concepts**: OOP basics, constructors, encapsulation, try-catch blocks

**Programs**:
- A Tasks: Student class, Rectangle class, Divide by zero
- B Tasks: Bank account, Constructor overloading, Employee class
- C Tasks: Shopping cart, Car rental, Flight ticket

**Learning Outcomes**: Class design, object creation, exception handling, constructor types

---

### Lab 04: Method Overloading, Overriding & Access Modifiers
**Concepts**: Compile-time vs runtime polymorphism, access control

**Programs**:
- A Tasks: Calculator overloading, Employee overloading, Access modifiers demo
- B Tasks: Animal sound (overriding), Shape area (virtual methods)
- C Tasks: Bank transaction, Library item, Billing system

**Learning Outcomes**: Polymorphism, virtual/override keywords, encapsulation

---

### Lab 05: Inheritance, Interface & Abstraction
**Concepts**: Single/multilevel inheritance, abstract classes, interfaces

**Programs**:
- A Tasks: Animal inheritance, Vehicle inheritance, Shape polymorphism
- B Tasks: Appliance abstraction, Printable interface, Robot multi-interface
- C Tasks: Payment system, Employee bonus, Vehicle rental

**Learning Outcomes**: Inheritance hierarchies, abstraction, interface implementation

---

### Lab 06: Collection Classes & Strings
**Concepts**: Generic collections, LINQ, string manipulation

**Programs**:
- A Tasks: Task stack, Customer queue, Vowel/consonant counter, Palindrome
- B Tasks: Word frequency, Shopping list, Word count (Dictionary), Email set
- C Tasks: Library borrowing (nested collections), Hospital queue (priority)

**Learning Outcomes**: Stack, Queue, List, Dictionary, HashSet, string methods

---

## Code Quality Standards

All programs follow these standards:

✅ **Professional Formatting**: Box-drawing characters, aligned output
✅ **Educational Comments**: Explain concepts for teaching purposes
✅ **Error Handling**: Try-catch blocks where appropriate
✅ **Naming Conventions**: Clear, descriptive variable/method names
✅ **Consistent Style**: Standardized code structure across all programs
✅ **Documentation**: Each lab includes README with detailed explanations

---

## Teaching Approach

Each lab follows a **progressive difficulty model**:

- **A Tasks** (Basic): Core concepts, simple implementations
- **B Tasks** (Medium): Multiple features, moderate complexity
- **C Tasks** (Complex): Real-world scenarios, advanced logic

This structure allows instructors to:
1. Start with simple examples for concept introduction
2. Build complexity gradually
3. Demonstrate real-world applications in advanced tasks

---

## Program Naming Convention

```
Lab{XX}_{Difficulty}{TaskNumber}_{Description}/
```

**Examples**:
- `Lab01_A01_PrintDetails` - Lab 1, A Task 1
- `Lab03_B02_ConstructorOverloading` - Lab 3, B Task 2
- `Lab05_C01_PaymentSystem` - Lab 5, C Task 1

---

## Technology Stack

- **Language**: C# 12 (.NET 9)
- **Framework**: .NET 9 (Latest LTS)
- **Project Type**: Console Applications
- **Collections**: System.Collections.Generic
- **LINQ**: Basic queries in Lab06

---

## Project Structure

```
DotNet-TA/
├── Lab01/          # Variables, Data Types, Operators (10 programs)
├── Lab02/          # Conditions & Looping (10 programs)
├── Lab03/          # Classes, Objects, Constructors (9 programs)
├── Lab04/          # Method Overloading/Overriding (8 programs)
├── Lab05/          # Inheritance, Interface, Abstraction (9 programs)
├── Lab06/          # Collections & Strings (10 programs)
├── CLAUDE.md       # Project instructions for AI assistance
├── lab.md          # Original lab manual
└── README.md       # This file
```

---

## Build & Test Summary

| Lab | Programs | Build Status | Errors | Warnings |
|-----|----------|--------------|--------|----------|
| Lab01 | 10 | ✅ Success | 0 | Minor nullable warnings |
| Lab02 | 10 | ✅ Success | 0 | Minor nullable warnings |
| Lab03 | 9 | ✅ Success | 0 | Minor nullable warnings |
| Lab04 | 8 | ✅ Success | 0 | Minor nullable warnings |
| Lab05 | 9 | ✅ Success | 0 | Minor nullable warnings |
| Lab06 | 10 | ✅ Success | 0 | Minor nullable warnings |

**Note**: Nullable reference type warnings are expected in .NET 9 and do not affect functionality.

---

## Key Concepts Covered

### Programming Fundamentals
- Variables, data types, operators
- Console input/output
- Type conversion and casting
- Conditional statements (if/else, switch)
- Loops (for, while, do-while)

### Object-Oriented Programming
- Classes and objects
- Constructors (default, parameterized, overloaded)
- Properties and methods
- Encapsulation (access modifiers)
- Inheritance (single, multilevel, hierarchical)
- Polymorphism (compile-time, runtime)
- Abstraction (abstract classes, interfaces)

### Advanced Concepts
- Exception handling (try-catch-finally)
- Generic collections (Stack, Queue, List, Dictionary, HashSet)
- String manipulation
- LINQ basics
- File I/O concepts
- Custom exceptions

---

## Usage for Students

1. **Study the code**: Read through implementations to understand concepts
2. **Run examples**: Execute programs to see output
3. **Modify code**: Experiment with changes to learn
4. **Complete exercises**: Use as reference for assignments
5. **Ask questions**: Code includes educational comments

---

## Usage for Instructors

1. **Live Demonstrations**: Run programs during lectures
2. **Code Walkthroughs**: Explain implementation line by line
3. **Concept Reinforcement**: Show multiple examples of same concept
4. **Assignment Creation**: Use as base for student exercises
5. **Assessment**: Evaluate student understanding against reference

---

## Contributing

This repository is maintained as Teaching Assistant work for Darshan University. For improvements or corrections:

1. Test the proposed change
2. Follow existing code style
3. Update documentation
4. Ensure all programs still build successfully

---

## License

Educational use only. Property of Darshan University Computer Science Department.

---

## Contact

**Course**: 2301CS412 – ASP.NET Core
**Department**: Computer Science and Engineering
**Darshan University**

---

## Acknowledgments

- All programs implemented following lab manual specifications
- Code designed for educational clarity
- Professional standards maintained throughout
- .NET 9 latest features utilized where appropriate

---

**Last Updated**: November 2025
**Version**: 1.0
**Status**: Complete (Labs 1-6)
