using AutoMapper;
using t2.Application.DTOs.Book;
using t2.Application.DTOs.Loan;
using t2.Domain.Entities;

namespace t2.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Book, BookDto>();
            CreateMap<CreateBookDto, Book>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));

            CreateMap<Loan, LoanDto>()
                .ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book != null ? src.Book.Title : string.Empty));
            CreateMap<CreateLoanDto, Loan>()
                .ForMember(dest => dest.LoanDate, opt => opt.MapFrom(src => src.LoanDate == default ? DateTime.Now : src.LoanDate))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Active"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now));
        }
    }
}

