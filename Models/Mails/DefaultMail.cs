namespace VacancyProAPI.Models.Mails;

public abstract class DefaultMail
{
    public string Subject { get; set; }
    public string To { get; set; }
    public string From { get; set; }
    public string[] Values { get; set; }

    protected DefaultMail(string subject, string to, string from, string[] values)
    {
        Subject = subject;
        To = to;
        From = from;
        Values = values;
    }

    public abstract string GetMailBody();
    
    
}