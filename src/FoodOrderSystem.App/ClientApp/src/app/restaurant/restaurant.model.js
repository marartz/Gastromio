"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var RestaurantModel = /** @class */ (function () {
    function RestaurantModel() {
        this.address = new AddressModel();
        this.deliveryTimes = new Array();
        this.paymentMethods = new Array();
    }
    return RestaurantModel;
}());
exports.RestaurantModel = RestaurantModel;
var AddressModel = /** @class */ (function () {
    function AddressModel() {
    }
    return AddressModel;
}());
exports.AddressModel = AddressModel;
var DeliveryTimeModel = /** @class */ (function () {
    function DeliveryTimeModel() {
    }
    return DeliveryTimeModel;
}());
exports.DeliveryTimeModel = DeliveryTimeModel;
//# sourceMappingURL=restaurant.model.js.map