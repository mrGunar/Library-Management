using LibraryManagement.Application.Commands.Authors;
using LibraryManagement.Application.IRepositories;
using LibraryManagement.Application.Queries;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Infrastructure.Data;
using LibraryManagement.Shared.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Infrastructure.Repositories
{
    public class AuthorRepository : IAuthorRepository
    {
        private readonly ApplicationDbContext _context;

        public AuthorRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<Author> AddAsync(CreateAuthorCommand author, CancellationToken cancellationToken = default)
        {
            var newAuthor = new Author(
                author.FirstName,
                author.LastName,
                author.Biography
                );
            await _context.Authors.AddAsync(newAuthor, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return newAuthor;
        }

        public async Task<int> GetBookCountAsync(long authorId, CancellationToken cancellationToken = default)
        {
            return await _context.Books.CountAsync(x => x.AuthorId == authorId, cancellationToken);
        }

        public async Task<Author?> GetByIdAsync(long authorId, CancellationToken cancellationToken = default)
        {
            return await _context.Authors
                .Include(x => x.Books)
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.AuthorId == authorId, cancellationToken);
        }

        public async Task<PagedResult<Author>> SearchAsync(AuthorSearchArgs args, CancellationToken cancellationToken = default)
        {
            var q = _context.Authors
                .Include(x => x.Books)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(args.SearchString))
            {
                var term = args.SearchString.ToLower();
                q = q.Where(x => EF.Functions.Like(x.FirstName, $"{term}") || EF.Functions.Like(x.LastName, $"{term}"));
            }

            if (args.IsActive.HasValue)
            {
                q = q.Where(x => x.IsActive == args.IsActive.Value);
            }

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Skip((args.PageNumber - 1) * args.PageSize)
                .Take(args.PageSize).ToListAsync();

            return PagedResult<Author>.Create(items, totalCount, args.PageNumber, args.PageSize);
        }

        public async Task<Author?> UpdateAsync(UpdateAuthorCommand author, CancellationToken cancellationToken = default)
        {

            await _context.Authors
                .Where(a => a.AuthorId == author.AuthorId)
                .ExecuteUpdateAsync(a => a
                .SetProperty(a => a.AuthorId, author.AuthorId)
                .SetProperty(a => a.FirstName, author.FirstName)
                .SetProperty(a => a.LastName, author.LastName)
                .SetProperty(a => a.Biography, author.Biography));

            await _context.SaveChangesAsync(cancellationToken);

            return await GetByIdAsync(author.AuthorId);
        }
    }
}
