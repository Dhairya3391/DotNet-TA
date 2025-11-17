using System;
using System.Collections.Generic;
using System.Linq;

namespace Lab06_B03_WordCount
{
    /// <summary>
    /// Program to count word occurrences using Dictionary<string, int>
    /// Key = Word, Value = Count
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║     WORD COUNT USING DICTIONARY<STRING, INT>      ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Demonstrate with hardcoded examples
            Console.WriteLine("--- Hardcoded Examples ---");
            Console.WriteLine();

            string[] testSentences = {
                "the quick brown fox jumps over the lazy dog the fox is quick",
                "to be or not to be that is the question",
                "programming is fun programming is creative programming is rewarding",
                "hello world hello everyone welcome to the world of programming"
            };

            foreach (string sentence in testSentences)
            {
                CountWords(sentence);
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
                Console.WriteLine("1. Count Words in Sentence");
                Console.WriteLine("2. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter a sentence: ");
                        string input = Console.ReadLine();

                        if (!string.IsNullOrWhiteSpace(input))
                        {
                            CountWords(input);
                        }
                        else
                        {
                            Console.WriteLine("❌ Please enter a valid sentence!");
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
        /// Count word occurrences using Dictionary<string, int>
        /// </summary>
        static void CountWords(string sentence)
        {
            // Dictionary to store word counts (Key = word, Value = count)
            Dictionary<string, int> wordCount = new Dictionary<string, int>();

            // Clean and split the sentence into words
            string cleanedSentence = RemovePunctuation(sentence.ToLower());
            string[] words = cleanedSentence.Split(new char[] { ' ', '\t', '\n', '\r' },
                                                   StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0)
            {
                Console.WriteLine("❌ No words found in the sentence!");
                return;
            }

            // Count occurrences using Dictionary
            foreach (string word in words)
            {
                if (wordCount.ContainsKey(word))
                {
                    // Word already exists, increment count
                    wordCount[word]++;
                }
                else
                {
                    // New word, add to dictionary with count 1
                    wordCount.Add(word, 1);
                }
            }

            // Display results
            Console.WriteLine("┌────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ Sentence: {TruncateString(sentence, 40),-40}│");
            Console.WriteLine("├────────────────────────────────────────────────────┤");
            Console.WriteLine($"│ Total Words:    {words.Length,-34}│");
            Console.WriteLine($"│ Unique Words:   {wordCount.Count,-34}│");
            Console.WriteLine("└────────────────────────────────────────────────────┘");
            Console.WriteLine();

            Console.WriteLine("Word Count Dictionary:");
            Console.WriteLine("┌────┬──────────────────────┬───────────┬────────────┐");
            Console.WriteLine("│ No │ Word (Key)           │ Count     │ Percentage │");
            Console.WriteLine("├────┼──────────────────────┼───────────┼────────────┤");

            // Sort by count (descending) using LINQ
            var sortedWords = wordCount.OrderByDescending(x => x.Value).ThenBy(x => x.Key);

            int index = 1;
            foreach (var kvp in sortedWords)
            {
                double percentage = (kvp.Value * 100.0) / words.Length;
                Console.WriteLine($"│ {index,-2} │ {kvp.Key,-20} │ {kvp.Value,-9} │ {percentage,9:F2}% │");
                index++;
            }

            Console.WriteLine("└────┴──────────────────────┴───────────┴────────────┘");

            // Display Dictionary operations explanation
            Console.WriteLine();
            Console.WriteLine("Dictionary Operations Used:");
            Console.WriteLine("  • ContainsKey(word)  - Check if word exists in dictionary");
            Console.WriteLine("  • Add(word, 1)       - Add new word with count 1");
            Console.WriteLine("  • wordCount[word]++  - Increment count for existing word");
            Console.WriteLine("  • wordCount.Count    - Get number of unique words");
            Console.WriteLine("  • foreach with KeyValuePair - Iterate through dictionary");
        }

        /// <summary>
        /// Remove common punctuation marks from string
        /// </summary>
        static string RemovePunctuation(string input)
        {
            char[] punctuation = { '.', ',', '!', '?', ';', ':', '"', '\'', '(', ')', '[', ']', '{', '}', '-', '_' };

            foreach (char p in punctuation)
            {
                input = input.Replace(p.ToString(), "");
            }

            return input;
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
