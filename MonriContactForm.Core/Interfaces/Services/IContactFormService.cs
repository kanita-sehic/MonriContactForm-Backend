using MonriContactForm.Core.Models.Requests;

namespace MonriContactForm.Core.Interfaces.Services;

/// <summary>
/// Interface for services related to contacting users.
/// </summary>
public interface IContactFormService
{
    /// <summary>
    /// Sends a contact message to the user.
    /// </summary>
    /// <param name="contactUserRequest">The details of the contact request, including the recipient's information.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    Task ContactUserAsync(ContactUserRequest contactUserRequest);
}
