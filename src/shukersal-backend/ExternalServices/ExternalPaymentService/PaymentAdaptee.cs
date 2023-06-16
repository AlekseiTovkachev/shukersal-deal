
using System.Net;

namespace shukersal_backend.ExternalServices.ExternalPaymentService
{
    public class PaymentAdaptee
    {
        HttpClient httpClient;
        private readonly string url;

        public PaymentAdaptee(string url)
        {
            this.url = url;
            httpClient = new HttpClient();
        }

        public async Task<string> handshake()
        {
            var postContent = new Dictionary<string, string>
            {
                {"action_type", "handshake"}
            };

            var content = new FormUrlEncodedContent(postContent);
            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    return "OK";
                }
                else
                {
                    return "Handshake failed";
                }
            }
            catch (Exception)
            {
                return "Handshake failed";
            }
        }

        public async Task<int> pay(string cardNumber, string month, string year, string holder, string ccv, string id)
        {
            var postContent = new Dictionary<string, string>
            {
                {"action_type", "pay"},
                {"card_number", cardNumber},
                {"month", month},
                {"year", year},
                {"holder", holder},
                {"ccv", ccv},
                {"id", id}
            };

            var content = new FormUrlEncodedContent(postContent);

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    int transactionId;
                    if (int.TryParse(responseBody, out transactionId) && transactionId!=-1)
                    {
                        return transactionId;
                    }
                }
            }
            catch (Exception)
            {
                return -1;
            }

            return -1; 
        }

        public async Task<long> cancel_pay(string transactionId)
        {
            var postContent = new Dictionary<string, string>
            {
                {"action_type", "cancel_pay"},
                {"transaction_id", transactionId}
            };

            var content = new FormUrlEncodedContent(postContent);

            try
            {
                HttpResponseMessage response = await httpClient.PostAsync(url, content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    int res;
                    if (int.TryParse(responseBody, out res) && res == 1)
                    {
                        return res;
                    }
                }
            }
            catch (Exception)
            {

                return -1;
            }

            return -1; 
        }
    }
}