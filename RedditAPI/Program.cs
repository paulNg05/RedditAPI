using System;
using System.Diagnostics;
using RedditAPI.Model;
using RedditAPI.Service;
using Microsoft.Extensions.DependencyInjection;

class Program
{
    static async Task Main(string[] args)
    {

        // setup DI
        var serviceProvider = new ServiceCollection()
            .AddSingleton<IRetrieveAccessToken, RetrieveAccessToken>()
            .AddSingleton<IGetUserPosts, GetUserPosts>()
            .BuildServiceProvider();

        var tokenObj = serviceProvider.GetService<IRetrieveAccessToken>();
        var topPostObject = serviceProvider.GetRequiredService<IGetUserPosts>();


        Console.WriteLine("Welcome to my Reddit API app");

        string subreddit = "politics";

        int durationMillSec = 6 * 60 * 1000; // 6 minutes
        int waitTimeToretrieveInfo = 3 * 60 * 1000; // 3 minutes
                                                    // 
        //var tokenObj = new RetrieveAccessToken();
        //var topPostObject = new GetUserPosts();

        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();

        do
        {          
            string accessToken = tokenObj.GetAccessTokenAsync().Result;          

            var upVotePost = topPostObject.PostsWithMostUpVotes(subreddit, accessToken).Result;

            int i = 0;
            Console.WriteLine("================ Report for top 5 Post with Most Up Votes ========================");
            foreach (var post in upVotePost)
            {
                Console.WriteLine($"Title: {post.Key}");
                Console.WriteLine($"Votes: {post.Value}");
                Console.WriteLine();
                i++;
                if (i == 5) break;
            }
            var topPosts = topPostObject.GetUsersWithMostPostsAsyn(subreddit, accessToken).Result;
            Console.WriteLine("================ Report for top 5 Users with Most Posts ========================");
            foreach (var post in topPosts)
            {
                Console.WriteLine($"Author: {post.Key}");
                Console.WriteLine($"Number of Posts: {post.Value}");
                Console.WriteLine();
            }
            await Task.Delay(waitTimeToretrieveInfo);

        } while (stopwatch.ElapsedMilliseconds < durationMillSec);
        stopwatch.Stop();
        Console.WriteLine();
        Console.WriteLine("Thank you for using my Reddit API app. Have a good day! ");
    }   
}