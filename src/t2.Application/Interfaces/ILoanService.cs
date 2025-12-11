using t2.Application.DTOs.Loan;

namespace t2.Application.Interfaces
{
    public interface ILoanService
    {
        Task<LoanDto?> GetByIdAsync(int id);
        Task<IEnumerable<LoanDto>> GetAllAsync();
        Task<LoanDto> CreateAsync(CreateLoanDto loanDto);
        Task<LoanDto> ReturnLoanAsync(int id);
        Task<IEnumerable<LoanDto>> GetByBookIdAsync(int bookId);
        Task<IEnumerable<LoanDto>> GetByStatusAsync(string status);
    }
}

