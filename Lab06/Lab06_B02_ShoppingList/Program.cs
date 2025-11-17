using System;
using System.Collections.Generic;

namespace Lab06_B02_ShoppingList
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> shoppingList = new List<string>();

            Console.WriteLine("╔════════════════════════════════════════════════════╗");
            Console.WriteLine("║         SHOPPING LIST MANAGER (LIST)              ║");
            Console.WriteLine("╚════════════════════════════════════════════════════╝");
            Console.WriteLine();

            Console.WriteLine("--- Hardcoded Example ---");
            Console.WriteLine();

            Console.WriteLine("Adding initial items to shopping list:");
            AddItem(shoppingList, "Milk");
            AddItem(shoppingList, "Bread");
            AddItem(shoppingList, "Eggs");
            AddItem(shoppingList, "Butter");
            AddItem(shoppingList, "Cheese");
            AddItem(shoppingList, "Milk");

            Console.WriteLine();
            DisplayShoppingList(shoppingList);

            Console.WriteLine();
            Console.WriteLine("--- Removing Items ---");
            RemoveItem(shoppingList, "Bread");
            RemoveItem(shoppingList, "Sugar");

            Console.WriteLine();
            DisplayShoppingList(shoppingList);

            Console.WriteLine();
            Console.WriteLine("═══════════════════════════════════════════════════");
            Console.WriteLine("           INTERACTIVE MODE");
            Console.WriteLine("═══════════════════════════════════════════════════");

            bool continueRunning = true;
            while (continueRunning)
            {
                Console.WriteLine();
                Console.WriteLine("Menu:");
                Console.WriteLine("1. Add Item");
                Console.WriteLine("2. Remove Item");
                Console.WriteLine("3. Display Shopping List");
                Console.WriteLine("4. Search for Item");
                Console.WriteLine("5. Clear All Items");
                Console.WriteLine("6. Sort Shopping List");
                Console.WriteLine("7. Display Item Count");
                Console.WriteLine("8. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();
                Console.WriteLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Enter item name: ");
                        string itemToAdd = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(itemToAdd))
                            AddItem(shoppingList, itemToAdd);
                        else
                            Console.WriteLine("❌ Item name cannot be empty!");
                        break;

                    case "2":
                        Console.Write("Enter item name to remove: ");
                        string itemToRemove = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(itemToRemove))
                            RemoveItem(shoppingList, itemToRemove);
                        else
                            Console.WriteLine("❌ Item name cannot be empty!");
                        break;

                    case "3":
                        DisplayShoppingList(shoppingList);
                        break;

                    case "4":
                        Console.Write("Enter item name to search: ");
                        string itemToSearch = Console.ReadLine();
                        if (!string.IsNullOrWhiteSpace(itemToSearch))
                            SearchItem(shoppingList, itemToSearch);
                        else
                            Console.WriteLine("❌ Item name cannot be empty!");
                        break;

                    case "5":
                        shoppingList.Clear();
                        Console.WriteLine("✓ All items cleared from shopping list!");
                        break;

                    case "6":
                        shoppingList.Sort();
                        Console.WriteLine("✓ Shopping list sorted alphabetically!");
                        DisplayShoppingList(shoppingList);
                        break;

                    case "7":
                        Console.WriteLine($"📊 Total items in shopping list: {shoppingList.Count}");
                        break;

                    case "8":
                        continueRunning = false;
                        Console.WriteLine("Exiting Shopping List Manager. Goodbye!");
                        break;

                    default:
                        Console.WriteLine("❌ Invalid choice! Please try again.");
                        break;
                }
            }
        }

        static void AddItem(List<string> list, string item)
        {
            item = ToTitleCase(item);
            bool exists = false;
            foreach (string existingItem in list)
            {
                if (existingItem.Equals(item, StringComparison.OrdinalIgnoreCase))
                {
                    exists = true;
                    break;
                }
            }

            if (!exists)
            {
                list.Add(item);
                Console.WriteLine($"✓ Item added: {item}");
            }
            else
            {
                Console.WriteLine($"❌ Item '{item}' already exists! Duplicates not allowed.");
            }
        }

        static void RemoveItem(List<string> list, string item)
        {
            string itemToRemove = null;
            foreach (string existingItem in list)
            {
                if (existingItem.Equals(item, StringComparison.OrdinalIgnoreCase))
                {
                    itemToRemove = existingItem;
                    break;
                }
            }

            if (itemToRemove != null)
            {
                list.Remove(itemToRemove);
                Console.WriteLine($"✓ Item removed: {itemToRemove}");
            }
            else
            {
                Console.WriteLine($"❌ Item '{item}' not found in the list!");
            }
        }

        static void DisplayShoppingList(List<string> list)
        {
            if (list.Count > 0)
            {
                Console.WriteLine($"🛒 Shopping List (Total: {list.Count} items):");
                Console.WriteLine("┌────┬─────────────────────────────────────────┐");
                Console.WriteLine("│ No │ Item Name                               │");
                Console.WriteLine("├────┼─────────────────────────────────────────┤");

                for (int i = 0; i < list.Count; i++)
                {
                    Console.WriteLine($"│ {i + 1,-2} │ {list[i],-39} │");
                }

                Console.WriteLine("└────┴─────────────────────────────────────────┘");
            }
            else
            {
                Console.WriteLine("❌ Shopping list is empty!");
            }
        }

        static void SearchItem(List<string> list, string item)
        {
            bool found = false;
            int position = -1;

            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(item, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    position = i + 1;
                    break;
                }
            }

            if (found)
                Console.WriteLine($"✓ Item '{item}' found at position {position}!");
            else
                Console.WriteLine($"❌ Item '{item}' not found in the list!");
        }

        static string ToTitleCase(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return input;

            input = input.Trim();
            if (input.Length == 0)
                return input;

            return char.ToUpper(input[0]) + input.Substring(1).ToLower();
        }
    }
}
