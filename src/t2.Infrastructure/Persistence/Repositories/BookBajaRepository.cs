using Microsoft.EntityFrameworkCore;
using t2.Domain.Entities;
using t2.Domain.Ports.Out;
using t2.Infrastructure.Persistence.Context;

namespace t2.Infrastructure.Persistence.Repositories
{
    public class BookBajaRepository : Repository<BookBaja>, IBookBajaRepository
    {
        public BookBajaRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<BookBaja>> GetByBookIdAsync(int bookId)
        {
            return await _dbSet
                .Where(b => b.BookId == bookId)
                .ToListAsync();
        }

        public async Task<bool> ExistsByBookIdAsync(int bookId)
        {
            return await _dbSet
                .AnyAsync(b => b.BookId == bookId);
        }
    }
}


