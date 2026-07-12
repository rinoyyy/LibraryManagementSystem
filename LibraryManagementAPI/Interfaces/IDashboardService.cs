using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Interfaces
{
    public interface IDashboardService
    {
        StudentDashboardResponse GetStudentDashboard(int memberId);

        AdminDashboardResponse GetAdminDashboard();
    }
}