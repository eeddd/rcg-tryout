using Microsoft.AspNetCore.SignalR;

namespace TRYOUT.Hubs
{
    public class Base64Hub : Hub
    {
        public async Task Convert(string text)
        {
            await Clients.Clients(Context.ConnectionId).SendAsync("conversionStarted", Context.ConnectionId);

            AppClient appClient = new AppClient
            {
                Text = text,
                ConnectionId = Context.ConnectionId,
                token = new CancellationTokenSource(),
                Clients = Clients,
                Context = Context,
            };

            _clients.Add(Context.ConnectionId, appClient);

            WaitCallback wc = new WaitCallback(Base64Conversion);
            ThreadPool.QueueUserWorkItem(wc, appClient);

        }

        public void Cancel(string connectionId)
        {
            _clients[connectionId].token.Cancel(true);
        }

        private static IDictionary<string, AppClient> _clients = new Dictionary<string, AppClient>();


        private static void Base64Conversion(object msg)
        {
            AppClient client = (AppClient)msg;

            ThreadPool.QueueUserWorkItem(s =>
            {
                var encoded = System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(client.Text));
                var rand = new Random(DateTime.Now.Millisecond);

                //CancellationToken token = (CancellationToken) s;

                DateTime dt = DateTime.Now;
                TimeSpan ts = TimeSpan.Zero;


                for (int i = 0; i < encoded.Length; i++)
                {
                    if (client.token.Token.IsCancellationRequested)
                        break;
                    ts = DateTime.Now - dt;
                    dt = DateTime.Now;
                    var task = client.Clients.Clients(client.Context.ConnectionId).SendAsync("conversionUpdate", i, encoded[i]);
                    var result = task.WaitAsync(ts);

                    client.token.Token.WaitHandle.WaitOne(rand.Next(1000, 5000));
                }

                string textResult = client.token.Token.IsCancellationRequested ? "" : encoded;
                string method = client.token.Token.IsCancellationRequested ? "conversionCancelled" : "conversionCompleted";

                var taskcomplete = client.Clients.Clients(client.Context.ConnectionId).SendAsync(method, textResult);
                var resultcomplete = taskcomplete.WaitAsync(ts);

                _clients.Remove(client.ConnectionId);

            }, client.token.Token);
        }
    }

    class AppClient
    {
        public string Text { get; set; }
        public string ConnectionId { get; set; }

        public CancellationTokenSource token { get; set; }

        public IHubCallerClients Clients { get; set; }

        public HubCallerContext Context { get; set; }
    }
}
