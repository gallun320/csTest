using System.IO;
using CsTest.InterfaceData;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System.Text;
using System;
using System.Linq;
using CsTest.Context;
using CsTest.Model;
using Microsoft.EntityFrameworkCore;


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
               using(var db = new DbWorkerContext())
               {
                   await db.AddAsync(new DbWorkerModel() { Text = data});
                   await db.SaveChangesAsync();
               }
               dbCollection.Add(data);
            }
            finally
            {
                m_lock.Release();
            }
            

            return 0;
        }

        public async Task<int> UpdateData(int idx, string data)
        {
            await m_lock.WaitAsync();

            try 
            {
                using(var db = new DbWorkerContext())
                {
                    db.Model.Update(new DbWorkerModel() { Id = idx, Text = data });
                    await db.SaveChangesAsync();
                }
                dbCollection[idx - 1] = data;
            }
            catch(InvalidDataException)
            {
              if(dbCollection.Count > 0) await SetData(data);
              else dbCollection.Add(data);
            }
            finally
            {
                m_lock.Release();
            }


            return 0;
        } 

        public async Task<int> DeleteData(int idx)
        {
            await m_lock.WaitAsync();

            try 
            {
                using(var db = new DbWorkerContext())
                {
                    db.Model.Remove(new DbWorkerModel() { Id = idx });
                    await db.SaveChangesAsync();
                }
                dbCollection.RemoveAt(idx - 1);
            }
            catch(InvalidOperationException)
            {
                Console.WriteLine("Elemnt is not defined in db");
            }
            finally
            {
                m_lock.Release();
            }
            
            return 0;
        }

        public async Task<int> DeleteData()
        {
            await m_lock.WaitAsync();

            try
            {
                using(var db = new DbWorkerContext())
                {
                    var data = await db.Model.ToListAsync();
                    db.Model.RemoveRange(data);
                    await db.SaveChangesAsync();
                }

                dbCollection.Clear();
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
                DbWorkerModel bbData = new DbWorkerModel();

                using(var db = new DbWorkerContext())
                {
                    data = await db.Model.Select(item => item.Text).ToListAsync();
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