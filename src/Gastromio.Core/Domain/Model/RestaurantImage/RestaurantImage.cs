using System;
using Gastromio.Core.Domain.Model.Restaurant;
using Gastromio.Core.Domain.Model.User;

namespace Gastromio.Core.Domain.Model.RestaurantImage
{
    public class RestaurantImage
    {
        public RestaurantImage(
            RestaurantImageId id,
            RestaurantId restaurantId,
            string type,
            byte[] data,
            DateTime createdOn,
            UserId createdBy,
            DateTime updatedOn,
            UserId updatedBy
        )
        {
            Id = id;
            RestaurantId = restaurantId;
            Type = type;
            Data = data;
            CreatedOn = createdOn;
            CreatedBy = createdBy;
            UpdatedOn = updatedOn;
            UpdatedBy = updatedBy;
        }

        public RestaurantImageId Id { get; }
        
        public RestaurantId RestaurantId { get; }
        
        public string Type { get; }

        public byte[] Data { get; }
        
        public DateTime CreatedOn { get; }
        
        public UserId CreatedBy { get; }
        
        public DateTime UpdatedOn { get; }
        
        public UserId UpdatedBy { get; }
    }
}