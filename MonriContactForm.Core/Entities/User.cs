using System.ComponentModel.DataAnnotations;

namespace MonriContactForm.Core.Entities;

public class User
{
    public int Id { get; set; }

    [MaxLength(150)]
    public string FirstName { get; set; }

    [MaxLength(150)]
    public string LastName { get; set; }

    [MaxLength(150)]
    public string? Username { get; set; }

    [MaxLength(250)]
    public string Email { get; set; }

    [MaxLength(50)]
    public string? Phone { get; set; }

    [MaxLength(450)]
    public string? Website { get; set; }

    [MaxLength(250)]
    public string? Company { get; set; }

    [MaxLength(450)]
    public string? Address { get; set; }
}
