using FluentValidation;
using LibraryManagement.Application.Commands.Borrowing;

namespace LibraryManagement.Application.Validations.Borrowing
{
    public class BorrowBookCommandValidator : AbstractValidator<BorrowBookCommand>
    {
        public BorrowBookCommandValidator() 
        {
            RuleFor( x => x.UserId ).NotEmpty();
        }
    }
}
