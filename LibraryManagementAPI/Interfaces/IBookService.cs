using LibraryManagementAPI.Models;
using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Interfaces
{
    public interface IBookService
    {
        PagedResponse<BookResponse> GetAllBooks(
    int pageNumber,
    int pageSize,
    string? search = null);

        BookResponse? GetBookById(int id);

        BookResponse AddBook(AddBookRequest request);

        int GetBookCount();

        List<BookResponse> SearchBooks(string keyword);

        List<BookResponse> GetBooksSortedByYear();

        BookResponse? UpdateBook(int id, UpdateBookRequest request);

        bool DeleteBook(int id);

        bool BorrowBook(int bookId, int memberId);

        bool ReturnBook(int bookId, int memberId);

        PagedResponse<BorrowRecordResponse> GetCurrentBorrowedBooks(
    int memberId,
    int pageNumber,
    int pageSize,
    string? search = null);

        PagedResponse<BorrowRecordResponse> GetBorrowHistory(
    int memberId,
    int pageNumber,
    int pageSize,
    string? search = null);

        PagedResponse<BorrowRecordResponse> GetBorrowRecords(
    int pageNumber,
    int pageSize,
    string? search = null);
    }
}