/*
 * Lab02_B01_SumEvenOdd
 * Description: Calculate sum of even and odd numbers separately up to a limit
 * Difficulty: B (Medium)
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("      EVEN & ODD SUM CALCULATOR");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            // Input limit
            Console.Write("Enter the limit: ");
            int limit = Convert.ToInt32(Console.ReadLine());

            if (limit < 1)
            {
                Console.WriteLine("Error: Limit must be a positive number.");
                return;
            }

            // Initialize sums
            int evenSum = 0;
            int oddSum = 0;

            // Calculate sums using for loop
            for (int i = 1; i <= limit; i++)
            {
                if (i % 2 == 0)
                {
                    evenSum += i;
                }
                else
                {
                    oddSum += i;
                }
            }

            // Display results
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("           RESULTS");
            Console.WriteLine("========================================");
            Console.WriteLine($"Range:      1 to {limit}");
            Console.WriteLine($"Even Sum:   {evenSum}");
            Console.WriteLine($"Odd Sum:    {oddSum}");
            Console.WriteLine($"Total Sum:  {evenSum + oddSum}");
            Console.WriteLine("========================================");

            // Additional information
            Console.WriteLine();
            Console.WriteLine("Even numbers in range:");
            for (int i = 2; i <= limit; i += 2)
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Odd numbers in range:");
            for (int i = 1; i <= limit; i += 2)
            {
                Console.Write($"{i} ");
            }
            Console.WriteLine();
            Console.WriteLine("========================================");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Please enter a valid integer value.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
