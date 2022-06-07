using Bogus;
using Product.Application.DTO;
using Product.Domain.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Product.UnitTest.AutoFaker
{
    public class ProductFaker
    {
        public static ProductDTO GenerateProdDTO()
        {
            ProductDTO product = new Faker<ProductDTO>()
                .RuleFor(x => x.Code, x => x.Random.String(1, 256))
                .RuleFor(x => x.Description, x => x.Random.String(1, 256))
                .RuleFor(x => x.ProductCategory, x => x.PickRandom<ProductCategory>())
                .RuleFor(x => x.GTIN, x => x.Random.String(1, 256))
                .RuleFor(x => x.QRCode, x => x.Random.String(1, 256));

            return product;
        }

        public static Domain.Entities.Product GenerateProd()
        {
            Domain.Entities.Product product = new Faker<Domain.Entities.Product>()
                .RuleFor(x => x.Code, x => x.Random.String(1, 256))
                .RuleFor(x => x.Description, x => x.Random.String(1, 256))
                .RuleFor(x => x.ProductCategory, x => x.PickRandom<ProductCategory>())
                .RuleFor(x => x.GTIN, x => x.Random.String(1, 256))
                .RuleFor(x => x.QRCode, x => x.Random.String(1, 256));

            return product;
        }

        public static List<Domain.Entities.Product> GenerateProdList()
        {
            List<Domain.Entities.Product> product = new Faker<Domain.Entities.Product>()
                .RuleFor(x => x.Code, x => x.Random.String(1, 256))
                .RuleFor(x => x.Description, x => x.Random.String(1, 256))
                .RuleFor(x => x.ProductCategory, x => x.PickRandom<ProductCategory>())
                .RuleFor(x => x.GTIN, x => x.Random.String(1, 256))
                .RuleFor(x => x.QRCode, x => x.Random.String(1, 256))
                .GenerateBetween(1, 5);

            return product;
        }
    }
}