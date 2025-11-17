using System;
using System.Collections.Generic;

namespace Lab05_C03_VehicleRental
{
    // Interface - IRentable
    // Defines contract for rentable vehicles
    interface IRentable
    {
        double CalculateRent(int days);
        void DisplayDetails();
    }

    // Class - Car implements IRentable
    class Car : IRentable
    {
        public string CarId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string FuelType { get; set; }
        public int SeatingCapacity { get; set; }
        public double RentPerDay { get; set; }
        public bool HasAC { get; set; }

        public Car(string carId, string brand, string model, string fuelType, int seating, double rentPerDay, bool hasAC)
        {
            CarId = carId;
            Brand = brand;
            Model = model;
            FuelType = fuelType;
            SeatingCapacity = seating;
            RentPerDay = rentPerDay;
            HasAC = hasAC;
        }

        // Implement CalculateRent - Car has different pricing logic
        public double CalculateRent(int days)
        {
            if (days <= 0)
            {
                throw new ArgumentException("Number of days must be greater than 0");
            }

            double totalRent = days * RentPerDay;

            // Discount for longer rentals
            if (days >= 7 && days < 30)
            {
                totalRent *= 0.90; // 10% discount for weekly rental
            }
            else if (days >= 30)
            {
                totalRent *= 0.80; // 20% discount for monthly rental
            }

            // Additional charges for AC
            if (HasAC)
            {
                totalRent += (days * 200); // Rs. 200 per day for AC
            }

            return totalRent;
        }

        // Implement DisplayDetails
        public void DisplayDetails()
        {
            Console.WriteLine("┌─────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                      CAR DETAILS                        │");
            Console.WriteLine("├─────────────────────────────────────────────────────────┤");
            Console.WriteLine($"│ Car ID        : {CarId,-41} │");
            Console.WriteLine($"│ Brand         : {Brand,-41} │");
            Console.WriteLine($"│ Model         : {Model,-41} │");
            Console.WriteLine($"│ Fuel Type     : {FuelType,-41} │");
            Console.WriteLine($"│ Seating       : {SeatingCapacity,-41} │");
            Console.WriteLine($"│ Rent/Day      : Rs. {RentPerDay,-37:N2} │");
            Console.WriteLine($"│ AC Available  : {(HasAC ? "Yes" : "No"),-41} │");
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }

        public void DisplayRentalInfo(int days)
        {
            double totalRent = CalculateRent(days);
            double baseRent = days * RentPerDay;
            double discount = baseRent - (HasAC ? totalRent - (days * 200) : totalRent);
            double acCharges = HasAC ? days * 200 : 0;

            Console.WriteLine($"\nRental Period : {days} days");
            Console.WriteLine($"Base Rent     : Rs. {baseRent:N2}");
            if (discount > 0)
                Console.WriteLine($"Discount      : Rs. {discount:N2}");
            if (acCharges > 0)
                Console.WriteLine($"AC Charges    : Rs. {acCharges:N2}");
            Console.WriteLine($"Total Rent    : Rs. {totalRent:N2}");
        }
    }

    // Class - Bike implements IRentable
    class Bike : IRentable
    {
        public string BikeId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public int EngineCC { get; set; }
        public string BikeType { get; set; } // Sport, Cruiser, Commuter
        public double RentPerDay { get; set; }
        public bool HasHelmet { get; set; }

        public Bike(string bikeId, string brand, string model, int engineCC, string bikeType, double rentPerDay, bool hasHelmet)
        {
            BikeId = bikeId;
            Brand = brand;
            Model = model;
            EngineCC = engineCC;
            BikeType = bikeType;
            RentPerDay = rentPerDay;
            HasHelmet = hasHelmet;
        }

        // Implement CalculateRent - Bike has different pricing logic
        public double CalculateRent(int days)
        {
            if (days <= 0)
            {
                throw new ArgumentException("Number of days must be greater than 0");
            }

            double totalRent = days * RentPerDay;

            // Discount for longer rentals (different from car)
            if (days >= 5 && days < 15)
            {
                totalRent *= 0.95; // 5% discount for 5+ days
            }
            else if (days >= 15)
            {
                totalRent *= 0.85; // 15% discount for 15+ days
            }

            // Helmet charges
            if (!HasHelmet)
            {
                totalRent += (days * 50); // Rs. 50 per day for helmet rental
            }

            return totalRent;
        }

