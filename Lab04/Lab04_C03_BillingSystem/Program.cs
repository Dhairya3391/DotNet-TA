/*
 * Lab04_C03_BillingSystem
 * Demonstrates method overriding with different discount logic
 * Customer base class with CalculateBill(), RegularCustomer and PremiumCustomer
 */

using System;
using System.Collections.Generic;

namespace Lab04_C03_BillingSystem
{
    // Base class Customer
    abstract class Customer
    {
        // Protected fields
        protected string customerId;
        protected string customerName;
        protected string customerType;

        // Private field
        private DateTime registrationDate;

        // Constructor
        public Customer(string customerId, string customerName, string customerType)
        {
            this.customerId = customerId;
            this.customerName = customerName;
            this.customerType = customerType;
            this.registrationDate = DateTime.Now;
        }

        // Public properties
        public string CustomerId { get { return customerId; } }
        public string CustomerName { get { return customerName; } }
        public string CustomerType { get { return customerType; } }

        // Abstract method - must be overridden
        public abstract double CalculateBill(double purchaseAmount);

        // Virtual method - can be overridden
        public virtual void DisplayCustomerInfo()
        {
            Console.WriteLine("┌────────────────────────────────────────────┐");
            Console.WriteLine("│         CUSTOMER INFORMATION               │");
            Console.WriteLine("├────────────────────────────────────────────┤");
            Console.WriteLine($"│ ID:   {customerId,-35} │");
            Console.WriteLine($"│ Name: {customerName,-35} │");
            Console.WriteLine($"│ Type: {customerType,-35} │");
            Console.WriteLine("└────────────────────────────────────────────┘");
        }

        // Virtual method for displaying benefits
        public virtual void DisplayBenefits()
        {
            Console.WriteLine("Standard customer benefits");
        }
    }

    // Regular Customer - no discount
    class RegularCustomer : Customer
    {
        private int purchaseCount;

        public RegularCustomer(string customerId, string customerName)
            : base(customerId, customerName, "Regular Customer")
        {
            this.purchaseCount = 0;
        }

        // Override CalculateBill - no discount
        public override double CalculateBill(double purchaseAmount)
        {
            purchaseCount++;
            return purchaseAmount; // No discount for regular customers
        }

        public override void DisplayCustomerInfo()
        {
            Console.WriteLine("┌────────────────────────────────────────────┐");
            Console.WriteLine("│      REGULAR CUSTOMER INFORMATION          │");
            Console.WriteLine("├────────────────────────────────────────────┤");
            Console.WriteLine($"│ ID:              {customerId,-25} │");
            Console.WriteLine($"│ Name:            {customerName,-25} │");
            Console.WriteLine($"│ Type:            {customerType,-25} │");
            Console.WriteLine($"│ Purchase Count:  {purchaseCount,-25} │");
            Console.WriteLine($"│ Discount Rate:   0%{"",-25} │");
            Console.WriteLine("└────────────────────────────────────────────┘");
        }

        public override void DisplayBenefits()
        {
            Console.WriteLine("\nRegular Customer Benefits:");
            Console.WriteLine("  • Standard pricing");
            Console.WriteLine("  • Access to all products");
            Console.WriteLine("  • Customer support");
        }
    }

    // Premium Customer - 15% discount
    class PremiumCustomer : Customer
    {
        private double discountRate = 0.15; // 15% discount
        private int purchaseCount;
        private double totalSavings;

        public PremiumCustomer(string customerId, string customerName)
            : base(customerId, customerName, "Premium Customer")
        {
            this.purchaseCount = 0;
            this.totalSavings = 0;
        }

        // Override CalculateBill - 15% discount
        public override double CalculateBill(double purchaseAmount)
        {
            purchaseCount++;
            double discount = purchaseAmount * discountRate;
            double finalAmount = purchaseAmount - discount;
            totalSavings += discount;
            return finalAmount;
        }

        public override void DisplayCustomerInfo()
        {
            Console.WriteLine("┌────────────────────────────────────────────┐");
            Console.WriteLine("│      PREMIUM CUSTOMER INFORMATION          │");
            Console.WriteLine("├────────────────────────────────────────────┤");
            Console.WriteLine($"│ ID:              {customerId,-25} │");
            Console.WriteLine($"│ Name:            {customerName,-25} │");
            Console.WriteLine($"│ Type:            {customerType,-25} │");
            Console.WriteLine($"│ Purchase Count:  {purchaseCount,-25} │");
            Console.WriteLine($"│ Discount Rate:   {discountRate * 100}%{"",-24} │");
            Console.WriteLine($"│ Total Savings:   ${totalSavings,-24:F2} │");
            Console.WriteLine("└────────────────────────────────────────────┘");
        }

        public override void DisplayBenefits()
        {
            Console.WriteLine("\nPremium Customer Benefits:");
            Console.WriteLine("  • 15% discount on all purchases");
            Console.WriteLine("  • Free shipping");
            Console.WriteLine("  • Priority customer support");
            Console.WriteLine("  • Early access to new products");
            Console.WriteLine("  • Extended return policy (60 days)");
        }
    }

