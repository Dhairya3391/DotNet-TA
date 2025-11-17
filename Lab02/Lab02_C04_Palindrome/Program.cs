/*
 * Lab02_C04_Palindrome
 * Description: Check if a number is a palindrome
 * Difficulty: C (Complex)
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("        PALINDROME CHECKER");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine("A palindrome number reads the same");
        Console.WriteLine("forwards and backwards (e.g., 121, 1331)");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            // Input number
            Console.Write("Enter a number: ");
            int number = Convert.ToInt32(Console.ReadLine());

            // Store original number
            int originalNumber = number;

            // Handle negative numbers (they cannot be palindromes)
            if (number < 0)
            {
                Console.WriteLine();
                Console.WriteLine("========================================");
                Console.WriteLine("           RESULT");
                Console.WriteLine("========================================");
                Console.WriteLine($"Number: {number}");
                Console.WriteLine("Result: NOT A PALINDROME");
                Console.WriteLine("Note: Negative numbers are not palindromes");
                Console.WriteLine("========================================");
                return;
            }

            // Reverse the number using while loop
            int reversedNumber = 0;
            int temp = number;

            while (temp > 0)
            {
                int digit = temp % 10;
                reversedNumber = reversedNumber * 10 + digit;
                temp = temp / 10;
            }

            // Check if palindrome
            bool isPalindrome = (originalNumber == reversedNumber);

            // Display result
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("           RESULT");
            Console.WriteLine("========================================");
            Console.WriteLine($"Original Number:  {originalNumber}");
            Console.WriteLine($"Reversed Number:  {reversedNumber}");
            Console.WriteLine($"Result: {(isPalindrome ? "PALINDROME" : "NOT A PALINDROME")}");
            Console.WriteLine("========================================");

            // Show digit-by-digit comparison
            Console.WriteLine();
            Console.WriteLine("Digit Analysis:");
            Console.WriteLine("----------------------------------------");
            string originalStr = originalNumber.ToString();
            string reversedStr = reversedNumber.ToString();

            for (int i = 0; i < originalStr.Length; i++)
            {
                char origChar = originalStr[i];
                char revChar = reversedStr[reversedStr.Length - 1 - i];
                string match = (origChar == revChar) ? "[✓]" : "[✗]";
                Console.WriteLine($"Position {i + 1}: {origChar} vs {revChar} {match}");
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
