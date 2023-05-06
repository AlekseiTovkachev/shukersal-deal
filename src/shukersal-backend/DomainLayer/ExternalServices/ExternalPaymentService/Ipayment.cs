using shukersal_backend.Models.PurchaseModels;

namespace shukersal_backend.DomainLayer.ExternalServices
{
    public interface IPayment
    {
        public bool ConfirmPayment(PaymentDetails paymentDetails);
    }
}
