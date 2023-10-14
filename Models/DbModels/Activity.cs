namespace VacancyProAPI.Models.DbModels
{
    public class Activity
    {
        public Activity() {}
        public Activity(string name, string description, DateTime beginDate, DateTime endDate, ActivityPlace activityPlace)
        {
            Name = name;
            Description = description;
            BeginDate = beginDate;
            EndDate = endDate;
            ActivityPlace = activityPlace;
        }
        
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public virtual ActivityPlace ActivityPlace { get; set; }

        
    }
}

