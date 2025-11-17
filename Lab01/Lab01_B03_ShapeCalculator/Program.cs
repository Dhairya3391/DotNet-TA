// Lab 1 - B Task 3
// Shape calculator: Calculate area and perimeter of rectangle, circle, and triangle

Console.WriteLine("========================================");
Console.WriteLine("      SHAPE CALCULATOR");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine("Select a shape:");
Console.WriteLine("1. Rectangle");
Console.WriteLine("2. Circle");
Console.WriteLine("3. Triangle");
Console.Write("Enter your choice (1-3): ");
int choice = Convert.ToInt32(Console.ReadLine());

Console.WriteLine();

if (choice == 1)
{
    // Rectangle
    Console.Write("Enter length: ");
    double length = Convert.ToDouble(Console.ReadLine());
    Console.Write("Enter width: ");
    double width = Convert.ToDouble(Console.ReadLine());

    double area = length * width;
    double perimeter = 2 * (length + width);

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("         RECTANGLE");
    Console.WriteLine("========================================");
    Console.WriteLine($"Area      : {area:F2} sq units");
    Console.WriteLine($"Perimeter : {perimeter:F2} units");
    Console.WriteLine("========================================");
}
else if (choice == 2)
{
    // Circle
    Console.Write("Enter radius: ");
    double radius = Convert.ToDouble(Console.ReadLine());

    double area = Math.PI * radius * radius;
    double perimeter = 2 * Math.PI * radius;

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("          CIRCLE");
    Console.WriteLine("========================================");
    Console.WriteLine($"Area      : {area:F2} sq units");
    Console.WriteLine($"Perimeter : {perimeter:F2} units");
    Console.WriteLine("========================================");
}
else if (choice == 3)
{
    // Triangle
    Console.Write("Enter side a: ");
    double a = Convert.ToDouble(Console.ReadLine());
    Console.Write("Enter side b: ");
    double b = Convert.ToDouble(Console.ReadLine());
    Console.Write("Enter side c: ");
    double c = Convert.ToDouble(Console.ReadLine());

    double perimeter = a + b + c;
    double s = perimeter / 2; // semi-perimeter
    double area = Math.Sqrt(s * (s - a) * (s - b) * (s - c)); // Heron's formula

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("         TRIANGLE");
    Console.WriteLine("========================================");
    Console.WriteLine($"Area      : {area:F2} sq units");
    Console.WriteLine($"Perimeter : {perimeter:F2} units");
    Console.WriteLine("========================================");
}
else
{
    Console.WriteLine("Invalid choice! Please enter 1, 2, or 3.");
}
