using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DbModels
{
    public class Period
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Place { get; set; }
        
        public User Creator { get; set; }
        public IEnumerable<User> ListUser { get; set; }
        public IEnumerable<Activity> ListActivity { get; set; }
        
      
        
        public Period(string name, string description, DateTime beginDate, DateTime endDate, User creator, string place)
        {
            Name = name;
            Description = description;
            BeginDate = beginDate;
            EndDate = endDate;
            Creator = creator;
            Place = place;

            ListUser = new HashSet<User>();
            ListActivity = new List<Activity>();
        }
    }
}

