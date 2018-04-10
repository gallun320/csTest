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
        private object locker = new Object();
        public async Task<int> SetData(string data) 
        {
            var buffer = Encoding.UTF8.GetBytes(data);

            try
            {
                Monitor.Enter(locker);

                using(var fs = new FileStream(@"db.txt", FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, buffer.Length))
                using(var stream = new StreamWriter(fs, Encoding.UTF8))
                {
                    await stream.WriteLineAsync(data.ToArray(), 0, data.Length).ConfigureAwait(false);
                }
            }
            finally
            {
                Monitor.Exit(locker);
            }
            

            return 0;
        }

        public async Task<List<string>> GetData()
        {
            
            var data = new List<string>();
            try
            {
                Monitor.Enter(locker);
                using(var fs = new FileStream(@"db.txt", FileMode.Open, FileAccess.Read))
                using(var stream = new StreamReader(fs, Encoding.UTF8))
                {
                    foreach (var i in await stream.ReadLineAsync())
                    {
                        data.Add(i.ToString());
                    }
                }
            } 
            finally
            {
                Monitor.Exit(locker);
            }

            return data;
        }
    } 
}