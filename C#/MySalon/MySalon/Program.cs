using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text.RegularExpressions;

/// <summary>
/// 1. Object-Oriented Hierarchy & Abstraction
/// Interface IPayable:
/// Contract methods:
/// void ProcessPayment(decimal amount)
/// bool IsPaid { get; }
/// 
/// Abstract Base Class ServiceBooking (Implements IPayable):
/// Properties:
/// BookingId (int, unique auto-generated)
/// BookingNumber (string, unique auto-generated system code e.g. BK-1001)
/// CustomerName (string)
/// BasePrice (decimal)
/// DurationMinutes (int)
/// IsPaid (bool)
/// Abstract Method:
/// public abstract decimal CalculateFinalPrice();
/// 
/// Derived Class 1: StandardBooking:
/// Overrides CalculateFinalPrice() → Returns BasePrice + 8% Service Tax.
/// 
/// Derived Class 2: VipBooking:
/// Additional Property:
/// VipDiscountPercentage (decimal, e.g., 15 for 15%).
/// Overrides CalculateFinalPrice() → Applies discount to BasePrice first, then adds 8% Service Tax.
/// 
/// 
/// 1. Create Standard Booking (Prompt for Customer Name, Base Price, Duration in minutes).
/// 2. Create VIP Booking (Prompt for Customer Name, Base Price, Duration, and VIP Discount %).
/// 3. Process Payment for a Booking (Search by Booking Number / ID, display calculated final price, mark IsPaid = true).
/// 4. Display All Bookings (Print formatted table/list of all bookings with status).
/// 5. Display Total Revenue & Outstanding Balance (LINQ):
/// - Total revenue collected (Sum of CalculateFinalPrice() for paid bookings).
/// - Total unpaid pending amount (Sum of CalculateFinalPrice() for unpaid bookings).
/// 6. Search Booking by Customer Name (LINQ) (Case-insensitive search returning all matching bookings).
/// 7. Filter Bookings by Duration (LINQ) (Show bookings with duration greater than user-entered value).
/// 8. Exit Application
/// </summary>
Console.WriteLine("-----------------------------------------");
Console.WriteLine("Welcome to My Salon");
Console.WriteLine("-----------------------------------------");

MainMenu:
Console.WriteLine();
ShowOptions();
string? option = Console.ReadLine();
if (int.TryParse(option, out int selectedOption))
{
	switch (selectedOption)
	{
		case 1:
			CreateBooking(BookingType.Standard);
			break;
		case 2:
			CreateBooking(BookingType.VIP);
			break;
		case 3:
			ProcessPayment();
			break;
		case 4:
			DisplayAllBookings();
			break;
		case 5:
			DisplayRevenueAndOutstandingBalance();
			break;
		case 6:
			SearchBookingByCustomerName();
			break;
		case 7:
			FilterBookingsByDuration();
			break;
		case 8:
			ShowSuccessMessage("Exiting application. Goodbye!");
			return;
		default:
			ShowErrorMessage("Invalid option. Please select a valid option.");
			goto MainMenu;
	}
}
else
{
	ShowErrorMessage("Invalid input. Please enter a number corresponding to the options.");
	goto MainMenu;
}
goto MainMenu;

#region Helper Methods
static void ShowOptions()
{
	Console.WriteLine("Select an option:");
	Console.WriteLine("1. Create Standard Booking");
	Console.WriteLine("2. Create VIP Booking");
	Console.WriteLine("3. Process Payment for a Booking");
	Console.WriteLine("4. Display All Bookings");
	Console.WriteLine("5. Display Total Revenue & Outstanding Balance");
	Console.WriteLine("6. Search Booking by Customer Name");
	Console.WriteLine("7. Filter Bookings by Duration");
	Console.WriteLine("8. Exit Application");
}

static void DisplayBookingDetails(ServiceBooking booking)
{
	Console.WriteLine($"Booking ID: {booking.BookingId}");
	Console.WriteLine($"Booking Number: {booking.BookingNumber}");
	Console.WriteLine($"Customer Name: {booking.CustomerName}");
	Console.WriteLine($"Booking Type: {(booking is VipBooking ? "VIP" : "Standard")}");
	if (booking is VipBooking vip)
	{
		Console.WriteLine($"VIP Discount: {vip.VipDiscountPercentage}%");
	}
	Console.WriteLine($"Base Price: {booking.BasePrice:C}");
	Console.WriteLine($"Duration (minutes): {booking.DurationMinutes}");
	Console.WriteLine($"Final Price: {booking.CalculateFinalPrice():C}");
	Console.WriteLine($"Status: {(booking.IsPaid ? "Paid" : "Pending")}");
	Console.WriteLine(new string('-', 30));
}

