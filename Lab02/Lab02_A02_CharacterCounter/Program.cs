/*
 * Lab02_A02_CharacterCounter
 * Description: Count digits, alphabets, and special characters in a string
 * Difficulty: A (Easy)
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("       CHARACTER COUNTER");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            // Input string
            Console.Write("Enter a string: ");
            string input = Console.ReadLine();

            // Initialize counters
            int digitCount = 0;
            int alphabetCount = 0;
            int specialCharCount = 0;

            // Count characters using for loop
            for (int i = 0; i < input.Length; i++)
            {
                char ch = input[i];

                if (char.IsDigit(ch))
                {
                    digitCount++;
                }
                else if (char.IsLetter(ch))
                {
                    alphabetCount++;
                }
                else if (!char.IsWhiteSpace(ch))
                {
                    specialCharCount++;
                }
            }

            // Display results
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("           RESULTS");
            Console.WriteLine("========================================");
            Console.WriteLine($"Total Characters: {input.Length}");
            Console.WriteLine($"Digits:           {digitCount}");
            Console.WriteLine($"Alphabets:        {alphabetCount}");
            Console.WriteLine($"Special Chars:    {specialCharCount}");
            Console.WriteLine($"Spaces:           {input.Length - digitCount - alphabetCount - specialCharCount}");
            Console.WriteLine("========================================");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
