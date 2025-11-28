using AutoMapper;
using FluentValidation;
using LibraryManagement.Application.Commands.Authors;
using LibraryManagement.Application.Commands.Books;
using LibraryManagement.Application.Commands.Borrowing;
using LibraryManagement.Application.Commands.Categories;
using LibraryManagement.Application.IServices;
using LibraryManagement.Application.Mappings;
using LibraryManagement.Application.Services;
using LibraryManagement.Application.Validations.Authors;
using LibraryManagement.Application.Validations.Books;
using LibraryManagement.Application.Validations.Borrowing;
using LibraryManagement.Application.Validations.Categories;
using Microsoft.Extensions.Logging;
using SimpleInjector;

namespace LibraryManagement.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationLayer(this Container container)
        {
            // REgister Services
            container.Register<IBookService, BookService>(Lifestyle.Scoped);
            container.Register<IAuthorService, AuthorService>(Lifestyle.Scoped);
            container.Register<IBorrowingService, BorrowingService>(Lifestyle.Scoped);
            container.Register<ICategoryService, CategoryService>(Lifestyle.Scoped);

            // Register Validators
            container.Register<IValidator<CreateAuthorCommand>, CreateAuthorCommandValidator>(Lifestyle.Scoped);
            container.Register<IValidator<UpdateAuthorCommand>, UpdateAuthorCommandValidator>(Lifestyle.Scoped);

            container.Register<IValidator<CreateBookCommand>, CreateBookCommandValidator>(Lifestyle.Scoped);
            container.Register<IValidator<UpdateBookCommand>, UpdateBookCommandValidator>(Lifestyle.Scoped);

            container.Register<IValidator<BorrowBookCommand>, BorrowBookCommandValidator>(Lifestyle.Scoped);
            container.Register<IValidator<ReturnBookCommand>, ReturnBookCommandValidator>(Lifestyle.Scoped);

            container.Register<IValidator<CreateCategoryCommand>, CreateCategoryCommandValidator>(Lifestyle.Scoped);

            // Register AutoMappers
            container.RegisterSingleton<IMapper>(() =>
            {
                var loggerFactory = container.GetInstance<ILoggerFactory>();
                var config = new MapperConfiguration(cfg =>
                {
                    cfg.AddProfile<AuthorMappingProfile>();
                    cfg.AddProfile<BookMappingProfile>();
                    cfg.AddProfile<BorrowingMappingProfile>();
                    cfg.AddProfile<CategoryMappingProfile>();
                }, loggerFactory);

                return config.CreateMapper();
            });

        }
    }
}
