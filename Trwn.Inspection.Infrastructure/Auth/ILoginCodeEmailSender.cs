namespace Trwn.Inspection.Infrastructure.Auth;

public interface ILoginCodeEmailSender
{
    Task SendSignInCodeAsync(string toEmail, string code, CancellationToken cancellationToken);
}
