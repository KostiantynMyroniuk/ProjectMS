namespace Shared.Services
{
    public interface IIdentityService
    {
        string GetUserId();
        string GetUserName();
        string GetEmail();
    }
}
