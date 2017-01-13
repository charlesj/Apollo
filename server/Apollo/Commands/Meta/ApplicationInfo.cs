using System.Threading.Tasks;
using Apollo.CommandSystem;

namespace Apollo.Commands.Meta
{
    public class ApplicationInfo : ICommand
    {
        public Task<CommandResult> Execute()
        {
            var result = new CommandResult
            {
                Result = new ApplicationInfoResult
                {
                    version = Apollo.Version,
                    commitHash = Apollo.CommitHash,
                    compiledOn = Apollo.CompiledOn,
                }
            };

            return Task.FromResult(result);
        }

        public Task<bool> IsValid()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Authorize()
        {
            return Task.FromResult(true);
        }
    }

    public class ApplicationInfoResult
    {
        public string version { get; set; }
        public string commitHash { get; set; }
        public string compiledOn { get; set; }
    }
}