using Microsoft.EntityFrameworkCore;
using t2.Domain.Entities;
using t2.Infrastructure.Persistence.Configurations;

namespace t2.Infrastructure.Persistence.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Loan> Loans { get; set; }
        public DbSet<BookBaja> BooksBaja { get; set; }
        public DbSet<BookLiquidacion> BooksLiquidacion { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new LoanConfiguration());
            modelBuilder.ApplyConfiguration(new BookBajaConfiguration());
            modelBuilder.ApplyConfiguration(new BookLiquidacionConfiguration());
        }
    }
}

