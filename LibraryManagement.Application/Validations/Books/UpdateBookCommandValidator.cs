using FluentValidation;
using LibraryManagement.Application.Commands.Books;

namespace LibraryManagement.Application.Validations.Books
{
    public class UpdateBookCommandValidator : AbstractValidator<UpdateBookCommand>
    {
        public UpdateBookCommandValidator() 
        {
            RuleFor(x => x.ISBN).NotEmpty().WithMessage("ISBN is required.");
        }
    }
}
