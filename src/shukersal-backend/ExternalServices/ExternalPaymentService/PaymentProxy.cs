using Microsoft.IdentityModel.Tokens;
using shukersal_backend.ExternalServices.ExternalDeliveryService;
using shukersal_backend.Models.PurchaseModels;
using System.Security.Policy;

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
        public void SetPaymentProvider(string url)
        {
            if (url.IsNullOrEmpty())
            {
                RealPaymentAdapter = null;
            }
            else RealPaymentAdapter = new RealPaymentAdapter(new PaymentAdaptee(url));
        }


        public void SetProxyAnswer(bool NewAns) { ProxyAns = NewAns; }
    }
}
