public class StripePaymentServiceTests_MockingDependencies
{
    private readonly Mock<ILogger<StripePaymentService>> _mockLogger;
    private readonly IConfiguration _configuration;
    private readonly StripePaymentService _service;

    public StripePaymentServiceTests_MockingDependencies()
    {
        _mockLogger = new Mock<ILogger<StripePaymentService>>();

        var configurationBuilder = new ConfigurationBuilder();
        configurationBuilder.AddUserSecrets<StripePaymentService>();
        _configuration = configurationBuilder.Build();
        _service = new StripePaymentService(_mockLogger.Object, _configuration);
    }

    [Fact]
    public void CreatePaymentMethod_Should_Log_Success_With_Valid_Card()
    {
        // Prevent test running with production keys
        Assert.StartsWith("pk_test", _configuration["StripeConfig:PublicApiKey"]);
        Assert.StartsWith("sk_test", _configuration["StripeConfig:SecretApiKey"]);

        var cardPaymentModel = new CardPaymentModel {Amount = 1000, Cvc = "234", ExpMonth=9, ExpYear=2029, Number="4111111111111111"};

        var methodId = _service.CreatePaymentMethod(cardPaymentModel);

        Assert.NotNull(methodId);
    }

    [Fact]
    public void CreatePaymentIntent_Should_Throw_If_Amount_Is_Zero()
    {
        // Prevent test running with production keys
        Assert.StartsWith("pk_test", _configuration["StripeConfig:PublicApiKey"]);
        Assert.StartsWith("sk_test", _configuration["StripeConfig:SecretApiKey"]);

        var cardPaymentModel = new CardPaymentModel {Amount = 0, Cvc = "234", ExpMonth=9, ExpYear=2029, Number="4111111111111111"};

        var methodId = _service.CreatePaymentMethod(cardPaymentModel);

        Assert.Throws<Stripe.StripeException>(() => _service.CreatePaymentIntent(cardPaymentModel.Amount, cardPaymentModel.Currenty, methodId));
    }
}