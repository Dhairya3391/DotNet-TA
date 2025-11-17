# Collection Classes & Strings

## 1. Description
Collection classes (like `List<T>`, `Dictionary<TKey, TValue>`, `Queue<T>`, `Stack<T>`) store and manage groups of objects. Strings are sequences of characters (`string`) with many helper methods for manipulation and formatting.

Think of collections as different types of containers: Lists are like ordered shopping carts, Dictionaries are like phone books for fast lookups, Queues are like waiting lines, and Stacks are like plates stacked on top of each other.

## 2. Why It Is Important
Most applications process collections of data (lists of users, lookup maps, message queues). Efficient use of collections and proper string handling are essential for performance and correctness. Understanding collections is critical because:
- **Real Data Comes in Groups**: Users, orders, products - everything comes in collections
- **Performance Matters**: Choosing the right collection type affects speed (List vs Dictionary vs HashSet)
- **String Operations Are Everywhere**: User input, file processing, API responses all involve strings
- **Memory Efficiency**: Proper string handling prevents memory issues in large applications

## 3. Real-World Examples
- **E-commerce**: Use `List<Product>` to display products, `Dictionary<int, Cart>` for user shopping carts, `Queue<Order>` for order processing
- **Social Media**: Store user posts in `List<Post>`, friend connections in `Dictionary<string, User>`, notification queue
- **Customer Service**: Use `Queue<Ticket>` for support tickets (first-come, first-served)
- **Undo Functionality**: Use `Stack<Action>` to track user actions (Ctrl+Z in editors)
- **Text Processing**: Parse CSV files, validate email formats, clean user input, format reports
- **Caching**: Use `Dictionary<string, object>` for fast data lookups without database hits

## 4. Syntax & Explanation

