using MimeKit;
using MailKit.Net.Smtp;
using TravelAgencyService.Models;
using TravelAgencyService.Settings;

namespace TravelAgencyService.Services;

public class MailService
{
    private readonly MailSetting _mailSettings;
    private string passwordPage = File.ReadAllText("AdditionalFiles/sendPassword.html");
    private string reservePasswordPage = File.ReadAllText("AdditionalFiles/sendReservePassword.html");
    private string tourMessagePage = File.ReadAllText("AdditionalFiles/sendTourMessage.html");
    
    public MailService(MailSetting mailSettings)
    {
        _mailSettings = mailSettings;
    }
    
    public bool SendPassword(string toEmail, User user)
    {
        try
        {
            using MimeMessage emailMessage = new MimeMessage();

            MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);
            MailboxAddress emailTo = new MailboxAddress(null, toEmail);
            emailMessage.To.Add(emailTo);


            emailMessage.Subject = "Your temporary password";
            
            string textHtmlPasswordPage = passwordPage.Replace("{0}", user.Role);
            textHtmlPasswordPage = textHtmlPasswordPage.Replace("{1}", user.Password);


            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.HtmlBody = textHtmlPasswordPage;

            emailMessage.Body = emailBodyBuilder.ToMessageBody();


            using SmtpClient mailClient = new SmtpClient();
            mailClient.Connect(_mailSettings.Server, _mailSettings.Port,
                MailKit.Security.SecureSocketOptions.StartTls);
            mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            mailClient.Send(emailMessage);
            mailClient.Disconnect(true);

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
            using MimeMessage emailMessage = new MimeMessage();

            MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);
            MailboxAddress emailTo = new MailboxAddress(null, email);
            emailMessage.To.Add(emailTo);


            emailMessage.Subject = "Your temporary password";
            
            string textHtmlReservePasswordPage = reservePasswordPage.Replace("{0}", password);


            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.HtmlBody = textHtmlReservePasswordPage;

            emailMessage.Body = emailBodyBuilder.ToMessageBody();


            using SmtpClient mailClient = new SmtpClient();
            mailClient.Connect(_mailSettings.Server, _mailSettings.Port,
                MailKit.Security.SecureSocketOptions.StartTls);
            mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            mailClient.Send(emailMessage);
            mailClient.Disconnect(true);

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
            using MimeMessage emailMessage = new MimeMessage();

            MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);
            MailboxAddress emailTo = new MailboxAddress(null, email);
            emailMessage.To.Add(emailTo);
            

            emailMessage.Subject = $"Welcome to {tour.Name}!";
            
            string textHtmlTourMessagePage = tourMessagePage.Replace("{0}", tour.Name);
            textHtmlTourMessagePage = textHtmlTourMessagePage.Replace("{1}", tour.ImageUrl);
            textHtmlTourMessagePage = textHtmlTourMessagePage.Replace("{2}", tour.Description);


            BodyBuilder emailBodyBuilder = new BodyBuilder();
            emailBodyBuilder.HtmlBody = textHtmlTourMessagePage;

            emailMessage.Body = emailBodyBuilder.ToMessageBody();


            using SmtpClient mailClient = new SmtpClient();
            mailClient.Connect(_mailSettings.Server, _mailSettings.Port,
                MailKit.Security.SecureSocketOptions.StartTls);
            mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            mailClient.Send(emailMessage);
            mailClient.Disconnect(true);

            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}