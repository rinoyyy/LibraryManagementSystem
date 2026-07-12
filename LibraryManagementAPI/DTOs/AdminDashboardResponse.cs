namespace LibraryManagementAPI.DTOs
{
    public class AdminDashboardResponse
    {
        public int TotalBooks { get; set; }

        public int AvailableBooks { get; set; }

        public int BorrowedBooks { get; set; }

        public int Students { get; set; }
    }
}