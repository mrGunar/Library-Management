using FluentValidation;
using LibraryManagement.Application.Commands.Authors;

namespace LibraryManagement.Application.Validations.Authors
{
    public class UpdateAuthorCommandValidator : AbstractValidator<UpdateAuthorCommand>
    {
        public UpdateAuthorCommandValidator() 
        {
            RuleFor(x => x.AuthorId).GreaterThan(0).WithMessage("Author must be set.");
        }
    }
}
