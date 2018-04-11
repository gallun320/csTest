
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CsTest.InterfaceData {
    
    public interface IData 
    {
        Task<int> SetData(string data);
        Task<List<string>> GetData();
    }
        
    
}