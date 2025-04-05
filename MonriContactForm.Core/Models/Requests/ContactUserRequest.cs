using System.ComponentModel.DataAnnotations;
using System.Text.Json;

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

    public override string ToString()
    {
        return JsonSerializer.Serialize(this);
    }
}
