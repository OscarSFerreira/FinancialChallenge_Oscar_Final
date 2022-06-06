using Bogus;
using BuyRequest.Application.DTO;
using BuyRequest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;

namespace BuyRequest.UnitTest.AutoFaker
{
    public class BuyRequestFaker
    {
        public static BuyRequestDTO GenerateDTO()
        {
            Faker<ProductRequestDTO> prodReq = new Faker<ProductRequestDTO>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.ProductDescription, x => x.Random.String(1, 256))
                .RuleFor(x => x.ProductCategory, ProductCategory.Digital)
                .RuleFor(x => x.ProductQuantity, x => x.Random.Int(1, 10))
                .RuleFor(x => x.ProductPrice, x => x.Random.Decimal(1, 100));

            BuyRequestDTO buyReq = new Faker<BuyRequestDTO>(locale: "pt_PT")
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
                .RuleFor(x => x.BuyRequestProducts, x => prodReq.GenerateBetween(1, 5));

            return buyReq;
        }

        public static Domain.Entities.BuyRequest Generate()
        {
            Faker<Domain.Entities.ProductRequest> prodReq = new Faker<Domain.Entities.ProductRequest>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.ProductDescription, x => x.Random.String(1, 256))
                .RuleFor(x => x.ProductCategory, ProductCategory.Physical)
                .RuleFor(x => x.ProductQuantity, x => x.Random.Int(1, 10))
                .RuleFor(x => x.ProductPrice, x => x.Random.Decimal(1, 100))
                .RuleFor(x => x.Total, x => x.Random.Decimal(1, 2000));

            Domain.Entities.BuyRequest buyReq = new Faker<Domain.Entities.BuyRequest>(locale: "pt_PT")
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.Code, x => x.Random.Number(1, 20000))
                .RuleFor(x => x.Date, DateTime.UtcNow)
                .RuleFor(x => x.DeliveryDate, DateTime.UtcNow)
                .RuleFor(x => x.ClientId, Guid.NewGuid())
                .RuleFor(x => x.ClientDescription, x => x.Random.String(1, 256))
                .RuleFor(x => x.ClientEmail, x => x.Person.Email)
                .RuleFor(x => x.ClientPhone, x => x.Person.Phone)
                .RuleFor(x => x.Status, Status.Received)
                .RuleFor(x => x.Street, x => x.Random.String(1, 256))
                .RuleFor(x => x.StreetNum, x => x.Random.String(1, 10))
                .RuleFor(x => x.Sector, x => x.Random.String(1, 10))
                .RuleFor(x => x.Complement, x => x.Random.String(1, 10))
                .RuleFor(x => x.City, x => x.Random.String(1, 10))
                .RuleFor(x => x.State, x => x.Random.String(1, 10))
                .RuleFor(x => x.Discount, 0)
                .RuleFor(x => x.CostPrice, 0)
                .RuleFor(x => x.TotalPricing, x => x.Random.Decimal(1, 1000))
                .RuleFor(x => x.BuyRequestProducts, x => prodReq.GenerateBetween(1, 5));

            return buyReq;
        }

        public static List<Domain.Entities.BuyRequest> GenerateList()
        {
            Faker<Domain.Entities.ProductRequest> prodReq = new Faker<Domain.Entities.ProductRequest>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.ProductDescription, x => x.Random.String(1, 256))
                .RuleFor(x => x.ProductCategory, x => x.PickRandom<ProductCategory>())
                .RuleFor(x => x.ProductQuantity, x => x.Random.Int(1, 10))
                .RuleFor(x => x.ProductPrice, x => x.Random.Decimal(1, 100));

            List<Domain.Entities.BuyRequest> buyReq = new Faker<Domain.Entities.BuyRequest>(locale: "pt_PT")
                .RuleFor(x => x.Id, Guid.NewGuid())
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
                .RuleFor(x => x.BuyRequestProducts, x => prodReq.GenerateBetween(1, 5))
                .GenerateBetween(1, 5);

            return buyReq;
        }

        public static Domain.Entities.ProductRequest GenerateProd()
        {
            Faker<Domain.Entities.ProductRequest> prodReq = new Faker<Domain.Entities.ProductRequest>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.ProductDescription, x => x.Random.String(1, 256))
                .RuleFor(x => x.ProductCategory, ProductCategory.Physical)
                .RuleFor(x => x.ProductQuantity, x => x.Random.Int(1, 10))
                .RuleFor(x => x.ProductPrice, x => x.Random.Decimal(1, 100))
                .RuleFor(x => x.Total, x => x.Random.Decimal(1, 2000));

            return prodReq;
        }
    }
}