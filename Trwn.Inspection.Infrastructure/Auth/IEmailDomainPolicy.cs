namespace Trwn.Inspection.Infrastructure.Auth;

public interface IEmailDomainPolicy
{
    bool IsDomainAllowed(string emailAddress);
}
