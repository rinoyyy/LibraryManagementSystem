namespace LibraryManagementAPI.DTOs
{
    public class BorrowRecordResponse
    {
        public int BorrowRecordId { get; set; }

        public int BookId { get; set; }

        public string BookTitle { get; set; } = string.Empty;

        public string StudentName { get; set; } = string.Empty;

        public DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}