using Microsoft.EntityFrameworkCore;
using t2.Domain.Entities;
using t2.Domain.Ports.Out;
using t2.Infrastructure.Persistence.Context;

namespace t2.Infrastructure.Persistence.Repositories
{
    public class BookLiquidacionRepository : Repository<BookLiquidacion>, IBookLiquidacionRepository
    {
        public BookLiquidacionRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<BookLiquidacion>> GetByBookIdAsync(int bookId)
        {
            return await _dbSet
                .Where(b => b.BookId == bookId)
                .ToListAsync();
        }
    }
}


