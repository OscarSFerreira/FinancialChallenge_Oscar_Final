using AutoMapper;
using Document.Application.DTO;

namespace Document.Application.Mapping
{
    public class DocumentMappingProfile : Profile
    {

        public DocumentMappingProfile()
        {

            CreateMap<Domain.Entities.Document, DocumentDTO>().ReverseMap();

        }

    }
}
