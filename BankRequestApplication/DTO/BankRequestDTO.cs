using BankRequest.Domain.Entities.Enum;
using System;
using Type = BankRequest.Domain.Entities.Enum.Type;

namespace BankRequest.Application.DTO
{
    public class BankRequestDTO
    {

        public Origin? Origin { get; set; }
        public Guid? OriginId { get; set; }
        public string Description { get; set; }
        public Type Type { get; set; }
        public decimal Amount { get; set; }

    }
}
