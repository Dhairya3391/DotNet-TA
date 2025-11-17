using System;

namespace Lab05_B02_PrintableInterface
{
    // Interface - IPrintable
    // Demonstrates interface concept
    // Interfaces define a contract that implementing classes must follow
    interface IPrintable
    {
        // Interface method (no implementation)
        void PrintDetails();
    }

    // Class - Book implements IPrintable
    class Book : IPrintable
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public int Pages { get; set; }
        public double Price { get; set; }

        public Book(string title, string author, string isbn, int pages, double price)
        {
            Title = title;
            Author = author;
            ISBN = isbn;
            Pages = pages;
            Price = price;
        }

        // Implement PrintDetails method from IPrintable interface
        public void PrintDetails()
        {
            Console.WriteLine("┌─────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                    BOOK DETAILS                         │");
            Console.WriteLine("├─────────────────────────────────────────────────────────┤");
            Console.WriteLine($"│ Title      : {Title,-43} │");
            Console.WriteLine($"│ Author     : {Author,-43} │");
            Console.WriteLine($"│ ISBN       : {ISBN,-43} │");
            Console.WriteLine($"│ Pages      : {Pages,-43} │");
            Console.WriteLine($"│ Price      : Rs. {Price,-39:F2} │");
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }
    }

    // Class - Magazine implements IPrintable
    class Magazine : IPrintable
    {
        public string Name { get; set; }
        public string Publisher { get; set; }
        public string IssueNumber { get; set; }
        public DateTime PublicationDate { get; set; }
        public double Price { get; set; }

        public Magazine(string name, string publisher, string issueNumber, DateTime publicationDate, double price)
        {
            Name = name;
            Publisher = publisher;
            IssueNumber = issueNumber;
            PublicationDate = publicationDate;
            Price = price;
        }

        // Implement PrintDetails method from IPrintable interface
        public void PrintDetails()
        {
            Console.WriteLine("┌─────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                  MAGAZINE DETAILS                       │");
            Console.WriteLine("├─────────────────────────────────────────────────────────┤");
            Console.WriteLine($"│ Name       : {Name,-43} │");
            Console.WriteLine($"│ Publisher  : {Publisher,-43} │");
            Console.WriteLine($"│ Issue No.  : {IssueNumber,-43} │");
            Console.WriteLine($"│ Pub. Date  : {PublicationDate.ToString("dd MMM yyyy"),-43} │");
            Console.WriteLine($"│ Price      : Rs. {Price,-39:F2} │");
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }
    }

    // Additional class - Newspaper implements IPrintable
    class Newspaper : IPrintable
    {
        public string Name { get; set; }
        public string Language { get; set; }
        public DateTime Date { get; set; }
        public int Pages { get; set; }
        public double Price { get; set; }

        public Newspaper(string name, string language, DateTime date, int pages, double price)
        {
            Name = name;
            Language = language;
            Date = date;
            Pages = pages;
            Price = price;
        }

        // Implement PrintDetails method from IPrintable interface
        public void PrintDetails()
        {
            Console.WriteLine("┌─────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                 NEWSPAPER DETAILS                       │");
            Console.WriteLine("├─────────────────────────────────────────────────────────┤");
            Console.WriteLine($"│ Name       : {Name,-43} │");
            Console.WriteLine($"│ Language   : {Language,-43} │");
            Console.WriteLine($"│ Date       : {Date.ToString("dd MMM yyyy"),-43} │");
            Console.WriteLine($"│ Pages      : {Pages,-43} │");
            Console.WriteLine($"│ Price      : Rs. {Price,-39:F2} │");
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Display header
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║        Lab 05 - B02: IPrintable Interface Demo             ║");
            Console.WriteLine("║         (Interface Implementation)                         ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Creating Printable Objects:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                // Create Book objects
                Book book1 = new Book(
                    "The Complete Guide to C# Programming",
                    "John Smith",
                    "978-0-123456-78-9",
                    850,
                    1299.00
                );

                Book book2 = new Book(
                    "Data Structures and Algorithms",
                    "Jane Doe",
                    "978-0-987654-32-1",
                    650,
                    899.00
                );

                // Create Magazine objects
                Magazine mag1 = new Magazine(
                    "Tech Today",
                    "Future Publishing",
                    "Vol 25, Issue 3",
                    new DateTime(2025, 11, 1),
                    150.00
                );

                Magazine mag2 = new Magazine(
                    "Science Weekly",
                    "Knowledge Press",
                    "Issue 142",
                    new DateTime(2025, 11, 15),
                    120.00
                );

                // Create Newspaper objects
                Newspaper news1 = new Newspaper(
                    "The Daily Chronicle",
                    "English",
                    DateTime.Today,
                    32,
                    10.00
                );

                Newspaper news2 = new Newspaper(
                    "Gujarat Samachar",
                    "Gujarati",
                    DateTime.Today,
                    24,
                    8.00
                );

                // Demonstrate interface usage
                Console.WriteLine("[PRINTING BOOKS]\n");
                book1.PrintDetails();
                Console.WriteLine();
                book2.PrintDetails();
                Console.WriteLine();

                Console.WriteLine("\n[PRINTING MAGAZINES]\n");
                mag1.PrintDetails();
                Console.WriteLine();
                mag2.PrintDetails();
                Console.WriteLine();

                Console.WriteLine("\n[PRINTING NEWSPAPERS]\n");
                news1.PrintDetails();
                Console.WriteLine();
                news2.PrintDetails();
                Console.WriteLine();

                // Polymorphism with interface
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Polymorphism with IPrintable Interface:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                // Store different objects in IPrintable array
                IPrintable[] printables = { book1, mag1, news1, book2, mag2, news2 };

                Console.WriteLine($"Total printable items: {printables.Length}");
                Console.WriteLine("\nPrinting all items using interface reference:\n");

                foreach (IPrintable item in printables)
                {
                    item.PrintDetails();
                    Console.WriteLine();
                }

                // Explain interface concept
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Interface Concept Demonstration:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("✓ IPrintable is an interface with PrintDetails() method");
                Console.WriteLine("✓ Book, Magazine, Newspaper implement IPrintable");
                Console.WriteLine("✓ Each class provides its own implementation");
                Console.WriteLine("✓ Interface provides a contract for all implementing classes");
                Console.WriteLine("✓ Polymorphism achieved through interface reference");
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR] {ex.Message}");
            }

            // Footer
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              Program Completed Successfully                ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        }
    }
}
