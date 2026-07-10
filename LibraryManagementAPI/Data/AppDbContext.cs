using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books => Set<Book>();

        public DbSet<Member> Members => Set<Member>();

        public DbSet<User> Users => Set<User>();

        public DbSet<BorrowRecord> BorrowRecords => Set<BorrowRecord>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Member>()
                .HasOne(m => m.User)
                .WithOne(u => u.Member)
                .HasForeignKey<Member>(m => m.UserId);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Member>()
                      .HasIndex(m => m.Email)
                      .IsUnique();
        }
    }
}