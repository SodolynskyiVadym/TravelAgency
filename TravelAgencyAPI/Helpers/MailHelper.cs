using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TravelAgencyAPI.Settings;

namespace TravelAgencyAPI.Helpers;

public class MailHelper
{
    private readonly MailSetting _mailSettings;

    public MailHelper(IOptions<MailSetting> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }


    string pathHTMLPasswordPage = @"AdditionalFiles/sendPassword.html";
    string pathHTMLReservePasswordPage = @"AdditionalFiles/sendReservePassword.html";


    public bool SendPassword(string toEmail, string password, string role)
    {
        try
        {
            using (MimeMessage emailMessage = new MimeMessage())
            {
                if (!File.Exists(pathHTMLPasswordPage)) return false;


                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(null, toEmail);
                emailMessage.To.Add(emailTo);


                emailMessage.Subject = "Your temporary password";

                string htmlTemplate = File.ReadAllText(pathHTMLPasswordPage);
                string textHTMLPasswordPage = htmlTemplate.Replace("{0}", role);
                textHTMLPasswordPage = textHTMLPasswordPage.Replace("{1}", password);


                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = textHTMLPasswordPage;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();


                using (SmtpClient mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.Server, _mailSettings.Port,
                        MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }


    public bool SendReservePassword(string email, string password)
    {
        try
        {
            using (MimeMessage emailMessage = new MimeMessage())
            {
                if (!File.Exists(pathHTMLReservePasswordPage)) return false;


                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(null, email);
                emailMessage.To.Add(emailTo);


                emailMessage.Subject = "Your temporary password";

                string htmlTemplate = File.ReadAllText(pathHTMLReservePasswordPage);
                string textHTMLReservePasswordPage = htmlTemplate.Replace("{0}", password);


                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = textHTMLReservePasswordPage;

                emailMessage.Body = emailBodyBuilder.ToMessageBody();


                using (SmtpClient mailClient = new SmtpClient())
                {
                    mailClient.Connect(_mailSettings.Server, _mailSettings.Port,
                        MailKit.Security.SecureSocketOptions.StartTls);
                    mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                    mailClient.Send(emailMessage);
                    mailClient.Disconnect(true);
                }
            }

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}