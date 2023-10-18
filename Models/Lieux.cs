using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacancyProAPI.Models;

public class Lieux
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]public int IdLieu { get; set; }
    [Required]public string Lieu { get; set; } = null!;
    public string Description { get; set; } = null!;
}