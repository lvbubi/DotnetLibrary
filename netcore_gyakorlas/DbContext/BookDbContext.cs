using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using netcore_gyakorlas.Models;

namespace netcore_gyakorlas.Context
{
    public class BookDbContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public BookDbContext(DbContextOptions<BookDbContext> options) : base(options)
        {

        }
    }
}
