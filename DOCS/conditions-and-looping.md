# Conditions & Looping

## 1. Description
Conditions (like `if`, `else`, and `switch`) let a program choose behavior based on values. Looping constructs (`for`, `while`, `do-while`, `foreach`) repeat actions until a condition is met. Together, they control the flow of your program.

Without conditions and loops, programs would execute the same way every time - they couldn't make decisions or repeat tasks. These are the building blocks that make programs intelligent and efficient.

## 2. Why It Is Important
Branching and repetition are essential: they let programs react to input, iterate collections, and perform repeated tasks such as processing lists or retrying operations. Mastering these concepts is crucial because:
- **Decision Making**: Programs need to respond differently based on user input or data
- **Efficiency**: Loops prevent code duplication when performing repetitive tasks
- **Data Processing**: Most real applications process collections of data (users, products, orders)
- **Error Handling**: Retry logic and validation require conditions and loops

## 3. Real-World Examples
- **E-commerce**: Validating credit card information and showing appropriate error messages
- **Inventory System**: Iterating through all products to find items that need restocking
- **Banking**: Processing batch transactions in a loop with conditional approval logic
- **User Authentication**: Retrying password validation up to 3 times before locking the account
- **Report Generation**: Looping through database records to calculate totals and averages
- **Menu Systems**: Building interactive console applications with menu options

## 4. Syntax & Explanation

### Example 1: Student Grade Management System
```csharp
using System;
using System.Collections.Generic;

class StudentGradeSystem
{
    static void Main()
    {
        // ===== IF / ELSE IF / ELSE Statements =====
        Console.WriteLine("===== Grade Calculator =====");
        
        int testScore = 85;
        string studentName = "Alex Martinez";
        bool hasExtraCredit = true;
        
        // Apply bonus for extra credit
        if (hasExtraCredit)
        {
            testScore += 5;  // Add 5 points bonus
            Console.WriteLine($"Extra credit applied! New score: {testScore}");
        }
        
        // Determine letter grade with multiple conditions
        string grade;
        string feedback;
        
        if (testScore >= 90)
        {
            grade = "A";
            feedback = "Excellent work!";
        }
        else if (testScore >= 80)
        {
            grade = "B";
            feedback = "Good job!";
        }
        else if (testScore >= 70)
        {
            grade = "C";
            feedback = "Satisfactory.";
        }
        else if (testScore >= 60)
        {
            grade = "D";
            feedback = "Needs improvement.";
        }
        else
        {
            grade = "F";
            feedback = "Please see instructor.";
        }
        
        Console.WriteLine($"Student: {studentName}");
        Console.WriteLine($"Final Score: {testScore}");
        Console.WriteLine($"Grade: {grade} - {feedback}");
        
        // ===== SWITCH Statement (Modern C# 8.0+ Pattern) =====
        Console.WriteLine("\n===== Course Registration System =====");
        
        string department = "CS";
        int courseLevel = 101;
        
        // Traditional switch
        switch (department)
        {
            case "CS":
                Console.WriteLine("Computer Science Department");
                Console.WriteLine("Building: Engineering Hall");
                break;
            case "MATH":
                Console.WriteLine("Mathematics Department");
                Console.WriteLine("Building: Science Center");
                break;
            case "ENG":
                Console.WriteLine("English Department");
                Console.WriteLine("Building: Liberal Arts");
                break;
            default:
                Console.WriteLine("Department not found");
                break;
        }
        
        // Switch expression (C# 8.0+) - more concise
        string difficulty = courseLevel switch
        {
            <= 100 => "Introductory",
            <= 200 => "Intermediate",
            <= 300 => "Advanced",
            _ => "Graduate Level"  // _ is the default case
        };
        Console.WriteLine($"Course Level: {difficulty}");
    }
}
```

