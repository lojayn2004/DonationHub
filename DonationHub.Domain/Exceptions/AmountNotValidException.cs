
namespace DonationHub.Domain.Exceptions
{
    public class AmountNotValidException: Exception
    {
        public AmountNotValidException(string message): base(message)
        {

        }
    }
}
