using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Gastromio.App.Helper;
using Gastromio.App.Models;
using Gastromio.Core.Application.Commands;
using Gastromio.Core.Application.Commands.AddDeviatingOpeningDayToRestaurant;
using Gastromio.Core.Application.Commands.AddDeviatingOpeningPeriodToRestaurant;
using Gastromio.Core.Application.Commands.AddDishCategoryToRestaurant;
using Gastromio.Core.Application.Commands.AddOrChangeDishOfRestaurant;
using Gastromio.Core.Application.Commands.AddOrChangeExternalMenuOfRestaurant;
using Gastromio.Core.Application.Commands.AddPaymentMethodToRestaurant;
using Gastromio.Core.Application.Commands.AddRegularOpeningPeriodToRestaurant;
using Gastromio.Core.Application.Commands.ChangeDeviatingOpeningDayStatusOfRestaurant;
using Gastromio.Core.Application.Commands.ChangeDeviatingOpeningPeriodOfRestaurant;
using Gastromio.Core.Application.Commands.ChangeDishCategoryOfRestaurant;
using Gastromio.Core.Application.Commands.ChangeRegularOpeningPeriodOfRestaurant;
using Gastromio.Core.Application.Commands.ChangeRestaurantAddress;
using Gastromio.Core.Application.Commands.ChangeRestaurantContactInfo;
using Gastromio.Core.Application.Commands.ChangeRestaurantImage;
using Gastromio.Core.Application.Commands.ChangeRestaurantServiceInfo;
using Gastromio.Core.Application.Commands.ChangeSupportedOrderModeOfRestaurant;
using Gastromio.Core.Application.Commands.DecOrderOfDish;
using Gastromio.Core.Application.Commands.DecOrderOfDishCategory;
using Gastromio.Core.Application.Commands.DisableDishCategory;
using Gastromio.Core.Application.Commands.EnableDishCategory;
using Gastromio.Core.Application.Commands.IncOrderOfDish;
using Gastromio.Core.Application.Commands.IncOrderOfDishCategory;
using Gastromio.Core.Application.Commands.RemoveDeviatingOpeningDayFromRestaurant;
using Gastromio.Core.Application.Commands.RemoveDeviatingOpeningPeriodFromRestaurant;
using Gastromio.Core.Application.Commands.RemoveDishCategoryFromRestaurant;
using Gastromio.Core.Application.Commands.RemoveDishFromRestaurant;
using Gastromio.Core.Application.Commands.RemoveExternalMenuFromRestaurant;
using Gastromio.Core.Application.Commands.RemovePaymentMethodFromRestaurant;
using Gastromio.Core.Application.Commands.RemoveRegularOpeningPeriodFromRestaurant;
using Gastromio.Core.Application.DTOs;
using Gastromio.Core.Application.Queries;
using Gastromio.Core.Application.Queries.GetAllCuisines;
using Gastromio.Core.Application.Queries.GetAllPaymentMethods;
using Gastromio.Core.Application.Queries.GetRestaurantById;
using Gastromio.Core.Application.Queries.RestAdminMyRestaurants;
using Gastromio.Core.Domain.Model.PaymentMethods;
using Gastromio.Core.Domain.Model.Restaurants;
using Gastromio.Core.Domain.Model.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gastromio.App.Controllers.V1
{
    [Route("api/v1/restaurantadmin")]
    [ApiController]
    [Authorize()]
    public class RestaurantAdminController : ControllerBase
    {
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;

        public RestaurantAdminController(
            ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher
        )
        {
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
        }

        [Route("myrestaurants")]
        [HttpGet]
        public async Task<IActionResult> GetMyRestaurantsAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var restaurantDtos =
                await queryDispatcher.PostAsync<RestAdminMyRestaurantsQuery, ICollection<RestaurantDTO>>(
                    new RestAdminMyRestaurantsQuery(),
                    new UserId(currentUserId)
                );

            return Ok(restaurantDtos);
        }

        [Route("restaurants/{restaurant}")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantAsync(string restaurant)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var restaurantDto = await queryDispatcher.PostAsync<GetRestaurantByIdQuery, RestaurantDTO>(
                new GetRestaurantByIdQuery(restaurant, false),
                new UserId(currentUserId)
            );

            return Ok(restaurantDto);
        }

        [Route("cuisines")]
        [HttpGet]
        public async Task<IActionResult> GetCuisinesAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var cuisineDtos = await queryDispatcher.PostAsync<GetAllCuisinesQuery, ICollection<CuisineDTO>>(
                new GetAllCuisinesQuery(),
                new UserId(currentUserId)
            );

            return Ok(cuisineDtos);
        }

        [Route("paymentmethods")]
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethodsAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var paymentMethodDtos =
                await queryDispatcher.PostAsync<GetAllPaymentMethodsQuery, ICollection<PaymentMethodDTO>>(
                    new GetAllPaymentMethodsQuery(), new UserId(currentUserId));
            return Ok(paymentMethodDtos);
        }

        [Route("restaurants/{restaurantId}/changeimage")]
        [HttpPost]
        public async Task<IActionResult> PostChangeImageAsync(Guid restaurantId,
            [FromBody] ChangeRestaurantImageModel changeRestaurantImageModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var image = string.IsNullOrWhiteSpace(changeRestaurantImageModel.Image)
                ? null
                : ImageHelper.ConvertFromImageUrl(changeRestaurantImageModel.Image);

            var command = new ChangeRestaurantImageCommand(new RestaurantId(restaurantId),
                changeRestaurantImageModel.Type, image);

            await commandDispatcher.PostAsync(command, new UserId(currentUserId));
            return Ok();
        }

        [Route("restaurants/{restaurantId}/changeaddress")]
        [HttpPost]
        public async Task<IActionResult> PostChangeAddressAsync(Guid restaurantId,
            [FromBody] ChangeRestaurantAddressModel changeRestaurantAddressModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var command = new ChangeRestaurantAddressCommand(
                new RestaurantId(restaurantId),
                changeRestaurantAddressModel.Street,
                changeRestaurantAddressModel.ZipCode,
                changeRestaurantAddressModel.City
            );

            await commandDispatcher.PostAsync(command, new UserId(currentUserId));
            return Ok();
        }

        [Route("restaurants/{restaurantId}/changecontactinfo")]
        [HttpPost]
        public async Task<IActionResult> PostChangeContactInfoAsync(Guid restaurantId,
            [FromBody] ChangeRestaurantContactInfoModel changeRestaurantContactInfoModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var command = new ChangeRestaurantContactInfoCommand(
                new RestaurantId(restaurantId),
                changeRestaurantContactInfoModel.Phone,
                changeRestaurantContactInfoModel.Fax,
                changeRestaurantContactInfoModel.WebSite,
                changeRestaurantContactInfoModel.ResponsiblePerson,
                changeRestaurantContactInfoModel.EmailAddress,
                changeRestaurantContactInfoModel.Mobile,
                changeRestaurantContactInfoModel.OrderNotificationByMobile
            );

            await commandDispatcher.PostAsync(command, new UserId(currentUserId));
            return Ok();
        }

        [Route("restaurants/{restaurantId}/changeserviceinfo")]
        [HttpPost]
        public async Task<IActionResult> PostChangePickupInfoAsync(Guid restaurantId,
            [FromBody] ChangeRestaurantServiceInfoModel changeRestaurantServiceInfoModel)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
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
                        changeRestaurantServiceInfoModel.ReservationEnabled,
                        changeRestaurantServiceInfoModel.ReservationSystemUrl
                    ),
                    changeRestaurantServiceInfoModel.HygienicHandling
                ),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/addregularopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostAddRegularOpeningPeriodAsync(Guid restaurantId,
            [FromBody] AddRegularOpeningPeriodToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var start = TimeSpan.FromMinutes(model.Start);
            var end = TimeSpan.FromMinutes(model.End);

            await commandDispatcher.PostAsync(
                new AddRegularOpeningPeriodToRestaurantCommand(new RestaurantId(restaurantId), model.DayOfWeek, start,
                    end), new UserId(currentUserId));

            return Ok();
        }

        [Route("restaurants/{restaurantId}/changeregularopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostChangeRegularOpeningPeriodAsync(Guid restaurantId,
            [FromBody] ChangeRegularOpeningPeriodOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var oldStart = TimeSpan.FromMinutes(model.OldStart);
            var newStart = TimeSpan.FromMinutes(model.NewStart);
            var newEnd = TimeSpan.FromMinutes(model.NewEnd);

            await commandDispatcher.PostAsync(
                new ChangeRegularOpeningPeriodOfRestaurantCommand(new RestaurantId(restaurantId), model.DayOfWeek,
                    oldStart, newStart, newEnd), new UserId(currentUserId));

            return Ok();
        }

        [Route("restaurants/{restaurantId}/removeregularopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveRegularOpeningPeriodAsync(Guid restaurantId,
            [FromBody] RemoveRegularOpeningPeriodFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var start = TimeSpan.FromMinutes(model.Start);

            await commandDispatcher.PostAsync(
                new RemoveRegularOpeningPeriodFromRestaurantCommand(new RestaurantId(restaurantId), model.DayOfWeek,
                    start), new UserId(currentUserId));

            return Ok();
        }

        [Route("restaurants/{restaurantId}/adddeviatingopeningday")]
        [HttpPost]
        public async Task<IActionResult> PostAddDeviatingOpeningDayAsync(Guid restaurantId,
            [FromBody] AddDeviatingOpeningDayToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new AddDeviatingOpeningDayToRestaurantCommand(new RestaurantId(restaurantId), model.Date.ToDomain(),
                    model.Status.ToDeviatingOpeningDayStatus()), new UserId(currentUserId));

            return Ok();
        }

        [Route("restaurants/{restaurantId}/changedeviatingopeningdaystatus")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDeviatingOpeningDayStatusAsync(Guid restaurantId,
            [FromBody] ChangeDeviatingOpeningDayStatusOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new ChangeDeviatingOpeningDayStatusOfRestaurantCommand(new RestaurantId(restaurantId),
                    model.Date.ToDomain(), model.Status.ToDeviatingOpeningDayStatus()), new UserId(currentUserId));

            return Ok();
        }

        [Route("restaurants/{restaurantId}/removedeviatingopeningday")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveDeviatingOpeningDayAsync(Guid restaurantId,
            [FromBody] RemoveDeviatingOpeningDayFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new RemoveDeviatingOpeningDayFromRestaurantCommand(new RestaurantId(restaurantId),
                    model.Date.ToDomain()),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/adddeviatingopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostAddDeviatingOpeningPeriodAsync(Guid restaurantId,
            [FromBody] AddDeviatingOpeningPeriodToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var start = TimeSpan.FromMinutes(model.Start);
            var end = TimeSpan.FromMinutes(model.End);

            await commandDispatcher.PostAsync(
                new AddDeviatingOpeningPeriodToRestaurantCommand(new RestaurantId(restaurantId), model.Date.ToDomain(),
                    start, end),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/changedeviatingopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDeviatingOpeningPeriodAsync(Guid restaurantId,
            [FromBody] ChangeDeviatingOpeningPeriodOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var oldStart = TimeSpan.FromMinutes(model.OldStart);
            var newStart = TimeSpan.FromMinutes(model.NewStart);
            var newEnd = TimeSpan.FromMinutes(model.NewEnd);

            await commandDispatcher.PostAsync(
                new ChangeDeviatingOpeningPeriodOfRestaurantCommand(new RestaurantId(restaurantId),
                    model.Date.ToDomain(), oldStart, newStart, newEnd),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/removedeviatingopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveDeviatingOpeningPeriodAsync(Guid restaurantId,
            [FromBody] RemoveDeviatingOpeningPeriodFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var start = TimeSpan.FromMinutes(model.Start);

            await commandDispatcher.PostAsync(
                new RemoveDeviatingOpeningPeriodFromRestaurantCommand(new RestaurantId(restaurantId), model.Date.ToDomain(), start),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/addpaymentmethod")]
        [HttpPost]
        public async Task<IActionResult> PostAddPaymentMethodAsync(Guid restaurantId,
            [FromBody] AddPaymentMethodToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new AddPaymentMethodToRestaurantCommand(new RestaurantId(restaurantId),
                    new PaymentMethodId(model.PaymentMethodId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/removepaymentmethod")]
        [HttpPost]
        public async Task<IActionResult> PostRemovePaymentMethodAsync(Guid restaurantId,
            [FromBody] RemovePaymentMethodFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new RemovePaymentMethodFromRestaurantCommand(new RestaurantId(restaurantId),
                    new PaymentMethodId(model.PaymentMethodId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/adddishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostAddDishCategoryAsync(Guid restaurantId,
            [FromBody] AddDishCategoryToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var dishCategoryId = await commandDispatcher.PostAsync<AddDishCategoryToRestaurantCommand, Guid>(
                new AddDishCategoryToRestaurantCommand(new RestaurantId(restaurantId), model.Name,
                    model.AfterCategoryId.HasValue ? new DishCategoryId(model.AfterCategoryId.Value) : null),
                new UserId(currentUserId)
            );

            return Ok(dishCategoryId);
        }

        [Route("restaurants/{restaurantId}/changedishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDishCategoryAsync(Guid restaurantId,
            [FromBody] ChangeDishCategoryOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new ChangeDishCategoryOfRestaurantCommand(new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId), model.Name),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/incorderofdishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostIncOrderOfDishCategoryAsync(Guid restaurantId,
            [FromBody] IncOrderOfDishCategoryModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new IncOrderOfDishCategoryCommand(new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/decorderofdishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostDecOrderOfDishCategoryAsync(Guid restaurantId,
            [FromBody] DecOrderOfDishCategoryModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new DecOrderOfDishCategoryCommand(
                    new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId)
                ),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/enabledishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostEnableDishCategoryAsync(Guid restaurantId,
            [FromBody] EnableDishCategoryModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new EnableDishCategoryCommand(
                    new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId)
                ),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/disabledishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostDisableDishCategoryAsync(Guid restaurantId,
            [FromBody] DisableDishCategoryModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new DisableDishCategoryCommand(
                    new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId)
                ),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/removedishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveDishCategoryAsync(Guid restaurantId,
            [FromBody] RemoveDishCategoryFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new RemoveDishCategoryFromRestaurantCommand(new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/addoreditdish")]
        [HttpPost]
        public async Task<IActionResult> PostAddOrEditDishAsync(Guid restaurantId,
            [FromBody] AddOrChangeDishOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var dishId = await commandDispatcher.PostAsync<AddOrChangeDishOfRestaurantCommand, Guid>(
                new AddOrChangeDishOfRestaurantCommand(
                    new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId),
                    new DishId(model.Dish.Id),
                    model.Dish.Name,
                    model.Dish.Description,
                    model.Dish.ProductInfo,
                    model.Dish.OrderNo,
                    model.Dish.Variants?.Select(
                        variant => new DishVariant(
                            new DishVariantId(variant.VariantId),
                            variant.Name,
                            variant.Price
                        )
                    )
                ),
                new UserId(currentUserId)
            );

            return Ok(dishId);
        }

        [Route("restaurants/{restaurantId}/incorderofdish")]
        [HttpPost]
        public async Task<IActionResult> PostIncOrderOfDishAsync(Guid restaurantId,
            [FromBody] IncOrderOfDishModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new IncOrderOfDishCommand(
                    new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId),
                    new DishId(model.DishId)
                ),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/decorderofdish")]
        [HttpPost]
        public async Task<IActionResult> PostDecOrderOfDishAsync(Guid restaurantId,
            [FromBody] DecOrderOfDishModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new DecOrderOfDishCommand(
                    new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId),
                    new DishId(model.DishId)
                ),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/removedish")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveDishAsync(Guid restaurantId,
            [FromBody] RemoveDishFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new RemoveDishFromRestaurantCommand(new RestaurantId(restaurantId),
                    new DishCategoryId(model.DishCategoryId), new DishId(model.DishId)),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/changesupportedordermode")]
        [HttpPost]
        public async Task<IActionResult> PostChangeSupportedOrderModeAsync(Guid restaurantId,
            [FromBody] ChangeSupportedOrderModeOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var supportedOrderMode = model.SupportedOrderMode.ToSupportedOrderMode();

            await commandDispatcher.PostAsync(
                new ChangeSupportedOrderModeOfRestaurantCommand(new RestaurantId(restaurantId), supportedOrderMode),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/addorchangeexternalmenu")]
        [HttpPost]
        public async Task<IActionResult> PostAddOrChangeExternalMenuAsync(Guid restaurantId,
            [FromBody] AddOrChangeExternalMenuOfRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            var externalMenu = new ExternalMenu(
                new ExternalMenuId(model.ExternalMenuId),
                model.Name,
                model.Description,
                model.Url
            );

            await commandDispatcher.PostAsync(
                new AddOrChangeExternalMenuOfRestaurantCommand(new RestaurantId(restaurantId), externalMenu),
                new UserId(currentUserId)
            );

            return Ok();
        }

        [Route("restaurants/{restaurantId}/removeexternalmenu")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveExternalMenuAsync(Guid restaurantId,
            [FromBody] RemoveExternalMenuFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity)?.Claims
                .FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();

            await commandDispatcher.PostAsync(
                new RemoveExternalMenuFromRestaurantCommand(
                    new RestaurantId(restaurantId),
                    new ExternalMenuId(model.ExternalMenuId)
                ),
                new UserId(currentUserId)
            );

            return Ok();
        }
    }
}
