# Variables, Data Types, Operators

## 1. Description
Variables are named storage locations for values. Data types specify what kind of values can be stored (integers, text, booleans, decimals, etc.). Operators are symbols or keywords that perform operations on values (for arithmetic, comparison, logical operations and more).

Think of variables as labeled boxes where you store information temporarily while your program runs. Just like you wouldn't store milk in a toolbox, you need to pick the right data type for each piece of information.

## 2. Why It Is Important
Every program stores and manipulates data. Knowing variables, types, and operators helps you write correct logic, avoid type-related bugs, and choose the right representations (for example, using `decimal` for money instead of `double`).

**Common Mistake**: Using `float` or `double` for money calculations can cause rounding errors. Always use `decimal` for financial calculations!

## 3. Real-World Examples
- **E-commerce**: Track product quantity (`int`), price (`decimal`), product name (`string`), and availability status (`bool`)
- **Student Management**: Store student ID (`int`), name (`string`), GPA (`double`), and graduated status (`bool`)
- **Banking Application**: Calculate account balance (`decimal`), transaction amounts, and determine if account is active (`bool`)
- **Game Development**: Track player health points (`int`), position coordinates (`double`), player name (`string`), and game over status (`bool`)

## 4. Syntax & Explanation

### Example 1: E-commerce Shopping Cart Calculator
```csharp
using System;

class ShoppingCartDemo
{
    static void Main()
    {
        // ===== Variables Declaration =====
        // Integer: whole numbers (product quantities, item counts)
        int itemQuantity = 3;
        int shippingDays = 5;
        
        // Decimal: precise decimal numbers (always use for money!)
        decimal itemPrice = 29.99m;  // Note the 'm' suffix for decimal
        decimal taxRate = 0.08m;     // 8% tax
        
        // Double: floating-point numbers (measurements, calculations)
        double distanceKm = 125.5;
        double deliveryWeight = 2.5;  // kg
        
        // String: text data
        string customerName = "Sarah Johnson";
        string productName = "Wireless Headphones";
        
        // Boolean: true/false values
        bool isPremiumMember = true;
        bool isInStock = true;
        
        // ===== Arithmetic Operators =====
        decimal subtotal = itemPrice * itemQuantity;           // Multiplication: 29.99 * 3 = 89.97
        decimal taxAmount = subtotal * taxRate;                // 89.97 * 0.08 = 7.1976
        decimal shippingCost = isPremiumMember ? 0m : 9.99m;  // Ternary operator
        decimal totalCost = subtotal + taxAmount + shippingCost;
        
        // ===== Comparison Operators =====
        bool qualifiesForDiscount = subtotal >= 100m;    // >= greater than or equal
        bool isFastShipping = shippingDays <= 3;         // <= less than or equal
        bool isExpensive = totalCost > 100m;             // > greater than
        
        // ===== Logical Operators =====
        bool canPurchase = isInStock && totalCost > 0;   // AND: both must be true
        bool needsAlert = !isInStock || itemQuantity > 10; // OR: at least one true, NOT operator !
        bool eligibleForPromo = isPremiumMember && subtotal >= 50m; // Combine multiple conditions
        
        // ===== Type Conversion =====
        // Explicit cast: converting double to decimal for calculations
        decimal convertedWeight = (decimal)deliveryWeight;  
        
        // Implicit conversion: int to double (no data loss)
        double averagePrice = subtotal / itemQuantity;  // result is double automatically
        
        // String to number conversion (useful for user input)
        string userInput = "42";
        int parsedNumber = int.Parse(userInput);        // Convert string to int
        
        // Safe conversion with TryParse (recommended in production)
        if (int.TryParse("100", out int result))
        {
            Console.WriteLine($"Successfully parsed: {result}");
        }
        
        // ===== Output with String Interpolation =====
        Console.WriteLine("====== SHOPPING CART SUMMARY ======");
        Console.WriteLine($"Customer: {customerName}");
        Console.WriteLine($"Product: {productName}");
        Console.WriteLine($"Quantity: {itemQuantity}");
        Console.WriteLine($"Item Price: {itemPrice:C}");  // :C formats as currency
        Console.WriteLine($"Subtotal: {subtotal:C}");
        Console.WriteLine($"Tax ({taxRate:P0}): {taxAmount:C}");  // :P0 formats as percentage
        Console.WriteLine($"Shipping: {shippingCost:C}");
        Console.WriteLine($"Total: {totalCost:C2}");  // :C2 ensures 2 decimal places
        Console.WriteLine($"\nPremium Member: {isPremiumMember}");
        Console.WriteLine($"In Stock: {isInStock}");
        Console.WriteLine($"Qualifies for Discount: {qualifiesForDiscount}");
        Console.WriteLine($"Estimated Delivery: {shippingDays} days");
    }
}
```

