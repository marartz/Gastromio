using System;
using System.Collections.Generic;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.Order;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Template.DotLiquid.Tests
{
    public class TestFixture
    {
        public Order GetOrderWithCosts()
        {
            return new Order(
                new OrderId(Guid.NewGuid()),
                new CustomerInfo(
                    "Max",
                    "Mustermann",
                    "Musterstraße 1",
                    "4. Obergeschoss",
                    "12345",
                    "Musterstadt",
                    "01234/56789",
                    "max.mustermann@googlemail.com"
                ),
                new CartInfo(
                    OrderType.Pickup,
                    new RestaurantId(Guid.NewGuid()),
                    "Pizzeria Parma",
                    "Pizzeria Parma (Pizzastr. 1, 12345 Musterstadt)",
                    "012345/67890",
                    "bestellung@pizzeria-parma.de",
                    new List<OrderedDishInfo>
                    {
                        new OrderedDishInfo(
                            Guid.NewGuid(),
                            new DishId(Guid.NewGuid()),
                            "Pizza Margaritha",
                            Guid.NewGuid(),
                            "",
                            (decimal)5,
                            2,
                            "Pizza bitte warm liefern"
                        ),
                        new OrderedDishInfo(
                            Guid.NewGuid(),
                            new DishId(Guid.NewGuid()),
                            "Pizza Salami",
                            Guid.NewGuid(),
                            "",
                            (decimal)4.50,
                            2,
                            "Pizza bitte warm liefern"
                        ),
                        new OrderedDishInfo(
                            Guid.NewGuid(),
                            new DishId(Guid.NewGuid()),
                            "Insalata Mista",
                            Guid.NewGuid(),
                            "Groß",
                            (decimal)5.5,
                            1,
                            ""
                        ),
                    }
                ),
                "Bitte flott!",
                new PaymentMethodId(Guid.NewGuid()),
                "Bar",
                "Sie zahlen Bar",
                (decimal)2,
                (decimal)26.5
            );
        }
    }
}