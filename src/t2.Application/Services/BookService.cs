using AutoMapper;
using t2.Application.DTOs.Book;
using t2.Application.Interfaces;
using t2.Domain.Entities;
using t2.Domain.Exceptions;
using t2.Domain.Ports.Out;

namespace t2.Application.Services
{
    public class BookService : IBookService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<BookDto?> GetByIdAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
                return null;

            var isBaja = await _unitOfWork.BooksBaja.ExistsByBookIdAsync(id);
            if (isBaja)
                return null;

            return _mapper.Map<BookDto>(book);
        }

        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            var books = await _unitOfWork.Books.GetAllAsync();
            
            var booksBaja = await _unitOfWork.BooksBaja.GetAllAsync();
            var booksBajaIds = booksBaja.Select(b => b.BookId).ToHashSet();
            
            var activeBooks = books.Where(b => !booksBajaIds.Contains(b.Id));
            return _mapper.Map<IEnumerable<BookDto>>(activeBooks);
        }

        public async Task<BookDto> CreateAsync(CreateBookDto dto)
        {
            var existingBook = await _unitOfWork.Books.GetByISBNAsync(dto.ISBN);
            if (existingBook != null)
            {
                throw new DuplicateEntityException("Book", "ISBN", dto.ISBN);
            }

            var book = _mapper.Map<Book>(dto);
            var createdBook = await _unitOfWork.Books.CreateAsync(book);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookDto>(createdBook);
        }

        public async Task<BookDto> UpdateAsync(int id, CreateBookDto dto)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
            {
                throw new NotFoundException("Book", id);
            }
            if (book.ISBN != dto.ISBN)
            {
                var existingBook = await _unitOfWork.Books.GetByISBNAsync(dto.ISBN);
                if (existingBook != null)
                {
                    throw new DuplicateEntityException("Book", "ISBN", dto.ISBN);
                }
            }

            book.Title = dto.Title;
            book.Author = dto.Author;
            book.ISBN = dto.ISBN;
            book.Stock = dto.Stock;

            var updatedBook = await _unitOfWork.Books.UpdateAsync(book);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<BookDto>(updatedBook);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
            {
                throw new NotFoundException("Book", id);
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                if (book.Stock == 0)
                {
                    var existsBaja = await _unitOfWork.BooksBaja.ExistsByBookIdAsync(id);
                    if (!existsBaja)
                    {
                        var bookBaja = new BookBaja
                        {
                            BookId = id,
                            FechaBaja = DateTime.Now,
                            Motivo = "Eliminado",
                            CreatedAt = DateTime.Now
                        };

                        await _unitOfWork.BooksBaja.CreateAsync(bookBaja);
                    }
                }
                var result = await _unitOfWork.Books.DeleteAsync(id);
                
                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return result;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }

        public async Task<bool> DarBajaAsync(int id, string motivo)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(id);
            if (book == null)
            {
                throw new NotFoundException("Book", id);
            }

            var existsBaja = await _unitOfWork.BooksBaja.ExistsByBookIdAsync(id);
            if (existsBaja)
            {
                throw new BusinessRuleException(
                    "BookAlreadyBaja",
                    $"Libro con {id} dado de baja");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var bookBaja = new BookBaja
                {
                    BookId = id,
                    FechaBaja = DateTime.Now,
                    Motivo = motivo,
                    CreatedAt = DateTime.Now
                };

                await _unitOfWork.BooksBaja.CreateAsync(bookBaja);

                if (book.Stock > 0)
                {
                    var bookLiquidacion = new BookLiquidacion
                    {
                        BookId = id,
                        StockLiquidado = book.Stock,
                        FechaLiquidacion = DateTime.Now,
                        CreatedAt = DateTime.Now
                    };

                    await _unitOfWork.BooksLiquidacion.CreateAsync(bookLiquidacion);
                }

                await _unitOfWork.SaveChangesAsync();
                await _unitOfWork.CommitTransactionAsync();

                return true;
            }
            catch
            {
                await _unitOfWork.RollbackTransactionAsync();
                throw;
            }
        }
    }
}

