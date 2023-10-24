﻿using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DTOs
{
    public class UserSignUpDto
    {
        [Required]
        public string LastName { get; set; } = string.Empty;
        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required] [EmailAddress] 
        public string Email { get; set; } = string.Empty;
            
        public string Password { get; set; } = string.Empty;
    }
}
