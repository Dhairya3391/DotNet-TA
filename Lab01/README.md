# Lab 01 - Variables, Data Types, Operators

This folder contains 10 console applications demonstrating C# fundamentals.

## Structure

All programs are built with **.NET 9** and can be run independently.

### A Tasks (Basic)

1. **Lab01_A01_PrintDetails** - Print personal details (name, address, contact, city)
2. **Lab01_A02_TwoNumbers** - Get two numbers from user and print them
3. **Lab01_A03_HelloMessage** - Display greeting message with name and country

### B Tasks (Medium)

4. **Lab01_B01_TemperatureConverter** - Convert temperature between Celsius and Fahrenheit
5. **Lab01_B02_SalaryCalculator** - Calculate employee gross and net salary with HRA, DA, and deductions
6. **Lab01_B03_ShapeCalculator** - Calculate area and perimeter for rectangle, circle, and triangle
7. **Lab01_B04_GradeCalculator** - Calculate grade from 5 subject marks (A/B/C/Fail)

### C Tasks (Complex)

8. **Lab01_C01_CurrencyConverter** - Convert INR to USD, EUR, GBP
9. **Lab01_C02_ShoppingDiscount** - Calculate tiered shopping discounts
10. **Lab01_C03_CabFareSystem** - Complex cab fare calculator with multiple conditions

## How to Run

### Run a specific program:
```bash
cd Lab01_A01_PrintDetails
dotnet run
```

### Build all programs:
```bash
# From Lab01 directory
for dir in Lab01_*/; do
    echo "Building $dir..."
    cd "$dir" && dotnet build && cd ..
done
```

## Teaching Order

When demonstrating to students:

1. Start with **A tasks** to show basic input/output and string formatting
2. Move to **B tasks** to introduce conditions, calculations, and menu systems
3. Finish with **C tasks** to demonstrate complex logic with multiple conditions

Each program is self-contained and demonstrates specific C# concepts like:
- Console I/O
- Variables and data types
- Operators (arithmetic, comparison)
- Conditional statements (if-else, switch)
- String interpolation
- Type conversion
