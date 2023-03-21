using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.SignalR;

namespace TRYOUT.Hubs
{
    public class LoopCharactersProcess : ILoopProcess
    {
        private CancellationTokenSource? _tokenSource;

        private IHubContext<ConversionHub> _hubContext;

        public string ConnectionId { get; set; } = "";

        public LoopCharactersProcess(IHubContext<ConversionHub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void StartLoopProcess(string text)
        {
            _tokenSource = new CancellationTokenSource();
            var token = _tokenSource.Token;

            Task.Run(() =>
            {
                _hubContext.Clients.Clients(ConnectionId).SendAsync("conversionStarted", ConnectionId);

                var randomizeSeconds = new Random();

                foreach (var character in text)
                {
                    if (token.IsCancellationRequested)
                    {
                        _hubContext.Clients.Clients(ConnectionId).SendAsync("conversionCancelled", ConnectionId);
                        return;
                    }

                    Thread.Sleep(randomizeSeconds.Next(1, 5) * 1000);

                    _hubContext.Clients.Clients(ConnectionId).SendAsync("conversionUpdate", character);
                }

                _hubContext.Clients.Clients(ConnectionId).SendAsync("conversionCompleted", ConnectionId);
            }, token);
        }

        public void StopLoopProcess()
        {
            _tokenSource?.Cancel();
        }

    }
}
