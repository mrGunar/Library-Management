using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands.Authors;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Application.IRepositories;
using LibraryManagement.Application.IServices;
using LibraryManagement.Application.Queries;
using LibraryManagement.Application.Validations.Authors;
using LibraryManagement.Domain.Entities;
using LibraryManagement.Shared.Exceptions;
using LibraryManagement.Shared.Models;
using Microsoft.Extensions.Logging;

namespace LibraryManagement.Application.Services
{
    public class AuthorService : IAuthorService
    {

        private readonly IAuthorRepository _authorRepository;
        private readonly IMapper _mapper;
        private readonly IValidator<CreateAuthorCommand> _createValidator;
        private readonly IValidator<UpdateAuthorCommand> _updateValidator;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(
            IAuthorRepository authorRepository,
            IMapper mapper, 
            IValidator<CreateAuthorCommand> createValidator, 
            IValidator<UpdateAuthorCommand> updateValidator,
            ILogger<AuthorService> logger)
        {
            _authorRepository = authorRepository;
            _mapper = mapper;
            _createValidator = createValidator;
            _updateValidator = updateValidator;
            _logger = logger;
        }

        public async Task<AuthorDto> CreateAuthorAsync(CreateAuthorCommand command, CancellationToken cancellationToken = default)
        {
            await _createValidator.ValidateAndThrowAsync(command, cancellationToken);

            var author = new Author(command.FirstName, command.LastName);
            
            // TODO: Update the profile

            await _authorRepository.AddAsync(author, cancellationToken);
            _logger.LogInformation("Author has been created.");

            return _mapper.Map<AuthorDto>(author);
        }

        public async Task<AuthorDto> GetAuthorAsync(long authorId, CancellationToken cancellationToken = default)
        {
            var author = await _authorRepository.GetByIdAsync(authorId, cancellationToken)
                ?? throw new NotFoundException(nameof(Author), authorId);

            return _mapper.Map<AuthorDto>(author);

        }

        public Task<int> GetAuthorBookCountAsync(long authorId, CancellationToken cancellationToken = default)
        {
            return _authorRepository.GetBookCountAsync(authorId, cancellationToken);
        }

        public async Task<PagedResult<AuthorDto>> GetAuthorsAsync(AuthorSearchArgs args, CancellationToken cancellationToken = default)
        {
            var result = await _authorRepository.SearchAsync(args, cancellationToken);
            var mappedItems = result.Items.Select(_mapper.Map<AuthorDto>);
            return PagedResult<AuthorDto>.Create(mappedItems, result.TotalCount, result.PagedNumber, result.PagedSize);
        }

        public async Task<AuthorDto> UpdateAuthorAsync(UpdateAuthorCommand command, CancellationToken cancellationToken = default)
        {
            await _updateValidator.ValidateAndThrowAsync(command, cancellationToken);


            var author = await _authorRepository.GetByIdAsync(command.AuthorId, cancellationToken)
                ?? throw new NotFoundException(nameof(Author), command.AuthorId);

            // TODO: Update the profile
            author.FirstName = command.FirstName;
            author.LastName = command.LastName;
            author.Biography = command.Biography;

            await _authorRepository.UpdateAsync(author, cancellationToken);
            return _mapper.Map<AuthorDto>(author);

        }
    }
}
