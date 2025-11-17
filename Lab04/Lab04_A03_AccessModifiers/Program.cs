/*
 * Lab04_A03_AccessModifiers
 * Demonstrates public, private, protected, and internal access modifiers
 * Person class with different access levels
 */

using System;

namespace Lab04_A03_AccessModifiers
{
    // Person class demonstrating all access modifiers
    class Person
    {
        // PUBLIC: Accessible from anywhere
        public string Name;

        // PRIVATE: Accessible only within this class
        private int age;

        // PROTECTED: Accessible within this class and derived classes
        protected string address;

        // INTERNAL: Accessible within the same assembly
        internal string phoneNumber;

        // PUBLIC property to access private field
        public int Age
        {
            get { return age; }
            set
            {
                if (value > 0 && value < 150)
                    age = value;
                else
                    Console.WriteLine("Invalid age!");
            }
        }

        // Constructor
        public Person(string name, int age, string address, string phone)
        {
            this.Name = name;
            this.age = age;
            this.address = address;
            this.phoneNumber = phone;
        }

        // PUBLIC method - accessible from anywhere
        public void DisplayPublicInfo()
        {
            Console.WriteLine($"Public Access: Name = {Name}");
        }

        // PRIVATE method - only accessible within this class
        private void DisplayPrivateInfo()
        {
            Console.WriteLine($"Private Access: Age = {age}");
        }

        // PROTECTED method - accessible in this class and derived classes
        protected void DisplayProtectedInfo()
        {
            Console.WriteLine($"Protected Access: Address = {address}");
        }

        // INTERNAL method - accessible within same assembly
        internal void DisplayInternalInfo()
        {
            Console.WriteLine($"Internal Access: Phone = {phoneNumber}");
        }

        // PUBLIC method to demonstrate access to all members
        public void DisplayAllInfo()
        {
            Console.WriteLine("\n┌──────────────────────────────────────────────┐");
            Console.WriteLine("│         PERSON INFORMATION                   │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine($"│ Name:    {Name,-34} │");
            Console.WriteLine($"│ Age:     {age,-34} │");
            Console.WriteLine($"│ Address: {address,-34} │");
            Console.WriteLine($"│ Phone:   {phoneNumber,-34} │");
            Console.WriteLine("└──────────────────────────────────────────────┘");
        }

        // Method to demonstrate private method call
        public void CallPrivateMethod()
        {
            Console.WriteLine("\nCalling private method from within class:");
            DisplayPrivateInfo();
        }
    }

    // Derived class to demonstrate protected access
    class Employee : Person
    {
        private string employeeId;

        public Employee(string name, int age, string address, string phone, string empId)
            : base(name, age, address, phone)
        {
            this.employeeId = empId;
        }

        // This method can access protected members from base class
        public void DisplayEmployeeInfo()
        {
            Console.WriteLine("\n┌──────────────────────────────────────────────┐");
            Console.WriteLine("│         EMPLOYEE INFORMATION                 │");
            Console.WriteLine("├──────────────────────────────────────────────┤");
            Console.WriteLine($"│ Employee ID: {employeeId,-30} │");
            Console.WriteLine($"│ Name:        {Name,-30} │"); // Public member
            Console.WriteLine($"│ Address:     {address,-30} │"); // Protected member - accessible!
            Console.WriteLine("└──────────────────────────────────────────────┘");
        }

