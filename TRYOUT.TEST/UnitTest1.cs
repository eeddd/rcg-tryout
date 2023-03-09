using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Moq;
using System.Threading.Tasks;
using TRYOUT.Hubs;
using TRYOUT.Services;
using static System.Net.Mime.MediaTypeNames;

namespace TRYOUT.TEST
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            // Arrange
            IBase64Service service = new Base64Service();
            var inputText = "Hello";
            var expectedValue = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(inputText));

            // Act
            var result = service.ConvertToBase64(inputText);


            // Assert
            Assert.Equal(expectedValue, result);

        }
    }
}