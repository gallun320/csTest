
using System.Threading.Tasks;
using CsTest.Requester.InterfaceRequest;
using System.Net;
using CsTest.InterfaceData;

namespace CsTest.Pattern 
{
    interface IVisitor
    {
        Task<string> Visit(IGetRequest getWorker);
        Task<string> Visit(IPostRequest postWorker);
        Task<string> Visit(IPutRequest putWorker);
        Task<string> Visit(IDeleteRequest deleteWorker);
        Task<string> Visit (dynamic worker, string reqUrl, HttpListenerRequest request, IDbWorker saver);
    }
}