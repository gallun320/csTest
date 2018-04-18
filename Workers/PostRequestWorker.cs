using System;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using CsTest.InterfaceWorker;
using System.Linq;
using CsTest.InterfaceData;
using Ninject;
using CsTest.Utils;
using CsTest.Requester.InterfaceRequest;



namespace CsTest.Workers.Requests  
{
    class PostRequestWorker : IPostRequest
    {
        public string reqUrl {get; set;}
        public HttpListenerRequest request {get; set;}

        public IDbWorker saver { get; set; }
        
        public async Task<string> Worker(string reqUrl, HttpListenerRequest request, IDbWorker saver)
        {
            var result = "";
            var data = "";
            using ( var stream = new StreamReader(request.InputStream, Encoding.UTF8))
            {
                data = await stream.ReadToEndAsync();
            }
            var rt = await saver.SetData(data);
            
            result = "OK POST";
            return result;
        }
    }
}