namespace Apollo.Console
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            CommandRunner.Run<AvailableCommands>(args);
        }
    }
}