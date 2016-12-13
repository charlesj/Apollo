namespace Apollo.Runtime
{
    public class RuntimeContext
    {
        public bool Ending { get; private set; }
        public long FrameNumber { get; private set; }

        public void End()
        {
            this.Ending = true;
        }

        public void StartFrame()
        {
            this.FrameNumber++;
        }
    }
}
