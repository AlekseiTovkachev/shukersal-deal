using shukersal_backend.DomainLayer.ExternalServices.ExternalPaymentService;
using shukersal_backend.Models.PurchaseModels;

namespace shukersal_backend.DomainLayer.ExternalServices.ExternalDeliveryService
{
    public class RealDeliveryAdapter: IDelivery
    {
        private DeliveryAdaptee adaptee;
        public RealDeliveryAdapter(DeliveryAdaptee adaptee)
        {
            this.adaptee = adaptee;
        }

        public bool ConfirmDelivery(DeliveryDetails deliveryDetails)
        {
            return adaptee.Deliver();
        }
    }
}
