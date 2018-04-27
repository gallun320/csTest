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
    [HttpMethod("GET")]
    class GetRequestWorker : IGetRequest 
    {
        public string reqUrl {get; set;}
        public HttpListenerRequest request {get; set;}

        public IDbWorker saver { get; set; }
        
        public async Task<string> Worker(string reqUrl, HttpListenerRequest request, IDbWorker saver)
        {
            var result = "";
            var reqGetParams = reqUrl.Split('/');
            var reqGetParamsType = reqGetParams.Select((item, index) => new {
                Item = item,
                Position = index
            }).Where(i => i.Item == "item" || i.Item == "items")
            .FirstOrDefault();
            
            if(reqGetParamsType == null) return "";

            var rt = await saver.GetData().ConfigureAwait(false);

            if(reqGetParamsType.Item == "item") 
            {
                var reqGetDataIndex = int.Parse(reqGetParams.ElementAt(reqGetParamsType.Position + 1));
                return rt.ElementAt(reqGetDataIndex);
            } 
            

            result = string.Join(", ", rt.ToArray()); 
            
            
            return result;
        }
    }
}