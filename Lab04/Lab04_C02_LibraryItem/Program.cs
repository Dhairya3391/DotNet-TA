/*
 * Lab04_C02_LibraryItem
 * Demonstrates inheritance, method overriding, and access modifiers
 * LibraryItem base class with Book and Magazine subclasses
 */

using System;
using System.Collections.Generic;

namespace Lab04_C02_LibraryItem
{
    // Base class LibraryItem
    class LibraryItem
    {
        // Protected fields - accessible in derived classes
        protected string itemId;
        protected string title;
        protected string author;
        protected int yearPublished;

        // Private field - not accessible in derived classes
        private bool isCheckedOut;

        // Internal field - accessible within same assembly
        internal string location;

        // Constructor
        public LibraryItem(string itemId, string title, string author, int yearPublished, string location)
        {
            this.itemId = itemId;
            this.title = title;
            this.author = author;
            this.yearPublished = yearPublished;
            this.isCheckedOut = false;
            this.location = location;
        }

        // Public properties
        public string ItemId { get { return itemId; } }
        public string Title { get { return title; } }
        public bool IsCheckedOut { get { return isCheckedOut; } }

        // Virtual method - can be overridden
        public virtual void DisplayInfo()
        {
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│         LIBRARY ITEM                         │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine($"│ ID:       {itemId,-34} │");
            Console.WriteLine($"│ Title:    {title,-34} │");
            Console.WriteLine($"│ Author:   {author,-34} │");
            Console.WriteLine($"│ Year:     {yearPublished,-34} │");
            Console.WriteLine($"│ Location: {location,-34} │");
            Console.WriteLine($"│ Status:   {(isCheckedOut ? "Checked Out" : "Available"),-34} │");
            Console.WriteLine("└──────────────────────────────────────────────┘");
        }

        // Public method to checkout item
        public virtual bool CheckOut()
        {
            if (!isCheckedOut)
            {
                isCheckedOut = true;
                return true;
            }
            return false;
        }

        // Public method to return item
        public virtual bool ReturnItem()
        {
            if (isCheckedOut)
            {
                isCheckedOut = false;
                return true;
            }
            return false;
        }

        // Virtual method for calculating late fees
        public virtual double CalculateLateFee(int daysLate)
        {
            return daysLate * 0.50; // Base rate: $0.50 per day
        }
    }

    // Book class - derived from LibraryItem
    class Book : LibraryItem
    {
        // Private fields specific to Book
        private string isbn;
        private int numberOfPages;
        private string genre;

        // Constructor
        public Book(string itemId, string title, string author, int yearPublished,
                   string location, string isbn, int numberOfPages, string genre)
            : base(itemId, title, author, yearPublished, location)
        {
            this.isbn = isbn;
            this.numberOfPages = numberOfPages;
            this.genre = genre;
        }

        // Override DisplayInfo method
        public override void DisplayInfo()
        {
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│         BOOK INFORMATION                     │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine($"│ ID:       {itemId,-34} │");
            Console.WriteLine($"│ Title:    {title,-34} │");
            Console.WriteLine($"│ Author:   {author,-34} │");
            Console.WriteLine($"│ Year:     {yearPublished,-34} │");
            Console.WriteLine($"│ ISBN:     {isbn,-34} │");
            Console.WriteLine($"│ Pages:    {numberOfPages,-34} │");
            Console.WriteLine($"│ Genre:    {genre,-34} │");
            Console.WriteLine($"│ Location: {location,-34} │"); // Internal field accessible
            Console.WriteLine($"│ Status:   {(IsCheckedOut ? "Checked Out" : "Available"),-34} │");
            Console.WriteLine("└──────────────────────────────────────────────┘");
        }

        // Override late fee calculation (books have lower late fees)
        public override double CalculateLateFee(int daysLate)
        {
            return daysLate * 0.25; // $0.25 per day for books
        }

        // Book-specific method
        public string GetBookSummary()
        {
            return $"{title} by {author} ({genre}, {numberOfPages} pages)";
        }
    }

    // Magazine class - derived from LibraryItem
    class Magazine : LibraryItem
    {
        // Private fields specific to Magazine
        private string issueNumber;
        private string month;
        private string publisher;

        // Constructor
        public Magazine(string itemId, string title, string author, int yearPublished,
                       string location, string issueNumber, string month, string publisher)
            : base(itemId, title, author, yearPublished, location)
        {
            this.issueNumber = issueNumber;
            this.month = month;
            this.publisher = publisher;
        }

