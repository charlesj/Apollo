using System.Threading.Tasks;
using Apollo.Utilities;

namespace Apollo.Runtime
{
    public class SimpleLoopRuntime : IRuntime
    {
        private readonly IClock clock;
        private readonly IRuntimeContext context;

        public SimpleLoopRuntime(IClock clock, IRuntimeContext context)
        {
            this.clock = clock;
            this.context = context;
        }

        public async Task Run()
        {
            while (!this.context.Ending)
            {
                this.context.StartFrame();
                await this.clock.Delay(10);
            }

            this.context.CompleteShutdown();
        }
    }
}