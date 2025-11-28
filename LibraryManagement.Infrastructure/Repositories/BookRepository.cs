using LibraryManagement.Application.Commands.Books;
using LibraryManagement.Application.IRepositories;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace LibraryManagement.Infrastructure.IRepositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ApplicationDbContext _context;

        public BookRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Book> AddAsync(CreateBookCommand book, CancellationToken cancellationToken = default)
        {
            var newBook = new Book(
                 book.Title,
                 book.ISBN,
                 book.AUthorId,
                 book.CategoryId,
                 book.Description,
                 book.PublishedDate,
                 book.PageCount);

            await _context.Books.AddAsync(newBook, cancellationToken);
            await _context.SaveChangesAsync();
            return newBook;
        }

        public async Task<int> CountAsync(Expression<Func<Book, bool>> predicates, CancellationToken cancellationToken = default)
        {
            return await _context.Books.CountAsync(predicates, cancellationToken);
        }

        public async Task DeleteAsync(Book book, CancellationToken cancellationToken = default)
        {
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
        }

        public async Task<Book?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            return await _context.Books
                .Include(x => x.Author)
                .Include(x => x.Category)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.BookId == id, cancellationToken);
        }

        public async Task<PagedResult<Book>> SearchAsync(BookSearchArgs args, CancellationToken cancellationToken = default)
        {
            var q = _context.Books
                .Include(x => x.Author)
                .Include(x => x.Category)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(args.SearchString))
            {
                var term = args.SearchString.ToLower();
                q = q.Where(x => EF.Functions.Like(x.Title, $"{term}"));
            }

            if (args.AuthorId.HasValue)
            {
                q = q.Where(x => x.AuthorId == args.AuthorId.Value);
            }

            if (args.CategoryId.HasValue)
            {
                q = q.Where(x => x.CategoryId == args.CategoryId.Value);
            }
            if (args.IsAvailable.HasValue)
            {
                q = q.Where(x => x.IsAvailable == args.IsAvailable.Value);
            }

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q.Skip((args.PageNumber - 1) * args.PageSize)
                                .Take(args.PageSize)
                                .ToListAsync();
            return PagedResult<Book>.Create(items, totalCount, args.PageNumber, args.PageSize);

        }

        public async Task<Book?> UpdateAsync(UpdateBookCommand book, CancellationToken cancellationToken = default)
        {
            await _context.Books
                .Where(b => b.BookId == book.BookId)
                .ExecuteUpdateAsync(b => b
                    .SetProperty(b => b.Title, book.Title)
                    .SetProperty(b => b.Description, book.Description)
                    .SetProperty(b => b.PageCount, book.PageCount));
            await _context.SaveChangesAsync(cancellationToken);

            return await GetByIdAsync(book.BookId);
        }

        public async Task MarkAsInactiveAsync(long bookId, CancellationToken cancellationToken = default)
        {

            await _context.Books
                .Where(b => b.BookId == bookId)
                .ExecuteUpdateAsync(b => b
                    .SetProperty(b => b.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(b => b.IsAvailable, false));
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task UnmarkAsInactiveAsync(long bookId, CancellationToken cancellationToken = default)
        {

            await _context.Books
                .Where(b => b.BookId == bookId)
                .ExecuteUpdateAsync(b => b
                    .SetProperty(b => b.UpdatedDate, DateTime.UtcNow)
                    .SetProperty(b => b.IsAvailable, true));
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
