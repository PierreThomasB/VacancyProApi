using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DbModels;

public class Notification
{
    [Key]
    public int Id { get; set; }
    [Required]
    
    public User user { get; set; }

    [Required] public string contenu { get; set; } = string.Empty;
}