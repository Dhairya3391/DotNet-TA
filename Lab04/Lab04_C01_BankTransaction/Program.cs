/*
 * Lab04_C01_BankTransaction
 * Demonstrates method overloading with private fields and public methods
 * BankTransaction class with overloaded Transfer() methods
 */

using System;
using System.Collections.Generic;

namespace Lab04_C01_BankTransaction
{
    // BankAccount class with encapsulation
    class BankAccount
    {
        // Private fields - encapsulation
        private string accountNumber;
        private string accountHolder;
        private double balance;

        // Constructor
        public BankAccount(string accountNumber, string accountHolder, double initialBalance)
        {
            this.accountNumber = accountNumber;
            this.accountHolder = accountHolder;
            this.balance = initialBalance;
        }

        // Public properties for controlled access
        public string AccountNumber { get { return accountNumber; } }
        public string AccountHolder { get { return accountHolder; } }
        public double Balance { get { return balance; } }

        // Method to deposit money
        public void Deposit(double amount)
        {
            if (amount > 0)
            {
                balance += amount;
            }
        }

        // Method to withdraw money
        public bool Withdraw(double amount)
        {
            if (amount > 0 && balance >= amount)
            {
                balance -= amount;
                return true;
            }
            return false;
        }
    }

    // Transaction class with overloaded methods
    class BankTransaction
    {
        // Private fields
        private List<string> transactionHistory;
        private static int transactionCounter = 1000;

        // Constructor
        public BankTransaction()
        {
            transactionHistory = new List<string>();
        }

        // Overloaded method 1: Transfer with amount only (no description)
        public bool Transfer(BankAccount fromAccount, BankAccount toAccount, double amount)
        {
            return Transfer(fromAccount, toAccount, amount, "Regular Transfer");
        }

        // Overloaded method 2: Transfer with amount and description
        public bool Transfer(BankAccount fromAccount, BankAccount toAccount, double amount, string description)
        {
            // Validate transfer
            if (amount <= 0)
            {
                RecordTransaction(fromAccount.AccountNumber, toAccount.AccountNumber, amount,
                                description, false, "Invalid amount");
                return false;
            }

            if (fromAccount.Balance < amount)
            {
                RecordTransaction(fromAccount.AccountNumber, toAccount.AccountNumber, amount,
                                description, false, "Insufficient balance");
                return false;
            }

            // Perform transfer
            if (fromAccount.Withdraw(amount))
            {
                toAccount.Deposit(amount);
                RecordTransaction(fromAccount.AccountNumber, toAccount.AccountNumber, amount,
                                description, true, "Success");
                return true;
            }

            return false;
        }

        // Overloaded method 3: Transfer with amount, description, and transaction fee
        public bool Transfer(BankAccount fromAccount, BankAccount toAccount, double amount,
                           string description, double transactionFee)
        {
            double totalAmount = amount + transactionFee;

            if (totalAmount <= 0)
            {
                RecordTransaction(fromAccount.AccountNumber, toAccount.AccountNumber, amount,
                                description + " (with fee)", false, "Invalid amount");
                return false;
            }

            if (fromAccount.Balance < totalAmount)
            {
                RecordTransaction(fromAccount.AccountNumber, toAccount.AccountNumber, amount,
                                description + " (with fee)", false, "Insufficient balance");
                return false;
            }

            // Perform transfer with fee
            if (fromAccount.Withdraw(totalAmount))
            {
                toAccount.Deposit(amount);
                RecordTransaction(fromAccount.AccountNumber, toAccount.AccountNumber, amount,
                                description + $" (Fee: ${transactionFee:F2})", true, "Success");
                return true;
            }

            return false;
        }

        // Private method to record transaction
        private void RecordTransaction(string fromAccount, string toAccount, double amount,
                                      string description, bool success, string status)
        {
            transactionCounter++;
            string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string record = $"TXN{transactionCounter} | {timestamp} | From: {fromAccount} | " +
                          $"To: {toAccount} | Amount: ${amount:F2} | {description} | " +
                          $"Status: {status}";
            transactionHistory.Add(record);
        }

        // Public method to display transaction history
        public void DisplayTransactionHistory()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║                    TRANSACTION HISTORY                         ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════════╝");

            if (transactionHistory.Count == 0)
            {
                Console.WriteLine("\nNo transactions recorded.");
                return;
            }

            foreach (string transaction in transactionHistory)
            {
                Console.WriteLine($"\n{transaction}");
            }
        }

