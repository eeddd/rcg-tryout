namespace TRYOUT.Hubs
{
    public interface ILoopProcess
    {
        public string ConnectionId { get; set; }
        void StartLoopProcess(string text);
        void StopLoopProcess();
    }
}