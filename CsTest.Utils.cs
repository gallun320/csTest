
using Ninject.Modules;
using CsTest.RequestWorker;
using CsTest.InterfaceWorker;
using CsTest.InterfaceData;
using CsTest.SaveData;

namespace CsTest.Utils {
    class Registration : NinjectModule {
        public override void Load() 
        {
            Bind<IWorker>().To<Worker>();
            Bind<IData>().To<Save>();
        }
    }
}