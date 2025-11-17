using System;
using System.Collections.Generic;

namespace Lab06_C01_LibraryBorrowing
{
    /// <summary>
    /// Program to manage library book borrowing system
    /// Uses Dictionary<string, Queue<string>> where:
    /// - Key: Book Title
    /// - Value: Queue of Borrower Names (FIFO order)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, Queue<string>> librarySystem = new Dictionary<string, Queue<string>>();

            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║    LIBRARY BORROWING SYSTEM (DICTIONARY + QUEUE)  ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine("--- Hardcoded Example ---");
            Console.WriteLine();

            Console.WriteLine("Setting up library system with sample data:");
            Console.WriteLine();

            AddBorrower(librarySystem, "C# Programming", "John Smith");
            AddBorrower(librarySystem, "C# Programming", "Sarah Johnson");
            AddBorrower(librarySystem, "C# Programming", "Michael Brown");
            AddBorrower(librarySystem, "ASP.NET Core", "Emily Davis");
            AddBorrower(librarySystem, "ASP.NET Core", "David Wilson");
            AddBorrower(librarySystem, "SQL Server", "Robert Garcia");
            AddBorrower(librarySystem, "Design Patterns", "Jennifer Martinez");
            AddBorrower(librarySystem, "Design Patterns", "William Anderson");

            Console.WriteLine();
            DisplayAllBooks(librarySystem);

            Console.WriteLine();
            Console.WriteLine("--- Processing Book Returns ---");
            Console.WriteLine();

            ProcessReturn(librarySystem, "C# Programming");
            ProcessReturn(librarySystem, "ASP.NET Core");

            Console.WriteLine();
            DisplayAllBooks(librarySystem);

            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine("           INTERACTIVE MODE");
            Console.WriteLine("═══════════════════════════════════════════════════");

            bool continueRunning = true;
            while (continueRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Add Borrower to Queue");
                Console.WriteLine("2. Process Book Return (Serve Next Borrower)");
                Console.WriteLine("3. View Next Borrower for Book");
                Console.WriteLine("4. Display All Books and Queues");
                Console.WriteLine("5. Display Specific Book Queue");
                Console.WriteLine("6. Check Queue Length for Book");
                Console.WriteLine("7. Remove Book from System");
                Console.WriteLine("8. Display System Statistics");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter book title: ");
                        string bookTitle = Console.ReadLine();
                        Console.Write("Enter borrower name: ");
                        string borrowerName = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(bookTitle) && !string.IsNullOrWhiteSpace(borrowerName))
                        {
                            AddBorrower(librarySystem, bookTitle.Trim(), borrowerName.Trim());
                        }
                        else
                        {
                            Console.WriteLine("❌ Book title and borrower name cannot be empty!");
                        }
                        break;

