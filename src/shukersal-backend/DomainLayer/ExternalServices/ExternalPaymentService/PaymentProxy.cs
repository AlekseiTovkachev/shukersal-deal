using shukersal_backend.Models.PurchaseModels;

namespace shukersal_backend.DomainLayer.ExternalServices.ExternalPaymentService
{
    public class PaymentProxy : IPayment
    {
        private RealPaymentAdapter? RealPaymentAdapter =null;
        private bool ProxyAns;

        public PaymentProxy() { ProxyAns = true; }
        public bool ConfirmPayment(PaymentDetails paymentDetails)
        {
            if (RealPaymentAdapter == null) { return ProxyAns; }
            return RealPaymentAdapter.ConfirmPayment(paymentDetails);
        }

        public void SetPaymentProvider(RealPaymentAdapter Adapter)
        {
            this.RealPaymentAdapter = Adapter;
        }

        public void SetProxyAnswer(bool NewAns) { ProxyAns = NewAns; }
    }
}
