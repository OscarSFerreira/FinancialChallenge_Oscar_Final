using BankRequest.Domain.Entities.Enum;
using FinancialChallenge_Oscar.Infrastructure.BaseClass;
using System;

namespace BankRequest.Domain.Entities
{
    public class BankRequest : EntityBase
    {
        public Origin? Origin { get; set; }
        public Guid? OriginId { get; set; }
        public string Description { get; set; }
        public Enum.Type Type { get; set; }
        public decimal Amount { get; set; }
    }
}
