namespace LibraryManagementAPI.DTOs
{
    public class BorrowRequest
    {
        public int BookId { get; set; }

        public int MemberId { get; set; }
    }
}