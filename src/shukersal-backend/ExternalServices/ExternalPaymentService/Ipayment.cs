using shukersal_backend.Models.PurchaseModels;

namespace shukersal_backend.ExternalServices.ExternalPaymentService
{
    public interface IPayment
    {
        public bool Handshake();
        public bool ConfirmPayment(PaymentDetails paymentDetails);
        public bool CancelPayment(long transactionId);
    }
}
