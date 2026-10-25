namespace DonationHub.Domain.Shared
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public Money(decimal amount, string currency)
        {
            // check amoount throws exception if negative
            // check curreny if null or empty 
            // c
            Amount = amount;
            Currency = currency.ToUpper();
        }


        public static Money Zero(string currency)
        {
            return new Money(0, currency);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            // TODO: study yield 
            yield return Amount;

            yield return Currency;
        }
    }
}
