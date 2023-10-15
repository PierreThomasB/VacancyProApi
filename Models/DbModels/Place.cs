using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DbModels
{
    public class Place
    {
        [Key]
        public int Id { get; set; }
        public string Street { get; set; }
        public int PostalCode { get; set; }
        public string Number { get; set; }
        public string Locality { get; set; }
        public string Country { get; set; }
        
        public Place() {}

        public Place(string street, int postalCode, string number, string locality, string country)
        {
            Street = street;
            PostalCode = postalCode;
            Number = number;
            Locality = locality;
            Country = country;
        }

        
    }
}

