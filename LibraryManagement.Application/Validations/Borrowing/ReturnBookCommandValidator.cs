using FluentValidation;
using LibraryManagement.Application.Commands.Borrowing;

namespace LibraryManagement.Application.Validations.Borrowing
{
    internal class ReturnBookCommandValidator : AbstractValidator<ReturnBookCommand>
    {
        public ReturnBookCommandValidator() 
        {
            RuleFor( x => x.BorrowingId ).GreaterThan(0).WithMessage("Id is required.");
        }
    }
}
