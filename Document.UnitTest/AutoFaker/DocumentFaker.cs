using Bogus;
using Document.Application.DTO;
using Document.Domain.Entities.Enum;
using System;
using System.Collections.Generic;

namespace Document.UnitTest.AutoFaker
{
    public class DocumentFaker
    {
        public static DocumentDTO GenerateDocDTO()
        {
            DocumentDTO document = new Faker<DocumentDTO>()
                .RuleFor(x => x.Number, x => x.Random.String(1, 256))
                .RuleFor(x => x.Date, DateTime.UtcNow)
                .RuleFor(x => x.DocType, x => x.PickRandom<DocType>())
                .RuleFor(x => x.Operation, Operation.Entry)
                .RuleFor(x => x.Paid, x => x.Random.Bool())
                .RuleFor(x => x.PaymentDate, DateTime.UtcNow)
                .RuleFor(x => x.Description, x => x.Random.String(1, 256))
                .RuleFor(x => x.Total, x => x.Random.Decimal(1, 1000))
                .RuleFor(x => x.Observation, x => x.Random.String(1, 256));

            return document;
        }

        public static Domain.Entities.Document GenerateDoc()
        {
            Domain.Entities.Document document = new Faker<Domain.Entities.Document>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.Number, x => x.Random.String(1, 256))
                .RuleFor(x => x.Date, DateTime.UtcNow)
                .RuleFor(x => x.DocType, x => x.PickRandom<DocType>())
                .RuleFor(x => x.Operation, Operation.Entry)
                .RuleFor(x => x.Paid, x => x.Random.Bool())
                .RuleFor(x => x.PaymentDate, DateTime.UtcNow)
                .RuleFor(x => x.Description, x => x.Random.String(1, 256))
                .RuleFor(x => x.Total, x => x.Random.Decimal(1, 1000))
                .RuleFor(x => x.Observation, x => x.Random.String(1, 256));

            return document;
        }

        public static List<Domain.Entities.Document> GenerateDocList()
        {
            List<Domain.Entities.Document> document = new Faker<Domain.Entities.Document>()
                .RuleFor(x => x.Id, Guid.NewGuid())
                .RuleFor(x => x.Number, x => x.Random.String(1, 256))
                .RuleFor(x => x.Date, DateTime.UtcNow)
                .RuleFor(x => x.DocType, x => x.PickRandom<DocType>())
                .RuleFor(x => x.Operation, x => x.PickRandom<Operation>())
                .RuleFor(x => x.Paid, x => x.Random.Bool())
                .RuleFor(x => x.PaymentDate, DateTime.UtcNow)
                .RuleFor(x => x.Description, x => x.Random.String(1, 256))
                .RuleFor(x => x.Total, x => x.Random.Decimal(1, 1000))
                .RuleFor(x => x.Observation, x => x.Random.String(1, 256))
                .GenerateBetween(1, 5);

            return document;
        }
    }
}