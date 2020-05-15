"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var guid_typescript_1 = require("guid-typescript");
var OrderedDishModel = /** @class */ (function () {
    function OrderedDishModel() {
        this.itemId = guid_typescript_1.Guid.create().toString();
    }
    OrderedDishModel.prototype.getPrice = function () {
        return this.count * this.variant.price;
    };
    return OrderedDishModel;
}());
exports.OrderedDishModel = OrderedDishModel;
//# sourceMappingURL=ordered-dish.model.js.map