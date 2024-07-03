using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using TravelAgencyAPI.Models;
using TravelAgencyAPI.Settings;

namespace TravelAgencyAPI.Helpers;

public class MailHelper
{
    private readonly MailSetting _mailSettings;

    public MailHelper(IOptions<MailSetting> mailSettings)
    {
        _mailSettings = mailSettings.Value;
    }


    private string pathHTMLPasswordPage = @"AdditionalFiles/sendPassword.html";
    private string pathHTMLReservePasswordPage = @"AdditionalFiles/sendReservePassword.html";
    private string pathHTMLTourMessage = @"AdditionalFiles/sendTourMessage.html";


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
                string textHtmlPasswordPage = htmlTemplate.Replace("{0}", role);
                textHtmlPasswordPage = textHtmlPasswordPage.Replace("{1}", password);


                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = textHtmlPasswordPage;

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
                string textHtmlReservePasswordPage = htmlTemplate.Replace("{0}", password);


                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = textHtmlReservePasswordPage;

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
    
    public bool SendTourMessage(string email, Tour tour)
    {
        try
        {
            using (MimeMessage emailMessage = new MimeMessage())
            {
                if (!File.Exists(pathHTMLTourMessage)) return false;


                MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                emailMessage.From.Add(emailFrom);
                MailboxAddress emailTo = new MailboxAddress(null, email);
                emailMessage.To.Add(emailTo);


                emailMessage.Subject = $"Welcome to {tour.Name}!";

                string htmlTemplate = File.ReadAllText(pathHTMLTourMessage);
                string textHtmlReservePasswordPage = htmlTemplate.Replace("{0}", tour.Name);
                textHtmlReservePasswordPage = textHtmlReservePasswordPage.Replace("{1}", tour.ImageUrl);
                textHtmlReservePasswordPage = textHtmlReservePasswordPage.Replace("{2}", tour.Description);


                BodyBuilder emailBodyBuilder = new BodyBuilder();
                emailBodyBuilder.HtmlBody = textHtmlReservePasswordPage;

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