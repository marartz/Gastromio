namespace FoodOrderSystem.Domain.Model.Cuisine
{
    public class Cuisine
    {
        public Cuisine(CuisineId id, string name, byte[] image)
        {
            Id = id;
            Name = name;
            Image = image;
        }

        public CuisineId Id { get; }
        public string Name { get; private set; }
        public byte[] Image { get; private set; }

        public void Change(string name, byte[] image)
        {
            Name = name;
            Image = image;
        }
    }
}
