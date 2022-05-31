using FinancialChallenge_Oscar.Infrastructure.BaseClass;
using Product.Domain.Entities.Enums;

namespace Product.Domain.Entities
{
    public class Product : EntityBase
    {
        public string? Code { get; set; }
        public string Description { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string GTIN { get; set; }
        public string? QRCode { get; set; }
    }
}
