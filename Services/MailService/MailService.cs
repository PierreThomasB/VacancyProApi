using MailKit.Net.Smtp;
using MimeKit;
using VacancyProAPI.Models.Mails;

namespace VacancyProAPI.Services.MailService;

public class MailService : IMailService
{
    private readonly IConfiguration _configuration;

    public MailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendMail(DefaultMail defaultMail)
    {
        var mail = new MimeMessage();
        mail.From.Add(MailboxAddress.Parse(defaultMail.From ?? _configuration["MailSettings:ServerFrom"]));
        mail.To.Add(MailboxAddress.Parse(defaultMail.To ?? _configuration["MailSettings:ServerAdmin"]));
        mail.Subject = defaultMail.Subject;
        mail.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = defaultMail.GetMailBody() };

        /*var bodyBuilder = new BodyBuilder();
        bodyBuilder.TextBody = defaultMail.GetMailBody();
        mail.Body = bodyBuilder.ToMessageBody();
        using (var smtp = new SmtpClient())
        {
            smtp.Connect(_configuration["MailSettings:ServerName"],
                int.Parse(_configuration["MailSettings:ServerPort"]!), MailKit.Security.SecureSocketOptions.None);
            smtp.Send(mail);
            smtp.Disconnect(true);
        }*/
        new Thread(() =>
        {
            var smtp = new SmtpClient();
            try
            {
                smtp.Connect(_configuration["MailSettings:ServerName"],
                    int.Parse(_configuration["MailSettings:ServerPort"]),
                    MailKit.Security.SecureSocketOptions.None);
                smtp.Send(mail);
            }
            catch (Exception)
            {
            }
            finally
            {
                smtp.Disconnect(true);
            }
        });
    }
}