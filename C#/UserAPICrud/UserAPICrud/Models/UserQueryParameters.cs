using System.Web;

namespace UserAPICrud.Models;

public class UserQueryParameters
{
    #region Properties

    /// <summary>
    /// Page number for pagination (1-indexed).
    /// </summary>
    public int? Page { get; set; }

    /// <summary>
    /// Number of items per page.
    /// </summary>
    public int? Limit { get; set; }

    /// <summary>
    /// Search keyword to filter users by (matches username/email on MockAPI).
    /// </summary>
    public string? Search { get; set; }

    /// <summary>
    /// Field name to sort by (e.g. UserName, CreatedOn, UserID).
    /// </summary>
    public string? SortBy { get; set; }

    /// <summary>
    /// Sort direction: "asc" or "desc".
    /// </summary>
    public string? Order { get; set; }

    #endregion

    #region Helper Methods

    /// <summary>
    /// Builds the URL query string from non-null parameter properties.
    /// </summary>
    /// <returns>Query string starting with '?' if parameters exist, else empty string.</returns>
    public string ToQueryString()
    {
        var queryParams = new List<string>();

        if (Page.HasValue && Page.Value > 0)
        {
            queryParams.Add($"page={Page.Value}");
        }

        if (Limit.HasValue && Limit.Value > 0)
        {
            queryParams.Add($"limit={Limit.Value}");
        }

        if (!string.IsNullOrWhiteSpace(Search))
        {
            queryParams.Add($"search={HttpUtility.UrlEncode(Search.Trim())}");
        }

        if (!string.IsNullOrWhiteSpace(SortBy))
        {
            queryParams.Add($"sortBy={HttpUtility.UrlEncode(SortBy.Trim())}");
        }

        if (!string.IsNullOrWhiteSpace(Order))
        {
            queryParams.Add($"order={HttpUtility.UrlEncode(Order.Trim().ToLower())}");
        }

        return queryParams.Count > 0 ? "?" + string.Join("&", queryParams) : string.Empty;
    }

    #endregion
}