    // VIP Customer - 25% discount + loyalty points
    class VIPCustomer : Customer
    {
        private double discountRate = 0.25; // 25% discount
        private int purchaseCount;
        private double totalSavings;
        private int loyaltyPoints;

        public VIPCustomer(string customerId, string customerName)
            : base(customerId, customerName, "VIP Customer")
        {
            this.purchaseCount = 0;
            this.totalSavings = 0;
            this.loyaltyPoints = 0;
        }

        // Override CalculateBill - 25% discount + loyalty points
        public override double CalculateBill(double purchaseAmount)
        {
            purchaseCount++;
            double discount = purchaseAmount * discountRate;
            double finalAmount = purchaseAmount - discount;
            totalSavings += discount;

            // Award loyalty points (1 point per $10 spent)
            loyaltyPoints += (int)(finalAmount / 10);

            return finalAmount;
        }

        public override void DisplayCustomerInfo()
        {
            Console.WriteLine("┌────────────────────────────────────────────┐");
            Console.WriteLine("│         VIP CUSTOMER INFORMATION           │");
            Console.WriteLine("├────────────────────────────────────────────┤");
            Console.WriteLine($"│ ID:              {customerId,-25} │");
            Console.WriteLine($"│ Name:            {customerName,-25} │");
            Console.WriteLine($"│ Type:            {customerType,-25} │");
            Console.WriteLine($"│ Purchase Count:  {purchaseCount,-25} │");
            Console.WriteLine($"│ Discount Rate:   {discountRate * 100}%{"",-24} │");
            Console.WriteLine($"│ Total Savings:   ${totalSavings,-24:F2} │");
            Console.WriteLine($"│ Loyalty Points:  {loyaltyPoints,-25} │");
            Console.WriteLine("└────────────────────────────────────────────┘");
        }

        public override void DisplayBenefits()
        {
            Console.WriteLine("\nVIP Customer Benefits:");
            Console.WriteLine("  • 25% discount on all purchases");
            Console.WriteLine("  • Free express shipping");
            Console.WriteLine("  • 24/7 dedicated customer support");
            Console.WriteLine("  • Exclusive access to limited editions");
            Console.WriteLine("  • Lifetime return policy");
            Console.WriteLine("  • Loyalty points program");
            Console.WriteLine("  • Birthday gifts and special offers");
        }

