using System.ComponentModel.DataAnnotations;

namespace shukersal_backend.Models.PurchaseModels
{
    public class PaymentDetails
    {
        public double TotalPrice { get; set; }
        public string HolderFirstName { get; set; }
        public string HolderLastName { get; set; }
        public string HolderID { get; set; }
        public string CardNumber { get; set; }
        public DateOnly expirationDate { get; set; }
        public string CVC { get; set; }

        public PaymentDetails(TransactionPost transactionDeails) {
            TotalPrice = transactionDeails.TotalPrice;
            HolderFirstName = transactionDeails.HolderFirstName;
            HolderLastName = transactionDeails.HolderLastName;
            HolderID = transactionDeails.HolderID;
            CardNumber = transactionDeails.CardNumber;
            expirationDate = transactionDeails.expirationDate;
            CVC = transactionDeails.CVC;
        }
        


    }
}
