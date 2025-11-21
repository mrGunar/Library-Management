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
        public async Task AddAsync(Author author, CancellationToken cancellationToken = default)
        {
            await _context.Authors.AddAsync(author, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<int> GetBookCountAsync(long authorId, CancellationToken cancellationToken = default)
        {
            return await _context.Books.CountAsync( x => x.AuthorId == authorId, cancellationToken );
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
                // TODO: create patterns
                q = q.Where(x => EF.Functions.Like(x.FirstName, $"{term}") || EF.Functions.Like(x.LastName, $"{term}"));
            }

            if (args.IsActive.HasValue)
            {
                q = q.Where( x => x.IsActive == args.IsActive.Value );
            }

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q
                .OrderBy(x => x.LastName)
                .ThenBy( x => x.FirstName)
                .Skip( (args.PageNumber - 1) * args.PageSize)
                .Take(args.PageSize).ToListAsync();

            return PagedResult<Author>.Create(items, totalCount, args.PageNumber, args.PageSize);
        }

        public async Task UpdateAsync(Author author, CancellationToken cancellationToken = default)
        {

            _context.Authors.Update(author);
            await _context.SaveChangesAsync();
        }
    }
}
