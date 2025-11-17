using System;

namespace Lab06_B01_WordFrequency
{
    /// <summary>
    /// Program to count word occurrences in a sentence using string methods and loops
    /// Does not use Dictionary - uses manual counting with arrays
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║         WORD FREQUENCY COUNTER (MANUAL)           ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Demonstrate with hardcoded examples
            Console.WriteLine("--- Hardcoded Examples ---");
            Console.WriteLine();

            string[] testSentences = {
                "the quick brown fox jumps over the lazy dog the fox is quick",
                "hello world hello",
                "programming is fun programming is creative programming is rewarding",
                "to be or not to be that is the question"
            };

            foreach (string sentence in testSentences)
            {
                CountWordFrequency(sentence);
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
                Console.WriteLine("1. Count Word Frequency in Sentence");
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
                            CountWordFrequency(input);
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
        /// Count word frequency using manual counting (without Dictionary)
        /// </summary>
        static void CountWordFrequency(string sentence)
        {
            // Split sentence into words and convert to lowercase
            string cleanedSentence = RemovePunctuation(sentence.ToLower());
            string[] words = cleanedSentence.Split(new char[] { ' ', '\t', '\n', '\r' },
                                                   StringSplitOptions.RemoveEmptyEntries);

            if (words.Length == 0)
            {
                Console.WriteLine("❌ No words found in the sentence!");
                return;
            }

            // Arrays to store unique words and their counts
            string[] uniqueWords = new string[words.Length];
            int[] wordCounts = new int[words.Length];
            int uniqueWordCount = 0;

            // Count occurrences of each word
            foreach (string word in words)
            {
                // Check if word already exists in uniqueWords array
                int existingIndex = -1;
                for (int i = 0; i < uniqueWordCount; i++)
                {
                    if (uniqueWords[i] == word)
                    {
                        existingIndex = i;
                        break;
                    }
                }

                if (existingIndex != -1)
                {
                    // Word already exists, increment count
                    wordCounts[existingIndex]++;
                }
                else
                {
                    // New word, add to arrays
                    uniqueWords[uniqueWordCount] = word;
                    wordCounts[uniqueWordCount] = 1;
                    uniqueWordCount++;
                }
            }

            // Display results
            Console.WriteLine("┌────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ Sentence: {TruncateString(sentence, 40),-40}│");
            Console.WriteLine("├────────────────────────────────────────────────────┤");
            Console.WriteLine($"│ Total Words:    {words.Length,-34}│");
            Console.WriteLine($"│ Unique Words:   {uniqueWordCount,-34}│");
            Console.WriteLine("└────────────────────────────────────────────────────┘");
            Console.WriteLine();

            Console.WriteLine("Word Frequency Table:");
            Console.WriteLine("┌────┬──────────────────────┬───────────┬────────────┐");
            Console.WriteLine("│ No │ Word                 │ Count     │ Percentage │");
            Console.WriteLine("├────┼──────────────────────┼───────────┼────────────┤");

            // Sort by count (descending) using bubble sort
            for (int i = 0; i < uniqueWordCount - 1; i++)
            {
                for (int j = 0; j < uniqueWordCount - i - 1; j++)
                {
                    if (wordCounts[j] < wordCounts[j + 1])
                    {
                        // Swap counts
                        int tempCount = wordCounts[j];
                        wordCounts[j] = wordCounts[j + 1];
                        wordCounts[j + 1] = tempCount;

                        // Swap words
                        string tempWord = uniqueWords[j];
                        uniqueWords[j] = uniqueWords[j + 1];
                        uniqueWords[j + 1] = tempWord;
                    }
                }
            }

            // Display sorted results
            for (int i = 0; i < uniqueWordCount; i++)
            {
                double percentage = (wordCounts[i] * 100.0) / words.Length;
                Console.WriteLine($"│ {i + 1,-2} │ {uniqueWords[i],-20} │ {wordCounts[i],-9} │ {percentage,9:F2}% │");
            }

            Console.WriteLine("└────┴──────────────────────┴───────────┴────────────┘");
        }

        /// <summary>
        /// Remove common punctuation marks from string
        /// </summary>
        static string RemovePunctuation(string input)
        {
            char[] punctuation = { '.', ',', '!', '?', ';', ':', '"', '\'', '(', ')', '[', ']', '{', '}' };

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
