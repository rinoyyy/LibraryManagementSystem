using LibraryManagementAPI.Data;
using LibraryManagementAPI.DTOs;
using LibraryManagementAPI.Interfaces;

namespace LibraryManagementAPI.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly AppDbContext _context;

        public DashboardService(AppDbContext context)
        {
            _context = context;
        }

        public StudentDashboardResponse GetStudentDashboard(int memberId)
        {
            return new StudentDashboardResponse
            {
                // AvailableBooks = _context.Books.Sum(b => b.AvailableCopies),
                AvailableBooks = _context.Books.Count(),

                BorrowedBooks = _context.BorrowRecords.Count(br =>
                    br.MemberId == memberId &&
                    br.ReturnDate == null),

                ReturnedBooks = _context.BorrowRecords.Count(br =>
                    br.MemberId == memberId &&
                    br.ReturnDate != null),

                TotalBorrowed = _context.BorrowRecords.Count(br =>
                    br.MemberId == memberId)
            };
        }

        public AdminDashboardResponse GetAdminDashboard()
        {
            return new AdminDashboardResponse
            {
                TotalBooks = _context.Books.Count(),

                AvailableBooks = _context.Books.Sum(b => b.AvailableCopies),

                BorrowedBooks = _context.BorrowRecords.Count(br =>
                    br.ReturnDate == null),

                Students = _context.Members.Count()
            };
        }
    }
}