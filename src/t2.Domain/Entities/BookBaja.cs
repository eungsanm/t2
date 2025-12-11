namespace t2.Domain.Entities
{
    public class BookBaja
    {
        public int Id { get; set; }
        public int? BookId { get; set; }
        public DateTime FechaBaja { get; set; } = DateTime.Now;
        public string Motivo { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Book? Book { get; set; }
    }
}


