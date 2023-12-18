namespace VacancyProAPI.Models.Mails;

public class ContactToAdminMail : DefaultMail
{
    public ContactToAdminMail(string subject, string to, string from, string[] values) : base(subject, to, from, values)
    {
    }

    public override string GetMailBody()
        => GetMailStyle("Mail d'un membre de VacancyPro", Values[0]);
}