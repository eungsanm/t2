using Microsoft.AspNetCore.Mvc;
using t2.Application.DTOs.Loan;
using t2.Application.Interfaces;
using t2.Domain.Exceptions;

namespace t2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoansController : ControllerBase
    {
        private readonly ILoanService _loanService;

        public LoansController(ILoanService loanService)
        {
            _loanService = loanService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetAll()
        {
            var loans = await _loanService.GetAllAsync();
            return Ok(loans);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<LoanDto>> GetById(int id)
        {
            var loan = await _loanService.GetByIdAsync(id);
            if (loan == null)
                return NotFound(new { message = $"ID {id} no encontrado." });

            return Ok(loan);
        }

        [HttpGet("book/{bookId}")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetByBookId(int bookId)
        {
            try
            {
                var loans = await _loanService.GetByBookIdAsync(bookId);
                return Ok(loans);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
        }

        [HttpGet("status/{status}")]
        public async Task<ActionResult<IEnumerable<LoanDto>>> GetByStatus(string status)
        {
            var loans = await _loanService.GetByStatusAsync(status);
            return Ok(loans);
        }

        [HttpPost]
        public async Task<ActionResult<LoanDto>> Create([FromBody] CreateLoanDto dto)
        {
            try
            {
                var loan = await _loanService.CreateAsync(dto);
                return CreatedAtAction(nameof(GetById), new { id = loan.Id }, loan);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("{id}/return")]
        public async Task<ActionResult<LoanDto>> ReturnLoan(int id)
        {
            try
            {
                var loan = await _loanService.ReturnLoanAsync(id);
                return Ok(loan);
            }
            catch (NotFoundException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (BusinessRuleException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }
    }
}

