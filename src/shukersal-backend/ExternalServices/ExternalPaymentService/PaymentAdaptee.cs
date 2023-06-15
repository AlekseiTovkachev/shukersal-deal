
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

                if (response.IsSuccessStatusCode)
                {
                    return "OK";
                }
                else
                {
                    // Handle specific error codes or error messages if required
                    return "Handshake failed";
                }
            }
            catch (Exception ex)
            {
                // Handle general exception or log the error
                return "Handshake failed: " + ex.Message;
            }
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

            try
            {
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
            }
            catch (Exception ex)
            {
                // Handle general exception or log the error
                return -1;
            }

            return -1; // Handle invalid response or other specific cases
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
                    return 1;
                }
            }
            catch (Exception ex)
            {
                // Handle general exception or log the error
                return -1;
            }

            return -1; // Handle invalid response or other specific cases
        }
    }
}