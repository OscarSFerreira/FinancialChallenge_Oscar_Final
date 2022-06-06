using AutoMapper;
using BankRequest.Application.DTO;
using BankRequest.Application.Mapping;
using BankRequest.UnitTest.AutoFaker;
using Moq.AutoMock;
using Shouldly;
using Xunit;

namespace BankRequest.UnitTest.BankRequestTest
{
    public class BankRequestAutoMapperTest
    {
        private readonly AutoMocker Mocker;
        private readonly IMapper _mapper;

        public BankRequestAutoMapperTest()
        {
            Mocker = new AutoMocker();
            if (_mapper == null)
            {
                var mapConfig = new MapperConfiguration(x =>
                {
                    x.AddProfile(new BankRequestMappingProfile());
                });
                _mapper = mapConfig.CreateMapper();
            }
        }

        [Fact(DisplayName = "BankRequestAutoMapper Test")]
        public void AutoMapperBankRequest()
        {
            var bankReq = BankRequestFaker.GenerateBankReq();

            var result = _mapper.Map<BankRequestDTO>(bankReq);

            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.Origin.ShouldBe(bankReq.Origin),
                () => result.OriginId.ShouldBe(bankReq.OriginId),
                () => result.Description.ShouldBe(bankReq.Description),
                () => result.Type.ShouldBe(bankReq.Type),
                () => result.Amount.ShouldBe(bankReq.Amount)
            );
        }
    }
}