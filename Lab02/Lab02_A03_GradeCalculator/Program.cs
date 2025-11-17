/*
 * Lab02_A03_GradeCalculator
 * Description: Calculate grade (A/B/C/D/Fail) based on marks using conditions
 * Difficulty: A (Easy)
 */

using System;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("========================================");
        Console.WriteLine("         GRADE CALCULATOR");
        Console.WriteLine("========================================");
        Console.WriteLine();

        try
        {
            // Input marks
            Console.Write("Enter marks (0-100): ");
            int marks = Convert.ToInt32(Console.ReadLine());

            // Validate marks
            if (marks < 0 || marks > 100)
            {
                Console.WriteLine("Error: Marks must be between 0 and 100.");
                return;
            }

            // Determine grade using if-else conditions
            string grade;
            string remarks;

            if (marks >= 90)
            {
                grade = "A";
                remarks = "Outstanding";
            }
            else if (marks >= 80)
            {
                grade = "B";
                remarks = "Very Good";
            }
            else if (marks >= 70)
            {
                grade = "C";
                remarks = "Good";
            }
            else if (marks >= 60)
            {
                grade = "D";
                remarks = "Satisfactory";
            }
            else if (marks >= 35)
            {
                grade = "E";
                remarks = "Pass";
            }
            else
            {
                grade = "F";
                remarks = "Fail";
            }

            // Display results
            Console.WriteLine();
            Console.WriteLine("========================================");
            Console.WriteLine("           RESULT");
            Console.WriteLine("========================================");
            Console.WriteLine($"Marks:   {marks}");
            Console.WriteLine($"Grade:   {grade}");
            Console.WriteLine($"Remarks: {remarks}");
            Console.WriteLine("========================================");
        }
        catch (FormatException)
        {
            Console.WriteLine("Error: Please enter a valid integer value.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
        }
    }
}
