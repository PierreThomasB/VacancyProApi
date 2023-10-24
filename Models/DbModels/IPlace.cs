namespace VacancyProAPI.Models.DbModels;

public interface IPlace
{
    public int Id { get; set; }
    public string Street { get; set; }
    public int PostalCode { get; set; }
    public string Number { get; set; }
    public string Locality { get; set; }
}