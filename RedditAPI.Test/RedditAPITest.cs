using RedditAPI.Service;
using Moq;
using System.Buffers;

namespace RedditAPI.Test
{
    [TestClass]
    public class RedditAPITest
    {


        [TestMethod]
        public void GetToken_Return_TokenValue()
        {
            //Arrange
            var retrieveTokenMock = new Mock<IRetrieveAccessToken>();
            var retrieveToken = retrieveTokenMock.Object;

            //set up behavior
            retrieveTokenMock.Setup(d => d.GetAccessTokenAsync()).ReturnsAsync("ABC_eFg_234_eng");

            //Act
            var token = retrieveToken.GetAccessTokenAsync().Result;

            //Assert 
            Assert.AreEqual("ABC_eFg_234_eng", token);
        }


        [TestMethod]
        public void GetToken_Return_Empty()
        {
            //Arrange
            var retrieveTokenMock = new Mock<IRetrieveAccessToken>();
            var retrieveToken = retrieveTokenMock.Object;

            //set up behavior
            retrieveTokenMock.Setup(d => d.GetAccessTokenAsync()).ReturnsAsync("");

            //Act
            var token = retrieveToken.GetAccessTokenAsync().Result;

            //Assert 
            Assert.AreEqual("", token);
        }

        [TestMethod]
        public void GetMostUpVotePost_return_DictionaryWithValue() 
        {
            //Arrange
            var mostUpvoteMock = new Mock<IGetUserPosts>();
            var mostUpVote = mostUpvoteMock.Object;
            string post = "business";
            string token = "ABC_eFg_234_eng";
            var upMostPost = new Dictionary<string, int>()
            { {"Inflation Is under Control", 1000 } };

            //Set up behavior
            mostUpvoteMock.Setup(d => d.GetPostsWithMostUpVotesAsync(post, token)).ReturnsAsync(upMostPost);

            //Act 
            var result = mostUpVote.GetPostsWithMostUpVotesAsync(post, token).Result;

            //Assert
            Assert.AreEqual(upMostPost, result);        
        }

        [TestMethod]
        public void GetMostUpVotePost_return_EmptyDictionary()
        {
            //Arrange
            var mostUpvoteMock = new Mock<IGetUserPosts>();
            var mostUpVote = mostUpvoteMock.Object;
            string post = "business";
            string token = "ABC_eFg_234_eng";
            var upMostPost = new Dictionary<string, int>();
            
            //Set up behavior
            mostUpvoteMock.Setup(d => d.GetPostsWithMostUpVotesAsync(post, token)).ReturnsAsync(upMostPost);

            //Act 
            var result = mostUpVote.GetPostsWithMostUpVotesAsync(post, token).Result;

            //Assert
            Assert.AreEqual(upMostPost, result);
        }

        [TestMethod]
        public void GetUserWithMostPost_rturn_DictionaryWithValue()
        {
            //Arrange
            var userWithMostPostMock = new Mock<IGetUserPosts>();
            var userMostPost = userWithMostPostMock.Object;
            string post = "business";
            string token = "ABC_eFg_234_eng";
            var userWithMostPost = new Dictionary<string, int>()
            { {"_entrepreneur1234", 10 } };

            //Set up behavior
            userWithMostPostMock.Setup(d => d.GetUsersWithMostPostsAsync(post, token)).ReturnsAsync(userWithMostPost);

            //Act 
            var result = userMostPost.GetUsersWithMostPostsAsync(post, token).Result;

            //Assert
            Assert.AreEqual(userWithMostPost, result);
        }

        [TestMethod]
        public void GetUserWithMostPost_rturn_EmptyDictionary()
        {
            //Arrange
            var userWithMostPostMock = new Mock<IGetUserPosts>();
            var userMostPost = userWithMostPostMock.Object;
            string post = "business";
            string token = "ABC_eFg_234_eng";
            var userWithMostPost = new Dictionary<string, int>();          

            //Set up behavior
            userWithMostPostMock.Setup(d => d.GetUsersWithMostPostsAsync(post, token)).ReturnsAsync(userWithMostPost);

            //Act 
            var result = userMostPost.GetUsersWithMostPostsAsync(post, token).Result;

            //Assert
            Assert.AreEqual(userWithMostPost, result);
        }
    }  
        
}