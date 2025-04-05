using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace MonriContactForm.Core.Models.Requests;

/// <summary>
/// Represents the data required to contact a user.
/// </summary>
public sealed class ContactUserRequest
{
    /// <summary>
    /// The first name of the user.
    /// </summary>
    /// <remarks>
    /// The first name can be up to 150 characters in length.
    /// </remarks>
    [StringLength(150)]
    public string FirstName { get; set; }

    /// <summary>
    /// The last name of the user.
    /// </summary>
    /// <remarks>
    /// The last name can be up to 150 characters in length.
    /// </remarks>
    [StringLength(150)]
    public string LastName { get; set; }

    /// <summary>
    /// The email address of the user to contact.
    /// </summary>
    /// <remarks>
    /// The email address should be in a valid format and cannot exceed 250 characters.
    /// </remarks>
    [EmailAddress]
    [StringLength(250)]
    public string Email { get; set; }

    /// <summary>
    /// Serializes the current object to a JSON string.
    /// </summary>
    /// <returns>A JSON string representing the contact user request.</returns>
    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}

