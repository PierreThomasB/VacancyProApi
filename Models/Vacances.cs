namespace VacancyProAPI.Models;

public class Vacances
{
    public string Nom { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DateDebut { get; set; }
    public DateTime DateFin { get; set; }
    
}