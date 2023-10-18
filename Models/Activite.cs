using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacancyProAPI.Models;

public class Activite
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]public int IdActivite { get; set; }
    [Required]public string Nom { get; set; } = null!;
    [Required]public string Description { get; set; } = null!;
    [Required] public virtual Lieux Lieux { get; set; } = null!;

}