namespace DonationHub.Domain.Models.Shared
{
    public class Money : ValueObject
    {
        public decimal Amount { get; private set; }

        public string Currency { get; private set; }

        public Money(decimal amount, string currency)
        {
            
            Amount = amount;
            Currency = currency.ToUpper();
        }

        public static Money operator+(Money first, Money second)
        {
            if (first.Currency != second.Currency)
                throw new Exception("Invalid Currency Addition");
            return new Money(first.Amount + second.Amount, first.Currency);
        }

        public static Money Zero(string currency)
        {
            return new Money(0, currency);
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Amount;

            yield return Currency;
        }
    }
}