        // Implement DisplayDetails
        public void DisplayDetails()
        {
            Console.WriteLine("┌─────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                     BIKE DETAILS                        │");
            Console.WriteLine("├─────────────────────────────────────────────────────────┤");
            Console.WriteLine($"│ Bike ID       : {BikeId,-41} │");
            Console.WriteLine($"│ Brand         : {Brand,-41} │");
            Console.WriteLine($"│ Model         : {Model,-41} │");
            Console.WriteLine($"│ Engine        : {EngineCC,-41} CC │");
            Console.WriteLine($"│ Type          : {BikeType,-41} │");
            Console.WriteLine($"│ Rent/Day      : Rs. {RentPerDay,-37:N2} │");
            Console.WriteLine($"│ Helmet Inc.   : {(HasHelmet ? "Yes" : "No (Extra Rs. 50/day)"),-41} │");
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }

        public void DisplayRentalInfo(int days)
        {
            double totalRent = CalculateRent(days);
            double baseRent = days * RentPerDay;
            double discount = baseRent - (HasHelmet ? totalRent : totalRent - (days * 50));
            double helmetCharges = HasHelmet ? 0 : days * 50;

            Console.WriteLine($"\nRental Period : {days} days");
            Console.WriteLine($"Base Rent     : Rs. {baseRent:N2}");
            if (discount > 0)
                Console.WriteLine($"Discount      : Rs. {discount:N2}");
            if (helmetCharges > 0)
                Console.WriteLine($"Helmet Charges: Rs. {helmetCharges:N2}");
            Console.WriteLine($"Total Rent    : Rs. {totalRent:N2}");
        }
    }

