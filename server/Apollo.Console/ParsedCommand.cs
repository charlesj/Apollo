namespace Apollo.Console
{
    public class ParsedCommand
    {
        public ParsedCommand(string verb, ICommand command)
        {
            Verb = verb;
            Command = command;
        }

        public string Verb { get; }
        public ICommand Command { get; }
    }
}