### Example 1: E-commerce Product Management with Collections
```csharp
using System;
using System.Collections.Generic;
using System.Linq;

class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Category { get; set; }
    public int StockQuantity { get; set; }
}

class EcommerceDemo
{
    static void Main()
    {
        // ===== LIST<T> - Ordered collection, allows duplicates =====
        Console.WriteLine("===== Product Catalog (List) =====");
        
        var products = new List<Product>
        {
            new Product { Id = 1, Name = "Laptop", Price = 999.99m, Category = "Electronics", StockQuantity = 15 },
            new Product { Id = 2, Name = "Mouse", Price = 25.50m, Category = "Electronics", StockQuantity = 50 },
            new Product { Id = 3, Name = "Keyboard", Price = 75.00m, Category = "Electronics", StockQuantity = 30 },
            new Product { Id = 4, Name = "Monitor", Price = 299.99m, Category = "Electronics", StockQuantity = 20 },
            new Product { Id = 5, Name = "USB Cable", Price = 12.99m, Category = "Accessories", StockQuantity = 100 }
        };
        
        // Adding items
        products.Add(new Product { Id = 6, Name = "Webcam", Price = 89.99m, Category = "Electronics", StockQuantity = 25 });
        
        // Accessing by index
        Console.WriteLine($"First product: {products[0].Name}");
        Console.WriteLine($"Total products: {products.Count}");
        
        // Finding items
        var laptop = products.Find(p => p.Name == "Laptop");
        Console.WriteLine($"Found: {laptop.Name} - ${laptop.Price}");
        
        // Filtering with LINQ
        var affordableProducts = products.Where(p => p.Price < 100).ToList();
        Console.WriteLine($"\nAffordable Products (under $100): {affordableProducts.Count}");
        foreach (var p in affordableProducts)
        {
            Console.WriteLine($"  - {p.Name}: ${p.Price}");
        }
        
        // Sorting
        var sortedByPrice = products.OrderBy(p => p.Price).ToList();
        Console.WriteLine($"\nCheapest product: {sortedByPrice[0].Name} (${sortedByPrice[0].Price})");
        Console.WriteLine($"Most expensive: {sortedByPrice[sortedByPrice.Count - 1].Name} (${sortedByPrice[sortedByPrice.Count - 1].Price})");
        
        // Calculating totals
        decimal totalInventoryValue = products.Sum(p => p.Price * p.StockQuantity);
        Console.WriteLine($"Total Inventory Value: ${totalInventoryValue:N2}");
        
        // Removing items
        products.RemoveAll(p => p.StockQuantity == 0);  // Remove out-of-stock items
        
        // ===== DICTIONARY<TKey, TValue> - Fast lookups by key =====
        Console.WriteLine("\n===== Shopping Cart System (Dictionary) =====");
        
        // Dictionary: ProductId -> Quantity
        var shoppingCart = new Dictionary<int, int>();
        
        // Adding items (key-value pairs)
        shoppingCart[1] = 1;     // 1 Laptop
        shoppingCart[2] = 2;     // 2 Mice
        shoppingCart.Add(3, 1);  // 1 Keyboard
        
        // Checking if key exists
        if (shoppingCart.ContainsKey(1))
        {
            Console.WriteLine("Laptop is in cart");
        }
        
        // Safe retrieval with TryGetValue (recommended)
        if (shoppingCart.TryGetValue(2, out int mouseQuantity))
        {
            Console.WriteLine($"Mouse quantity in cart: {mouseQuantity}");
        }
        
        // Iterating dictionary
        decimal cartTotal = 0;
        Console.WriteLine("\nCart Contents:");
        foreach (var item in shoppingCart)
        {
            int productId = item.Key;
            int quantity = item.Value;
            var product = products.Find(p => p.Id == productId);
            
            if (product != null)
            {
                decimal itemTotal = product.Price * quantity;
                cartTotal += itemTotal;
                Console.WriteLine($"  {product.Name} x{quantity} = ${itemTotal:F2}");
            }
        }
        Console.WriteLine($"Cart Total: ${cartTotal:F2}");
        
        // Updating values
        shoppingCart[2] = 3;  // Update mouse quantity to 3
        
        // Removing items
        shoppingCart.Remove(3);  // Remove keyboard from cart
        
        // ===== User Account Lookup =====
        Console.WriteLine("\n===== User Account Lookup (Dictionary) =====");
        
        var userAccounts = new Dictionary<string, string>
        {
            { "john.doe@email.com", "John Doe" },
            { "jane.smith@email.com", "Jane Smith" },
            { "bob.jones@email.com", "Bob Jones" }
        };
        
        string loginEmail = "jane.smith@email.com";
        if (userAccounts.TryGetValue(loginEmail, out string userName))
        {
            Console.WriteLine($"Welcome back, {userName}!");
        }
        else
        {
            Console.WriteLine("Account not found.");
        }
        
        // ===== QUEUE<T> - First In, First Out (FIFO) =====
        Console.WriteLine("\n===== Order Processing Queue =====");
        
        var orderQueue = new Queue<string>();
        
        // Enqueue: Add to end of queue
        orderQueue.Enqueue("Order #1001 - John Doe");
        orderQueue.Enqueue("Order #1002 - Jane Smith");
        orderQueue.Enqueue("Order #1003 - Bob Jones");
        
        Console.WriteLine($"Orders in queue: {orderQueue.Count}");
        
        // Peek: Look at next item without removing
        Console.WriteLine($"Next order to process: {orderQueue.Peek()}");
        
        // Dequeue: Remove and return first item
        while (orderQueue.Count > 0)
        {
            string order = orderQueue.Dequeue();
            Console.WriteLine($"Processing: {order}");
        }
        
        // ===== STACK<T> - Last In, First Out (LIFO) =====
        Console.WriteLine("\n===== Page Navigation History (Stack) =====");
        
        var pageHistory = new Stack<string>();
        
        // Push: Add to top of stack
        pageHistory.Push("Home Page");
        pageHistory.Push("Products Page");
        pageHistory.Push("Product Details");
        pageHistory.Push("Shopping Cart");
        
        Console.WriteLine($"Current page: {pageHistory.Peek()}");
        Console.WriteLine($"Pages in history: {pageHistory.Count}");
        
        // Pop: Remove and return top item (like browser back button)
        Console.WriteLine("\nNavigating back:");
        while (pageHistory.Count > 0)
        {
            string page = pageHistory.Pop();
            Console.WriteLine($"  <- Back to: {page}");
        }
        
        // ===== HASHSET<T> - Unique items, fast lookups =====
        Console.WriteLine("\n===== Featured Product Tags (HashSet) =====");
        
        var productTags = new HashSet<string> { "sale", "new", "trending" };
        
        // Add returns false if item already exists
        bool added = productTags.Add("sale");  // false - already exists
        Console.WriteLine($"'sale' tag added: {added}");
        
        productTags.Add("featured");  // true - new item
        
        // Fast membership check
        if (productTags.Contains("new"))
        {
            Console.WriteLine("This product is NEW!");
        }
        
        Console.WriteLine("All tags: " + string.Join(", ", productTags));
    }
}
```

