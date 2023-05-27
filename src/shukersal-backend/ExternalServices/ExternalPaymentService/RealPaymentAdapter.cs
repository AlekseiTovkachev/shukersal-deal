using shukersal_backend.Models.PurchaseModels;

namespace shukersal_backend.ExternalServices.ExternalPaymentService
{
    public class RealPaymentAdapter : IPayment
    {
        private PaymentAdaptee adaptee;
        public RealPaymentAdapter(PaymentAdaptee adaptee)
        {
            this.adaptee = adaptee;
        }

        public bool Handshake()
        {
            var response=adaptee.handshake();
            if(response==null || response.Result!="OK") 
            { 
                return false; 
            }
            return true;

        }
        public bool ConfirmPayment(PaymentDetails paymentDetails)
        {
            var month = paymentDetails.ExpirationDate.Month.ToString();
            var year = paymentDetails.ExpirationDate.Year.ToString();
            var holder = paymentDetails.HolderFirstName + " " + paymentDetails.HolderFirstName;
            var response=adaptee.pay(paymentDetails.CardNumber, month, year, holder, paymentDetails.CVC, paymentDetails.HolderID);
            
            if (response == null || response.Result == -1) 
            { 
                return false;
            }
            return true;
        }
        public bool CancelPayment(long transactionId)
        {
            var response = adaptee.cancel_pay(transactionId.ToString());
            if(response==null||response.Result==-1) 
            { 
                return false; 
            }

            return true;
        }

        public void SetPaymentAdaptee(string url)
        {
            adaptee = new PaymentAdaptee(url);
        }


    }
}
