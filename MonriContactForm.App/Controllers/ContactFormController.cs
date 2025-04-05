using Microsoft.AspNetCore.Mvc;
using MonriContactForm.Core.Interfaces.Services;
using MonriContactForm.Core.Models.Requests;

namespace MonriContactForm.App.Controllers;

[Route("contact-form")]
[ApiController]
public class ContactFormController : Controller
{
    private readonly IContactFormService _contactFormService;
    private readonly ILogger<ContactFormController> _logger;

    public ContactFormController(IContactFormService contactFormService, ILogger<ContactFormController> logger)
    {
        _contactFormService = contactFormService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> ContactUser([FromBody] ContactUserRequest contactUserRequest)
    {
        _logger.LogInformation($"Contact user called with request: {contactUserRequest}");

        await _contactFormService.ContactUserAsync(contactUserRequest);

        return Ok("User is successfully contacted.");
    }
}
