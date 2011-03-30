using MvcTurbine.ComponentModel;
using MvcTurbine.Ninject;
using MvcTurbine.Web;

namespace hyrmn
{
    public class MvcApplication : TurbineApplication
    {
        static MvcApplication()
        {
            ServiceLocatorManager.SetLocatorProvider(() => new NinjectServiceLocator());
        }
    }
}