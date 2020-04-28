namespace FoodOrderSystem.Domain.Model.Cuisine
{
    public interface ICuisineFactory
    {
        Cuisine Create(string name, byte[] image);
    }
}
