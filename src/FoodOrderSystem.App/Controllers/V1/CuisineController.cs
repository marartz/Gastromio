using FoodOrderSystem.App.Models;
using FoodOrderSystem.Domain.Commands;
using FoodOrderSystem.Domain.Commands.AddCuisine;
using FoodOrderSystem.Domain.Commands.ChangeCuisine;
using FoodOrderSystem.Domain.Commands.RemoveCuisine;
using FoodOrderSystem.Domain.Model.Cuisine;
using FoodOrderSystem.Domain.Model.User;
using FoodOrderSystem.Domain.Queries;
using FoodOrderSystem.Domain.Queries.GetAllCuisines;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FoodOrderSystem.App.Controllers.V1
{
    [Route("api/v1")]
    [ApiController]
    [Authorize]
    public class CuisineController : ControllerBase
    {
        private readonly ILogger logger;
        private readonly IUserRepository userRepository;
        private readonly IQueryDispatcher queryDispatcher;

        public CuisineController(ILogger<CuisineAdminController> logger, IUserRepository userRepository, IQueryDispatcher queryDispatcher)
        {
            this.logger = logger;
            this.userRepository = userRepository;
            this.queryDispatcher = queryDispatcher;
        }

        [Route("cuisines")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetCuisinesAsync()
        {
            var queryResult = await queryDispatcher.PostAsync(new GetAllCuisinesQuery(), null);
            switch (queryResult)
            {
                case UnauthorizedQueryResult _:
                    return Unauthorized();
                case ForbiddenQueryResult _:
                    return Forbid();
                case SuccessQueryResult<ICollection<Cuisine>> result:
                    var model = result.Value.Select(CuisineModel.FromCuisine).ToList();
                    return Ok(model);
                default:
                    throw new InvalidOperationException("internal server error");
            }
        }
    }
}