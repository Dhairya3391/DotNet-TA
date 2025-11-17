/*
 * Lab03_B02_ConstructorOverloading
 * Problem: Create a Person class with multiple constructors (overloading).
 *          Display values for objects created with different constructors.
 *
 * Learning Objectives:
 * - Understanding constructor overloading
 * - Using 'this' keyword for constructor chaining
 * - Default vs parameterized constructors
 */

using System;

namespace Lab03_B02_ConstructorOverloading
{
    // Person class with overloaded constructors
    class Person
    {
        // Properties
        public string Name { get; set; }
        public int Age { get; set; }
        public string City { get; set; }
        public string Occupation { get; set; }

        // Constructor 1: Default constructor
        public Person()
        {
            Name = "Unknown";
            Age = 0;
            City = "Unknown";
            Occupation = "Not specified";
            Console.WriteLine("Default constructor called.");
        }

        // Constructor 2: Constructor with name parameter
        public Person(string name) : this()
        {
            Name = name;
            Console.WriteLine("Constructor with name parameter called.");
        }

        // Constructor 3: Constructor with name and age parameters
        public Person(string name, int age) : this(name)
        {
            Age = age;
            Console.WriteLine("Constructor with name and age parameters called.");
        }

        // Constructor 4: Constructor with name, age, and city parameters
        public Person(string name, int age, string city) : this(name, age)
        {
            City = city;
            Console.WriteLine("Constructor with name, age, and city parameters called.");
        }

        // Constructor 5: Constructor with all parameters
        public Person(string name, int age, string city, string occupation) : this(name, age, city)
        {
            Occupation = occupation;
            Console.WriteLine("Constructor with all parameters called.");
        }

        // Method to display person details
        public void DisplayDetails()
        {
            Console.WriteLine(new string('-', 40));
            Console.WriteLine($"Name       : {Name}");
            Console.WriteLine($"Age        : {Age} years");
            Console.WriteLine($"City       : {City}");
            Console.WriteLine($"Occupation : {Occupation}");
            Console.WriteLine(new string('-', 40));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader("Constructor Overloading Demonstration");

            // Person 1: Using default constructor
            Console.WriteLine("\n*** Person 1: Default Constructor ***");
            Person person1 = new Person();
            person1.DisplayDetails();

            // Person 2: Using constructor with name
            Console.WriteLine("\n*** Person 2: Constructor with Name ***");
            Person person2 = new Person("Ravi Sharma");
            person2.DisplayDetails();

            // Person 3: Using constructor with name and age
            Console.WriteLine("\n*** Person 3: Constructor with Name and Age ***");
            Person person3 = new Person("Priya Mehta", 25);
            person3.DisplayDetails();

            // Person 4: Using constructor with name, age, and city
            Console.WriteLine("\n*** Person 4: Constructor with Name, Age, and City ***");
            Person person4 = new Person("Vikram Singh", 30, "Mumbai");
            person4.DisplayDetails();

            // Person 5: Using constructor with all parameters
            Console.WriteLine("\n*** Person 5: Constructor with All Parameters ***");
            Person person5 = new Person("Sneha Patel", 28, "Ahmedabad", "Software Engineer");
            person5.DisplayDetails();

            Console.WriteLine("\n*** Summary ***");
            Console.WriteLine("Demonstrated 5 different constructor overloads:");
            Console.WriteLine("1. Default constructor (no parameters)");
            Console.WriteLine("2. Constructor with 1 parameter (name)");
            Console.WriteLine("3. Constructor with 2 parameters (name, age)");
            Console.WriteLine("4. Constructor with 3 parameters (name, age, city)");
            Console.WriteLine("5. Constructor with 4 parameters (all details)");

            PrintFooter();
        }

        // Helper method to print header
        static void PrintHeader(string title)
        {
            Console.WriteLine(new string('=', 50));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('=', 50));
        }

        // Helper method to print footer
        static void PrintFooter()
        {
            Console.WriteLine(new string('=', 50));
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
