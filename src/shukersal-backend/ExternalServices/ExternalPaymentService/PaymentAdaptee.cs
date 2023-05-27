using Azure;
using shukersal_backend.Models;
using shukersal_backend.Utility;
using System.Net;
using System.Net.Http;

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

            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return "OK";
            }
            return "Handshake failed";
        }

        public async Task<long> pay(string cardNumber, string month, string year, string holder, string ccv, string id)
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

            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                int transactionId;
                if (int.TryParse(responseBody, out transactionId))
                {
                    return transactionId;
                }
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

            HttpResponseMessage response = await httpClient.PostAsync(url, content);

            if (response.IsSuccessStatusCode)
            {
                return 1;
            }
            return -1;
        
        }
    }
}
