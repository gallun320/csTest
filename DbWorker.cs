using System.IO;
using CsTest.InterfaceData;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System;
using System.Linq;


namespace CsTest.Db {
    public class DbWorker : IDbWorker {

        private List<string> dbCollection = new List<string>() { };
        private readonly SemaphoreSlim m_lock = new SemaphoreSlim(1,1);

        public DbWorker () 
        {
           Init();
        }

        private async void Init()
        {
            dbCollection = await GetData();
        }
        public async Task<int> SetData(string data) 
        {
            
            

            await m_lock.WaitAsync();

            try
            {
               await File.AppendAllTextAsync(@"db.txt", data + Environment.NewLine);
               dbCollection.Add(data);
            }
            finally
            {
                m_lock.Release();
            }
            

            return 0;
        }

        public async Task<List<string>> GetData()
        {
            
            if(dbCollection.Count > 0) return dbCollection;

            var data = new List<string>();
            await m_lock.WaitAsync();

            try
            {
                
                

                    foreach (var i in await File.ReadAllLinesAsync(@"db.txt"))
                    {
                        data.Add(i);
                    }
                    
                
            } 
            finally
            {
                 m_lock.Release();
            }

            return data;
        }
    } 
}