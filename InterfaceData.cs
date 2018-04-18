
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsTest.InterfaceData {
    
    public interface IDbWorker 
    {
        Task<int> SetData(string data);
        Task<List<string>> GetData();
    }
        
    
}