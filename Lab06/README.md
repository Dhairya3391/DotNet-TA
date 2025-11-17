# Lab 06: Collection Classes & Strings

This lab focuses on demonstrating the proper use of generic collection classes and string manipulation in C#. All programs feature hardcoded examples followed by interactive menu systems for hands-on exploration.

## Programs Overview

### A Tasks (Basic Level)

#### Lab06_A01_TaskStack
- **Purpose**: Recent tasks tracker using Stack<string>
- **Collection**: Stack<T> - LIFO (Last In First Out)
- **Operations**: Push, Pop, Peek, Count, Clear
- **Features**:
  - Add tasks to stack
  - Undo last task (pop)
  - View top task without removing (peek)
  - Display all tasks
  - Educational comments explaining LIFO behavior

#### Lab06_A02_CustomerQueue
- **Purpose**: Customer service queue system
- **Collection**: Queue<T> - FIFO (First In First Out)
- **Operations**: Enqueue, Dequeue, Peek, Count, Clear
- **Features**:
  - Add customers to queue
  - Serve next customer (dequeue)
  - View next customer without removing (peek)
  - Display all waiting customers
  - Educational comments explaining FIFO behavior

#### Lab06_A03_VowelConsonant
- **Purpose**: Count vowels and consonants in strings
- **String Methods**: ToLower(), Contains(), char.IsLetter(), char.IsWhiteSpace()
- **Features**:
  - Case-insensitive counting
  - Ignores spaces
  - Detailed analysis with percentages
  - Lists all vowels and consonants found

#### Lab06_A04_PalindromeString
- **Purpose**: Check if string is palindrome
- **String Methods**: ToLower(), Replace(), Array.Reverse(), Substring()
- **Features**:
  - Case-insensitive checking
  - Ignores spaces
  - Shows original, cleaned, and reversed strings
  - Detailed analysis

### B Tasks (Intermediate Level)

#### Lab06_B01_WordFrequency
- **Purpose**: Count word occurrences using manual array-based counting
- **Methods**: Split(), ToLower(), Replace(), bubble sort
- **Features**:
  - Manual counting without Dictionary
  - Removes punctuation
  - Sorts by frequency (descending)
  - Shows word count and percentage
  - Educational approach showing logic without collections

#### Lab06_B02_ShoppingList
- **Purpose**: Manage shopping list with duplicate prevention
- **Collection**: List<T>
- **Operations**: Add, Remove, Search, Sort, Clear
- **Features**:
  - Prevents duplicate items (case-insensitive)
  - Add/remove items
  - Search for items
  - Sort alphabetically
  - Display item count and full list

#### Lab06_B03_WordCount
- **Purpose**: Count word occurrences using Dictionary
- **Collection**: Dictionary<string, int>
- **Operations**: ContainsKey, Add, indexer [], Count
- **Features**:
  - Key-value pair storage (word -> count)
  - Automatic duplicate handling
  - Sorts by frequency using LINQ
  - Shows word count and percentage
  - Explains Dictionary operations

#### Lab06_B04_EmailSet
- **Purpose**: Manage unique email addresses
- **Collection**: HashSet<T>
- **Operations**: Add, Remove, Contains, Clear, CopyTo
- **Features**:
  - Automatic duplicate prevention
  - O(1) lookup time
  - Basic email validation
  - Bulk operations
  - Export to array
  - Case-sensitive comparison demonstration

### C Tasks (Advanced Level)

#### Lab06_C01_LibraryBorrowing
- **Purpose**: Library book borrowing system with queues per book
- **Collections**: Dictionary<string, Queue<string>>
- **Complexity**: Nested collections (Dictionary of Queues)
- **Features**:
  - Multiple books with separate borrower queues
  - Key = Book Title, Value = Queue of Borrower Names
  - Process book returns (serve next borrower)
  - View next borrower for any book
  - Display all books and their queues
  - System statistics (most popular book, average queue length)
  - Demonstrates complex data structure management

