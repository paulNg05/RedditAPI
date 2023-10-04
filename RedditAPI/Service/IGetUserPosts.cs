namespace RedditAPI.Service
{
    public interface IGetUserPosts
    {
        Task<Dictionary<string, int>> GetUsersWithMostPostsAsyn(string subredditname, string accessToken);
        Task<Dictionary<string, int>> PostsWithMostUpVotes(string subRedditName, string accessToken);
    }
}