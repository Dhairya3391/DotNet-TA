/*
 * Lab03_C02_CarRental
 * Problem: Create a CarRental class with carModel, dailyRate, and rentedDays.
 *          Implement CalculateRent() method and throw exception if days <= 0.
 *
 * Learning Objectives:
 * - Understanding business logic implementation
 * - Custom validation and exception handling
 * - Method implementation with calculations
 * - Professional output formatting
 */

using System;

namespace Lab03_C02_CarRental
{
    // Custom exception for invalid rental days
    class InvalidRentalDaysException : Exception
    {
        public InvalidRentalDaysException(string message) : base(message) { }
    }

    // CarRental class
    class CarRental
    {
        // Properties
        public string CarModel { get; set; }
        public double DailyRate { get; set; }
        private int rentedDays;
        public string CustomerName { get; set; }
        public DateTime RentalStartDate { get; set; }

        // Property with validation for rented days
        public int RentedDays
        {
            get { return rentedDays; }
            set
            {
                if (value <= 0)
                {
                    throw new InvalidRentalDaysException(
                        "Rental days must be greater than 0. Cannot rent a car for zero or negative days!");
                }
                rentedDays = value;
            }
        }

        // Parameterized constructor
        public CarRental(string carModel, double dailyRate, int rentedDays, string customerName)
        {
            CarModel = carModel;
            DailyRate = dailyRate;
            CustomerName = customerName;
            RentalStartDate = DateTime.Now;

            // This will trigger validation
            RentedDays = rentedDays;
        }

        // Method to calculate total rent
        public double CalculateRent()
        {
            if (RentedDays <= 0)
            {
                throw new InvalidRentalDaysException(
                    "Cannot calculate rent for zero or negative days!");
            }

            double baseRent = DailyRate * RentedDays;

            // Apply discount for longer rentals
            double discount = 0;
            if (RentedDays >= 7 && RentedDays < 30)
            {
                discount = baseRent * 0.10; // 10% discount for weekly rental
            }
            else if (RentedDays >= 30)
            {
                discount = baseRent * 0.20; // 20% discount for monthly rental
            }

            return baseRent - discount;
        }

        // Method to calculate discount percentage
        public double GetDiscountPercentage()
        {
            if (RentedDays >= 30)
                return 20;
            else if (RentedDays >= 7)
                return 10;
            else
                return 0;
        }

        // Method to get rental end date
        public DateTime GetRentalEndDate()
        {
            return RentalStartDate.AddDays(RentedDays);
        }

        // Method to display rental details
        public void DisplayRentalDetails()
        {
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"Customer Name    : {CustomerName}");
            Console.WriteLine($"Car Model        : {CarModel}");
            Console.WriteLine($"Daily Rate       : Rs. {DailyRate:N2}");
            Console.WriteLine($"Rented Days      : {RentedDays} days");
            Console.WriteLine($"Rental Start     : {RentalStartDate:dd-MMM-yyyy}");
            Console.WriteLine($"Rental End       : {GetRentalEndDate():dd-MMM-yyyy}");

            double baseRent = DailyRate * RentedDays;
            double discount = GetDiscountPercentage();
            double totalRent = CalculateRent();

            Console.WriteLine($"Base Rent        : Rs. {baseRent:N2}");
            if (discount > 0)
            {
                Console.WriteLine($"Discount         : {discount}% (Rs. {(baseRent - totalRent):N2})");
            }
            Console.WriteLine($"Total Rent       : Rs. {totalRent:N2}");
            Console.WriteLine(new string('-', 60));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader("Car Rental Management System");

            try
            {
                // Rental 1: Short term rental (no discount)
                Console.WriteLine("\n*** Rental 1: Short Term ***");
                CarRental rental1 = new CarRental("Honda City", 1200.00, 3, "Amit Desai");
                rental1.DisplayRentalDetails();

                // Rental 2: Weekly rental (10% discount)
                Console.WriteLine("\n*** Rental 2: Weekly Rental ***");
                CarRental rental2 = new CarRental("Toyota Fortuner", 3500.00, 10, "Priya Mehta");
                rental2.DisplayRentalDetails();

                // Rental 3: Monthly rental (20% discount)
                Console.WriteLine("\n*** Rental 3: Monthly Rental ***");
                CarRental rental3 = new CarRental("Hyundai Creta", 2000.00, 30, "Vikram Singh");
                rental3.DisplayRentalDetails();

                // Demonstrate exception: Invalid rental days (zero days)
                Console.WriteLine("\n*** Attempting Rental with Zero Days ***");
                CarRental rental4 = new CarRental("Maruti Swift", 800.00, 0, "Invalid Customer");
            }
            catch (InvalidRentalDaysException ex)
            {
                Console.WriteLine($"\n*** RENTAL FAILED ***");
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n*** ERROR ***");
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Demonstrate exception: Negative rental days
            Console.WriteLine("\n\n*** Testing Negative Rental Days ***");
            try
            {
                CarRental rental5 = new CarRental("BMW X5", 5000.00, -5, "Test Customer");
            }
            catch (InvalidRentalDaysException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            Console.WriteLine("\n*** Discount Policy ***");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine("1-6 days   : No discount");
            Console.WriteLine("7-29 days  : 10% discount");
            Console.WriteLine("30+ days   : 20% discount");

            PrintFooter();
        }

        // Helper method to print header
        static void PrintHeader(string title)
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('=', 60));
        }

        // Helper method to print footer
        static void PrintFooter()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
