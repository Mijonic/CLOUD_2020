using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface ILogger
    {
        [OperationContract]
        List<string> VratiSveLogove();

        [OperationContract]
        List<string> AutomatskoSlanjeLogova(List<string> postojeci);
    }
}
