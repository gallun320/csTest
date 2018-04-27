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
using CsTest.Attributes;


namespace CsTest.Workers.Requests 
{
    [HttpMethod("Delete")]
    class DeleteRequestWorker : IDeleteRequest 
    {
        public string reqUrl {get; set;}
        public HttpListenerRequest request {get; set;}

        public IDbWorker saver { get; set; }

        public async Task<string> Worker(string reqUrl, HttpListenerRequest request, IDbWorker saver)
        {
            var result = "";
            var reqDeleteDataIndex =  int.Parse(reqUrl.Split('/').Last());
            
            if(reqDeleteDataIndex >= 9999) 
            {
                await saver.DeleteData().ConfigureAwait(false);
            }
            await saver.DeleteData(reqDeleteDataIndex).ConfigureAwait(false);
            result = "OK DELETE";
            return result;
        }
    }
}