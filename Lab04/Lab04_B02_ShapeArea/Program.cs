/*
 * Lab04_B02_ShapeArea
 * Demonstrates method overriding with mathematical calculations
 * Shape base class with virtual CalculateArea(), Circle, Rectangle, Triangle override
 */

using System;

namespace Lab04_B02_ShapeArea
{
    // Base class Shape with virtual methods
    abstract class Shape
    {
        protected string name;

        public Shape(string name)
        {
            this.name = name;
        }

        // Virtual method - can be overridden
        public abstract double CalculateArea();

        // Virtual method for perimeter
        public abstract double CalculatePerimeter();

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Shape: {name}");
        }
    }

    // Circle class - overrides CalculateArea()
    class Circle : Shape
    {
        private double radius;

        public Circle(double radius) : base("Circle")
        {
            this.radius = radius;
        }

        public override double CalculateArea()
        {
            return Math.PI * radius * radius;
        }

        public override double CalculatePerimeter()
        {
            return 2 * Math.PI * radius;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("┌────────────────────────────────────────┐");
            Console.WriteLine("│           CIRCLE                       │");
            Console.WriteLine("├────────────────────────────────────────┤");
            Console.WriteLine($"│ Radius:     {radius,10:F2} units       │");
            Console.WriteLine($"│ Area:       {CalculateArea(),10:F2} sq units  │");
            Console.WriteLine($"│ Perimeter:  {CalculatePerimeter(),10:F2} units       │");
            Console.WriteLine("└────────────────────────────────────────┘");
        }
    }

    // Rectangle class - overrides CalculateArea()
    class Rectangle : Shape
    {
        private double length;
        private double width;

        public Rectangle(double length, double width) : base("Rectangle")
        {
            this.length = length;
            this.width = width;
        }

        public override double CalculateArea()
        {
            return length * width;
        }

        public override double CalculatePerimeter()
        {
            return 2 * (length + width);
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("┌────────────────────────────────────────┐");
            Console.WriteLine("│           RECTANGLE                    │");
            Console.WriteLine("├────────────────────────────────────────┤");
            Console.WriteLine($"│ Length:     {length,10:F2} units       │");
            Console.WriteLine($"│ Width:      {width,10:F2} units       │");
            Console.WriteLine($"│ Area:       {CalculateArea(),10:F2} sq units  │");
            Console.WriteLine($"│ Perimeter:  {CalculatePerimeter(),10:F2} units       │");
            Console.WriteLine("└────────────────────────────────────────┘");
        }
    }

    // Triangle class - overrides CalculateArea()
    class Triangle : Shape
    {
        private double baseLength;
        private double height;
        private double side1, side2, side3;

        public Triangle(double baseLength, double height, double side1, double side2, double side3)
            : base("Triangle")
        {
            this.baseLength = baseLength;
            this.height = height;
            this.side1 = side1;
            this.side2 = side2;
            this.side3 = side3;
        }

        public override double CalculateArea()
        {
            return 0.5 * baseLength * height;
        }

        public override double CalculatePerimeter()
        {
            return side1 + side2 + side3;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("┌────────────────────────────────────────┐");
            Console.WriteLine("│           TRIANGLE                     │");
            Console.WriteLine("├────────────────────────────────────────┤");
            Console.WriteLine($"│ Base:       {baseLength,10:F2} units       │");
            Console.WriteLine($"│ Height:     {height,10:F2} units       │");
            Console.WriteLine($"│ Area:       {CalculateArea(),10:F2} sq units  │");
            Console.WriteLine($"│ Perimeter:  {CalculatePerimeter(),10:F2} units       │");
            Console.WriteLine("└────────────────────────────────────────┘");
        }
    }

    // Square class (special case of Rectangle)
    class Square : Shape
    {
        private double side;

        public Square(double side) : base("Square")
        {
            this.side = side;
        }

        public override double CalculateArea()
        {
            return side * side;
        }

        public override double CalculatePerimeter()
        {
            return 4 * side;
        }

        public override void DisplayInfo()
        {
            Console.WriteLine("┌────────────────────────────────────────┐");
            Console.WriteLine("│           SQUARE                       │");
            Console.WriteLine("├────────────────────────────────────────┤");
            Console.WriteLine($"│ Side:       {side,10:F2} units       │");
            Console.WriteLine($"│ Area:       {CalculateArea(),10:F2} sq units  │");
            Console.WriteLine($"│ Perimeter:  {CalculatePerimeter(),10:F2} units       │");
            Console.WriteLine("└────────────────────────────────────────┘");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════╗");
            Console.WriteLine("║   LAB 04 - B02: SHAPE AREA (OVERRIDING)    ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");

            Console.WriteLine("\n" + new string('═', 46));
            Console.WriteLine("   METHOD OVERRIDING WITH CALCULATIONS");
            Console.WriteLine(new string('═', 46));

            // Create different shapes
            Circle circle1 = new Circle(7.5);
            Rectangle rect1 = new Rectangle(10, 5);
            Triangle tri1 = new Triangle(8, 6, 6, 7, 8);
            Square square1 = new Square(5);

            // Test 1: Direct object references
            Console.WriteLine("\n[Test 1] Direct Shape Objects");
            Console.WriteLine(new string('-', 46));

            Console.WriteLine("\nCircle Details:");
            circle1.DisplayInfo();

            Console.WriteLine("\nRectangle Details:");
            rect1.DisplayInfo();

            Console.WriteLine("\nTriangle Details:");
            tri1.DisplayInfo();

            Console.WriteLine("\nSquare Details:");
            square1.DisplayInfo();

            // Test 2: Polymorphism - Base class reference
            Console.WriteLine("\n" + new string('═', 46));
            Console.WriteLine("[Test 2] Polymorphic Behavior");
            Console.WriteLine(new string('-', 46));

            Shape shape1 = new Circle(10);
            Shape shape2 = new Rectangle(15, 8);
            Shape shape3 = new Triangle(12, 9, 10, 12, 15);
            Shape shape4 = new Square(7);

            Console.WriteLine("\nShape 1 (Circle):");
            shape1.DisplayInfo();

            Console.WriteLine("\nShape 2 (Rectangle):");
            shape2.DisplayInfo();

            Console.WriteLine("\nShape 3 (Triangle):");
            shape3.DisplayInfo();

            Console.WriteLine("\nShape 4 (Square):");
            shape4.DisplayInfo();

            // Test 3: Array of shapes
            Console.WriteLine("\n" + new string('═', 46));
            Console.WriteLine("[Test 3] Shape Collection");
            Console.WriteLine(new string('-', 46));

            Shape[] shapes = new Shape[4];
            shapes[0] = new Circle(5);
            shapes[1] = new Rectangle(8, 4);
            shapes[2] = new Triangle(6, 5, 5, 6, 7);
            shapes[3] = new Square(6);

            double totalArea = 0;
            double totalPerimeter = 0;

            Console.WriteLine("\nCalculating areas and perimeters:");
            for (int i = 0; i < shapes.Length; i++)
            {
                double area = shapes[i].CalculateArea();
                double perimeter = shapes[i].CalculatePerimeter();
                totalArea += area;
                totalPerimeter += perimeter;

                Console.WriteLine($"\n{i + 1}. ");
                shapes[i].DisplayInfo();
            }

            Console.WriteLine("\n╔════════════════════════════════════════════╗");
            Console.WriteLine("║         SUMMARY STATISTICS                 ║");
            Console.WriteLine("╠════════════════════════════════════════════╣");
            Console.WriteLine($"║ Total Area:       {totalArea,20:F2} sq units ║");
            Console.WriteLine($"║ Total Perimeter:  {totalPerimeter,20:F2} units    ║");
            Console.WriteLine($"║ Number of Shapes: {shapes.Length,20}           ║");
            Console.WriteLine($"║ Average Area:     {totalArea / shapes.Length,20:F2} sq units ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");

            // Test 4: Practical application
            Console.WriteLine("\n" + new string('═', 46));
            Console.WriteLine("[Test 4] Practical Application");
            Console.WriteLine(new string('-', 46));

            Console.WriteLine("\nScenario: Calculating paint required for rooms");
            Console.WriteLine("Paint coverage: 10 sq units per liter\n");

            Shape[] rooms = new Shape[3];
            rooms[0] = new Rectangle(20, 15); // Living Room
            rooms[1] = new Rectangle(12, 10); // Bedroom
            rooms[2] = new Square(10);        // Study Room

            string[] roomNames = { "Living Room", "Bedroom", "Study Room" };

            for (int i = 0; i < rooms.Length; i++)
            {
                double area = rooms[i].CalculateArea();
                double paintNeeded = area / 10;

                Console.WriteLine($"{roomNames[i]}:");
                Console.WriteLine($"  Area: {area:F2} sq units");
                Console.WriteLine($"  Paint needed: {paintNeeded:F2} liters");
            }

            // Key Learning Points
            Console.WriteLine("\n╔════════════════════════════════════════════╗");
            Console.WriteLine("║         KEY LEARNING POINTS                ║");
            Console.WriteLine("╚════════════════════════════════════════════╝");
            Console.WriteLine("\n✓ Abstract classes cannot be instantiated");
            Console.WriteLine("\n✓ Abstract methods must be overridden in");
            Console.WriteLine("  derived classes");
            Console.WriteLine("\n✓ Each shape implements its own calculation");
            Console.WriteLine("  formula (Circle: πr², Rectangle: l×w, etc.)");
            Console.WriteLine("\n✓ Polymorphism allows treating different shapes");
            Console.WriteLine("  uniformly through base class reference");
            Console.WriteLine("\n✓ Useful for writing generic code that works");
            Console.WriteLine("  with multiple types of objects");

            Console.WriteLine("\n" + new string('═', 46));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
