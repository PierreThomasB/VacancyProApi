using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacancyProAPI.Models;

public class Vacances
{
    [Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]public int IdVacances { get; set; }
    [Required]public string Nom { get; set; } = null!;
    [Required]public string Description { get; set; } = null!;
    [Required] public DateTime DateDebut { get; set; }
    [Required]public DateTime DateFin { get; set; }

    public virtual Activite[] Activite { get; set; } = null!;
    public virtual Lieux Lieux { get; set; } = null!;
    

}