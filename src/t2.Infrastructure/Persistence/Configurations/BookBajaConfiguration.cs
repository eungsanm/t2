using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using t2.Domain.Entities;

namespace t2.Infrastructure.Persistence.Configurations
{
    public class BookBajaConfiguration : IEntityTypeConfiguration<BookBaja>
    {
        public void Configure(EntityTypeBuilder<BookBaja> builder)
        {
            builder.ToTable("tb_articulos_baja");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.BookId)
                .IsRequired(false);

            builder.Property(b => b.FechaBaja)
                .IsRequired();

            builder.Property(b => b.Motivo)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            builder.HasOne(b => b.Book)
                .WithMany()
                .HasForeignKey(b => b.BookId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}

