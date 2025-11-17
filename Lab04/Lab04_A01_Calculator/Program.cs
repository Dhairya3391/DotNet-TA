/*
 * Lab04_A01_Calculator
 * Demonstrates method overloading with different parameter types and counts
 * Calculator class with overloaded Add() methods
 */

using System;

namespace Lab04_A01_Calculator
{
    // Calculator class demonstrating method overloading
    class Calculator
    {
        // Overloaded method 1: Add two integers
        public int Add(int a, int b)
        {
            return a + b;
        }

        // Overloaded method 2: Add three integers
        public int Add(int a, int b, int c)
        {
            return a + b + c;
        }

        // Overloaded method 3: Add two doubles
        public double Add(double a, double b)
        {
            return a + b;
        }

        // Display calculator information
        public void DisplayInfo()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════╗");
            Console.WriteLine("║      CALCULATOR - METHOD OVERLOADING       ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.WriteLine("\nAvailable overloaded Add() methods:");
            Console.WriteLine("  1. Add(int, int) - Adds two integers");
            Console.WriteLine("  2. Add(int, int, int) - Adds three integers");
            Console.WriteLine("  3. Add(double, double) - Adds two doubles");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║     LAB 04 - A01: CALCULATOR OVERLOADING   ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");

            Calculator calc = new Calculator();

            // Display information about overloaded methods
            calc.DisplayInfo();

            Console.WriteLine("\n" + new string('═', 46));
            Console.WriteLine("    DEMONSTRATION OF METHOD OVERLOADING");
            Console.WriteLine(new string('═', 46));

            // Test 1: Add two integers
            int result1 = calc.Add(10, 20);
            Console.WriteLine("\n[Test 1] Add(10, 20)");
            Console.WriteLine("└─→ Method signature: Add(int, int)");
            Console.WriteLine($"└─→ Result: {result1}");

            // Test 2: Add three integers
            int result2 = calc.Add(5, 15, 25);
            Console.WriteLine("\n[Test 2] Add(5, 15, 25)");
            Console.WriteLine("└─→ Method signature: Add(int, int, int)");
            Console.WriteLine($"└─→ Result: {result2}");

            // Test 3: Add two doubles
            double result3 = calc.Add(12.5, 7.3);
            Console.WriteLine("\n[Test 3] Add(12.5, 7.3)");
            Console.WriteLine("└─→ Method signature: Add(double, double)");
            Console.WriteLine($"└─→ Result: {result3:F2}");

            // Additional demonstrations
            Console.WriteLine("\n" + new string('═', 46));
            Console.WriteLine("    ADDITIONAL DEMONSTRATIONS");
            Console.WriteLine(new string('═', 46));

            Console.WriteLine("\n[Demo 1] Adding exam scores (3 subjects)");
            int totalScore = calc.Add(85, 92, 78);
            Console.WriteLine($"└─→ Scores: 85 + 92 + 78 = {totalScore}");
            Console.WriteLine($"└─→ Average: {totalScore / 3.0:F2}");

            Console.WriteLine("\n[Demo 2] Adding product prices");
            double price1 = calc.Add(299.99, 199.50);
            Console.WriteLine($"└─→ Item 1 + Item 2 = ${price1:F2}");

            Console.WriteLine("\n[Demo 3] Simple addition");
            int sum = calc.Add(100, 200);
            Console.WriteLine($"└─→ 100 + 200 = {sum}");

            // Key Learning Points
            Console.WriteLine("\n╔════════════════════════════════════════════╗");
            Console.WriteLine("║          KEY LEARNING POINTS               ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.WriteLine("\n✓ Method Overloading: Same method name, different");
            Console.WriteLine("  parameters (count or type)");
            Console.WriteLine("\n✓ Compiler determines which method to call based");
            Console.WriteLine("  on arguments provided at compile time");
            Console.WriteLine("\n✓ Return types can vary but parameters must differ");
            Console.WriteLine("\n✓ Also known as Compile-Time Polymorphism");

            Console.WriteLine("\n" + new string('═', 46));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
