using System;

namespace Lab05_A01_AnimalInheritance
{
    // Base class - Animal
    // Demonstrates single inheritance concept
    class Animal
    {
        // Property to store animal name
        public string Name { get; set; }

        // Constructor
        public Animal(string name)
        {
            Name = name;
        }

        // Method available to all animals
        public void Eat()
        {
            Console.WriteLine($"{Name} is eating.");
        }
    }

    // Derived class - Dog inherits from Animal
    // This demonstrates single inheritance where Dog inherits from Animal
    class Dog : Animal
    {
        // Property specific to Dog
        public string Breed { get; set; }

        // Constructor - calls base class constructor using 'base' keyword
        public Dog(string name, string breed) : base(name)
        {
            Breed = breed;
        }

        // Method specific to Dog class
        public void Bark()
        {
            Console.WriteLine($"{Name} ({Breed}) is barking: Woof! Woof!");
        }

        // Additional method for Dog
        public void DisplayInfo()
        {
            Console.WriteLine($"Dog Name: {Name}");
            Console.WriteLine($"Breed: {Breed}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Display header
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         Lab 05 - A01: Animal Inheritance Demo              ║");
            Console.WriteLine("║         (Single Inheritance: Animal → Dog)                 ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                // Create an instance of Animal class
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Creating Animal object:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Animal genericAnimal = new Animal("Generic Animal");
                Console.WriteLine($"Animal Name: {genericAnimal.Name}");
                genericAnimal.Eat(); // Call method from Animal class
                Console.WriteLine();

                // Create instances of Dog class
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Creating Dog objects:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                // Dog 1
                Dog dog1 = new Dog("Buddy", "Golden Retriever");
                dog1.DisplayInfo();
                dog1.Eat();  // Inherited method from Animal class
                dog1.Bark(); // Method specific to Dog class
                Console.WriteLine();

                // Dog 2
                Dog dog2 = new Dog("Max", "German Shepherd");
                dog2.DisplayInfo();
                dog2.Eat();  // Inherited method from Animal class
                dog2.Bark(); // Method specific to Dog class
                Console.WriteLine();

                // Dog 3
                Dog dog3 = new Dog("Charlie", "Labrador");
                dog3.DisplayInfo();
                dog3.Eat();  // Inherited method from Animal class
                dog3.Bark(); // Method specific to Dog class
                Console.WriteLine();

                // Demonstrating inheritance concept
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Inheritance Concept Demonstration:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("✓ Dog class inherits from Animal class (Single Inheritance)");
                Console.WriteLine("✓ Dog has access to Animal's Eat() method");
                Console.WriteLine("✓ Dog has its own Bark() method");
                Console.WriteLine("✓ Dog can use properties from both classes");
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