        // Calling protected method from base class
        public void ShowProtectedAccess()
        {
            Console.WriteLine("\nAccessing protected method from derived class:");
            DisplayProtectedInfo(); // This works because it's protected
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════╗");
            Console.WriteLine("║   LAB 04 - A03: ACCESS MODIFIERS             ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");

            Person person1 = new Person("Rahul Verma", 30, "Mumbai, India", "+91-9876543210");

            Console.WriteLine("\n" + new string('═', 48));
            Console.WriteLine("   ACCESS MODIFIER DEMONSTRATIONS");
            Console.WriteLine(new string('═', 48));

            // 1. PUBLIC ACCESS
            Console.WriteLine("\n[1] PUBLIC ACCESS MODIFIER");
            Console.WriteLine("    ✓ Accessible from anywhere");
            person1.DisplayPublicInfo();
            Console.WriteLine($"    Direct access to Name: {person1.Name}");

            // 2. PRIVATE ACCESS
            Console.WriteLine("\n[2] PRIVATE ACCESS MODIFIER");
            Console.WriteLine("    ✓ Only accessible within the class");
            Console.WriteLine("    ✗ Cannot call DisplayPrivateInfo() directly");
            Console.WriteLine("    ✓ Can access through public method:");
            person1.CallPrivateMethod();

            // 3. PROTECTED ACCESS
            Console.WriteLine("\n[3] PROTECTED ACCESS MODIFIER");
            Console.WriteLine("    ✓ Accessible in class and derived classes");
            Console.WriteLine("    ✗ Cannot access 'address' field directly");
            Console.WriteLine("    ✓ Can access from derived class:");

            Employee emp1 = new Employee("Priya Singh", 28, "Delhi, India", "+91-9988776655", "EMP001");
            emp1.ShowProtectedAccess();

            // 4. INTERNAL ACCESS
            Console.WriteLine("\n[4] INTERNAL ACCESS MODIFIER");
            Console.WriteLine("    ✓ Accessible within same assembly");
            person1.DisplayInternalInfo();
            Console.WriteLine($"    Direct access to phoneNumber: {person1.phoneNumber}");

            // Display complete information
            Console.WriteLine("\n" + new string('═', 48));
            Console.WriteLine("   COMPLETE INFORMATION");
            Console.WriteLine(new string('═', 48));

            person1.DisplayAllInfo();

            // Employee demonstration
            Console.WriteLine("\n" + new string('═', 48));
            Console.WriteLine("   DERIVED CLASS (EMPLOYEE) DEMONSTRATION");
            Console.WriteLine(new string('═', 48));

            emp1.DisplayEmployeeInfo();

            // Access modifier comparison table
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║      ACCESS MODIFIER COMPARISON              ║");
            Console.WriteLine("╠══════════════════════════════════════════════╣");
            Console.WriteLine("║ Modifier   │ Same Class │ Derived │ Assembly ║");
            Console.WriteLine("╠════════════╪════════════╪═════════╪══════════╣");
            Console.WriteLine("║ public     │     ✓      │    ✓    │    ✓     ║");
            Console.WriteLine("║ private    │     ✓      │    ✗    │    ✗     ║");
            Console.WriteLine("║ protected  │     ✓      │    ✓    │    ✗     ║");
            Console.WriteLine("║ internal   │     ✓      │    ✓    │    ✓     ║");
            Console.WriteLine("╚════════════╧════════════╧═════════╧══════════╝");

            // Key Learning Points
            Console.WriteLine("\n╔══════════════════════════════════════════════╗");
            Console.WriteLine("║         KEY LEARNING POINTS                  ║");
            Console.WriteLine("╚══════════════════════════════════════════════╝");
            Console.WriteLine("\n✓ PUBLIC: No restrictions, accessible everywhere");
            Console.WriteLine("\n✓ PRIVATE: Most restrictive, only within class");
            Console.WriteLine("  (Used for encapsulation and data hiding)");
            Console.WriteLine("\n✓ PROTECTED: Accessible in class and subclasses");
            Console.WriteLine("  (Used in inheritance hierarchies)");
            Console.WriteLine("\n✓ INTERNAL: Accessible within same assembly/project");
            Console.WriteLine("  (Used for internal implementation details)");
            Console.WriteLine("\n✓ Access modifiers enforce encapsulation and");
            Console.WriteLine("  control visibility of class members");

            Console.WriteLine("\n" + new string('═', 48));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
