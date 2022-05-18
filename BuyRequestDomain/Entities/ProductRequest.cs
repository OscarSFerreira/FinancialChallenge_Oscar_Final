using BuyRequest.Domain.Entities.Enum;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace BuyRequest.Domain.Entities
{
    public class ProductRequest
    {

        private decimal _total;

        public Guid Id { get; set; } = Guid.NewGuid();
        [ForeignKey("Fk_BuyRequests")]
        public Guid RequestId { get; set; }
        public Guid ProductId { get; set; } = Guid.NewGuid();
        public string ProductDescription { get; set; }
        public ProductCategory ProductCategory { get; set; } //enum
        public decimal ProductQuantity { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal Total
        {
            get
            {
                return _total = Convert.ToDecimal((ProductQuantity * ProductPrice).ToString("N2"));
            }
            set
            {
                _total = value;
            }

        }

        [JsonIgnore]
        public virtual BuyRequest BuyRequests { get; set; }

    }
}
