using Microsoft.AspNetCore.SignalR;

namespace TRYOUT.Hubs
{
    public class LoopCharactersProcess
    {
        private readonly CancellationTokenSource _tokenSource = new CancellationTokenSource();
        public string ConnectionId { get; set; } = "";

        private IHubCallerClients _clients;

        public LoopCharactersProcess(string connectionId, IHubCallerClients clients)
        {
            _clients = clients;
            ConnectionId = connectionId;
        }

        public void StartLoopProcess(string text)
        {
            _clients.Clients(ConnectionId).SendAsync("conversionStarted", ConnectionId);

            var cancellationToken = GetToken();

            var randomizedSeconds = new Random();

            foreach (char characterInText in text)
            {
                if (cancellationToken.IsCancellationRequested)
                {
                    _clients.Clients(ConnectionId).SendAsync("conversionCancelled", ConnectionId);
                    return;
                }

                Thread.Sleep(randomizedSeconds.Next(1, 5) * 1000);

                _clients.Clients(ConnectionId).SendAsync("conversionUpdate", characterInText);
            }
            _clients.Clients(ConnectionId).SendAsync("conversionCompleted", text);
        }

        public void StopLoopProcess()
        {
            _tokenSource?.Cancel();
        }

        public CancellationToken GetToken()
        {
            return _tokenSource.Token;
        }

    }
}
