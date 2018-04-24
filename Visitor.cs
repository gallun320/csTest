
using CsTest.InterfacePattern;
using CsTest.Requester;
using System.Threading.Tasks;
using CsTest.Utils;
using Ninject;
using System.Net;
using CsTest.InterfaceData;
using System;
using CsTest.Workers.Requests;


namespace CsTest.Pattern
{
    class Visitor : IVisitor
    {
        static Registration registartion = new Registration();
        StandardKernel ninjectKernel = new StandardKernel(registartion);
        string vReqUrl;
        HttpListenerRequest vRequest;
        IDbWorker vSaver;

        public async Task<string> Visit(GetRequestWorker getWorker)
        {
            return await getWorker.Worker(vReqUrl, vRequest, vSaver);
        }

        public async Task<string> Visit(PostRequestWorker postWorker)
        {
            return await postWorker.Worker(vReqUrl, vRequest, vSaver);
        }

        public async Task<string> Visit(PutRequestWorker putWorker)
        {
            return await putWorker.Worker(vReqUrl, vRequest, vSaver);
        }

        public async Task<string> Visit(DeleteRequestWorker deleteWorker)
        {
            return await deleteWorker.Worker(vReqUrl, vRequest, vSaver);
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