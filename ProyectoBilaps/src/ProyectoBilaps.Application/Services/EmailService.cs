using MailKit.Net.Smtp;
using MimeKit;
using Microsoft.Extensions.Configuration;
using MailKit.Security;

public class EmailService : IEmailService
{
    private readonly IConfiguration _config;

    public EmailService(IConfiguration config)
    {
        _config = config;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string htmlBody)
    {
        var smtp = _config.GetSection("Smtp");

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(smtp["FromName"], smtp["FromEmail"]));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = subject;

        var body = new BodyBuilder
        {
            HtmlBody = htmlBody
        };

        message.Body = body.ToMessageBody();

        using var client = new SmtpClient();

        // Gmail requiere desactivar OAuth
        client.AuthenticationMechanisms.Remove("XOAUTH2");

        await client.ConnectAsync(
            smtp["Host"],
            int.Parse(smtp["Port"]),
            SecureSocketOptions.StartTls
        );

        await client.AuthenticateAsync(
            smtp["Username"],
            smtp["Password"]
        );

        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }
}