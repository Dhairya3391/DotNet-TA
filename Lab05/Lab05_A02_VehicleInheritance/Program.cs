using System;

namespace Lab05_A02_VehicleInheritance
{
    // Base class - Vehicle (Level 1)
    // Demonstrates multilevel inheritance concept
    class Vehicle
    {
        public string Brand { get; set; }
        public string Model { get; set; }

        public Vehicle(string brand, string model)
        {
            Brand = brand;
            Model = model;
        }

        // Method to display vehicle type
        public virtual void DisplayType()
        {
            Console.WriteLine("This is a Vehicle.");
        }

        // Method to display basic info
        public void DisplayBasicInfo()
        {
            Console.WriteLine($"Brand: {Brand}, Model: {Model}");
        }
    }

    // Derived class - Car inherits from Vehicle (Level 2)
    class Car : Vehicle
    {
        public int NumberOfDoors { get; set; }

        public Car(string brand, string model, int doors) : base(brand, model)
        {
            NumberOfDoors = doors;
        }

        // Override DisplayType method
        public override void DisplayType()
        {
            Console.WriteLine("This is a Car (4-wheeler).");
        }

        // Method specific to Car
        public void DisplayCarInfo()
        {
            Console.WriteLine($"Number of Doors: {NumberOfDoors}");
        }
    }

    // Derived class - ElectricCar inherits from Car (Level 3)
    // This demonstrates multilevel inheritance: Vehicle → Car → ElectricCar
    class ElectricCar : Car
    {
        public int BatteryCapacity { get; set; } // in kWh
        public int Range { get; set; } // in kilometers

        public ElectricCar(string brand, string model, int doors, int batteryCapacity, int range)
            : base(brand, model, doors)
        {
            BatteryCapacity = batteryCapacity;
            Range = range;
        }

        // Override DisplayType method
        public override void DisplayType()
        {
            Console.WriteLine("This is an Electric Car (Zero Emission Vehicle).");
        }

        // Method specific to ElectricCar
        public void DisplayElectricCarInfo()
        {
            Console.WriteLine($"Battery Capacity: {BatteryCapacity} kWh");
            Console.WriteLine($"Range: {Range} km on full charge");
        }

        // Method to display complete information
        public void DisplayCompleteInfo()
        {
            Console.WriteLine("\n┌─────────────────────────────────────────────────────────┐");
            DisplayBasicInfo();         // From Vehicle (Grandparent)
            DisplayCarInfo();            // From Car (Parent)
            DisplayElectricCarInfo();    // From ElectricCar (Self)
            DisplayType();               // Overridden method
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Display header
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║       Lab 05 - A02: Vehicle Inheritance Demo               ║");
            Console.WriteLine("║  (Multilevel Inheritance: Vehicle → Car → ElectricCar)    ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                // Level 1: Create Vehicle object
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Level 1: Creating Vehicle object:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Vehicle vehicle = new Vehicle("Generic", "Vehicle-X");
                vehicle.DisplayBasicInfo();
                vehicle.DisplayType();
                Console.WriteLine();

                // Level 2: Create Car object
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Level 2: Creating Car object:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Car car = new Car("Toyota", "Camry", 4);
                car.DisplayBasicInfo();
                car.DisplayCarInfo();
                car.DisplayType();
                Console.WriteLine();

                // Level 3: Create ElectricCar objects
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Level 3: Creating ElectricCar objects:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                // Electric Car 1
                Console.WriteLine("\n[Electric Car 1]");
                ElectricCar tesla = new ElectricCar("Tesla", "Model S", 4, 100, 650);
                tesla.DisplayCompleteInfo();

                // Electric Car 2
                Console.WriteLine("\n[Electric Car 2]");
                ElectricCar tata = new ElectricCar("Tata", "Nexon EV", 4, 40, 312);
                tata.DisplayCompleteInfo();

                // Electric Car 3
                Console.WriteLine("\n[Electric Car 3]");
                ElectricCar mg = new ElectricCar("MG", "ZS EV", 4, 44, 419);
                mg.DisplayCompleteInfo();

                Console.WriteLine();

                // Demonstrating multilevel inheritance
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Multilevel Inheritance Demonstration:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("✓ Level 1: Vehicle (Base class)");
                Console.WriteLine("✓ Level 2: Car inherits from Vehicle");
                Console.WriteLine("✓ Level 3: ElectricCar inherits from Car");
                Console.WriteLine("✓ ElectricCar has access to all methods from Vehicle & Car");
                Console.WriteLine("✓ Method overriding demonstrated with DisplayType()");
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
