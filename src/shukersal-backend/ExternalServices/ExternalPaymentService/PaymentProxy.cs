using shukersal_backend.Models.PurchaseModels;

namespace shukersal_backend.ExternalServices.ExternalPaymentService
{
    public class PaymentProxy : IPayment
    {
        private RealPaymentAdapter? RealPaymentAdapter = null;
        private bool ProxyAns;

        public PaymentProxy() { ProxyAns = true; }

        public bool CancelPayment(long transactionId)
        {
            if (RealPaymentAdapter == null) { return ProxyAns; }
            return RealPaymentAdapter.CancelPayment(transactionId);
        }

        public bool ConfirmPayment(PaymentDetails paymentDetails)
        {
            if (RealPaymentAdapter == null) { return ProxyAns; }
            return RealPaymentAdapter.ConfirmPayment(paymentDetails);
        }

        public bool Handshake()
        {
            if (RealPaymentAdapter == null) { return ProxyAns; }
            return RealPaymentAdapter.Handshake();
        }
        public void SetPaymentProvider(RealPaymentAdapter Adapter)
        {
            RealPaymentAdapter = Adapter;
        }


        public void SetProxyAnswer(bool NewAns) { ProxyAns = NewAns; }
    }
}
