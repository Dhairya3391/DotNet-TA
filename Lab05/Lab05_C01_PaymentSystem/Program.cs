using System;

namespace Lab05_C01_PaymentSystem
{
    // Custom exception for invalid payment amount
    class InvalidPaymentException : Exception
    {
        public InvalidPaymentException(string message) : base(message)
        {
        }
    }

    // Abstract base class - Payment
    // Demonstrates abstraction with exception handling
    abstract class Payment
    {
        public string PaymentId { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public string Status { get; protected set; }

        protected Payment(double amount)
        {
            // Validate amount - must be >= 100
            if (amount < 100)
            {
                throw new InvalidPaymentException($"Payment amount must be at least Rs. 100. Provided: Rs. {amount}");
            }

            Amount = amount;
            PaymentId = GeneratePaymentId();
            PaymentDate = DateTime.Now;
            Status = "Pending";
        }

        // Abstract method - must be implemented by derived classes
        public abstract void MakePayment();

        // Abstract method for processing refund
        public abstract void ProcessRefund();

        // Concrete method
        protected string GeneratePaymentId()
        {
            return "PAY" + DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public void DisplayPaymentInfo()
        {
            Console.WriteLine($"Payment ID    : {PaymentId}");
            Console.WriteLine($"Amount        : Rs. {Amount:F2}");
            Console.WriteLine($"Date          : {PaymentDate:dd-MMM-yyyy HH:mm:ss}");
            Console.WriteLine($"Status        : {Status}");
        }
    }

    // Derived class - CreditCardPayment
    class CreditCardPayment : Payment
    {
        public string CardNumber { get; set; }
        public string CardHolderName { get; set; }
        public string ExpiryDate { get; set; }

        public CreditCardPayment(double amount, string cardNumber, string cardHolderName, string expiryDate)
            : base(amount)
        {
            CardNumber = MaskCardNumber(cardNumber);
            CardHolderName = cardHolderName;
            ExpiryDate = expiryDate;
        }

        private string MaskCardNumber(string cardNumber)
        {
            if (cardNumber.Length >= 4)
            {
                return "XXXX-XXXX-XXXX-" + cardNumber.Substring(cardNumber.Length - 4);
            }
            return cardNumber;
        }

        public override void MakePayment()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║           PROCESSING CREDIT CARD PAYMENT                   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine("\nConnecting to payment gateway...");
            Console.WriteLine("Verifying card details...");
            Console.WriteLine($"Card: {CardNumber}");
            Console.WriteLine($"Holder: {CardHolderName}");
            Console.WriteLine($"Expiry: {ExpiryDate}");
            Console.WriteLine("Authorizing payment...");
            Console.WriteLine($"Processing Rs. {Amount:F2}...");

            Status = "Completed";

            Console.WriteLine("\n✓ Credit Card Payment Successful!");
            DisplayPaymentInfo();
        }

        public override void ProcessRefund()
        {
            Console.WriteLine($"\n[REFUND] Processing refund of Rs. {Amount:F2} to card {CardNumber}");
            Console.WriteLine("Refund will be credited within 5-7 business days.");
            Status = "Refunded";
        }
    }

    // Derived class - UPIPayment
    class UPIPayment : Payment
    {
        public string UpiId { get; set; }
        public string BankName { get; set; }

        public UPIPayment(double amount, string upiId, string bankName)
            : base(amount)
        {
            UpiId = upiId;
            BankName = bankName;
        }

        public override void MakePayment()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              PROCESSING UPI PAYMENT                        ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine("\nInitiating UPI transaction...");
            Console.WriteLine($"UPI ID: {UpiId}");
            Console.WriteLine($"Bank: {BankName}");
            Console.WriteLine("Sending payment request...");
            Console.WriteLine($"Amount: Rs. {Amount:F2}");
            Console.WriteLine("Waiting for UPI PIN authorization...");

            Status = "Completed";

            Console.WriteLine("\n✓ UPI Payment Successful!");
            DisplayPaymentInfo();
        }

        public override void ProcessRefund()
        {
            Console.WriteLine($"\n[REFUND] Processing refund of Rs. {Amount:F2} to UPI ID: {UpiId}");
            Console.WriteLine("Refund will be credited instantly.");
            Status = "Refunded";
        }
    }

    // Additional class - NetBankingPayment
    class NetBankingPayment : Payment
    {
        public string BankName { get; set; }
        public string AccountNumber { get; set; }

