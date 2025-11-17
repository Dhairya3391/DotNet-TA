// Lab 1 - C Task 3
// Cab fare system with complex conditions
// Vehicle types: Sedan (₹12/km), SUV (₹15/km), Luxury (₹20/km)
// Driver allowance if distance > 150 km: Sedan-₹500, SUV-₹700, Luxury-₹1000
// Fuel surcharge: 5% on every trip
// Discount: 10% if distance > 100 km (before surcharge and driver allowance)
// Trip type: One-way or Round-trip (round-trip doubles the distance)

Console.WriteLine("========================================");
Console.WriteLine("       CAB FARE CALCULATOR");
Console.WriteLine("========================================");
Console.WriteLine();

// Vehicle selection
Console.WriteLine("Select vehicle type:");
Console.WriteLine("1. Sedan (₹12/km)");
Console.WriteLine("2. SUV (₹15/km)");
Console.WriteLine("3. Luxury (₹20/km)");
Console.Write("Enter your choice (1-3): ");
int vehicleChoice = Convert.ToInt32(Console.ReadLine());

// Trip type selection
Console.WriteLine();
Console.WriteLine("Select trip type:");
Console.WriteLine("1. One-way");
Console.WriteLine("2. Round-trip");
Console.Write("Enter your choice (1-2): ");
int tripChoice = Convert.ToInt32(Console.ReadLine());

// Distance input
Console.WriteLine();
Console.Write("Enter distance (in km): ");
double baseDistance = Convert.ToDouble(Console.ReadLine());

// Calculate total distance based on trip type
double totalDistance = baseDistance;
if (tripChoice == 2)
{
    totalDistance = baseDistance * 2; // Round-trip doubles the distance
}

// Determine rate per km and driver allowance based on vehicle type
double ratePerKm;
double driverAllowance = 0;
string vehicleType;

switch (vehicleChoice)
{
    case 1: // Sedan
        vehicleType = "Sedan";
        ratePerKm = 12;
        if (totalDistance > 150)
            driverAllowance = 500;
        break;
    case 2: // SUV
        vehicleType = "SUV";
        ratePerKm = 15;
        if (totalDistance > 150)
            driverAllowance = 700;
        break;
    case 3: // Luxury
        vehicleType = "Luxury";
        ratePerKm = 20;
        if (totalDistance > 150)
            driverAllowance = 1000;
        break;
    default:
        vehicleType = "Unknown";
        ratePerKm = 0;
        break;
}

// Calculate base fare
double baseFare = totalDistance * ratePerKm;

// Apply discount if distance > 100 km
double discount = 0;
if (totalDistance > 100)
{
    discount = baseFare * 0.10; // 10% discount
}

double fareAfterDiscount = baseFare - discount;

// Add fuel surcharge (5% on fare after discount)
double fuelSurcharge = fareAfterDiscount * 0.05;

// Calculate final fare
double finalFare = fareAfterDiscount + fuelSurcharge + driverAllowance;

// Display detailed fare breakdown
Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine("       FARE BREAKDOWN");
Console.WriteLine("========================================");
Console.WriteLine($"Vehicle Type        : {vehicleType}");
Console.WriteLine($"Trip Type           : {(tripChoice == 1 ? "One-way" : "Round-trip")}");
Console.WriteLine($"Base Distance       : {baseDistance} km");
Console.WriteLine($"Total Distance      : {totalDistance} km");
Console.WriteLine($"Rate per km         : ₹{ratePerKm}");
Console.WriteLine("----------------------------------------");
Console.WriteLine($"Base Fare           : ₹{baseFare:F2}");
if (discount > 0)
{
    Console.WriteLine($"Discount (10%)      : -₹{discount:F2}");
    Console.WriteLine($"After Discount      : ₹{fareAfterDiscount:F2}");
}
Console.WriteLine($"Fuel Surcharge (5%) : +₹{fuelSurcharge:F2}");
if (driverAllowance > 0)
{
    Console.WriteLine($"Driver Allowance    : +₹{driverAllowance:F2}");
}
Console.WriteLine("========================================");
Console.WriteLine($"FINAL PAYABLE FARE  : ₹{finalFare:F2}");
Console.WriteLine("========================================");
