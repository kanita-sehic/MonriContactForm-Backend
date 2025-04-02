using System.ComponentModel.DataAnnotations;

namespace MonriContactForm.Core.Models.Requests;

public sealed class ContactUserRequest
{
    [StringLength(150)]
    public string FirstName { get; set; }

    [StringLength(150)]
    public string LastName { get; set; }

    [EmailAddress]
    [StringLength(250)]
    public string Email { get; set; }
}
