using System;

namespace Manufactures.Dtos
{
    public abstract class BaseDto
    {
        public string CreatedBy { get; set; }

        public DateTimeOffset CreatedDate { get; set; }

        public string LastModifiedBy { get; set; }

        public DateTimeOffset? LastModifiedDate { get; set; }
    }
}
