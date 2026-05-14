using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.Logging
{
    public class ClientLogger : IDisposable
    {
        // TODO: Imlementirati logger za rejected_client.csv (pise u fajl)
        private bool _disposed = false;
        public ClientLogger() { }
        ~ClientLogger()
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
                // Dispose managed resources here
            }
            // Dispose unmanaged resources here
            _disposed = true;
        }
    }
}
