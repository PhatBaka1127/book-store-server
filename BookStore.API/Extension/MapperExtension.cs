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
        }
    }
}
