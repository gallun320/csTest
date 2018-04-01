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
        public static List<string> ReqDataCollection = new List<string>() { }; 
        static void Main(string[] args)
        {
            //GetRequestMethod();
            SendRequestMethod();
        }

        public static void GetRequestMethod()
        {
            var Site = "https://google.com";

            var Req = HttpWebRequest.Create(Site);
            var Res = Req.GetResponse();

            using (var Stream = new StreamReader(Res.GetResponseStream(), Encoding.UTF8))
            {
                Console.WriteLine(Stream.ReadToEnd());
            }
        }

        public static void SendRequestMethod()
        {
            var Listener = new HttpListener();

            Listener.Prefixes.Add("http://localhost:8888/");
            Listener.Start();
            Console.WriteLine("waiting connections...");

            while (true)
            {
                var Context = Listener.GetContext();
                var Request = Context.Request;
                var Respones = Context.Response;




                var ResponseString = RequestWorker(Request);
                var Buffer = Encoding.UTF8.GetBytes(ResponseString);
                Respones.ContentLength64 = Buffer.Length;
                var Output = Respones.OutputStream;

                Output.Write(Buffer, 0, Buffer.Length);
                Output.Close();
            }

            Listener.Stop();

        }

        public static string RequestWorker(HttpListenerRequest Request)
        {
            var ReqMethod = Request.HttpMethod;
            var ReqUrl = Request.Url.OriginalString;
            var Result = "";


            switch(ReqMethod)
            {
                case "GET":
                    var ReqGetParams = ReqUrl.Split('/');
                    if(Array.IndexOf(ReqGetParams, "items") > -1) 
                    {
                        Result = String.Join(", ", ReqDataCollection.ToArray());    
                    } 
                    else if(Array.IndexOf(ReqGetParams, "item") > -1) 
                    {
                        var ReqGetDataIndex = Int32.Parse(ReqGetParams[ReqGetParams.Length - 1]);
                        var ReqEl = ReqDataCollection[ReqGetDataIndex];
                        Result = ReqEl;

                    }
                    break;
                case "POST":
                    var Data = "";
                    using ( var Stream = new StreamReader(Request.InputStream, Encoding.UTF8))
                    {
                        Data = Stream.ReadToEnd();
                    }


                    ReqDataCollection.Add(Data);
                    Result = "OK POST";
                    break;
                case "DELETE":
                    var ReqDeleteParams = ReqUrl.Split('/');
                    var ReqDeleteDataIndex = Int32.Parse(ReqDeleteParams[ReqDeleteParams.Length - 1]);
                    if(ReqDeleteDataIndex >= 9999) 
                    {
                        ReqDataCollection.Clear();
                    }
                    var ReqDeletEl = ReqDataCollection.Remove(ReqDataCollection[ReqDeleteDataIndex]);
                    Result = "OK DELETE";
                    break;
                case "PUT":
                    var DataPut = "";
                    var ReqPutParams = ReqUrl.Split('/');
                    var ReqPutDataIndex = Int32.Parse(ReqPutParams[ReqPutParams.Length - 1]);
                    using (var Stream = new StreamReader(Request.InputStream, Encoding.UTF8))
                    {
                        DataPut = Stream.ReadToEnd();
                    }
                    ReqDataCollection[ReqPutDataIndex] = DataPut;
                    Result = "OK PUT";
                    break;
            }

            var ResponseString = String.Format("<html><head><meta charset='utf8'></head><body>Привет мир! {0}</body></html>", Result);

            return ResponseString;
        }
    }
}


