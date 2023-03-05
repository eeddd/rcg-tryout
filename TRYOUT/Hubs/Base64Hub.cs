using Microsoft.AspNetCore.SignalR;

namespace TRYOUT.Hubs
{
    public class Base64Hub : Hub
    {
        public async Task Convert(string text)
        {
            string encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(text));

            await Clients.Clients(Context.ConnectionId).SendAsync("conversionCompleted", encoded);
        }
    }
}
