namespace t2.Domain.Entities
{
    public class BookLiquidacion
    {
        public int Id { get; set; }
        public int? BookId { get; set; }
        public int StockLiquidado { get; set; }
        public DateTime FechaLiquidacion { get; set; } = DateTime.Now;
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public Book? Book { get; set; }
    }
}


