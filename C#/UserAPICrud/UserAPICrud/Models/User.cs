using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

namespace UserAPICrud.Models;

public class User
{
    #region Properties

    [JsonPropertyName("UserID")]
    public string UserID { get; set; } = string.Empty;

    [JsonPropertyName("UserName")]
    public string UserName { get; set; } = string.Empty;

    [JsonPropertyName("UserEmail")]
    public string UserEmail { get; set; } = string.Empty;

    [JsonPropertyName("PhoneNumber")]
    public string PhoneNumber { get; set; } = string.Empty;

    [JsonPropertyName("CreatedOn")]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    [JsonPropertyName("UpdatedOn")]
    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;

    #endregion

    #region Validation Methods

    /// <summary>
    /// Validates the User instance properties.
    /// </summary>
    /// <returns>A list of validation error messages. Empty list if valid.</returns>
    public List<string> Validate()
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(UserName))
        {
            errors.Add("UserName is required.");
        }
        else if (UserName.Trim().Length < 2)
        {
            errors.Add("UserName must be at least 2 characters long.");
        }

        if (string.IsNullOrWhiteSpace(UserEmail))
        {
            errors.Add("UserEmail is required.");
        }
        else
        {
            // Simple robust regex for email validation
            var emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            if (!Regex.IsMatch(UserEmail.Trim(), emailPattern, RegexOptions.IgnoreCase))
            {
                errors.Add("UserEmail is not a valid email address.");
            }
        }

        if (string.IsNullOrWhiteSpace(PhoneNumber))
        {
            errors.Add("PhoneNumber is required.");
        }
        else
        {
            var phonePattern = @"^\+?[0-9\s\-\.\(\)]+(?:\s*[xX]\s*[0-9]+)?$";
            var digitsCount = PhoneNumber.Count(char.IsDigit);
            if (!Regex.IsMatch(PhoneNumber.Trim(), phonePattern) || digitsCount < 7)
            {
                errors.Add("PhoneNumber is invalid. Must contain valid phone format with at least 7 digits.");
            }
        }

        return errors;
    }

    #endregion
}
