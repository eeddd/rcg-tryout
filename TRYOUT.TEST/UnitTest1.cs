using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System.Threading.Tasks;
using TRYOUT.Hubs;

namespace TRYOUT.TEST
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            string inputText = "edorgel";
            string encoded = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(inputText));

            // arrange
            Mock<IHubCallerClients> mockClients = new Mock<IHubCallerClients>();
            Mock<IClientProxy> mockClientProxy = new Mock<IClientProxy>();

            mockClients.Setup(clients => clients.All).Returns(mockClientProxy.Object);

            Base64Hub simpleHub = new Base64Hub()
            {
                Clients = mockClients.Object
            };

            // act
            await simpleHub.Convert(inputText);


            // assert
            mockClients.Verify(clients => clients.All, Times.Once);

            for (int i = 0; i < encoded.Length; i++)
            {
                mockClientProxy.Verify(
                    clientProxy => clientProxy.SendAsync(
                        "conversionUpdate",
                        It.Is<int>(index => index == i),
                        It.Is<string>(ch => ch.Equals(char.ToString(encoded[i]))),
                        default(CancellationToken)),
                    Times.Once);
            }

        }
    }
}