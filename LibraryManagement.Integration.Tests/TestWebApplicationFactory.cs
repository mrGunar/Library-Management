using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Integration.Tests;

public class TestWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

            if (descriptor != null)
            {
                services.Remove(descriptor);
            }

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite($"DataSource=file::memory:?cache=shared");
            });

            var sp = services.BuildServiceProvider();

            using (var scope = sp.CreateScope())
            {
                var scopedServices = scope.ServiceProvider;
                var db = scopedServices.GetRequiredService<ApplicationDbContext>();
                db.Database.EnsureCreated();
                SeedTestData(db);
            }
        });
    }

    private void SeedTestData(ApplicationDbContext context)
    {
        var categories = new List<Category>
        {
            new Category("Actions"),
            new Category("Cartoons")
        };

        var authors = new List<Author>
        {
            new Author
            (
                "John",
                "Doe",
                "A great author",
                new DateTime(1980, 1, 1)
            ),
            new Author
            (
                "Jane",
                "Doe",
                "Another great author",
                new DateTime(1985, 5, 15)
            )
        };

        var books = new List<Book>
        {
            new Book
            (
                "Test Book 1",
                "ISBN-001",
                1,
                1,
                "1",
                new DateTime(2020, 1, 1),
                200
            ),

        };

        var borrowings = new List<Borrowing>
        {
            new Borrowing
            (
                1,
                1,
                DateTime.UtcNow.AddDays(1),
                DateTime.UtcNow.AddDays(1)
            )
        };

        //context.Categories.AddRange(categories);
        //context.Authors.AddRange(authors);
        ////context.Books.AddRange(books);
        ////context.Borrowings.AddRange(borrowings);
        //context.SaveChanges();
    }
}

