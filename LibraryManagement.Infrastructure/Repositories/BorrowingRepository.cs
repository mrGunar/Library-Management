using LibraryManagement.Application.IRepositories;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Domain.Enum;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class BorrowingRepository : IBorrowingRepository
    {
        private readonly ApplicationDbContext _context;

        public BorrowingRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Borrowing borrowing, CancellationToken cancellationToken = default)
        {
            await _context.Borrowings.AddAsync(borrowing, cancellationToken);
            await _context.SaveChangesAsync();
        }

        public async Task<Borrowing?> GetActiveBorrwoingForBookAsync(long bookId, CancellationToken cancellationToken = default)
        {
            return await _context.Borrowings.AsNoTracking()
                .Where(x => x.BookId == bookId && x.Status == BorrowingStatus.Active)
                .FirstOrDefaultAsync( cancellationToken );
        }

        public async Task<Borrowing?> GetByIdAsync(long borrowingId, CancellationToken cancellationToken = default)
        {
            return await _context.Borrowings.AsNoTracking()
                .Include(x => x.Book)
                .FirstOrDefaultAsync( x => x.BorrowingId == borrowingId, cancellationToken);
        }

        public async Task<PagedResult<Borrowing>> GetOverdueBooksAsync(BorrowingSearchArgs args, CancellationToken cancellationToken = default)
        {
            var now = DateTime.UtcNow;

            var q = _context.Borrowings.Include(x => x.Book).AsNoTracking()
                .Where(x => x.Status == BorrowingStatus.Overdue || (x.Status == BorrowingStatus.Active && x.DueDate < now));
            
            var totalCount = await q.CountAsync(cancellationToken);
            var items = await q.Skip((args.PageNumber - 1) * args.PageSize).Take(args.PageSize)
                .ToListAsync(cancellationToken);

            return PagedResult<Borrowing>.Create(items, totalCount, args.PageNumber, args.PageSize);
        }

        public async Task<PagedResult<Borrowing>> GetUserBorrowingsAsync(BorrowingSearchArgs args, CancellationToken cancellationToken = default)
        {
            var q = _context.Borrowings.Include(x => x.Book).AsNoTracking()
                .AsQueryable();

            if (args.UserId.HasValue)
            {
                q = q.Where( x => x.UserId == args.UserId.Value );
            }

            if (args.BorrowStatus.HasValue)
            {
                q = q.Where( x => x.Status == args.BorrowStatus.Value );
            } else if (args.OverdueOnly)
            {
                q = q.Where(x => x.Status == BorrowingStatus.Overdue);
            }

            var totalCount = await q.CountAsync(cancellationToken);
            var items = await q.Skip((args.PageNumber - 1) * args.PageSize).Take(args.PageSize)
                .ToListAsync(cancellationToken);

            return PagedResult<Borrowing>.Create(items, totalCount, args.PageNumber, args.PageSize);

        }

        public async Task UpdateAsync(Borrowing borrowing, CancellationToken cancellationToken = default)
        {
            _context.Borrowings.Update(borrowing);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
