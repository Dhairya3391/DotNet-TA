// Lab 1 - C Task 1
// Currency converter: Convert INR to USD, EUR, GBP
// Fixed exchange rates (example rates)

Console.WriteLine("========================================");
Console.WriteLine("     CURRENCY CONVERTER");
Console.WriteLine("========================================");
Console.WriteLine();

// Fixed exchange rates (1 INR = ?)
const double INR_TO_USD = 0.012;  // 1 INR = 0.012 USD
const double INR_TO_EUR = 0.011;  // 1 INR = 0.011 EUR
const double INR_TO_GBP = 0.0095; // 1 INR = 0.0095 GBP

Console.Write("Enter amount in Indian Rupees (INR): ₹");
double inr = Convert.ToDouble(Console.ReadLine());

// Convert to different currencies
double usd = inr * INR_TO_USD;
double eur = inr * INR_TO_EUR;
double gbp = inr * INR_TO_GBP;

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("      CONVERSION RESULTS");
Console.WriteLine("========================================");
Console.WriteLine($"INR Amount     : ₹{inr:F2}");
Console.WriteLine("----------------------------------------");
Console.WriteLine($"US Dollar      : ${usd:F2}");
Console.WriteLine($"Euro           : €{eur:F2}");
Console.WriteLine($"British Pound  : £{gbp:F2}");
Console.WriteLine("========================================");
Console.WriteLine("Note: Fixed exchange rates used");
