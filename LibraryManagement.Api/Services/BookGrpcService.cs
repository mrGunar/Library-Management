using AutoMapper;
using Grpc.Core;
using LibraryManagement.Api.GprcResponses;
using LibraryManagement.Api.Helpers;
using LibraryManagement.Application.Commands.Books;
using LibraryManagement.Application.IServices;
using LibraryManagement.Application.Queries;
using LibraryManagement.Contract.Books;

namespace LibraryManagement.Api.Services
{
    public class BookGrpcService : BookService.BookServiceBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookGrpcService> _logger;
        private readonly IMapper _mapper;

        public BookGrpcService(IBookService bookService, ILogger<BookGrpcService> logger, IMapper mapper)
        {
            _bookService = bookService;
            _logger = logger;
            _mapper = mapper;
        }

        public override async Task<BookListResponse> GetBooks(BookSearchRequest r, ServerCallContext ctx)
        {
            // TODO: Add try-catch
            // TODO: Add logs
            // TODO: Add Paged Result

            var args = new BookSearchArgs
            {
                SearchString = string.IsNullOrWhiteSpace(r.SearchTerm) ? null : r.SearchTerm,
                AuthorId = r.AuthorId > 0 ? r.AuthorId : null,
                CategoryId = r.CategoryId > 0 ? r.CategoryId : null,
                IsAvailable = r.IsAvailable ? r.IsAvailable : null,
                PageNumber = r.PageNumber > 0 ? r.PageNumber : 1,
                PageSize = r.PageSize > 0 ? r.PageSize : 20,
            };


            var result = await _bookService.GetBooksAsync(args, ctx.CancellationToken);
            var response = new BookListResponse
            {
                TotalCount = result.TotalCount,
                PageNumber = result.PagedNumber,
                PageSize = result.PagedSize
            };

            response.Books.AddRange(result.Items.Select(x => x.ToGrpcResponse()));

            return _mapper.Map<BookListResponse>(result);
        }

        public override async Task<BookGetResponse> GetBook( BookGetRequest r, ServerCallContext ctx)
        {
            var book = await _bookService.GetBookAsync(r.BookId, ctx.CancellationToken);

            return new BookGetResponse { Book = book.ToGrpcResponse() };
        }

        public override async Task<BookResponse> CreateBook(CreateBookRequest r, ServerCallContext ctx)
        {
            var command = new CreateBookCommand
            {
                Title = r.Title,
                ISBN = r.Isbn,
                Description = string.IsNullOrWhiteSpace(r.Description) ? null : r.Description,
                AUthorId = r.AuthorId,
                CategoryId = r.CategoryId,
                PublishedDate = DateTimeParser.ParseDate(r.PublishedDate),
                PageCount = r.PageCount > 0 ? r.PageCount : null
            };

            var newBook = await _bookService.CreateBookAsync(command, ctx.CancellationToken);
            return newBook.ToGrpcResponse();
        }

        public override async Task<BookResponse> UpdateBook(UpdateBookRequest r, ServerCallContext ctx)
        {
            // TODO: Check if the book is available
            var command = new UpdateBookCommand
            {
                BookId = r.BookId,
                Title = r.Title,
                Description = string.IsNullOrWhiteSpace(r.Description) ? null : r.Description,
                CategoryId = r.CategoryId,
                PublishedDate = DateTimeParser.ParseDate(r.PublishedDate),
                PageCount = r.PageCount > 0 ? r.PageCount : null
            };

            var book = await _bookService.UpdateBookAsync(command,ctx.CancellationToken);
            return book.ToGrpcResponse();
        }

        public override async Task<DeleteResponse> DeleteBook(BookDeleteRequest r, ServerCallContext ctx)
        {
            await _bookService.DeleteBookAsync(r.BookId, ctx.CancellationToken);
            return new DeleteResponse { Success = true, Message = "The book has been deleted." };
        }


    }
}
