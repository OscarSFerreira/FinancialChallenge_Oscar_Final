using BuyRequest.Domain.Entities.Enum;
using FinancialChallenge_Oscar.Infrastructure.BaseClass;
using System;
using System.Collections.Generic;

namespace BuyRequest.Domain.Entities
{
    public class BuyRequest : EntityBase
    {

        public long Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset DeliveryDate { get; set; }
        public Guid ClientId { get; set; }
        public string ClientDescription { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public Status Status { get; set; } //Enum
        public string Street { get; set; }
        public string StreetNum { get; set; }
        public string Sector { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal ProductPrices { get; set; }
        public decimal Discount { get; set; }
        public decimal CostPrice { get; set; }
        public decimal TotalPricing { get; set; }

        private List<ProductRequest> _products = new List<ProductRequest>();

        public List<ProductRequest> Products
        {
            get { return _products; }
            set { _products = value ?? new List<ProductRequest>(); }
        }

        //public virtual ICollection<ProductRequest> Products { get; set; }

    }
}
