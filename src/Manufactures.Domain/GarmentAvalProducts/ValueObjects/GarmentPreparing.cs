using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentAvalProducts.ValueObjects
{
    public class GarmentPreparing : ValueObject
    {
        public GarmentPreparing()
        {

        }

        public GarmentPreparing(string preparingId, string roNo, string article)
        {
            Id = preparingId;
            RONo = roNo;
            Article = article;
        }

        public string Id { get; set; }
        public string RONo { get; }
        public string Article { get; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Id;
            yield return RONo;
            yield return Article;
        }
    }
}