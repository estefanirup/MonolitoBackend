using AutoMapper;
using MonolitoBackend.Core.Entities;
using MonolitoBackend.Api.DTOs;

namespace MonolitoBackend.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => new CategoryDTO
                {
                    Name = src.Category.Name,
                    Description = src.Category.Description
                }));

            CreateMap<ProductDTO, Product>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId));
        }
    }
}
