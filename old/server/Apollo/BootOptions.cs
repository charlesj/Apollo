namespace Apollo
{
    public class BootOptions
    {
        public BootOptions()
        {
            EnableLogging = false;
        }

        public bool EnableLogging { get; set; }

        public static BootOptions Defaults => new BootOptions();
    }
}