**Expected Output:**
```
Successfully parsed: 100
====== SHOPPING CART SUMMARY ======
Customer: Sarah Johnson
Product: Wireless Headphones
Quantity: 3
Item Price: $29.99
Subtotal: $89.97
Tax (8%): $7.20
Shipping: $0.00
Total: $97.17

Premium Member: True
In Stock: True
Qualifies for Discount: False
Estimated Delivery: 5 days
```

### Common Data Types Reference
| Type | Size | Range | Use Case |
|------|------|-------|----------|
| `int` | 4 bytes | -2.1B to 2.1B | Counters, IDs, quantities |
| `long` | 8 bytes | Very large numbers | Large datasets, timestamps |
| `decimal` | 16 bytes | ±7.9 × 10^28 | **Money, precise calculations** |
| `double` | 8 bytes | ±5.0 × 10^324 | Scientific calculations, measurements |
| `float` | 4 bytes | ±3.4 × 10^38 | Graphics, game physics (when precision isn't critical) |
| `bool` | 1 byte | true/false | Flags, conditions |
| `char` | 2 bytes | Single character | Single letters, symbols |
| `string` | Variable | Text | Names, descriptions, messages |

### Key Operators
- **Arithmetic**: `+`, `-`, `*`, `/`, `%` (modulus - remainder)
- **Comparison**: `==`, `!=`, `<`, `>`, `<=`, `>=`
- **Logical**: `&&` (AND), `||` (OR), `!` (NOT)
- **Assignment**: `=`, `+=`, `-=`, `*=`, `/=`, `++`, `--`

## 5. Use Cases
- **Financial Applications**: Calculate totals, discounts, taxes, and balances with `decimal` type
- **User Authentication**: Use boolean flags like `isAuthenticated`, `isAdmin`, `isEmailVerified`
- **Data Processing**: Parse and convert user input (strings ➜ numbers) with error handling
- **Business Logic**: Implement validations, comparisons, and conditional logic
- **Inventory Management**: Track quantities, check stock levels, calculate reorder points

## 6. Common Pitfalls & Best Practices

### ❌ Common Mistakes:
```csharp
// WRONG: Using double for money (causes rounding errors)
double price = 10.10;
double tax = price * 0.1;  // Might get 1.0099999999

// WRONG: Not checking for division by zero
int result = 10 / 0;  // Runtime error!

// WRONG: Comparing floating-point numbers with ==
double a = 0.1 + 0.2;
if (a == 0.3) { }  // Might not work due to precision
```

### ✅ Best Practices:
```csharp
// CORRECT: Use decimal for money
decimal price = 10.10m;
decimal tax = price * 0.1m;  // Exactly 1.01

// CORRECT: Check before division
if (divisor != 0)
{
    int result = 10 / divisor;
}

// CORRECT: Use TryParse for safe conversion
if (int.TryParse(userInput, out int number))
{
    // Use number safely
}
else
{
    Console.WriteLine("Invalid input!");
}

// CORRECT: Use meaningful variable names
decimal customerAccountBalance = 1500.00m;  // Clear
// Instead of: decimal x = 1500.00m;  // What is x?
```

## 7. Mini Practice Tasks

### Task 1: Student Grade Calculator
Write a program that:
1. Stores 5 test scores (use `int` or `double`)
2. Calculates the average score
3. Determines if the student passed (average >= 60)
4. Displays the results with proper formatting

**Hint**: You'll need variables for scores, average, and a boolean for pass/fail status.

### Task 2: Restaurant Bill Calculator
Create a program that:
1. Takes the meal cost as input (`decimal`)
2. Calculates 18% tip
3. Calculates 8% tax (on original amount, not tip)
4. Shows subtotal, tip, tax, and total
5. Determines if the bill is over $50 (boolean)

**Expected Output Format**:
```
Meal: $45.00
Tip (18%): $8.10
Tax (8%): $3.60
Total: $56.70
Over $50: True
```

### Task 3: Temperature Converter
Build a program that:
1. Asks for temperature in Celsius (use `double`)
2. Converts to Fahrenheit using formula: `F = (C × 9/5) + 32`
3. Converts to Kelvin using formula: `K = C + 273.15`
4. Displays all three temperatures formatted to 2 decimal places

**Bonus**: Add a boolean to indicate if water would be frozen at that temperature (C <= 0).
