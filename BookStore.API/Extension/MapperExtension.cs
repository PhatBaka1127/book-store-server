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
            CreateMap<User, UserResponse>().ReverseMap();

            // BOOK
            CreateMap<Book, BookDTO>().ReverseMap();
            CreateMap<Book, GetBookDTO>().ReverseMap();
            CreateMap<Book, CreateBookDTO>().ReverseMap();
            CreateMap<Book, UpdateBookDTO>().ReverseMap()
             .ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

            // ORDER DETAIL
            CreateMap<OrderDetail, OrderDetailResponse>().ReverseMap();

            // ORDER
            CreateMap<Order, OrderResponse>()
                .ForMember(dest => dest.buyerEmail, opt => opt.MapFrom(src => src.Buyer.Email))
                .ReverseMap();
            CreateMap<Order, DetailOrderResponse>().ReverseMap();
            CreateMap<Order, CreateOrderRequest>().ReverseMap();
            CreateMap<OrderResponse, OrderFilter>().ReverseMap();

            // CATEGORY
            CreateMap<Category, GetCategoryDTO>().ReverseMap();

            // SHOP
            CreateMap<Shop, CreateShopRequest>().ReverseMap();
            CreateMap<Shop, UpdateShopRequest>().ReverseMap();
            CreateMap<Shop, ShopResponse>().ReverseMap();
            CreateMap<ShopResponse, ShopFilter>().ReverseMap();
        }
    }
}