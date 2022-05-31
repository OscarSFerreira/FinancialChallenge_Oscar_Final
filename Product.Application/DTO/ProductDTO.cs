using Product.Domain.Entities.Enums;

namespace Product.Application.DTO
{
    public class ProductDTO
    {
        public string? Code { get; set; }
        public string Description { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public string GTIN { get; set; }
        public string? QRCode { get; set; }
    }
}
