/*
 * Lab04_B01_AnimalSound
 * Demonstrates method overriding using virtual and override keywords
 * Animal base class with virtual Sound() method, Dog and Cat derived classes
 */

using System;

namespace Lab04_B01_AnimalSound
{
    // Base class Animal with virtual method
    class Animal
    {
        protected string name;
        protected string species;

        public Animal(string name, string species)
        {
            this.name = name;
            this.species = species;
        }

        // Virtual method - can be overridden by derived classes
        public virtual void Sound()
        {
            Console.WriteLine($"{name} makes a generic animal sound.");
        }

        public virtual void DisplayInfo()
        {
            Console.WriteLine($"Animal: {name} ({species})");
        }
    }

    // Derived class Dog - overrides Sound() method
    class Dog : Animal
    {
        private string breed;

        public Dog(string name, string breed) : base(name, "Dog")
        {
            this.breed = breed;
        }

        // Override the virtual Sound() method
        public override void Sound()
        {
            Console.WriteLine($"{name} barks: Woof! Woof! Woof!");
        }

        // Override DisplayInfo with additional information
        public override void DisplayInfo()
        {
            Console.WriteLine($"Dog: {name} | Breed: {breed}");
        }

        public void Fetch()
        {
            Console.WriteLine($"{name} is fetching the ball!");
        }
    }

    // Derived class Cat - overrides Sound() method
    class Cat : Animal
    {
        private string color;

        public Cat(string name, string color) : base(name, "Cat")
        {
            this.color = color;
        }

        // Override the virtual Sound() method
        public override void Sound()
        {
            Console.WriteLine($"{name} meows: Meow! Meow! Meow!");
        }

        // Override DisplayInfo with additional information
        public override void DisplayInfo()
        {
            Console.WriteLine($"Cat: {name} | Color: {color}");
        }

        public void Scratch()
        {
            Console.WriteLine($"{name} is scratching the post!");
        }
    }

    // Additional derived class: Cow
    class Cow : Animal
    {
        private int age;

        public Cow(string name, int age) : base(name, "Cow")
        {
            this.age = age;
        }

        public override void Sound()
        {
            Console.WriteLine($"{name} moos: Moo! Moo! Moo!");
        }

        public override void DisplayInfo()
        {
            Console.WriteLine($"Cow: {name} | Age: {age} years");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("╔════════════════════════════════════════════════╗");
            Console.WriteLine("║   LAB 04 - B01: ANIMAL SOUND (OVERRIDING)      ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");

            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("   METHOD OVERRIDING DEMONSTRATION");
            Console.WriteLine(new string('═', 50));

            // Create instances of different animals
            Dog dog1 = new Dog("Bruno", "German Shepherd");
            Cat cat1 = new Cat("Whiskers", "Orange");
            Cow cow1 = new Cow("Bessie", 5);

            // Test 1: Calling overridden methods on specific objects
            Console.WriteLine("\n[Test 1] Direct Object References");
            Console.WriteLine(new string('-', 50));

            Console.WriteLine("\nDog Information:");
            dog1.DisplayInfo();
            dog1.Sound();
            dog1.Fetch();

            Console.WriteLine("\nCat Information:");
            cat1.DisplayInfo();
            cat1.Sound();
            cat1.Scratch();

            Console.WriteLine("\nCow Information:");
            cow1.DisplayInfo();
            cow1.Sound();

            // Test 2: Runtime Polymorphism - Base class reference
            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("[Test 2] Runtime Polymorphism");
            Console.WriteLine("(Base class reference pointing to derived objects)");
            Console.WriteLine(new string('-', 50));

            // Base class reference pointing to derived objects
            Animal animal1 = new Dog("Max", "Labrador");
            Animal animal2 = new Cat("Luna", "Black");
            Animal animal3 = new Cow("Daisy", 3);

            Console.WriteLine("\nAnimal Reference 1 (actually a Dog):");
            animal1.DisplayInfo();
            animal1.Sound(); // Calls Dog's Sound()

            Console.WriteLine("\nAnimal Reference 2 (actually a Cat):");
            animal2.DisplayInfo();
            animal2.Sound(); // Calls Cat's Sound()

            Console.WriteLine("\nAnimal Reference 3 (actually a Cow):");
            animal3.DisplayInfo();
            animal3.Sound(); // Calls Cow's Sound()

            // Test 3: Array of Animals (Polymorphic behavior)
            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("[Test 3] Polymorphic Array");
            Console.WriteLine(new string('-', 50));

            Animal[] animals = new Animal[5];
            animals[0] = new Dog("Rocky", "Bulldog");
            animals[1] = new Cat("Mittens", "White");
            animals[2] = new Dog("Charlie", "Beagle");
            animals[3] = new Cow("Molly", 4);
            animals[4] = new Cat("Shadow", "Gray");

            Console.WriteLine("\nAnimal sounds in the farm:");
            for (int i = 0; i < animals.Length; i++)
            {
                Console.WriteLine($"\n{i + 1}. ");
                animals[i].DisplayInfo();
                animals[i].Sound();
            }

            // Test 4: Method selection at runtime
            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("[Test 4] Dynamic Method Selection");
            Console.WriteLine(new string('-', 50));

            Console.WriteLine("\nWhich animal do you want to hear?");
            Console.WriteLine("1. Dog");
            Console.WriteLine("2. Cat");
            Console.WriteLine("3. Cow");
            Console.Write("\nDemonstration with option 1 (Dog): ");

            Animal selectedAnimal = new Dog("Buddy", "Golden Retriever");
            Console.WriteLine();
            selectedAnimal.DisplayInfo();
            selectedAnimal.Sound();

            // Key Learning Points
            Console.WriteLine("\n╔════════════════════════════════════════════════╗");
            Console.WriteLine("║         KEY LEARNING POINTS                    ║");
            Console.WriteLine("╚════════════════════════════════════════════════╝");
            Console.WriteLine("\n✓ VIRTUAL keyword: Marks base class method as");
            Console.WriteLine("  overridable");
            Console.WriteLine("\n✓ OVERRIDE keyword: Replaces base class method");
            Console.WriteLine("  implementation in derived class");
            Console.WriteLine("\n✓ Runtime Polymorphism: Method called is determined");
            Console.WriteLine("  at runtime based on actual object type");
            Console.WriteLine("\n✓ Base class reference can point to derived class");
            Console.WriteLine("  objects (Animal ref = new Dog())");
            Console.WriteLine("\n✓ Enables writing flexible code that works with");
            Console.WriteLine("  base class but executes derived class behavior");

            Console.WriteLine("\n" + new string('═', 50));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
