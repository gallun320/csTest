
using Ninject.Modules;
using RequestWorker;

namespace Utils {
    class Registration : NinjectModule {
        public override void Load() 
        {
            Bind<IWorker>().To<Worker>();
        }
    }
}