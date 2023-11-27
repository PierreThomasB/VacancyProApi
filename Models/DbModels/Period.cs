using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace VacancyProAPI.Models.DbModels
{
    public class Period
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Description { get; set; } = null!;
        [Required]
        public DateTime BeginDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public Place Place { get; set; } = null!;
        
        //[Required]
        //public User Creator { get; set; }
        
       
        public  List<User> ListUser { get; set; } = new();
       

        public Period() {}
        
        public Period(string name, string description, DateTime beginDate, DateTime endDate, Place place )
        {
            Name = name;
            Description = description;
            BeginDate = beginDate;
            EndDate = endDate;
            Place = place;
           
        }
    }
}