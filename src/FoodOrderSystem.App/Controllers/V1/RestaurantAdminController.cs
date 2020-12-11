using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Core.Application.Commands;
using FoodOrderSystem.Core.Application.Commands.AddDishCategoryToRestaurant;
using FoodOrderSystem.Core.Application.Commands.AddOpeningPeriodToRestaurant;
using FoodOrderSystem.Core.Application.Commands.AddOrChangeDishOfRestaurant;
using FoodOrderSystem.Core.Application.Commands.AddOrChangeExternalMenuOfRestaurant;
using FoodOrderSystem.Core.Application.Commands.AddPaymentMethodToRestaurant;
using FoodOrderSystem.Core.Application.Commands.ChangeDishCategoryOfRestaurant;
using FoodOrderSystem.Core.Application.Commands.ChangeOpeningPeriodOfRestaurant;
using FoodOrderSystem.Core.Application.Commands.ChangeRestaurantAddress;
using FoodOrderSystem.Core.Application.Commands.ChangeRestaurantContactInfo;
using FoodOrderSystem.Core.Application.Commands.ChangeRestaurantImage;
using FoodOrderSystem.Core.Application.Commands.ChangeRestaurantServiceInfo;
using FoodOrderSystem.Core.Application.Commands.ChangeSupportedOrderModeOfRestaurant;
using FoodOrderSystem.Core.Application.Commands.DecOrderOfDish;
using FoodOrderSystem.Core.Application.Commands.DecOrderOfDishCategory;
using FoodOrderSystem.Core.Application.Commands.IncOrderOfDish;
using FoodOrderSystem.Core.Application.Commands.IncOrderOfDishCategory;
using FoodOrderSystem.Core.Application.Commands.RemoveDishCategoryFromRestaurant;
using FoodOrderSystem.Core.Application.Commands.RemoveDishFromRestaurant;
using FoodOrderSystem.Core.Application.Commands.RemoveExternalMenuFromRestaurant;
using FoodOrderSystem.Core.Application.Commands.RemoveOpeningPeriodFromRestaurant;
using FoodOrderSystem.Core.Application.Commands.RemovePaymentMethodFromRestaurant;
using FoodOrderSystem.Core.Application.DTOs;
using FoodOrderSystem.Core.Application.Queries;
using FoodOrderSystem.Core.Application.Queries.GetAllCuisines;
using FoodOrderSystem.Core.Application.Queries.GetAllPaymentMethods;
using FoodOrderSystem.Core.Application.Queries.GetDishesOfRestaurant;
using FoodOrderSystem.Core.Application.Queries.GetRestaurantById;
using FoodOrderSystem.Core.Application.Queries.RestAdminMyRestaurants;
using FoodOrderSystem.Core.Application.Services;
using FoodOrderSystem.Core.Domain.Model.Dish;
using FoodOrderSystem.Core.Domain.Model.DishCategory;
using FoodOrderSystem.Core.Domain.Model.PaymentMethod;
using FoodOrderSystem.Core.Domain.Model.Restaurant;
using FoodOrderSystem.Core.Domain.Model.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1/restaurantadmin")]
    [ApiController]
    [Authorize()]
    public class RestaurantAdminController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly IFailureMessageService failureMessageService;

        public RestaurantAdminController(ILogger<RestaurantAdminController> logger,
            ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher,
            IFailureMessageService failureMessageService)
        {
            this.logger = logger;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
            this.failureMessageService = failureMessageService;
        }

        [Route("myrestaurants")]
        [HttpGet]
        public async Task<IActionResult> GetMyRestaurantsAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var queryResult =
                await queryDispatcher.PostAsync<RestAdminMyRestaurantsQuery, ICollection<RestaurantDTO>>(
                    new RestAdminMyRestaurantsQuery(), new UserId(currentUserId));
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants/{restaurant}")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantAsync(string restaurant)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var queryResult =
                await queryDispatcher.PostAsync<GetRestaurantByIdQuery, RestaurantDTO>(
                    new GetRestaurantByIdQuery(restaurant, false), new UserId(currentUserId));

            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants/{restaurant}/dishes")]
        [HttpGet]
        public async Task<IActionResult> GetDishesOfRestaurantAsync(string restaurant)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var queryResult =
                await queryDispatcher.PostAsync<GetDishesOfRestaurantQuery, ICollection<DishCategoryDTO>>(
                    new GetDishesOfRestaurantQuery(restaurant), new UserId(currentUserId));
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("cuisines")]
        [HttpGet]
        public async Task<IActionResult> GetCuisinesAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var queryResult =
                await queryDispatcher.PostAsync<GetAllCuisinesQuery, ICollection<CuisineDTO>>(new GetAllCuisinesQuery(),
                    new UserId(currentUserId));
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("paymentmethods")]
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethodsAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var queryResult =
                await queryDispatcher.PostAsync<GetAllPaymentMethodsQuery, ICollection<PaymentMethodDTO>>(
                    new GetAllPaymentMethodsQuery(), new UserId(currentUserId));
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changeimage")]
        [HttpPost]
        public async Task<IActionResult> PostChangeImageAsync(Guid restaurantId,
            [FromBody] ChangeRestaurantImageModel changeRestaurantImageModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var image = string.IsNullOrWhiteSpace(changeRestaurantImageModel.Image)
                ? null
                : ImageHelper.ConvertFromImageUrl(changeRestaurantImageModel.Image);

            var command = new ChangeRestaurantImageCommand(new RestaurantId(restaurantId),
                changeRestaurantImageModel.Type, image);

            var commandResult =
                await commandDispatcher.PostAsync<ChangeRestaurantImageCommand, bool>(command,
                    new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changeaddress")]
        [HttpPost]
        public async Task<IActionResult> PostChangeAddressAsync(Guid restaurantId,
            [FromBody] ChangeRestaurantAddressModel changeRestaurantAddressModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var command = new ChangeRestaurantAddressCommand(
                new RestaurantId(restaurantId),
                changeRestaurantAddressModel.Street,
                changeRestaurantAddressModel.ZipCode,
                changeRestaurantAddressModel.City
            );

            var commandResult =
                await commandDispatcher.PostAsync<ChangeRestaurantAddressCommand, bool>(command,
                    new UserId(currentUserId));
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changecontactinfo")]
        [HttpPost]
        public async Task<IActionResult> PostChangeContactInfoAsync(Guid restaurantId,
            [FromBody] ChangeRestaurantContactInfoModel changeRestaurantContactInfoModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var command = new ChangeRestaurantContactInfoCommand(
                new RestaurantId(restaurantId),
                changeRestaurantContactInfoModel.Phone,
                changeRestaurantContactInfoModel.Fax,
                changeRestaurantContactInfoModel.WebSite,
                changeRestaurantContactInfoModel.ResponsiblePerson,
                changeRestaurantContactInfoModel.EmailAddress
            );

            var commandResult =
                await commandDispatcher.PostAsync<ChangeRestaurantContactInfoCommand, bool>(command,
                    new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changeserviceinfo")]
        [HttpPost]
        public async Task<IActionResult> PostChangePickupInfoAsync(Guid restaurantId,
            [FromBody] ChangeRestaurantServiceInfoModel changeRestaurantServiceInfoModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<ChangeRestaurantServiceInfoCommand, bool>(
                new ChangeRestaurantServiceInfoCommand(new RestaurantId(restaurantId),
                    new PickupInfo(
                        changeRestaurantServiceInfoModel.PickupEnabled,
                        changeRestaurantServiceInfoModel.PickupAverageTime,
                        changeRestaurantServiceInfoModel.PickupMinimumOrderValue,
                        changeRestaurantServiceInfoModel.PickupMaximumOrderValue
                    ),
                    new DeliveryInfo(
                        changeRestaurantServiceInfoModel.DeliveryEnabled,
                        changeRestaurantServiceInfoModel.DeliveryAverageTime,
                        changeRestaurantServiceInfoModel.DeliveryMinimumOrderValue,
                        changeRestaurantServiceInfoModel.DeliveryMaximumOrderValue,
                        changeRestaurantServiceInfoModel.DeliveryCosts
                    ),
                    new ReservationInfo(
                        changeRestaurantServiceInfoModel.ReservationEnabled
                    ),
                    changeRestaurantServiceInfoModel.HygienicHandling
                ),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/addopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostAddOpeningPeriodAsync(Guid restaurantId,
            [FromBody] AddOpeningPeriodToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var start = TimeSpan.FromMinutes(model.Start);
            var end = TimeSpan.FromMinutes(model.End);

            var commandResult = await commandDispatcher.PostAsync<AddOpeningPeriodToRestaurantCommand, bool>(
                new AddOpeningPeriodToRestaurantCommand(new RestaurantId(restaurantId), model.DayOfWeek, start, end),
                new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changeopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostChangeOpeningPeriodAsync(Guid restaurantId,
            [FromBody] ChangeOpeningPeriodOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var oldStart = TimeSpan.FromMinutes(model.OldStart);
            var newStart = TimeSpan.FromMinutes(model.NewStart);
            var newEnd = TimeSpan.FromMinutes(model.NewEnd);

            var commandResult = await commandDispatcher.PostAsync<ChangeOpeningPeriodOfRestaurantCommand, bool>(
                new ChangeOpeningPeriodOfRestaurantCommand(new RestaurantId(restaurantId), model.DayOfWeek, oldStart, newStart, newEnd),
                new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removeopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveOpeningPeriodAsync(Guid restaurantId,
            [FromBody] RemoveOpeningPeriodFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var start = TimeSpan.FromMinutes(model.Start);

            var commandResult = await commandDispatcher.PostAsync<RemoveOpeningPeriodFromRestaurantCommand, bool>(
                new RemoveOpeningPeriodFromRestaurantCommand(new RestaurantId(restaurantId), model.DayOfWeek, start),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/addpaymentmethod")]
        [HttpPost]
        public async Task<IActionResult> PostAddPaymentMethodAsync(Guid restaurantId,
            [FromBody] AddPaymentMethodToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<AddPaymentMethodToRestaurantCommand, bool>(
                new AddPaymentMethodToRestaurantCommand(new RestaurantId(restaurantId),
                    new PaymentMethodId(model.PaymentMethodId)),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removepaymentmethod")]
        [HttpPost]
        public async Task<IActionResult> PostRemovePaymentMethodAsync(Guid restaurantId,
            [FromBody] RemovePaymentMethodFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<RemovePaymentMethodFromRestaurantCommand, bool>(
                new RemovePaymentMethodFromRestaurantCommand(new RestaurantId(restaurantId),
                    new PaymentMethodId(model.PaymentMethodId)),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/adddishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostAddDishCategoryAsync(Guid restaurantId,
            [FromBody] AddDishCategoryToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<AddDishCategoryToRestaurantCommand, Guid>(
                new AddDishCategoryToRestaurantCommand(new RestaurantId(restaurantId), model.Name,
                    model.AfterCategoryId.HasValue ? new DishCategoryId(model.AfterCategoryId.Value) : null),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changedishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDishCategoryAsync(Guid restaurantId,
            [FromBody] ChangeDishCategoryOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<ChangeDishCategoryOfRestaurantCommand, bool>(
                new ChangeDishCategoryOfRestaurantCommand(new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId), model.Name),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/incorderofdishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostIncOrderOfDishCategoryAsync(Guid restaurantId,
            [FromBody] IncOrderOfDishCategoryModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<IncOrderOfDishCategoryCommand, bool>(
                new IncOrderOfDishCategoryCommand(new DishCategoryId(model.DishCategoryId)),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/decorderofdishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostDecOrderOfDishCategoryAsync(Guid restaurantId,
            [FromBody] DecOrderOfDishCategoryModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<DecOrderOfDishCategoryCommand, bool>(
                new DecOrderOfDishCategoryCommand(new DishCategoryId(model.DishCategoryId)),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removedishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveDishCategoryAsync(Guid restaurantId,
            [FromBody] RemoveDishCategoryFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<RemoveDishCategoryFromRestaurantCommand, bool>(
                new RemoveDishCategoryFromRestaurantCommand(new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId)),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/addoreditdish")]
        [HttpPost]
        public async Task<IActionResult> PostAddOrEditDishAsync(Guid restaurantId,
            [FromBody] AddOrChangeDishOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<AddOrChangeDishOfRestaurantCommand, Guid>(
                new AddOrChangeDishOfRestaurantCommand(
                    new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId),
                    model.Dish.Id,
                    model.Dish.Name,
                    model.Dish.Description,
                    model.Dish.ProductInfo,
                    model.Dish.OrderNo,
                    model.Dish.Variants?.Select(
                        variant => new DishVariant(
                            variant.VariantId,
                            variant.Name,
                            variant.Price
                        )
                    )
                ),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/incorderofdish")]
        [HttpPost]
        public async Task<IActionResult> PostIncOrderOfDishAsync(Guid restaurantId,
            [FromBody] IncOrderOfDishModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<IncOrderOfDishCommand, bool>(
                new IncOrderOfDishCommand(new DishId(model.DishId)),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/decorderofdish")]
        [HttpPost]
        public async Task<IActionResult> PostDecOrderOfDishAsync(Guid restaurantId,
            [FromBody] DecOrderOfDishModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<DecOrderOfDishCommand, bool>(
                new DecOrderOfDishCommand(new DishId(model.DishId)),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removedish")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveDishAsync(Guid restaurantId,
            [FromBody] RemoveDishFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<RemoveDishFromRestaurantCommand, bool>(
                new RemoveDishFromRestaurantCommand(new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId), new DishId(model.DishId)),
                new UserId(currentUserId)
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
        
        [Route("restaurants/{restaurantId}/changesupportedordermode")]
        [HttpPost]
        public async Task<IActionResult> PostChangeSupportedOrderModeAsync(Guid restaurantId,
            [FromBody] ChangeSupportedOrderModeOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var supportedOrderMode = model.SupportedOrderMode.ToSupportedOrderMode();

            var commandResult = await commandDispatcher.PostAsync<ChangeSupportedOrderModeOfRestaurantCommand, bool>(
                new ChangeSupportedOrderModeOfRestaurantCommand(new RestaurantId(restaurantId), supportedOrderMode),
                new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
        
        [Route("restaurants/{restaurantId}/addorchangeexternalmenu")]
        [HttpPost]
        public async Task<IActionResult> PostAddOrChangeExternalMenuAsync(Guid restaurantId,
            [FromBody] AddOrChangeExternalMenuOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var externalMenu = new ExternalMenu(
                model.ExternalMenuId,
                model.Name,
                model.Description,
                model.Url
            );

            var commandResult = await commandDispatcher.PostAsync<AddOrChangeExternalMenuOfRestaurantCommand, bool>(
                new AddOrChangeExternalMenuOfRestaurantCommand(new RestaurantId(restaurantId), externalMenu),
                new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
        
        [Route("restaurants/{restaurantId}/removeexternalmenu")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveExternalMenuAsync(Guid restaurantId,
            [FromBody] RemoveExternalMenuFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var commandResult = await commandDispatcher.PostAsync<RemoveExternalMenuFromRestaurantCommand, bool>(
                new RemoveExternalMenuFromRestaurantCommand(new RestaurantId(restaurantId), model.ExternalMenuId),
                new UserId(currentUserId));

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
    }
}