using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DbModels
{
    public class Period
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public Place Place { get; set; } = null!;

     

        //public User Creator { get; set; } = null!;
        //public IEnumerable<User> ListUser { get; set; } = null!;
        //public IEnumerable<Activity> ListActivity { get; set; } = null!;

        public Period() {}
        
        public Period(string name, string description, DateTime beginDate, DateTime endDate, Place place )
        {
            Name = name;
            Description = description;
            BeginDate = beginDate;
            EndDate = endDate;
            //Creator = creator;
            Place = place;
            //ListUser = new HashSet<User>();
            //ListActivity = new List<Activity>();
        }
    }
}