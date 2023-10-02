using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using RedditAPI.Service;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Welcome to my Reddit API app");

        string subreddit = "news";

        var tokenObj = new RetrieveAccessToken();
        string accessToken = tokenObj.GetAccessTokenAsync().Result;

        var mostUpVoteObj = new GetPostsWithMostUpVotes();
        var mostUpVotes = mostUpVoteObj.PostsWithMostUpVotes(subreddit, accessToken);  // This has issue when call 

        var topPostObject = new GetUserWithTopPosts();
        var topPosts = topPostObject.GetUserWithMostPostsAsyn(subreddit,accessToken).Result;

        
    }
}