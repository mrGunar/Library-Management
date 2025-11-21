using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands.Categories;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.IRepositories;
using LibraryManagement.Application.IServices;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Exceptions;
using LibraryManagement.Shared.Models;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Services
{
    public class CategoryService : ICategoryService
    {

        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<CreateCategoryCommand> _createValidator;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IValidator<CreateCategoryCommand> createValidator,
            IMapper mapper,
            ILogger<CategoryService> logger
            )
        {
            _categoryRepository = categoryRepository;
            _createValidator = createValidator;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CategoryDto> CreateCategoryAsync(CreateCategoryCommand command, CancellationToken cancellationToken)
        {
            await _createValidator.ValidateAndThrowAsync(command, cancellationToken);

            // TODO: Add some checks

            var category = new Category(command.Name);
            // TODO: Update the category

            await _categoryRepository.AddAsync(category, cancellationToken);

            _logger.LogInformation("Category has been created.");

            return _mapper.Map<CategoryDto>(category);
        }

        public async Task<IReadOnlyCollection<CategoryDto>> GetCategoriesAsync(CategorySearchArgs args, CancellationToken cancellationToken)
        {
            var categories = await _categoryRepository.GetCategoriesAsync(args, cancellationToken);

            return categories.Select(_mapper.Map<CategoryDto>).ToList().AsReadOnly();
        }

        public async Task<CategoryDto> GetCategoryAsync(long categoryId, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(categoryId, cancellationToken)
                ?? throw new NotFoundException(nameof(Category), categoryId);
            return _mapper.Map<CategoryDto>(category);

        }

        public async Task<CategoryStatisticDto> GetCategoryStatisticsAsync(long categoryId, CancellationToken cancellationToken)
        {
            var statistic = await _categoryRepository.GetCategoryStatistic(categoryId, cancellationToken);

            return new CategoryStatisticDto
            {
                CategoryId = statistic.CategoryId,
                BookCount = statistic.BookCount,
            }; ;
        }

        public async Task<IReadOnlyCollection<CategoryDto>> GetCategoryTreeAsync(bool includeInactive, CancellationToken cancellationToken)
        {
            var tree = await _categoryRepository.GetCategoryTreeAsync(includeInactive, cancellationToken);
            return tree.Select(_mapper.Map<CategoryDto>).ToList().AsReadOnly();
        }
    }
}
