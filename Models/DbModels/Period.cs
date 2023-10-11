namespace VacancyProAPI.Models.DbModels
{
    public class Period
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public Place Place { get; set; }
        
        public User Creator { get; set; }
        public IList<User> ListUser { get; set; }
        public IList<Activity> ListActivity { get; set; }

        public Period(string name, string description, DateTime beginDate, DateTime endDate, User creator, Place place)
        {
            Name = name;
            Description = description;
            BeginDate = beginDate;
            EndDate = endDate;
            Creator = creator;
            Place = place;

            ListUser = new List<User>();
            ListActivity = new List<Activity>();
        }
    }
}

