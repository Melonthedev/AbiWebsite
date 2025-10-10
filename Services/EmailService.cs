using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Options;

namespace AbiWebsite.Services;

public class EmailService(IOptions<Models.HostOptions> hostOptions)
{
    private readonly Models.HostOptions _hostOptions = hostOptions.Value;

    /// <summary>
    /// Sendet eine Login-Code-Mail als HTML an den angegebenen Empfänger.
    /// </summary>
    public bool SendLoginCodeMail(string recepientAddress, string studentName, string loginCode, bool isResend = false)
    {
        string? from = _hostOptions.HostMail ?? _hostOptions.MailCredentials?.Username;
        if (string.IsNullOrEmpty(recepientAddress) || string.IsNullOrEmpty(from))
            return false;

        string subject = "Dein Login-Code für das Motto Ranking (Abi27 Winfriedschule)";
        string htmlBody = $@"
            <html>
            <body style='font-family:Arial,sans-serif;background:#f8f9fa;padding:2rem;'>
                <div style='max-width:500px;margin:auto;background:#fff;border-radius:1rem;padding:2rem;box-shadow:0 2px 8px rgba(0,0,0,0.08);'>
                    <h2 style='color:#1b6ec2;'>Hallo {studentName},</h2>
                    <p>Dein persönlicher Login-Code für das Mottoranking lautet:</p>
                    <div style='font-size:2rem;font-weight:bold;color:#28a745;margin:1.5rem 0;'>{loginCode}</div>
                    <p>{(isResend ? "Dies ist ein erneut angeforderter Code. Wenn du keinen neuen Code angefordert hast, ignoriere diese Mail." : "")}</p>
                    <p>Bitte bewahre diesen Code sicher auf. Du kannst ihn für zukünftige Logins nutzen.</p>
                    <hr style='margin:2rem 0;' />
                    <small style='color:#888;'>Diese E-Mail wurde automatisch generiert. Bei Fragen wende dich bitte an die Stufensprecher.</small>
                </div>
            </body>
            </html>
        ";

        MailMessage message = new()
        {
            From = new MailAddress(from, "Abi27 Winfriedschule"),
            Subject = subject,
            Body = htmlBody,
            IsBodyHtml = true
        };
        message.To.Add(new MailAddress(recepientAddress));

        return SendEmail(message);
    }

    public bool SendEmail(MailMessage message)
    {
        if (_hostOptions.MailCredentials is null)
            throw new InvalidDataException("MailCredentials in HostOptions were not defined.");
        string? username = _hostOptions.MailCredentials.Username;
        string? passwort = _hostOptions.MailCredentials.Password;
        string? host = _hostOptions.MailCredentials.Host;
        int port = _hostOptions.MailCredentials.Port;

        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(passwort) || string.IsNullOrEmpty(host))
        {
            return false;
        }

        var smtpClient = new SmtpClient(host)
        {
            Port = port,
            Credentials = new NetworkCredential(username, passwort),
            EnableSsl = true
        };

        try
        {
            smtpClient.Send(message);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}