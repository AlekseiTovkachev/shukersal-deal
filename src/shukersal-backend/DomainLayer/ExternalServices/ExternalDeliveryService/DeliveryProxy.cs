using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;

namespace shukersal_backend.DomainLayer.ExternalServices.ExternalDeliveryService
{
    public class DeliveryProxy : IDelivery
    {
        private RealDeliveryAdapter? RealDeliveryAdapter;
        private bool ProxyAns;

        public DeliveryProxy()
        {
            ProxyAns = true;
        }

        public bool ConfirmDelivery(DeliveryDetails deliveryDetails, List<TransactionItem> items)
        {
            if (RealDeliveryAdapter == null) { return ProxyAns; }
            return RealDeliveryAdapter.ConfirmDelivery(deliveryDetails,items);

        }

        public void SetProxyAnswer(bool NewAns) { ProxyAns = NewAns; }
    }
}
