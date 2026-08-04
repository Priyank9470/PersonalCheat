using System.Text.RegularExpressions;

/// <summary>
/// Task:
/// Create a simple Bank Management Console Application using C# and .NET 9 in a single Program.cs file.
/// Requirements:
/// - Use OOP concepts: Encapsulation, Inheritance, Polymorphism, Abstraction, and Interface.
/// - Create an abstract Account class and derived classes: SavingsAccount and CurrentAccount.
/// - Implement an ITransaction interface for Deposit() and Withdraw().
/// - Use List<Account> to store data in memory.
/// - Provide a menu - driven console application with the following features:
///   1.Create Account
///   2.Deposit Money
///   3.Withdraw Money
///   4.Check Balance
///   5.Display All Accounts
///   6.Display Total Bank Balance using LINQ
///   7.Search Account by Account Number using LINQ
///   8.Exit
/// - Include proper input validation and exception handling.
/// - Do not use a database or multiple files.
/// - Keep the code simple, clean, beginner-friendly, and within a single Program.cs file.
/// </summary>
public class Program
{
	private static BankService bankService = new BankService();
	private static List<Account> accounts => bankService.GetAllAccounts();

	public static void Main(string[] args)
	{
		Console.WriteLine("===================================================");
		Console.WriteLine(" Welcome to the Bank Management Console Application!");
		Console.WriteLine("===================================================");

	MainMenu:
		Console.WriteLine();
		ShowOptions();
		string? option = Console.ReadLine();
		if (int.TryParse(option, out int selectedOption))
		{
			switch (selectedOption)
			{
				case 1:
					CreateAccount();
					break;
				case 2:
					DepositMoney();
					break;
				case 3:
					WithdrawMoney();
					break;
				case 4:
					CheckBalance();
					break;
				case 5:
					DisplayAllAccounts();
					break;
				case 6:
					DisplayTotalBankBalance();
					break;
				case 7:
					SearchAccount();
					break;
				case 8:
					ShowSuccessMessage("Thank you for using Bank Management Console Application. Goodbye!");
					return;
				default:
					ShowErrorMessage("Invalid option. Please select a valid option between 1 and 8.");
					goto MainMenu;
			}
		}
		else
		{
			ShowErrorMessage("Invalid input. Please enter a valid number between 1 and 8.");
			goto MainMenu;
		}

		goto MainMenu;
	}

	#region HelperMethods
	private static void ShowOptions()
	{
		Console.WriteLine("Select Options:");
		Console.WriteLine("1. Create Account");
		Console.WriteLine("2. Deposit Money");
		Console.WriteLine("3. Withdraw Money");
		Console.WriteLine("4. Check Balance");
		Console.WriteLine("5. Display All Accounts");
		Console.WriteLine("6. Display Total Bank Balance using LINQ");
		Console.WriteLine("7. Search Account by Account Number using LINQ");
		Console.WriteLine("8. Exit");
		Console.Write("Enter your choice (1-8): ");
	}

	private static void CreateAccount()
	{
		Console.WriteLine("\n--- Create Account ---");

	GetAccountType:
		Console.Write("Enter Account Type (Savings/Current): ");
		string? accountTypeInput = Console.ReadLine();
		if (!Enum.TryParse(accountTypeInput, true, out AccountType accountType))
		{
			ShowErrorMessage("Invalid account type. Please enter 'Savings' or 'Current'.");
			goto GetAccountType;
		}

	GetName:
		Console.Write("Enter Account Holder Name: ");
		string? holderName = Console.ReadLine();
		if (string.IsNullOrWhiteSpace(holderName))
		{
			ShowErrorMessage("Account holder name cannot be empty.");
			goto GetName;
		}
		if (!Regex.IsMatch(holderName, @"^[a-zA-Z\s]+$"))
		{
			ShowErrorMessage("Invalid account holder name. Please use only letters and spaces.");
			goto GetName;
		}

	GetInitialDeposit:
		Console.Write("Enter Initial Deposit Amount: ");
		string? depositInput = Console.ReadLine();
		if (!decimal.TryParse(depositInput, out decimal initialDeposit))
		{
			ShowErrorMessage("Invalid amount. Please enter a valid decimal number.");
			goto GetInitialDeposit;
		}

		try
		{
			Account newAccount;
			if (accountType == AccountType.Savings)
			{
				newAccount = new SavingsAccount("", holderName, initialDeposit);
			}
			else
			{
				newAccount = new CurrentAccount("", holderName, initialDeposit);
			}

			bankService.AddAccount(newAccount);
			ShowSuccessMessage($"Account created successfully! Account Number: {newAccount.AccountNumber}");
		}
		catch (Exception ex)
		{
			ShowErrorMessage($"Error creating account: {ex.Message}");
			goto GetInitialDeposit;
		}
	}

