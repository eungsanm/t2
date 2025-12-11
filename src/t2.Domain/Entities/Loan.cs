namespace t2.Domain.Entities
{
    public class Loan
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public DateTime LoanDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public string Status { get; set; } = "Active"; 
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Book? Book { get; set; }
    }
}

