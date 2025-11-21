using LibraryManagement.Application.IRepositories;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Infrastructure.IRepositories;
using LibraryManagement.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using SimpleInjector;

namespace LibraryManagement.Infrastructure
{
    public static class DependencyInjection
    {
        public static void AddInfrastructureLayer(this Container container, DbContextOptions<ApplicationDbContext> dbOptions)
        {
            container.Register(() => new ApplicationDbContext(dbOptions), Lifestyle.Scoped);

            container.Register<IAuthorRepository, AuthorRepository>(Lifestyle.Scoped);
            container.Register<IBookRepository, BookRepository>(Lifestyle.Scoped);
            container.Register<ICategoryRepository, CategoryRepository>(Lifestyle.Scoped);
            container.Register<IBorrowingRepository, BorrowingRepository>(Lifestyle.Scoped);
        }
    }
}
