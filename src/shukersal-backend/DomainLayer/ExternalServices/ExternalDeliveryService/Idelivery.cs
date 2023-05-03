using shukersal_backend.Models.PurchaseModels;
using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.DomainLayer.ExternalServices.ExternalDeliveryService
{
    public interface IDelivery
    {
        public bool ConfirmDelivery(DeliveryDetails deliveryDetails);
    }
}


