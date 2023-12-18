using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace VacancyProAPI.Models;

public class AnonymUser
{
    [Key][DatabaseGenerated(DatabaseGeneratedOption.Identity)]public int IdAnonym { get; set; }
    [EmailAddress][Required] public string Email { get; set; } = null!;
    [Required] public string Sujet { get; set; } = null!;
    [Required] public string Description { get; set; } = null!;
    [Required] public bool IsResolve { get; set; } = false;

}