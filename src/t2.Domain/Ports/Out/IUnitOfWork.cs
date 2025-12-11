namespace t2.Domain.Ports.Out
{
    public interface IUnitOfWork : IDisposable
    {
        IBookRepository Books { get; }
        ILoanRepository Loans { get; }
        IBookBajaRepository BooksBaja { get; }
        IBookLiquidacionRepository BooksLiquidacion { get; }

        Task<int> SaveChangesAsync();
        Task BeginTransactionAsync();
        Task CommitTransactionAsync();
        Task RollbackTransactionAsync();
    }
}

