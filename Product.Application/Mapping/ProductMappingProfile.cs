using AutoMapper;
using Product.Application.DTO;

namespace Product.Application.Mapping
{
    public class ProductMappingProfile : Profile
    {

        public ProductMappingProfile()
        {
            CreateMap<Domain.Entities.Product, ProductDTO>().ReverseMap();
        }

    }
}
