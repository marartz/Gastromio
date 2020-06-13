using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.Order;
using FoodOrderSystem.Domain.Model.Restaurant;
using MongoDB.Driver;

namespace FoodOrderSystem.Persistence.MongoDB
{
    public class OrderRepository : IOrderRepository
    {
        private readonly IMongoDatabase database;

        public OrderRepository(IMongoDatabase database)
        {
            this.database = database;
        }

        public async Task<IEnumerable<Order>> FindByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<OrderModel>.Filter.Eq(en => en.CartInfo.RestaurantId, restaurantId.Value),
                new FindOptions<OrderModel>
                {
                    Sort = Builders<OrderModel>.Sort.Ascending(en => en.Id)
                },
                cancellationToken);
            return cursor.ToEnumerable().Select(FromDocument);
        }

        public async Task<Order> FindByOrderIdAsync(OrderId orderId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var cursor = await collection.FindAsync(Builders<OrderModel>.Filter.Eq(en => en.Id, orderId.Value),
                cancellationToken: cancellationToken);
            var document = await cursor.FirstOrDefaultAsync(cancellationToken);
            return document != null ? FromDocument(document) : null;
        }

        public async Task StoreAsync(Order order, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            var filter = Builders<OrderModel>.Filter.Eq(en => en.Id, order.Id.Value);
            var document = ToDocument(order);
            var options = new ReplaceOptions { IsUpsert = true };
            await collection.ReplaceOneAsync(filter, document, options, cancellationToken);
        }

        public async Task RemoveByRestaurantIdAsync(RestaurantId restaurantId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            await collection.DeleteManyAsync(Builders<OrderModel>.Filter.Eq(en => en.CartInfo.RestaurantId, restaurantId.Value),
                cancellationToken);
        }

        public async Task RemoveAsync(OrderId orderId, CancellationToken cancellationToken = default)
        {
            var collection = GetCollection();
            await collection.DeleteOneAsync(Builders<OrderModel>.Filter.Eq(en => en.Id, orderId.Value),
                cancellationToken);
        }
        
        private IMongoCollection<OrderModel> GetCollection()
        {
            return database.GetCollection<OrderModel>(Constants.OrderCollectionName);
        }

        private static OrderType FromDbOrderType(string orderType)
        {
            switch (orderType)
            {
                case "pickup":
                    return OrderType.Pickup;
                case "delivery":
                    return OrderType.Delivery;
                case "reservation":
                    return OrderType.Reservation;
                default:
                    throw new InvalidOperationException($"unknown order type: {orderType}");
            }
        }
        
        private static Order FromDocument(OrderModel row)
        {
            return new Order(
                new OrderId(row.Id),
                row.CustomerInfo != null ? new CustomerInfo(
                    row.CustomerInfo.GivenName,
                    row.CustomerInfo.LastName,
                    row.CustomerInfo.Street,
                    row.CustomerInfo.AddAddressInfo,
                    row.CustomerInfo.ZipCode,
                    row.CustomerInfo.City,
                    row.CustomerInfo.Phone,
                    row.CustomerInfo.Email
                ) : null,
                row.CartInfo != null ? new CartInfo(
                    FromDbOrderType(row.CartInfo.OrderType),
                    new RestaurantId(row.CartInfo.RestaurantId),
                    row.CartInfo.RestaurantInfo,
                    row.CartInfo.OrderedDishes?.Select(en => new OrderedDishInfo(
                        en.ItemId,
                        new DishId(en.DishId),
                        en.DishName,
                        en.VariantId,
                        en.VariantName,
                        (decimal)en.VariantPrice,
                        en.Count,
                        en.Remarks
                    )).ToList()
                ) : null,
                row.Comments
            );
        }

        private static string ToDbOrderType(OrderType orderType)
        {
            switch (orderType)
            {
                case OrderType.Pickup:
                    return "pickup";
                case OrderType.Delivery:
                    return "delivery";
                case OrderType.Reservation:
                    return "reservation";
                default:
                    throw new InvalidOperationException($"unknown order type: {orderType}");
            }
        }

        private static OrderModel ToDocument(Order obj)
        {
            return new OrderModel
            {
                Id = obj.Id.Value,
                CustomerInfo = obj.CustomerInfo != null
                    ? new CustomerInfoModel
                    {
                        GivenName = obj.CustomerInfo.GivenName,
                        LastName = obj.CustomerInfo.LastName,
                        Street = obj.CustomerInfo.Street,
                        AddAddressInfo = obj.CustomerInfo.AddAddressInfo,
                        ZipCode = obj.CustomerInfo.ZipCode,
                        City = obj.CustomerInfo.City,
                        Phone = obj.CustomerInfo.Phone,
                        Email = obj.CustomerInfo.Email
                    }
                    : null,
                CartInfo = obj.CartInfo != null
                    ? new CartInfoModel
                    {
                        OrderType = ToDbOrderType(obj.CartInfo.OrderType),
                        RestaurantId = obj.CartInfo.RestaurantId.Value,
                        RestaurantInfo = obj.CartInfo.RestaurantInfo,
                        OrderedDishes = obj.CartInfo.OrderedDishes?.Select(en => new OrderedDishInfoModel
                        {
                            ItemId = en.ItemId,
                            DishId = en.DishId.Value,
                            DishName = en.DishName,
                            VariantId = en.VariantId,
                            VariantName = en.VariantName,
                            VariantPrice = (double)en.VariantPrice,
                            Count = en.Count,
                            Remarks = en.Remarks
                        }).ToList()
                    }
                    : null,
                Comments = obj.Comments
            };
        }
    }
}