### Example 2: String Manipulation - User Data Processing
```csharp
using System;
using System.Text;

class StringOperationsDemo
{
    static void Main()
    {
        // ===== Basic String Operations =====
        Console.WriteLine("===== Customer Name Processing =====");
        
        string rawInput = "   john.doe@email.com   ";  // User input with spaces
        
        // Trim: Remove leading/trailing whitespace
        string email = rawInput.Trim();
        Console.WriteLine($"Cleaned email: '{email}'");
        
        // ToLower/ToUpper: Case conversion
        string lowerEmail = email.ToLower();
        Console.WriteLine($"Normalized: {lowerEmail}");
        
        // Length: Get character count
        Console.WriteLine($"Email length: {email.Length} characters");
        
        // Contains: Check for substring
        if (email.Contains("@"))
        {
            Console.WriteLine("✓ Valid email format");
        }
        
        // StartsWith/EndsWith: Check prefix/suffix
        if (email.EndsWith(".com"))
        {
            Console.WriteLine("✓ Commercial email domain");
        }
        
        // ===== String Splitting and Joining =====
        Console.WriteLine("\n===== CSV Data Processing =====");
        
        string csvLine = "1001,John Doe,john@email.com,123-456-7890";
        
        // Split: Break string into array
        string[] fields = csvLine.Split(',');
        
        Console.WriteLine("Parsed CSV fields:");
        Console.WriteLine($"  ID: {fields[0]}");
        Console.WriteLine($"  Name: {fields[1]}");
        Console.WriteLine($"  Email: {fields[2]}");
        Console.WriteLine($"  Phone: {fields[3]}");
        
        // Join: Combine array into string
        string[] names = { "Alice", "Bob", "Charlie", "Diana" };
        string nameList = string.Join(", ", names);
        Console.WriteLine($"\nInvited guests: {nameList}");
        
        string pathParts = string.Join("/", new[] { "users", "documents", "file.txt" });
        Console.WriteLine($"File path: {pathParts}");
        
        // ===== String Replacement =====
        Console.WriteLine("\n===== Text Template Processing =====");
        
        string template = "Hello {name}, your order #{order} has been shipped!";
        string message = template
            .Replace("{name}", "John")
            .Replace("{order}", "1001");
        Console.WriteLine(message);
        
        // Replace for data sanitization
        string userComment = "This product is @#$% amazing!";
        string sanitized = userComment.Replace("@#$%", "****");
        Console.WriteLine($"Sanitized: {sanitized}");
        
        // ===== Substring and Indexing =====
        Console.WriteLine("\n===== Order Number Extraction =====");
        
        string orderRef = "ORD-2024-001234";
        
        // Substring: Extract part of string
        string year = orderRef.Substring(4, 4);      // Start at index 4, take 4 chars
        string orderNum = orderRef.Substring(9);     // From index 9 to end
        Console.WriteLine($"Year: {year}, Order: {orderNum}");
        
        // IndexOf: Find position of substring
        int dashPos = orderRef.IndexOf('-');
        Console.WriteLine($"First dash at position: {dashPos}");
        
        // LastIndexOf: Find last occurrence
        int lastDash = orderRef.LastIndexOf('-');
        string justNumber = orderRef.Substring(lastDash + 1);
        Console.WriteLine($"Order number: {justNumber}");
        
        // ===== String Formatting =====
        Console.WriteLine("\n===== Report Generation =====");
        
        string productName = "Laptop";
        decimal price = 999.99m;
        int quantity = 5;
        DateTime orderDate = DateTime.Now;
        
        // String interpolation (modern, readable)
        string report = $"Product: {productName}\n" +
                       $"Price: {price:C}\n" +           // :C = currency format
                       $"Quantity: {quantity}\n" +
                       $"Total: {price * quantity:C2}\n" + // :C2 = currency with 2 decimals
                       $"Date: {orderDate:yyyy-MM-dd}";    // Custom date format
        
        Console.WriteLine(report);
        
        // Alignment in string interpolation
        Console.WriteLine("\n===== Sales Report =====");
        Console.WriteLine($"{"Product",-20} {"Price",10} {"Qty",5} {"Total",10}");
        Console.WriteLine(new string('-', 50));
        Console.WriteLine($"{"Laptop",-20} {999.99m,10:C} {5,5} {4999.95m,10:C}");
        Console.WriteLine($"{"Mouse",-20} {25.50m,10:C} {10,5} {255.00m,10:C}");
        
        // ===== StringBuilder for Multiple Modifications =====
        Console.WriteLine("\n===== Building HTML Content =====");
        
        // StringBuilder is more efficient for multiple string operations
        var html = new StringBuilder();
        html.AppendLine("<html>");
        html.AppendLine("  <body>");
        html.AppendLine("    <h1>Product List</h1>");
        html.AppendLine("    <ul>");
        
        var productNames = new[] { "Laptop", "Mouse", "Keyboard" };
        foreach (var name in productNames)
        {
            html.AppendLine($"      <li>{name}</li>");
        }
        
        html.AppendLine("    </ul>");
        html.AppendLine("  </body>");
        html.AppendLine("</html>");
        
        Console.WriteLine(html.ToString());
        
        // ===== String Comparison =====
        Console.WriteLine("\n===== Password Validation =====");
        
        string password1 = "SecurePass123";
        string password2 = "securepass123";
        
        // Case-sensitive comparison
        bool exactMatch = password1 == password2;  // false
        Console.WriteLine($"Exact match: {exactMatch}");
        
        // Case-insensitive comparison
        bool caseInsensitiveMatch = password1.Equals(password2, StringComparison.OrdinalIgnoreCase);
        Console.WriteLine($"Case-insensitive match: {caseInsensitiveMatch}");
        
        // ===== String Validation =====
        Console.WriteLine("\n===== Input Validation =====");
        
        string username = "john_doe123";
        
        // Check if string is null or empty
        if (string.IsNullOrEmpty(username))
        {
            Console.WriteLine("Username is required!");
        }
        
        // Check if string is null, empty, or whitespace
        string comments = "   ";
        if (string.IsNullOrWhiteSpace(comments))
        {
            Console.WriteLine("Please provide comments.");
        }
        
        // Check for specific characters
        if (username.Contains("_") || username.Contains("-"))
        {
            Console.WriteLine("✓ Username contains valid separators");
        }
    }
}
```

