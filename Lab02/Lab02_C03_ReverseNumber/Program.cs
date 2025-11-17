/*
 * Lab02_C03_ReverseNumber
 * Description: Reverse digits of a number using while loop
 * Difficulty: C (Complex)
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("         NUMBER REVERSER");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            // Input number
            Console.Write("Enter a number: ");
            int number = Convert.ToInt32(Console.ReadLine());

            // Store original number and handle negative numbers
            int originalNumber = number;
            bool isNegative = number < 0;
            number = Math.Abs(number);

            // Reverse the number using while loop
            int reversedNumber = 0;
            int temp = number;

            while (temp > 0)
            {
                int digit = temp % 10;
                reversedNumber = reversedNumber * 10 + digit;
                temp = temp / 10;
            }

            // Apply negative sign if original was negative
            if (isNegative)
            {
                reversedNumber = -reversedNumber;
            }

            // Display result
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("           RESULT");
            Console.WriteLine("========================================");
            Console.WriteLine($"Original Number:  {originalNumber}");
            Console.WriteLine($"Reversed Number:  {reversedNumber}");
            Console.WriteLine("========================================");

            // Show step-by-step reversal process
            Console.WriteLine();
            Console.WriteLine("Reversal Process:");
            Console.WriteLine("----------------------------------------");
            temp = Math.Abs(originalNumber);
            int step = 1;
            int result = 0;

            while (temp > 0)
            {
                int digit = temp % 10;
                result = result * 10 + digit;
                Console.WriteLine($"Step {step}: Extract digit {digit}, Result = {result}");
                temp = temp / 10;
                step++;
            }

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
