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
    class DeleteRequestWorker : IDeleteRequest 
    {
        public string reqUrl {get; set;}
        public HttpListenerRequest request {get; set;}

        public IDbWorker saver { get; set; }

        public async Task<string> Worker(string reqUrl, HttpListenerRequest request, IDbWorker saver)
        {
            var result = "";
            var reqDeleteDataIndex =  int.Parse(reqUrl.Split('/').Last());
            var reqDataCollection = await saver.GetData().ConfigureAwait(false);
            
            if(reqDeleteDataIndex >= 9999) 
            {
                reqDataCollection.Clear();
            }
            var reqDeletEl = reqDataCollection.Remove(reqDataCollection[reqDeleteDataIndex]);
            result = "OK DELETE";
            return result;
        }
    }
}