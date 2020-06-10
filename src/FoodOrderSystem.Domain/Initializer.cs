using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Model;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.DishCategory;

namespace FoodOrderSystem.Domain
{
    public static class Initializer
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            // Register model classes
            services.AddTransient<IUserFactory, UserFactory>();
            services.AddTransient<ICuisineFactory, CuisineFactory>();
            services.AddTransient<IPaymentMethodRepository, PaymentMethodRepository>();
            services.AddTransient<IRestaurantFactory, RestaurantFactory>();
            services.AddTransient<IDishCategoryFactory, DishCategoryFactory>();
            services.AddTransient<IDishFactory, DishFactory>();

            // Register command classes
            services.AddTransient<ICommandDispatcher, CommandDispatcher>();
            CommandDispatcher.Initialize(services);

            // Register query classes
            services.AddTransient<IQueryDispatcher, QueryDispatcher>();
            QueryDispatcher.Initialize(services);

            // Initialize failure messages
            var failureMessageService = new FailureMessageService();
            services.AddSingleton<IFailureMessageService>(failureMessageService);

            var deDeCultureInfo = new CultureInfo("de-DE");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.SessionExpired, "Sie sind nicht angemeldet");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.Forbidden, "Sie sind nicht berechtigt, diese Aktion auszuführen");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.InternalServerError, "Es ist ein technischer Fehler aufgetreten. Bitte versuchen Sie es erneut bzw. kontaktieren Sie uns, wenn das Problem anhält.");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RequiredFieldEmpty, "Nicht alle Pflichtfelder sind ausgefüllt");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.FieldValueTooLong, "Feldwert zu lang");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.FieldValueInvalid, "Feldwert hat einen ungültigen Wert");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.ValueMustNotBeNegative, "Der Wert darf nicht negativ sein");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.WrongCredentials, "Emailadresse und/oder Passwort ist nicht korrekt");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.UserDoesNotExist, "Benutzer existiert nicht");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.UserAlreadyExists, "Benutzer existiert bereits");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.CannotRemoveCurrentUser, "Sie können nicht den gerade angemeldeten Benutzer löschen");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.CuisineDoesNotExist, "Cuisine existiert nicht");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.CuisineAlreadyExists, "Cuisine existiert bereits");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.PaymentMethodDoesNotExist, "Zahlungsmethode existiert nicht");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.PaymentMethodAlreadyExists, "Zahlungsmethode existiert bereits");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantImageDataTooBig, "Die Bilddatei ist zu groß (max. 1MB)");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantImageNotValid, "Die angegebene Bilddatei ist nicht gültig");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.NoRestaurantPickupInfosSpecified, "Keine Informationen über die Abholung spezifiziert");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantAveragePickupTimeTooLow, "Die durchschnittliche Zeit bis zur Abholung ist zu klein (mind. 5 Minuten)");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantAveragePickupTimeTooHigh, "Die durchschnittliche Zeit bis zur Abholung ist zu groß (max. 120 Minuten)");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantMinimumPickupOrderValueTooLow, "Der Mindestbestellwert für Abholung ist negativ");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantMinimumPickupOrderValueTooHigh, "Der Mindestbestellwert für Abholung ist zu groß (max. 50€)");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantMaximumPickupOrderValueTooLow, "Der Höchstbestellwert für Abholung ist negativ");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.NoRestaurantDeliveryInfosSpecified, "Keine Informationen über die Lieferung spezifiziert");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantAverageDeliveryTimeTooLow, "Die durchschnittliche Zeit bis zur Lieferung ist zu klein (mind. 5 Minuten)");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantAverageDeliveryTimeTooHigh, "Die durchschnittliche Zeit bis zur Lieferung ist zu groß (max. 120 Minuten)");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantMinimumDeliveryOrderValueTooLow, "Der Mindestbestellwert für Lieferung ist negativ");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantMinimumDeliveryOrderValueTooHigh, "Der Mindestbestellwert für Lieferung ist zu groß (max. 50€)");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantMaximumDeliveryOrderValueTooLow, "Der Höchstbestellwert für Lieferung ist negativ");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantDeliveryCostsTooLow, "Die Lieferkosten sind negativ");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantDeliveryCostsTooHigh, "Die Lieferkosten sind zu groß (max. 10€)");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.NoRestaurantReservationInfosSpecified, "Keine Informationen über die Reservierung spezifiziert");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantOpeningPeriodIntersects, "Die Öffnungsperiode überschneidet sich mit einer bereits eingetragenen Öffnungsperiode");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantOpeningPeriodBeginsTooEarly, "Die Öffnungsperiode darf nicht vor 4 Uhr morgens beginnen");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantOpeningPeriodEndsBeforeStart, "Das Ende der Öffnungsperiode muss nach dem Start liegen");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.RestaurantDoesNotExist, "Restaurant existiert nicht");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.DishCategoryDoesNotBelongToRestaurant, "Gerichtkategorie gehört nicht zum Restaurant");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.DishCategoryDoesNotExist, "Dish category does not exists");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.DishCategoryInvalidOrderNo, "Gerichtkategorie hat eine ungültige Reihenfolgenkennzahl");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.DishDoesNotBelongToDishCategory, "Gericht gehört nicht zur Gerichtkategorie");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.DishDoesNotExist, "Dish does not exists");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.DishInvalidOrderNo, "Gericht hat eine ungültige Reihenfolgenkennzahl");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.DishVariantPriceIsNegativeOrZero, "Das Gericht / die Variante muss einen Preis > 0 besitzen");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.DishVariantPriceIsTooBig, "Das Gericht / die Variante muss einen Preis <= 200 besitzen");
            failureMessageService.RegisterMessage(deDeCultureInfo, FailureResultCode.CannotRemoveCurrentUserFromRestaurantAdmins, "Sie können nicht den gerade angemeldeten Benutzer aus den Administratoren des Restaurants löschen");

            if (!failureMessageService.AreAllCodesRegisteredForCulture(deDeCultureInfo))
                throw new InvalidOperationException($"Not all messages for culture {deDeCultureInfo} are registered");
        }
    }
}
