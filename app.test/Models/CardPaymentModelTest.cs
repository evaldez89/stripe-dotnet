namespace app.test;

public class CardPaymentModelTest
{
    [Fact]
    public void TestCardPaymentModel__DefaultCurrency_Is_USD()
    {
        var paymentDetails = new CardPaymentModel{Amount = 1000, Cvc = "234", ExpMonth=9, ExpYear=2014, Number="41111111111111"};

        Assert.True(paymentDetails.Currenty == "usd");
    }

}