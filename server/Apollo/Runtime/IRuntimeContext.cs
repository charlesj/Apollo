namespace Apollo.Runtime
{
    public interface IRuntimeContext
    {
        bool Ending { get; }
        bool Ended { get; }
        long FrameNumber { get; }
        void End();
        void StartFrame();
        void CompleteShutdown();
    }
}