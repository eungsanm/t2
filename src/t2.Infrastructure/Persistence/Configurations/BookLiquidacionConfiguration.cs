using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using t2.Domain.Entities;

namespace t2.Infrastructure.Persistence.Configurations
{
    public class BookLiquidacionConfiguration : IEntityTypeConfiguration<BookLiquidacion>
    {
        public void Configure(EntityTypeBuilder<BookLiquidacion> builder)
        {
            builder.ToTable("tb_articulos_liquidacion");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.BookId)
                .IsRequired(false);

            builder.Property(b => b.StockLiquidado)
                .IsRequired();

            builder.Property(b => b.FechaLiquidacion)
                .IsRequired();

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.HasOne(b => b.Book)
                .WithMany()
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

