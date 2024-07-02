using app.Models;

namespace app.Services
{
    public interface IPaymentService
    {
        string CreatePaymentMethod(CardPaymentModel cardPaymentModel);
        string CreatePaymentIntent(int amount, string currency, string methodId);
        string ConfirmPayment(string intentId, string methodId, string returnUrl);
    }
}