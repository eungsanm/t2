using Microsoft.EntityFrameworkCore;
using t2.Domain.Entities;
using t2.Domain.Ports.Out;
using t2.Infrastructure.Persistence.Context;

namespace t2.Infrastructure.Persistence.Repositories
{
    public class BookRepository : Repository<Book>, IBookRepository
    {
        public BookRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<Book?> GetByISBNAsync(string isbn)
        {
            return await _dbSet
                .FirstOrDefaultAsync(b => b.ISBN == isbn);
        }

        public async Task<bool> ExistsByISBNAsync(string isbn)
        {
            return await _dbSet
                .AnyAsync(b => b.ISBN == isbn);
        }
    }
}

