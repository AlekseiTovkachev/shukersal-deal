namespace shukersal_backend.DomainLayer.ExternalServices
{
    public interface IPayment
    {
        public bool ConfirmPayment(double totalPrice, string HolderFirstName,
            string HolderLastName, string HolderID, string CardNumber,
            DateOnly expirationDate, string CVC);
    }
}
