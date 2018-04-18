
using CsTest.Pattern;
using CsTest.Requester.InterfaceRequest;
using System.Threading.Tasks;
using CsTest.Utils;
using Ninject;
using System.Net;
using CsTest.InterfaceData;
using System;

namespace CsTest.Pattern
{
    class Visitor : IVisitor
    {
        static Registration registartion = new Registration();
        StandardKernel ninjectKernel = new StandardKernel(registartion);
        string vReqUrl;
        HttpListenerRequest vRequest;
        IDbWorker vSaver;

        public async Task<string> Visit(IGetRequest getWorker)
        {
            var worker = ninjectKernel.Get<IGetRequest>();
            return await worker.Worker(vReqUrl, vRequest, vSaver);
        }

        public async Task<string> Visit(IPostRequest postWorker)
        {
            var worker = ninjectKernel.Get<IPostRequest>();
            return await worker.Worker(vReqUrl, vRequest, vSaver);
        }

        public async Task<string> Visit(IPutRequest putWorker)
        {
            var worker = ninjectKernel.Get<IPutRequest>();
            return await worker.Worker(vReqUrl, vRequest, vSaver);
        }

        public async Task<string> Visit(IDeleteRequest deleteWorker)
        {
            var worker = ninjectKernel.Get<IDeleteRequest>();
            return await worker.Worker(vReqUrl, vRequest, vSaver);
        }

        public async Task<string> Visit(dynamic worker, string reqUrl, HttpListenerRequest request, IDbWorker saver)
        {
            vReqUrl = reqUrl;
            vRequest = request;
            vSaver = saver;
            
            return await Visit(worker);
        }
    }
}