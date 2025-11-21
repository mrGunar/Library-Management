using FluentValidation;
using LibraryManagement.Application.Commands.Books;

namespace LibraryManagement.Application.Validations.Books
{
    internal class CreateBookCommandValidator : AbstractValidator<CreateBookCommand>
    {
        public CreateBookCommandValidator()
        {
            RuleFor( c => c.Title)
                .NotEmpty()
                .WithMessage("Title is required.")
                .MaximumLength(250)
                .WithMessage("Title too long.");

            RuleFor(c => c.ISBN).NotEmpty().WithMessage("ISBN is required.")
                .Must(BeValidIsbn).WithMessage("Invalid ISBN format.");

            RuleFor(c => c.AUthorId).GreaterThan(0).WithMessage("Author must be specified.");

            /* TODO: another Rules if needed */
        }

        private bool BeValidIsbn(string isbn)
        {
            /* Validation of ISBN here */
            return true;
        }
    }
}
