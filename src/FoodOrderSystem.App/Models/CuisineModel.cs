using FoodOrderSystem.Domain.Model.Cuisine;
using System;
using System.Text;

namespace FoodOrderSystem.App.Models
{
    public class CuisineModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public static CuisineModel FromCuisine(Cuisine cuisine)
        {
            string image = null;
            if (cuisine.Image != null && cuisine.Image.Length != 0)
            {
                var sb = new StringBuilder();
                sb.Append("data:image/png;base64,");
                sb.Append(Convert.ToBase64String(cuisine.Image));
                image = sb.ToString();
            }

            return new CuisineModel
            {
                Id = cuisine.Id.Value,
                Name = cuisine.Name,
                Image = image
            };
        }
    }
}
