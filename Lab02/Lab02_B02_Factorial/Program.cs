/*
 * Lab02_B02_Factorial
 * Description: Compute factorial of a number using for loop
 * Difficulty: B (Medium)
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("        FACTORIAL CALCULATOR");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            // Input number
            Console.Write("Enter a number: ");
            int number = Convert.ToInt32(Console.ReadLine());

            // Validate input
            if (number < 0)
            {
                Console.WriteLine("Error: Factorial is not defined for negative numbers.");
                return;
            }

            // Calculate factorial using for loop
            long factorial = 1;

            for (int i = 1; i <= number; i++)
            {
                factorial *= i;
            }

            // Display result
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("           RESULT");
            Console.WriteLine("========================================");
            Console.WriteLine($"Number:    {number}");
            Console.WriteLine($"Factorial: {factorial}");
            Console.WriteLine("========================================");

            // Show calculation steps for numbers up to 10
            if (number > 0 && number <= 10)
            {
                Console.WriteLine();
                Console.WriteLine("Calculation Steps:");
                Console.Write($"{number}! = ");
                for (int i = number; i >= 1; i--)
                {
                    Console.Write(i);
                    if (i > 1)
                        Console.Write(" x ");
                }
                Console.WriteLine($" = {factorial}");
                Console.WriteLine("========================================");
            }
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