        public int GetLoyaltyPoints()
        {
            return loyaltyPoints;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║   LAB 04 - C03: BILLING SYSTEM (OVERRIDING)      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");

            // Create different types of customers
            RegularCustomer customer1 = new RegularCustomer("C001", "Rahul Verma");
            PremiumCustomer customer2 = new PremiumCustomer("C002", "Priya Sharma");
            VIPCustomer customer3 = new VIPCustomer("C003", "Amit Patel");

            Console.WriteLine("\n" + new string('═', 52));
            Console.WriteLine("   CUSTOMER REGISTRATION");
            Console.WriteLine(new string('═', 52));

            customer1.DisplayCustomerInfo();
            customer1.DisplayBenefits();

            Console.WriteLine();
            customer2.DisplayCustomerInfo();
            customer2.DisplayBenefits();

            Console.WriteLine();
            customer3.DisplayCustomerInfo();
            customer3.DisplayBenefits();

            // Test 1: Process purchases for each customer
            Console.WriteLine("\n" + new string('═', 52));
            Console.WriteLine("[Test 1] Processing Purchases");
            Console.WriteLine(new string('═', 52));

            double[] purchaseAmounts = { 1000, 2500, 1500 };

            Console.WriteLine("\n--- Purchase 1: $1,000 ---");
            ProcessPurchase(customer1, purchaseAmounts[0]);
            ProcessPurchase(customer2, purchaseAmounts[0]);
            ProcessPurchase(customer3, purchaseAmounts[0]);

            Console.WriteLine("\n--- Purchase 2: $2,500 ---");
            ProcessPurchase(customer1, purchaseAmounts[1]);
            ProcessPurchase(customer2, purchaseAmounts[1]);
            ProcessPurchase(customer3, purchaseAmounts[1]);

            Console.WriteLine("\n--- Purchase 3: $1,500 ---");
            ProcessPurchase(customer1, purchaseAmounts[2]);
            ProcessPurchase(customer2, purchaseAmounts[2]);
            ProcessPurchase(customer3, purchaseAmounts[2]);

            // Test 2: Display updated customer information
            Console.WriteLine("\n" + new string('═', 52));
            Console.WriteLine("[Test 2] Updated Customer Information");
            Console.WriteLine(new string('═', 52));

            Console.WriteLine("\nCustomer 1:");
            customer1.DisplayCustomerInfo();

            Console.WriteLine("\nCustomer 2:");
            customer2.DisplayCustomerInfo();

            Console.WriteLine("\nCustomer 3:");
            customer3.DisplayCustomerInfo();

            // Test 3: Polymorphic behavior
            Console.WriteLine("\n" + new string('═', 52));
            Console.WriteLine("[Test 3] Polymorphic Array of Customers");
            Console.WriteLine(new string('═', 52));

            Customer[] customers = new Customer[3];
            customers[0] = customer1;
            customers[1] = customer2;
            customers[2] = customer3;

            double testPurchase = 5000;
            Console.WriteLine($"\nProcessing a ${testPurchase:F2} purchase for all customers:\n");

            foreach (Customer customer in customers)
            {
                double bill = customer.CalculateBill(testPurchase);
                double savings = testPurchase - bill;

                Console.WriteLine($"{customer.CustomerName} ({customer.CustomerType}):");
                Console.WriteLine($"  Original Amount: ${testPurchase:F2}");
                Console.WriteLine($"  Final Bill:      ${bill:F2}");
                Console.WriteLine($"  You Saved:       ${savings:F2}");
                Console.WriteLine();
            }

            // Test 4: Comparison of total costs
            Console.WriteLine(new string('═', 52));
            Console.WriteLine("[Test 4] Cost Comparison Analysis");
            Console.WriteLine(new string('═', 52));

            double totalPurchases = 0;
            foreach (double amount in purchaseAmounts)
            {
                totalPurchases += amount;
            }
            totalPurchases += testPurchase; // Add the polymorphic test purchase

            Console.WriteLine($"\nTotal Purchase Amount: ${totalPurchases:F2}\n");

            // Calculate what each customer type would pay
            RegularCustomer tempRegular = new RegularCustomer("TEMP1", "Test Regular");
            PremiumCustomer tempPremium = new PremiumCustomer("TEMP2", "Test Premium");
            VIPCustomer tempVIP = new VIPCustomer("TEMP3", "Test VIP");

            double regularTotal = tempRegular.CalculateBill(totalPurchases);
            double premiumTotal = tempPremium.CalculateBill(totalPurchases);
            double vipTotal = tempVIP.CalculateBill(totalPurchases);

            Console.WriteLine("Cost Breakdown by Customer Type:");
            Console.WriteLine("┌────────────────────┬──────────────┬──────────────┐");
            Console.WriteLine("│ Customer Type      │ Amount Paid  │ Savings      │");
            Console.WriteLine("├────────────────────┼──────────────┼──────────────┤");
            Console.WriteLine($"│ Regular Customer   │ ${regularTotal,-11:F2} │ ${totalPurchases - regularTotal,-11:F2} │");
            Console.WriteLine($"│ Premium Customer   │ ${premiumTotal,-11:F2} │ ${totalPurchases - premiumTotal,-11:F2} │");
            Console.WriteLine($"│ VIP Customer       │ ${vipTotal,-11:F2} │ ${totalPurchases - vipTotal,-11:F2} │");
            Console.WriteLine("└────────────────────┴──────────────┴──────────────┘");

            // Final customer status
            Console.WriteLine("\n" + new string('═', 52));
            Console.WriteLine("   FINAL CUSTOMER STATUS");
            Console.WriteLine(new string('═', 52));

            foreach (Customer customer in customers)
            {
                Console.WriteLine();
                customer.DisplayCustomerInfo();
            }

            // Key Learning Points
            Console.WriteLine("\n╔══════════════════════════════════════════════════╗");
            Console.WriteLine("║         KEY LEARNING POINTS                      ║");
            Console.WriteLine("╚══════════════════════════════════════════════════╝");
            Console.WriteLine("\n✓ ABSTRACT CLASS: Customer base class cannot be");
            Console.WriteLine("  instantiated directly");
            Console.WriteLine("\n✓ ABSTRACT METHOD: CalculateBill() must be");
            Console.WriteLine("  implemented by all derived classes");
            Console.WriteLine("\n✓ METHOD OVERRIDING: Each customer type implements");
            Console.WriteLine("  different discount logic:");
            Console.WriteLine("  - Regular: 0% discount");
            Console.WriteLine("  - Premium: 15% discount");
            Console.WriteLine("  - VIP: 25% discount + loyalty points");
            Console.WriteLine("\n✓ POLYMORPHISM: Same method call produces different");
            Console.WriteLine("  results based on object type");
            Console.WriteLine("\n✓ ENCAPSULATION: Protected fields accessible in");
            Console.WriteLine("  derived classes, private fields hidden");
            Console.WriteLine("\n✓ REAL-WORLD APPLICATION: Billing systems commonly");
            Console.WriteLine("  use inheritance hierarchies for customer tiers");

            Console.WriteLine("\n" + new string('═', 52));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // Helper method to process purchase
        static void ProcessPurchase(Customer customer, double amount)
        {
            double finalBill = customer.CalculateBill(amount);
            double discount = amount - finalBill;

            Console.WriteLine($"\n{customer.CustomerName} ({customer.CustomerType}):");
            Console.WriteLine($"  Purchase Amount: ${amount:F2}");
            Console.WriteLine($"  Discount:        ${discount:F2}");
            Console.WriteLine($"  Final Bill:      ${finalBill:F2}");
        }
    }
}
