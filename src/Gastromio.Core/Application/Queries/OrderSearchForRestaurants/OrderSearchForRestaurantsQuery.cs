﻿using System;
using System.Collections.Generic;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Domain.Model.Cuisine;
using Gastromio.Core.Domain.Model.Order;

namespace Gastromio.Core.Application.Queries.OrderSearchForRestaurants
{
    public class OrderSearchForRestaurantsQuery : IQuery<ICollection<RestaurantDTO>>
    {
        public OrderSearchForRestaurantsQuery(string searchPhrase, OrderType? orderType, CuisineId cuisineId, DateTime? openingHour)
        {
            SearchPhrase = searchPhrase;
            OrderType = orderType;
            CuisineId = cuisineId;
            OpeningHour = openingHour;
        }

        public string SearchPhrase { get; }
        public OrderType? OrderType { get; }
        public CuisineId CuisineId { get; }
        public DateTime? OpeningHour { get; }
    }
}