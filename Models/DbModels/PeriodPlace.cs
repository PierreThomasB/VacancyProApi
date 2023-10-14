namespace VacancyProAPI.Models.DbModels
{
    public class PeriodPlace : IPlace
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public int PostalCode { get; set; }
        public string Number { get; set; }
        public string Locality { get; set; }
        public string Country { get; set; }
        
        public PeriodPlace() {}

        public PeriodPlace(string street, int postalCode, string number, string locality, string country)
        {
            Street = street;
            PostalCode = postalCode;
            Number = number;
            Locality = locality;
            Country = country;
        }

        
    }
}

