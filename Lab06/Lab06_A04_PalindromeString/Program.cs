using System;
using System.Text;

namespace Lab06_A04_PalindromeString
{
    /// <summary>
    /// Program to check if a string is a palindrome
    /// Ignores case and spaces
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║           PALINDROME STRING CHECKER               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Demonstrate with hardcoded examples
            Console.WriteLine("--- Hardcoded Examples ---");
            Console.WriteLine();

            string[] testStrings = {
                "radar",
                "Madam",
                "A man a plan a canal Panama",
                "racecar",
                "Hello",
                "Was it a car or a cat I saw",
                "noon",
                "level",
                "programming",
                "nurses run"
            };

            foreach (string testString in testStrings)
            {
                CheckPalindrome(testString);
                Console.WriteLine();
            }

            // Interactive mode
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine("           INTERACTIVE MODE");
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine();

            bool continueRunning = true;
            while (continueRunning)
            {
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Check if String is Palindrome");
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter a string: ");
                        string input = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            CheckPalindrome(input);
                        }
                        else
                        {
                            Console.WriteLine("❌ Please enter a valid string!");
                        }
                        Console.WriteLine();
                        break;

                    case "2":
                        continueRunning = false;
                        Console.WriteLine("Exiting program. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("❌ Invalid choice! Please try again.");
                        Console.WriteLine();
                        break;
                }
            }
        }

        /// <summary>
        /// Check if the given string is a palindrome
        /// </summary>
        static void CheckPalindrome(string input)
        {
            // Clean the string: remove spaces and convert to lowercase
            string cleanedString = RemoveSpaces(input.ToLower());

            // Reverse the cleaned string
            string reversedString = ReverseString(cleanedString);

            // Check if the cleaned string equals its reverse
            bool isPalindrome = cleanedString.Equals(reversedString);

            // Display results
            Console.WriteLine("┌────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ Original String:  {TruncateString(input, 32),-32}│");
            Console.WriteLine($"│ Cleaned String:   {TruncateString(cleanedString, 32),-32}│");
            Console.WriteLine($"│ Reversed String:  {TruncateString(reversedString, 32),-32}│");
            Console.WriteLine("├────────────────────────────────────────────────────┤");

            if (isPalindrome)
            {
                Console.WriteLine("│ Result: ✓ YES, it is a PALINDROME!                │");
            }
            else
            {
                Console.WriteLine("│ Result: ✗ NO, it is NOT a palindrome.             │");
            }

            Console.WriteLine("└────────────────────────────────────────────────────┘");

            // Additional information
            Console.WriteLine();
            Console.WriteLine("Analysis:");
            Console.WriteLine($"  • Original length: {input.Length}");
            Console.WriteLine($"  • Cleaned length:  {cleanedString.Length}");
            Console.WriteLine($"  • Ignores case:    Yes");
            Console.WriteLine($"  • Ignores spaces:  Yes");
        }

        /// <summary>
        /// Remove all spaces from a string
        /// </summary>
        static string RemoveSpaces(string input)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (!char.IsWhiteSpace(c))
                {
                    result.Append(c);
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Reverse a string
        /// </summary>
        static string ReverseString(string input)
        {
            char[] charArray = input.ToCharArray();
            Array.Reverse(charArray);
            return new string(charArray);
        }

        /// <summary>
        /// Truncate string if it's too long for display
        /// </summary>
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
