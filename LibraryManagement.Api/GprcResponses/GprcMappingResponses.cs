using LibraryManagement.Application.DTOs;
using LibraryManagement.Contract.Authors;
using LibraryManagement.Contract.Books;
using LibraryManagement.Contract.Borrowings;
using LibraryManagement.Contract.Categories;

namespace LibraryManagement.Api.GprcResponses
{
    public static class GprcMappingResponses
    {
        public static AuthorResponse ToGrpcResponse( this AuthorDto dto)
        {
            return new AuthorResponse 
            {
                AuthorId = dto.AuthorId,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Biography = dto.Biography ?? string.Empty,
                DateOfBirth = dto.DateOfBirth.ToString(),
                IsActive = dto.IsActive,
                BookCount = dto.BookCount,
                //CreatedDate = authorDto.CreatedDate.ToString() ?? string.Empty,
                //UpdatedDate = authorDto.UpdatedDate.ToString() ?? string.Empty,
            };
        }

        public static BookResponse ToGrpcResponse(this BookDto dto)
        {
            return new BookResponse
            {
                BookId = dto.BookId,
                Title = dto.Title,
                Isbn = dto.ISBN,
                Description = dto.Description,
                AuthorId = dto.AuthorId,
                AuthorName = dto.AuthorName,
                CategoryId = dto.CategoryId,
                CategoryName = dto.CategoryName,
                PublishedDate = dto.PublishDate.ToString(),
                PageCount = dto.PageCount ?? 0,
                IsAvailable = dto.IsAvailable,
            };
        }

        public static CategoryResponse ToGrpcResponse(this CategoryDto dto)
        {
            return new CategoryResponse
            {
                CategoryId = dto.CategoryId,
                Name = dto.Name,
                Description = dto.Description,
                ParentCategoryId = dto.ParentCategoryId ?? 0,
                SortOrder = dto.SortOrder,
                IsActive = dto.IsActive,
                BookCount = dto.BookCount,
            };
        }

        public static BorrowingResponse ToGrpcResponse(this BorrowingDto dto)
        {
            return new BorrowingResponse
            {
                BorrowingId = dto.BorrowingId,
                BookId = dto.BookId,
                BookTitle = dto.BookTitle,
                UserId = dto.UserId,
                BorrowDate = dto.BorrowDate.ToShortDateString(),
                DueDate = dto.DueDate.ToString(),
                ReturnDate = dto.ReturnedDate.ToString(),
                Status = dto.Status.ToString(),
                FineAmount = (double)dto.FineAmount
            };
        }

    }
}