static void CreateBooking(BookingType bookingType)
{
	string customerName = PromptForString("Enter Customer Name: ");
	decimal basePrice = PromptForDecimal("Enter Base Price: ");
	int duration = PromptForInt("Enter Duration (minutes): ");

	BookingService bookingService = new BookingService();
	ServiceBooking booking;

	if (bookingType == BookingType.Standard)
	{
		booking = new StandardBooking(customerName, basePrice, duration);
	}
	else if (bookingType == BookingType.VIP)
	{
		decimal vipDiscount = PromptForDecimal("Enter VIP Discount Percentage: ");
		booking = new VipBooking(customerName, basePrice, duration, vipDiscount);
	}
	else
	{
		ShowErrorMessage("Invalid booking type selected.");
		return;
	}

	bookingService.CreateBooking(booking);
	ShowSuccessMessage($"{(bookingType == BookingType.Standard ? "Standard" : "VIP")} booking created successfully.");
	Console.WriteLine();
	DisplayBookingDetails(booking);
}

static void ProcessPayment()
{
	Console.Write("Enter Booking Number (or Booking ID) to process payment: ");
	string? searchInput = Console.ReadLine();
	if (string.IsNullOrWhiteSpace(searchInput))
	{
		ShowErrorMessage("Booking Number cannot be empty.");
		return;
	}

	BookingService bookingService = new BookingService();
	ServiceBooking? booking = bookingService.GetBookingByNumber(searchInput);
	if (booking == null)
	{
		ShowErrorMessage($"No booking found matching '{searchInput}'.");
		return;
	}

	Console.WriteLine();
	DisplayBookingDetails(booking);

	if (booking.IsPaid)
	{
		ShowErrorMessage($"Booking Number {booking.BookingNumber} has already been paid.");
		return;
	}

	Console.WriteLine($"Final Price for Booking Number {booking.BookingNumber} (ID {booking.BookingId}): {booking.CalculateFinalPrice():C}");
	decimal paymentAmount = PromptForDecimal("Enter Payment Amount: ");
	booking.ProcessPayment(paymentAmount);
}

static void DisplayAllBookings()
{
	BookingService bookingService = new BookingService();
	var bookings = bookingService.GetAllBookings();

	if (bookings.Count == 0)
	{
		Console.WriteLine("\nNo bookings found.");
		return;
	}

	Console.WriteLine("\n--- All Bookings ---");
	foreach (var booking in bookings)
	{
		DisplayBookingDetails(booking);
	}
}

static void DisplayRevenueAndOutstandingBalance()
{
	BookingService bookingService = new BookingService();
	decimal revenue = bookingService.GetTotalRevenue();
	decimal outstanding = bookingService.GetOutstandingBalance();

	Console.WriteLine("\n--- Revenue & Outstanding Balance Summary ---");
	Console.WriteLine($"Total Revenue Collected (Paid): {revenue:C}");
	Console.WriteLine($"Total Outstanding Balance (Pending): {outstanding:C}");
	Console.WriteLine(new string('-', 45));
}

static void SearchBookingByCustomerName()
{
	string searchName = PromptForString("Enter Customer Name to search: ");
	BookingService bookingService = new BookingService();
	var results = bookingService.SearchByCustomerName(searchName);

	if (results.Count == 0)
	{
		ShowErrorMessage($"No bookings found for customer matching '{searchName}'.");
		return;
	}

	Console.WriteLine($"\n--- Found {results.Count} Booking(s) for '{searchName}' ---");
	foreach (var booking in results)
	{
		DisplayBookingDetails(booking);
	}
}

static void FilterBookingsByDuration()
{
	int minDuration = PromptForInt("Enter minimum Duration in minutes: ");
	BookingService bookingService = new BookingService();
	var results = bookingService.FilterByDuration(minDuration);

	if (results.Count == 0)
	{
		ShowErrorMessage($"No bookings found with duration greater than {minDuration} minutes.");
		return;
	}

	Console.WriteLine($"\n--- Bookings with Duration > {minDuration} minutes ---");
	foreach (var booking in results)
	{
		DisplayBookingDetails(booking);
	}
}

static string PromptForString(string prompt)
{
	Console.Write(prompt);
	string? input = Console.ReadLine();
	if (string.IsNullOrWhiteSpace(input))
	{
		ShowErrorMessage("Input cannot be empty. Please enter a valid value.");
		return PromptForString(prompt);
	}
	if (!Regex.IsMatch(input, @"^[a-zA-Z\s]+$"))
	{
		ShowErrorMessage("Invalid input. Please enter a valid name.");
		return PromptForString(prompt);
	}
	return input;
}

static decimal PromptForDecimal(string prompt)
{
	Console.Write(prompt);
	if (decimal.TryParse(Console.ReadLine(), out decimal result) && result >= 0)
	{
		return result;
	}
	ShowErrorMessage("Invalid input. Please enter a valid non-negative decimal number.");
	return PromptForDecimal(prompt);
}

