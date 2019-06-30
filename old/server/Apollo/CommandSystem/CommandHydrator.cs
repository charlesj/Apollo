using Newtonsoft.Json;

namespace Apollo.CommandSystem
{
    public class CommandHydrator : ICommandHydrator
    {
        public void Hydrate(ref ICommand command, object parameters)
        {
            var json = JsonConvert.SerializeObject(parameters);
            JsonConvert.PopulateObject(json, command);
        }
    }
}