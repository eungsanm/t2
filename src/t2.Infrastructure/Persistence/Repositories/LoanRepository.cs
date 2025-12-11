using Microsoft.EntityFrameworkCore;
using t2.Domain.Entities;
using t2.Domain.Ports.Out;
using t2.Infrastructure.Persistence.Context;

namespace t2.Infrastructure.Persistence.Repositories
{
    public class LoanRepository : Repository<Loan>, ILoanRepository
    {
        public LoanRepository(ApplicationDbContext context)
            : base(context)
        {
        }

        public async Task<IEnumerable<Loan>> GetByBookIdAsync(int bookId)
        {
            return await _dbSet
                .Where(l => l.BookId == bookId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Loan>> GetByStatusAsync(string status)
        {
            return await _dbSet
                .Where(l => l.Status == status)
                .ToListAsync();
        }

        public async Task<Loan?> GetWithBookAsync(int id)
        {
            return await _dbSet
                .Include(l => l.Book)
                .FirstOrDefaultAsync(l => l.Id == id);
        }
    }
}

