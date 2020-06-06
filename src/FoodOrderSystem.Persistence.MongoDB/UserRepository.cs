using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.User;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class UserRepository : IUserRepository
    {
        private readonly IMongoDatabase database;

        public UserRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<User>> FindAllAsync(CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(new BsonDocument(),
                new FindOptions<UserModel>
                {
                    Sort = Builders<UserModel>.Sort.Ascending(en => en.Email)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<IEnumerable<User>> SearchAsync(string searchPhrase,
            CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();

            FilterDefinition<UserModel> filter;
            if (!string.IsNullOrEmpty(searchPhrase))
            {
                var bsonRegEx = new BsonRegularExpression($".*{Regex.Escape(searchPhrase)}.*", "i");
                filter = Builders<UserModel>.Filter.Regex(en => en.Email, bsonRegEx);
            }
            else
            {
                filter = new BsonDocument();
            }

            var cursor = await collection.FindAsync(filter, 
                new FindOptions<UserModel>
                {
                    Sort = Builders<UserModel>.Sort.Ascending(en => en.Email)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<IEnumerable<User>> FindByRoleAsync(Role role, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<UserModel>.Filter.Eq(en => en.Role, ToDbRole(role)),
                new FindOptions<UserModel>
                {
                    Sort = Builders<UserModel>.Sort.Ascending(en => en.Email)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<UserModel>.Filter.Eq(en => en.Email, email),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task<User> FindByUserIdAsync(UserId userId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<UserModel>.Filter.Eq(en => en.Id, userId.Value),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task StoreAsync(User user, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var filter = Builders<UserModel>.Filter.Eq(en => en.Id, user.Id.Value);
            var document = ToDocument(user);
            var options = new ReplaceOptions { IsUpsert = true };
            await collection.ReplaceOneAsync(filter, document, options, cancellationToken);
        }

        public async Task RemoveAsync(UserId userId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            await collection.DeleteOneAsync(Builders<UserModel>.Filter.Eq(en => en.Id, userId.Value),
                cancellationToken);
        }

        private IMongoCollection<UserModel> GetCollection()
        {
            return database.GetCollection<UserModel>(Constants.UserCollectionName);
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

        private static User FromDocument(UserModel model)
        {
            var role = FromDbRole(model.Role);

            return new User(new UserId(model.Id),
                role,
                model.Email,
                model.PasswordSalt,
                model.PasswordHash
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

        private static UserModel ToDocument(User obj)
        {
            return new UserModel
            {
                Id = obj.Id.Value,
                Role = ToDbRole(obj.Role),
                Email = obj.Email,
                PasswordSalt = obj.PasswordSalt,
                PasswordHash = obj.PasswordHash
            };
        }
    }
}