                    case "2":
                        Console.Write("Enter book title: ");
                        string bookToReturn = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(bookToReturn))
                        {
                            ProcessReturn(librarySystem, bookToReturn.Trim());
                        }
                        else
                        {
                            Console.WriteLine("❌ Book title cannot be empty!");
                        }
                        break;

                    case "3":
                        Console.Write("Enter book title: ");
                        string bookToCheck = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(bookToCheck))
                        {
                            ViewNextBorrower(librarySystem, bookToCheck.Trim());
                        }
                        else
                        {
                            Console.WriteLine("❌ Book title cannot be empty!");
                        }
                        break;

                    case "4":
                        DisplayAllBooks(librarySystem);
                        break;

                    case "5":
                        Console.Write("Enter book title: ");
                        string specificBook = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(specificBook))
                        {
                            DisplaySpecificBookQueue(librarySystem, specificBook.Trim());
                        }
                        else
                        {
                            Console.WriteLine("❌ Book title cannot be empty!");
                        }
                        break;

                    case "6":
                        Console.Write("Enter book title: ");
                        string bookForLength = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(bookForLength))
                        {
                            CheckQueueLength(librarySystem, bookForLength.Trim());
                        }
                        else
                        {
                            Console.WriteLine("❌ Book title cannot be empty!");
                        }
                        break;

                    case "7":
                        Console.Write("Enter book title to remove: ");
                        string bookToRemove = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(bookToRemove))
                        {
                            RemoveBook(librarySystem, bookToRemove.Trim());
                        }
                        else
                        {
                            Console.WriteLine("❌ Book title cannot be empty!");
                        }
                        break;

                    case "8":
                        DisplayStatistics(librarySystem);
                        break;

                    case "9":
                        continueRunning = false;
                        Console.WriteLine("Exiting Library System. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("❌ Invalid choice! Please try again.");
                        break;
                }
            }
        }

        static void AddBorrower(Dictionary<string, Queue<string>> library, string bookTitle, string borrowerName)
        {
            if (library.ContainsKey(bookTitle))
            {
                library[bookTitle].Enqueue(borrowerName);
                Console.WriteLine($"✓ {borrowerName} added to queue for '{bookTitle}'");
            }
            else
            {
                Queue<string> newQueue = new Queue<string>();
                newQueue.Enqueue(borrowerName);
                library.Add(bookTitle, newQueue);
                Console.WriteLine($"✓ New book '{bookTitle}' added with borrower {borrowerName}");
            }
        }

        static void ProcessReturn(Dictionary<string, Queue<string>> library, string bookTitle)
        {
            if (!library.ContainsKey(bookTitle))
            {
                Console.WriteLine($"❌ Book '{bookTitle}' not found in system!");
                return;
            }

            Queue<string> borrowerQueue = library[bookTitle];

            if (borrowerQueue.Count > 0)
            {
                string nextBorrower = borrowerQueue.Dequeue();
                Console.WriteLine($"✓ Book '{bookTitle}' returned and assigned to: {nextBorrower}");

                if (borrowerQueue.Count == 0)
                {
                    Console.WriteLine($"  ℹ️  No more borrowers in queue for '{bookTitle}'");
                }
            }
            else
            {
                Console.WriteLine($"❌ No borrowers in queue for '{bookTitle}'!");
            }
        }

        static void ViewNextBorrower(Dictionary<string, Queue<string>> library, string bookTitle)
        {
            if (!library.ContainsKey(bookTitle))
            {
                Console.WriteLine($"❌ Book '{bookTitle}' not found in system!");
                return;
            }

            Queue<string> borrowerQueue = library[bookTitle];

            if (borrowerQueue.Count > 0)
            {
                string nextBorrower = borrowerQueue.Peek();
                Console.WriteLine($"👁️  Next borrower for '{bookTitle}': {nextBorrower}");
            }
            else
            {
                Console.WriteLine($"❌ No borrowers in queue for '{bookTitle}'!");
            }
        }

        static void DisplayAllBooks(Dictionary<string, Queue<string>> library)
        {
            if (library.Count == 0)
            {
                Console.WriteLine("❌ No books in the library system!");
                return;
            }

            Console.WriteLine($"📚 Library System (Total Books: {library.Count}):");
            Console.WriteLine("════════════════════════════════════════════════════");

            foreach (var kvp in library)
            {
                string bookTitle = kvp.Key;
                Queue<string> borrowerQueue = kvp.Value;

                Console.WriteLine();
                Console.WriteLine($"📖 Book: {bookTitle}");
                Console.WriteLine($"   Borrowers in Queue: {borrowerQueue.Count}");

                if (borrowerQueue.Count > 0)
                {
                    Console.WriteLine("   ┌─────┬────────────────────────────────────┐");
                    Console.WriteLine("   │ Pos │ Borrower Name                      │");
                    Console.WriteLine("   ├─────┼────────────────────────────────────┤");

                    int position = 1;
                    foreach (string borrower in borrowerQueue)
                    {
                        Console.WriteLine($"   │ {position,-3} │ {borrower,-34} │");
                        position++;
                    }

                    Console.WriteLine("   └─────┴────────────────────────────────────┘");
                }
                else
                {
                    Console.WriteLine("   No borrowers waiting.");
                }
            }

            Console.WriteLine();
            Console.WriteLine("════════════════════════════════════════════════════");
        }

        static void DisplaySpecificBookQueue(Dictionary<string, Queue<string>> library, string bookTitle)
        {
            if (!library.ContainsKey(bookTitle))
            {
                Console.WriteLine($"❌ Book '{bookTitle}' not found in system!");
                return;
            }

            Queue<string> borrowerQueue = library[bookTitle];

            Console.WriteLine($"📖 Book: {bookTitle}");
            Console.WriteLine($"   Borrowers in Queue: {borrowerQueue.Count}");
            Console.WriteLine();

            if (borrowerQueue.Count > 0)
            {
                Console.WriteLine("┌─────┬────────────────────────────────────────┐");
                Console.WriteLine("│ Pos │ Borrower Name                          │");
                Console.WriteLine("├─────┼────────────────────────────────────────┤");

                int position = 1;
                foreach (string borrower in borrowerQueue)
                {
                    Console.WriteLine($"│ {position,-3} │ {borrower,-38} │");
                    position++;
                }

                Console.WriteLine("└─────┴────────────────────────────────────────┘");
            }
            else
            {
                Console.WriteLine("No borrowers in queue.");
            }
        }

        static void CheckQueueLength(Dictionary<string, Queue<string>> library, string bookTitle)
        {
            if (!library.ContainsKey(bookTitle))
            {
                Console.WriteLine($"❌ Book '{bookTitle}' not found in system!");
                return;
            }

            int queueLength = library[bookTitle].Count;
            Console.WriteLine($"📊 Queue length for '{bookTitle}': {queueLength} borrower(s)");
        }

        static void RemoveBook(Dictionary<string, Queue<string>> library, string bookTitle)
        {
            if (library.ContainsKey(bookTitle))
            {
                int borrowersCount = library[bookTitle].Count;
                library.Remove(bookTitle);
                Console.WriteLine($"✓ Book '{bookTitle}' removed from system.");

                if (borrowersCount > 0)
                {
                    Console.WriteLine($"  ⚠️  Warning: {borrowersCount} borrower(s) were in queue!");
                }
            }
            else
            {
                Console.WriteLine($"❌ Book '{bookTitle}' not found in system!");
            }
        }

        static void DisplayStatistics(Dictionary<string, Queue<string>> library)
        {
            if (library.Count == 0)
            {
                Console.WriteLine("❌ No books in the library system!");
                return;
            }

            int totalBooks = library.Count;
            int totalBorrowers = 0;
            int booksWithNoBorrowers = 0;
            string mostPopularBook = "";
            int maxBorrowers = 0;

            foreach (var kvp in library)
            {
                int borrowerCount = kvp.Value.Count;
                totalBorrowers += borrowerCount;

                if (borrowerCount == 0)
                {
                    booksWithNoBorrowers++;
                }

                if (borrowerCount > maxBorrowers)
                {
                    maxBorrowers = borrowerCount;
                    mostPopularBook = kvp.Key;
                }
            }

            Console.WriteLine("📊 Library System Statistics:");
            Console.WriteLine("┌────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ Total Books:              {totalBooks,-24}│");
            Console.WriteLine($"│ Total Borrowers in Queue: {totalBorrowers,-24}│");
            Console.WriteLine($"│ Books with No Borrowers:  {booksWithNoBorrowers,-24}│");
            Console.WriteLine($"│ Average Queue Length:     {(totalBooks > 0 ? (double)totalBorrowers / totalBooks : 0),-24:F2}│");

            if (!string.IsNullOrEmpty(mostPopularBook))
            {
                Console.WriteLine($"│ Most Popular Book:        {TruncateString(mostPopularBook, 24),-24}│");
                Console.WriteLine($"│   Borrowers Waiting:      {maxBorrowers,-24}│");
            }

            Console.WriteLine("└────────────────────────────────────────────────────┘");
        }

        static string TruncateString(string input, int maxLength)
        {
            if (input.Length <= maxLength)
            {
                return input;
            }
            else
            {
                return input.Substring(0, maxLength - 3) + "...";
            }
        }
    }
}
