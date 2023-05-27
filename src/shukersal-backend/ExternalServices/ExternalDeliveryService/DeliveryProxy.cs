using shukersal_backend.ExternalServices.ExternalPaymentService;
using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;

namespace shukersal_backend.ExternalServices.ExternalDeliveryService
{
    public class DeliveryProxy : IDelivery
    {
        private RealDeliveryAdapter? RealDeliveryAdapter;
        private bool ProxyAns;

        public DeliveryProxy()
        {
            ProxyAns = true;
        }

        public bool CancelDelivery(long TransactionId)
        {
            if (RealDeliveryAdapter == null) { return ProxyAns; }
            return RealDeliveryAdapter.CancelDelivery(TransactionId);
        }

        public bool ConfirmDelivery(DeliveryDetails deliveryDetails, List<TransactionItem> items)
        {
            if (RealDeliveryAdapter == null) { return ProxyAns; }
            return RealDeliveryAdapter.ConfirmDelivery(deliveryDetails, items);

        }

        public bool Handshake()
        {
            if (RealDeliveryAdapter == null) { return ProxyAns; }
            return RealDeliveryAdapter.Handshake();
        }

        public void SetProxyAnswer(bool NewAns) { ProxyAns = NewAns; }


        public void SetDeliveryProvider(RealDeliveryAdapter Adapter)
        {
            RealDeliveryAdapter = Adapter;
        }

    }
}
