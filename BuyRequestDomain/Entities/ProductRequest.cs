using BuyRequest.Domain.Entities.Enum;
using FinancialChallenge_Oscar.Infrastructure.BaseClass;
using System;

namespace BuyRequest.Domain.Entities
{
    public class ProductRequest : EntityBase
    {
        private decimal _total;
        public Guid BuyRequestId { get; set; } 
        public virtual BuyRequest BuyRequest { get; set; }
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string ProductDescription { get; set; }
        public ProductCategory ProductCategory { get; set; } //enum
        public decimal ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal Total
        {
            get
            {
                return _total;
            }
            set
            {
                _total = Convert.ToDecimal((ProductQuantity * ProductPrice).ToString("N2"));
            }

        }

    }
}
