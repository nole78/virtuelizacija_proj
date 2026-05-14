using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Proxy
{
    // Veza sa serverom
    // TODO: Zatvarati konekciju sa serverom
    public class PvServiceProxy : IDisposable
    {
        // TODO: Implementirati
        private bool _disposed = false;
        PvServiceProxy() { }
        ~PvServiceProxy()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            if (disposing)
            {
                // Oslobodi managed resurse
            }
            // Oslobodi unmanaged resurse
            _disposed = true;
        }
    }
}
