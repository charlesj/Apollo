using System;
using Apollo.Data;
using Apollo.ServiceLocator;

namespace Apollo
{
    public class Kernel
    {
        public IServiceLocator Boot(BootOptions options, IServiceLocator serviceLocator)
        {
            serviceLocator.RegisterServices();
            var locator =  serviceLocator.Get<IServiceLocator>();

            var configuration = locator.Get<IConfiguration>();
            if (!configuration.IsValid())
            {
                Logger.Error("Invalid Configuration");
                return null;
            }

            Logger.Info("Validated Configuration");

            var documentBootstrapper = locator.Get<IDocumentStoreBoostrapper>();

            documentBootstrapper.Bootstrap().GetAwaiter().GetResult();
            Logger.Info("Bootstrapped Documents");

            return locator;
        }
    }
}
