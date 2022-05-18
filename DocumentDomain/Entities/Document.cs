using Document.Domain.Entities.Enum;
using FinancialChallenge_Oscar.Infrastructure.BaseClass;
using System;

namespace Document.Domain.Entities
{
    public class Document : EntityBase
    {

        private DateTimeOffset? _date;
        public string Number { get; set; }
        public DateTimeOffset Date { get; set; }
        public DocType DocType { get; set; }
        public Operation Operation { get; set; }
        public bool Paid { get; set; }
        public DateTimeOffset? PaymentDate
        {
            get
            {
                if (Paid == true)
                {
                    return _date = DateTimeOffset.Now;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                _date = value;
            }
        }
        public string Description { get; set; }
        public decimal Total { get; set; }
        public string Observation { get; set; }

    }
}
