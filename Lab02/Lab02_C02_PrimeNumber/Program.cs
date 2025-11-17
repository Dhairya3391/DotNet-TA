/*
 * Lab02_C02_PrimeNumber
 * Description: Check if a number is prime
 * Difficulty: C (Complex)
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("         PRIME NUMBER CHECKER");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            // Input number
            Console.Write("Enter a number: ");
            int number = Convert.ToInt32(Console.ReadLine());

            // Check if number is prime
            bool isPrime = true;
            int divisorCount = 0;

            if (number <= 1)
            {
                isPrime = false;
            }
            else if (number == 2)
            {
                isPrime = true;
            }
            else if (number % 2 == 0)
            {
                isPrime = false;
            }
            else
            {
                // Check for divisors from 3 to sqrt(number)
                for (int i = 3; i * i <= number; i += 2)
                {
                    if (number % i == 0)
                    {
                        isPrime = false;
                        break;
                    }
                }
            }

            // Display result
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("           RESULT");
            Console.WriteLine("========================================");
            Console.WriteLine($"Number: {number}");
            Console.WriteLine($"Result: {(isPrime ? "PRIME NUMBER" : "NOT A PRIME NUMBER")}");
            Console.WriteLine("========================================");

            // Additional information
            if (isPrime && number > 1)
            {
                Console.WriteLine();
                Console.WriteLine($"{number} is divisible only by 1 and {number}");
            }
            else if (number <= 1)
            {
                Console.WriteLine();
                Console.WriteLine("Note: Numbers less than or equal to 1 are not prime.");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("Divisors of " + number + ":");
                for (int i = 1; i <= number; i++)
                {
                    if (number % i == 0)
                    {
                        Console.Write($"{i} ");
                        divisorCount++;
                    }
                }
                Console.WriteLine();
                Console.WriteLine($"Total divisors: {divisorCount}");
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
