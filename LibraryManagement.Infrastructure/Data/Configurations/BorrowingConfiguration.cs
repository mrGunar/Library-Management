using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enum;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configuration
{
    internal class BorrowingConfiguration : IEntityTypeConfiguration<Borrowing>
    {
        public void Configure(EntityTypeBuilder<Borrowing> builder)
        {
            builder.ToTable("borrowings");

            builder.HasKey(x => x.BorrowingId);

            builder.Property(x => x.BorrowingId).ValueGeneratedOnAdd();

            builder.Property(x => x.BorrowDate).IsRequired();

            builder.Property(x => x.DueDate).IsRequired();

            builder.Property(x => x.Status).HasConversion<string>().HasDefaultValue(BorrowingStatus.Active);

            builder.HasOne(x => x.Book)
                .WithMany()
                .HasForeignKey(x => x.BookId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(x => x.BookId);
            builder.HasIndex(x => x.UserId);
            builder.HasIndex(x => x.DueDate);
        }
    }
}
