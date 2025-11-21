using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            // TODO: other restrictions
            builder.ToTable("authors");
            builder.HasKey(x => x.AuthorId);
            
            builder.Property(x => x.AuthorId).ValueGeneratedOnAdd();
            
            builder.Property(x => x.FirstName).IsRequired();

            builder.Property(x => x.LastName).IsRequired();

            builder.Property(x => x.Biography).HasMaxLength(1000);

            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder.HasIndex(x => new { x.LastName, x.FirstName });
        }
    }
}
