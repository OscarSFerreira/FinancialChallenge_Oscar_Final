using BuyRequest.Domain.Entities.Enum;

namespace BuyRequest.Application.DTO
{
    public class ProductRequestDTO
    {
        public string ProductDescription { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public decimal ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }

    }
}
