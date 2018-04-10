
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsTest.InterfaceData {
    
    interface IData 
    {
        Task<int> SetData(string data);
        Task<List<string>> GetData();
    }
        
    
}