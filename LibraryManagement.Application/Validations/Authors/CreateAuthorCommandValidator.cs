
using FluentValidation;
using LibraryManagement.Application.Commands.Authors;

namespace LibraryManagement.Application.Validations.Authors
{
    public class CreateAuthorCommandValidator : AbstractValidator<CreateAuthorCommand>
    {
        public CreateAuthorCommandValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("Name cannot be empty.")
                .MaximumLength(100);
        }
    }
}
