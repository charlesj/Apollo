using System.Threading.Tasks;

namespace Apollo.Runtime
{
    public interface IRuntime
    {
        Task Run();
    }

    public class SimpleLoopRuntime : IRuntime
    {
        public Task Run()
        {
            throw new System.NotImplementedException();
        }
    }
}