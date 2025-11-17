using System;

namespace Lab06_A03_VowelConsonant
{
    /// <summary>
    /// Program to count vowels and consonants in a string
    /// Ignores spaces and handles case-insensitivity
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║       VOWEL AND CONSONANT COUNTER                 ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Demonstrate with hardcoded examples
            Console.WriteLine("--- Hardcoded Examples ---");
            Console.WriteLine();

            string[] testStrings = {
                "Hello World",
                "Programming in C Sharp",
                "ASP.NET Core Development",
                "AEIOU",
                "bcdfg",
                "The Quick Brown Fox Jumps Over The Lazy Dog"
            };

            foreach (string testString in testStrings)
            {
                CountVowelsAndConsonants(testString);
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
                Console.WriteLine("1. Count Vowels and Consonants");
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
                            CountVowelsAndConsonants(input);
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
        /// Count vowels and consonants in the given string
        /// </summary>
        static void CountVowelsAndConsonants(string input)
        {
            int vowelCount = 0;
            int consonantCount = 0;
            int spaceCount = 0;
            int otherCount = 0;

            // Convert to lowercase for case-insensitive comparison
            string lowerInput = input.ToLower();

            // Define vowels
            string vowels = "aeiou";

            // Count characters
            foreach (char c in lowerInput)
            {
                if (char.IsLetter(c))
                {
                    // Check if the character is a vowel
                    if (vowels.Contains(c))
                    {
                        vowelCount++;
                    }
                    else
                    {
                        consonantCount++;
                    }
                }
                else if (char.IsWhiteSpace(c))
                {
                    spaceCount++;
                }
                else
                {
                    otherCount++;
                }
            }

            // Display results
            Console.WriteLine("┌────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ Input String: {TruncateString(input, 36),-36}│");
            Console.WriteLine("├────────────────────────────────────────────────────┤");
            Console.WriteLine($"│ Total Characters: {input.Length,-32}│");
            Console.WriteLine($"│ Vowels:           {vowelCount,-32}│");
            Console.WriteLine($"│ Consonants:       {consonantCount,-32}│");
            Console.WriteLine($"│ Spaces:           {spaceCount,-32}│");
            Console.WriteLine($"│ Other Characters: {otherCount,-32}│");
            Console.WriteLine("└────────────────────────────────────────────────────┘");

            // Display detailed breakdown
            if (input.Length > 0)
            {
                Console.WriteLine();
                Console.WriteLine("Detailed Analysis:");

                string vowelsList = "";
                string consonantsList = "";

                foreach (char c in lowerInput)
                {
                    if (char.IsLetter(c))
                    {
                        if (vowels.Contains(c))
                        {
                            vowelsList += c + " ";
                        }
                        else
                        {
                            consonantsList += c + " ";
                        }
                    }
                }

                if (!string.IsNullOrWhiteSpace(vowelsList))
                {
                    Console.WriteLine($"Vowels found:     {vowelsList.Trim()}");
                }

                if (!string.IsNullOrWhiteSpace(consonantsList))
                {
                    Console.WriteLine($"Consonants found: {consonantsList.Trim()}");
                }

                // Calculate percentages
                int totalLetters = vowelCount + consonantCount;
                if (totalLetters > 0)
                {
                    double vowelPercentage = (vowelCount * 100.0) / totalLetters;
                    double consonantPercentage = (consonantCount * 100.0) / totalLetters;

                    Console.WriteLine();
                    Console.WriteLine($"Vowel Percentage:     {vowelPercentage:F2}%");
                    Console.WriteLine($"Consonant Percentage: {consonantPercentage:F2}%");
                }
            }
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
