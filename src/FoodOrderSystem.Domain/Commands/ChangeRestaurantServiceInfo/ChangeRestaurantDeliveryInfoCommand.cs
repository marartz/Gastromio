﻿using FoodOrderSystem.Domain.Model.Restaurant;

namespace FoodOrderSystem.Domain.Commands.ChangeRestaurantServiceInfo
{
    public class ChangeRestaurantServiceInfoCommand : ICommand<bool>
    {
        public ChangeRestaurantServiceInfoCommand(RestaurantId restaurantId, PickupInfo pickupInfo,
            DeliveryInfo deliveryInfo, ReservationInfo reservationInfo, string hygienicHandling)
        {
            RestaurantId = restaurantId;
            PickupInfo = pickupInfo;
            DeliveryInfo = deliveryInfo;
            ReservationInfo = reservationInfo;
            HygienicHandling = hygienicHandling;
        }

        public RestaurantId RestaurantId { get; }
        public PickupInfo PickupInfo { get; }
        public DeliveryInfo DeliveryInfo { get; }
        public ReservationInfo ReservationInfo { get; }
        public string HygienicHandling { get; }
    }
}