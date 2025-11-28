using FluentValidation;
using LibraryManagement.Application.Commands.Categories;

namespace LibraryManagement.Application.Validations.Categories
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Category Name is required.");
        }
    }
}
