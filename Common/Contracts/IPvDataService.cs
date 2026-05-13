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
        // TODO: Promeniti return tipe servisa po potrebi

        [OperationContract]
        void StartSession(PvMeta meta);

        [OperationContract]
        void PushSample(PvSample sample);

        [OperationContract]
        void EndSession();
    }
}
