using MonriContactForm.Core.Models.Requests;

namespace MonriContactForm.Core.Interfaces.Services;

public interface IContactFormService
{
    Task ContactUserAsync(ContactUserRequest contactUserRequest);
}