        public NetBankingPayment(double amount, string bankName, string accountNumber)
            : base(amount)
        {
            BankName = bankName;
            AccountNumber = MaskAccountNumber(accountNumber);
        }

        private string MaskAccountNumber(string accountNumber)
        {
            if (accountNumber.Length >= 4)
            {
                return "XXXXXX" + accountNumber.Substring(accountNumber.Length - 4);
            }
            return accountNumber;
        }

        public override void MakePayment()
        {
            Console.WriteLine("\n╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║            PROCESSING NET BANKING PAYMENT                  ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine("\nRedirecting to bank portal...");
            Console.WriteLine($"Bank: {BankName}");
            Console.WriteLine($"Account: {AccountNumber}");
            Console.WriteLine("Authenticating user...");
            Console.WriteLine($"Debiting Rs. {Amount:F2}...");

            Status = "Completed";

            Console.WriteLine("\n✓ Net Banking Payment Successful!");
            DisplayPaymentInfo();
        }

        public override void ProcessRefund()
        {
            Console.WriteLine($"\n[REFUND] Processing refund of Rs. {Amount:F2} to account {AccountNumber}");
            Console.WriteLine("Refund will be credited within 3-5 business days.");
            Status = "Refunded";
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Display header
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║         Lab 05 - C01: Payment System Demo                  ║");
            Console.WriteLine("║    (Abstract Class + Exception Handling)                   ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
            Console.WriteLine();

            try
            {
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Test Case 1: Valid Credit Card Payment");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                CreditCardPayment ccPayment = new CreditCardPayment(
                    2500.00,
                    "1234567812345678",
                    "John Doe",
                    "12/26"
                );
                ccPayment.MakePayment();

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Test Case 2: Valid UPI Payment");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                UPIPayment upiPayment = new UPIPayment(
                    1500.00,
                    "john@oksbi",
                    "State Bank of India"
                );
                upiPayment.MakePayment();

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Test Case 3: Valid Net Banking Payment");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                NetBankingPayment nbPayment = new NetBankingPayment(
                    3000.00,
                    "HDFC Bank",
                    "123456789012"
                );
                nbPayment.MakePayment();

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Test Case 4: Invalid Payment (Amount < Rs. 100)");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                try
                {
                    Console.WriteLine("\nAttempting to create payment with Rs. 50...");
                    CreditCardPayment invalidPayment = new CreditCardPayment(
                        50.00,
                        "9876543298765432",
                        "Jane Smith",
                        "06/25"
                    );
                    invalidPayment.MakePayment();
                }
                catch (InvalidPaymentException ex)
                {
                    Console.WriteLine("\n[EXCEPTION CAUGHT]");
                    Console.WriteLine($"Error: {ex.Message}");
                    Console.WriteLine("Transaction declined!");
                }

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Test Case 5: Processing Refunds");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                ccPayment.ProcessRefund();
                upiPayment.ProcessRefund();
                nbPayment.ProcessRefund();

                Console.WriteLine("\n\n━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Polymorphism Demonstration:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");

                Payment[] payments = {
                    new CreditCardPayment(500, "1111222233334444", "Alice", "10/27"),
                    new UPIPayment(750, "alice@paytm", "Paytm Payments Bank"),
                    new NetBankingPayment(1000, "ICICI Bank", "987654321098")
                };

                Console.WriteLine("\nProcessing multiple payments using polymorphism:\n");
                foreach (Payment payment in payments)
                {
                    payment.MakePayment();
                    Console.WriteLine();
                }

                Console.WriteLine();

                // Explain concepts
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("  Concepts Demonstrated:");
                Console.WriteLine("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━");
                Console.WriteLine("✓ Abstract Payment class with abstract MakePayment() method");
                Console.WriteLine("✓ Custom InvalidPaymentException for amount validation");
                Console.WriteLine("✓ Exception thrown if amount < Rs. 100");
                Console.WriteLine("✓ CreditCardPayment, UPIPayment, NetBanking implementations");
                Console.WriteLine("✓ Each payment type has unique processing logic");
                Console.WriteLine("✓ Polymorphism with Payment reference");
                Console.WriteLine("✓ Try-catch for exception handling");
                Console.WriteLine();

            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n[UNEXPECTED ERROR] {ex.Message}");
            }

            // Footer
            Console.WriteLine("╔════════════════════════════════════════════════════════════╗");
            Console.WriteLine("║              Program Completed Successfully                ║");
            Console.WriteLine("╚════════════════════════════════════════════════════════════╝");
        }
    }
}
