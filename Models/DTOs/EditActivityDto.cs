namespace VacancyProAPI.Models.DTOs;

public class EditActivityDto
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime BeginDate { get; set; }
    public DateTime EndDate { get; set; }
    
}