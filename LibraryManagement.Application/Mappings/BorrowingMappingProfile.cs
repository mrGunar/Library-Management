using AutoMapper;
using LibraryManagement.Application.DTOs;
using LibraryManagement.Domain.Entities;

namespace LibraryManagement.Application.Mappings
{
    public class BorrowingMappingProfile : Profile
    {
        public BorrowingMappingProfile() 
        {
            CreateMap<Borrowing, BorrowingDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book));
        }
    }
}
