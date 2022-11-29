using Moonlay.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Manufactures.Domain.GarmentSample.SampleAvalProducts.ValueObjects
{
    public class GarmentSamplePreparing : ValueObject
    {
        public GarmentSamplePreparing()
        {

        }

        public GarmentSamplePreparing(string samplePreparingId, string roNo, string article)
        {
            Id = samplePreparingId;
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
