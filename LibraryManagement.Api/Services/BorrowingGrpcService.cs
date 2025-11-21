using AutoMapper;
using Grpc.Core;
using LibraryManagement.Api.GprcResponses;
using LibraryManagement.Api.Helpers;
using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.IServices;
using LibraryManagement.Application.Models;
using LibraryManagement.Application.Queries;
using LibraryManagement.Contract.Borrowings;

namespace LibraryManagement.Api.Services
{
    public class BorrowingGrpcService : BorrowingService.BorrowingServiceBase
    {
        private readonly IBorrowingService _borrowingService;
        private readonly ILogger<BorrowingGrpcService> _logger;

        public BorrowingGrpcService(
            IBorrowingService borrowingService,
            ILogger<BorrowingGrpcService> logger)
        {
            _borrowingService = borrowingService;
            _logger = logger;
        }

        public override async Task<BorrowingResponse> BorrowBook(BorrowBookRequest r, ServerCallContext ctx)
        {
            var cmd = new BorrowBookCommand
            {
                BookId = r.BookId,
                UserId = r.UserId,
                BorrowDays = r.DaysToReturn > 0 ? r.DaysToReturn : 14
            };

            var borrow = await _borrowingService.BorrowBookAsync(cmd, ctx.CancellationToken);
            return borrow.ToGrpcResponse();
        }

        public override async Task<BorrowingResponse> ReturnBook(ReturnBookRequest r, ServerCallContext ctx) 
        {
            var cmd = new ReturnBookCommand
            {
                BorrowingId = r.BorrowingId,
                // TODO: Something brilliant
                ReturnedDate = DateTime.UtcNow,
            };

            var dto = await _borrowingService.ReturnBookAsync(cmd, ctx.CancellationToken);
            return dto.ToGrpcResponse();
        }

        public override async Task<BorrowingListResponse> GetUserBorrowings(UserBorrowingsRequest r, ServerCallContext ctx) 
        {
            var status = StatusParser.ParseStatus(r.Status);
            var args = new BorrowingSearchArgs
            {
                UserId = r.UserId,
                BorrowStatus = status,
            };

            var result = await _borrowingService.GetUserBorrowingsAsync(args, ctx.CancellationToken);
            
            var response = new BorrowingListResponse
            {
                TotalCount = result.TotalCount,
                PageNumber = result.PagedNumber,
                PageSize = result.PagedSize
            };

            response.Borrowings.AddRange(result.Items.Select(x => x.ToGrpcResponse()));
            return response;
        }

        public override async Task<BorrowingListResponse> GetOverdueBooks(OverdueBooksRequest r, ServerCallContext ctx) 
        {
            // TODO: No need full args here or not?
            var args = new BorrowingSearchArgs
            {
                PageNumber = 1,
                PageSize = 20,
                BorrowStatus = Domain.Enum.BorrowingStatus.Overdue,
            };

            var result = await _borrowingService.GetOverdueBooksAsync(args, ctx.CancellationToken);
            var response = new BorrowingListResponse
            {
                TotalCount = result.TotalCount,
                PageNumber = result.PagedNumber,
                PageSize = result.PagedSize
            };
            response.Borrowings.AddRange( result.Items.Select( x => x.ToGrpcResponse() ));
            return response;
        }


    }
}
