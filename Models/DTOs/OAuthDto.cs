using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DTOs
{
    public class OAuthDto
    {
        [Required]
        public string Credentials { get; set; } = string.Empty;
    }
}

