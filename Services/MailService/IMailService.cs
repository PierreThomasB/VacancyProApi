using VacancyProAPI.Models.Mails;

namespace VacancyProAPI.Services.MailService;

public interface IMailService
{
    void SendMail(DefaultMail defaultMail);
}