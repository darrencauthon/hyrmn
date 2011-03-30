using System.Configuration;
using MvcTurbine.ComponentModel;
using Spark;

namespace hyrmn.Infrastructure.ServiceRegistration
{
    public class DefaultServiceRegistration : IServiceRegistration
    {
        public void Register(IServiceLocator locator)
        {
            locator.Register<ISparkSettings>(() => ConfigurationManager.GetSection("spark") as ISparkSettings);
        }
    }
}