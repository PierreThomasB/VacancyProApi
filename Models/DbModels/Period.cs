namespace VacancyProAPI.Models.DbModels
{
    public class Period
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual PeriodPlace PeriodPlace { get; set; }
        
        public virtual User Creator { get; set; }
        public virtual IList<User> ListUser { get; set; }
        public virtual IList<Activity> ListActivity { get; set; }
        
        public Period() {}
        
        public Period(string name, string description, DateTime beginDate, DateTime endDate, User creator, PeriodPlace periodPlace)
        {
            Name = name;
            Description = description;
            BeginDate = beginDate;
            EndDate = endDate;
            Creator = creator;
            PeriodPlace = periodPlace;

            ListUser = new List<User>();
            ListActivity = new List<Activity>();
        }
    }
}

