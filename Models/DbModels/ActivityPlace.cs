namespace VacancyProAPI.Models.DbModels;

public class ActivityPlace : IPlace
{
    public int Id { get; set; }
    public string Street { get; set; }
    public int PostalCode { get; set; }
    public string Number { get; set; }
    public string Locality { get; set; }
    
    public ActivityPlace() {}

    public ActivityPlace(string street, int postalCode, string number, string locality)
    {
        Street = street;
        PostalCode = postalCode;
        Number = number;
        Locality = locality;
    }
}