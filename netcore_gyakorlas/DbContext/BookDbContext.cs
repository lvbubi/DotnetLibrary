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

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.Entity<Book>().HasOne(e => e.Author).WithMany(p => p.Events).HasForeignKey(e => e.PlaceIdentity);

            //modelBuilder.Entity<EventStaff>().HasKey(es => new { es.EventId, es.OrganizerId });
            //modelBuilder.Entity<EventStaff>().HasOne(es => es.Event).WithMany(e => e.Staff).HasForeignKey(es => es.EventId);
            //modelBuilder.Entity<EventStaff>().HasOne(es => es.Organizer).WithMany(o => o.Events).HasForeignKey(es => es.OrganizerId);

            //modelBuilder.RemoveOneToManyCascadeDeleteConvention();
            base.OnModelCreating(modelBuilder);
        }
    }
}
