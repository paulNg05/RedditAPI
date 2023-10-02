using Newtonsoft.Json.Linq;
using Reddit.Things;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditAPI.Service
{
    public class GetPostsWithMostUpVotes
    {
        public async Task<Dictionary<string, int>> PostsWithMostUpVotes(string subRedditName, string accessToken) 
        {
            var topPosts = await GetTopPostsAsync(subRedditName, accessToken);

            var topPostsDictionary = new Dictionary<string, int>();

            foreach (var post in topPosts)
            {
                string title = post["data"]["title"].ToString();
                int score = int.Parse(post["data"]["score"].ToString());
                topPostsDictionary.Add(title, score);
            }
            return SortDictionaryByValues(topPostsDictionary); 
        }
        static async Task<IEnumerable<JToken>> GetTopPostsAsync(string subredditName, string accessToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "puppyParser");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                //string apiUrl = $"https://www.reddit.com/r/{subredditName}/top/.json?limit=100";
                string apiUrl = $"https://oauth.reddit.com/r/{subredditName}/top/.json?limit=10";

                try
                {
                    var response = await httpClient.GetAsync(apiUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseData = await response.Content.ReadAsStringAsync();
                        var data = JObject.Parse(responseData);
                        return data["data"]["children"];
                    }
                    else
                    {
                        throw new Exception($"Error: {response.StatusCode}");

                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                

                
            }
        }
        static Dictionary<TKey, TValue> SortDictionaryByValues<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            
            var sortedPairs = dictionary.OrderBy(kvp => kvp.Value);
            
            var sortedDictionary = new Dictionary<TKey, TValue>();
            foreach (var kvp in sortedPairs)
            {
                sortedDictionary.Add(kvp.Key, kvp.Value);
            }

            return sortedDictionary;
        }
    }
}

