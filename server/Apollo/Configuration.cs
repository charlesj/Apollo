using System;
using System.Collections.Generic;
using System.Linq;
using Apollo.Utilities;

namespace Apollo
{
    public interface IConfiguration
    {
        string DatabaseName();
        string DatabaseServer();
        string DatabaseUsername();
        string DatabasePassword();
        string LoginPasswordHash();

        bool IsValid();
    }

    public class Configuration : IConfiguration
    {
        private readonly IEnvironmentReader reader;

        public Configuration(IEnvironmentReader reader)
        {
            this.reader = reader;
        }

        public string DatabaseName()
        {
            return reader.Read(Constants.EnvironmentalVars.DatabaseName);
        }

        public string DatabaseServer()
        {
            return reader.Read(Constants.EnvironmentalVars.DatabaseServer);
        }

        public string DatabaseUsername()
        {
            return reader.Read(Constants.EnvironmentalVars.DatabaseUsername);
        }

        public string DatabasePassword()
        {
            return reader.Read(Constants.EnvironmentalVars.DatabasePassword);
        }

        public string LoginPasswordHash()
        {
            return reader.Read(Constants.EnvironmentalVars.LoginHash);
        }

        public bool IsValid()
        {
            try
            {
                var checkVals = new List<Func<string>>()
                {
                    DatabaseName,
                    DatabaseServer,
                    DatabaseUsername,
                    DatabaseUsername,
                    LoginPasswordHash
                };

                return checkVals.All(v => !string.IsNullOrWhiteSpace(v()));
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}