
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using TRYOUT.Services;

namespace TRYOUT.Hubs
{
    public class ConversionHub : Hub
    {
        private IConverterService _converterService;

        private static ConcurrentDictionary<string, LoopCharactersProcess> _processMap = new ConcurrentDictionary<string, LoopCharactersProcess>();
        
        public ConversionHub(IConverterService converterService)
        {
            this._converterService = converterService;
        }

        public void ProcessConversion(string text)
        {
            string encodedText = _converterService.Convert(text);

            var loopProcess = new LoopCharactersProcess(Context.ConnectionId, Clients);
            
            var task = Task.Run(() => loopProcess.StartLoopProcess(encodedText), loopProcess.GetToken());

            _processMap.TryAdd(Context.ConnectionId, loopProcess);
        }

        public void CancelConversion(string connectionId)
        {
            if (_processMap.ContainsKey(connectionId))
            {
                _processMap[connectionId].StopLoopProcess();
                _processMap.TryRemove(connectionId, out _);
            }
        }

    }    
}
