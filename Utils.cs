
using Ninject.Modules;
using CsTest.Request;
using CsTest.InterfaceWorker;
using CsTest.InterfaceData;
using CsTest.Db;
using CsTest.Requester.InterfaceRequest;
using CsTest.Workers.Requests;
using CsTest.Pattern;

namespace CsTest.Utils {
    class Registration : NinjectModule {
        public override void Load() 
        {
            Bind<IRequestWorker>().To<RequestWorker>();
            Bind<IDbWorker>().To<DbWorker>();
            Bind<IGetRequest>().To<GetRequestWorker>();
            Bind<IPostRequest>().To<PostRequestWorker>();
            Bind<IPutRequest>().To<PutRequestWorker>();
            Bind<IDeleteRequest>().To<DeleteRequestWorker>();
            Bind<IVisitor>().To<Visitor>();
            
        }
    }
}