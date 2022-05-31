using AutoMapper;
using BuyRequest.Application.DTO;

namespace BuyRequest.Application.Mapping
{
    public class BuyRequestMappingProfile : Profile
    {

        public BuyRequestMappingProfile()
        {
            CreateMap<Domain.Entities.BuyRequest, BuyRequestDTO>().ReverseMap();
            CreateMap<Domain.Entities.ProductRequest, ProductRequestDTO>().ReverseMap();
            CreateMap<BuyRequestDTO, Domain.Entities.BuyRequest>().ReverseMap();
        }

    }
}
