using System.Text.RegularExpressions;
using UserAPICrud.Models;
using UserAPICrud.Services;

namespace UserAPICrud;

internal class Program
{
    private static readonly IUserService UserService = new UserService();

    #region Main Entry

    static async Task Main(string[] args)
    {
        while (true)
        {
            ShowMainMenu();
            var inputLine = Console.ReadLine();
            if (inputLine == null)
            {
                // EOF reached (e.g. redirected input stream ended)
                break;
            }
            var choice = inputLine.Trim();

            Console.WriteLine();
            switch (choice)
            {
                case "1":
                    await HandleGetAllUsersAsync();
                    break;
                case "2":
                    await HandleGetUserByIdAsync();
                    break;
                case "3":
                    await HandleCreateUserAsync();
                    break;
                case "4":
                    await HandleUpdateUserAsync();
                    break;
                case "5":
                    await HandleDeleteUserAsync();
                    break;
                case "6":
                    await RunAutomatedTestSuiteAsync();
                    break;
                case "0":
                    Console.WriteLine("Exiting application. Goodbye!");
                    return;
                default:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Invalid choice. Please select an option from the menu.");
                    Console.ResetColor();
                    break;
            }

            Console.WriteLine("\nPress any key to return to the menu...");
            if (!Console.IsInputRedirected)
            {
                Console.ReadKey();
            }
        }
    }

    #endregion

    #region Menu Display

