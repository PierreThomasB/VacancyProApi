using System.ComponentModel.DataAnnotations;

namespace VacancyProAPI.Models.DbModels
{
    public class Activity
    {
        public Activity()
        {
            
        }
        public Activity(string name, string description, DateTime beginDate, DateTime endDate, Place activityPlace, Period period)
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
        public string Name { get; set; } = null!;

        [Required]
        public string Description { get; set; } = null!;

        [Required]
        public DateTime BeginDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        
        public Place Place { get; set; } = null!;

        [Required]
        public Period Period { get; set; } = null!;
    }
}