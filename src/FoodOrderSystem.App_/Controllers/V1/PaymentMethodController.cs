using FoodOrderSystem.Domain.Model.PaymentMethod;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace FoodOrderSystem.App.Controllers
{
    [Route("api/v1/paymentmethods")]
    [ApiController]
    public class PaymentMethodController : ControllerBase
    {
        private readonly IPaymentMethodRepository paymentMethodRepository;

        public PaymentMethodController(IPaymentMethodRepository paymentMethodRepository)
        {
            this.paymentMethodRepository = paymentMethodRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRootAsync()
        {
            var paymentMethods = await paymentMethodRepository.FindAllAsync(CancellationToken.None);
            return Ok();
        }
    }
}