#### Lab06_C02_HospitalQueue
- **Purpose**: Hospital patient queue with priority handling
- **Collections**: Two Queue<string> objects (Normal + Emergency)
- **Priority Logic**: Emergency patients always served first
- **Features**:
  - Separate queues for normal and emergency patients
  - Priority-based serving
  - Both queues follow FIFO internally
  - Display both queues with visual distinction
  - Statistics (percentages, total counts)
  - Simulate busy hospital scenario
  - Demonstrates priority queue logic using two standard queues

## Key Learning Objectives

### Collections
1. **Stack<T>**: LIFO behavior (Last In First Out)
2. **Queue<T>**: FIFO behavior (First In First Out)
3. **List<T>**: Dynamic array with Add/Remove operations
4. **Dictionary<TKey, TValue>**: Key-value pairs with fast lookup
5. **HashSet<T>**: Unique elements with O(1) operations
6. **Nested Collections**: Dictionary<string, Queue<string>>

### String Manipulation
1. **Case Conversion**: ToLower(), ToUpper()
2. **Trimming**: Trim(), TrimStart(), TrimEnd()
3. **Splitting**: Split() with various delimiters
4. **Replacing**: Replace() for character/string replacement
5. **Searching**: Contains(), IndexOf(), LastIndexOf()
6. **Substring**: Substring() for extracting parts
7. **Character Analysis**: char.IsLetter(), char.IsWhiteSpace()
8. **String Building**: StringBuilder for efficient concatenation

### Best Practices Demonstrated
1. **Null/Empty Checking**: string.IsNullOrWhiteSpace()
2. **Case-Insensitive Comparison**: StringComparison.OrdinalIgnoreCase
3. **Input Validation**: Validate before processing
4. **Clear UI**: Formatted tables with borders
5. **Educational Comments**: Explain collection behavior
6. **Error Handling**: Graceful handling of empty collections
7. **LINQ Usage**: OrderByDescending(), ThenBy()

## Running the Programs

Each program can be run independently:

```bash
# Run any program
cd Lab06_A01_TaskStack
dotnet run

# Or build first, then run
dotnet build
dotnet run
```

## Program Structure

All programs follow this structure:
1. **Hardcoded Examples**: Demonstrate functionality with sample data
2. **Visual Output**: Formatted tables and clear labeling
3. **Interactive Menu**: User can test functionality hands-on
4. **Input Validation**: Proper error handling
5. **Educational Comments**: Explain collection operations

## Expected Output

Each program displays:
- Clear title and purpose
- Hardcoded example output
- Interactive menu for hands-on testing
- Formatted tables for data display
- Success/error messages
- Collection operation explanations

## Testing Notes

- All programs compile successfully on .NET 8
- Nullable reference warnings are expected (not errors)
- Each program includes:
  - Hardcoded demonstration
  - Interactive mode
  - Input validation
  - Clear documentation
  - Educational value

## Collection Complexity Analysis

| Collection | Add | Remove | Search | Best Use Case |
|------------|-----|--------|--------|---------------|
| Stack<T> | O(1) | O(1) | O(n) | LIFO operations, undo functionality |
| Queue<T> | O(1) | O(1) | O(n) | FIFO operations, task scheduling |
| List<T> | O(1)* | O(n) | O(n) | Dynamic arrays, indexed access |
| Dictionary<TKey,TValue> | O(1)* | O(1) | O(1) | Key-value lookup, counting |
| HashSet<T> | O(1)* | O(1) | O(1) | Unique items, membership testing |

*Amortized time complexity

## File Structure

```
Lab06/
├── Lab06_A01_TaskStack/
│   ├── Program.cs
│   └── Lab06_A01_TaskStack.csproj
├── Lab06_A02_CustomerQueue/
├── Lab06_A03_VowelConsonant/
├── Lab06_A04_PalindromeString/
├── Lab06_B01_WordFrequency/
├── Lab06_B02_ShoppingList/
├── Lab06_B03_WordCount/
├── Lab06_B04_EmailSet/
├── Lab06_C01_LibraryBorrowing/
├── Lab06_C02_HospitalQueue/
└── README.md
```

## Author
Teaching Assistant - ASP.NET Core Course (2301CS412)
Darshan University, Computer Science and Engineering
Semester IV, 2025-26
