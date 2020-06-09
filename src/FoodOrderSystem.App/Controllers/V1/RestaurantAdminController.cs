using FoodOrderSystem.App.Helper;
using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.AddAdminToRestaurant;
using FoodOrderSystem.Domain.Commands.AddCuisineToRestaurant;
using FoodOrderSystem.Domain.Commands.AddDishCategoryToRestaurant;
using FoodOrderSystem.Domain.Commands.AddOrChangeDishOfRestaurant;
using FoodOrderSystem.Domain.Commands.AddPaymentMethodToRestaurant;
using FoodOrderSystem.Domain.Commands.ChangeDishCategoryOfRestaurant;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantAddress;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantImage;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantName;
using FoodOrderSystem.Domain.Commands.RemoveAdminFromRestaurant;
using FoodOrderSystem.Domain.Commands.RemoveCuisineFromRestaurant;
using FoodOrderSystem.Domain.Commands.RemoveDishCategoryFromRestaurant;
using FoodOrderSystem.Domain.Commands.RemoveDishFromRestaurant;
using FoodOrderSystem.Domain.Commands.RemovePaymentMethodFromRestaurant;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.Dish;
using FoodOrderSystem.Domain.Model.DishCategory;
using FoodOrderSystem.Domain.Model.PaymentMethod;
using FoodOrderSystem.Domain.Model.Restaurant;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.GetAllCuisines;
using FoodOrderSystem.Domain.Queries.GetAllPaymentMethods;
using FoodOrderSystem.Domain.Queries.GetDishesOfRestaurant;
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
using System.Threading.Tasks;
using FoodOrderSystem.Domain.Commands.AddOpeningPeriodToRestaurant;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantContactInfo;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantDeliveryInfo;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantPickupInfo;
using FoodOrderSystem.Domain.Commands.ChangeRestaurantReservationInfo;
using FoodOrderSystem.Domain.Commands.DecOrderOfDish;
using FoodOrderSystem.Domain.Commands.DecOrderOfDishCategory;
using FoodOrderSystem.Domain.Commands.IncOrderOfDish;
using FoodOrderSystem.Domain.Commands.IncOrderOfDishCategory;
using FoodOrderSystem.Domain.Commands.RemoveOpeningPeriodFromRestaurant;

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

            var queryResult = await queryDispatcher.PostAsync<GetDishesOfRestaurantQuery, ICollection<DishCategoryViewModel>>(
                new GetDishesOfRestaurantQuery(new RestaurantId(restaurantId)),
                currentUser
            );
            return ResultHelper.HandleResult(queryResult, failureMessageService);
        }

        [Route("cuisines")]
        [HttpGet]
        public async Task<IActionResult> GetCuisinesAsync()
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var queryResult = await queryDispatcher.PostAsync<GetAllCuisinesQuery, ICollection<CuisineViewModel>>(new GetAllCuisinesQuery(), currentUser);
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
            
            var query = new SearchForUsersQuery(search, 0, 20);

            var queryResult =
                await queryDispatcher
                    .PostAsync<SearchForUsersQuery, PagingViewModel<UserViewModel>>(query, currentUser);
            
            if (queryResult.IsFailure)
                return ResultHelper.HandleResult(queryResult, failureMessageService);

            return Ok(queryResult.Value.Items);
        }

        [Route("restaurants/{restaurantId}/changename")]
        [HttpPost]
        public async Task<IActionResult> PostChangeNameAsync(Guid restaurantId, [FromBody] ChangeRestaurantNameModel changeRestaurantNameModel)
        {
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

        [Route("restaurants/{restaurantId}/changecontactinfo")]
        [HttpPost]
        public async Task<IActionResult> PostChangeContactInfoAsync(Guid restaurantId, [FromBody] ChangeRestaurantContactInfoModel changeRestaurantContactInfoModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<ChangeRestaurantContactInfoCommand, bool>(
                new ChangeRestaurantContactInfoCommand(
                    new RestaurantId(restaurantId),
                    new ContactInfo(
                        changeRestaurantContactInfoModel.Phone,
                        changeRestaurantContactInfoModel.Fax,
                        changeRestaurantContactInfoModel.WebSite,
                        changeRestaurantContactInfoModel.ResponsiblePerson,
                        changeRestaurantContactInfoModel.EmailAddress
                    )
                ),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changepickupinfo")]
        [HttpPost]
        public async Task<IActionResult> PostChangePickupInfoAsync(Guid restaurantId, [FromBody] ChangeRestaurantPickupInfoModel changeRestaurantPickupInfoModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<ChangeRestaurantPickupInfoCommand, bool>(
                new ChangeRestaurantPickupInfoCommand(new RestaurantId(restaurantId),
                    new PickupInfo(
                        TimeSpan.FromMinutes(changeRestaurantPickupInfoModel.AverageTime),
                        changeRestaurantPickupInfoModel.MinimumOrderValue,
                        changeRestaurantPickupInfoModel.MaximumOrderValue,
                        changeRestaurantPickupInfoModel.HygienicHandling
                    )
                ),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changedeliveryinfo")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDeliveryInfoAsync(Guid restaurantId, [FromBody] ChangeRestaurantDeliveryInfoModel changeRestaurantDeliveryInfoModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<ChangeRestaurantDeliveryInfoCommand, bool>(
                new ChangeRestaurantDeliveryInfoCommand(new RestaurantId(restaurantId),
                    new DeliveryInfo(
                        TimeSpan.FromMinutes(changeRestaurantDeliveryInfoModel.AverageTime),
                        changeRestaurantDeliveryInfoModel.MinimumOrderValue,
                        changeRestaurantDeliveryInfoModel.MaximumOrderValue,
                        changeRestaurantDeliveryInfoModel.Costs,
                        changeRestaurantDeliveryInfoModel.HygienicHandling
                    )
                ),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changereservationinfo")]
        [HttpPost]
        public async Task<IActionResult> PostChangeReservationInfoAsync(Guid restaurantId, [FromBody] ChangeRestaurantReservationInfoModel changeRestaurantReservationInfoModel)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<ChangeRestaurantReservationInfoCommand, bool>(
                new ChangeRestaurantReservationInfoCommand(new RestaurantId(restaurantId),
                    new ReservationInfo(
                        changeRestaurantReservationInfoModel.HygienicHandling
                    )
                ),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/addopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostAddOpeningPeriodAsync(Guid restaurantId, [FromBody] AddOpeningPeriodToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var start = TimeSpan.FromMinutes(model.Start);
            var end = TimeSpan.FromMinutes(model.End);

            var commandResult = await commandDispatcher.PostAsync<AddOpeningPeriodToRestaurantCommand, bool>(
                new AddOpeningPeriodToRestaurantCommand(new RestaurantId(restaurantId),
                    new OpeningPeriod(model.DayOfWeek, start, end)), currentUser);

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removeopeningperiod")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveOpeningPeriodAsync(Guid restaurantId, [FromBody] RemoveOpeningPeriodFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var start = TimeSpan.FromMinutes(model.Start);

            var commandResult = await commandDispatcher.PostAsync<RemoveOpeningPeriodFromRestaurantCommand, bool>(
                new RemoveOpeningPeriodFromRestaurantCommand(new RestaurantId(restaurantId), model.DayOfWeek, start),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/addcuisine")]
        [HttpPost]
        public async Task<IActionResult> PostAddCuisineAsync(Guid restaurantId, [FromBody] AddCuisineToRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<AddCuisineToRestaurantCommand, bool>(
                new AddCuisineToRestaurantCommand(new RestaurantId(restaurantId), new CuisineId(model.CuisineId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removecuisine")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveCuisineAsync(Guid restaurantId, [FromBody] RemoveCuisineFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<RemoveCuisineFromRestaurantCommand, bool>(
                new RemoveCuisineFromRestaurantCommand(new RestaurantId(restaurantId), new CuisineId(model.CuisineId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/addpaymentmethod")]
        [HttpPost]
        public async Task<IActionResult> PostAddPaymentMethodAsync(Guid restaurantId, [FromBody] AddPaymentMethodToRestaurantModel model)
        {
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
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<AddDishCategoryToRestaurantCommand, Guid>(
                new AddDishCategoryToRestaurantCommand(new RestaurantId(restaurantId), model.Name, new DishCategoryId(model.AfterCategoryId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/changedishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostChangeDishCategoryAsync(Guid restaurantId, [FromBody] ChangeDishCategoryOfRestaurantModel model)
        {
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

        [Route("restaurants/{restaurantId}/incorderofdishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostIncOrderOfDishCategoryAsync(Guid restaurantId, [FromBody] IncOrderOfDishCategoryModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<IncOrderOfDishCategoryCommand, bool>(
                new IncOrderOfDishCategoryCommand(new DishCategoryId(model.DishCategoryId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/decorderofdishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostDecOrderOfDishCategoryAsync(Guid restaurantId, [FromBody] DecOrderOfDishCategoryModel model)
        {

            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<DecOrderOfDishCategoryCommand, bool>(
                new DecOrderOfDishCategoryCommand(new DishCategoryId(model.DishCategoryId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/removedishcategory")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveDishCategoryAsync(Guid restaurantId, [FromBody] RemoveDishCategoryFromRestaurantModel model)
        {
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

        [Route("restaurants/{restaurantId}/incorderofdish")]
        [HttpPost]
        public async Task<IActionResult> PostIncOrderOfDishAsync(Guid restaurantId, [FromBody] IncOrderOfDishModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<IncOrderOfDishCommand, bool>(
                new IncOrderOfDishCommand(new DishId(model.DishId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

        [Route("restaurants/{restaurantId}/decorderofdish")]
        [HttpPost]
        public async Task<IActionResult> PostDecOrderOfDishAsync(Guid restaurantId, [FromBody] DecOrderOfDishModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<DecOrderOfDishCommand, bool>(
                new DecOrderOfDishCommand(new DishId(model.DishId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }
        
        [Route("restaurants/{restaurantId}/removedish")]
        [HttpPost]
        public async Task<IActionResult> PostRemoveDishAsync(Guid restaurantId, [FromBody] RemoveDishFromRestaurantModel model)
        {
            var identityName = (User.Identity as ClaimsIdentity).Claims.FirstOrDefault(en => en.Type == ClaimTypes.NameIdentifier)?.Value;
            if (identityName == null || !Guid.TryParse(identityName, out var currentUserId))
                return Unauthorized();
            var currentUser = await userRepository.FindByUserIdAsync(new UserId(currentUserId));

            var commandResult = await commandDispatcher.PostAsync<RemoveDishFromRestaurantCommand, bool>(
                new RemoveDishFromRestaurantCommand(new RestaurantId(restaurantId), new DishCategoryId(model.DishCategoryId), new DishId(model.DishId)),
                currentUser
            );

            return ResultHelper.HandleResult(commandResult, failureMessageService);
        }

    }
}