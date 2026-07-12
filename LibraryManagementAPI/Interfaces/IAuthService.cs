using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Interfaces
{
    public interface IAuthService
    {
        bool RegisterStudent(RegisterRequest request);

        bool RegisterAdmin(RegisterRequest request);

        LoginResponse? Login(LoginRequest request);
    }
}