using ExtendedApi.Providers;
using Ninject.Modules;

namespace ExtendedApi
{
    public class ApiNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Kernel.Bind<IMessageProvider>().To<MessageProvider>();
        }
    }
}