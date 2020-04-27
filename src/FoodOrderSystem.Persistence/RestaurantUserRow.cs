using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderSystem.Persistence
{
    [Table("RestaurantUser")]
    public class RestaurantUserRow
    {
        public Guid RestaurantId { get; set; }
        public virtual RestaurantRow Restaurant { get; set; }
        public Guid UserId { get; set; }
        public virtual UserRow User { get; set; }
    }
}
