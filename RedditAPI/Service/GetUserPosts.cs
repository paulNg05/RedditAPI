using Newtonsoft.Json.Linq;
using RedditAPI.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RedditAPI.Service
{
    public class GetUserPosts : IGetUserPosts
    {

        private RadditRateLimitInfo rateLimitInfo = new();
        

        public async Task<Dictionary<string, int>> GetUsersWithMostPostsAsyn(string subredditname, string accessToken)
        {
            if (rateLimitInfo.RemainingCalls == 0 && rateLimitInfo.Reset != 0)
            {
                // delay for the Reset time and call to get new token
                await Task.Delay(rateLimitInfo.Reset);
                var tokenObj = new RetrieveAccessToken();
                accessToken = await tokenObj.GetAccessTokenAsync();
            }

            var posts = await GetSubredditPostsAsync(subredditname, "new", 1000, accessToken);
            var userPostsCount = CountUserPosts(posts);

            var sortedUsers = userPostsCount.OrderByDescending(kv => kv.Value);

            var topUsers = new Dictionary<string, int>();
            foreach (var user in sortedUsers.Take(5))
            {
                topUsers.Add(user.Key, user.Value);
            }

            return topUsers;
        }

        public async Task<Dictionary<string, int>> PostsWithMostUpVotes(string subRedditName, string accessToken)
        {
            if (rateLimitInfo.RemainingCalls == 0 && rateLimitInfo.Reset != 0)
            {
                // delay for the Reset time and call to get new token
                await Task.Delay(rateLimitInfo.Reset);
                var tokenObj = new RetrieveAccessToken();
                accessToken = await tokenObj.GetAccessTokenAsync();
            }
            var topPosts = await GetSubredditPostsAsync(subRedditName, "top", 1000, accessToken);

            var topPostsDictionary = new Dictionary<string, int>();

            foreach (var post in topPosts)
            {
                string title = post["data"]["title"].ToString();
                int score = int.Parse(post["data"]["score"].ToString());
                topPostsDictionary.Add(title, score);
            }
            return SortDictionaryByValues(topPostsDictionary);
        }

        private async Task<IEnumerable<JToken>> GetSubredditPostsAsync(string subredditName, string type, int limit, string accessToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "puppyParser");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                string apiUrl = $"https://oauth.reddit.com/r/{subredditName}/{type}/.json?limit={limit}";
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    if (response.Headers.TryGetValues("x-ratelimit-remaining", out var remainingValues) && response.Headers.TryGetValues("x-ratelimit-reset", out var resetValues))
                    {
                        decimal rtRemainingValues = decimal.Parse(remainingValues.First());
                        int rlReset = int.Parse(resetValues.First());
                        rateLimitInfo.RemainingCalls = Convert.ToInt32(rtRemainingValues);
                        rateLimitInfo.Reset = rlReset * 1000;
                    }

                    var responseData = await response.Content.ReadAsStringAsync();
                    var data = JObject.Parse(responseData);
                    return data["data"]["children"];
                }
                else
                {
                    throw new Exception($"Error: Get subreddit post failed. Error code {response.StatusCode}");
                }
            }
        }

        static Dictionary<string, int> CountUserPosts(IEnumerable<JToken> posts)
        {
            var userPostsCount = new Dictionary<string, int>();

            foreach (var post in posts)
            {
                var author = post["data"]["author"].ToString();
                if (userPostsCount.ContainsKey(author))
                {
                    userPostsCount[author]++;
                }
                else
                {
                    userPostsCount[author] = 1;
                }
            }
            return userPostsCount;
        }
        static Dictionary<TKey, TValue> SortDictionaryByValues<TKey, TValue>(Dictionary<TKey, TValue> dictionary)
        {
            var sortedPairs = dictionary.OrderByDescending(kvp => kvp.Value);

            var sortedDictionary = new Dictionary<TKey, TValue>();
            foreach (var kvp in sortedPairs)
            {
                sortedDictionary.Add(kvp.Key, kvp.Value);
            }
            return sortedDictionary;
        }
    }
}
