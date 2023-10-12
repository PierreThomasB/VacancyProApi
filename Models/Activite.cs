namespace VacancyProAPI.Models;

public class Activite
{
    public string Nom { get; set; } = null!;
    public string Prenom { get; set; } = null!;
    public DateTime JourDebut { get; set; }
    public DateTime JourFin { get; set; }
}