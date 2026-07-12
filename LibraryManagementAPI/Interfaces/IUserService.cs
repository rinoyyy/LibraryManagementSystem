namespace LibraryManagementAPI.Interfaces
{
    public interface IUserService
    {
        int? GetMemberId(string username);
    }
}