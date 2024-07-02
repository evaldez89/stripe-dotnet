namespace app.Models
{
    public class CardPaymentModel()
    {
        public required string Number {get; set;}
        public required int ExpMonth {get; set;}
        public required int ExpYear {get; set;}
        public required string Cvc {get; set;}
        public required int Amount {get; set;}
        public string Currenty {get; set;} = "usd";
    }
}

