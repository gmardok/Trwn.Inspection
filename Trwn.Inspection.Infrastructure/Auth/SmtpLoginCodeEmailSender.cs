using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Trwn.Inspection.Configuration;

namespace Trwn.Inspection.Infrastructure.Auth;

public sealed class SmtpLoginCodeEmailSender : ILoginCodeEmailSender
{
    private readonly IOptions<AuthSettings> _options;

    public SmtpLoginCodeEmailSender(IOptions<AuthSettings> options)
    {
        _options = options;
    }

    public async Task SendSignInCodeAsync(string toEmail, string code, CancellationToken cancellationToken)
    {
        var auth = _options.Value;
        var smtp = auth.Smtp;

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(smtp.FromDisplayName, smtp.FromAddress));
        message.To.Add(MailboxAddress.Parse(toEmail));
        message.Subject = "Your sign-in code";

        var builder = new BodyBuilder
        {
            HtmlBody = BuildHtmlBody(code),
            TextBody = $"Your sign-in code is: {code}\r\n\r\nIf you did not request this, you can ignore this email.",
        };
        message.Body = builder.ToMessageBody();

        using var client = new SmtpClient();
        var secure =
            smtp.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.SslOnConnect;
        await client.ConnectAsync(smtp.Host, smtp.Port, secure, cancellationToken).ConfigureAwait(false);
        await client.AuthenticateAsync(smtp.UserName, smtp.Password, cancellationToken).ConfigureAwait(false);
        await client.SendAsync(message, cancellationToken).ConfigureAwait(false);
        await client.DisconnectAsync(true, cancellationToken).ConfigureAwait(false);
    }

    private static string BuildHtmlBody(string code)
    {
        var safeCode = System.Net.WebUtility.HtmlEncode(code);
        return $"""
            <!DOCTYPE html>
            <html>
            <head><meta charset="utf-8" /></head>
            <body style="font-family:Segoe UI,Roboto,Helvetica,Arial,sans-serif;background:#f6f7fb;padding:24px;">
              <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="max-width:480px;margin:0 auto;background:#ffffff;border-radius:8px;padding:32px;box-shadow:0 1px 3px rgba(0,0,0,.08);">
                <tr>
                  <td>
                    <p style="margin:0 0 16px;color:#333;font-size:16px;">Use this code to sign in:</p>
                    <p style="margin:0 0 24px;font-size:28px;font-weight:600;letter-spacing:4px;font-family:Consolas,monospace;color:#111;">{safeCode}</p>
                    <table role="presentation" cellspacing="0" cellpadding="0" style="margin:0 0 16px;">
                      <tr>
                        <td style="border-radius:6px;background:#1a73e8;">
                          <span style="display:inline-block;padding:12px 24px;color:#ffffff;font-size:14px;font-weight:600;">Copy the code above</span>
                        </td>
                      </tr>
                    </table>
                    <p style="margin:0;color:#666;font-size:13px;line-height:1.5;">This code expires in a few minutes. If you did not request it, you can ignore this message.</p>
                  </td>
                </tr>
              </table>
            </body>
            </html>
            """;
    }
}
