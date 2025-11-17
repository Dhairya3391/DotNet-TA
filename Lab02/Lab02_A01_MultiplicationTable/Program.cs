/*
 * Lab02_A01_MultiplicationTable
 * Description: Display multiplication table of a given number using for loop
 * Difficulty: A (Easy)
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("    MULTIPLICATION TABLE GENERATOR");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            // Input number
            Console.Write("Enter a number: ");
            int number = Convert.ToInt32(Console.ReadLine());

            // Input range
            Console.Write("Enter range (e.g., 10 for 1-10): ");
            int range = Convert.ToInt32(Console.ReadLine());

            Console.WriteLine();
            Console.WriteLine($"========================================");
            Console.WriteLine($"   Multiplication Table of {number}");
            Console.WriteLine($"========================================");

            // Generate multiplication table using for loop
            for (int i = 1; i <= range; i++)
            {
                int result = number * i;
                Console.WriteLine($"{number} x {i,2} = {result,4}");
            }

            Console.WriteLine("========================================");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Please enter valid integer values.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
