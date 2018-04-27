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
using CsTest.Pattern;
using CsTest.Workers.Requests;
using CsTest.InterfacePattern;
using CsTest.Extentions;


namespace CsTest.Request {
    

    public class RequestWorker : IRequestWorker {
        private IDbWorker saver;
        private IVisitor visitor;
        private StandardKernel ninjectKernel;

        public RequestWorker()
        {
            var registartion = new Registration();
            ninjectKernel = new StandardKernel(registartion);
            saver = ninjectKernel.Get<IDbWorker>();
            visitor = ninjectKernel.Get<IVisitor>();

        }
        

        public async Task<string> Worker(HttpListenerRequest request)
        {
            var reqMethod = request.HttpMethod;
            var reqUrl = request.Url.OriginalString;


            var result = "";
            
            if(reqMethod.TryGetRequestClass<IRequest>(out var worker)) 
            {

                result = await visitor.Visit(worker, reqUrl, request, saver);
                
            } 
            else
            {
                return result.ConverResult();
            }

            

            var responseString = string.Format("<html><head><meta charset='utf8'></head><body>Привет мир! {0}</body></html>", result);

            return result.ConverResult();
        }

    } 
}