**Expected Output** (partial):
```
===== Product Catalog (List) =====
First product: Laptop
Total products: 6

Affordable Products (under $100): 3
  - Mouse: $25.50
  - Keyboard: $75.00
  - USB Cable: $12.99

Cheapest product: USB Cable ($12.99)
Total Inventory Value: $34,789.20

===== Shopping Cart System (Dictionary) =====
Laptop is in cart
Mouse quantity in cart: 2

Cart Contents:
  Laptop x1 = $999.99
  Mouse x2 = $51.00
  Keyboard x1 = $75.00
Cart Total: $1125.99

===== Customer Name Processing =====
Cleaned email: 'john.doe@email.com'
✓ Valid email format
✓ Commercial email domain
```

### Collection Selection Guide
| Collection | Use When | Time Complexity | Example |
|------------|----------|-----------------|---------|
| **List<T>** | Ordered data, access by index | O(1) access, O(n) search | Order items, product catalog |
| **Dictionary<K,V>** | Fast lookups by key | O(1) average | User profiles, cache |
| **HashSet<T>** | Unique items, membership test | O(1) average | Tags, unique IDs |
| **Queue<T>** | FIFO processing | O(1) enqueue/dequeue | Order processing, tasks |
| **Stack<T>** | LIFO processing | O(1) push/pop | Undo/redo, navigation |
| **SortedList<K,V>** | Ordered key-value pairs | O(log n) | Leaderboard, timeline |

### String Operations Performance Tips
- **Use StringBuilder** when concatenating strings in loops (more efficient)
- **Avoid excessive string.Replace()** chains (consider regex for complex patterns)
- **Use string.Compare()** with options instead of ToLower() for comparisons
- **Cache string.Length** if used multiple times in a loop

## 5. Use Cases
- **Database Operations**: Hold query results in List<T> and process with LINQ
- **Caching**: Fast lookup tables with Dictionary for frequently accessed data
- **Task Management**: Message buffering and job processing with Queue
- **Undo/Redo**: Implement history features with Stack
- **Data Import/Export**: Parse CSV files, format reports, build data files
- **User Input**: Validate, sanitize, and format user-provided text
- **API Integration**: Parse JSON responses, build query strings
- **Logging**: Format log messages, concatenate large text outputs with StringBuilder

## 6. Common Pitfalls & Best Practices

### ❌ Common Mistakes:
```csharp
// WRONG: Modifying list while iterating
var list = new List<int> { 1, 2, 3, 4, 5 };
foreach (var item in list)
{
    if (item % 2 == 0)
        list.Remove(item);  // Exception!
}

// WRONG: Using + for many string concatenations
string result = "";
for (int i = 0; i < 1000; i++)
{
    result += i.ToString();  // Very slow! Creates 1000 string objects
}

// WRONG: Not checking if dictionary key exists
var dict = new Dictionary<string, int>();
int value = dict["missing"];  // KeyNotFoundException!

// WRONG: Using List<T> when you need fast lookups
var ids = new List<int> { 1, 2, 3, ... , 10000 };
if (ids.Contains(5000)) { }  // O(n) - slow!
```