	private static void DepositMoney()
	{
		Console.WriteLine("\n--- Deposit Money ---");

	GetAccountNumber:
		Console.Write("Enter Account Number (or enter 'M' to return to Main Menu): ");
		string? accountNumber = Console.ReadLine()?.Trim();
		if (string.Equals(accountNumber, "M", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(accountNumber))
		{
			ShowErrorMessage("Account number cannot be empty.");
			goto GetAccountNumber;
		}

		Account? account = bankService.SearchAccount(accountNumber);
		if (account == null)
		{
			ShowErrorMessage($"Account '{accountNumber}' not found. Please enter a valid account number.");
			goto GetAccountNumber;
		}

	GetDepositAmount:
		Console.Write("Enter Deposit Amount (or enter 'M' to return to Main Menu): ");
		string? amountInput = Console.ReadLine()?.Trim();
		if (string.Equals(amountInput, "M", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		if (!decimal.TryParse(amountInput, out decimal amount))
		{
			ShowErrorMessage("Invalid amount. Please enter a valid decimal number.");
			goto GetDepositAmount;
		}

		try
		{
			account.Deposit(amount);
			account.UpdatedOn = DateTime.UtcNow;
			ShowSuccessMessage($"Deposit successful! Deposited: {amount:C}. New Balance: {account.Balance:C}");
		}
		catch (Exception ex)
		{
			ShowErrorMessage($"Deposit failed: {ex.Message}");
			goto GetDepositAmount;
		}
	}

	private static void WithdrawMoney()
	{
		Console.WriteLine("\n--- Withdraw Money ---");

	GetAccountNumber:
		Console.Write("Enter Account Number (or enter 'M' to return to Main Menu): ");
		string? accountNumber = Console.ReadLine()?.Trim();
		if (string.Equals(accountNumber, "M", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(accountNumber))
		{
			ShowErrorMessage("Account number cannot be empty.");
			goto GetAccountNumber;
		}

		Account? account = bankService.SearchAccount(accountNumber);
		if (account == null)
		{
			ShowErrorMessage($"Account '{accountNumber}' not found. Please enter a valid account number.");
			goto GetAccountNumber;
		}

	GetWithdrawAmount:
		Console.Write("Enter Withdrawal Amount (or enter 'M' to return to Main Menu): ");
		string? amountInput = Console.ReadLine()?.Trim();
		if (string.Equals(amountInput, "M", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		if (!decimal.TryParse(amountInput, out decimal amount))
		{
			ShowErrorMessage("Invalid amount. Please enter a valid decimal number.");
			goto GetWithdrawAmount;
		}

		try
		{
			account.Withdraw(amount);
			account.UpdatedOn = DateTime.UtcNow;
			ShowSuccessMessage($"Withdrawal successful! Withdrew: {amount:C}. Remaining Balance: {account.Balance:C}");
		}
		catch (Exception ex)
		{
			ShowErrorMessage($"Withdrawal failed: {ex.Message}");
			goto GetWithdrawAmount;
		}
	}

	private static void CheckBalance()
	{
		Console.WriteLine("\n--- Check Balance ---");

	GetAccountNumber:
		Console.Write("Enter Account Number (or enter 'M' to return to Main Menu): ");
		string? accountNumber = Console.ReadLine()?.Trim();
		if (string.Equals(accountNumber, "M", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(accountNumber))
		{
			ShowErrorMessage("Account number cannot be empty.");
			goto GetAccountNumber;
		}

		Account? account = bankService.SearchAccount(accountNumber);
		if (account == null)
		{
			ShowErrorMessage($"Account '{accountNumber}' not found. Please enter a valid account number.");
			goto GetAccountNumber;
		}

		ShowSuccessMessage($"Account Number: {account.AccountNumber} | Holder: {account.AccountHolderName} | Type: {account.AccountType} | Balance: {account.Balance:C}");
	}

	private static void DisplayAllAccounts()
	{
		Console.WriteLine("\n--- Display All Accounts ---");
		List<Account> allAccounts = bankService.GetAllAccounts();
		if (allAccounts.Count == 0)
		{
			ShowErrorMessage("No accounts found in the bank.");
			return;
		}

		Console.WriteLine("-----------------------------------------------------------------------------------------");
		foreach (var acc in allAccounts)
		{
			acc.DisplayInfo();
		}
		Console.WriteLine("-----------------------------------------------------------------------------------------");
	}

	private static void DisplayTotalBankBalance()
	{
		Console.WriteLine("\n--- Total Bank Balance (LINQ) ---");
		decimal totalBalance = bankService.GetTotalBankBalance();
		ShowSuccessMessage($"Total Bank Balance across all accounts: {totalBalance:C}");
	}

	private static void SearchAccount()
	{
		Console.WriteLine("\n--- Search Account by Account Number (LINQ) ---");

	GetAccountNumber:
		Console.Write("Enter Account Number to Search (or enter 'M' to return to Main Menu): ");
		string? accountNumber = Console.ReadLine()?.Trim();
		if (string.Equals(accountNumber, "M", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}

		if (string.IsNullOrWhiteSpace(accountNumber))
		{
			ShowErrorMessage("Account number cannot be empty.");
			goto GetAccountNumber;
		}

		Account? account = bankService.SearchAccount(accountNumber);
		if (account == null)
		{
			ShowErrorMessage($"No account found with Account Number: {accountNumber}. Please try again.");
			goto GetAccountNumber;
		}

		ShowSuccessMessage("Account found:");
		account.DisplayInfo();
	}

	private static void ShowSuccessMessage(string message)
	{
		Console.ForegroundColor = ConsoleColor.Green;
		Console.WriteLine(message);
		Console.ResetColor();
	}

	private static void ShowErrorMessage(string message)
	{
		Console.ForegroundColor = ConsoleColor.Red;
		Console.WriteLine(message);
		Console.ResetColor();
	}
	#endregion
}


#region 1. Interfaces & Enums
public enum AccountType { Savings, Current }

public interface ITransaction
{
	void Deposit(decimal amount);
	void Withdraw(decimal amount);
}
#endregion

#region 2. Base Abstract Account
public abstract class Account : ITransaction
{
	public int AccountId { get; set; }
	public string AccountNumber { get; set; }
	public string AccountHolderName { get; set; }
	public decimal Balance { get; protected set; }
	public AccountType AccountType { get; protected set; }
	public DateTime CreatedOn { get; } = DateTime.UtcNow;
	public DateTime UpdatedOn { get; set; }

	protected Account(string accountNumber, string holderName, decimal initialBalance)
	{
		AccountNumber = accountNumber;
		AccountHolderName = holderName;
		Balance = initialBalance;
	}

	public abstract void Deposit(decimal amount);
	public abstract void Withdraw(decimal amount);
	public abstract void DisplayInfo();
}
#endregion


#region 3. Derived Accounts
public class SavingsAccount : Account
{
	public decimal MinimumBalance { get; } = 500.00m;

	public SavingsAccount(string accountNumber, string holderName, decimal initialBalance)
		: base(accountNumber, holderName, initialBalance)
	{
		AccountType = AccountType.Savings;
		if (initialBalance < MinimumBalance)
		{
			throw new ArgumentException($"Initial deposit for Savings Account must be at least {MinimumBalance:C}.");
		}
	}

	public override void Deposit(decimal amount)
	{
		if (amount <= 0) throw new ArgumentException("Deposit amount must be positive.");
		Balance += amount;
	}

	public override void Withdraw(decimal amount)
	{
		if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive.");
		if (Balance - amount < MinimumBalance)
		{
			throw new InvalidOperationException($"Withdrawal denied. Minimum balance of {MinimumBalance:C} must be maintained.");
		}
		Balance -= amount;
	}

	public override void DisplayInfo()
	{
		Console.WriteLine($"[Savings] Acc: {AccountNumber} | Name: {AccountHolderName} | Balance: {Balance:C} | Min Balance: {MinimumBalance:C}");
	}
}

public class CurrentAccount : Account
{
	public decimal MinimumBalance { get; } = 5000.00m;

	public CurrentAccount(string accountNumber, string holderName, decimal initialBalance)
		: base(accountNumber, holderName, initialBalance)
	{
		AccountType = AccountType.Current;
		if (initialBalance < MinimumBalance)
		{
			throw new ArgumentException($"Initial deposit for Current Account must be at least {MinimumBalance:C}.");
		}
	}

	public override void Deposit(decimal amount)
	{
		if (amount <= 0) throw new ArgumentException("Deposit amount must be positive.");
		Balance += amount;
	}

	public override void Withdraw(decimal amount)
	{
		if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive.");
		if (Balance - amount < MinimumBalance)
		{
			throw new InvalidOperationException($"Withdrawal denied. Minimum balance of {MinimumBalance:C} must be maintained.");
		}
		Balance -= amount;
	}

	public override void DisplayInfo()
	{
		Console.WriteLine($"[Current] Acc: {AccountNumber} | Name: {AccountHolderName} | Balance: {Balance:C} | Min Balance: {MinimumBalance:C}");
	}
}
#endregion


#region 4. Bank Service
public class BankService
{
	private readonly List<Account> _accounts = new();

	public void AddAccount(Account account)
	{
		account.AccountId = _accounts.Count + 1;
		account.AccountNumber = "ACC" + account.AccountId.ToString("D4");
		_accounts.Add(account);
	}

	public Account? SearchAccount(string accountNumber)
	{
		return _accounts.FirstOrDefault(a => a.AccountNumber.Equals(accountNumber, StringComparison.OrdinalIgnoreCase));
	}

	public decimal GetTotalBankBalance() => _accounts.Sum(a => a.Balance);

	public List<Account> GetAllAccounts() => _accounts;
}
#endregion