// Lab 1 - B Task 1
// Temperature converter: Celsius to Fahrenheit and vice versa

Console.WriteLine("========================================");
Console.WriteLine("    TEMPERATURE CONVERTER");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine("Select conversion type:");
Console.WriteLine("1. Celsius to Fahrenheit");
Console.WriteLine("2. Fahrenheit to Celsius");
Console.Write("Enter your choice (1 or 2): ");
int choice = Convert.ToInt32(Console.ReadLine());

Console.WriteLine();

if (choice == 1)
{
    Console.Write("Enter temperature in Celsius: ");
    double celsius = Convert.ToDouble(Console.ReadLine());

    double fahrenheit = (celsius * 9 / 5) + 32;

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine($"{celsius}°C = {fahrenheit:F2}°F");
    Console.WriteLine("========================================");
}
else if (choice == 2)
{
    Console.Write("Enter temperature in Fahrenheit: ");
    double fahrenheit = Convert.ToDouble(Console.ReadLine());

    double celsius = (fahrenheit - 32) * 5 / 9;

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine($"{fahrenheit}°F = {celsius:F2}°C");
    Console.WriteLine("========================================");
}
else
{
    Console.WriteLine("Invalid choice! Please enter 1 or 2.");
}
