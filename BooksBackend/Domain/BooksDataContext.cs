using Microsoft.EntityFrameworkCore;

namespace BooksBackend.Domain
{
    public class BooksDataContext : DbContext
    {
        public BooksDataContext(DbContextOptions<BooksDataContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
    }
}
