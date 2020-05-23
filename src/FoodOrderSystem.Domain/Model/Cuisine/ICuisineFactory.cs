namespace FoodOrderSystem.Domain.Model.Cuisine
{
    public interface ICuisineFactory
    {
        Result<Cuisine> Create(string name);
    }
}
