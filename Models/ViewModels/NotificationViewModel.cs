using VacancyProAPI.Models.DbModels;

namespace VacancyProAPI.Models.ViewModels;

public class NotificationViewModel
{
    public List<Notification> Notifications { get; set; }
    public int Count { get; set; }


    public NotificationViewModel(int count, List<Notification> notifications)
    {
        Count = count;
        Notifications = notifications;
    }
}