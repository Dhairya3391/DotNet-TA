/*
 * Lab02_C05_Fibonacci
 * Description: Display first n terms of Fibonacci series
 * Difficulty: C (Complex)
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("       FIBONACCI SERIES GENERATOR");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine("Fibonacci series: Each term is the sum");
        Console.WriteLine("of the two preceding terms");
        Console.WriteLine("(0, 1, 1, 2, 3, 5, 8, 13, ...)");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            // Input number of terms
            Console.Write("Enter number of terms: ");
            int n = Convert.ToInt32(Console.ReadLine());

            // Validate input
            if (n <= 0)
            {
                Console.WriteLine("Error: Please enter a positive number.");
                return;
            }

            // Display header
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine($"   First {n} Terms of Fibonacci Series");
            Console.WriteLine("========================================");
            Console.WriteLine();

            // Initialize first two terms
            long first = 0;
            long second = 1;

            // Generate and display Fibonacci series
            for (int i = 1; i <= n; i++)
            {
                if (i == 1)
                {
                    Console.Write($"{first}");
                }
                else if (i == 2)
                {
                    Console.Write($", {second}");
                }
                else
                {
                    long next = first + second;
                    Console.Write($", {next}");
                    first = second;
                    second = next;
                }
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("========================================");

            // Show detailed calculation for first few terms (if n <= 10)
            if (n > 2 && n <= 10)
            {
                Console.WriteLine();
                Console.WriteLine("Calculation Steps:");
                Console.WriteLine("----------------------------------------");
                first = 0;
                second = 1;

                Console.WriteLine($"Term 1: {first}");
                Console.WriteLine($"Term 2: {second}");

                for (int i = 3; i <= n; i++)
                {
                    long next = first + second;
                    Console.WriteLine($"Term {i}: {first} + {second} = {next}");
                    first = second;
                    second = next;
                }

                Console.WriteLine("========================================");
            }

            // Calculate sum of series
            first = 0;
            second = 1;
            long sum = 0;

            for (int i = 1; i <= n; i++)
            {
                if (i == 1)
                {
                    sum += first;
                }
                else if (i == 2)
                {
                    sum += second;
                }
                else
                {
                    long next = first + second;
                    sum += next;
                    first = second;
                    second = next;
                }
            }

            Console.WriteLine();
            Console.WriteLine($"Sum of first {n} terms: {sum}");
            Console.WriteLine("========================================");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Please enter a valid integer value.");
        }
        catch (OverflowException)
        {
            Console.WriteLine("Error: Number is too large. Result exceeds maximum value.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
