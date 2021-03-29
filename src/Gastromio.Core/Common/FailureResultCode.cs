﻿namespace Gastromio.Core.Common
{
    public enum FailureResultCode
    {
        SessionExpired,
        Forbidden,
        InternalServerError,
        LoginEmailRequired,
        LoginPasswordRequired,
        WrongCredentials,
        PasswordResetCodeIsInvalid,
        UserDoesNotExist,
        UserAlreadyExists,
        PasswordIsNotValid,
        CannotRemoveCurrentUser,
        UserIsRestaurantAdmin,
        CuisineNameIsRequired,
        CuisineNameTooLong,
        CuisineImageTooLong,
        CuisineDoesNotExist,
        CuisineAlreadyExists,
        PaymentMethodDoesNotExist,
        PaymentMethodAlreadyExists,
        RestaurantAlreadyExists,
        RestaurantNameRequired,
        RestaurantNameTooLong,
        RestaurantAddressRequired,
        RestaurantStreetRequired,
        RestaurantStreetTooLong,
        RestaurantStreetInvalid,
        RestaurantZipCodeRequired,
        RestaurantZipCodeInvalid,
        RestaurantCityRequired,
        RestaurantCityTooLong,
        RestaurantPhoneRequired,
        RestaurantPhoneInvalid,
        RestaurantFaxInvalid,
        RestaurantWebSiteInvalid,
        RestaurantResponsiblePersonRequired,
        RestaurantEmailRequired,
        RestaurantEmailInvalid,
        RestaurantMobileInvalid,
        RestaurantImageTypeRequired,
        RestaurantImageDataTooBig,
        RestaurantImageNotValid,
        NoRestaurantPickupInfosSpecified,
        RestaurantAveragePickupTimeTooLow,
        RestaurantAveragePickupTimeTooHigh,
        RestaurantMinimumPickupOrderValueTooLow,
        RestaurantMinimumPickupOrderValueTooHigh,
        RestaurantMaximumPickupOrderValueTooLow,
        NoRestaurantDeliveryInfosSpecified,
        RestaurantAverageDeliveryTimeTooLow,
        RestaurantAverageDeliveryTimeTooHigh,
        RestaurantMinimumDeliveryOrderValueTooLow,
        RestaurantMinimumDeliveryOrderValueTooHigh,
        RestaurantMaximumDeliveryOrderValueTooLow,
        RestaurantDeliveryCostsTooLow,
        RestaurantDeliveryCostsTooHigh,
        NoRestaurantReservationInfosSpecified,
        RestaurantWithoutCashPaymentNotAllowed,
        RestaurantOpeningPeriodBeginsTooEarly,
        RestaurantOpeningPeriodEndsBeforeStart,
        RestaurantOpeningPeriodIntersects,
        RestaurantDeviatingOpeningDayDoesNotExist,
        RestaurantDeviatingOpeningDayHasStillOpenPeriods,
        RestaurantDoesNotExist,
        RestaurantNotActive,
        DishCategoryDoesNotBelongToRestaurant,
        DishCategoryAlreadyExists,
        DishCategoryDoesNotExist,
        DishCategoryNameRequired,
        DishCategoryNameTooLong,
        DishCategoryInvalidOrderNo,
        DishDoesNotBelongToDishCategory,
        DishDoesNotExist,
        DishRestaurantIdRequired,
        DishCategoryIdRequired,
        DishNameRequired,
        DishNameTooLong,
        DishDescriptionTooLong,
        DishProductInfoTooLong,
        DishInvalidOrderNo,
        DishVariantNameTooLong,
        DishVariantPriceIsNegativeOrZero,
        DishVariantPriceIsTooBig,
        CannotRemoveCurrentUserFromRestaurantAdmins,
        OrderIsInvalid,
        ImportOpeningPeriodIsInvalid,
        ImportOrderTypeIsInvalid,
        ImportCuisinesNotFound,
        ImportPaymentMethodsNotFound,
        ImportPaymentMethodNotFound,
        ImportUnknownSupportedOrderMode,
        ExternalMenuDoesNotExist,
        ExternalMenuHasNoName,
        ExternalMenuHasNoDescription,
        ExternalMenuHasNoUrl,
    }
}
