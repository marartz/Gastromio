"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var guid_typescript_1 = require("guid-typescript");
var CartModel = /** @class */ (function () {
    function CartModel() {
        this.orderId = guid_typescript_1.Guid.create().toString();
    }
    CartModel.prototype.getPriceOfOrder = function () {
        if (this.orderedDishes === undefined || this.orderedDishes.length === 0)
            return 0;
        var result = 0;
        for (var _i = 0, _a = this.orderedDishes; _i < _a.length; _i++) {
            var orderedDish = _a[_i];
            result += orderedDish.count * orderedDish.variant.price;
        }
        return result;
    };
    CartModel.prototype.getDeliveryCosts = function () {
        return this.restaurant.deliveryCosts;
    };
    CartModel.prototype.getTotalPrice = function () {
        return this.getPriceOfOrder() + this.getDeliveryCosts();
    };
    return CartModel;
}());
exports.CartModel = CartModel;
//# sourceMappingURL=cart.model.js.map