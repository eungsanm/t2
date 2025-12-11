using t2.Domain.Entities;

namespace t2.Domain.Ports.Out
{
    public interface IBookRepository : IRepository<Book>
    {
        Task<Book?> GetByISBNAsync(string isbn);
        Task<bool> ExistsByISBNAsync(string isbn);
    }
}

