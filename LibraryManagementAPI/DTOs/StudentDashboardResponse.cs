namespace LibraryManagementAPI.DTOs
{
    public class StudentDashboardResponse
    {
        public int AvailableBooks { get; set; }

        public int BorrowedBooks { get; set; }

        public int ReturnedBooks { get; set; }

        public int TotalBorrowed { get; set; }
    }
}