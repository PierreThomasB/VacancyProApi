using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DTOs
{
    public class UsernameDto
    {
        [Required]
        public string Username { get; set; } = String.Empty;
    }
}

