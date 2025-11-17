/*
 * Lab03_C03_FlightTicket
 * Problem: Create a FlightTicket class with passengerName, flightNumber, and ticketPrice.
 *          Throw exception if ticket price < Rs. 500.
 *
 * Learning Objectives:
 * - Understanding validation in constructors
 * - Custom business rule exceptions
 * - Property validation and encapsulation
 * - Real-world application scenario
 */

using System;

namespace Lab03_C03_FlightTicket
{
    // Custom exception for invalid ticket price
    class InvalidTicketPriceException : Exception
    {
        public InvalidTicketPriceException(string message) : base(message) { }
    }

    // FlightTicket class
    class FlightTicket
    {
        // Properties
        public string PassengerName { get; set; }
        public string FlightNumber { get; set; }
        private double ticketPrice;
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime DepartureTime { get; set; }
        public string SeatNumber { get; set; }
        public string TicketClass { get; set; } // Economy, Business, First

        // Minimum ticket price constant
        private const double MINIMUM_TICKET_PRICE = 500.00;

        // Property with validation for ticket price
        public double TicketPrice
        {
            get { return ticketPrice; }
            set
            {
                if (value < MINIMUM_TICKET_PRICE)
                {
                    throw new InvalidTicketPriceException(
                        $"Invalid ticket price! Minimum ticket price is Rs. {MINIMUM_TICKET_PRICE:F2}. " +
                        $"Provided price: Rs. {value:F2}");
                }
                ticketPrice = value;
            }
        }

        // Basic constructor
        public FlightTicket(string passengerName, string flightNumber, double ticketPrice)
        {
            PassengerName = passengerName;
            FlightNumber = flightNumber;
            // This will trigger validation
            TicketPrice = ticketPrice;
        }

        // Extended constructor with additional details
        public FlightTicket(string passengerName, string flightNumber, double ticketPrice,
                           string source, string destination, DateTime departureTime,
                           string seatNumber, string ticketClass)
            : this(passengerName, flightNumber, ticketPrice)
        {
            Source = source;
            Destination = destination;
            DepartureTime = departureTime;
            SeatNumber = seatNumber;
            TicketClass = ticketClass;
        }

        // Method to calculate GST (5% for domestic flights)
        public double CalculateGST()
        {
            return TicketPrice * 0.05;
        }

        // Method to calculate total price including GST
        public double GetTotalPrice()
        {
            return TicketPrice + CalculateGST();
        }

        // Method to display basic ticket details
        public void DisplayBasicDetails()
        {
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"Passenger Name   : {PassengerName}");
            Console.WriteLine($"Flight Number    : {FlightNumber}");
            Console.WriteLine($"Ticket Price     : Rs. {TicketPrice:N2}");
            Console.WriteLine($"GST (5%)         : Rs. {CalculateGST():N2}");
            Console.WriteLine($"Total Amount     : Rs. {GetTotalPrice():N2}");
            Console.WriteLine(new string('-', 60));
        }

        // Method to display complete ticket details
        public void DisplayCompleteDetails()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("               FLIGHT TICKET");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"Passenger Name   : {PassengerName}");
            Console.WriteLine($"Flight Number    : {FlightNumber}");
            Console.WriteLine($"Route            : {Source} → {Destination}");
            Console.WriteLine($"Departure Time   : {DepartureTime:dd-MMM-yyyy HH:mm}");
            Console.WriteLine($"Seat Number      : {SeatNumber}");
            Console.WriteLine($"Class            : {TicketClass}");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"Base Fare        : Rs. {TicketPrice:N2}");
            Console.WriteLine($"GST (5%)         : Rs. {CalculateGST():N2}");
            Console.WriteLine($"Total Amount     : Rs. {GetTotalPrice():N2}");
            Console.WriteLine(new string('=', 60));
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader("Flight Ticket Booking System");

            try
            {
                // Valid Ticket 1: Basic details
                Console.WriteLine("\n*** Ticket 1: Economy Class ***");
                FlightTicket ticket1 = new FlightTicket(
                    "Rajesh Kumar",
                    "AI-101",
                    2500.00
                );
                ticket1.DisplayBasicDetails();

                // Valid Ticket 2: Complete details
                Console.WriteLine("\n*** Ticket 2: Business Class ***");
                FlightTicket ticket2 = new FlightTicket(
                    "Priya Sharma",
                    "6E-205",
                    8500.00,
                    "Delhi",
                    "Mumbai",
                    new DateTime(2025, 12, 25, 14, 30, 0),
                    "12A",
                    "Business"
                );
                ticket2.DisplayCompleteDetails();

                // Valid Ticket 3: Another example
                Console.WriteLine("\n*** Ticket 3: First Class ***");
                FlightTicket ticket3 = new FlightTicket(
                    "Vikram Patel",
                    "UK-890",
                    15000.00,
                    "Ahmedabad",
                    "Bangalore",
                    new DateTime(2025, 12, 28, 10, 15, 0),
                    "1F",
                    "First Class"
                );
                ticket3.DisplayCompleteDetails();

                // Demonstrate exception: Ticket price below minimum
                Console.WriteLine("\n*** Attempting to Book Ticket with Invalid Price ***");
                FlightTicket ticket4 = new FlightTicket(
                    "Invalid Passenger",
                    "TEST-001",
                    300.00  // Below minimum price of Rs. 500
                );
            }
            catch (InvalidTicketPriceException ex)
            {
                Console.WriteLine($"\n*** BOOKING FAILED ***");
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine("Please check the ticket price and try again.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n*** ERROR ***");
                Console.WriteLine($"Error: {ex.Message}");
            }

            // Demonstrate another invalid price scenario
            Console.WriteLine("\n\n*** Testing Zero Price Ticket ***");
            try
            {
                FlightTicket ticket5 = new FlightTicket(
                    "Test Passenger",
                    "TEST-002",
                    0.00
                );
            }
            catch (InvalidTicketPriceException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            // Demonstrate negative price scenario
            Console.WriteLine("\n*** Testing Negative Price Ticket ***");
            try
            {
                FlightTicket ticket6 = new FlightTicket(
                    "Test Passenger",
                    "TEST-003",
                    -100.00
                );
            }
            catch (InvalidTicketPriceException ex)
            {
                Console.WriteLine($"Exception caught: {ex.Message}");
            }

            Console.WriteLine("\n*** Booking Policy ***");
            Console.WriteLine(new string('-', 60));
            Console.WriteLine($"Minimum Ticket Price: Rs. 500.00");
            Console.WriteLine("GST: 5% on base fare");
            Console.WriteLine("Classes: Economy, Business, First Class");

            PrintFooter();
        }

        // Helper method to print header
        static void PrintHeader(string title)
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"  {title}");
            Console.WriteLine(new string('=', 60));
        }

        // Helper method to print footer
        static void PrintFooter()
        {
            Console.WriteLine(new string('=', 60));
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
