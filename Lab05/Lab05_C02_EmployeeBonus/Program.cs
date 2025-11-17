using System;
using System.Collections.Generic;

namespace Lab05_C02_EmployeeBonus
{
    // Abstract base class - Employee
    // Demonstrates abstraction with polymorphic bonus calculation
    abstract class Employee
    {
        public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }
        public double BaseSalary { get; set; }
        public DateTime JoiningDate { get; set; }

        protected Employee(int empId, string name, string department, double baseSalary, DateTime joiningDate)
        {
            EmployeeId = empId;
            Name = name;
            Department = department;
            BaseSalary = baseSalary;
            JoiningDate = joiningDate;
        }

        // Abstract method - must be implemented by derived classes
        public abstract double CalculateBonus();

        // Abstract method for performance rating
        public abstract string GetPerformanceRating();

        // Concrete method
        public void DisplayEmployeeInfo()
        {
            Console.WriteLine($"Employee ID   : {EmployeeId}");
            Console.WriteLine($"Name          : {Name}");
            Console.WriteLine($"Department    : {Department}");
            Console.WriteLine($"Base Salary   : Rs. {BaseSalary:N2}");
            Console.WriteLine($"Joining Date  : {JoiningDate:dd-MMM-yyyy}");
            Console.WriteLine($"Rating        : {GetPerformanceRating()}");
        }

        public double GetTotalCompensation()
        {
            return BaseSalary + CalculateBonus();
        }

