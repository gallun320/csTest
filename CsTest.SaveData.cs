using System.IO;
using CsTest.InterfaceData;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System;
using System.Linq;

namespace CsTest.SaveData {
    public class Save : IData {
        private readonly SemaphoreSlim m_lock = new SemaphoreSlim(1,1);
        public async Task<int> SetData(string data) 
        {
            //var buffer = Encoding.UTF8.GetBytes(data);

            await m_lock.WaitAsync();

            try
            {
               await File.AppendAllTextAsync(@"db.txt", data + Environment.NewLine);
            }
            finally
            {
                m_lock.Release();
            }
            

            return 0;
        }

        public async Task<List<string>> GetData()
        {
            
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