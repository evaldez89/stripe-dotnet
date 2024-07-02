using app.Models;
using Stripe;

namespace app.Services
{
    public class StripePaymentService : IPaymentService
    {
        private PaymentIntentService paymentIntentService;
        private ILogger<StripePaymentService> logger;
        private IConfiguration configuration;

        public StripePaymentService(ILogger<StripePaymentService> logger, IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
            paymentIntentService = new PaymentIntentService();
        }

        public string CreatePaymentMethod(CardPaymentModel cardPaymentModel)
        {
            StripeConfiguration.ApiKey = configuration["StripeConfig:PublicApiKey"];

            var options = new PaymentMethodCreateOptions
            {
                Type = "card",
                Card = new PaymentMethodCardOptions
                {
                    Number = cardPaymentModel.Number,
                    ExpMonth = cardPaymentModel.ExpMonth,
                    ExpYear = cardPaymentModel.ExpYear,
                    Cvc = cardPaymentModel.Cvc,
                },
            };
            var service = new PaymentMethodService();

            // TODO: Save response to DB
            var paymentMethod = service.Create(options);
            logger.LogInformation($"Payment Method Create{paymentMethod}");

            return paymentMethod.Id;
        }

        public string CreatePaymentIntent(int amount, string currency, string methodId)
        {
            StripeConfiguration.ApiKey = configuration["StripeConfig:SecretApiKey"];
            var optionsPaymentIntent = new PaymentIntentCreateOptions
            {
                Amount = amount,
                Currency = currency,
                PaymentMethod = methodId
            };
            var result = paymentIntentService.Create(optionsPaymentIntent);

            logger.LogInformation($"Payment Intent Create: {result}");

            return result.Id;
        }

        public string ConfirmPayment(string intentId, string methodId, string returnUrl)
        {
            StripeConfiguration.ApiKey = configuration["StripeConfig:SecretApiKey"];
            var paymentIntentConfirmOptions = new PaymentIntentConfirmOptions
            {
                PaymentMethod = methodId,
                ReturnUrl = returnUrl,
            };

            var resultConfirm = paymentIntentService.Confirm(intentId, paymentIntentConfirmOptions);

            logger.LogInformation($"Payment Intent Confirm: {resultConfirm}");

            return resultConfirm.Status;
        }
    }
}

// ,