using AutoMapper;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappings
{
    public class BookMappingProfile : Profile
    {
        public BookMappingProfile() 
        {
            CreateMap<Book, BookDto>()
                .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author != null ? $"{src.Author.FirstName}" : "Unknown" ))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category != null ? $"{src.Category.Name}" : "Unknown"))
                .ForMember(dest => dest.BookId, opt => opt.MapFrom(src => src.BookId));
        }
    }
}
