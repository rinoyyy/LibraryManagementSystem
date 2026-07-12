using LibraryManagementAPI.Models;
using LibraryManagementAPI.DTOs;

namespace LibraryManagementAPI.Interfaces
{
    public interface IBookService
    {
        List<BookResponse> GetAllBooks();

        BookResponse? GetBookById(int id);

        BookResponse AddBook(AddBookRequest request);

        int GetBookCount();

        List<BookResponse> SearchBooks(string keyword);

        List<BookResponse> GetBooksSortedByYear();

        BookResponse? UpdateBook(int id, UpdateBookRequest request);

        bool DeleteBook(int id);

        bool BorrowBook(int bookId, int memberId);

        bool ReturnBook(int bookId, int memberId);

        List<BorrowRecordResponse> GetCurrentBorrowedBooks(int memberId);

        List<BorrowRecordResponse> GetBorrowHistory(int memberId);

        PagedResponse<BorrowRecordResponse> GetBorrowRecords(
    int pageNumber,
    int pageSize);
    }
}