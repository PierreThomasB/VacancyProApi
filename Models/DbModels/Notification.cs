using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DbModels;

public class Notification
{
    [Key]
    public int Id { get; set; }
    [Required]
    
    public User User { get; set; }
    
    [Required] public string Contenu { get; set; } = string.Empty;

    public DateTime Date { get; set; } 

    public Notification()
    {
    }
}