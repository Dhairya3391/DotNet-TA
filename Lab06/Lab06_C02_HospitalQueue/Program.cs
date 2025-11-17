using System;
using System.Collections.Generic;

namespace Lab06_C02_HospitalQueue
{
    /// <summary>
    /// Program to manage hospital patient queues
    /// Uses two Queue<string> objects:
    /// - Normal Queue: Regular patients (FIFO)
    /// - Emergency Queue: Emergency patients (served first)
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            Queue<string> normalQueue = new Queue<string>();
            Queue<string> emergencyQueue = new Queue<string>();

            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║       HOSPITAL PATIENT QUEUE SYSTEM               ║");
            Console.WriteLine("║      (Emergency Priority + Normal Queue)          ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine("--- Hardcoded Example ---");
            Console.WriteLine();

            Console.WriteLine("Adding patients to queues:");
            AddNormalPatient(normalQueue, "John Smith");
            AddNormalPatient(normalQueue, "Sarah Johnson");
            AddEmergencyPatient(emergencyQueue, "Michael Brown (Heart Attack)");
            AddNormalPatient(normalQueue, "Emily Davis");
            AddEmergencyPatient(emergencyQueue, "David Wilson (Severe Injury)");
            AddNormalPatient(normalQueue, "Robert Garcia");
            AddEmergencyPatient(emergencyQueue, "Jennifer Martinez (Critical)");
            AddNormalPatient(normalQueue, "William Anderson");

            Console.WriteLine();
            DisplayAllQueues(normalQueue, emergencyQueue);

            Console.WriteLine();
            Console.WriteLine("--- Serving Patients (Emergency Priority) ---");
            Console.WriteLine();

            for (int i = 0; i < 6; i++)
            {
                ServeNextPatient(normalQueue, emergencyQueue);
            }

            Console.WriteLine();
            DisplayAllQueues(normalQueue, emergencyQueue);

            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine("           INTERACTIVE MODE");
            Console.WriteLine("═══════════════════════════════════════════════════");

            bool continueRunning = true;
            while (continueRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Add Normal Patient");
                Console.WriteLine("2. Add Emergency Patient");
                Console.WriteLine("3. Serve Next Patient (Priority Based)");
                Console.WriteLine("4. View Next Patient to be Served");
                Console.WriteLine("5. Display All Queues");
                Console.WriteLine("6. Display Queue Statistics");
                Console.WriteLine("7. Clear All Queues");
                Console.WriteLine("8. Simulate Busy Hospital");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter patient name: ");
                        string normalPatient = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(normalPatient))
                        {
                            AddNormalPatient(normalQueue, normalPatient.Trim());
                        }
                        else
                        {
                            Console.WriteLine("❌ Patient name cannot be empty!");
                        }
                        break;

                    case "2":
                        Console.Write("Enter patient name (with condition): ");
                        string emergencyPatient = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(emergencyPatient))
                        {
                            AddEmergencyPatient(emergencyQueue, emergencyPatient.Trim());
                        }
                        else
                        {
                            Console.WriteLine("❌ Patient name cannot be empty!");
                        }
                        break;

                    case "3":
                        ServeNextPatient(normalQueue, emergencyQueue);
                        break;

                    case "4":
                        ViewNextPatient(normalQueue, emergencyQueue);
                        break;

                    case "5":
                        DisplayAllQueues(normalQueue, emergencyQueue);
                        break;

                    case "6":
                        DisplayStatistics(normalQueue, emergencyQueue);
                        break;

                    case "7":
                        normalQueue.Clear();
                        emergencyQueue.Clear();
                        Console.WriteLine("✓ All queues cleared!");
                        break;

                    case "8":
                        SimulateBusyHospital(normalQueue, emergencyQueue);
                        break;

