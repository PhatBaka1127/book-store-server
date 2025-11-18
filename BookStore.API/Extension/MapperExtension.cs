using System.ComponentModel.DataAnnotations;
using System.Reflection;
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
            CreateMap<Book, GetBookResponse>().ReverseMap();
            CreateMap<Book, CreateBookRequest>().ReverseMap();
            CreateMap<Book, UpdateBookRequest>().ReverseMap()
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

    static class EnumMapper<T> where T : struct
    {
        public static T MapType(string value)
        {
            return EnumMapper<T>.Parse(value);
        }

        public static IDictionary<string, T> GetValues(bool ignoreCase)
        {
            var enumValues = new Dictionary<string, T>();

            foreach (FieldInfo fi in typeof(T).GetFields(BindingFlags.Static | BindingFlags.Public))
            {
                string key = fi.Name;

                var display = fi.GetCustomAttributes(typeof(DisplayAttribute), false) as DisplayAttribute[];
                if (display != null)
                    key = (display.Length > 0) ? display[0].Name : fi.Name;

                if (ignoreCase)
                    key = key.ToLower();

                if (!enumValues.ContainsKey(key))
                    enumValues[key] = (T)fi.GetRawConstantValue();
            }

            return enumValues;
        }

        public static T Parse(string value)
        {
            T result = default(T);

            try
            {
                result = (T)Enum.Parse(typeof(T), value, true);
            }
            catch (Exception)
            {
                if (IsParsable<T>(value))
                {
                    result = ParseDisplayValues(value, true);
                }
            }
            return result;
        }

        private static T ParseDisplayValues(string value, bool ignoreCase)
        {
            IDictionary<string, T> values = GetValues(ignoreCase);

            string key = null;
            if (ignoreCase)
                key = value.ToLower();
            else
                key = value;

            if (values.ContainsKey(key))
                return values[key];

            throw new ArgumentException(value);
        }

        public static bool IsParsable<T>(string value) where T : struct
        {
            return Enum.TryParse<T>(value, true, out _);
        }
    }
}