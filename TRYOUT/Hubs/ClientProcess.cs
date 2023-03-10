using Microsoft.AspNetCore.SignalR;

namespace TRYOUT.Hubs
{
    public class ClientProcess
    {
        private CancellationTokenSource _tokenSource;
        public string ConnectionId { get; set; } = "";

        private IHubCallerClients _clients;

        public ClientProcess(string connectionId, IHubCallerClients clients)
        {
            _clients = clients;
            ConnectionId = connectionId;
            _tokenSource = new CancellationTokenSource();
        }

        public void ProcessConversion(string text)
        {
            var token = _tokenSource.Token;

            Random random = new Random();

            foreach (char characterInText in text)
            {
                if (token.IsCancellationRequested)
                {
                    _clients.Clients(ConnectionId).SendAsync("conversionCancelled", ConnectionId);
                    return;
                }

                Thread.Sleep(random.Next(1, 5) * 1000);

                _clients.Clients(ConnectionId).SendAsync("conversionUpdate", characterInText);
            }

            _clients.Clients(ConnectionId).SendAsync("conversionCompleted", text);
        }

        public async Task CancelConversion()
        {
            _tokenSource?.Cancel();
            await Task.Delay(100);
        }

    }
}
