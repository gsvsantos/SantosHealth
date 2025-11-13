using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using OrganizaMed.Aplicacao.EmailSender.DTOs;

namespace OrganizaMed.Aplicacao.EmailSender.Commands;

public interface IEmailSender
{
    Task EnviarEmailAsync(EmailDto request);
}

public class EmailSender(IOptions<MailOptions> mailOptions, ILogger<EmailSender> logger) : IEmailSender
{
    private readonly MailOptions mailOptions = mailOptions.Value;

    public async Task EnviarEmailAsync(EmailDto request)
    {
        MimeMessage email = new();

        email.From.Add(MailboxAddress.Parse(this.mailOptions.UserName));
        email.To.Add(MailboxAddress.Parse(request.Para));
        email.Subject = request.Assunto;
        email.Body = new TextPart(TextFormat.Html) { Text = request.Conteudo };

        using SmtpClient smtp = new();

        await smtp.ConnectAsync(
            this.mailOptions.Host,
            this.mailOptions.Port,
            MailKit.Security.SecureSocketOptions.StartTls
        );
        await smtp.AuthenticateAsync(
            this.mailOptions.UserName,
            this.mailOptions.Password
        );
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true);

        logger.LogInformation("Sucesso! Email enviado para {Email}", request.Para);
    }
}