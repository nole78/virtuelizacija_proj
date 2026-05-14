using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Storage
{
    public class SessionWriter : IDisposable
    {
        // TODO: Implementirati
        private bool _disposed = false;
        SessionWriter() { }
        ~SessionWriter()
        {
            Dispose(false);
        }
        public void Dispose()
        {
            throw new NotImplementedException();
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
