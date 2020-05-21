namespace FoodOrderSystem.Domain.Model
{
    public enum FailureResultCode
    {
        SessionExpired,
        Forbidden,
        WrongCredentials,
        RequiredFieldEmpty,
        UserDoesNotExist,
        UserAlreadyExists,
        CannotRemoveCurrentUser,
        CuisineDoesNotExist,
        CuisineAlreadyExists,
        PaymentMethodDoesNotExist,
        PaymentMethodAlreadyExists,
        RestaurantHasToHaveAName,
        RestaurantImageNotValid,
        RestaurantDoesNotExist,
        RestaurantContainsDishCategories,
        RestaurantContainsDishes,
        DishCategoryDoesNotBelongToRestaurant,
        DishCategoryContainsDishes,
        DishDoesNotBelongToDishCategory,
        CannotRemoveCurrentUserFromRestaurantAdmins
    }
}
