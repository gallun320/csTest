﻿using System;
using System.Net;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using RequestWorker;
using Ninject;
using Utils;

namespace csTest
{
    class Program
    {
        private static IWorker _requestWorker;
        static void Main(string[] args)
        {
            var registartion = new Registration();
            var ninjectKernel = new StandardKernel(registartion);
            _requestWorker = ninjectKernel.Get<Worker>();
            
            //GetRequestMethod();
            SendRequestMethod(_requestWorker).Wait();
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

        public static async Task SendRequestMethod(IWorker requestWorker)
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




                var responseString = requestWorker.RequestWorker(request);
                var buffer = Encoding.UTF8.GetBytes(responseString);
                respones.ContentLength64 = buffer.Length;
                var output = respones.OutputStream;

                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }

            listener.Stop();

        }

    
    }
}
