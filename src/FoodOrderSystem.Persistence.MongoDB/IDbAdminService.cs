namespace FoodOrderSystem.Persistence.MongoDB
{
    public interface IDbAdminService
    {
        void PurgeDatabase();

        void PrepareDatabase();
    }
}