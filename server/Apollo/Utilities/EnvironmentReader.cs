using System;

namespace Apollo.Utilities
{
    public interface IEnvironmentReader
    {
        string Read(string variableName);
    }

    public class EnvironmentReader : IEnvironmentReader
    {
        public string Read(string variableName)
        {
            return Environment.GetEnvironmentVariable(variableName);
        }
    }
}