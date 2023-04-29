namespace shukersal_backend.DomainLayer.ExternalServices.ExternalDeliveryService
{
    public class DeliveryProxy : IDelivery
    {
        private RealDeliveryAdapter realDeliveryAdapter;

        public DeliveryProxy(RealDeliveryAdapter realDeliveryAdapter)
        {
            this.realDeliveryAdapter = realDeliveryAdapter;
        }

        public bool ConfirmDelivery(string ReceiverFirstName, string ReceiverLastName, string ReceiverPhoneNum, string ReceiverAddress, string ReceiverPostalCode)
        {
            throw new NotImplementedException();
        }
    }
}
