using Common;
using Common.PvDataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    //Dispose zbog SW-a
    public class PvDataService : IPvDataService, IDisposable
    {
        private StreamWriter _sessionWriter;
        private StreamWriter _rejectWriter;
        private string _sessionDir;
        private PvMeta _currentMeta;
        private bool _sessionActive = false;
        private bool _disposed = false;

        // TODO: Realizovati metode (šta se desi kad klijent pozove PushSample)
        [OperationBehavior(AutoDisposeParameters = true)]
        public void EndSession()
        {
            if(!_sessionActive)
            {
                Console.WriteLine("[END_SESSION] Nema aktivne sesije");
                return;
            }

            CloseCurrentSession();
            Console.WriteLine("[END_SESSION] Sesija zatvorena, resursi su oslobodjeni");
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void PushSample(PvSample sample)
        {
            throw new NotImplementedException();
        }

        [OperationBehavior(AutoDisposeParameters = true)]
        public void StartSession(PvMeta meta)
        {
            
        }

        private void CloseCurrentSession()
        {
            _sessionActive = false;

            _sessionWriter?.Close();
            _sessionWriter?.Dispose();
            _sessionWriter = null;

            _rejectWriter?.Close();
            _rejectWriter.Dispose();
            _rejectWriter = null;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                // Free the unmanaged resource anytime.
                if (disposing)
                {
                    if (_sessionWriter != null)
                    {
                        _sessionWriter.Dispose();
                    }
                    if (_rejectWriter != null)
                    {
                        _rejectWriter.Dispose();
                    }
                }
                _disposed = true;
            }

        }
    }
}
