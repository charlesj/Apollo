using Apollo.Runtime;

namespace Apollo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var kernel = new Kernel();
            var serviceLocator = kernel.Boot(BootOptions.Defaults);

            var runTime = serviceLocator.Get<IRuntime>();

            runTime.Run().GetAwaiter().GetResult();
        }
    }
}