        // Display specific transaction
        public void DisplayLastTransaction()
        {
            if (transactionHistory.Count > 0)
            {
                Console.WriteLine($"\nLast Transaction: {transactionHistory[transactionHistory.Count - 1]}");
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║   LAB 04 - C01: BANK TRANSACTION (OVERLOADING)           ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");

            // Create bank accounts
            BankAccount acc1 = new BankAccount("ACC001", "Rajesh Kumar", 50000.00);
            BankAccount acc2 = new BankAccount("ACC002", "Priya Sharma", 30000.00);
            BankAccount acc3 = new BankAccount("ACC003", "Amit Patel", 75000.00);

            // Create transaction manager
            BankTransaction txnManager = new BankTransaction();

            // Display initial balances
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("   INITIAL ACCOUNT BALANCES");
            Console.WriteLine(new string('═', 60));
            DisplayAccountInfo(acc1);
            DisplayAccountInfo(acc2);
            DisplayAccountInfo(acc3);

            // Test 1: Transfer with amount only (default description)
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("[Test 1] Transfer(from, to, amount)");
            Console.WriteLine("Method Signature: Transfer(BankAccount, BankAccount, double)");
            Console.WriteLine(new string('-', 60));

            Console.WriteLine($"\nTransferring $5,000 from {acc1.AccountHolder} to {acc2.AccountHolder}");
            bool result1 = txnManager.Transfer(acc1, acc2, 5000);
            Console.WriteLine($"Status: {(result1 ? "SUCCESS" : "FAILED")}");

            DisplayAccountInfo(acc1);
            DisplayAccountInfo(acc2);

            // Test 2: Transfer with amount and description
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("[Test 2] Transfer(from, to, amount, description)");
            Console.WriteLine("Method Signature: Transfer(BankAccount, BankAccount, double, string)");
            Console.WriteLine(new string('-', 60));

            Console.WriteLine($"\nTransferring $10,000 from {acc3.AccountHolder} to {acc1.AccountHolder}");
            Console.WriteLine("Description: 'Business Payment'");
            bool result2 = txnManager.Transfer(acc3, acc1, 10000, "Business Payment");
            Console.WriteLine($"Status: {(result2 ? "SUCCESS" : "FAILED")}");

            DisplayAccountInfo(acc3);
            DisplayAccountInfo(acc1);

            // Test 3: Transfer with amount, description, and fee
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("[Test 3] Transfer(from, to, amount, description, fee)");
            Console.WriteLine("Method Signature: Transfer(BankAccount, BankAccount, double, string, double)");
            Console.WriteLine(new string('-', 60));

            Console.WriteLine($"\nTransferring $8,000 from {acc2.AccountHolder} to {acc3.AccountHolder}");
            Console.WriteLine("Description: 'International Transfer'");
            Console.WriteLine("Transaction Fee: $50.00");
            bool result3 = txnManager.Transfer(acc2, acc3, 8000, "International Transfer", 50.00);
            Console.WriteLine($"Status: {(result3 ? "SUCCESS" : "FAILED")}");

            DisplayAccountInfo(acc2);
            DisplayAccountInfo(acc3);

            // Test 4: Failed transaction (insufficient balance)
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("[Test 4] Failed Transaction (Insufficient Balance)");
            Console.WriteLine(new string('-', 60));

            Console.WriteLine($"\nAttempting to transfer $100,000 from {acc2.AccountHolder} to {acc1.AccountHolder}");
            bool result4 = txnManager.Transfer(acc2, acc1, 100000, "Large Transfer");
            Console.WriteLine($"Status: {(result4 ? "SUCCESS" : "FAILED")}");

            // Display final balances
            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("   FINAL ACCOUNT BALANCES");
            Console.WriteLine(new string('═', 60));
            DisplayAccountInfo(acc1);
            DisplayAccountInfo(acc2);
            DisplayAccountInfo(acc3);

            // Display complete transaction history
            txnManager.DisplayTransactionHistory();

            // Key Learning Points
            Console.WriteLine("\n╔══════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         KEY LEARNING POINTS                              ║");
            Console.WriteLine("╚══════════════════════════════════════════════════════════╝");
            Console.WriteLine("\n✓ ENCAPSULATION: Private fields (balance, accountNumber)");
            Console.WriteLine("  protected from direct access");
            Console.WriteLine("\n✓ METHOD OVERLOADING: Same method name (Transfer) with");
            Console.WriteLine("  different parameters:");
            Console.WriteLine("  - Transfer(from, to, amount)");
            Console.WriteLine("  - Transfer(from, to, amount, description)");
            Console.WriteLine("  - Transfer(from, to, amount, description, fee)");
            Console.WriteLine("\n✓ PUBLIC METHODS: Provide controlled access to private");
            Console.WriteLine("  data (Withdraw, Deposit, Balance property)");
            Console.WriteLine("\n✓ DATA HIDING: Internal transaction logic hidden from");
            Console.WriteLine("  external code");
            Console.WriteLine("\n✓ VALIDATION: Methods validate inputs before executing");
            Console.WriteLine("  transactions");

            Console.WriteLine("\n" + new string('═', 60));
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // Helper method to display account information
        static void DisplayAccountInfo(BankAccount account)
        {
            Console.WriteLine($"\n┌────────────────────────────────────────────────────┐");
            Console.WriteLine($"│ Account: {account.AccountNumber,-39} │");
            Console.WriteLine($"│ Holder:  {account.AccountHolder,-39} │");
            Console.WriteLine($"│ Balance: ${account.Balance,-38:F2} │");
            Console.WriteLine($"└────────────────────────────────────────────────────┘");
        }
    }
}