### Example 2: Comprehensive Looping Examples
```csharp
using System;
using System.Collections.Generic;

class LoopingExamples
{
    static void Main()
    {
        // ===== FOR Loop - When you know exact number of iterations =====
        Console.WriteLine("===== Processing Daily Sales =====");
        
        decimal totalSales = 0;
        int daysInWeek = 7;
        decimal[] dailySales = { 150.50m, 200.00m, 175.25m, 220.75m, 190.00m, 250.50m, 300.00m };
        
        for (int day = 0; day < daysInWeek; day++)
        {
            totalSales += dailySales[day];
            Console.WriteLine($"Day {day + 1}: ${dailySales[day]:F2}");
        }
        
        decimal averageSales = totalSales / daysInWeek;
        Console.WriteLine($"Total Sales: ${totalSales:F2}");
        Console.WriteLine($"Average Daily Sales: ${averageSales:F2}");
        
        // ===== WHILE Loop - When condition is checked before execution =====
        Console.WriteLine("\n===== Password Validation (Max 3 Attempts) =====");
        
        string correctPassword = "SecurePass123";
        int maxAttempts = 3;
        int attemptCount = 0;
        bool isAuthenticated = false;
        
        while (attemptCount < maxAttempts && !isAuthenticated)
        {
            attemptCount++;
            Console.Write($"Attempt {attemptCount}/{maxAttempts} - Enter password: ");
            
            // Simulating user input for demo
            string userPassword = attemptCount == 2 ? correctPassword : "wrong";
            Console.WriteLine(userPassword);
            
            if (userPassword == correctPassword)
            {
                isAuthenticated = true;
                Console.WriteLine("✓ Login successful!");
            }
            else
            {
                Console.WriteLine($"✗ Incorrect password. {maxAttempts - attemptCount} attempts remaining.");
            }
        }
        
        if (!isAuthenticated)
        {
            Console.WriteLine("Account locked. Please contact support.");
        }
        
        // ===== DO-WHILE Loop - Executes at least once =====
        Console.WriteLine("\n===== ATM Menu System =====");
        
        int menuChoice;
        do
        {
            Console.WriteLine("\n--- ATM Menu ---");
            Console.WriteLine("1. Check Balance");
            Console.WriteLine("2. Deposit");
            Console.WriteLine("3. Withdraw");
            Console.WriteLine("4. Exit");
            Console.Write("Enter choice: ");
            
            // Simulating user input
            menuChoice = 4;  // Exit for demo
            Console.WriteLine(menuChoice);
            
            switch (menuChoice)
            {
                case 1:
                    Console.WriteLine("Your balance: $1,234.56");
                    break;
                case 2:
                    Console.WriteLine("Deposit successful!");
                    break;
                case 3:
                    Console.WriteLine("Withdrawal processed.");
                    break;
                case 4:
                    Console.WriteLine("Thank you for using our ATM!");
                    break;
                default:
                    Console.WriteLine("Invalid choice. Please try again.");
                    break;
            }
        } while (menuChoice != 4);
        
        // ===== FOREACH Loop - For iterating collections =====
        Console.WriteLine("\n===== Processing Customer Orders =====");
        
        var orders = new List<Order>
        {
            new Order { OrderId = 1001, CustomerName = "John Doe", Amount = 150.00m, Status = "Pending" },
            new Order { OrderId = 1002, CustomerName = "Jane Smith", Amount = 275.50m, Status = "Shipped" },
            new Order { OrderId = 1003, CustomerName = "Bob Johnson", Amount = 89.99m, Status = "Pending" },
            new Order { OrderId = 1004, CustomerName = "Alice Brown", Amount = 425.00m, Status = "Delivered" }
        };
        
        decimal totalRevenue = 0;
        int pendingCount = 0;
        
        foreach (var order in orders)
        {
            Console.WriteLine($"Order #{order.OrderId}: {order.CustomerName} - ${order.Amount:F2} [{order.Status}]");
            totalRevenue += order.Amount;
            
            if (order.Status == "Pending")
            {
                pendingCount++;
            }
        }
        
        Console.WriteLine($"\nTotal Orders: {orders.Count}");
        Console.WriteLine($"Total Revenue: ${totalRevenue:F2}");
        Console.WriteLine($"Pending Orders: {pendingCount}");
        
        // ===== NESTED Loops - Processing 2D data =====
        Console.WriteLine("\n===== Seating Chart (3 rows x 5 seats) =====");
        
        bool[,] seatingChart = new bool[3, 5]
        {
            { true, false, true, true, false },
            { true, true, false, true, true },
            { false, true, true, false, true }
        };
        
        int availableSeats = 0;
        
        for (int row = 0; row < 3; row++)
        {
            Console.Write($"Row {row + 1}: ");
            for (int seat = 0; seat < 5; seat++)
            {
                string status = seatingChart[row, seat] ? "[X]" : "[ ]";  // X = occupied, empty = available
                Console.Write(status + " ");
                
                if (!seatingChart[row, seat])
                {
                    availableSeats++;
                }
            }
            Console.WriteLine();
        }
        
        Console.WriteLine($"Available seats: {availableSeats}");
        
        // ===== BREAK and CONTINUE =====
        Console.WriteLine("\n===== Finding First Out-of-Stock Item =====");
        
        var products = new[] { "Laptop", "Mouse", "Keyboard", "Monitor", "Headset" };
        var stockLevels = new[] { 10, 5, 0, 8, 15 };
        
        for (int i = 0; i < products.Length; i++)
        {
            if (stockLevels[i] == 0)
            {
                Console.WriteLine($"⚠ ALERT: {products[i]} is out of stock!");
                break;  // Stop checking once we find first out-of-stock item
            }
            Console.WriteLine($"✓ {products[i]}: {stockLevels[i]} in stock");
        }
        
        // Using continue to skip items
        Console.WriteLine("\n===== Processing Only High-Value Orders =====");
        
        foreach (var order in orders)
        {
            if (order.Amount < 100)
            {
                continue;  // Skip this order and move to next
            }
            
            Console.WriteLine($"Processing high-value order #{order.OrderId}: ${order.Amount:F2}");
        }
    }
}

// Helper class for Order example
class Order
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; }
    public decimal Amount { get; set; }
    public string Status { get; set; }
}
```

