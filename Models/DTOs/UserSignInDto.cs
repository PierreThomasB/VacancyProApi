using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DTOs
{
    public class UserSignInDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

    }
}

