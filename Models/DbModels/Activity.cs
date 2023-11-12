using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DbModels
{
    public class Activity
    {
     
        public Activity(string name, string description, DateTime beginDate, DateTime endDate, string activityPlace, Period period)
        {
            Name = name;
            Description = description;
            BeginDate = beginDate;
            EndDate = endDate;
            Place = activityPlace;
            Period = period;
        }
        
        [Key]
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public DateTime BeginDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public string Place { get; set; }
        [Required]
        public Period Period { get; set; }

        
    }
}

