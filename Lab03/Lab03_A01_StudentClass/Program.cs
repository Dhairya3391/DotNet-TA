/*
 * Lab03_A01_StudentClass
 * Problem: Create a Student class with Name, RollNo, and Marks properties.
 *          Create 2 student objects and display their details.
 *
 * Learning Objectives:
 * - Understanding class definition and properties
 * - Creating objects and accessing properties
 * - Basic display methods
 */

using System;

namespace Lab03_A01_StudentClass
{
    // Student class with properties and display method
    class Student
    {
        // Properties
        public string Name { get; set; }
        public int RollNo { get; set; }
        public double Marks { get; set; }

        // Method to display student details
        public void DisplayDetails()
        {
            Console.WriteLine($"Roll No   : {RollNo}");
            Console.WriteLine($"Name      : {Name}");
            Console.WriteLine($"Marks     : {Marks}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader("Student Management System");

            // Create first student object
            Student student1 = new Student();
            student1.Name = "Raj Patel";
            student1.RollNo = 101;
            student1.Marks = 85.5;

            // Create second student object
            Student student2 = new Student();
            student2.Name = "Priya Shah";
            student2.RollNo = 102;
            student2.Marks = 92.0;

            // Display details of both students
            Console.WriteLine("\n*** Student 1 Details ***");
            Console.WriteLine(new string('-', 40));
            student1.DisplayDetails();

            Console.WriteLine("\n*** Student 2 Details ***");
            Console.WriteLine(new string('-', 40));
            student2.DisplayDetails();

            PrintFooter();
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
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
