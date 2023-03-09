using Microsoft.AspNetCore.SignalR;
using TRYOUT.Services;

namespace TRYOUT.Hubs
{
    public class Base64Hub : Hub
    {
        private IBase64Service _base64Service;


        public Base64Hub(IBase64Service base64Service)
        {
            this._base64Service = base64Service;
        }


        public async Task ConvertToBase64(string text, CancellationToken cancellationTokenSource)
        {
            string encodedText = _base64Service.ConvertToBase64(text);

            Random random = new Random();

            for (int i = 0; i < encodedText.Length; i++)
            {
                if (cancellationTokenSource.IsCancellationRequested)
                {
                    break;
                }

                await Clients.Clients(Context.ConnectionId).SendAsync("conversionUpdate", encodedText[i]);

                await Task.Delay(random.Next(1, 5) * 1000);
            }

            await Clients.Clients(Context.ConnectionId).SendAsync("conversionCompleted", encodedText);
        }

    } 
}
