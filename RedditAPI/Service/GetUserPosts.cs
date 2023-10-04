using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditAPI.Service
{
    public class GetUserPosts
    {        
        public async Task<Dictionary<string, int>> GetUsersWithMostPostsAsyn(string subredditname,  string accessToken)
        {

            var posts = await GetSubredditPostsAsync(subredditname, "new", 1000, accessToken);           
            var userPostsCount = CountUserPosts(posts);
            
            var sortedUsers = userPostsCount.OrderByDescending(keyValue => keyValue.Value);

            var topUsers = new Dictionary<string, int>();           
            foreach (var user in sortedUsers.Take(5))  
            {
               topUsers.Add(user.Key, user.Value);
            }

            return topUsers;
        }

        public async Task<Dictionary<string, int>> GetPostsWithMostUpVotes(string subRedditName, string accessToken)
        {
            var topPosts = await GetSubredditPostsAsync(subRedditName, "top", 1000, accessToken);

            var topPostsDictionary = new Dictionary<string, int>();

            foreach (var post in topPosts)
            {
                string title = post["data"]["title"].ToString();
                int score = int.Parse(post["data"]["ups"].ToString());
                topPostsDictionary.Add(title, score);
            }
            return SortDictionaryByValues(topPostsDictionary);
        }

        static async Task<IEnumerable<JToken>> GetSubredditPostsAsync(string subredditName, string type, int limit, string accessToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "puppyParser");
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                
                string apiUrl = $"https://oauth.reddit.com/r/{subredditName}/{type}/.json?limit={limit}";
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
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
            var sortedPairs = dictionary.OrderByDescending(KeyValue => KeyValue.Value);

            var sortedDictionary = new Dictionary<TKey, TValue>();
            foreach (var KeyValue in sortedPairs)
            {
                sortedDictionary.Add(KeyValue.Key, KeyValue.Value);
            }
            return sortedDictionary;
        }
    }
}
