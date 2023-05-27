namespace shukersal_backend.ExternalServices.ExternalDeliveryService
{
    public class DeliveryAdaptee
    {
        HttpClient httpClient;
        private readonly string url;
        public DeliveryAdaptee(string url) 
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

        public async Task<int> supply(string name, string address, string city, string country, string zip)
        {
            var postContent = new Dictionary<string, string>
        {
            {"action_type", "supply"},
            {"name", name},
            {"address", address},
            {"city", city},
            {"country", country},
            {"zip", zip}
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

        public async Task<int> cancel_supply(string transactionId)
        {
            var postContent = new Dictionary<string, string>
        {
            {"action_type", "cancel_supply"},
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

