using System;
using System.Collections.Generic;

namespace Lab06_B04_EmailSet
{
    /// <summary>
    /// Program to manage email addresses using HashSet<string>
    /// HashSet automatically prevents duplicate entries
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // HashSet to store unique email addresses
            HashSet<string> emailSet = new HashSet<string>();

            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║      EMAIL ADDRESS MANAGER (HASHSET)              ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Demonstrate with hardcoded examples
            Console.WriteLine("--- Hardcoded Example ---");
            Console.WriteLine();

            Console.WriteLine("Adding email addresses to HashSet:");
            AddEmail(emailSet, "john.smith@example.com");
            AddEmail(emailSet, "sarah.johnson@example.com");
            AddEmail(emailSet, "michael.brown@example.com");
            AddEmail(emailSet, "emily.davis@example.com");
            AddEmail(emailSet, "john.smith@example.com"); // Duplicate
            AddEmail(emailSet, "JOHN.SMITH@EXAMPLE.COM"); // Different case
            AddEmail(emailSet, "david.wilson@example.com");

            Console.WriteLine();
            DisplayAllEmails(emailSet);

            Console.WriteLine();
            Console.WriteLine("--- Removing Emails ---");
            RemoveEmail(emailSet, "michael.brown@example.com");
            RemoveEmail(emailSet, "test@example.com"); // Not in set

            Console.WriteLine();
            DisplayAllEmails(emailSet);

            // Interactive menu
            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine("           INTERACTIVE MODE");
            Console.WriteLine("═══════════════════════════════════════════════════");

            bool continueRunning = true;
            while (continueRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Add Email Address");
                Console.WriteLine("2. Remove Email Address");
                Console.WriteLine("3. Check if Email Exists");
                Console.WriteLine("4. Display All Emails");
                Console.WriteLine("5. Display Email Count");
                Console.WriteLine("6. Clear All Emails");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter email address: ");
                        string email = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(email))
                            AddEmail(emailSet, email.Trim());
                        else
                            Console.WriteLine("❌ Email address cannot be empty!");
                        break;

                    case "2":
                        Console.Write("Enter email address to remove: ");
                        string emailToRemove = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(emailToRemove))
                            RemoveEmail(emailSet, emailToRemove.Trim());
                        else
                            Console.WriteLine("❌ Email address cannot be empty!");
                        break;

                    case "3":
                        Console.Write("Enter email address to check: ");
                        string emailToCheck = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(emailToCheck))
                            CheckEmailExists(emailSet, emailToCheck.Trim());
                        else
                            Console.WriteLine("❌ Email address cannot be empty!");
                        break;

                    case "4":
                        DisplayAllEmails(emailSet);
                        break;

                    case "5":
                        Console.WriteLine($"📊 Total unique email addresses: {emailSet.Count}");
                        break;

                    case "6":
                        emailSet.Clear();
                        Console.WriteLine("✓ All emails cleared!");
                        break;

                    case "7":
                        continueRunning = false;
                        Console.WriteLine("Exiting Email Manager. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("❌ Invalid choice! Please try again.");
                        break;
                }
            }
        }

        static void AddEmail(HashSet<string> set, string email)
        {
            if (!IsValidEmail(email))
            {
                Console.WriteLine($"❌ Invalid email format: {email}");
                return;
            }

            bool added = set.Add(email);

            if (added)
                Console.WriteLine($"✓ Email added: {email}");
            else
                Console.WriteLine($"❌ Email '{email}' already exists! HashSet prevents duplicates.");
        }

        static void RemoveEmail(HashSet<string> set, string email)
        {
            bool removed = set.Remove(email);

            if (removed)
                Console.WriteLine($"✓ Email removed: {email}");
            else
                Console.WriteLine($"❌ Email '{email}' not found in the set!");
        }

        static void CheckEmailExists(HashSet<string> set, string email)
        {
            bool exists = set.Contains(email);

            if (exists)
                Console.WriteLine($"✓ Email '{email}' exists in the set!");
            else
                Console.WriteLine($"❌ Email '{email}' does not exist in the set!");
        }

        static void DisplayAllEmails(HashSet<string> set)
        {
            if (set.Count > 0)
            {
                Console.WriteLine($"📧 Email Addresses (Total: {set.Count} unique emails):");
                Console.WriteLine("┌────┬──────────────────────────────────────────────┐");
                Console.WriteLine("│ No │ Email Address                                │");
                Console.WriteLine("├────┼──────────────────────────────────────────────┤");

                int index = 1;
                foreach (string email in set)
                {
                    Console.WriteLine($"│ {index,-2} │ {email,-44} │");
                    index++;
                }

                Console.WriteLine("└────┴──────────────────────────────────────────────┘");
            }
            else
            {
                Console.WriteLine("❌ No emails in the set!");
            }
        }

        static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            int atIndex = email.IndexOf('@');
            if (atIndex <= 0 || atIndex == email.Length - 1)
                return false;

            int dotIndex = email.LastIndexOf('.');
            if (dotIndex <= atIndex || dotIndex == email.Length - 1)
                return false;

            return true;
        }
    }
}
