using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodOrderSystem.Persistence
{
    [Table("User")]
    public class UserRow
    {
        [Key]
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public virtual ICollection<RestaurantUserRow> RestaurantUsers { get; set; } = new List<RestaurantUserRow>();

    }
}
