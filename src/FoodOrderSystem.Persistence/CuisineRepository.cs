using FoodOrderSystem.Domain.Model.Cuisine;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Persistence
{
    public class CuisineRepository : ICuisineRepository
    {
        private readonly SystemDbContext dbContext;

        public CuisineRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<ICollection<Cuisine>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                return (ICollection<Cuisine>)dbContext.Cuisines.OrderBy(en => en.Name).Select(en => FromRow(en)).ToList();
            }, cancellationToken);
        }

        public Task<Cuisine> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var row = dbContext.Cuisines.FirstOrDefault(en => en.Name == name);
                if (row == null)
                    return null;
                return FromRow(row);
            }, cancellationToken);
        }

        public Task<Cuisine> FindByCuisineIdAsync(CuisineId cuisineId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var row = dbContext.Cuisines.FirstOrDefault(en => en.Id == cuisineId.Value);
                if (row == null)
                    return null;
                return FromRow(row);
            }, cancellationToken);
        }

        public Task StoreAsync(Cuisine cuisine, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.Cuisines;

                var row = dbSet.FirstOrDefault(x => x.Id == cuisine.Id.Value);
                if (row != null)
                {
                    ToRow(cuisine, row);
                    dbSet.Update(row);
                }
                else
                {
                    row = new CuisineRow();
                    ToRow(cuisine, row);
                    dbSet.Add(row);
                }

                dbContext.SaveChanges();
            }, cancellationToken);
        }

        public Task RemoveAsync(CuisineId cuisineId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.Cuisines;

                var row = dbSet.FirstOrDefault(en => en.Id == cuisineId.Value);
                if (row != null)
                {
                    dbSet.Remove(row);
                    dbContext.SaveChanges();
                }
            }, cancellationToken);
        }

        private static Cuisine FromRow(CuisineRow row)
        {
            return new Cuisine(
                new CuisineId(row.Id),
                row.Name
            );
        }

        private static void ToRow(Cuisine obj, CuisineRow row)
        {
            row.Id = obj.Id.Value;
            row.Name = obj.Name;
        }
    }
}
