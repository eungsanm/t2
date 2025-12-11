using Microsoft.AspNetCore.Mvc;
using t2.Application.DTOs.Book;
using t2.Application.Interfaces;
using t2.Domain.Exceptions;

namespace t2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BookDto>>> GetAll()
        {
            var books = await _bookService.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BookDto>> GetById(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
                return NotFound(new { message = $"ID {id} not found." });

            return Ok(book);
        }

        [HttpPost]
        public async Task<ActionResult<BookDto>> Create([FromBody] CreateBookDto dto)
        {
            try
            {
                var book = await _bookService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = book.Id }, book);
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BookDto>> Update(int id, [FromBody] CreateBookDto dto)
        {
            try
            {
                var book = await _bookService.UpdateAsync(id, dto);
                return Ok(book);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (DuplicateEntityException ex)
            {
                return Conflict(new { message = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _bookService.DeleteAsync(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<BookDto>> GetDetails(int id)
        {
            var book = await _bookService.GetByIdAsync(id);
            if (book == null)
                return NotFound(new { message = $"ID {id} not found" });

            var canDarBaja = book.Stock > 0;
            return Ok(new { book, canDarBaja });
        }

        [HttpPost("{id}/darbaja")]
        public async Task<IActionResult> DarBaja(int id, [FromBody] DarBajaBookDto dto)
        {
            try
            {
                var result = await _bookService.DarBajaAsync(id, dto.Motivo);
                
                if (result)
                {
                    return Ok(new 
                    { 
                        success = true, 
                        message = $"ID {id} dado de baja.",
                        bookId = id
                    });
                }

                return BadRequest(new { message = "Fallo al dar de baja el libro." });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error request", error = ex.Message });
            }
        }
    }
}

