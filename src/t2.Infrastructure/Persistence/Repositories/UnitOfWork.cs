using Microsoft.EntityFrameworkCore.Storage;
using t2.Domain.Ports.Out;
using t2.Infrastructure.Persistence.Context;

namespace t2.Infrastructure.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        private IDbContextTransaction? _transaction;

        public IBookRepository Books { get; }
        public ILoanRepository Loans { get; }
        public IBookBajaRepository BooksBaja { get; }
        public IBookLiquidacionRepository BooksLiquidacion { get; }

        public UnitOfWork(ApplicationDbContext context,
                          IBookRepository bookRepository,
                          ILoanRepository loanRepository,
                          IBookBajaRepository bookBajaRepository,
                          IBookLiquidacionRepository bookLiquidacionRepository)
        {
            _context = context;
            Books = bookRepository;
            Loans = loanRepository;
            BooksBaja = bookBajaRepository;
            BooksLiquidacion = bookLiquidacionRepository;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task BeginTransactionAsync()
        {
            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackTransactionAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}

