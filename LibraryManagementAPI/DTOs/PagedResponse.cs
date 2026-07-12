namespace LibraryManagementAPI.DTOs
{
    public class PagedResponse<T>
    {
        public int CurrentPage { get; set; }

        public int PageSize { get; set; }

        public int TotalRecords { get; set; }

        public int TotalPages { get; set; }

        public List<T> Items { get; set; } = new();
    }
}