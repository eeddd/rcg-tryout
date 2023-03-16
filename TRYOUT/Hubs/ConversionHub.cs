
using Microsoft.AspNetCore.SignalR;
using System.Collections.Concurrent;
using TRYOUT.Services;

namespace TRYOUT.Hubs
{
    public class ConversionHub : Hub
    {
        private IConverterService _converterService;

        private ILoopProcess _loopCharactersProcess;
                
        public ConversionHub(IConverterService converterService, ILoopProcess loopCharactersProcess)
        {
            _converterService = converterService;
            _loopCharactersProcess = loopCharactersProcess;
        }

        public void ProcessConversion(string text)
        {
            string encoded = _converterService.Convert(text);

            _loopCharactersProcess.StartLoopProcess(encoded);
        }

        public void CancelConversion()
        {
            _loopCharactersProcess.StopLoopProcess();
        }

    }    
}
