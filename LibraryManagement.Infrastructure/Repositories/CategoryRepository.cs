using LibraryManagement.Application.IRepositories;
using LibraryManagement.Application.Models;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public CategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Category category, CancellationToken cancellationToken = default)
        {
            await _context.Categories.AddAsync(category, cancellationToken);
            await _context.SaveChangesAsync();
        }

        public async Task<Category?> GetByIdAsync(long categoryId, CancellationToken cancellationToken = default)
        {
            return await _context.Categories
                .Include(x => x.ParentCategory)
                .Include(x => x.SubCategories)
                .Include(x => x.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.CategoryId == categoryId, cancellationToken);
        }

        public async Task<IReadOnlyCollection<Category>> GetCategoriesAsync(CategorySearchArgs args, CancellationToken cancellationToken = default)
        {
            var q = _context.Categories
                .Include(x => x.ParentCategory)
                .Include(x => x.Books)
                .AsNoTracking()
                .AsQueryable();

            // TODO: Double-check the correctness of this
            if (!string.IsNullOrWhiteSpace(args.SearchString))
            {
                var term = args.SearchString.ToLower();
                q = q.Where(x => EF.Functions.Like(x.Name, $"{term}"));
            }

            if (args.IsActive.HasValue)
            {
                q = q.Where(x => x.IsActive == args.IsActive.Value);
            }

            return await q.OrderBy(x => x.SortOrder).ThenBy(x => x.Name).ToListAsync(cancellationToken);
        }

        public async Task<CategoryStatistics> GetCategoryStatistic(long categoryId, CancellationToken cancellationToken = default)
        {
            var bookCount = await _context.Books.CountAsync(x => x.CategoryId == categoryId, cancellationToken);
            var activeBookCount = await _context.Books.CountAsync(x => x.CategoryId == categoryId && x.IsAvailable, cancellationToken);
            var subCategoriesCount = await _context.Categories.CountAsync(x => x.ParentCategoryId == categoryId, cancellationToken);

            return new CategoryStatistics(categoryId, bookCount, activeBookCount, subCategoriesCount);
        }

        public async Task<IReadOnlyCollection<Category>> GetCategoryTreeAsync(bool includeInactive, CancellationToken cancellationToken = default)
        {
            var categories = await _context.Categories
                .Include(x => x.Books)
                .AsNoTracking()
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name)
                .ToListAsync(cancellationToken);

            // TODO: Something brilliant
            return categories;
        }
    }
}
