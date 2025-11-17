/*
 * Lab02_C01_StrongPassword
 * Description: Check password strength (8+ chars, uppercase, lowercase, digit, special char)
 * Difficulty: C (Complex)
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("      PASSWORD STRENGTH CHECKER");
        Console.WriteLine("========================================");
        Console.WriteLine();
        Console.WriteLine("A strong password must have:");
        Console.WriteLine("- At least 8 characters");
        Console.WriteLine("- At least one uppercase letter");
        Console.WriteLine("- At least one lowercase letter");
        Console.WriteLine("- At least one digit");
        Console.WriteLine("- At least one special character");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            // Input password
            Console.Write("Enter password: ");
            string password = Console.ReadLine();

            // Check password strength criteria
            bool hasMinLength = password.Length >= 8;
            bool hasUppercase = false;
            bool hasLowercase = false;
            bool hasDigit = false;
            bool hasSpecialChar = false;

            // Check each character
            for (int i = 0; i < password.Length; i++)
            {
                char ch = password[i];

                if (char.IsUpper(ch))
                    hasUppercase = true;
                else if (char.IsLower(ch))
                    hasLowercase = true;
                else if (char.IsDigit(ch))
                    hasDigit = true;
                else if (!char.IsWhiteSpace(ch))
                    hasSpecialChar = true;
            }

            // Determine if password is strong
            bool isStrong = hasMinLength && hasUppercase && hasLowercase && hasDigit && hasSpecialChar;

            // Display results
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("         PASSWORD ANALYSIS");
            Console.WriteLine("========================================");
            Console.WriteLine($"Length: {password.Length} chars {(hasMinLength ? "[✓]" : "[✗]")}");
            Console.WriteLine($"Uppercase letter:   {(hasUppercase ? "[✓]" : "[✗]")}");
            Console.WriteLine($"Lowercase letter:   {(hasLowercase ? "[✓]" : "[✗]")}");
            Console.WriteLine($"Digit:              {(hasDigit ? "[✓]" : "[✗]")}");
            Console.WriteLine($"Special character:  {(hasSpecialChar ? "[✓]" : "[✗]")}");
            Console.WriteLine("========================================");
            Console.WriteLine();

            if (isStrong)
            {
                Console.WriteLine("Result: STRONG PASSWORD");
            }
            else
            {
                Console.WriteLine("Result: WEAK PASSWORD");
                Console.WriteLine();
                Console.WriteLine("Missing criteria:");
                if (!hasMinLength)
                    Console.WriteLine("- Password must be at least 8 characters long");
                if (!hasUppercase)
                    Console.WriteLine("- Add at least one uppercase letter (A-Z)");
                if (!hasLowercase)
                    Console.WriteLine("- Add at least one lowercase letter (a-z)");
                if (!hasDigit)
                    Console.WriteLine("- Add at least one digit (0-9)");
                if (!hasSpecialChar)
                    Console.WriteLine("- Add at least one special character (!@#$%^&*)");
            }

            Console.WriteLine("========================================");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
