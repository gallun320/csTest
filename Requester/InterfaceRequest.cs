using System.Threading.Tasks;
using System.Net;
using CsTest.InterfaceData;


namespace CsTest.Requester.InterfaceRequest 
{
    public interface IRequest
    {
        string reqUrl {get; set;}
        HttpListenerRequest request {get; set;}

        IDbWorker saver { get; set; }

        Task<string> Worker(string reqUrl, HttpListenerRequest request, IDbWorker saver);
    }
}