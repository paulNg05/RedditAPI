using System;
using System.Net.Http;
using System.Runtime.Caching;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace RedditAPI.Service
{
    public class RetrieveAccessToken
    {

        private string clientId = "0RDV33GD8F-yh73dP7sHsw";
        private string clientSecret = "h80frHMgvt2AsQE8MOL7IkkvATyQOQ";
        private string username = "paulNg05";
        private string password = "unixCPS12345";

        protected DateTime? expired_in = null;
        protected string access_token = string.Empty;
        ObjectCache cacheToken = MemoryCache.Default;
        public async Task<string> GetAccessTokenAsync()
        {

            access_token = (string)cacheToken["AccessToken"];

            if (access_token == null)
            {                 
                    using (HttpClient httpClient = new HttpClient())
                    {
                        var content = new FormUrlEncodedContent(new[]
                        {
                            new KeyValuePair<string, string>("grant_type", "password"),
                            new KeyValuePair<string, string>("username", username),
                            new KeyValuePair<string, string>("password", password)
                        });

                        httpClient.DefaultRequestHeaders.Add("User-Agent", "puppyParser");

                        var authHeaderValue = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"{clientId}:{clientSecret}"));
                        httpClient.DefaultRequestHeaders.Add("Authorization", $"Basic {authHeaderValue}");

                        var response = await httpClient.PostAsync("https://www.reddit.com/api/v1/access_token", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = await response.Content.ReadAsStringAsync();
                        dynamic tokenResponse = JsonConvert.DeserializeObject(responseContent);
                        string accessToken = tokenResponse.access_token;
                        access_token = accessToken;
                        int expire_in = (int) tokenResponse.expires_in;
                        CacheItemPolicy policy = new CacheItemPolicy
                        {
                            AbsoluteExpiration = DateTimeOffset.UtcNow.AddSeconds(expire_in)
                        };
                        cacheToken.Add("AccessToken", access_token, policy);
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}");
                    }                    
                }
            }
            return access_token;
        }
    }
}
