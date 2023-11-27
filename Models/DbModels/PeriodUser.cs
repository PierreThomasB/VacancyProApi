namespace VacancyProAPI.Models.DbModels;

public class PeriodUser
{
    public int PeriodId { get; set; }
    public string UserId { get; set; } = string.Empty;
    public bool IsCreator { get; set; }
}