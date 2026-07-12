using LibraryManagementAPI.Data;
using LibraryManagementAPI.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services
{
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public int? GetMemberId(string username)
        {
            var member = _context.Members
                .Include(m => m.User)
                .FirstOrDefault(m => m.User!.Username == username);

            return member?.Id;
        }
    }
}