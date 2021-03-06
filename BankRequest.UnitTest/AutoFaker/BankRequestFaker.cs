using BankRequest.Application.DTO;
using BankRequest.Domain.Entities.Enum;
using Bogus;
using System;
using System.Collections.Generic;

namespace BankRequest.UnitTest.AutoFaker
{
    public class BankRequestFaker
    {
        public static BankRequestDTO GenerateBankReqDTO()
        {
            BankRequestDTO bankReqDTO = new Faker<BankRequestDTO>()
           .RuleFor(x => x.Origin, x => x.PickRandom<Origin>())
           .RuleFor(x => x.OriginId, Guid.NewGuid())
           .RuleFor(x => x.Description, x => x.Random.String(1, 256))
           .RuleFor(x => x.Type, Domain.Entities.Enum.Type.Receive)
           .RuleFor(x => x.Amount, x => x.Random.Decimal(1, 200));

            return bankReqDTO;
        }

        public static Domain.Entities.BankRequest GenerateBankReq()
        {
            Domain.Entities.BankRequest bankReq = new Faker<Domain.Entities.BankRequest>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.Origin, x => x.PickRandom<Origin>())
                .RuleFor(x => x.OriginId, Guid.NewGuid())
                .RuleFor(x => x.Description, x => x.Random.String(1, 256))
                .RuleFor(x => x.Type, x => x.PickRandom<Domain.Entities.Enum.Type>())
                .RuleFor(x => x.Amount, x => x.Random.Decimal(1, 200));

            return bankReq;
        }

        public static List<Domain.Entities.BankRequest> GenerateBankList()
        {
            List<Domain.Entities.BankRequest> listBankReq = new Faker<Domain.Entities.BankRequest>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.Origin, x => x.PickRandom<Origin>())
                .RuleFor(x => x.OriginId, Guid.NewGuid())
                .RuleFor(x => x.Description, x => x.Random.String(1, 256))
                .RuleFor(x => x.Type, x => x.PickRandom<Domain.Entities.Enum.Type>())
                .RuleFor(x => x.Amount, x => x.Random.Decimal(1, 200))
                .GenerateBetween(1, 5);

            return listBankReq;
        }

        public static Domain.Entities.BankRequest GenerateBankReqNoOrigin()
        {
            Domain.Entities.BankRequest bankReq = new Faker<Domain.Entities.BankRequest>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.Description, x => x.Random.String(1, 256))
                .RuleFor(x => x.Type, Domain.Entities.Enum.Type.Receive)
                .RuleFor(x => x.Amount, x => x.Random.Decimal(1, 200));

            return bankReq;
        }
    }
}