using LibraryManagement.Application.Models;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.IRepositories
{
    public interface ICategoryRepository
    {
        Task<Category?> GetByIdAsync(long categoryId, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<Category>> GetCategoriesAsync(CategorySearchArgs args, CancellationToken cancellationToken = default);

        Task<IReadOnlyCollection<Category>> GetCategoryTreeAsync(bool includeInactive, CancellationToken cancellationToken = default);

        Task AddAsync(Category category, CancellationToken cancellationToken = default);

        Task<CategoryStatistics> GetCategoryStatistic(long categoryId, CancellationToken cancellationToken = default);
    }
}
