﻿using BuyRequest.Domain.Entities.Enum;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuyRequest.Application.DTO
{
    public class BuyRequestDTO
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public long Code { get; set; }
        public DateTimeOffset Date { get; set; }
        public DateTimeOffset? DeliveryDate { get; set; }
        public Guid ClientId { get; set; }
        public string ClientDescription { get; set; }
        public string ClientEmail { get; set; }
        public string ClientPhone { get; set; }
        public Status Status { get; set; } 
        public string Street { get; set; }
        public string StreetNum { get; set; }
        public string Sector { get; set; }
        public string Complement { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public decimal Discount { get; set; }
        public decimal CostPrice { get; set; }

        private List<ProductRequestDTO> _buyRequestProducts = new List<ProductRequestDTO>();

        public List<ProductRequestDTO> BuyRequestProducts
        {
            get { return _buyRequestProducts; }
            set { _buyRequestProducts = value ?? new List<ProductRequestDTO>(); }
        }

        public decimal TotalPricing
        => BuyRequestProducts.Any() ? BuyRequestProducts.Sum(x => x.ProductPrice * x.ProductQuantity) - Discount : 0;

        public decimal ProductPrices
            => BuyRequestProducts.Any() ? BuyRequestProducts.Sum(x => x.ProductPrice * x.ProductQuantity) : 0;
    }
}