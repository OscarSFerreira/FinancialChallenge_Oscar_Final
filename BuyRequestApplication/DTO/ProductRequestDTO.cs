using BuyRequest.Domain.Entities.Enum;
using System;

namespace BuyRequest.Application.DTO
{
    public class ProductRequestDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ProductDescription { get; set; }
        public ProductCategory ProductCategory { get; set; }
        public decimal ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal Total
            => ProductPrice * ProductQuantity;

    }
}
