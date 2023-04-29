using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.DomainLayer.ExternalServices.ExternalDeliveryService
{
    public interface IDelivery
    {
        public bool ConfirmDelivery(string ReceiverFirstName, string ReceiverLastName,
           string ReceiverPhoneNum, string ReceiverAddress, string ReceiverPostalCode);
    }
}


