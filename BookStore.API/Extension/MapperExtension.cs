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
            CreateMap<Book, GetBookDTO>().ReverseMap();
            CreateMap<Book, CreateBookDTO>().ReverseMap();

            // ORDER DETAIL
            CreateMap<OrderDetail, GetOrderDetailDTO>().ReverseMap();

            // ORDER
            CreateMap<Order, GetOrderDTO>().ReverseMap();
            CreateMap<Order, CreateOrderDTO>().ReverseMap();

            // CATEGORY
            CreateMap<Category, GetCategoryDTO>().ReverseMap();
        }
    }
}