        // Override DisplayInfo method
        public override void DisplayInfo()
        {
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│         MAGAZINE INFORMATION                 │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine($"│ ID:        {itemId,-33} │");
            Console.WriteLine($"│ Title:     {title,-33} │");
            Console.WriteLine($"│ Editor:    {author,-33} │");
            Console.WriteLine($"│ Year:      {yearPublished,-33} │");
            Console.WriteLine($"│ Issue:     {issueNumber,-33} │");
            Console.WriteLine($"│ Month:     {month,-33} │");
            Console.WriteLine($"│ Publisher: {publisher,-33} │");
            Console.WriteLine($"│ Location:  {location,-33} │"); // Internal field accessible
            Console.WriteLine($"│ Status:    {(IsCheckedOut ? "Checked Out" : "Available"),-33} │");
            Console.WriteLine("└──────────────────────────────────────────────┘");
        }

        // Override late fee calculation (magazines have higher late fees)
        public override double CalculateLateFee(int daysLate)
        {
            return daysLate * 1.00; // $1.00 per day for magazines
        }

        // Magazine-specific method
        public string GetIssueInfo()
        {
            return $"{title} - Issue {issueNumber} ({month} {yearPublished})";
        }
    }

    // DVD class - another derived class
    class DVD : LibraryItem
    {
        private int duration; // in minutes
        private string director;
        private string rating;

