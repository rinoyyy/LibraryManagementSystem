using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Interfaces
{
    public interface IBookService
    {
        List<Book> GetAllBooks();

        Book? GetBookById(int id);

        Book AddBook(Book book);

        int GetBookCount();

        List<Book> SearchBooks(string keyword);

        List<Book> GetBooksSortedByYear();

        Book? UpdateBook(int id, Book updatedBook);

        bool DeleteBook(int id);

        bool BorrowBook(int bookId, int memberId);

        bool ReturnBook(int bookId, int memberId);
    }
}