using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LibraryManagement.Infrastructure.Data.Configuration
{
    internal class CategoryConfiguration : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            builder.ToTable("categories");

            builder.HasKey( x => x.CategoryId );

            builder.Property(x => x.CategoryId).ValueGeneratedOnAdd();

            builder.Property(x => x.Name).IsRequired();
            builder.Property(x => x.Description).HasMaxLength(1000);
            builder.Property(x => x.SortOrder).HasDefaultValue(0);
            builder.Property(x => x.IsActive).HasDefaultValue(true);

            builder.HasOne(x => x.ParentCategory)
                .WithMany( x => x.SubCategories)
                .HasForeignKey( x => x.ParentCategoryId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex( x => x.Name).IsUnique();
            builder.HasIndex(x => x.ParentCategoryId);



        }
    }
}
