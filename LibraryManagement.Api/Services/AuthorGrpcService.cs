using Grpc.Core;
using LibraryManagement.Application.IServices;
using LibraryManagement.Application.Queries;
using LibraryManagement.Contract.Authors;
using LibraryManagement.Api.GprcResponses;
using LibraryManagement.Application.Commands.Authors;
using LibraryManagement.Api.Helpers;

namespace LibraryManagement.Api.Services
{
    // TODO: Logging
    // TODO: errors handling
    public class AuthorGrpcService : AuthorService.AuthorServiceBase
    {
        private readonly IAuthorService _authorService;
        private readonly ILogger<AuthorGrpcService> _logger;


        public AuthorGrpcService(IAuthorService authorService, ILogger<AuthorGrpcService> logger)
        {
            _authorService = authorService;
            _logger = logger;
        }

        public override async Task<AuthorListResponse> GetAuthors(AuthorSearchRequest request, ServerCallContext context)
        {
            var args = new AuthorSearchArgs
            {
                SearchString = string.IsNullOrWhiteSpace(request.SearchTerm) ? null : request.SearchTerm,
                IsActive = request.IsActive ? request.IsActive : false,
                PageNumber = request.PageNumber > 0 ? request.PageNumber : 1,
                PageSize = request.PageSize > 0 ? request.PageSize : 10,
            };

            var result = await _authorService.GetAuthorsAsync(args, context.CancellationToken);
            // TOOD: Take values from the `result`
            var response = new AuthorListResponse
            {
                TotalCount = result.TotalCount,
                PageNumber = result.PagedNumber,
                PageSize = result.PagedSize,
            };

            response.Authors.AddRange(result.Items.Select(x => x.ToGrpcResponse()));
            return response;

        }
        public override async Task<AuthorGetResponse> GetAuthor(AuthorGetRequest request, ServerCallContext context)
        {
            var author = await _authorService.GetAuthorAsync(request.AuthorId, context.CancellationToken);
            return new AuthorGetResponse { Author = author.ToGrpcResponse() };
        }
        public override async Task<AuthorResponse> CreateAuthor(CreateAuthorRequest request, ServerCallContext context)
        {
            var command = new CreateAuthorCommand
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Biography = request.Biography,
                DateOfBirth = DateTimeParser.ParseDate(request.DateOfBirth),
            };

            var result = await _authorService.CreateAuthorAsync(command, context.CancellationToken);
            return result.ToGrpcResponse();


        }
        public override async Task<AuthorResponse> UpdateAuthor(UpdateAuthorRequest request, ServerCallContext context)
        {
            var command = new UpdateAuthorCommand
            {
                AuthorId = request.AuthorId,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Biography = request.Biography,
                DateOfBirth = DateTimeParser.ParseDate(request.DateOfBirth),
                IsActive = request.IsActive,
            };

            var result = await _authorService.UpdateAuthorAsync(command, context.CancellationToken);
            return result.ToGrpcResponse();
        }

    }
}