    private static void ShowMainMenu()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("           USER API MANAGEMENT SERVICE            ");
        Console.WriteLine("==================================================");
        Console.ResetColor();
        Console.WriteLine(" 1. Get All Users (With Filters & Pagination)");
        Console.WriteLine(" 2. Get User By ID");
        Console.WriteLine(" 3. Create User (With Validation)");
        Console.WriteLine(" 4. Update User Details (Updates UpdatedOn date)");
        Console.WriteLine(" 5. Delete User");
        Console.WriteLine(" 6. Run Automated Demonstration & Test Suite");
        Console.WriteLine(" 0. Exit");
        Console.WriteLine("--------------------------------------------------");
        Console.Write("Enter your choice: ");
    }

    #endregion

    #region Menu Handlers

    private static async Task HandleGetAllUsersAsync()
    {
        #region Read Options
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("--- Get Users Filter & Pagination Options ---");
        Console.ResetColor();

    GetPageNumber:
        Console.Write("Page number (press Enter to skip): ");
        var pageInput = Console.ReadLine()?.Trim();
        int? page = null;
        if (!string.IsNullOrEmpty(pageInput))
        {
            if (!int.TryParse(pageInput, out var parsedPage) || parsedPage <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Validation Error: Page number must be a positive integer.");
                Console.ResetColor();
                goto GetPageNumber;
            }
            page = parsedPage;
        }

    GetLimitNumber:
        Console.Write("Limit per page (press Enter to skip): ");
        var limitInput = Console.ReadLine()?.Trim();
        int? limit = null;
        if (!string.IsNullOrEmpty(limitInput))
        {
            if (!int.TryParse(limitInput, out var parsedLimit) || parsedLimit <= 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Validation Error: Limit per page must be a positive integer.");
                Console.ResetColor();
                goto GetLimitNumber;
            }
            limit = parsedLimit;
        }

        Console.Write("Search keyword (matches username/email, press Enter to skip): ");
        var search = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(search)) search = null;

    GetSortByField:
        Console.Write("Sort By field (e.g. UserName, UserEmail, PhoneNumber, CreatedOn, UpdatedOn, UserID, press Enter to skip): ");
        var sortBy = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(sortBy))
        {
            sortBy = null;
        }
        else
        {
            var validFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "UserID", "UserName", "UserEmail", "PhoneNumber", "CreatedOn", "UpdatedOn"
            };
            if (!validFields.Contains(sortBy))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"Validation Error: Invalid sort field '{sortBy}'. Allowed fields: UserID, UserName, UserEmail, PhoneNumber, CreatedOn, UpdatedOn.");
                Console.ResetColor();
                goto GetSortByField;
            }
        }

    GetOrderDirection:
        Console.Write("Order direction (asc/desc, press Enter to skip): ");
        var order = Console.ReadLine()?.Trim();
        if (string.IsNullOrEmpty(order))
        {
            order = null;
        }
        else if (!order.Equals("asc", StringComparison.OrdinalIgnoreCase) && !order.Equals("desc", StringComparison.OrdinalIgnoreCase))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Validation Error: Invalid order direction '{order}'. Must be either 'asc' or 'desc'.");
            Console.ResetColor();
            goto GetOrderDirection;
        }

        var queryParams = new UserQueryParameters
        {
            Page = page,
            Limit = limit,
            Search = search,
            SortBy = sortBy,
            Order = order
        };
        #endregion

        Console.WriteLine("\nFetching users from API...");
        var users = await UserService.GetAllUsersAsync(queryParams);

        DisplayUserTable(users);
    }

    private static async Task HandleGetUserByIdAsync()
    {
    GetUserId:
        Console.Write("Enter User ID: ");
        var id = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(id))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: User ID cannot be empty.");
            Console.ResetColor();
            goto GetUserId;
        }

        Console.WriteLine($"\nFetching user with ID {id}...");
        var user = await UserService.GetUserByIdAsync(id);

        if (user == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"User with ID '{id}' was not found.");
            Console.ResetColor();
        }
        else
        {
            DisplaySingleUser(user);
        }
    }

    private static async Task HandleCreateUserAsync()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("--- Create New User ---");
        Console.ResetColor();

    GetUserName:
        Console.Write("Enter Username: ");
        var userName = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userName))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: Username is required.");
            Console.ResetColor();
            goto GetUserName;
        }
        if (userName.Length < 2)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: Username must be at least 2 characters long.");
            Console.ResetColor();
            goto GetUserName;
        }

    GetUserEmail:
        Console.Write("Enter Email: ");
        var userEmail = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(userEmail))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: Email is required.");
            Console.ResetColor();
            goto GetUserEmail;
        }
        if (!Regex.IsMatch(userEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: UserEmail is not a valid email address.");
            Console.ResetColor();
            goto GetUserEmail;
        }

    GetPhoneNumber:
        Console.Write("Enter Phone Number: ");
        var phoneNumber = Console.ReadLine()?.Trim() ?? string.Empty;
        if (string.IsNullOrWhiteSpace(phoneNumber))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: Phone Number is required.");
            Console.ResetColor();
            goto GetPhoneNumber;
        }
        var phonePattern = @"^\+?[0-9\s\-\.\(\)]+(?:\s*[xX]\s*[0-9]+)?$";
        if (!Regex.IsMatch(phoneNumber, phonePattern) || phoneNumber.Count(char.IsDigit) < 7)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: Invalid Phone Number format. Must contain valid digits (minimum 7 digits).");
            Console.ResetColor();
            goto GetPhoneNumber;
        }

        var newUser = new User
        {
            UserName = userName,
            UserEmail = userEmail,
            PhoneNumber = phoneNumber
        };

        Console.WriteLine("\nSending create request to API...");
        try
        {
            var createdUser = await UserService.CreateUserAsync(newUser);
            if (createdUser != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nUser created successfully!");
                Console.ResetColor();
                DisplaySingleUser(createdUser);
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to create user: {ex.Message}");
            Console.ResetColor();
        }
    }

    private static async Task HandleUpdateUserAsync()
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("--- Update User Details ---");
        Console.ResetColor();

    GetUpdateId:
        Console.Write("Enter User ID to update: ");
        var id = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(id))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: User ID cannot be empty.");
            Console.ResetColor();
            goto GetUpdateId;
        }

        var existingUser = await UserService.GetUserByIdAsync(id);
        if (existingUser == null)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"User with ID '{id}' not found.");
            Console.ResetColor();
            goto GetUpdateId;
        }

        Console.WriteLine($"Currently editing User ID: {existingUser.UserID} (CreatedOn: {existingUser.CreatedOn:g})");

    GetUpdateUserName:
        Console.Write($"Enter New Username [{existingUser.UserName}]: ");
        var userNameInput = Console.ReadLine()?.Trim();
        var newUserName = string.IsNullOrWhiteSpace(userNameInput) ? existingUser.UserName : userNameInput;
        if (newUserName.Length < 2)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: Username must be at least 2 characters long.");
            Console.ResetColor();
            goto GetUpdateUserName;
        }

    GetUpdateUserEmail:
        Console.Write($"Enter New Email [{existingUser.UserEmail}]: ");
        var userEmailInput = Console.ReadLine()?.Trim();
        var newUserEmail = string.IsNullOrWhiteSpace(userEmailInput) ? existingUser.UserEmail : userEmailInput;
        if (!Regex.IsMatch(newUserEmail, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: UserEmail is not a valid email address.");
            Console.ResetColor();
            goto GetUpdateUserEmail;
        }

    GetUpdatePhone:
        Console.Write($"Enter New Phone Number [{existingUser.PhoneNumber}]: ");
        var phoneInput = Console.ReadLine()?.Trim();
        var newPhone = string.IsNullOrWhiteSpace(phoneInput) ? existingUser.PhoneNumber : phoneInput;
        if (string.IsNullOrWhiteSpace(newPhone))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: Phone Number is required.");
            Console.ResetColor();
            goto GetUpdatePhone;
        }
        if (!Regex.IsMatch(newPhone, @"^\+?[0-9\s\-\.\(\)]+(?:\s*[xX]\s*[0-9]+)?$") || newPhone.Count(char.IsDigit) < 7)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: Invalid Phone Number format. Must contain valid digits (minimum 7 digits).");
            Console.ResetColor();
            goto GetUpdatePhone;
        }

        var updatedUserPayload = new User
        {
            UserID = existingUser.UserID,
            UserName = newUserName,
            UserEmail = newUserEmail,
            PhoneNumber = newPhone,
            CreatedOn = existingUser.CreatedOn
        };

        Console.WriteLine("\nSending update request to API...");
        try
        {
            var updatedUser = await UserService.UpdateUserAsync(id, updatedUserPayload);
            if (updatedUser != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nUser updated successfully!");
                Console.ResetColor();
                DisplaySingleUser(updatedUser);
            }
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to update user: {ex.Message}");
            Console.ResetColor();
        }
    }

    private static async Task HandleDeleteUserAsync()
    {
    GetDeleteId:
        Console.Write("Enter User ID to delete: ");
        var id = Console.ReadLine()?.Trim();

        if (string.IsNullOrWhiteSpace(id))
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Validation Error: User ID cannot be empty.");
            Console.ResetColor();
            goto GetDeleteId;
        }

        Console.WriteLine($"Deleting user with ID '{id}'...");
        var success = await UserService.DeleteUserAsync(id);

        if (success)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"User with ID '{id}' was successfully deleted.");
            Console.ResetColor();
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Failed to delete user with ID '{id}' (User not found).");
            Console.ResetColor();
        }
    }

    #endregion

    #region Automated Test Suite

    private static async Task RunAutomatedTestSuiteAsync()
    {
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("==================================================");
        Console.WriteLine("      RUNNING AUTOMATED TEST SUITE & DEMO         ");
        Console.WriteLine("==================================================");
        Console.ResetColor();

        #region Test 1: Get All Users
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[Test 1] Fetching all users (First 5 users)...");
        Console.ResetColor();
        var allUsers = await UserService.GetAllUsersAsync(new UserQueryParameters { Limit = 5 });
        DisplayUserTable(allUsers);
        #endregion

        #region Test 2: Pagination Test
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[Test 2] Testing Pagination (Page 2, Limit 3)...");
        Console.ResetColor();
        var page2Users = await UserService.GetAllUsersAsync(new UserQueryParameters { Page = 2, Limit = 3 });
        DisplayUserTable(page2Users);
        #endregion

        #region Test 3: Filter & Search Test
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[Test 3] Testing Search Filter (Search: 'yahoo')...");
        Console.ResetColor();
        var filteredUsers = await UserService.GetAllUsersAsync(new UserQueryParameters { Search = "yahoo" });
        DisplayUserTable(filteredUsers);
        #endregion

        #region Test 4: Validation Handling Test
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[Test 4] Testing User Creation Validation (Invalid User Payload)...");
        Console.ResetColor();
        var invalidUser = new User
        {
            UserName = "A", // too short (< 2)
            UserEmail = "invalid-email-address", // invalid email
            PhoneNumber = "" // empty
        };
        var validationErrors = invalidUser.Validate();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Caught expected validation errors:");
        foreach (var err in validationErrors)
        {
            Console.WriteLine($" - {err}");
        }
        Console.ResetColor();
        #endregion

        #region Test 5: Create User Test
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n[Test 5] Testing User Creation (Valid User)...");
        Console.ResetColor();
        var newTestUser = new User
        {
            UserName = "TestUser_" + Guid.NewGuid().ToString("N")[..5],
            UserEmail = "testuser@example.com",
            PhoneNumber = "555-0199"
        };
        var createdUser = await UserService.CreateUserAsync(newTestUser);
        if (createdUser != null)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Created user successfully:");
            Console.ResetColor();
            DisplaySingleUser(createdUser);

            #region Test 6: Update User Test
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[Test 6] Testing User Update & UpdatedOn Timestamp Refresh for ID {createdUser.UserID}...");
            Console.ResetColor();
            
            // Wait brief moment to guarantee updated timestamp is visually different
            await Task.Delay(1000);
            
            createdUser.UserName += "_Updated";
            createdUser.UserEmail = "updated_testuser@example.com";
            
            var updatedUser = await UserService.UpdateUserAsync(createdUser.UserID, createdUser);
            if (updatedUser != null)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Updated user successfully (Note refreshed UpdatedOn date):");
                Console.ResetColor();
                DisplaySingleUser(updatedUser);
            }
            #endregion

            #region Test 7: Cleanup Created User
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"\n[Test 7] Cleaning up created test user (ID: {createdUser.UserID})...");
            Console.ResetColor();
            var deleted = await UserService.DeleteUserAsync(createdUser.UserID);
            if (deleted)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Test user ID {createdUser.UserID} deleted successfully.");
                Console.ResetColor();
            }
            #endregion
        }
        #endregion

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("\n==================================================");
        Console.WriteLine("        AUTOMATED TEST SUITE COMPLETED            ");
        Console.WriteLine("==================================================");
        Console.ResetColor();
    }

    #endregion

    #region Display Helpers

    private static void DisplayUserTable(List<User> users)
    {
        if (users == null || users.Count == 0)
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("No user records found.");
            Console.ResetColor();
            return;
        }

        Console.WriteLine(new string('-', 115));
        Console.WriteLine($"{"ID",-6} | {"UserName",-20} | {"UserEmail",-30} | {"PhoneNumber",-22} | {"UpdatedOn",-20}");
        Console.WriteLine(new string('-', 115));

        foreach (var u in users)
        {
            Console.WriteLine($"{u.UserID,-6} | {Truncate(u.UserName, 20),-20} | {Truncate(u.UserEmail, 30),-30} | {Truncate(u.PhoneNumber, 22),-22} | {u.UpdatedOn:yyyy-MM-dd HH:mm:ss}");
        }

        Console.WriteLine(new string('-', 115));
        Console.WriteLine($"Total Count: {users.Count}\n");
    }

    private static void DisplaySingleUser(User user)
    {
        Console.WriteLine(new string('=', 60));
        Console.WriteLine($"User ID     : {user.UserID}");
        Console.WriteLine($"UserName    : {user.UserName}");
        Console.WriteLine($"UserEmail   : {user.UserEmail}");
        Console.WriteLine($"PhoneNumber : {user.PhoneNumber}");
        Console.WriteLine($"CreatedOn   : {user.CreatedOn:yyyy-MM-dd HH:mm:ss.fff UTC}");
        Console.WriteLine($"UpdatedOn   : {user.UpdatedOn:yyyy-MM-dd HH:mm:ss.fff UTC}");
        Console.WriteLine(new string('=', 60));
    }

    private static string Truncate(string value, int maxLength)
    {
        if (string.IsNullOrEmpty(value)) return string.Empty;
        return value.Length <= maxLength ? value : value[..(maxLength - 3)] + "...";
    }

    #endregion
}
