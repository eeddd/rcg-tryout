
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using TRYOUT.Services;

namespace TRYOUT.Hubs
{
    public class ProcessConversionHub : Hub
    {
        private IConverterService _converterService;

        private static ClientProcess _appClient;
        

        public ProcessConversionHub(IConverterService base64Service)
        {
            this._converterService = base64Service;
        }

        public async Task ProcessConversion(string text)
        {
            _appClient = new ClientProcess(Context.ConnectionId, Clients);

            string encodedText = _converterService.Convert(text);

            await Clients.Clients(Context.ConnectionId).SendAsync("conversionStarted", Context.ConnectionId);
            
            var task = Task.Run(() => {
                _appClient.ProcessConversion(encodedText);
                });
        }

        
        public async Task CancelConversion(string connectionId)
        {
            await _appClient.CancelConversion();
        }

    }    
}
