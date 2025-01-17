﻿using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DTOs;

public class ContactDto
{
    [Required] public string LastName { get; set; } = string.Empty;
    [Required] public string FirstName { get; set; } = string.Empty;
    [Required] [EmailAddress] public string? Email { get; set; } = string.Empty;
    [Required] public string Subject { get; set; } = string.Empty;
    [Required] public string Message { get; set; } = string.Empty;
}