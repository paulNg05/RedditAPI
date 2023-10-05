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
        public void GetMostUpVotePost_return_UpVoteNumber() 
        {
            //Arrange
            var mostUpvoteMock = new Mock<IGetUserPosts>();
            var mostUpVote = mostUpvoteMock.Object;
            string post = "business";
            string token = "ABC_eFg_234_eng";
            var upMostPost = new Dictionary<string, int>()
            { {"Inflation", 1000 } };

            //Set up behavior
            mostUpvoteMock.Setup(d => d.PostsWithMostUpVotes(post, token)).ReturnsAsync(upMostPost);

            //Act 
            var result = mostUpVote.PostsWithMostUpVotes(post, token);

            //Assert
            Assert.Equals(upMostPost, result);
        
        }
    }  
        
}