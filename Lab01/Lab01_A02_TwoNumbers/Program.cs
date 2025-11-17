// Lab 1 - A Task 2
// Write a program to get two numbers from user and print those two numbers.

Console.WriteLine("========================================");
Console.WriteLine("     TWO NUMBERS INPUT");
Console.WriteLine("========================================");
Console.WriteLine();

Console.Write("Enter first number: ");
string? input1 = Console.ReadLine();
int num1 = Convert.ToInt32(input1);

Console.Write("Enter second number: ");
string? input2 = Console.ReadLine();
int num2 = Convert.ToInt32(input2);

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("       ENTERED NUMBERS");
Console.WriteLine("========================================");
Console.WriteLine($"First Number  : {num1}");
Console.WriteLine($"Second Number : {num2}");
Console.WriteLine("========================================");
