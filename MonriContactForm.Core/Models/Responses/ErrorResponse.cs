namespace MonriContactForm.Core.Models.Responses;

/// <summary>
/// Represents an error response returned by the API.
/// </summary>
public class ErrorResponse
{
    /// <summary>
    /// A detailed error message describing what went wrong.
    /// </summary>
    public string Message { get; set; }

    /// <summary>
    /// The HTTP status code associated with the error.
    /// </summary>
    public int StatusCode { get; set; }
}