        public DVD(string itemId, string title, string director, int yearPublished,
                  string location, int duration, string rating)
            : base(itemId, title, director, yearPublished, location)
        {
            this.duration = duration;
            this.director = director;
            this.rating = rating;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("┌──────────────────────────────────────────────┐");
            Console.WriteLine("│         DVD INFORMATION                      │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine($"│ ID:       {itemId,-34} │");
            Console.WriteLine($"│ Title:    {title,-34} │");
            Console.WriteLine($"│ Director: {director,-34} │");
            Console.WriteLine($"│ Year:     {yearPublished,-34} │");
            Console.WriteLine($"│ Duration: {duration} minutes{"",-25} │");
            Console.WriteLine($"│ Rating:   {rating,-34} │");
            Console.WriteLine($"│ Location: {location,-34} │");
            Console.WriteLine($"│ Status:   {(IsCheckedOut ? "Checked Out" : "Available"),-34} │");
            Console.WriteLine("└──────────────────────────────────────────────┘");
        }

        public override double CalculateLateFee(int daysLate)
        {
            return daysLate * 2.00; // $2.00 per day for DVDs
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║   LAB 04 - C02: LIBRARY ITEM SYSTEM            ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");

            // Create library items
            Book book1 = new Book("B001", "The Great Gatsby", "F. Scott Fitzgerald",
                                 1925, "Shelf A1", "978-0-7432-7356-5", 180, "Fiction");

            Book book2 = new Book("B002", "Clean Code", "Robert C. Martin",
                                 2008, "Shelf C5", "978-0-1323-5088-4", 464, "Technology");

            Magazine mag1 = new Magazine("M001", "National Geographic", "Various Authors",
                                        2024, "Shelf D2", "Issue 245", "January", "NatGeo Partners");

            Magazine mag2 = new Magazine("M002", "Scientific American", "Various Authors",
                                        2024, "Shelf D3", "Issue 330", "February", "Springer Nature");

            DVD dvd1 = new DVD("D001", "Inception", "Christopher Nolan",
                              2010, "Shelf E1", 148, "PG-13");

            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("   LIBRARY CATALOG - ALL ITEMS");
            Console.WriteLine(new string('═', 50));

            // Test 1: Display all items
            Console.WriteLine("\n[Test 1] Displaying All Library Items");
            Console.WriteLine(new string('-', 50));

            book1.DisplayInfo();
            Console.WriteLine();
            book2.DisplayInfo();
            Console.WriteLine();
            mag1.DisplayInfo();
            Console.WriteLine();
            mag2.DisplayInfo();
            Console.WriteLine();
            dvd1.DisplayInfo();

            // Test 2: Polymorphic behavior
            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("[Test 2] Polymorphic Array of Library Items");
            Console.WriteLine(new string('-', 50));

            LibraryItem[] items = new LibraryItem[5];
            items[0] = book1;
            items[1] = mag1;
            items[2] = dvd1;
            items[3] = book2;
            items[4] = mag2;

            for (int i = 0; i < items.Length; i++)
            {
                Console.WriteLine($"\nItem {i + 1}:");
                items[i].DisplayInfo();
            }

            // Test 3: Check out and return operations
            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("[Test 3] Check Out and Return Operations");
            Console.WriteLine(new string('-', 50));

            Console.WriteLine("\nChecking out 'The Great Gatsby'...");
            bool checkout1 = book1.CheckOut();
            Console.WriteLine($"Result: {(checkout1 ? "SUCCESS" : "FAILED")}");
            book1.DisplayInfo();

            Console.WriteLine("\nChecking out 'National Geographic'...");
            bool checkout2 = mag1.CheckOut();
            Console.WriteLine($"Result: {(checkout2 ? "SUCCESS" : "FAILED")}");

            Console.WriteLine("\nReturning 'The Great Gatsby'...");
            bool return1 = book1.ReturnItem();
            Console.WriteLine($"Result: {(return1 ? "SUCCESS" : "FAILED")}");
            book1.DisplayInfo();

            // Test 4: Late fee calculations
            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("[Test 4] Late Fee Calculations (5 days late)");
            Console.WriteLine(new string('-', 50));

            int daysLate = 5;

            Console.WriteLine($"\n{book1.Title}:");
            Console.WriteLine($"  Late Fee: ${book1.CalculateLateFee(daysLate):F2}");

            Console.WriteLine($"\n{mag1.Title}:");
            Console.WriteLine($"  Late Fee: ${mag1.CalculateLateFee(daysLate):F2}");

            Console.WriteLine($"\n{dvd1.Title}:");
            Console.WriteLine($"  Late Fee: ${dvd1.CalculateLateFee(daysLate):F2}");

            // Test 5: Access modifiers demonstration
            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("[Test 5] Access Modifiers Demonstration");
            Console.WriteLine(new string('-', 50));

            Console.WriteLine("\nPublic Access:");
            Console.WriteLine($"  ✓ book1.Title = {book1.Title}");
            Console.WriteLine($"  ✓ book1.ItemId = {book1.ItemId}");
            Console.WriteLine($"  ✓ book1.IsCheckedOut = {book1.IsCheckedOut}");

            Console.WriteLine("\nInternal Access (within same assembly):");
            Console.WriteLine($"  ✓ book1.location = {book1.location}");

            Console.WriteLine("\nProtected/Private Access:");
            Console.WriteLine("  ✗ Cannot access 'author' field directly (protected)");
            Console.WriteLine("  ✗ Cannot access 'isCheckedOut' field directly (private)");
            Console.WriteLine("  ✓ Access through public property: IsCheckedOut");

            // Summary statistics
            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("   LIBRARY STATISTICS");
            Console.WriteLine(new string('═', 50));

            int totalItems = items.Length;
            int checkedOut = 0;
            foreach (var item in items)
            {
                if (item.IsCheckedOut)
                    checkedOut++;
            }

            Console.WriteLine($"\nTotal Items:     {totalItems}");
            Console.WriteLine($"Checked Out:     {checkedOut}");
            Console.WriteLine($"Available:       {totalItems - checkedOut}");

            // Key Learning Points
            Console.WriteLine("\n╔════════════════════════════════════════════════╗");
            Console.WriteLine("║         KEY LEARNING POINTS                    ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
            Console.WriteLine("\n✓ INHERITANCE: Book, Magazine, DVD inherit from");
            Console.WriteLine("  LibraryItem base class");
            Console.WriteLine("\n✓ METHOD OVERRIDING: Each subclass overrides");
            Console.WriteLine("  DisplayInfo() with specific implementation");
            Console.WriteLine("\n✓ ACCESS MODIFIERS:");
            Console.WriteLine("  - PUBLIC: ItemId, Title, methods (accessible anywhere)");
            Console.WriteLine("  - PROTECTED: title, author (accessible in subclasses)");
            Console.WriteLine("  - PRIVATE: isCheckedOut (only within class)");
            Console.WriteLine("  - INTERNAL: location (within same assembly)");
            Console.WriteLine("\n✓ POLYMORPHISM: Base class reference can hold");
            Console.WriteLine("  derived class objects (LibraryItem[] array)");
            Console.WriteLine("\n✓ Each item type calculates late fees differently");
            Console.WriteLine("  demonstrating polymorphic behavior");

            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
