using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DbModels;

public class Chat
{
    
    [Key]
    public int Id { get; set; }

    [Required] public string Message { get; set; } = null!;

    [Required] public string Channel { get; set; } = null!;
    
    [Required] public DateTime Date { get; set; }
    
    [Required] public User User { get; set; } 


    public Chat()
    {
    }
}