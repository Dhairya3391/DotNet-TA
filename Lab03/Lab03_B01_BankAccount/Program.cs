/*
 * Lab03_B01_BankAccount
 * Problem: Create a BankAccount class with deposit and withdrawal methods.
 *          Prevent negative balance and implement exception handling.
 *
 * Learning Objectives:
 * - Understanding encapsulation and data validation
 * - Custom exception handling for business logic
 * - Method implementation with validation
 */

using System;

namespace Lab03_B01_BankAccount
{
    // Custom exception for insufficient funds
    class InsufficientFundsException : Exception
    {
        public InsufficientFundsException(string message) : base(message) { }
    }

    // BankAccount class with deposit and withdrawal functionality
    class BankAccount
    {
        // Private fields
        private string accountNumber;
        private string accountHolderName;
        private double balance;

        // Constructor
        public BankAccount(string accountNumber, string accountHolderName, double initialBalance)
        {
            this.accountNumber = accountNumber;
            this.accountHolderName = accountHolderName;

            if (initialBalance < 0)
            {
                throw new ArgumentException("Initial balance cannot be negative!");
            }
            this.balance = initialBalance;
        }

        // Method to deposit money
        public void Deposit(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Deposit amount must be positive!");
            }
            balance += amount;
            Console.WriteLine($"\nDeposit successful! Amount: Rs. {amount:F2}");
            DisplayBalance();
        }

        // Method to withdraw money
        public void Withdraw(double amount)
        {
            if (amount <= 0)
            {
                throw new ArgumentException("Withdrawal amount must be positive!");
            }

            if (amount > balance)
            {
                throw new InsufficientFundsException(
                    $"Insufficient funds! Available balance: Rs. {balance:F2}, Requested: Rs. {amount:F2}");
            }

            balance -= amount;
            Console.WriteLine($"\nWithdrawal successful! Amount: Rs. {amount:F2}");
            DisplayBalance();
        }

        // Method to display current balance
        public void DisplayBalance()
        {
            Console.WriteLine($"Current Balance: Rs. {balance:F2}");
        }

        // Method to display account details
        public void DisplayAccountDetails()
        {
            Console.WriteLine($"\nAccount Number : {accountNumber}");
            Console.WriteLine($"Account Holder : {accountHolderName}");
            Console.WriteLine($"Balance        : Rs. {balance:F2}");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader("Bank Account Management System");

            try
            {
                // Create a bank account
                Console.WriteLine("\n*** Creating New Account ***");
                BankAccount account = new BankAccount("ACC001", "Amit Kumar", 5000.00);
                account.DisplayAccountDetails();

                // Demonstrate deposit
                Console.WriteLine("\n*** Deposit Operation ***");
                Console.WriteLine(new string('-', 40));
                account.Deposit(2000.00);

                // Demonstrate successful withdrawal
                Console.WriteLine("\n*** Withdrawal Operation ***");
                Console.WriteLine(new string('-', 40));
                account.Withdraw(1500.00);

                // Display final account details
                Console.WriteLine("\n*** Final Account Details ***");
                Console.WriteLine(new string('-', 40));
                account.DisplayAccountDetails();

                // Demonstrate exception: withdrawal exceeding balance
                Console.WriteLine("\n*** Attempting Withdrawal Exceeding Balance ***");
                Console.WriteLine(new string('-', 40));
                account.Withdraw(10000.00); // This will throw exception
            }
            catch (InsufficientFundsException ex)
            {
                Console.WriteLine($"\n*** TRANSACTION FAILED ***");
                Console.WriteLine($"Error: {ex.Message}");
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
