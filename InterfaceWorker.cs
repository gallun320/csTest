using System.Net;
using System.Threading.Tasks;

namespace CsTest.InterfaceWorker {
   interface IRequestWorker {
        Task<string> Worker(HttpListenerRequest request);
    }
}