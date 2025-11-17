// Lab 1 - A Task 3
// Write program to prompt a user to input his/her name and country name
// and then output will be shown as given: Hello <yourname> from country <countryname>.

Console.WriteLine("========================================");
Console.WriteLine("       HELLO MESSAGE");
Console.WriteLine("========================================");
Console.WriteLine();

Console.Write("Enter your name: ");
string? name = Console.ReadLine();

Console.Write("Enter your country name: ");
string? country = Console.ReadLine();

Console.WriteLine();
Console.WriteLine("========================================");
Console.WriteLine($"Hello {name} from country {country}");
Console.WriteLine("========================================");
