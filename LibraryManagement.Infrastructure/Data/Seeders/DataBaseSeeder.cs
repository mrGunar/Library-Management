using LibraryManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;


namespace LibraryManagement.Infrastructure.Data.Seeders
{
    public class DataBaseSeeder
    {
        public static void SeedData( ApplicationDbContext ctx)
        {
            SeedAuthorsAsync(ctx);
        }

        private static void SeedAuthorsAsync(ApplicationDbContext ctx)
        {
            var authors = new List<Author>
            {
                new Author("Jonh", "Wick", "Amazing", new DateTime(1982, 10, 11)),
                new Author("Patrick", "Wayne", "I don't know who is he", new DateTime(1989, 1, 1))

            };

            ctx.Authors.AddRange(authors);
            ctx.SaveChangesAsync();
        }

        private static async Task SeedCategoriesAsync(ApplicationDbContext ctx)
        {
            var categories = new List<Category>
            {
                new Category("Actions"),
                new Category("Cartoons")

            };

            await ctx.Categories.AddRangeAsync(categories);
        }

        private static async Task SeedBookAsync(ApplicationDbContext ctx)
        {
            var books = new List<Book>
            {
                new Book("1", "162000012312423", 1, 1)

            };

            await ctx.Books.AddRangeAsync(books);
        }

        private static async Task SeedBorrowingsAsync(ApplicationDbContext ctx)
        {
            var borrwonings = new List<Borrowing>
            {

            };

            await ctx.Borrowings.AddRangeAsync(borrwonings);
        }
    }
}
