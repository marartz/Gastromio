using FoodOrderSystem.Domain.Model.User;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.Persistence
{
    public class UserRepository : IUserRepository
    {
        private readonly SystemDbContext dbContext;

        public UserRepository(SystemDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public Task<ICollection<User>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                return (ICollection<User>)dbContext.Users.Select(FromRow).ToList();
            }, cancellationToken);
        }

        public Task<ICollection<User>> SearchAsync(string searchPhrase, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                return (ICollection<User>)dbContext.Users.Where(en => EF.Functions.Like(en.Name, $"%{searchPhrase}%")).Select(FromRow).ToList();
            }, cancellationToken);
        }

        public Task<ICollection<User>> FindByRoleAsync(Role role, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbRole = ToDbRole(role);
                return (ICollection<User>)dbContext.Users.Where(en => en.Role == dbRole).Select(FromRow).ToList();
            }, cancellationToken);
        }

        public Task<User> FindByNameAsync(string name, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var row = dbContext.Users.FirstOrDefault(en => en.Name == name);
                if (row == null)
                    return null;
                return FromRow(row);
            }, cancellationToken);
        }

        public Task<User> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var row = dbContext.Users.FirstOrDefault(en => en.Id == userId.Value);
                if (row == null)
                    return null;
                return FromRow(row);
            }, cancellationToken);
        }

        public Task StoreAsync(User user, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.Users;

                var row = dbSet.FirstOrDefault(x => x.Id == user.Id.Value);
                if (row != null)
                {
                    ToRow(user, row);
                    dbSet.Update(row);
                }
                else
                {
                    row = new UserRow();
                    ToRow(user, row);
                    dbSet.Add(row);
                }

                dbContext.SaveChanges();
            }, cancellationToken);
        }

        public Task RemoveAsync(UserId userId, CancellationToken cancellationToken = default)
        {
            return Task.Factory.StartNew(() =>
            {
                var dbSet = dbContext.Users;

                var row = dbContext.Users.FirstOrDefault(en => en.Id == userId.Value);
                if (row != null)
                {
                    dbSet.Remove(row);
                    dbContext.SaveChanges();
                }
            }, cancellationToken);
        }

        private static Role FromDbRole(string role)
        {
            switch (role)
            {
                case "Customer":
                    return Role.Customer;
                case "RestaurantAdmin":
                    return Role.RestaurantAdmin;
                case "SystemAdmin":
                    return Role.SystemAdmin;
                default:
                    throw new InvalidOperationException($"unknown role with id: {role}");
            }
        }

        private static User FromRow(UserRow row)
        {
            var role = FromDbRole(row.Role);

            return new User(new UserId(row.Id),
                row.Name,
                role,
                row.PasswordSalt,
                row.PasswordHash
            );
        }

        private static string ToDbRole(Role role)
        {
            switch (role)
            {
                case Role.Customer:
                    return "Customer";
                case Role.RestaurantAdmin:
                    return "RestaurantAdmin";
                case Role.SystemAdmin:
                    return "SystemAdmin";
                default:
                    throw new InvalidOperationException($"unknown role: {role}");
            }
        }

        private static void ToRow(User obj, UserRow row)
        {
            row.Id = obj.Id.Value;
            row.Name = obj.Name;
            row.Role = ToDbRole(obj.Role);
            row.PasswordSalt = obj.PasswordSalt;
            row.PasswordHash = obj.PasswordHash;
            var role = ToDbRole(obj.Role);
        }
    }
}
