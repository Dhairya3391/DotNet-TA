// Lab 1 - B Task 2
// Employee salary calculator: Calculate gross and net salary
// HRA = 10%, DA = 15%, Deductions = 8%

Console.WriteLine("========================================");
Console.WriteLine("    EMPLOYEE SALARY CALCULATOR");
Console.WriteLine("========================================");
Console.WriteLine();

Console.Write("Enter basic salary: ₹");
double basicSalary = Convert.ToDouble(Console.ReadLine());

// Calculate components
double hra = basicSalary * 0.10;      // 10% of basic
double da = basicSalary * 0.15;       // 15% of basic
double grossSalary = basicSalary + hra + da;
double deductions = grossSalary * 0.08;  // 8% of gross
double netSalary = grossSalary - deductions;

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("       SALARY BREAKDOWN");
Console.WriteLine("========================================");
Console.WriteLine($"Basic Salary  : ₹{basicSalary:F2}");
Console.WriteLine($"HRA (10%)     : ₹{hra:F2}");
Console.WriteLine($"DA (15%)      : ₹{da:F2}");
Console.WriteLine("----------------------------------------");
Console.WriteLine($"Gross Salary  : ₹{grossSalary:F2}");
Console.WriteLine($"Deductions(8%): ₹{deductions:F2}");
Console.WriteLine("----------------------------------------");
Console.WriteLine($"Net Salary    : ₹{netSalary:F2}");
Console.WriteLine("========================================");
