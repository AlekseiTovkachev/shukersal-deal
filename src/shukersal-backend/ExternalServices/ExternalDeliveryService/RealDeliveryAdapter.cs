using shukersal_backend.ExternalServices.ExternalPaymentService;
using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;

namespace shukersal_backend.ExternalServices.ExternalDeliveryService
{
    public class RealDeliveryAdapter : IDelivery
    {
        private DeliveryAdaptee adaptee;
        public RealDeliveryAdapter(DeliveryAdaptee adaptee)
        {
            this.adaptee = adaptee;
        }

        public bool Handshake()
        {
            var response = adaptee.handshake();
            if (response == null || response.Result != "OK")
            {
                return false;
            }
            return true;
        }
        public bool CancelDelivery(long TransactionId)
        {
            var response = adaptee.cancel_supply(TransactionId.ToString());
            if (response == null || response.Result == -1)
            {
                return false;
            }

            return true;
        }

        public bool ConfirmDelivery(DeliveryDetails deliveryDetails, List<TransactionItem> items)
        {
            var name = deliveryDetails.ReceiverFirstName + " " + deliveryDetails.ReceiverLastName;
            var response = adaptee.supply(name, deliveryDetails.ReceiverAddress, deliveryDetails.ReceiverCity, deliveryDetails.ReceiverCountry,deliveryDetails.ReceiverPostalCode);

            if (response == null || response.Result == -1)
            {
                return false;
            }
            return true;
        }


        public void SetDeliveryAdaptee(string url)
        {
            adaptee = new DeliveryAdaptee(url);
        }


    }
}
