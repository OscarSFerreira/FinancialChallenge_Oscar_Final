using BankRequest.Domain.Entities.Enum;
using System;
using System.Threading.Tasks;

namespace BankRequest.ClientApi.Interfaces
{
    public interface IBankRequestClient
    {

        Task<bool> PostCashBank(Origin origin, Guid id, string description, Domain.Entities.Enum.Type type, decimal amount);

    }
}
