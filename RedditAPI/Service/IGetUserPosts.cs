namespace RedditAPI.Service
{
    public interface IGetUserPosts
    {
        Task<Dictionary<string, int>> GetUsersWithMostPostsAsync(string subredditname, string accessToken);
        Task<Dictionary<string, int>> GetPostsWithMostUpVotesAsync(string subRedditName, string accessToken);
    }
}