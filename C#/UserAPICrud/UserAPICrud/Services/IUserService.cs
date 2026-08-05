using UserAPICrud.Models;

namespace UserAPICrud.Services;

public interface IUserService
{
    #region Methods

    /// <summary>
    /// Fetches all users from MockAPI with optional filtering and pagination.
    /// </summary>
    Task<List<User>> GetAllUsersAsync(UserQueryParameters? queryParams = null);

    /// <summary>
    /// Fetches a single user by their UserID.
    /// </summary>
    Task<User?> GetUserByIdAsync(string id);

    /// <summary>
    /// Creates a new user after running validations and setting timestamps.
    /// </summary>
    Task<User?> CreateUserAsync(User user);

    /// <summary>
    /// Updates an existing user after running validations and updating the UpdatedOn timestamp.
    /// </summary>
    Task<User?> UpdateUserAsync(string id, User user);

    /// <summary>
    /// Deletes a user by UserID.
    /// </summary>
    Task<bool> DeleteUserAsync(string id);

    #endregion
}
