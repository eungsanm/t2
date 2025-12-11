namespace t2.Application.DTOs.Loan
{
    public class CreateLoanDto
    {
        public int BookId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
    }
}

