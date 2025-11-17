// Lab 1 - B Task 4
// Grade calculator: Calculate total, percentage and grade from 5 subject marks
// A >= 75, B >= 60, C >= 45, else Fail

Console.WriteLine("========================================");
Console.WriteLine("      GRADE CALCULATOR");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine("Enter marks for 5 subjects (out of 100):");
Console.Write("Subject 1: ");
double marks1 = Convert.ToDouble(Console.ReadLine());

Console.Write("Subject 2: ");
double marks2 = Convert.ToDouble(Console.ReadLine());

Console.Write("Subject 3: ");
double marks3 = Convert.ToDouble(Console.ReadLine());

Console.Write("Subject 4: ");
double marks4 = Convert.ToDouble(Console.ReadLine());

Console.Write("Subject 5: ");
double marks5 = Convert.ToDouble(Console.ReadLine());

// Calculate total and percentage
double total = marks1 + marks2 + marks3 + marks4 + marks5;
double percentage = (total / 500) * 100;

// Determine grade
string grade;
if (percentage >= 75)
{
    grade = "A";
}
else if (percentage >= 60)
{
    grade = "B";
}
else if (percentage >= 45)
{
    grade = "C";
}
else
{
    grade = "Fail";
}

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("         RESULT");
Console.WriteLine("========================================");
Console.WriteLine($"Total Marks : {total} / 500");
Console.WriteLine($"Percentage  : {percentage:F2}%");
Console.WriteLine($"Grade       : {grade}");
Console.WriteLine("========================================");
