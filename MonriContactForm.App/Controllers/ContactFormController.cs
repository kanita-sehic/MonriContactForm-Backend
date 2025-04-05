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

    /// <summary>
    /// Contacts a user (after collecting additional user's data) by sending an email.
    /// </summary>
    /// <param name="contactUserRequest">The request object containing the user's contact information.</param>
    /// <returns>Returns an OK response if the email is sent successfully.</returns>
    /// <response code="200">User contacted successfully</response>
    /// <response code="400">Invalid request data</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    public async Task<IActionResult> ContactUser([FromBody] ContactUserRequest contactUserRequest)
    {
        _logger.LogInformation($"Contact user called with request: {contactUserRequest}");

        await _contactFormService.ContactUserAsync(contactUserRequest);

        return Ok("User is successfully contacted.");
    }
}
