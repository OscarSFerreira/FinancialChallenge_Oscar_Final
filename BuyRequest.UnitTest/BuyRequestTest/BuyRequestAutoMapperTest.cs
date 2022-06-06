using AutoMapper;
using BuyRequest.Application.DTO;
using BuyRequest.Application.Mapping;
using BuyRequest.UnitTest.AutoFaker;
using Moq.AutoMock;
using Shouldly;
using Xunit;

namespace BuyRequest.UnitTest.BuyRequestTest
{
    public class BuyRequestAutoMapperTest
    {
        private readonly AutoMocker Mocker;
        private readonly IMapper _mapper;

        public BuyRequestAutoMapperTest()
        {
            Mocker = new AutoMocker();
            if (_mapper == null)
            {
                var mapConfig = new MapperConfiguration(x =>
                {
                    x.AddProfile(new BuyRequestMappingProfile());
                });
                _mapper = mapConfig.CreateMapper();
            }
        }

        [Fact(DisplayName = "ProductRequestAutoMapper Test")]
        public void AutoMapperProductRequest()
        {
            var prodReq = BuyRequestFaker.GenerateProd();

            var result = _mapper.Map<ProductRequestDTO>(prodReq);

            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.ProductDescription.ShouldBe(prodReq.ProductDescription),
                () => result.ProductCategory.ShouldBe(prodReq.ProductCategory),
                () => result.ProductQuantity.ShouldBe(prodReq.ProductQuantity),
                () => result.ProductPrice.ShouldBe(prodReq.ProductPrice)
                );
        }

        [Fact(DisplayName = "BuyRequestAutoMapper Test")]
        public void AutoMapperBuyRequest()
        {
            var buyReq = BuyRequestFaker.Generate();

            var result = _mapper.Map<BuyRequestDTO>(buyReq);

            result.ShouldNotBeNull();
            result.ShouldSatisfyAllConditions(
                () => result.Code.ShouldBe(buyReq.Code),
                () => result.Date.ShouldBe(buyReq.Date),
                () => result.DeliveryDate.ShouldBe(buyReq.DeliveryDate),
                () => result.ClientId.ShouldBe(buyReq.ClientId),
                () => result.ClientDescription.ShouldBe(buyReq.ClientDescription),
                () => result.ClientEmail.ShouldBe(buyReq.ClientEmail),
                () => result.ClientPhone.ShouldBe(buyReq.ClientPhone),
                () => result.Status.ShouldBe(buyReq.Status),
                () => result.Street.ShouldBe(buyReq.Street),
                () => result.StreetNum.ShouldBe(buyReq.StreetNum),
                () => result.Sector.ShouldBe(buyReq.Sector),
                () => result.Complement.ShouldBe(buyReq.Complement),
                () => result.City.ShouldBe(buyReq.City),
                () => result.State.ShouldBe(buyReq.State),
                //() => result.ProductPrices.ShouldBe(buyReq.ProductPrices),
                () => result.Discount.ShouldBe(buyReq.Discount),
                () => result.CostPrice.ShouldBe(buyReq.CostPrice)
                //() => result.TotalPricing.ShouldBe(buyReq.TotalPricing)
            );
        }
    }
}