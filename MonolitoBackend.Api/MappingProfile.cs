using AutoMapper;
using MonolitoBackend.Core.Entities;
using MonolitoBackend.Api.DTOs;

namespace MonolitoBackend.Api
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Saída (GET)
            CreateMap<Product, ProductDTO>() // CRIA UM MAPEAMENTO ENTRE DOIS TIPOS
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category)); // ESPECIFICA COMO MAPEAR ESSE CAMPO EM ESPECÍFICO
            CreateMap<Category, CategoryDTO>();

            // Entrada (POST/PUT)
            CreateMap<ProductCreateOrUpdateDTO, Product>();
            CreateMap<CategoryDTO, Category>(); 
        }
    }
}
