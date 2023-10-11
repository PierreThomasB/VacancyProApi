namespace VacancyProAPI.Models.DbModels
{
    public class Activity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime BeginDate { get; set; }
        public DateTime EndDate { get; set; }
        public Place Place { get; set; }

        public Activity(string name, string description, DateTime beginDate, DateTime endDate, Place place)
        {
            Name = name;
            Description = description;
            BeginDate = beginDate;
            EndDate = endDate;
            Place = place;
        }
    }
}

