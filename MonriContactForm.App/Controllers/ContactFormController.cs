using Microsoft.AspNetCore.Mvc;
using MonriContactForm.Core.Interfaces.Services;
using MonriContactForm.Core.Models.Requests;

namespace MonriContactForm.App.Controllers;

[Route("contact-form")]
[ApiController]
public class ContactFormController : Controller
{
    private readonly IContactFormService _contactFormService;

    public ContactFormController(IContactFormService contactFormService)
    {
        _contactFormService = contactFormService;
    }

    [HttpPost]
    public async Task<IActionResult> ContactUser([FromBody] ContactUserRequest contactUserRequest)
    {
        await _contactFormService.ContactUserAsync(contactUserRequest);
        return NoContent();
    }
}
