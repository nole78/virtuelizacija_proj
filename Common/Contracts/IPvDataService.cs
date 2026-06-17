using Common.Exceptions;
using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [ServiceContract]
    public interface IPvDataService
    {

        [OperationContract]
        [FaultContract(typeof(PvTransferException))]
        void StartSession(PvMeta meta);

        [OperationContract]
        void PushSample(PvSample sample);

        [OperationContract]
        void EndSession();
    }
}
