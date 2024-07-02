using Microsoft.AspNetCore.Mvc;
using app.Models;
using app.Services;

namespace app.Controllers
{
    [Route("api/stripe")]
    [ApiController]
    public class StripeController(IPaymentService paymentService) : ControllerBase
    {

        [HttpPost("payment-process")]
        public IActionResult ProcessPayment(CardPaymentModel cardPaymentModel)
        {
            var methodId = paymentService.CreatePaymentMethod(cardPaymentModel);
            var paymentIntentId = paymentService.CreatePaymentIntent(cardPaymentModel.Amount, cardPaymentModel.Currenty, methodId);

            // TODO: send server address
            var confirmationStatus = paymentService.ConfirmPayment(paymentIntentId, methodId, "https://b09a-38-52-222-227.ngrok-free.app");
            return Ok($"Payment intent status {confirmationStatus}");
        }
    }
}
