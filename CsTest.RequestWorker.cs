using System;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using CsTest.InterfaceWorker;
using System.Linq;


namespace CsTest.RequestWorker {
    

    public class Worker : IWorker {
        private static List<string> reqDataCollection = new List<string>() { }; 
        private delegate Task<string> ReqMethodDelegate(string reqUrl, HttpListenerRequest request);
        private  Dictionary<string, ReqMethodDelegate> _methods = new Dictionary<string, ReqMethodDelegate>() {
            {"GET", GetRequestWorker},
            {"POST", PostRequestWorker},
            {"DELETE", DeleteRequestWorker},
            {"PUT", PutRequestWorker}
        };

        public async Task<string> RequestWorker(HttpListenerRequest request)
        {
            var reqMethod = request.HttpMethod;
            var reqUrl = request.Url.OriginalString;


            var result = "";
            
            if(_methods.TryGetValue(reqMethod, out var func)) 
            {
                result = await func(reqUrl, request);
            } 
            else
            {
                return "<html><head><meta charset='utf8'></head><body>Привет мир!</body></html>";
            }

            

            var responseString = string.Format("<html><head><meta charset='utf8'></head><body>Привет мир! {0}</body></html>", result);

            return responseString;
        }

        public static async Task<string> GetRequestWorker(string reqUrl, HttpListenerRequest request)
        {

            var result = "";
            var reqGetParams = reqUrl.Split('/');
            var reqGetParamsType = reqGetParams.Select((item, index) => new {
                Item = item,
                Position = index
            }).Where(i => i.Item == "item" || i.Item == "items")
            .FirstOrDefault();
            
            if(reqGetParamsType == null) return "";

            if(reqGetParamsType.Item == "item") 
            {
                var reqGetDataIndex = reqGetParams.ElementAt(reqGetParamsType.Position + 1).Cast<int>().First();
                return reqDataCollection[reqGetDataIndex];
            } 
            
            result = string.Join(", ", reqDataCollection.ToArray()); 
            
            
            return result;
        }

        public static async Task<string> PostRequestWorker(string reqUrl, HttpListenerRequest request)
        {
            var result = "";
            var data = "";
            using ( var stream = new StreamReader(request.InputStream, Encoding.UTF8))
            {
                data = await stream.ReadToEndAsync();
            }

            Console.WriteLine(data);
            reqDataCollection.Add(data);
            result = "OK POST";
            return result;
        }

        public static async Task<string> DeleteRequestWorker(string reqUrl, HttpListenerRequest request)
        {
            var result = "";
            var reqDeleteDataIndex = reqUrl.Split('/').Last().Cast<int>().First();
            
            if(reqDeleteDataIndex >= 9999) 
            {
                reqDataCollection.Clear();
            }
            var reqDeletEl = reqDataCollection.Remove(reqDataCollection[reqDeleteDataIndex]);
            result = "OK DELETE";
            return result;
        }

        public static async Task<string> PutRequestWorker(string reqUrl, HttpListenerRequest request)
        {
            var result = "";
            var dataPut = "";
            var reqPutDataIndex = reqUrl.Split('/').Last().Cast<int>().First();
            using (var stream = new StreamReader(request.InputStream, Encoding.UTF8))
            {
                dataPut = await stream.ReadToEndAsync();
            }
            reqDataCollection[reqPutDataIndex] = dataPut;
            result = "OK PUT";
            return result;
        }

    } 
}