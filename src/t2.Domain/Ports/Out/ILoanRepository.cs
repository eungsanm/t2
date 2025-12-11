using t2.Domain.Entities;

namespace t2.Domain.Ports.Out
{
    public interface ILoanRepository : IRepository<Loan>
    {
        Task<IEnumerable<Loan>> GetByBookIdAsync(int bookId);
        Task<IEnumerable<Loan>> GetByStatusAsync(string status);
        Task<Loan?> GetWithBookAsync(int id);
    }
}

