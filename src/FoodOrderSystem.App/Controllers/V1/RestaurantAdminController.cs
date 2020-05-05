using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.AddAdminToRestaurant;
using FoodOrderSystem.Domain.Commands.AddDeliveryTimeToRestaurant;
using FoodOrderSystem.Domain.Commands.AddDishCategoryToRestaurant;
using FoodOrderSystem.Domain.Commands.AddOrChangeDishOfRestaurant;
using FoodOrderSystem.Domain.Commands.AddPaymentMethodToRestaurant;
using FoodOrderSystem.Domain.Commands.ChangeDishCategoryOfRestaurant;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantAddress;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantContactDetails;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantDeliveryData;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantImage;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantName;
using FoodOrderSystem.Domain.Commands.RemoveAdminFromRestaurant;
using FoodOrderSystem.Domain.Commands.RemoveDeliveryTimeFromRestaurant;
using FoodOrderSystem.Domain.Commands.RemoveDishCategoryFromRestaurant;
using FoodOrderSystem.Domain.Commands.RemovePaymentMethodFromRestaurant;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.GetAllPaymentMethods;
using FoodOrderSystem.Domain.Queries.GetDishesOfRestaurantForEdit;
using FoodOrderSystem.Domain.Queries.GetRestaurantById;
using FoodOrderSystem.Domain.Queries.RestAdminMyRestaurants;
using FoodOrderSystem.Domain.Queries.SearchForUsers;
using FoodOrderSystem.Domain.Services;
using FoodOrderSystem.Domain.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1/restaurantadmin")]
    [ApiController]
    [Authorize()]
    public class RestaurantAdminController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserRepository userRepository;
        private readonly ICommandDispatcher commandDispatcher;
        private readonly IQueryDispatcher queryDispatcher;
        private readonly IFailureMessageService failureMessageService;

        public RestaurantAdminController(ILogger<RestaurantAdminController> logger, IUserRepository userRepository, ICommandDispatcher commandDispatcher,
            IQueryDispatcher queryDispatcher, IFailureMessageService failureMessageService)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.commandDispatcher = commandDispatcher;
            this.queryDispatcher = queryDispatcher;
            this.failureMessageService = failureMessageService;
        }

        [Route("myrestaurants")]
        [HttpGet]
        public async Task<IActionResult> GetMyRestaurantsAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync<RestAdminMyRestaurantsQuery, ICollection<RestaurantViewModel>>(new RestAdminMyRestaurantsQuery(), currentUser);
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}")]
        [HttpGet]
        public async Task<IActionResult> GetRestaurantAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync<GetRestaurantByIdQuery, RestaurantViewModel>(new GetRestaurantByIdQuery(new RestaurantId(restaurantId)), currentUser);
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/dishes")]
        [HttpGet]
        public async Task<IActionResult> GetDishesOfRestaurantAsync(Guid restaurantId)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync<GetDishesOfRestaurantForEditQuery, ICollection<DishCategoryViewModel>>(
                new GetDishesOfRestaurantForEditQuery(new RestaurantId(restaurantId)),
                currentUser
            );
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("paymentmethods")]
        [HttpGet]
        public async Task<IActionResult> GetPaymentMethodsAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync<GetAllPaymentMethodsQuery, ICollection<PaymentMethodViewModel>>(new GetAllPaymentMethodsQuery(), currentUser);
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("users")]
        [HttpGet]
        public async Task<IActionResult> SearchForUsersAsync(string search)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync<SearchForUsersQuery, ICollection<UserViewModel>>(new SearchForUsersQuery(search), currentUser);
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changename")]
        [HttpPost]
        public async Task<IActionResult> PostChangeNameAsync(Guid restaurantId, [FromBody] ChangeRestaurantNameModel changeRestaurantNameModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<ChangeRestaurantNameCommand, bool>(new ChangeRestaurantNameCommand(new RestaurantId(restaurantId), changeRestaurantNameModel.Name), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changeimage")]
        [HttpPost]
        public async Task<IActionResult> PostChangeImageAsync(Guid restaurantId, [FromBody] ChangeRestaurantImageModel changeRestaurantImageModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var image = ImageHelper.ConvertFromImageUrl(changeRestaurantImageModel.Image);
            
            var commandResult = await commandDispatcher.PostAsync<ChangeRestaurantImageCommand, bool>(new ChangeRestaurantImageCommand(new RestaurantId(restaurantId), image), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changeaddress")]
        [HttpPost]
        public async Task<IActionResult> PostChangeAddressAsync(Guid restaurantId, [FromBody] ChangeRestaurantAddressModel changeRestaurantAddressModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var address = new Address(
                changeRestaurantAddressModel.Street,
                changeRestaurantAddressModel.ZipCode,
                changeRestaurantAddressModel.City
            );

            var commandResult = await commandDispatcher.PostAsync<ChangeRestaurantAddressCommand, bool>(new ChangeRestaurantAddressCommand(new RestaurantId(restaurantId), address), currentUser);
            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changecontactdetails")]
        [HttpPost]
        public async Task<IActionResult> PostChangeContactDetailsAsync(Guid restaurantId, [FromBody] ChangeRestaurantContactDetailsModel changeRestaurantContactDetailsModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<ChangeRestaurantContactDetailsCommand, bool>(
                new ChangeRestaurantContactDetailsCommand(
                    new RestaurantId(restaurantId),
                    changeRestaurantContactDetailsModel.Phone,
                    changeRestaurantContactDetailsModel.Website,
                    changeRestaurantContactDetailsModel.Imprint,
                    changeRestaurantContactDetailsModel.OrderEmailAddress
                ),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changedeliverydata")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDeliveryDataAsync(Guid restaurantId, [FromBody] ChangeRestaurantDeliveryDataModel changeRestaurantDeliveryDataModel)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<ChangeRestaurantDeliveryDataCommand, bool>(
                new ChangeRestaurantDeliveryDataCommand(new RestaurantId(restaurantId), changeRestaurantDeliveryDataModel.MinimumOrderValue, changeRestaurantDeliveryDataModel.DeliveryCosts),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/adddeliverytime")]
        [HttpPost]
        public async Task<IActionResult> PostAddDeliveryTimeAsync(Guid restaurantId, [FromBody] AddDeliveryTimeToRestaurantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var start = TimeSpan.FromMinutes(model.Start);
            var end = TimeSpan.FromMinutes(model.End);

            var commandResult = await commandDispatcher.PostAsync<AddDeliveryTimeToRestaurantCommand, bool>(
                new AddDeliveryTimeToRestaurantCommand(new RestaurantId(restaurantId), model.DayOfWeek, start, end),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removedeliverytime")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveDeliveryTimeAsync(Guid restaurantId, [FromBody] RemoveDeliveryTimeFromRestaurantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var start = TimeSpan.FromMinutes(model.Start);

            var commandResult = await commandDispatcher.PostAsync<RemoveDeliveryTimeFromRestaurantCommand, bool>(
                new RemoveDeliveryTimeFromRestaurantCommand(new RestaurantId(restaurantId), model.DayOfWeek, start),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/addpaymentmethod")]
        [HttpPost]
        public async Task<IActionResult> PostAddPaymentMethodAsync(Guid restaurantId, [FromBody] AddPaymentMethodToRestaurantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<AddPaymentMethodToRestaurantCommand, bool>(
                new AddPaymentMethodToRestaurantCommand(new RestaurantId(restaurantId), new PaymentMethodId(model.PaymentMethodId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removepaymentmethod")]
        [HttpPost]
        public async Task<IActionResult> PostRemovePaymentMethodAsync(Guid restaurantId, [FromBody] RemovePaymentMethodFromRestaurantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<RemovePaymentMethodFromRestaurantCommand, bool>(
                new RemovePaymentMethodFromRestaurantCommand(new RestaurantId(restaurantId), new PaymentMethodId(model.PaymentMethodId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/addadmin")]
        [HttpPost]
        public async Task<IActionResult> PostAddAdminAsync(Guid restaurantId, [FromBody] AddAdminToRestaurantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<AddAdminToRestaurantCommand, bool>(
                new AddAdminToRestaurantCommand(new RestaurantId(restaurantId), new UserId(model.UserId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removeadmin")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveAdminAsync(Guid restaurantId, [FromBody] RemoveAdminFromRestaurantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<RemoveAdminFromRestaurantCommand, bool>(
                new RemoveAdminFromRestaurantCommand(new RestaurantId(restaurantId), new UserId(model.UserId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/adddishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostAddDishCategoryAsync(Guid restaurantId, [FromBody] AddDishCategoryToRestaurantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<AddDishCategoryToRestaurantCommand, Guid>(
                new AddDishCategoryToRestaurantCommand(new RestaurantId(restaurantId), model.Name),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changedishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDishCategoryAsync(Guid restaurantId, [FromBody] ChangeDishCategoryOfRestaurantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<ChangeDishCategoryOfRestaurantCommand, bool>(
                new ChangeDishCategoryOfRestaurantCommand(new RestaurantId(restaurantId), new DishCategoryId(model.DishCategoryId), model.Name),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removedishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveDishCategoryAsync(Guid restaurantId, [FromBody] RemoveDishCategoryFromRestaurantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<RemoveDishCategoryFromRestaurantCommand, bool>(
                new RemoveDishCategoryFromRestaurantCommand(new RestaurantId(restaurantId), new DishCategoryId(model.DishCategoryId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/addoreditdish")]
        [HttpPost]
        public async Task<IActionResult> PostAddOrEditDishAsync(Guid restaurantId, [FromBody] AddOrChangeDishOfRestaurantModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<AddOrChangeDishOfRestaurantCommand, Guid>(
                new AddOrChangeDishOfRestaurantCommand(new RestaurantId(restaurantId), new DishCategoryId(model.DishCategoryId), model.Dish),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

    }
}