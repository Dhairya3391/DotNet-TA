/*
 * Lab03_C01_ShoppingCart
 * Problem: Create a ShoppingCart class with items (name, price, quantity).
 *          Calculate total price and throw exception if quantity <= 0.
 *
 * Learning Objectives:
 * - Understanding composition (Item class within ShoppingCart)
 * - Collection management (List of items)
 * - Custom exception handling for business rules
 * - Complex class interactions
 */

using System;
using System.Collections.Generic;

namespace Lab03_C01_ShoppingCart
{
    // Custom exception for invalid quantity
    class InvalidQuantityException : Exception
    {
        public InvalidQuantityException(string message) : base(message) { }
    }

    // Item class representing a product in the cart
    class Item
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }

        // Constructor with validation
        public Item(string name, double price, int quantity)
        {
            if (quantity <= 0)
            {
                throw new InvalidQuantityException(
                    $"Invalid quantity for item '{name}'. Quantity must be greater than 0.");
            }

            if (price < 0)
            {
                throw new ArgumentException($"Invalid price for item '{name}'. Price cannot be negative.");
            }

            Name = name;
            Price = price;
            Quantity = quantity;
        }

        // Calculate total price for this item
        public double GetTotalPrice()
        {
            return Price * Quantity;
        }

        // Display item details
        public void DisplayItem()
        {
            Console.WriteLine($"{Name,-20} | Rs. {Price,8:F2} | Qty: {Quantity,3} | Total: Rs. {GetTotalPrice(),10:F2}");
        }
    }

    // ShoppingCart class
    class ShoppingCart
    {
        private List<Item> items;
        public string CustomerName { get; set; }

        // Constructor
        public ShoppingCart(string customerName)
        {
            CustomerName = customerName;
            items = new List<Item>();
        }

        // Add item to cart
        public void AddItem(string name, double price, int quantity)
        {
            try
            {
                Item item = new Item(name, price, quantity);
                items.Add(item);
                Console.WriteLine($"Added: {name} x {quantity} to cart.");
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
                throw; // Re-throw to be handled by caller
            }
        }

        // Calculate total cart value
        public double CalculateTotal()
        {
            double total = 0;
            foreach (Item item in items)
            {
                total += item.GetTotalPrice();
            }
            return total;
        }

        // Display cart contents
        public void DisplayCart()
        {
            Console.WriteLine($"\n*** Shopping Cart for {CustomerName} ***");
            Console.WriteLine(new string('=', 70));

            if (items.Count == 0)
            {
                Console.WriteLine("Cart is empty!");
                return;
            }

            Console.WriteLine($"{"Item Name",-20} | {"Price",8} | {"Qty",7} | {"Total",14}");
            Console.WriteLine(new string('-', 70));

            foreach (Item item in items)
            {
                item.DisplayItem();
            }

            Console.WriteLine(new string('-', 70));
            Console.WriteLine($"{"Total Items:",-20}   {items.Count}");
            Console.WriteLine($"{"Grand Total:",-20}   Rs. {CalculateTotal(),10:F2}");
            Console.WriteLine(new string('=', 70));
        }

        // Get number of items in cart
        public int GetItemCount()
        {
            return items.Count;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader("Shopping Cart System");

            try
            {
                // Create shopping cart
                ShoppingCart cart = new ShoppingCart("Priya Sharma");

                Console.WriteLine("\n*** Adding Items to Cart ***\n");

                // Add valid items
                cart.AddItem("Laptop", 45000.00, 1);
                cart.AddItem("Wireless Mouse", 500.00, 2);
                cart.AddItem("USB Cable", 150.00, 3);
                cart.AddItem("Keyboard", 1200.00, 1);
                cart.AddItem("Monitor", 12000.00, 2);

                // Display cart
                cart.DisplayCart();

                // Demonstrate exception: Try to add item with invalid quantity
                Console.WriteLine("\n*** Attempting to Add Item with Invalid Quantity ***");
                cart.AddItem("Invalid Item", 100.00, 0); // This will throw exception
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine($"\n*** OPERATION FAILED ***");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Item was not added to cart.");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"\n*** INVALID OPERATION ***");
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n*** ERROR ***");
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Demonstrate another cart with negative quantity exception
            Console.WriteLine("\n\n*** Testing Another Scenario ***");
            try
            {
                ShoppingCart cart2 = new ShoppingCart("Rahul Patel");
                cart2.AddItem("Product A", 100.00, 5);
                cart2.AddItem("Product B", 200.00, -1); // Negative quantity
            }
            catch (InvalidQuantityException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            PrintFooter();
        }

        // Helper method to print header
        static void PrintHeader(string title)
        {
            Console.WriteLine(new string('=', 70));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('=', 70));
        }

        // Helper method to print footer
        static void PrintFooter()
        {
            Console.WriteLine(new string('=', 70));
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
