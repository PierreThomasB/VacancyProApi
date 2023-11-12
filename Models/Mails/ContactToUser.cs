namespace VacancyProAPI.Models.Mails;

public class ContactToUser : DefaultMail
{
    public ContactToUser(string subject, string to, string from, string[] values) : base(subject, to, from, values)
    {
    }

    public override string GetMailBody()
        => GetMailStyle("Bonjour,",
            "\n\nVotre mail a bien été pris en compte. L'équipde d'administration vous répondrons dès que possible.");
}