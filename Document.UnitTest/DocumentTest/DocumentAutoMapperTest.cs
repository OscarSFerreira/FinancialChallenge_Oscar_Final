using AutoMapper;
using Document.Application.DTO;
using Document.Application.Mapping;
using Document.UnitTest.AutoFaker;
using Moq.AutoMock;
using Shouldly;
using Xunit;

namespace Document.UnitTest.DocumentTest
{
    public class DocumentAutoMapperTest
    {
        private readonly AutoMocker Mocker;
        private readonly IMapper _mapper;

        public DocumentAutoMapperTest()
        {
            Mocker = new AutoMocker();
            if (_mapper == null)
            {
                var mapConfig = new MapperConfiguration(x =>
                {
                    x.AddProfile(new DocumentMappingProfile());
                });
                _mapper = mapConfig.CreateMapper();
            }
        }

        [Fact(DisplayName = "DocumentAutoMapper Test")]
        public void AutoMapperDocument()
        {
            var document = DocumentFaker.GenerateDoc();

            var result = _mapper.Map<DocumentDTO>(document);

            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.Number.ShouldBe(document.Number),
                () => result.Date.ShouldBe(document.Date),
                () => result.DocType.ShouldBe(document.DocType),
                () => result.Operation.ShouldBe(document.Operation),
                () => result.Paid.ShouldBe(document.Paid),
                () => result.PaymentDate.ShouldBe(document.PaymentDate),
                () => result.Description.ShouldBe(document.Description),
                () => result.Total.ShouldBe(document.Total),
                () => result.Observation.ShouldBe(document.Observation)
            );
        }
    }
}