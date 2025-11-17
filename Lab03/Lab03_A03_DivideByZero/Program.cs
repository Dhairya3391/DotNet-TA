/*
 * Lab03_A03_DivideByZero
 * Problem: Accept two numbers from user, divide them, and handle divide-by-zero exception.
 *
 * Learning Objectives:
 * - Understanding exception handling with try-catch blocks
 * - Handling DivideByZeroException
 * - Input validation and error messages
 */

using System;

namespace Lab03_A03_DivideByZero
{
    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader("Division Calculator with Exception Handling");

            bool continueProgram = true;

            while (continueProgram)
            {
                try
                {
                    // Accept first number
                    Console.Write("\nEnter first number (dividend): ");
                    double num1 = Convert.ToDouble(Console.ReadLine());

                    // Accept second number
                    Console.Write("Enter second number (divisor): ");
                    double num2 = Convert.ToDouble(Console.ReadLine());

                    // Perform division with exception handling
                    Console.WriteLine(new string('-', 40));
                    double result = DivideNumbers(num1, num2);
                    Console.WriteLine($"Result: {num1} / {num2} = {result:F2}");
                }
                catch (DivideByZeroException)
                {
                    Console.WriteLine("\n*** ERROR: Division by zero is not allowed! ***");
                    Console.WriteLine("Please enter a non-zero divisor.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("\n*** ERROR: Invalid input! ***");
                    Console.WriteLine("Please enter valid numeric values.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"\n*** ERROR: {ex.Message} ***");
                }

                // Ask if user wants to continue
                Console.Write("\nDo you want to perform another division? (y/n): ");
                string choice = Console.ReadLine()?.ToLower();
                continueProgram = (choice == "y" || choice == "yes");
            }

            PrintFooter();
        }

        // Method to divide two numbers
        static double DivideNumbers(double dividend, double divisor)
        {
            if (divisor == 0)
            {
                throw new DivideByZeroException("Cannot divide by zero!");
            }
            return dividend / divisor;
        }

        // Helper method to print header
        static void PrintHeader(string title)
        {
            Console.WriteLine(new string('=', 50));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('=', 50));
        }

        // Helper method to print footer
        static void PrintFooter()
        {
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("Thank you for using the calculator!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
