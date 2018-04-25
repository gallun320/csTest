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
using CsTest.Requester;



namespace CsTest.Workers.Requests 
{
    class PutRequestWorker : IPutRequest
    {

        public string reqUrl {get; set;}
        public HttpListenerRequest request {get; set;}

        public IDbWorker saver { get; set; }
        
        public async Task<string> Worker(string reqUrl, HttpListenerRequest request, IDbWorker saver)
        {
            var result = "";
            var dataPut = "";
            var reqPutDataIndex =  int.Parse(reqUrl.Split('/').Last());

            using (var stream = new StreamReader(request.InputStream, Encoding.UTF8))
            {
                dataPut = await stream.ReadToEndAsync();
            }
       
            await saver.UpdateData(reqPutDataIndex, dataPut).ConfigureAwait(false);
            
            result = "OK PUT";
            return result;
        }
    }

}