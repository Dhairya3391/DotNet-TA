using System;
using System.Collections.Generic;

namespace Lab06_A01_TaskStack
{
    /// <summary>
    /// Program to demonstrate Stack<string> for Recent Tasks Tracker
    /// Stack follows LIFO (Last In First Out) principle
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            // Stack to store recent tasks - LIFO behavior
            Stack<string> taskStack = new Stack<string>();

            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║         RECENT TASKS TRACKER (STACK)              ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            // Demonstrate with hardcoded examples
            Console.WriteLine("--- Hardcoded Example ---");
            Console.WriteLine();

            // Push tasks onto stack
            Console.WriteLine("Adding tasks to stack:");
            PushTask(taskStack, "Complete Lab Assignment");
            PushTask(taskStack, "Review Code");
            PushTask(taskStack, "Write Documentation");
            PushTask(taskStack, "Test Application");
            PushTask(taskStack, "Deploy to Server");

            Console.WriteLine();
            DisplayAllTasks(taskStack);

            Console.WriteLine();
            Console.WriteLine("--- Undo Operations (Pop) ---");
            Console.WriteLine();

            // Pop tasks (Undo functionality)
            UndoTask(taskStack);
            UndoTask(taskStack);

            Console.WriteLine();
            DisplayAllTasks(taskStack);

            Console.WriteLine();
            Console.WriteLine("--- Display Top Task (Peek) ---");
            DisplayTopTask(taskStack);

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
                Console.WriteLine("1. Add Task (Push)");
                Console.WriteLine("2. Undo Last Task (Pop)");
                Console.WriteLine("3. View Top Task (Peek)");
                Console.WriteLine("4. Display All Tasks");
                Console.WriteLine("5. Clear All Tasks");
                Console.WriteLine("6. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter task description: ");
                        string task = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(task))
                        {
                            PushTask(taskStack, task);
                        }
                        else
                        {
                            Console.WriteLine("❌ Task cannot be empty!");
                        }
                        break;

                    case "2":
                        UndoTask(taskStack);
                        break;

                    case "3":
                        DisplayTopTask(taskStack);
                        break;

                    case "4":
                        DisplayAllTasks(taskStack);
                        break;

                    case "5":
                        taskStack.Clear();
                        Console.WriteLine("✓ All tasks cleared!");
                        break;

                    case "6":
                        continueRunning = false;
                        Console.WriteLine("Exiting Task Tracker. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("❌ Invalid choice! Please try again.");
                        break;
                }
            }
        }

        /// <summary>
        /// Push a task onto the stack
        /// </summary>
        static void PushTask(Stack<string> stack, string task)
        {
            stack.Push(task);
            Console.WriteLine($"✓ Task added: {task}");
        }

        /// <summary>
        /// Pop a task from the stack (Undo operation)
        /// </summary>
        static void UndoTask(Stack<string> stack)
        {
            if (stack.Count > 0)
            {
                string undoneTask = stack.Pop();
                Console.WriteLine($"⟲ Undone task: {undoneTask}");
            }
            else
            {
                Console.WriteLine("❌ No tasks to undo! Stack is empty.");
            }
        }

        /// <summary>
        /// Display the top task without removing it (Peek operation)
        /// </summary>
        static void DisplayTopTask(Stack<string> stack)
        {
            if (stack.Count > 0)
            {
                string topTask = stack.Peek();
                Console.WriteLine($"👁️  Top Task: {topTask}");
            }
            else
            {
                Console.WriteLine("❌ No tasks available! Stack is empty.");
            }
        }

        /// <summary>
        /// Display all tasks in the stack
        /// </summary>
        static void DisplayAllTasks(Stack<string> stack)
        {
            if (stack.Count > 0)
            {
                Console.WriteLine($"📋 All Tasks (Total: {stack.Count}):");
                Console.WriteLine("┌────┬─────────────────────────────────────────┐");
                Console.WriteLine("│ No │ Task Description                    │");
                Console.WriteLine("├────┼─────────────────────────────────────────┤");

                int index = 1;
                // Note: foreach iterates from top to bottom (most recent to oldest)
                foreach (string task in stack)
                {
                    Console.WriteLine($"│ {index,-2} │ {task,-39} │");
                    index++;
                }

                Console.WriteLine("└────┴─────────────────────────────────────────┘");
            }
            else
            {
                Console.WriteLine("❌ No tasks available! Stack is empty.");
            }
        }
    }
}
