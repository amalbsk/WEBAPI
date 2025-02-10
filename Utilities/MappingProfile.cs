using AutoMapper;
using Webapi.DTO;
using Webapi.Models;

namespace Webapi.Utilities
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserRegisterDto, User>();
            CreateMap<UserLoginDto, User>();
            CreateMap<AddItemDto, Inventory>();
            CreateMap<UpdateItemDto, Inventory>();
        }
    }
}
