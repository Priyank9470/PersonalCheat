using System.Text;
using System.Text.Json;
using UserAPICrud.Models;

namespace UserAPICrud.Services;

public class UserService : IUserService
{
    #region Constants & Fields

    /// <summary>
    /// Base URL for the Mock API user endpoint.
    /// </summary>
    private const string BaseUrl = "https://6a360994766b831960f8e6ce.mockapi.io/api/Users";

    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    #endregion

    #region Constructors

    public UserService(HttpClient? httpClient = null)
    {
        _httpClient = httpClient ?? new HttpClient();
        _jsonOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        };
    }

    #endregion

    #region API Operations

    /// <inheritdoc />
    public async Task<List<User>> GetAllUsersAsync(UserQueryParameters? queryParams = null)
    {
        try
        {
            var queryString = queryParams?.ToQueryString() ?? string.Empty;
            var requestUrl = $"{BaseUrl}{queryString}";

            var response = await _httpClient.GetAsync(requestUrl);
            
            // MockAPI returns 404 if page requested is out of range or no matches found
            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return new List<User>();
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var users = JsonSerializer.Deserialize<List<User>>(content, _jsonOptions);
            return users ?? new List<User>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API Error - GetAllUsers]: {ex.Message}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<User?> GetUserByIdAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(id));
        }

        try
        {
            var requestUrl = $"{BaseUrl}/{id}";
            var response = await _httpClient.GetAsync(requestUrl);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(content, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API Error - GetUserById]: {ex.Message}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<User?> CreateUserAsync(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        #region Validation Check
        var validationErrors = user.Validate();
        if (validationErrors.Count > 0)
        {
            var errorMsg = string.Join("; ", validationErrors);
            throw new InvalidOperationException($"User validation failed: {errorMsg}");
        }
        #endregion

        // Set initial timestamps
        var now = DateTime.UtcNow;
        user.CreatedOn = now;
        user.UpdatedOn = now;

        try
        {
            var jsonPayload = JsonSerializer.Serialize(user, _jsonOptions);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(BaseUrl, httpContent);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseContent, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API Error - CreateUser]: {ex.Message}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<User?> UpdateUserAsync(string id, User user)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(id));
        }

        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        #region Validation Check
        var validationErrors = user.Validate();
        if (validationErrors.Count > 0)
        {
            var errorMsg = string.Join("; ", validationErrors);
            throw new InvalidOperationException($"User validation failed: {errorMsg}");
        }
        #endregion

        // Always update the UpdatedOn date on user modification
        user.UpdatedOn = DateTime.UtcNow;

        try
        {
            var requestUrl = $"{BaseUrl}/{id}";
            var jsonPayload = JsonSerializer.Serialize(user, _jsonOptions);
            var httpContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(requestUrl, httpContent);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<User>(responseContent, _jsonOptions);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API Error - UpdateUser]: {ex.Message}");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task<bool> DeleteUserAsync(string id)
    {
        if (string.IsNullOrWhiteSpace(id))
        {
            throw new ArgumentException("User ID cannot be null or empty.", nameof(id));
        }

        try
        {
            var requestUrl = $"{BaseUrl}/{id}";
            var response = await _httpClient.DeleteAsync(requestUrl);

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[API Error - DeleteUser]: {ex.Message}");
            throw;
        }
    }

    #endregion
}
