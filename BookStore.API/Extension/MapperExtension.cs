using System.ComponentModel.DataAnnotations;
using AutoMapper;
using BookStore.Business.Dto;
using BookStore.Data.Entity;

namespace BookStore.API.Extension
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // USER
            CreateMap<User, GetUserDTO>().ReverseMap();

            // BOOK
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Book, GetBookDTO>().ReverseMap();
            CreateMap<Book, CreateBookDTO>().ReverseMap();
            CreateMap<Book, UpdateBookDTO>().ReverseMap()
             .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ORDER DETAIL
            CreateMap<OrderDetail, GetOrderDetailDTO>().ReverseMap();

            // ORDER
            CreateMap<Order, GetOrderDTO>().ReverseMap();
            CreateMap<Order, GetDetailOrderDTO>().ReverseMap();
            CreateMap<Order, CreateOrderDTO>().ReverseMap();
            CreateMap<GetOrderDTO, OrderFilter>().ReverseMap();

            // CATEGORY
            CreateMap<Category, GetCategoryDTO>().ReverseMap();
        }
    }
}