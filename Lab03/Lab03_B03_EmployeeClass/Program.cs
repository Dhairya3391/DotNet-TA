/*
 * Lab03_B03_EmployeeClass
 * Problem: Create an Employee class with empID, empName, and salary
 *          using a parameterized constructor. Display employee details.
 *
 * Learning Objectives:
 * - Understanding parameterized constructors
 * - Property encapsulation
 * - Formatting output for professional display
 */

using System;

namespace Lab03_B03_EmployeeClass
{
    // Employee class with parameterized constructor
    class Employee
    {
        // Properties
        public int EmpID { get; set; }
        public string EmpName { get; set; }
        public double Salary { get; set; }

        // Parameterized constructor
        public Employee(int empID, string empName, double salary)
        {
            EmpID = empID;
            EmpName = empName;
            Salary = salary;
        }

        // Method to display employee details
        public void DisplayDetails()
        {
            Console.WriteLine($"Employee ID   : {EmpID}");
            Console.WriteLine($"Employee Name : {EmpName}");
            Console.WriteLine($"Salary        : Rs. {Salary:N2}");
        }

        // Method to calculate annual salary
        public double GetAnnualSalary()
        {
            return Salary * 12;
        }

        // Method to give raise
        public void GiveRaise(double percentage)
        {
            if (percentage < 0)
            {
                Console.WriteLine("Raise percentage cannot be negative!");
                return;
            }
            double raiseAmount = Salary * (percentage / 100);
            Salary += raiseAmount;
            Console.WriteLine($"Raise of {percentage}% applied. Raise amount: Rs. {raiseAmount:N2}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader("Employee Management System");

            try
            {
                // Create employee objects using parameterized constructor
                Console.WriteLine("\n*** Creating Employees ***\n");

                Employee emp1 = new Employee(1001, "Rajesh Kumar", 45000.00);
                Employee emp2 = new Employee(1002, "Anjali Sharma", 55000.00);
                Employee emp3 = new Employee(1003, "Vikram Patel", 38000.00);

                // Display employee 1 details
                Console.WriteLine("*** Employee 1 Details ***");
                Console.WriteLine(new string('-', 40));
                emp1.DisplayDetails();
                Console.WriteLine($"Annual Salary : Rs. {emp1.GetAnnualSalary():N2}");

                // Display employee 2 details
                Console.WriteLine("\n*** Employee 2 Details ***");
                Console.WriteLine(new string('-', 40));
                emp2.DisplayDetails();
                Console.WriteLine($"Annual Salary : Rs. {emp2.GetAnnualSalary():N2}");

                // Display employee 3 details
                Console.WriteLine("\n*** Employee 3 Details ***");
                Console.WriteLine(new string('-', 40));
                emp3.DisplayDetails();
                Console.WriteLine($"Annual Salary : Rs. {emp3.GetAnnualSalary():N2}");

                // Demonstrate salary raise
                Console.WriteLine("\n*** Applying Salary Raise ***");
                Console.WriteLine(new string('-', 40));
                Console.WriteLine($"\nGiving 10% raise to {emp1.EmpName}...");
                Console.WriteLine($"Current Salary: Rs. {emp1.Salary:N2}");
                emp1.GiveRaise(10);
                Console.WriteLine($"New Salary    : Rs. {emp1.Salary:N2}");
                Console.WriteLine($"New Annual    : Rs. {emp1.GetAnnualSalary():N2}");

                // Display summary
                Console.WriteLine("\n*** Employee Summary ***");
                Console.WriteLine(new string('-', 40));
                Console.WriteLine($"Total Employees: 3");
                double totalSalary = emp1.Salary + emp2.Salary + emp3.Salary;
                Console.WriteLine($"Total Monthly Salary: Rs. {totalSalary:N2}");
                Console.WriteLine($"Average Salary      : Rs. {(totalSalary / 3):N2}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n*** ERROR ***");
                Console.WriteLine($"Error: {ex.Message}");
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
