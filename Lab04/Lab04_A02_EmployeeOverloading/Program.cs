/*
 * Lab04_A02_EmployeeOverloading
 * Demonstrates method overloading with different parameter combinations
 * Employee class with overloaded DisplayInfo() methods
 */

using System;

namespace Lab04_A02_EmployeeOverloading
{
    // Employee class demonstrating method overloading
    class Employee
    {
        // Private fields
        private string name;
        private int age;
        private double salary;

        // Constructor
        public Employee(string name, int age, double salary)
        {
            this.name = name;
            this.age = age;
            this.salary = salary;
        }

        // Overloaded method 1: Display only name
        public void DisplayInfo(string name)
        {
            Console.WriteLine("┌─────────────────────────────────────────┐");
            Console.WriteLine("│   EMPLOYEE INFO (NAME ONLY)             │");
            Console.WriteLine("├─────────────────────────────────────────┤");
            Console.WriteLine($"│ Name: {name,-33} │");
            Console.WriteLine("└─────────────────────────────────────────┘");
        }

        // Overloaded method 2: Display name and age
        public void DisplayInfo(string name, int age)
        {
            Console.WriteLine("┌─────────────────────────────────────────┐");
            Console.WriteLine("│   EMPLOYEE INFO (NAME & AGE)            │");
            Console.WriteLine("├─────────────────────────────────────────┤");
            Console.WriteLine($"│ Name: {name,-33} │");
            Console.WriteLine($"│ Age:  {age,-33} │");
            Console.WriteLine("└─────────────────────────────────────────┘");
        }

        // Overloaded method 3: Display name, age, and salary
        public void DisplayInfo(string name, int age, double salary)
        {
            Console.WriteLine("┌─────────────────────────────────────────┐");
            Console.WriteLine("│   EMPLOYEE INFO (COMPLETE)              │");
            Console.WriteLine("├─────────────────────────────────────────┤");
            Console.WriteLine($"│ Name:   {name,-31} │");
            Console.WriteLine($"│ Age:    {age,-31} │");
            Console.WriteLine($"│ Salary: ${salary,-30:F2} │");
            Console.WriteLine("└─────────────────────────────────────────┘");
        }

        // Getters for demonstration
        public string GetName() { return name; }
        public int GetAge() { return age; }
        public double GetSalary() { return salary; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("╔═════════════════════════════════════════════╗");
            Console.WriteLine("║   LAB 04 - A02: EMPLOYEE OVERLOADING        ║");
            Console.WriteLine("╚═════════════════════════════════════════════╝");

            // Create sample employees
            Employee emp1 = new Employee("Rajesh Kumar", 28, 45000.00);
            Employee emp2 = new Employee("Priya Sharma", 32, 65000.00);
            Employee emp3 = new Employee("Amit Patel", 25, 38000.00);

            Console.WriteLine("\n" + new string('═', 47));
            Console.WriteLine("   DEMONSTRATION OF METHOD OVERLOADING");
            Console.WriteLine(new string('═', 47));

            // Test 1: Display name only
            Console.WriteLine("\n[Test 1] DisplayInfo(string name)");
            Console.WriteLine("Method Signature: DisplayInfo(string)");
            emp1.DisplayInfo(emp1.GetName());

            // Test 2: Display name and age
            Console.WriteLine("\n[Test 2] DisplayInfo(string name, int age)");
            Console.WriteLine("Method Signature: DisplayInfo(string, int)");
            emp2.DisplayInfo(emp2.GetName(), emp2.GetAge());

            // Test 3: Display complete information
            Console.WriteLine("\n[Test 3] DisplayInfo(string name, int age, double salary)");
            Console.WriteLine("Method Signature: DisplayInfo(string, int, double)");
            emp3.DisplayInfo(emp3.GetName(), emp3.GetAge(), emp3.GetSalary());

            // Additional demonstrations
            Console.WriteLine("\n" + new string('═', 47));
            Console.WriteLine("   PRACTICAL USE CASES");
            Console.WriteLine(new string('═', 47));

            Console.WriteLine("\n[Use Case 1] Quick name lookup");
            emp1.DisplayInfo("John Doe");

            Console.WriteLine("\n[Use Case 2] Age verification");
            emp2.DisplayInfo("Sarah Smith", 29);

            Console.WriteLine("\n[Use Case 3] Complete employee record");
            emp1.DisplayInfo(emp1.GetName(), emp1.GetAge(), emp1.GetSalary());

            // Demonstrating multiple employees
            Console.WriteLine("\n" + new string('═', 47));
            Console.WriteLine("   EMPLOYEE DIRECTORY (ALL DETAILS)");
            Console.WriteLine(new string('═', 47));

            Console.WriteLine("\nEmployee #1:");
            emp1.DisplayInfo(emp1.GetName(), emp1.GetAge(), emp1.GetSalary());

            Console.WriteLine("\nEmployee #2:");
            emp2.DisplayInfo(emp2.GetName(), emp2.GetAge(), emp2.GetSalary());

            Console.WriteLine("\nEmployee #3:");
            emp3.DisplayInfo(emp3.GetName(), emp3.GetAge(), emp3.GetSalary());

            // Key Learning Points
            Console.WriteLine("\n╔═════════════════════════════════════════════╗");
            Console.WriteLine("║         KEY LEARNING POINTS                 ║");
            Console.WriteLine("╚═════════════════════════════════════════════╝");
            Console.WriteLine("\n✓ Method overloading allows same method name");
            Console.WriteLine("  with different parameter lists");
            Console.WriteLine("\n✓ Useful for providing flexibility in method calls");
            Console.WriteLine("  (show only needed information)");
            Console.WriteLine("\n✓ Parameters differ by: number, type, or order");
            Console.WriteLine("\n✓ Compiler decides which method to call based on");
            Console.WriteLine("  arguments provided");

            Console.WriteLine("\n" + new string('═', 47));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
