namespace FoodOrderSystem.Domain.Model
{
    public enum FailureResultCode
    {
        Unauthorized,
        Forbidden,
        UserDoesNotExist,
        UserAlreadyExists,
        CannotRemoveCurrentUser,
        CuisineDoesNotExist,
        CuisineAlreadyExists,
        PaymentMethodDoesNotExist,
        PaymentMethodAlreadyExists,
        RestaurantDoesNotExist,
        CannotRemoveCurrentUserFromRestaurantAdmins
    }
}
