using t2.Domain.Entities;

namespace t2.Domain.Ports.Out
{
    public interface IBookBajaRepository : IRepository<BookBaja>
    {
        Task<IEnumerable<BookBaja>> GetByBookIdAsync(int bookId);
        Task<bool> ExistsByBookIdAsync(int bookId);
    }
}


