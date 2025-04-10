﻿using Microsoft.Extensions.Logging;
using MonriContactForm.Core.Entities;
using MonriContactForm.Core.Interfaces.HttpClients;
using MonriContactForm.Core.Interfaces.Repositories;
using MonriContactForm.Core.Interfaces.Services;
using MonriContactForm.Core.Models.Requests;
using MonriContactForm.Core.Models.UsersApi;

namespace MonriContactForm.Core.Services;

public class ContactFormService : IContactFormService
{
    private readonly IUsersRepository _usersRepository;
    private readonly IEmailService _emailService;
    private readonly IEmailTemplateRenderer _emailTemplateRenderer;
    private readonly IUsersClient _usersClient;
    private readonly ILogger<ContactFormService> _logger;

    public ContactFormService(
        IUsersRepository usersRepository,
        IEmailService emailService,
        IEmailTemplateRenderer emailTemplateRenderer,
        IUsersClient usersClient,
        ILogger<ContactFormService> logger)
    {
        _usersRepository = usersRepository;
        _emailService = emailService;
        _emailTemplateRenderer = emailTemplateRenderer;
        _usersClient = usersClient;
        _logger = logger;
    }

    public async Task ContactUserAsync(ContactUserRequest contactUserRequest)
    {
        _logger.LogInformation($"Processing contact user request: {contactUserRequest}");

        var existingUser = await _usersRepository.GetUserByEmailAsync(contactUserRequest.Email);
        var userDetails = await _usersClient.GetUserByEmailAsync(contactUserRequest.Email);

        var user = existingUser ?? new User();
        MapUserData(user, userDetails, contactUserRequest);

        if (existingUser is null)
        {
            await _usersRepository.CreateUserAsync(user);
        }
        else
        {
            await _usersRepository.UpdateUserAsync(user);
        }

        var emailContent = await _emailTemplateRenderer.RenderTemplateAsync("UserInfo", user);
        await _emailService.SendEmailAsync(user.Email, "User Information", emailContent);
    }

    private string? GetFormattedAddress(Address? address)
    {
        if (address is null)
        {
            return default;
        }

        return $"{address.Street}, {address.City}  {address.Zipcode}";
    }

    private void MapUserData(User user, UserDetails userDetails, ContactUserRequest contactUserRequest)
    {
        user.FirstName = contactUserRequest.FirstName;
        user.LastName = contactUserRequest.LastName;
        user.Email = contactUserRequest.Email;
        user.Username = userDetails?.Username ?? user.Username;
        user.Phone = userDetails?.Phone ?? user.Phone;
        user.Address = GetFormattedAddress(userDetails?.Address) ?? user.Address;
        user.Company = userDetails?.Company?.Name ?? user.Company;
        user.Website = userDetails?.Website ?? user.Website;
    }
}
