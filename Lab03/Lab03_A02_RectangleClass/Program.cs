/*
 * Lab03_A02_RectangleClass
 * Problem: Create a Rectangle class with a parameterized constructor.
 *          Calculate and display the area of the rectangle.
 *
 * Learning Objectives:
 * - Understanding parameterized constructors
 * - Encapsulation with private fields
 * - Method implementation for calculations
 */

using System;

namespace Lab03_A02_RectangleClass
{
    // Rectangle class with parameterized constructor
    class Rectangle
    {
        // Private fields
        private double length;
        private double width;

        // Parameterized constructor
        public Rectangle(double length, double width)
        {
            this.length = length;
            this.width = width;
        }

        // Method to calculate area
        public double CalculateArea()
        {
            return length * width;
        }

        // Method to display rectangle details
        public void DisplayDetails()
        {
            Console.WriteLine($"Length    : {length} units");
            Console.WriteLine($"Width     : {width} units");
            Console.WriteLine($"Area      : {CalculateArea()} square units");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader("Rectangle Area Calculator");

            try
            {
                // Create rectangle objects using parameterized constructor
                Console.WriteLine("\n*** Rectangle 1 ***");
                Console.WriteLine(new string('-', 40));
                Rectangle rect1 = new Rectangle(10.5, 5.0);
                rect1.DisplayDetails();

                Console.WriteLine("\n*** Rectangle 2 ***");
                Console.WriteLine(new string('-', 40));
                Rectangle rect2 = new Rectangle(15.0, 8.5);
                rect2.DisplayDetails();

                Console.WriteLine("\n*** Rectangle 3 ***");
                Console.WriteLine(new string('-', 40));
                Rectangle rect3 = new Rectangle(20.0, 12.0);
                rect3.DisplayDetails();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError: {ex.Message}");
            }

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
