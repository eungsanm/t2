using t2.Domain.Entities;

namespace t2.Domain.Ports.Out
{
    public interface IBookLiquidacionRepository : IRepository<BookLiquidacion>
    {
        Task<IEnumerable<BookLiquidacion>> GetByBookIdAsync(int bookId);
    }
}


