using Document.Domain.Entities.Enum;
using System;

namespace Document.Application.DTO
{
    public class DocumentDTO
    {
        public string Number { get; set; }
        public DateTimeOffset Date { get; set; }
        public DocType DocType { get; set; }
        public Operation Operation { get; set; }
        public bool Paid { get; set; }
        public DateTimeOffset? PaymentDate { get; set; }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public string Observation { get; set; }
    }
}