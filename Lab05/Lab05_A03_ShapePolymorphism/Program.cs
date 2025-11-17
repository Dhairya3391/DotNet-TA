using System;

namespace Lab05_A03_ShapePolymorphism
{
    // Base class - Shape
    // Demonstrates polymorphism with virtual method
    class Shape
    {
        public string Name { get; set; }

        public Shape(string name)
        {
            Name = name;
        }

        // Virtual method - can be overridden in derived classes
        public virtual double Area()
        {
            return 0;
        }

        // Display shape information
        public void DisplayInfo()
        {
            Console.WriteLine($"Shape: {Name}");
            Console.WriteLine($"Area: {Area():F2} square units");
        }
    }

    // Derived class - Circle
    class Circle : Shape
    {
        public double Radius { get; set; }

        public Circle(double radius) : base("Circle")
        {
            Radius = radius;
        }

        // Override Area method for Circle
        public override double Area()
        {
            return Math.PI * Radius * Radius;
        }

        public void DisplayCircleDetails()
        {
            Console.WriteLine($"Radius: {Radius} units");
        }
    }

    // Derived class - Rectangle
    class Rectangle : Shape
    {
        public double Length { get; set; }
        public double Width { get; set; }

        public Rectangle(double length, double width) : base("Rectangle")
        {
            Length = length;
            Width = width;
        }

        // Override Area method for Rectangle
        public override double Area()
        {
            return Length * Width;
        }

        public void DisplayRectangleDetails()
        {
            Console.WriteLine($"Length: {Length} units, Width: {Width} units");
        }
    }

    // Additional derived class - Triangle
    class Triangle : Shape
    {
        public double Base { get; set; }
        public double Height { get; set; }

        public Triangle(double baseLength, double height) : base("Triangle")
        {
            Base = baseLength;
            Height = height;
        }

        // Override Area method for Triangle
        public override double Area()
        {
            return 0.5 * Base * Height;
        }

        public void DisplayTriangleDetails()
        {
            Console.WriteLine($"Base: {Base} units, Height: {Height} units");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Display header
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         Lab 05 - A03: Shape Polymorphism Demo              ║");
            Console.WriteLine("║     (Virtual Method Override & Runtime Polymorphism)       ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                // Create array of Shape references (polymorphism)
                // Each element can hold any derived class object
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Creating Different Shapes:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                // Create Circle objects
                Circle circle1 = new Circle(5.0);
                Circle circle2 = new Circle(7.5);

                // Create Rectangle objects
                Rectangle rect1 = new Rectangle(10.0, 5.0);
                Rectangle rect2 = new Rectangle(8.0, 6.0);

                // Create Triangle objects
                Triangle tri1 = new Triangle(6.0, 4.0);
                Triangle tri2 = new Triangle(10.0, 8.0);

                // Polymorphism demonstration - storing different shapes in Shape array
                Shape[] shapes = new Shape[] { circle1, rect1, tri1, circle2, rect2, tri2 };

                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Demonstrating Polymorphism (Calling Area() on each shape):");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                int shapeNumber = 1;
                foreach (Shape shape in shapes)
                {
                    Console.WriteLine($"[Shape {shapeNumber}]");
                    Console.WriteLine("┌─────────────────────────────────────────────────────────┐");

                    // Display specific details based on shape type
                    if (shape is Circle c)
                    {
                        c.DisplayCircleDetails();
                    }
                    else if (shape is Rectangle r)
                    {
                        r.DisplayRectangleDetails();
                    }
                    else if (shape is Triangle t)
                    {
                        t.DisplayTriangleDetails();
                    }

                    // Polymorphic call - calls the overridden method
                    shape.DisplayInfo();
                    Console.WriteLine("└─────────────────────────────────────────────────────────┘");
                    Console.WriteLine();
                    shapeNumber++;
                }

                // Calculate total area of all shapes
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Summary:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                double totalArea = 0;
                foreach (Shape shape in shapes)
                {
                    totalArea += shape.Area();
                }

                Console.WriteLine($"Total number of shapes: {shapes.Length}");
                Console.WriteLine($"Total area of all shapes: {totalArea:F2} square units");
                Console.WriteLine();

                // Explain polymorphism
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Polymorphism Concept Demonstration:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("✓ Base class Shape has virtual Area() method");
                Console.WriteLine("✓ Circle, Rectangle, Triangle override Area() method");
                Console.WriteLine("✓ Same method name, different implementations");
                Console.WriteLine("✓ Runtime polymorphism - correct method called at runtime");
                Console.WriteLine("✓ Shape reference can hold any derived class object");
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[ERROR] {ex.Message}");
            }

            // Footer
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              Program Completed Successfully                ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        }
    }
}
