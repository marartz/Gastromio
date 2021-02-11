using System;

namespace Gastromio.Persistence.MongoDB
{
    public class RestaurantImageModel
    {
        public Guid Id { get; set; }

        public Guid RestaurantId { get; set; }

        public string Type { get; set; }

        public byte[] Data { get; set; }

        public DateTimeOffset CreatedOn { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTimeOffset UpdatedOn { get; set; }

        public Guid UpdatedBy { get; set; }
    }
}
