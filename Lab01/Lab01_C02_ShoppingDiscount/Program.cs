// Lab 1 - C Task 2
// Shopping discount calculator with tiered discounts
// 5% for orders below ₹5000
// 10% for orders between ₹5000 and ₹10000
// 15% for orders above ₹10000

Console.WriteLine("========================================");
Console.WriteLine("   ONLINE SHOPPING DISCOUNT");
Console.WriteLine("========================================");
Console.WriteLine();

Console.Write("Enter total purchase amount: ₹");
double originalAmount = Convert.ToDouble(Console.ReadLine());

double discountPercent;
double discountAmount;

// Determine discount based on purchase amount
if (originalAmount < 5000)
{
    discountPercent = 5;
}
else if (originalAmount >= 5000 && originalAmount <= 10000)
{
    discountPercent = 10;
}
else // originalAmount > 10000
{
    discountPercent = 15;
}

discountAmount = originalAmount * (discountPercent / 100);
double finalAmount = originalAmount - discountAmount;

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("       BILL SUMMARY");
Console.WriteLine("========================================");
Console.WriteLine($"Original Amount   : ₹{originalAmount:F2}");
Console.WriteLine($"Discount ({discountPercent}%)     : -₹{discountAmount:F2}");
Console.WriteLine("----------------------------------------");
Console.WriteLine($"Final Amount      : ₹{finalAmount:F2}");
Console.WriteLine("========================================");
Console.WriteLine($"You saved ₹{discountAmount:F2} on this order!");
