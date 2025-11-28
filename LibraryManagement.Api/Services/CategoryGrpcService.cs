using Grpc.Core;
using LibraryManagement.Api.GprcResponses;
using LibraryManagement.Application.Commands.Categories;
using LibraryManagement.Application.IServices;
using LibraryManagement.Application.Queries;
using LibraryManagement.Contract.Categories;

namespace LibraryManagement.Api.Services
{
    public class CategoryGrpcService : CategoryService.CategoryServiceBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryGrpcService> _logger;

        public CategoryGrpcService(
            ICategoryService categoryService,
            ILogger<CategoryGrpcService> logger)
        {
            _categoryService = categoryService;
            _logger = logger;
        }

        public override async Task<CategoryListResponse> GetCategories(CategorySearchRequest r, ServerCallContext ctx)
        {
            var args = new CategorySearchArgs
            {
                SearchString = r.SearchTerm,
                ParentCategoryId = r.ParentCategoryId,
                IsActive = r.IsActive ? r.IsActive : null,
            };

            var categories = await _categoryService.GetCategoriesAsync(args, ctx.CancellationToken);
            var response = new CategoryListResponse();
            response.Categories.AddRange(categories.Select(x => x.ToGrpcResponse()));
            return response;
        }
        public override async Task<CategoryTreeResponse> GetCategoryTree(CategoryTreeRequest r, ServerCallContext ctx)
        {
            var categories = await _categoryService.GetCategoryTreeAsync(r.IncludeInactive, ctx.CancellationToken);
            var response = new CategoryTreeResponse();
            response.Categories.AddRange(categories.Select(x => x.ToGrpcResponse()));
            return response;
        }

        public override async Task<CategoryResponse> CreateCategory(CreateCategoryRequest r, ServerCallContext ctx)
        {
            var cmd = new CreateCategoryCommand
            {
                Name = r.Name,
                Description = r.Description,
                ParentCategory = r.ParentCategoryId,
                SortOrder = r.SortOrder,
            };

            var dto = await _categoryService.CreateCategoryAsync(cmd, ctx.CancellationToken);
            return dto.ToGrpcResponse();
        }
    }
}