**Expected Output** (partial):
```
===== Grade Calculator =====
Extra credit applied! New score: 90
Student: Alex Martinez
Final Score: 90
Grade: A - Excellent work!

===== Course Registration System =====
Computer Science Department
Building: Engineering Hall
Course Level: Introductory

===== Processing Daily Sales =====
Day 1: $150.50
Day 2: $200.00
...
Total Sales: $1487.00
Average Daily Sales: $212.43

===== Processing Customer Orders =====
Order #1001: John Doe - $150.00 [Pending]
Order #1002: Jane Smith - $275.50 [Shipped]
...
Total Revenue: $940.49
Pending Orders: 2
```

### Loop Selection Guide
| Loop Type | When to Use | Example Use Case |
|-----------|-------------|------------------|
| **for** | Know exact iterations | Processing arrays, counting 1 to N |
| **while** | Condition checked before entry | Retry logic, user input validation |
| **do-while** | Execute at least once | Menu systems, getting user input |
| **foreach** | Iterate collections | Processing lists, databases results |

## 5. Use Cases
- **Data Processing**: Looping through collections (files, database rows, API results)
- **Validation**: Checking user input and implementing retry logic with timeouts
- **Batch Operations**: Processing multiple records or transactions in sequence
- **Menu Systems**: Building interactive console applications with navigation
- **Calculations**: Computing totals, averages, finding min/max values
- **Filtering**: Selecting specific items from collections based on criteria
- **State Machines**: Implementing game logic, workflow systems

## 6. Common Pitfalls & Best Practices

### ❌ Common Mistakes:
```csharp
// WRONG: Infinite loop - condition never becomes false
while (true)
{
    Console.WriteLine("This will run forever!");
    // Missing break or condition change
}

// WRONG: Off-by-one error
for (int i = 0; i <= 10; i++)  // Runs 11 times (0-10)
{
    // Intended to run 10 times
}

// WRONG: Modifying collection while iterating
var list = new List<int> { 1, 2, 3 };
foreach (var item in list)
{
    list.Remove(item);  // Exception!
}

// WRONG: Using == to compare strings (case-sensitive)
if (userInput == "yes")  // Won't match "Yes" or "YES"
```

### ✅ Best Practices:
```csharp
// CORRECT: Always have exit condition
int counter = 0;
while (counter < 10)
{
    Console.WriteLine("Iteration: " + counter);
    counter++;  // Don't forget to increment!
}

// CORRECT: Use < instead of <= for arrays
for (int i = 0; i < array.Length; i++)  // Correct: 0 to Length-1
{
    Console.WriteLine(array[i]);
}

// CORRECT: Create new list when removing items
var list = new List<int> { 1, 2, 3, 4, 5 };
var itemsToKeep = new List<int>();
foreach (var item in list)
{
    if (item > 2)
        itemsToKeep.Add(item);
}

// CORRECT: Case-insensitive string comparison
if (userInput.Equals("yes", StringComparison.OrdinalIgnoreCase))
{
    // Matches "yes", "Yes", "YES", etc.
}

// CORRECT: Use break in switch to prevent fall-through
switch (option)
{
    case 1:
        DoSomething();
        break;  // Important!
    case 2:
        DoSomethingElse();
        break;
}
```

## 7. Mini Practice Tasks

### Task 1: Simple Calculator with Menu
Create a calculator program that:
1. Shows a menu with options: Add, Subtract, Multiply, Divide, Exit
2. Uses a loop to keep showing the menu until user chooses Exit
3. Gets two numbers from the user
4. Uses if/else or switch to perform the selected operation
5. Displays the result

**Hint**: Use a do-while loop for the menu, and switch for operation selection.

### Task 2: Number Guessing Game
Build a number guessing game that:
1. Computer picks a random number between 1-100
2. User has 7 attempts to guess
3. After each guess, show "Too high!" or "Too low!"
4. End game when user guesses correctly or runs out of attempts
5. Display how many attempts were used

**Skills practiced**: while loop, if/else, counter variables

### Task 3: Shopping List Total Calculator
Write a program that:
1. Asks user how many items they want to add (use this for loop count)
2. For each item, get the price (use for loop)
3. Calculate and display running subtotal after each item
4. Show final total with 8% tax
5. If total > $100, apply 10% discount before tax

**Expected Output**:
```
How many items? 3
Item 1 price: $25.00
  Subtotal: $25.00
Item 2 price: $50.00
  Subtotal: $75.00
Item 3 price: $35.00
  Subtotal: $110.00

Subtotal: $110.00
Discount (10%): -$11.00
After discount: $99.00
Tax (8%): $7.92
Final Total: $106.92
```

### Task 4: Grade Statistics
Create a program that:
1. Stores test scores for 5 students in an array or list
2. Uses a loop to calculate the highest score
3. Uses another loop to calculate the lowest score
4. Calculates the average score
5. Uses a loop to count how many students passed (score >= 60)

**Bonus**: Use nested loops if you want to process multiple classes!
