using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DTOs
{
    public class DateDto
    {
        [Required]
        public DateTime Date { get; set; }
    }
}

