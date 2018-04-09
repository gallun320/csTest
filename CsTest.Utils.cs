
using Ninject.Modules;
using CsTest.RequestWorker;
using CsTest.InterfaceWorker;

namespace CsTest.Utils {
    class Registration : NinjectModule {
        public override void Load() 
        {
            Bind<IWorker>().To<Worker>();
        }
    }
}