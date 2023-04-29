namespace shukersal_backend.DomainLayer.ExternalServices.ExternalPaymentService
{
    public class PaymentProxy : IPayment
    {
        private RealPaymentAdapter realPaymentAdapter;

        public PaymentProxy(RealPaymentAdapter realPaymentAdapter) {
            this.realPaymentAdapter = realPaymentAdapter;
        }
        public bool ConfirmPayment(double totalPrice, string HolderFirstName, string HolderLastName, string HolderID, string CardNumber, DateOnly expirationDate, string CVC)
        {
            throw new NotImplementedException();
        }
    }
}
