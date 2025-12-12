using AutoMapper;
using t2.Application.DTOs.Loan;
using t2.Application.Interfaces;
using t2.Domain.Entities;
using t2.Domain.Exceptions;
using t2.Domain.Ports.Out;

namespace t2.Application.Services
{
    public class LoanService : ILoanService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LoanService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LoanDto?> GetByIdAsync(int id)
        {
            var loan = await _unitOfWork.Loans.GetWithBookAsync(id);
            return loan == null ? null : _mapper.Map<LoanDto>(loan);
        }

        public async Task<IEnumerable<LoanDto>> GetAllAsync()
        {
            var loans = await _unitOfWork.Loans.GetAllAsync();
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        public async Task<LoanDto> CreateAsync(CreateLoanDto dto)
        {
            var book = await _unitOfWork.Books.GetByIdAsync(dto.BookId);
            if (book == null)
            {
                throw new NotFoundException("Book", dto.BookId);
            }

            if (book.Stock == 0)
            {
                throw new BusinessRuleException(
                    "Stock insuficiente",
                    $"ID {dto.BookId} no tiene stock");
            }

            book.Stock -= 1;

            var loan = _mapper.Map<Loan>(dto);
            var createdLoan = await _unitOfWork.Loans.CreateAsync(loan);
            await _unitOfWork.Books.UpdateAsync(book);
            await _unitOfWork.SaveChangesAsync();

            var loanWithBook = await _unitOfWork.Loans.GetWithBookAsync(createdLoan.Id);
            return _mapper.Map<LoanDto>(loanWithBook!);
        }

        public async Task<LoanDto> ReturnLoanAsync(int id)
        {
            var loan = await _unitOfWork.Loans.GetWithBookAsync(id);
            if (loan == null)
            {
                throw new NotFoundException("Loan", id);
            }

            if (loan.Status == "Returned")
            {
                throw new BusinessRuleException(
                    "LoanAlreadyReturned",
                    $"ID {id} devuelto");
            }

            if (loan.Book != null)
            {
                loan.Book.Stock += 1;
                await _unitOfWork.Books.UpdateAsync(loan.Book);
            }

            loan.Status = "Returned";
            loan.ReturnDate = DateTime.Now;

            var updatedLoan = await _unitOfWork.Loans.UpdateAsync(loan);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LoanDto>(updatedLoan);
        }

        public async Task<IEnumerable<LoanDto>> GetByBookIdAsync(int bookId)
        {
            var bookExists = await _unitOfWork.Books.ExistsAsync(bookId);
            if (!bookExists)
            {
                throw new NotFoundException("Book", bookId);
            }

            var loans = await _unitOfWork.Loans.GetByBookIdAsync(bookId);
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }

        public async Task<IEnumerable<LoanDto>> GetByStatusAsync(string status)
        {
            var loans = await _unitOfWork.Loans.GetByStatusAsync(status);
            return _mapper.Map<IEnumerable<LoanDto>>(loans);
        }
    }
}