    // Additional class - Scooter implements IRentable
    class Scooter : IRentable
    {
        public string ScooterId { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public double RentPerDay { get; set; }
        public bool IsElectric { get; set; }

        public Scooter(string scooterId, string brand, string model, double rentPerDay, bool isElectric)
        {
            ScooterId = scooterId;
            Brand = brand;
            Model = model;
            RentPerDay = rentPerDay;
            IsElectric = isElectric;
        }

        public double CalculateRent(int days)
        {
            if (days <= 0)
            {
                throw new ArgumentException("Number of days must be greater than 0");
            }

            double totalRent = days * RentPerDay;

            // Electric scooters get 10% discount
            if (IsElectric)
            {
                totalRent *= 0.90;
            }

            // Simple discount for longer rentals
            if (days >= 10)
            {
                totalRent *= 0.92; // Additional 8% off
            }

            return totalRent;
        }

        public void DisplayDetails()
        {
            Console.WriteLine("┌─────────────────────────────────────────────────────────┐");
            Console.WriteLine("│                   SCOOTER DETAILS                       │");
            Console.WriteLine("├─────────────────────────────────────────────────────────┤");
            Console.WriteLine($"│ Scooter ID    : {ScooterId,-41} │");
            Console.WriteLine($"│ Brand         : {Brand,-41} │");
            Console.WriteLine($"│ Model         : {Model,-41} │");
            Console.WriteLine($"│ Type          : {(IsElectric ? "Electric" : "Petrol"),-41} │");
            Console.WriteLine($"│ Rent/Day      : Rs. {RentPerDay,-37:N2} │");
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Display header
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         Lab 05 - C03: Vehicle Rental System                ║");
            Console.WriteLine("║      (IRentable Interface Implementation)                  ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Available Vehicles for Rent:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                // Create Car objects
                Car car1 = new Car("CAR001", "Maruti Suzuki", "Swift", "Petrol", 5, 2000, true);
                Car car2 = new Car("CAR002", "Hyundai", "Creta", "Diesel", 7, 3500, true);
                Car car3 = new Car("CAR003", "Tata", "Indica", "Petrol", 5, 1500, false);

                // Create Bike objects
                Bike bike1 = new Bike("BIKE001", "Royal Enfield", "Classic 350", 350, "Cruiser", 800, true);
                Bike bike2 = new Bike("BIKE002", "Yamaha", "R15", 155, "Sport", 1200, false);
                Bike bike3 = new Bike("BIKE003", "Honda", "Activa", 110, "Commuter", 500, true);

                // Create Scooter objects
                Scooter scooter1 = new Scooter("SCO001", "Ather", "450X", 600, true);
                Scooter scooter2 = new Scooter("SCO002", "TVS", "Jupiter", 400, false);

                // Display all vehicles
                Console.WriteLine("[CARS]\n");
                car1.DisplayDetails();
                Console.WriteLine();
                car2.DisplayDetails();
                Console.WriteLine();
                car3.DisplayDetails();

                Console.WriteLine("\n\n[BIKES]\n");
                bike1.DisplayDetails();
                Console.WriteLine();
                bike2.DisplayDetails();
                Console.WriteLine();
                bike3.DisplayDetails();

                Console.WriteLine("\n\n[SCOOTERS]\n");
                scooter1.DisplayDetails();
                Console.WriteLine();
                scooter2.DisplayDetails();

                // Rental calculations
                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Sample Rental Calculations:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                Console.WriteLine("\n[Rental 1: Swift for 5 days]");
                car1.DisplayRentalInfo(5);

                Console.WriteLine("\n[Rental 2: Creta for 10 days (10% discount)]");
                car2.DisplayRentalInfo(10);

                Console.WriteLine("\n[Rental 3: Classic 350 for 7 days]");
                bike1.DisplayRentalInfo(7);

                Console.WriteLine("\n[Rental 4: R15 for 15 days (with helmet charges)]");
                bike2.DisplayRentalInfo(15);

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Polymorphism with IRentable Interface:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                // Create array of IRentable references
                IRentable[] vehicles = { car1, car2, bike1, bike2, scooter1 };

                Console.WriteLine("\nCalculating rent for 3 days for all vehicles:\n");
                Console.WriteLine("┌──────────┬────────────────────────┬──────────────┬──────────────┐");
                Console.WriteLine("│ Vehicle  │         Name           │  Rent/Day    │  Total (3d)  │");
                Console.WriteLine("├──────────┼────────────────────────┼──────────────┼──────────────┤");

                foreach (IRentable vehicle in vehicles)
                {
                    string vehicleType = "";
                    string vehicleName = "";

                    if (vehicle is Car c)
                    {
                        vehicleType = "Car";
                        vehicleName = $"{c.Brand} {c.Model}";
                    }
                    else if (vehicle is Bike b)
                    {
                        vehicleType = "Bike";
                        vehicleName = $"{b.Brand} {b.Model}";
                    }
                    else if (vehicle is Scooter s)
                    {
                        vehicleType = "Scooter";
                        vehicleName = $"{s.Brand} {s.Model}";
                    }

                    double rent3Days = vehicle.CalculateRent(3);

                    Console.WriteLine($"│ {vehicleType,-8} │ {vehicleName,-22} │ Varies       │ {rent3Days,12:N2} │");
                }

                Console.WriteLine("└──────────┴────────────────────────┴──────────────────────────┘");

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Discount Comparison (30 days rental):");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                Console.WriteLine("Swift (Car) - 30 days:");
                car1.DisplayRentalInfo(30);

                Console.WriteLine("\n\nClassic 350 (Bike) - 30 days:");
                bike1.DisplayRentalInfo(30);

                Console.WriteLine();

                // Explain concepts
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Concepts Demonstrated:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("✓ IRentable interface with CalculateRent() & DisplayDetails()");
                Console.WriteLine("✓ Car, Bike, Scooter implement IRentable");
                Console.WriteLine("✓ Each vehicle has DIFFERENT rent calculation logic:");
                Console.WriteLine("  • Cars: 10% off (7+ days), 20% off (30+ days), AC charges");
                Console.WriteLine("  • Bikes: 5% off (5+ days), 15% off (15+ days), helmet charges");
                Console.WriteLine("  • Scooters: 10% off (electric), 8% off (10+ days)");
                Console.WriteLine("✓ Polymorphism with IRentable reference");
                Console.WriteLine("✓ Interface enables common operations on different types");
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
