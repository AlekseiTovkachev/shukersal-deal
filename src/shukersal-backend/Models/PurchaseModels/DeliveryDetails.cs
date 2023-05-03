using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models.PurchaseModels
{
    public class DeliveryDetails
    {
        public string ReceiverFirstName { get; set; }
        public string ReceiverLastName { get; set; }
        public string ReceiverPhoneNum { get; set; }
        public string ReceiverAddress { get; set; }
        public string ReceiverPostalCode { get; set; }

        public Dictionary<long, List<TransactionItem>> TransactionItems { get; set; }

        public DeliveryDetails(TransactionPost TransactionDetails, Dictionary<long, List<TransactionItem>> transactionItems)
        {
            ReceiverFirstName = TransactionDetails.ReceiverFirstName;
            ReceiverLastName = TransactionDetails.ReceiverLastName;
            ReceiverPhoneNum = TransactionDetails.ReceiverPhoneNum;
            ReceiverAddress = TransactionDetails.ReceiverAddress;
            ReceiverPostalCode = TransactionDetails.ReceiverPostalCode;
            TransactionItems = transactionItems;
        }

    }
}
