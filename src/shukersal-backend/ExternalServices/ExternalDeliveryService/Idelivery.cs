using shukersal_backend.Models;
using shukersal_backend.Models.PurchaseModels;
using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.ExternalServices.ExternalDeliveryService
{
    public interface IDelivery
    {
        public bool Handshake();
        public bool ConfirmDelivery(DeliveryDetails deliveryDetails, List<TransactionItem> items);
        public bool CancelDelivery(long TransactionId);
    }
}


