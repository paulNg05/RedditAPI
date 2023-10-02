using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RedditAPI.Service
{
    public class GetUserWithTopPosts
    {

        
        public async Task<Dictionary<string, int>> GetUserWithMostPostsAsyn(string subredditname, string accessToken)
        {
           // string subredditName = subredditname;
            //RetrieveAccessToken _retrieveAccessToken = new RetrieveAccessToken();
           // string accessToken = await _retrieveAccessToken.GetAccessTokenAsync();

            // Make a GET request to retrieve posts from the subreddit
            var posts = await GetSubredditPostsAsync(subredditname, accessToken);

            // Extract and count the usernames of authors
            var userPostsCount = CountUserPosts(posts);

            // Sort users by the number of posts (descending order)
            var sortedUsers = userPostsCount.OrderByDescending(kv => kv.Value);

            var topUsers = new Dictionary<string, int>();           
            foreach (var user in sortedUsers.Take(10))  
            {
               topUsers.Add(user.Key, user.Value);
            }

            return topUsers;
        }
        
        static async Task<IEnumerable<JToken>> GetSubredditPostsAsync(string subredditName, string accessToken)
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.DefaultRequestHeaders.Add("User-Agent", "puppyParser"); // Set your app's user-agent
                httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");

                //string apiUrl = $"https://www.reddit.com/r/{subredditName}/new/.json?limit=1000"; 
                string apiUrl = $"https://oauth.reddit.com/r/{subredditName}/new/.json?limit=1000";
                var response = await httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var data = JObject.Parse(responseData);
                    return data["data"]["children"];
                }
                else
                {
                    // Handle error
                    Console.WriteLine($"Error: {response.StatusCode}");
                    return null;
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
    }
}
