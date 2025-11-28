using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configuration
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("books");

            builder.HasKey(x => x.BookId);

            builder.Property(x => x.BookId).ValueGeneratedOnAdd();

            builder.Property(x => x.Title).IsRequired();

            builder.Property(x => x.Title).IsRequired();

            builder.Property(x => x.ISBN).IsRequired();

            builder.Property(x => x.Description).HasMaxLength(1000);

            builder.Property(x => x.PageCount);

            builder.Property(x => x.IsAvailable).HasDefaultValue(true);

            builder.HasOne(x => x.Author)
                .WithMany(a => a.Books)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Category)
                .WithMany(a => a.Books)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);


            builder.HasIndex(e => e.ISBN).IsUnique();

            builder.HasIndex(x => x.AuthorId);
            builder.HasIndex(x => x.AuthorId);
            builder.HasIndex(x => x.CategoryId);
        }
    }
}
