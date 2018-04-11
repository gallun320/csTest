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

namespace CsTest.RequestWorker {
    

    public class Worker : IWorker {
        private IData saver;
        private static List<string> reqDataCollection = new List<string>() { }; 
        private delegate Task<string> ReqMethodDelegate(string reqUrl, HttpListenerRequest request, IData saver);
        private  Dictionary<string, ReqMethodDelegate> _methods = new Dictionary<string, ReqMethodDelegate>() {
            {"GET", GetRequestWorker},
            {"POST", PostRequestWorker},
            {"DELETE", DeleteRequestWorker},
            {"PUT", PutRequestWorker}
        };

        public Worker()
        {
            var registartion = new Registration();
            var ninjectKernel = new StandardKernel(registartion);
            saver = ninjectKernel.Get<IData>();
        }
        

        public async Task<string> RequestWorker(HttpListenerRequest request)
        {
            var reqMethod = request.HttpMethod;
            var reqUrl = request.Url.OriginalString;


            var result = "";
            
            if(_methods.TryGetValue(reqMethod, out var func)) 
            {
                result = await func(reqUrl, request, saver);
            } 
            else
            {
                return "<html><head><meta charset='utf8'></head><body>Привет мир!</body></html>";
            }

            

            var responseString = string.Format("<html><head><meta charset='utf8'></head><body>Привет мир! {0}</body></html>", result);

            return responseString;
        }

        public static async Task<string> GetRequestWorker(string reqUrl, HttpListenerRequest request, IData saver)
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

        public static async Task<string> PostRequestWorker(string reqUrl, HttpListenerRequest request, IData saver)
        {
            var result = "";
            var data = "";
            using ( var stream = new StreamReader(request.InputStream, Encoding.UTF8))
            {
                data = await stream.ReadToEndAsync();
            }
            var rt = await saver.SetData(data);

            reqDataCollection.Add(data);
            result = "OK POST";
            return result;
        }

        public static async Task<string> DeleteRequestWorker(string reqUrl, HttpListenerRequest request, IData saver)
        {
            var result = "";
            var reqDeleteDataIndex =  int.Parse(reqUrl.Split('/').Last());
            
            if(reqDeleteDataIndex >= 9999) 
            {
                reqDataCollection.Clear();
            }
            var reqDeletEl = reqDataCollection.Remove(reqDataCollection[reqDeleteDataIndex]);
            result = "OK DELETE";
            return result;
        }

        public static async Task<string> PutRequestWorker(string reqUrl, HttpListenerRequest request, IData saver)
        {
            var result = "";
            var dataPut = "";
            var reqPutDataIndex =  int.Parse(reqUrl.Split('/').Last());
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