using System.Net;
using System.Threading.Tasks;

namespace CsTest.InterfaceWorker {
   interface IWorker {
        Task<string> RequestWorker(HttpListenerRequest request);
    }
}