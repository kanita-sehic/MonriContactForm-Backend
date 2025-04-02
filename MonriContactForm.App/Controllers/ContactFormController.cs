using Microsoft.AspNetCore.Mvc;
using MonriContactForm.Core.Interfaces.Repositories;
using MonriContactForm.Core.Interfaces.Services;
using MonriContactForm.Core.Models.Requests;

namespace MonriContactForm.App.Controllers;

[Route("contact-form")]
[ApiController]
public class ContactFormController : Controller
{
    private readonly IContactFormService _contactFormService;
    private readonly IUsersRepository _usersRepository;

    public ContactFormController(IContactFormService contactFormService, IUsersRepository usersRepository)
    {
        _contactFormService = contactFormService;
        _usersRepository = usersRepository;
    }

    [HttpPost]
    public async Task<IActionResult> ContactUser([FromBody] ContactUserRequest contactUserRequest)
    {
        return Ok();
    }

    // should be deleted
    [HttpGet]
    public async Task<IActionResult> GetUserDetails([FromQuery] string email)
    {
        var user = await _usersRepository.GetUserByEmailAsync(email);
        return Ok(user);
    } 
}
