namespace RedditAPI.Service
{
    public interface IRetrieveAccessToken
    {
        Task<string> GetAccessTokenAsync();
    }
}