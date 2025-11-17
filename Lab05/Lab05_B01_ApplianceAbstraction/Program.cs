using System;

namespace Lab05_B01_ApplianceAbstraction
{
    // Abstract base class - Appliance
    // Demonstrates abstraction concept
    // Abstract classes cannot be instantiated directly
    abstract class Appliance
    {
        public string Brand { get; set; }
        public string Model { get; set; }
        public int PowerConsumption { get; set; } // in Watts

        public Appliance(string brand, string model, int powerConsumption)
        {
            Brand = brand;
            Model = model;
            PowerConsumption = powerConsumption;
        }

        // Abstract method - must be implemented by derived classes
        public abstract void TurnOn();

        // Abstract method for turning off
        public abstract void TurnOff();

        // Concrete method - can be used by all derived classes
        public void DisplaySpecs()
        {
            Console.WriteLine($"Brand: {Brand}");
            Console.WriteLine($"Model: {Model}");
            Console.WriteLine($"Power Consumption: {PowerConsumption}W");
        }
    }

    // Derived class - Fan
    class Fan : Appliance
    {
        public int Speed { get; set; }
        public int NumberOfBlades { get; set; }

        public Fan(string brand, string model, int powerConsumption, int numberOfBlades)
            : base(brand, model, powerConsumption)
        {
            NumberOfBlades = numberOfBlades;
            Speed = 0; // Initially off
        }

        // Implement abstract method TurnOn
        public override void TurnOn()
        {
            Speed = 1;
            Console.WriteLine($"\n{Brand} {Model} Fan is now ON!");
            Console.WriteLine("Fan is spinning...");
            Console.WriteLine($"Number of Blades: {NumberOfBlades}");
            Console.WriteLine($"Current Speed Level: {Speed}");
        }

        // Implement abstract method TurnOff
        public override void TurnOff()
        {
            Speed = 0;
            Console.WriteLine($"\n{Brand} {Model} Fan is now OFF!");
        }

        // Additional method specific to Fan
        public void IncreaseSpeed()
        {
            if (Speed < 5)
            {
                Speed++;
                Console.WriteLine($"Fan speed increased to level {Speed}");
            }
            else
            {
                Console.WriteLine("Fan is already at maximum speed!");
            }
        }
    }

    // Derived class - Light
    class Light : Appliance
    {
        public string LightType { get; set; } // LED, CFL, Incandescent
        public int Brightness { get; set; }   // Percentage

        public Light(string brand, string model, int powerConsumption, string lightType)
            : base(brand, model, powerConsumption)
        {
            LightType = lightType;
            Brightness = 0; // Initially off
        }

        // Implement abstract method TurnOn
        public override void TurnOn()
        {
            Brightness = 100;
            Console.WriteLine($"\n{Brand} {Model} {LightType} Light is now ON!");
            Console.WriteLine("Light is glowing...");
            Console.WriteLine($"Brightness: {Brightness}%");
        }

        // Implement abstract method TurnOff
        public override void TurnOff()
        {
            Brightness = 0;
            Console.WriteLine($"\n{Brand} {Model} Light is now OFF!");
        }

        // Additional method specific to Light
        public void DimLight(int percentage)
        {
            if (percentage >= 0 && percentage <= 100)
            {
                Brightness = percentage;
                Console.WriteLine($"Light dimmed to {Brightness}%");
            }
            else
            {
                Console.WriteLine("Invalid brightness level! Use 0-100.");
            }
        }
    }

    // Additional derived class - AirConditioner
    class AirConditioner : Appliance
    {
        public int Temperature { get; set; } // in Celsius
        public string Mode { get; set; }

        public AirConditioner(string brand, string model, int powerConsumption)
            : base(brand, model, powerConsumption)
        {
            Temperature = 24; // Default temperature
            Mode = "Cool";
        }

        // Implement abstract method TurnOn
        public override void TurnOn()
        {
            Console.WriteLine($"\n{Brand} {Model} AC is now ON!");
            Console.WriteLine($"Mode: {Mode}");
            Console.WriteLine($"Temperature set to: {Temperature}°C");
            Console.WriteLine("Cooling in progress...");
        }

        // Implement abstract method TurnOff
        public override void TurnOff()
        {
            Console.WriteLine($"\n{Brand} {Model} AC is now OFF!");
        }

        // Additional method specific to AC
        public void SetTemperature(int temp)
        {
            if (temp >= 16 && temp <= 30)
            {
                Temperature = temp;
                Console.WriteLine($"Temperature adjusted to {Temperature}°C");
            }
            else
            {
                Console.WriteLine("Invalid temperature! Use 16-30°C.");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Display header
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║       Lab 05 - B01: Appliance Abstraction Demo             ║");
            Console.WriteLine("║      (Abstract Class with Abstract Methods)                ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Creating and Operating Appliances:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                // Create Fan objects
                Console.WriteLine("\n[APPLIANCE 1: FAN]");
                Console.WriteLine("┌─────────────────────────────────────────────────────────┐");
                Fan fan1 = new Fan("Havells", "Velocity Neo", 75, 3);
                fan1.DisplaySpecs();
                fan1.TurnOn();
                fan1.IncreaseSpeed();
                fan1.IncreaseSpeed();
                fan1.TurnOff();
                Console.WriteLine("└─────────────────────────────────────────────────────────┘");

                // Create Light objects
                Console.WriteLine("\n[APPLIANCE 2: LIGHT]");
                Console.WriteLine("┌─────────────────────────────────────────────────────────┐");
                Light light1 = new Light("Philips", "Hue Smart", 9, "LED");
                light1.DisplaySpecs();
                light1.TurnOn();
                light1.DimLight(50);
                light1.TurnOff();
                Console.WriteLine("└─────────────────────────────────────────────────────────┘");

                // Create AC objects
                Console.WriteLine("\n[APPLIANCE 3: AIR CONDITIONER]");
                Console.WriteLine("┌─────────────────────────────────────────────────────────┐");
                AirConditioner ac1 = new AirConditioner("Daikin", "Split AC 1.5 Ton", 1500);
                ac1.DisplaySpecs();
                ac1.TurnOn();
                ac1.SetTemperature(22);
                ac1.TurnOff();
                Console.WriteLine("└─────────────────────────────────────────────────────────┘");

                Console.WriteLine();

                // Demonstrate polymorphism with abstract class
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Polymorphism with Abstract Class:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine();

                Appliance[] appliances = { fan1, light1, ac1 };

                Console.WriteLine("Operating all appliances:");
                foreach (Appliance appliance in appliances)
                {
                    appliance.TurnOn();
                }

                Console.WriteLine("\n" + new string('─', 62));
                Console.WriteLine("Turning off all appliances:");
                foreach (Appliance appliance in appliances)
                {
                    appliance.TurnOff();
                }

                Console.WriteLine();

                // Explain abstraction
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Abstraction Concept Demonstration:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("✓ Appliance is an abstract class (cannot be instantiated)");
                Console.WriteLine("✓ TurnOn() and TurnOff() are abstract methods");
                Console.WriteLine("✓ Fan, Light, AC must implement abstract methods");
                Console.WriteLine("✓ Each appliance has its own implementation");
                Console.WriteLine("✓ Abstraction hides complex implementation details");
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
