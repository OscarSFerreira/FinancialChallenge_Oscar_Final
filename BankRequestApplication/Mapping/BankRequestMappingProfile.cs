using AutoMapper;
using BankRequest.Application.DTO;

namespace BankRequest.Application.Mapping
{
    public class BankRequestMappingProfile : Profile
    {
        public BankRequestMappingProfile()
        {
            CreateMap<Domain.Entities.BankRequest, BankRequestDTO>().ReverseMap();
        }
    }
}