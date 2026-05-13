using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common;
using Common.PvDataContracts;

namespace Server
{
    public class PvDataService : IPvDataService
    {
        // TODO: Realizovati metode (šta se desi kad klijent pozove PushSample)
        public void EndSession()
        {
            throw new NotImplementedException();
        }

        public void PushSample(PvSample sample)
        {
            throw new NotImplementedException();
        }

        public void StartSession(PvMeta meta)
        {
            throw new NotImplementedException();
        }
    }
}
