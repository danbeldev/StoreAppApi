using AutoMapper;
using StoreAppApi.DTOs.company;
using StoreAppApi.DTOs.product;
using StoreAppApi.DTOs.user;
using StoreAppApi.models.product;
using StoreAppApi.models.user;
using StoreAppApi.models.сompany;

namespace StoreAppApi.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductItemDTO>().ReverseMap();

            CreateMap<BaseUser, BaseUserDTO>().ReverseMap();

            CreateMap<Сompany, CompanyItemDTO>().ReverseMap();
        }
    }
}