### ✅ Best Practices:
```csharp
// CORRECT: Create new list without removed items
var list = new List<int> { 1, 2, 3, 4, 5 };
var filtered = list.Where(item => item % 2 != 0).ToList();

// OR use RemoveAll (safe)
list.RemoveAll(item => item % 2 == 0);

// CORRECT: Use StringBuilder for multiple concatenations
var sb = new StringBuilder();
for (int i = 0; i < 1000; i++)
{
    sb.Append(i);
}
string result = sb.ToString();

// CORRECT: Always use TryGetValue with Dictionary
var dict = new Dictionary<string, int>();
if (dict.TryGetValue("key", out int value))
{
    Console.WriteLine(value);
}
else
{
    Console.WriteLine("Key not found");
}

// CORRECT: Use HashSet for fast membership tests
var ids = new HashSet<int> { 1, 2, 3, ... , 10000 };
if (ids.Contains(5000)) { }  // O(1) - fast!

// CORRECT: Initialize collection with capacity if size is known
var bigList = new List<int>(1000);  // Preallocate space

// CORRECT: Use LINQ for clean, readable queries
var activeUsers = users
    .Where(u => u.IsActive)
    .OrderBy(u => u.LastName)
    .Take(10)
    .ToList();
```

## 7. Mini Practice Tasks

### Task 1: Student Grade Management
Create a program that:
1. Uses a `Dictionary<string, List<int>>` to store student names and their test scores
2. Add at least 3 students with 3-5 test scores each
3. Calculate and display each student's average
4. Find and display the student with the highest average
5. Display all students who have an average >= 80

**Expected Output**:
```
John Doe: Average = 85.5
Jane Smith: Average = 92.0
Bob Johnson: Average = 78.5

Top student: Jane Smith (92.0)
Honor Roll (>=80): John Doe, Jane Smith
```

### Task 2: Order Processing Queue Simulator
Build a program that:
1. Create a `Queue<string>` for order processing
2. Add 5 orders to the queue (e.g., "Order #1001", "Order #1002", etc.)
3. Process orders one by one (dequeue) and display "Processing: [order]"
4. After processing each order, show how many orders remain
5. Use a `Stack<string>` to keep a history of processed orders
6. After all orders are processed, show the history in reverse order (most recent first)

**Skills practiced**: Queue, Stack, while loops

### Task 3: Email Validator and Formatter
Write a program that:
1. Takes a list of email addresses (some valid, some invalid)
2. Use string methods to validate each email:
   - Must contain exactly one '@'
   - Must have characters before and after '@'
   - Must end with '.com', '.org', or '.edu'
3. Store valid emails in a `List<string>`
4. Format all valid emails to lowercase
5. Display the count of valid vs invalid emails
6. Display all valid emails in alphabetical order

**Test Data**:
```csharp
var emails = new[] {
    "john.doe@example.com",
    "invalid@",
    "@nodomain.com",
    "jane.smith@university.edu",
    "bob@company.org",
    "no-at-sign.com"
};
```

### Task 4: Shopping Cart with Product Catalog
Create a complete shopping cart system:
1. Create a `List<Product>` with at least 5 products (Id, Name, Price, Category)
2. Use a `Dictionary<int, int>` for shopping cart (ProductId -> Quantity)
3. Implement functions to:
   - Add product to cart (by ID)
   - Remove product from cart
   - Update quantity
   - Calculate cart total
   - Display cart contents with product names and prices
4. Use string formatting to display a nice receipt

**Expected Receipt Format**:
```
===== SHOPPING CART =====
Laptop            x1    $999.99
Mouse             x2     $51.00
Keyboard          x1     $75.00
-------------------------
Subtotal:              $1125.99
Tax (8%):                $90.08
Total:                 $1216.07
```

### Task 5: Text File Parser (CSV)
Build a CSV parser that:
1. Starts with a multi-line CSV string:
   ```
   "Name,Age,City,Salary"
   "John Doe,30,New York,75000"
   "Jane Smith,25,Chicago,68000"
   ```
2. Split by newlines to get each row
3. Split each row by commas to get fields
4. Store data in appropriate collections
5. Calculate average salary
6. Find the oldest person
7. Group people by city using `Dictionary<string, List<string>>`

**Bonus**: Handle edge cases like empty fields or extra spaces!
