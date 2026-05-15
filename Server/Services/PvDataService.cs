using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.PvDataContracts;

namespace Server
{
    public class PvDataService : IPvDataService
    {
        // TODO: Realizovati metode (šta se desi kad klijent pozove PushSample)
        [OperationBehavior(AutoDisposeParameters = true)]
        public void EndSession()
        {
            throw new NotImplementedException();
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void PushSample(PvSample sample)
        {
            throw new NotImplementedException();
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void StartSession(PvMeta meta)
        {
            throw new NotImplementedException();
        }
    }
}
