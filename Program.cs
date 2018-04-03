using System;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace csTest
{
    class Program
    {
        public static List<string> reqDataCollection = new List<string>() { }; 
        static void Main(string[] args)
        {
            //GetRequestMethod();
            SendRequestMethod().Wait();
        }

        public static void GetRequestMethod()
        {
            var site = "https://google.com";

            var req = HttpWebRequest.Create(site);
            var res = req.GetResponse();

            using (var stream = new StreamReader(res.GetResponseStream(), Encoding.UTF8))
            {
                Console.WriteLine(stream.ReadToEnd());
            }
        }

        public static async Task SendRequestMethod()
        {
            
            var listener = new HttpListener();

            listener.Prefixes.Add("http://localhost:8888/");
            listener.Start();
            Console.WriteLine("waiting connections...");

            while (listener.IsListening)
            {
                var context = await listener.GetContextAsync().ConfigureAwait(false);
                var request = context.Request;
                var respones = context.Response;




                var responseString = RequestWorker(request);
                var buffer = Encoding.UTF8.GetBytes(responseString);
                respones.ContentLength64 = buffer.Length;
                var output = respones.OutputStream;

                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }

            listener.Stop();

        }

        public static string RequestWorker(HttpListenerRequest request)
        {
            var reqMethod = request.HttpMethod;
            var reqUrl = request.Url.OriginalString;
            var result = "";


            switch(reqMethod)
            {
                case "GET":
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
                    break;
                case "POST":
                    var data = "";
                    using ( var stream = new StreamReader(request.InputStream, Encoding.UTF8))
                    {
                        data = stream.ReadToEnd();
                    }


                    reqDataCollection.Add(data);
                    result = "OK POST";
                    break;
                case "DELETE":
                    var reqDeleteParams = reqUrl.Split('/');
                    var reqDeleteDataIndex = Int32.Parse(reqDeleteParams[reqDeleteParams.Length - 1]);
                    if(reqDeleteDataIndex >= 9999) 
                    {
                        reqDataCollection.Clear();
                    }
                    var reqDeletEl = reqDataCollection.Remove(reqDataCollection[reqDeleteDataIndex]);
                    result = "OK DELETE";
                    break;
                case "PUT":
                    var dataPut = "";
                    var reqPutParams = reqUrl.Split('/');
                    var reqPutDataIndex = Int32.Parse(reqPutParams[reqPutParams.Length - 1]);
                    using (var stream = new StreamReader(request.InputStream, Encoding.UTF8))
                    {
                        dataPut = stream.ReadToEnd();
                    }
                    reqDataCollection[reqPutDataIndex] = dataPut;
                    result = "OK PUT";
                    break;
            }

            var responseString = String.Format("<html><head><meta charset='utf8'></head><body>Привет мир! {0}</body></html>", result);

            return responseString;
        }
    }
}