static int PromptForInt(string prompt)
{
	Console.Write(prompt);
	if (int.TryParse(Console.ReadLine(), out int result) && result > 0)
	{
		return result;
	}
	ShowErrorMessage("Invalid input. Please enter a valid positive integer.");
	return PromptForInt(prompt);
}

static void ShowErrorMessage(string message)
{
	Console.ForegroundColor = ConsoleColor.Red;
	Console.WriteLine($"{message}");
	Console.ResetColor();
}

static void ShowSuccessMessage(string message)
{
	Console.ForegroundColor = ConsoleColor.Green;
	Console.WriteLine($"{message}");
	Console.ResetColor();
}
#endregion

#region Enum and Interface
public enum BookingType
{
	Standard,
	VIP
}

public interface IPayable
{
	void ProcessPayment(decimal amount);
	bool IsPaid { get; }
}
#endregion

#region Abstract Base Class
public abstract class ServiceBooking : IPayable
{
	public int BookingId { get; set; }
	public string BookingNumber { get; set; } = string.Empty;
	public string CustomerName { get; set; }
	public decimal BasePrice { get; set; }
	public int DurationMinutes { get; set; }
	public bool IsPaid { get; private set; }

	public ServiceBooking(string customerName, decimal basePrice, int durationMinutes)
	{
		CustomerName = customerName;
		BasePrice = basePrice;
		DurationMinutes = durationMinutes;
		IsPaid = false;
	}

	public abstract decimal CalculateFinalPrice();

	public void ProcessPayment(decimal amount)
	{
		if (amount >= CalculateFinalPrice())
		{
			IsPaid = true;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine($"Payment of {amount:C} processed successfully for Booking Number: {BookingNumber} (ID: {BookingId}).");
			Console.ResetColor();
		}
		else
		{
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine($"Insufficient payment. Required: {CalculateFinalPrice():C}, Provided: {amount:C}");
			Console.ResetColor();
		}
	}
}
#endregion

#region Derived Classes
public class StandardBooking : ServiceBooking
{
	private const decimal ServiceTaxRate = 0.08m; // 8% service tax
	public StandardBooking(string customerName, decimal basePrice, int durationMinutes)
		: base(customerName, basePrice, durationMinutes)
	{
	}
	public override decimal CalculateFinalPrice()
	{
		return BasePrice + (BasePrice * ServiceTaxRate); // Adding 8% service tax
	}
}

public class VipBooking : ServiceBooking
{
	public decimal VipDiscountPercentage { get; set; }
	private const decimal ServiceTaxRate = 0.08m; // 8% service tax
	public VipBooking(string customerName, decimal basePrice, int durationMinutes, decimal vipDiscountPercentage)
		: base(customerName, basePrice, durationMinutes)
	{
		VipDiscountPercentage = vipDiscountPercentage;
	}
	public override decimal CalculateFinalPrice()
	{
		var discountedPrice = BasePrice - (BasePrice * (VipDiscountPercentage / 100));
		return discountedPrice + (discountedPrice * ServiceTaxRate); // Adding 8% service tax after discount
	}
}
#endregion

#region Booking service
public class BookingService
{
	private static readonly List<ServiceBooking> bookings = new List<ServiceBooking>();

	public void CreateBooking(ServiceBooking booking)
	{
		booking.BookingId = bookings.Count + 1;
		booking.BookingNumber = $"BK-{1000 + booking.BookingId}";
		bookings.Add(booking);
	}

	public ServiceBooking? GetBookingById(int id)
	{
		return bookings.FirstOrDefault(b => b.BookingId == id);
	}

	public ServiceBooking? GetBookingByNumber(string bookingNumber)
	{
		if (string.IsNullOrWhiteSpace(bookingNumber)) return null;

		var booking = bookings.FirstOrDefault(b => b.BookingNumber.Equals(bookingNumber.Trim(), StringComparison.OrdinalIgnoreCase));
		if (booking == null && int.TryParse(bookingNumber, out int id))
		{
			booking = GetBookingById(id);
		}
		return booking;
	}

	public List<ServiceBooking> GetAllBookings()
	{
		return bookings;
	}

	public List<ServiceBooking> SearchByCustomerName(string name)
	{
		return bookings.Where(b => b.CustomerName.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
	}

	public List<ServiceBooking> FilterByDuration(int minDuration)
	{
		return bookings.Where(b => b.DurationMinutes > minDuration).ToList();
	}

	public decimal GetTotalRevenue()
	{
		return bookings.Where(b => b.IsPaid).Sum(b => b.CalculateFinalPrice());
	}

	public decimal GetOutstandingBalance()
	{
		return bookings.Where(b => !b.IsPaid).Sum(b => b.CalculateFinalPrice());
	}
}
#endregion