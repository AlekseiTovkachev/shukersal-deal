using shukersal_backend.DomainLayer.ExternalServices.ExternalPaymentService;

namespace shukersal_backend.DomainLayer.ExternalServices.ExternalDeliveryService
{
    public class RealDeliveryAdapter: IDelivery
    {
        private DeliveryAdaptee adaptee;
        public RealDeliveryAdapter(DeliveryAdaptee adaptee)
        {
            this.adaptee = adaptee;
        }

        public bool ConfirmDelivery(string ReceiverFirstName, string ReceiverLastName, string ReceiverPhoneNum, string ReceiverAddress, string ReceiverPostalCode)
        {
            return adaptee.Deliver();
        }
    }
}