                    case "9":
                        continueRunning = false;
                        Console.WriteLine("Closing Hospital Queue System. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("❌ Invalid choice! Please try again.");
                        break;
                }
            }
        }

        static void AddNormalPatient(Queue<string> normalQueue, string patientName)
        {
            normalQueue.Enqueue(patientName);
            Console.WriteLine($"✓ Normal patient added: {patientName}");
        }

        static void AddEmergencyPatient(Queue<string> emergencyQueue, string patientName)
        {
            emergencyQueue.Enqueue(patientName);
            Console.WriteLine($"🚨 EMERGENCY patient added: {patientName}");
        }

        static void ServeNextPatient(Queue<string> normalQueue, Queue<string> emergencyQueue)
        {
            if (emergencyQueue.Count > 0)
            {
                string patient = emergencyQueue.Dequeue();
                Console.WriteLine($"🚨 Now serving EMERGENCY patient: {patient}");
            }
            else if (normalQueue.Count > 0)
            {
                string patient = normalQueue.Dequeue();
                Console.WriteLine($"✓ Now serving normal patient: {patient}");
            }
            else
            {
                Console.WriteLine("❌ No patients in queue! Both queues are empty.");
            }
        }

        static void ViewNextPatient(Queue<string> normalQueue, Queue<string> emergencyQueue)
        {
            if (emergencyQueue.Count > 0)
            {
                string patient = emergencyQueue.Peek();
                Console.WriteLine($"👁️  Next patient (EMERGENCY): {patient}");
            }
            else if (normalQueue.Count > 0)
            {
                string patient = normalQueue.Peek();
                Console.WriteLine($"👁️  Next patient (Normal): {patient}");
            }
            else
            {
                Console.WriteLine("❌ No patients waiting! Both queues are empty.");
            }
        }

        static void DisplayAllQueues(Queue<string> normalQueue, Queue<string> emergencyQueue)
        {
            Console.WriteLine("🏥 Hospital Patient Queues:");
            Console.WriteLine("════════════════════════════════════════════════════");

            Console.WriteLine();
            Console.WriteLine($"🚨 EMERGENCY QUEUE (Priority - Served First): {emergencyQueue.Count} patients");

            if (emergencyQueue.Count > 0)
            {
                Console.WriteLine("┌─────┬──────────────────────────────────────────┐");
                Console.WriteLine("│ Pos │ Patient Name                             │");
                Console.WriteLine("├─────┼──────────────────────────────────────────┤");

                int position = 1;
                foreach (string patient in emergencyQueue)
                {
                    Console.WriteLine($"│ {position,-3} │ {patient,-40} │");
                    position++;
                }

                Console.WriteLine("└─────┴──────────────────────────────────────────┘");
            }
            else
            {
                Console.WriteLine("  No emergency patients waiting.");
            }

            Console.WriteLine();
            Console.WriteLine($"📋 NORMAL QUEUE: {normalQueue.Count} patients");

            if (normalQueue.Count > 0)
            {
                Console.WriteLine("┌─────┬──────────────────────────────────────────┐");
                Console.WriteLine("│ Pos │ Patient Name                             │");
                Console.WriteLine("├─────┼──────────────────────────────────────────┤");

                int position = 1;
                foreach (string patient in normalQueue)
                {
                    Console.WriteLine($"│ {position,-3} │ {patient,-40} │");
                    position++;
                }

                Console.WriteLine("└─────┴──────────────────────────────────────────┘");
            }
            else
            {
                Console.WriteLine("  No normal patients waiting.");
            }

            Console.WriteLine();
            Console.WriteLine("════════════════════════════════════════════════════");
        }

        static void DisplayStatistics(Queue<string> normalQueue, Queue<string> emergencyQueue)
        {
            int totalPatients = normalQueue.Count + emergencyQueue.Count;

            Console.WriteLine("📊 Hospital Queue Statistics:");
            Console.WriteLine("┌────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ Total Patients:         {totalPatients,-26}│");
            Console.WriteLine($"│ Emergency Patients:     {emergencyQueue.Count,-26}│");
            Console.WriteLine($"│ Normal Patients:        {normalQueue.Count,-26}│");
            Console.WriteLine("├────────────────────────────────────────────────────┤");

            if (totalPatients > 0)
            {
                double emergencyPercentage = (emergencyQueue.Count * 100.0) / totalPatients;
                double normalPercentage = (normalQueue.Count * 100.0) / totalPatients;

                Console.WriteLine($"│ Emergency Percentage:   {emergencyPercentage,-26:F2}%");
                Console.WriteLine($"│ Normal Percentage:      {normalPercentage,-26:F2}%");
            }

            Console.WriteLine("└────────────────────────────────────────────────────┘");

            Console.WriteLine();
            Console.WriteLine("Priority Logic:");
            Console.WriteLine("  • Emergency patients are ALWAYS served first");
            Console.WriteLine("  • Normal patients are served only when no emergency patients are waiting");
            Console.WriteLine("  • Both queues follow FIFO (First In First Out) order");
        }

        static void SimulateBusyHospital(Queue<string> normalQueue, Queue<string> emergencyQueue)
        {
            Console.WriteLine("🏥 Simulating busy hospital...");
            Console.WriteLine();

            string[] normalPatients = {
                "Alice Cooper - Routine Checkup",
                "Bob Dylan - Flu Symptoms",
                "Charlie Brown - Vaccination",
                "Diana Prince - Follow-up Visit",
                "Ethan Hunt - Minor Injury"
            };

            string[] emergencyPatients = {
                "Frank Castle - Gunshot Wound",
                "Grace Hopper - Cardiac Arrest",
                "Henry Ford - Severe Burns"
            };

            Random random = new Random();
            int normalCount = random.Next(3, 6);
            int emergencyCount = random.Next(1, 4);

            Console.WriteLine($"Adding {normalCount} normal patients and {emergencyCount} emergency patients...");
            Console.WriteLine();

            for (int i = 0; i < normalCount && i < normalPatients.Length; i++)
            {
                AddNormalPatient(normalQueue, normalPatients[i]);
            }

            for (int i = 0; i < emergencyCount && i < emergencyPatients.Length; i++)
            {
                AddEmergencyPatient(emergencyQueue, emergencyPatients[i]);
            }

            Console.WriteLine();
            Console.WriteLine("✓ Simulation complete!");
            DisplayAllQueues(normalQueue, emergencyQueue);
        }
    }
}
