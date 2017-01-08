namespace Apollo.CommandSystem
{
    public interface ICommandHydrator
    {
        void Hydrate(ref ICommand command, object parameters);
    }
}