        public int GetExperienceYears()
        {
            return (DateTime.Now - JoiningDate).Days / 365;
        }
    }

    // Derived class - Manager (20% bonus)
    class Manager : Employee
    {
        public int TeamSize { get; set; }
        public string ProjectsHandled { get; set; }

        public Manager(int empId, string name, string department, double baseSalary, DateTime joiningDate, int teamSize, string projects)
            : base(empId, name, department, baseSalary, joiningDate)
        {
            TeamSize = teamSize;
            ProjectsHandled = projects;
        }

        // Implement CalculateBonus - Manager gets 20% bonus
        public override double CalculateBonus()
        {
            double baseBonus = BaseSalary * 0.20;

            // Additional bonus based on team size
            if (TeamSize > 10)
            {
                baseBonus += BaseSalary * 0.05; // Extra 5% for large teams
            }

            return baseBonus;
        }

        public override string GetPerformanceRating()
        {
            if (TeamSize > 15)
                return "Excellent (Large Team Management)";
            else if (TeamSize > 10)
                return "Very Good (Team Leadership)";
            else
                return "Good (Team Management)";
        }

        public void DisplayManagerDetails()
        {
            Console.WriteLine("\n┌─────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                    MANAGER DETAILS                      │");
            Console.WriteLine("├─────────────────────────────────────────────────────────┤");
            DisplayEmployeeInfo();
            Console.WriteLine($"Team Size     : {TeamSize} members");
            Console.WriteLine($"Projects      : {ProjectsHandled}");
            Console.WriteLine($"Bonus (20%+)  : Rs. {CalculateBonus():N2}");
            Console.WriteLine($"Total Pay     : Rs. {GetTotalCompensation():N2}");
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }
    }

    // Derived class - Developer (10% bonus)
    class Developer : Employee
    {
        public string TechnologyStack { get; set; }
        public int ProjectsCompleted { get; set; }

        public Developer(int empId, string name, string department, double baseSalary, DateTime joiningDate, string techStack, int projects)
            : base(empId, name, department, baseSalary, joiningDate)
        {
            TechnologyStack = techStack;
            ProjectsCompleted = projects;
        }

        // Implement CalculateBonus - Developer gets 10% bonus
        public override double CalculateBonus()
        {
            double baseBonus = BaseSalary * 0.10;

            // Additional bonus for experienced developers
            int experience = GetExperienceYears();
            if (experience > 5)
            {
                baseBonus += BaseSalary * 0.05; // Extra 5% for 5+ years
            }

            // Bonus for high project completion
            if (ProjectsCompleted > 10)
            {
                baseBonus += BaseSalary * 0.03; // Extra 3%
            }

            return baseBonus;
        }

        public override string GetPerformanceRating()
        {
            if (ProjectsCompleted > 15)
                return "Excellent (High Productivity)";
            else if (ProjectsCompleted > 10)
                return "Very Good (Consistent Delivery)";
            else if (ProjectsCompleted > 5)
                return "Good (Satisfactory)";
            else
                return "Average (Developing)";
        }

        public void DisplayDeveloperDetails()
        {
            Console.WriteLine("\n┌─────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                   DEVELOPER DETAILS                     │");
            Console.WriteLine("├─────────────────────────────────────────────────────────┤");
            DisplayEmployeeInfo();
            Console.WriteLine($"Tech Stack    : {TechnologyStack}");
            Console.WriteLine($"Projects Done : {ProjectsCompleted}");
            Console.WriteLine($"Experience    : {GetExperienceYears()} years");
            Console.WriteLine($"Bonus (10%+)  : Rs. {CalculateBonus():N2}");
            Console.WriteLine($"Total Pay     : Rs. {GetTotalCompensation():N2}");
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }
    }

    // Additional class - Intern (5% bonus)
    class Intern : Employee
    {
        public string University { get; set; }
        public int InternshipMonths { get; set; }

        public Intern(int empId, string name, string department, double baseSalary, DateTime joiningDate, string university, int months)
            : base(empId, name, department, baseSalary, joiningDate)
        {
            University = university;
            InternshipMonths = months;
        }

        public override double CalculateBonus()
        {
            // Interns get 5% bonus
            return BaseSalary * 0.05;
        }

        public override string GetPerformanceRating()
        {
            if (InternshipMonths >= 6)
                return "Good (Extended Internship)";
            else
                return "Satisfactory (Learning Phase)";
        }

        public void DisplayInternDetails()
        {
            Console.WriteLine("\n┌─────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                    INTERN DETAILS                       │");
            Console.WriteLine("├─────────────────────────────────────────────────────────┤");
            DisplayEmployeeInfo();
            Console.WriteLine($"University    : {University}");
            Console.WriteLine($"Duration      : {InternshipMonths} months");
            Console.WriteLine($"Bonus (5%)    : Rs. {CalculateBonus():N2}");
            Console.WriteLine($"Total Pay     : Rs. {GetTotalCompensation():N2}");
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Display header
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║        Lab 05 - C02: Employee Bonus Calculation            ║");
            Console.WriteLine("║      (Polymorphism with Abstract Class)                    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Creating Employee Objects:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                // Create Managers
                Manager manager1 = new Manager(
                    101,
                    "Rajesh Kumar",
                    "IT Department",
                    120000,
                    new DateTime(2020, 3, 15),
                    12,
                    "ERP System, Mobile App, Web Portal"
                );

                Manager manager2 = new Manager(
                    102,
                    "Priya Sharma",
                    "Sales Department",
                    150000,
                    new DateTime(2018, 6, 1),
                    18,
                    "CRM Implementation, Sales Strategy"
                );

                // Create Developers
                Developer dev1 = new Developer(
                    201,
                    "Amit Patel",
                    "IT Department",
                    80000,
                    new DateTime(2019, 9, 10),
                    "C#, ASP.NET, SQL Server",
                    12
                );

                Developer dev2 = new Developer(
                    202,
                    "Sneha Desai",
                    "IT Department",
                    75000,
                    new DateTime(2017, 1, 20),
                    "Java, Spring Boot, MongoDB",
                    18
                );

                // Create Interns
                Intern intern1 = new Intern(
                    301,
                    "Karan Shah",
                    "IT Department",
                    20000,
                    new DateTime(2025, 6, 1),
                    "Darshan University",
                    6
                );

                // Display individual details
                manager1.DisplayManagerDetails();
                manager2.DisplayManagerDetails();
                dev1.DisplayDeveloperDetails();
                dev2.DisplayDeveloperDetails();
                intern1.DisplayInternDetails();

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Polymorphism Demonstration:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                // Create array of Employee references
                Employee[] employees = { manager1, manager2, dev1, dev2, intern1 };

                Console.WriteLine("\nCalculating bonus for all employees:\n");
                Console.WriteLine("┌──────┬──────────────────┬────────────┬──────────────┬──────────────┬──────────────┐");
                Console.WriteLine("│  ID  │      Name        │    Role    │ Base Salary  │    Bonus     │  Total Pay   │");
                Console.WriteLine("├──────┼──────────────────┼────────────┼──────────────┼──────────────┼──────────────┤");

                double totalSalary = 0;
                double totalBonus = 0;

                foreach (Employee emp in employees)
                {
                    string role = emp is Manager ? "Manager" : emp is Developer ? "Developer" : "Intern";
                    double bonus = emp.CalculateBonus();
                    double total = emp.GetTotalCompensation();

                    totalSalary += emp.BaseSalary;
                    totalBonus += bonus;

                    Console.WriteLine($"│ {emp.EmployeeId,-4} │ {emp.Name,-16} │ {role,-10} │ {emp.BaseSalary,12:N2} │ {bonus,12:N2} │ {total,12:N2} │");
                }

                Console.WriteLine("├──────┴──────────────────┴────────────┼──────────────┼──────────────┼──────────────┤");
                Console.WriteLine($"│                          TOTAL       │ {totalSalary,12:N2} │ {totalBonus,12:N2} │ {(totalSalary + totalBonus),12:N2} │");
                Console.WriteLine("└──────────────────────────────────────┴──────────────┴──────────────┴──────────────┘");

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Bonus Percentage Analysis:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                foreach (Employee emp in employees)
                {
                    double bonusPercentage = (emp.CalculateBonus() / emp.BaseSalary) * 100;
                    Console.WriteLine($"{emp.Name,-20} : {bonusPercentage:F2}% bonus");
                }

                Console.WriteLine();

                // Explain concepts
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Concepts Demonstrated:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("✓ Abstract Employee class with CalculateBonus() method");
                Console.WriteLine("✓ Manager gets 20% base bonus (+ extra for team size)");
                Console.WriteLine("✓ Developer gets 10% base bonus (+ extra for experience)");
                Console.WriteLine("✓ Intern gets 5% bonus");
                Console.WriteLine("✓ Polymorphism - same method, different implementations");
                Console.WriteLine("✓ Employee array holds all types");
                Console.WriteLine("✓ Runtime polymorphism in action");
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
