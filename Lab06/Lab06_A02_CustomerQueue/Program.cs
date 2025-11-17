using System;
using System.Collections.Generic;

namespace Lab06_A02_CustomerQueue
{
    /// <summary>
    /// Program to demonstrate Queue<string> for Customer Service System
    /// Queue follows FIFO (First In First Out) principle
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Queue to store customers waiting for service - FIFO behavior
            Queue<string> customerQueue = new Queue<string>();

            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║       CUSTOMER SERVICE QUEUE (FIFO)               ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Demonstrate with hardcoded examples
            Console.WriteLine("--- Hardcoded Example ---");
            Console.WriteLine();

            // Enqueue customers
            Console.WriteLine("Adding customers to queue:");
            AddCustomer(customerQueue, "John Smith");
            AddCustomer(customerQueue, "Sarah Johnson");
            AddCustomer(customerQueue, "Michael Brown");
            AddCustomer(customerQueue, "Emily Davis");
            AddCustomer(customerQueue, "David Wilson");

            Console.WriteLine();
            DisplayAllCustomers(customerQueue);

            Console.WriteLine();
            Console.WriteLine("--- Serving Customers (Dequeue) ---");
            Console.WriteLine();

            // Serve customers (Dequeue - First In First Out)
            ServeCustomer(customerQueue);
            ServeCustomer(customerQueue);

            Console.WriteLine();
            DisplayAllCustomers(customerQueue);

            Console.WriteLine();
            Console.WriteLine("--- Display Next Customer (Peek) ---");
            DisplayNextCustomer(customerQueue);

            // Interactive menu
            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine("           INTERACTIVE MODE");
            Console.WriteLine("═══════════════════════════════════════════════════");

            bool continueRunning = true;
            while (continueRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Add Customer to Queue (Enqueue)");
                Console.WriteLine("2. Serve Next Customer (Dequeue)");
                Console.WriteLine("3. View Next Customer (Peek)");
                Console.WriteLine("4. Display All Waiting Customers");
                Console.WriteLine("5. Display Queue Count");
                Console.WriteLine("6. Clear Queue");
                Console.WriteLine("7. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter customer name: ");
                        string name = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            AddCustomer(customerQueue, name);
                        }
                        else
                        {
                            Console.WriteLine("❌ Customer name cannot be empty!");
                        }
                        break;

                    case "2":
                        ServeCustomer(customerQueue);
                        break;

                    case "3":
                        DisplayNextCustomer(customerQueue);
                        break;

                    case "4":
                        DisplayAllCustomers(customerQueue);
                        break;

                    case "5":
                        Console.WriteLine($"📊 Total customers in queue: {customerQueue.Count}");
                        break;

                    case "6":
                        customerQueue.Clear();
                        Console.WriteLine("✓ All customers cleared from queue!");
                        break;

                    case "7":
                        continueRunning = false;
                        Console.WriteLine("Closing Customer Service System. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("❌ Invalid choice! Please try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Add a customer to the queue (Enqueue operation)
        /// </summary>
        static void AddCustomer(Queue<string> queue, string customerName)
        {
            queue.Enqueue(customerName);
            Console.WriteLine($"✓ Customer added to queue: {customerName}");
        }

        /// <summary>
        /// Serve the next customer (Dequeue operation)
        /// </summary>
        static void ServeCustomer(Queue<string> queue)
        {
            if (queue.Count > 0)
            {
                string servedCustomer = queue.Dequeue();
                Console.WriteLine($"✓ Now serving: {servedCustomer}");
            }
            else
            {
                Console.WriteLine("❌ No customers in queue! Queue is empty.");
            }
        }

        /// <summary>
        /// Display the next customer without removing them (Peek operation)
        /// </summary>
        static void DisplayNextCustomer(Queue<string> queue)
        {
            if (queue.Count > 0)
            {
                string nextCustomer = queue.Peek();
                Console.WriteLine($"👁️  Next customer to be served: {nextCustomer}");
            }
            else
            {
                Console.WriteLine("❌ No customers waiting! Queue is empty.");
            }
        }

        /// <summary>
        /// Display all customers waiting in the queue
        /// </summary>
        static void DisplayAllCustomers(Queue<string> queue)
        {
            if (queue.Count > 0)
            {
                Console.WriteLine($"👥 Waiting Customers (Total: {queue.Count}):");
                Console.WriteLine("┌──────────┬─────────────────────────────────────┐");
                Console.WriteLine("│ Position │ Customer Name                       │");
                Console.WriteLine("├──────────┼─────────────────────────────────────┤");

                int position = 1;
                // Note: foreach iterates from front to back (first to last)
                foreach (string customer in queue)
                {
                    Console.WriteLine($"│ {position,-8} │ {customer,-35} │");
                    position++;
                }

                Console.WriteLine("└──────────┴─────────────────────────────────────┘");
            }
            else
            {
                Console.WriteLine("❌ No customers waiting! Queue is empty.");
            }
        }
    }
}
