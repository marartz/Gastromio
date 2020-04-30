namespace FoodOrderSystem.Domain.Model.Cuisine
{
    public class Cuisine
    {
        public Cuisine(CuisineId id, string name)
        {
            Id = id;
            Name = name;
        }

        public CuisineId Id { get; }
        public string Name { get; private set; }

        public void Change(string name)
        {
            Name = name;
        }
    }
}
