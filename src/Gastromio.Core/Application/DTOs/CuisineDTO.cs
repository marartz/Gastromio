using System;
using Gastromio.Core.Domain.Model.Cuisine;

namespace Gastromio.Core.Application.DTOs
{
    public class CuisineDTO
    {
        public CuisineDTO(Guid id, string name, string image)
        {
            Id = id;
            Name = name;
            Image = image;
        }

        internal CuisineDTO(Cuisine cuisine)
        {
            Id = cuisine.Id.Value;
            Name = cuisine.Name;
            Image = cuisine.Image;
        }

        public Guid Id { get; }

        public string Name { get; }
    
        public string Image { get; }
    }
}
