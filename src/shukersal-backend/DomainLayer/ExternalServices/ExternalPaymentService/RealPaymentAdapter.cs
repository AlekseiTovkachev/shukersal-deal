namespace shukersal_backend.DomainLayer.ExternalServices.ExternalPaymentService
{
    public class RealPaymentAdapter : IPayment
    {
        private PaymentAdaptee adaptee;
        public RealPaymentAdapter(PaymentAdaptee adaptee) {
            this.adaptee = adaptee;
        }
        public bool ConfirmPayment(double totalPrice, string HolderFirstName, string HolderLastName, string HolderID, string CardNumber, DateOnly expirationDate, string CVC)
        {
            return adaptee.Pay();
        }
    }
}
