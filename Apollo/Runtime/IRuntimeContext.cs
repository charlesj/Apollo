namespace Apollo.Runtime
{
    public interface IRuntimeContext
    {
        bool Ending { get; }
        long FrameNumber { get; }
        void End();
        void StartFrame();
    }
}