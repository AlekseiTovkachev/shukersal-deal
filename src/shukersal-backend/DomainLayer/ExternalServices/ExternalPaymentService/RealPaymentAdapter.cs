using shukersal_backend.Models.PurchaseModels;

namespace shukersal_backend.DomainLayer.ExternalServices.ExternalPaymentService
{
    public class RealPaymentAdapter : IPayment
    {
        private PaymentAdaptee adaptee;
        public RealPaymentAdapter(PaymentAdaptee adaptee) {
            this.adaptee = adaptee;
        }
        public bool ConfirmPayment(PaymentDetails paymentDetails)
        {
            return adaptee.Pay();
        }
    }
}
