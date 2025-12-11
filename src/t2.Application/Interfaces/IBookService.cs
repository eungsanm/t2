using t2.Application.DTOs.Book;

namespace t2.Application.Interfaces
{
    public interface IBookService
    {
        Task<BookDto?> GetByIdAsync(int id);
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto> CreateAsync(CreateBookDto bookDto);
        Task<BookDto> UpdateAsync(int id, CreateBookDto bookDto);
        Task<bool> DeleteAsync(int id);
        Task<bool> DarBajaAsync(int id, string motivo);
    }
}

