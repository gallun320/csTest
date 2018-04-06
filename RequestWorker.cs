using System;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;




namespace RequestWorker {
    interface IWorker {
        string RequestWorker(HttpListenerRequest request);
    }

    public class Worker : IWorker {
        private static List<string> reqDataCollection = new List<string>() { }; 
        private delegate string ReqMethodDelegate(string reqUrl, HttpListenerRequest request);
        private  Dictionary<string, ReqMethodDelegate> _methods = new Dictionary<string, ReqMethodDelegate>() {
            {"GET", GetRequestWorker},
            {"POST", PostRequestWorker},
            {"DELETE", DeleteRequestWorker},
            {"PUT", PutRequestWorker}
        };

        public string RequestWorker(HttpListenerRequest request)
        {
            var reqMethod = request.HttpMethod;
            var reqUrl = request.Url.OriginalString;

            if(!_methods.ContainsKey(reqMethod))
                return "<html><head><meta charset='utf8'></head><body>Привет мир!</body></html>";

            var result = _methods[reqMethod](reqUrl, request);

            var responseString = String.Format("<html><head><meta charset='utf8'></head><body>Привет мир! {0}</body></html>", result);

            return responseString;
        }

        public static string GetRequestWorker(string reqUrl, HttpListenerRequest request)
        {

            var result = "";
            var reqGetParams = reqUrl.Split('/');
            if(Array.IndexOf(reqGetParams, "items") > -1) 
            {
                result = String.Join(", ", reqDataCollection.ToArray());    
            } 
            else if(Array.IndexOf(reqGetParams, "item") > -1) 
            {
                var reqGetDataIndex = Int32.Parse(reqGetParams[reqGetParams.Length - 1]);
                var reqEl = reqDataCollection[reqGetDataIndex];
                result = reqEl;

            }

            return result;
        }

        public static string PostRequestWorker(string reqUrl, HttpListenerRequest request)
        {
            var result = "";
            var data = "";
            using ( var stream = new StreamReader(request.InputStream, Encoding.UTF8))
            {
                data = stream.ReadToEnd();
            }


            reqDataCollection.Add(data);
            result = "OK POST";
            return result;
        }

        public static string DeleteRequestWorker(string reqUrl, HttpListenerRequest request)
        {
            var result = "";
            var reqDeleteParams = reqUrl.Split('/');
            var reqDeleteDataIndex = Int32.Parse(reqDeleteParams[reqDeleteParams.Length - 1]);
            if(reqDeleteDataIndex >= 9999) 
            {
                reqDataCollection.Clear();
            }
            var reqDeletEl = reqDataCollection.Remove(reqDataCollection[reqDeleteDataIndex]);
            result = "OK DELETE";
            return result;
        }

        public static string PutRequestWorker(string reqUrl, HttpListenerRequest request)
        {
            var result = "";
            var dataPut = "";
            var reqPutParams = reqUrl.Split('/');
            var reqPutDataIndex = Int32.Parse(reqPutParams[reqPutParams.Length - 1]);
            using (var stream = new StreamReader(request.InputStream, Encoding.UTF8))
            {
                dataPut = stream.ReadToEnd();
            }
            reqDataCollection[reqPutDataIndex] = dataPut;
            result = "OK PUT";
            return result;
        }

    } 
}