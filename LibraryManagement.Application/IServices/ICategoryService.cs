using LibraryManagement.Domain.Entities;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Commands.Categories;
using LibraryManagement.Application.DTOs;

namespace LibraryManagement.Application.IServices
{
    public interface ICategoryService
    {
        Task<CategoryDto> GetCategoryAsync(long categoryId, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CategorySearchArgs args, CancellationToken cancellationToken);
        Task<IReadOnlyCollection<CategoryDto>> GetCategoryTreeAsync(bool includeInactive, CancellationToken cancellationToken);
        Task<CategoryDto> CreateCategoryAsync(CreateCategoryCommand command, CancellationToken cancellationToken);
        Task<CategoryStatisticDto> GetCategoryStatisticsAsync(long categoryId, CancellationToken cancellationToken);
    }
}