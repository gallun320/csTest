
using System.Threading.Tasks;
using CsTest.Requester;
using System.Net;
using CsTest.InterfaceData;
using CsTest.Workers.Requests;

namespace CsTest.InterfacePattern
{
    interface IVisitor
    {
        Task<string> Visit(GetRequestWorker getWorker);
        Task<string> Visit(PostRequestWorker postWorker);
        Task<string> Visit(PutRequestWorker putWorker);
        Task<string> Visit(DeleteRequestWorker deleteWorker);
        Task<string> Visit (dynamic worker, string reqUrl, HttpListenerRequest request, IDbWorker saver); 
    } 
}