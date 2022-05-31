using Bogus;
using BuyRequest.Application.DTO;
using BuyRequest.Domain.Entities.Enum;
using System;

namespace BuyRequest.UnitTest.AutoFaker
{
    public class BuyRequestFaker
    {

        public static BuyRequestDTO Generate()
        {

            Faker<ProductRequestDTO> prodReq = new Faker<ProductRequestDTO>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.ProductDescription, x => x.Random.String(1, 256))
                .RuleFor(x => x.ProductCategory, x => x.PickRandom<ProductCategory>())
                .RuleFor(x => x.ProductQuantity, x => x.Random.Int(1, 10))
                .RuleFor(x => x.ProductPrice, x => x.Random.Decimal(1, 100));

            BuyRequestDTO buyReq = new Faker<BuyRequestDTO>()
                .RuleFor(x => x.Code, x => x.Random.Number(1, 20000))
                .RuleFor(x => x.Date, DateTime.UtcNow)
                .RuleFor(x => x.DeliveryDate, DateTime.UtcNow)
                .RuleFor(x => x.ClientId, Guid.NewGuid())
                .RuleFor(x => x.ClientDescription, x => x.Random.String(1, 256))
                .RuleFor(x => x.ClientEmail, x => x.Person.Email)
                .RuleFor(x => x.ClientPhone, x => x.Person.Phone)
                .RuleFor(x => x.Status, x => x.PickRandom<Status>())
                .RuleFor(x => x.Discount, 0)
                .RuleFor(x => x.CostPrice, 0)
                .RuleFor(x => x.TotalPricing, 0)
                .RuleFor(x => x.Products, x => prodReq.GenerateBetween(1, 5));

            return buyReq;
        }
    }

}
