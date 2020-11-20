using Microsoft.EntityFrameworkCore;
using netcore_gyakorlas.Models;

namespace netcore_gyakorlas.Context
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }
        public DbSet<Library> Library { get; set; }
        public DbSet<BookLibrary> BookLibraries { get; set; }
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {

        }
    }
}
