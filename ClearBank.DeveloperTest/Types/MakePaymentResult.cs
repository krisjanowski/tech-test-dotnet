namespace ClearBank.DeveloperTest.Types
{
    public class MakePaymentResult
    {
        public MakePaymentResult(bool success)
        {
            Success = success;
        }

        public bool Success { get; set; }
    }
}
