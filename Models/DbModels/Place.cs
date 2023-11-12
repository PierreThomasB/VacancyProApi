using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DbModels;

public class Place
{
    [Key] public string Id { get; set; } = null!;
    [Required]public string Name { get; set; } = null!;
    public string UrlPhoto { get; set; } = null!;

}