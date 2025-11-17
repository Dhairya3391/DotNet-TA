using System;

namespace Lab05_B03_RobotInterface
{
    // Interface 1 - IMovable
    // Defines contract for objects that can move
    interface IMovable
    {
        void Move();
        void Stop();
    }

    // Interface 2 - ISound
    // Defines contract for objects that can make sound
    interface ISound
    {
        void MakeSound();
        void Mute();
    }

    // Class - Robot implements both IMovable and ISound
    // Demonstrates multiple interface implementation
    class Robot : IMovable, ISound
    {
        public string Name { get; set; }
        public string Model { get; set; }
        public bool IsMoving { get; private set; }
        public bool IsSoundEnabled { get; private set; }

        public Robot(string name, string model)
        {
            Name = name;
            Model = model;
            IsMoving = false;
            IsSoundEnabled = true;
        }

        // Implement Move() method from IMovable interface
        public void Move()
        {
            if (!IsMoving)
            {
                IsMoving = true;
                Console.WriteLine($"\n[{Name}] Robot is now MOVING!");
                Console.WriteLine("Status: Walking forward... Step-by-step motion activated.");
            }
            else
            {
                Console.WriteLine($"\n[{Name}] Robot is already moving!");
            }
        }

        // Implement Stop() method from IMovable interface
        public void Stop()
        {
            if (IsMoving)
            {
                IsMoving = false;
                Console.WriteLine($"\n[{Name}] Robot has STOPPED!");
                Console.WriteLine("Status: Motion halted. Standing still.");
            }
            else
            {
                Console.WriteLine($"\n[{Name}] Robot is not moving!");
            }
        }

        // Implement MakeSound() method from ISound interface
        public void MakeSound()
        {
            if (IsSoundEnabled)
            {
                Console.WriteLine($"\n[{Name}] Robot says: Beep Boop! I am {Name}, Model {Model}.");
                Console.WriteLine("Sound: Electronic beeping sounds...");
            }
            else
            {
                Console.WriteLine($"\n[{Name}] Robot is muted!");
            }
        }

        // Implement Mute() method from ISound interface
        public void Mute()
        {
            IsSoundEnabled = !IsSoundEnabled;
            string status = IsSoundEnabled ? "UNMUTED" : "MUTED";
            Console.WriteLine($"\n[{Name}] Sound {status}!");
        }

        // Additional method specific to Robot
        public void DisplayInfo()
        {
            Console.WriteLine("\n┌─────────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ Robot Name   : {Name,-41} │");
            Console.WriteLine($"│ Model        : {Model,-41} │");
            Console.WriteLine($"│ Moving       : {(IsMoving ? "Yes" : "No"),-41} │");
            Console.WriteLine($"│ Sound Enabled: {(IsSoundEnabled ? "Yes" : "No"),-41} │");
            Console.WriteLine("└─────────────────────────────────────────────────────────┘");
        }
    }

    // Additional class demonstrating multiple interfaces - Drone
    class Drone : IMovable, ISound
    {
        public string Model { get; set; }
        public bool IsFlying { get; private set; }
        public bool IsSoundOn { get; private set; }

        public Drone(string model)
        {
            Model = model;
            IsFlying = false;
            IsSoundOn = true;
        }

        public void Move()
        {
            IsFlying = true;
            Console.WriteLine($"\n[{Model}] Drone is taking off and flying!");
        }

        public void Stop()
        {
            IsFlying = false;
            Console.WriteLine($"\n[{Model}] Drone has landed!");
        }

        public void MakeSound()
        {
            if (IsSoundOn)
            {
                Console.WriteLine($"\n[{Model}] Drone sound: Whirrrrr... Propellers spinning!");
            }
        }

        public void Mute()
        {
            IsSoundOn = !IsSoundOn;
            Console.WriteLine($"\n[{Model}] Drone sound {(IsSoundOn ? "ON" : "OFF")}!");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Display header
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║      Lab 05 - B03: Robot Multiple Interfaces Demo         ║");
            Console.WriteLine("║        (IMovable + ISound Interfaces)                      ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Creating Robot Objects:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                // Create Robot 1
                Robot robot1 = new Robot("Alpha", "RX-2025");
                robot1.DisplayInfo();

                Console.WriteLine("\n--- Testing IMovable Interface ---");
                robot1.Move();
                robot1.Move(); // Try moving again
                robot1.Stop();
                robot1.Stop(); // Try stopping again

                Console.WriteLine("\n--- Testing ISound Interface ---");
                robot1.MakeSound();
                robot1.Mute();
                robot1.MakeSound(); // Try making sound when muted
                robot1.Mute(); // Unmute
                robot1.MakeSound();

                robot1.DisplayInfo();

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Creating Another Robot:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                // Create Robot 2
                Robot robot2 = new Robot("Beta", "RX-2026");
                robot2.DisplayInfo();
                robot2.Move();
                robot2.MakeSound();

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Creating Drone Object:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                Drone drone = new Drone("DJI Phantom");
                drone.Move();
                drone.MakeSound();
                drone.Stop();

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Polymorphism with Multiple Interfaces:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                // Using IMovable interface reference
                Console.WriteLine("\n[Using IMovable Reference]");
                IMovable[] movableObjects = { robot1, robot2, drone };
                foreach (IMovable obj in movableObjects)
                {
                    obj.Move();
                }

                Console.WriteLine("\n[Using ISound Reference]");
                // Using ISound interface reference
                ISound[] soundObjects = { robot1, robot2, drone };
                foreach (ISound obj in soundObjects)
                {
                    obj.MakeSound();
                }

                Console.WriteLine();

                // Explain multiple interface implementation
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Multiple Interface Implementation Demonstration:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("✓ IMovable interface defines Move() and Stop() methods");
                Console.WriteLine("✓ ISound interface defines MakeSound() and Mute() methods");
                Console.WriteLine("✓ Robot class implements BOTH interfaces");
                Console.WriteLine("✓ Drone class also implements BOTH interfaces");
                Console.WriteLine("✓ C# supports multiple interface implementation");
                Console.WriteLine("✓ Each class provides its own implementation");
                Console.WriteLine("✓ Interface references enable polymorphic behavior");
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
